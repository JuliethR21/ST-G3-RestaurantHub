using RestaurantHub.API.DTOs;
using RestaurantHub.API.Exceptions;
using RestaurantHub.API.Models;
using RestaurantHub.API.Repositories;

namespace RestaurantHub.API.Services;

public class PedidoService
{
    public const string Pendiente = "PENDIENTE";
    public const string Preparando = "PREPARANDO";
    public const string Entregado = "ENTREGADO";
    public const string Cancelado = "CANCELADO";

    // Cambios de estado permitidos: Pendiente -> Preparando -> Entregado, y se puede cancelar antes de entregar
    private static readonly Dictionary<string, string[]> Transiciones = new()
    {
        [Pendiente] = new[] { Preparando, Cancelado },
        [Preparando] = new[] { Entregado, Cancelado },
        [Entregado] = Array.Empty<string>(),
        [Cancelado] = Array.Empty<string>(),
    };

    private readonly PedidoRepository _pedidos;
    private readonly ProductoRepository _productos;
    private readonly IngredienteRepository _ingredientes;

    public PedidoService(PedidoRepository pedidos, ProductoRepository productos, IngredienteRepository ingredientes)
    {
        _pedidos = pedidos;
        _productos = productos;
        _ingredientes = ingredientes;
    }

    public async Task<List<PedidoResumenDto>> ListarAsync(string? estado, DateTime? desde, DateTime? hasta) =>
        (await _pedidos.ListarAsync(estado?.ToUpper(), desde, hasta)).Select(AResumenDto).ToList();

    public async Task<PedidoDto> ObtenerAsync(int id) => ADto(await BuscarAsync(id));

    // Registra el pedido como una única transacción:
    // valida productos, calcula totales, descuenta ingredientes según la receta y guarda pedido y detalles.
    // Si cualquier paso falla, se hace rollback y no queda nada guardado.
    public async Task<PedidoDto> CrearAsync(CrearPedidoDto datos)
    {
        if (!await _pedidos.ExisteClienteAsync(datos.IdCliente))
            throw new NoEncontradoException($"No existe el cliente {datos.IdCliente}.");
        if (!await _pedidos.ExisteUsuarioAsync(datos.IdUsuario))
            throw new NoEncontradoException($"No existe el usuario {datos.IdUsuario}.");

        // Si el mismo producto viene en varias líneas, se agrupa en una sola
        var items = datos.Items
            .GroupBy(i => i.IdProducto)
            .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(i => i.Cantidad) })
            .ToList();

        await using var transaccion = await _pedidos.IniciarTransaccionAsync();

        var productos = (await _productos.ObtenerConRecetaAsync(items.Select(i => i.IdProducto)))
            .ToDictionary(p => p.IdProducto);

        var pedido = new Pedido
        {
            IdCliente = datos.IdCliente,
            IdUsuario = datos.IdUsuario,
            Fecha = DateTime.Now,
            Estado = Pendiente,
        };
        var consumo = new Dictionary<int, decimal>();

        foreach (var item in items)
        {
            if (!productos.TryGetValue(item.IdProducto, out var producto))
                throw new NoEncontradoException($"No existe el producto {item.IdProducto}.");
            if (!producto.Disponible)
                throw new ReglaNegocioException($"El producto '{producto.Nombre}' no está disponible.");

            // Se guarda el precio vigente para conservar el valor histórico aunque el precio cambie después
            pedido.Detalles.Add(new DetallePedido
            {
                IdProducto = producto.IdProducto,
                Cantidad = item.Cantidad,
                PrecioUnitario = producto.Precio,
                Subtotal = producto.Precio * item.Cantidad,
            });

            foreach (var receta in producto.Receta)
                consumo[receta.IdIngrediente] = consumo.GetValueOrDefault(receta.IdIngrediente) + receta.Cantidad * item.Cantidad;
        }

        pedido.Total = pedido.Detalles.Sum(d => d.Subtotal);

        var ingredientes = productos.Values
            .SelectMany(p => p.Receta)
            .Select(r => r.Ingrediente!)
            .DistinctBy(i => i.IdIngrediente)
            .ToDictionary(i => i.IdIngrediente);

        foreach (var (idIngrediente, cantidad) in consumo)
        {
            if (!await _ingredientes.DescontarStockAsync(idIngrediente, cantidad))
            {
                var ingrediente = ingredientes[idIngrediente];
                throw new ReglaNegocioException(
                    $"Inventario insuficiente de '{ingrediente.Nombre}': se requieren {cantidad:0.##} " +
                    $"{ingrediente.UnidadMedida} y hay {ingrediente.StockActual:0.##}. El pedido no se registró.");
            }
        }

        _pedidos.Agregar(pedido);
        await _pedidos.GuardarAsync();

        await GenerarAlertasAsync(consumo.Keys);
        await _pedidos.GuardarAsync();

        await transaccion.CommitAsync();
        return await ObtenerAsync(pedido.IdPedido);
    }

    public async Task<PedidoDto> CambiarEstadoAsync(int id, string nuevoEstado)
    {
        nuevoEstado = nuevoEstado.Trim().ToUpper();
        if (!Transiciones.ContainsKey(nuevoEstado))
            throw new ReglaNegocioException(
                $"Estado '{nuevoEstado}' no válido. Use: {string.Join(", ", Transiciones.Keys)}.");

        await using var transaccion = await _pedidos.IniciarTransaccionAsync();

        var pedido = await BuscarAsync(id);
        if (!Transiciones.TryGetValue(pedido.Estado, out var permitidos) || !permitidos.Contains(nuevoEstado))
            throw new ReglaNegocioException($"No se puede pasar un pedido de {pedido.Estado} a {nuevoEstado}.");
        if (nuevoEstado == Cancelado && pedido.Pago != null)
            throw new ReglaNegocioException("No se puede cancelar un pedido que ya fue pagado.");

        // Al cancelar, los ingredientes descontados vuelven al inventario
        if (nuevoEstado == Cancelado)
        {
            var consumo = pedido.Detalles
                .SelectMany(d => d.Producto!.Receta.Select(r => new { r.IdIngrediente, Cantidad = r.Cantidad * d.Cantidad }))
                .GroupBy(c => c.IdIngrediente);
            foreach (var grupo in consumo)
                await _ingredientes.ReintegrarStockAsync(grupo.Key, grupo.Sum(c => c.Cantidad));
        }

        pedido.Estado = nuevoEstado;
        await _pedidos.GuardarAsync();
        await transaccion.CommitAsync();
        return ADto(pedido);
    }

    // Crea una alerta de reabastecimiento para cada ingrediente que quedó en su stock mínimo o por debajo
    private async Task GenerarAlertasAsync(IEnumerable<int> idsIngredientes)
    {
        var ingredientes = await _ingredientes.ObtenerPorIdsAsync(idsIngredientes);
        foreach (var ingrediente in ingredientes.Where(i => i.StockActual <= i.StockMinimo))
        {
            if (!await _ingredientes.TieneAlertaPendienteAsync(ingrediente.IdIngrediente))
                _ingredientes.AgregarAlerta(new AlertaReabastecimiento
                {
                    IdIngrediente = ingrediente.IdIngrediente,
                    Fecha = DateTime.Now,
                    Estado = "PENDIENTE",
                });
        }
    }

    private async Task<Pedido> BuscarAsync(int id) =>
        await _pedidos.ObtenerPorIdAsync(id)
        ?? throw new NoEncontradoException($"No existe el pedido {id}.");

    private static string NombreCompleto(string? nombre, string? apellido) => $"{nombre} {apellido}".Trim();

    private static PedidoResumenDto AResumenDto(Pedido p) =>
        new(p.IdPedido,
            NombreCompleto(p.Cliente?.Nombre, p.Cliente?.Apellido),
            NombreCompleto(p.Usuario?.Nombre, p.Usuario?.Apellido),
            p.Fecha, p.Estado, p.Total, p.Pago != null);

    private static PedidoDto ADto(Pedido p) =>
        new(p.IdPedido,
            p.IdCliente, NombreCompleto(p.Cliente?.Nombre, p.Cliente?.Apellido),
            p.IdUsuario, NombreCompleto(p.Usuario?.Nombre, p.Usuario?.Apellido),
            p.Fecha, p.Estado, p.Total, p.Pago != null,
            p.Detalles.Select(d => new DetallePedidoDto(
                d.IdProducto, d.Producto?.Nombre ?? "", d.Cantidad, d.PrecioUnitario, d.Subtotal)).ToList());
}
