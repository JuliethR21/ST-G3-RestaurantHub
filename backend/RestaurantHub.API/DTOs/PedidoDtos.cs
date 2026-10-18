using System.ComponentModel.DataAnnotations;

namespace RestaurantHub.API.DTOs;

public class ItemPedidoDto
{
    [Required]
    public int IdProducto { get; set; }

    [Range(1, 1000, ErrorMessage = "La cantidad debe ser mayor a 0.")]
    public int Cantidad { get; set; }
}

public class CrearPedidoDto
{
    [Required]
    public int IdCliente { get; set; }

    // Usuario (empleado) que registra el pedido; cuando exista el login se tomará de la sesión
    [Required]
    public int IdUsuario { get; set; }

    [Required, MinLength(1, ErrorMessage = "El pedido debe tener al menos un producto.")]
    public List<ItemPedidoDto> Items { get; set; } = new();
}

public class CambiarEstadoPedidoDto
{
    [Required]
    public string Estado { get; set; } = "";
}

public record DetallePedidoDto(int IdProducto, string Producto, int Cantidad, decimal PrecioUnitario, decimal Subtotal);

public record PedidoResumenDto(int IdPedido, string Cliente, string Usuario, DateTime Fecha, string Estado, decimal Total, bool Pagado);

public record PedidoDto(
    int IdPedido,
    int IdCliente,
    string Cliente,
    int IdUsuario,
    string Usuario,
    DateTime Fecha,
    string Estado,
    decimal Total,
    bool Pagado,
    List<DetallePedidoDto> Detalles);
