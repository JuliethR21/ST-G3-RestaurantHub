using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RestaurantHub.API.Data;
using RestaurantHub.API.Models;

namespace RestaurantHub.API.Repositories;

public class PedidoRepository
{
    private readonly RestaurantHubDbContext _db;

    public PedidoRepository(RestaurantHubDbContext db) => _db = db;

    // Todos los repositorios comparten el mismo DbContext por petición,
    // así que la transacción abarca también los cambios de inventario y alertas
    public Task<IDbContextTransaction> IniciarTransaccionAsync() => _db.Database.BeginTransactionAsync();

    public Task<List<Pedido>> ListarAsync(string? estado, DateTime? desde, DateTime? hasta)
    {
        var consulta = _db.Pedidos.AsNoTracking()
            .Include(p => p.Cliente)
            .Include(p => p.Usuario)
            .Include(p => p.Pago)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(estado))
            consulta = consulta.Where(p => p.Estado == estado);
        if (desde.HasValue)
            consulta = consulta.Where(p => p.Fecha >= desde);
        if (hasta.HasValue)
            consulta = consulta.Where(p => p.Fecha < hasta.Value.Date.AddDays(1));

        return consulta.OrderByDescending(p => p.Fecha).ToListAsync();
    }

    public Task<Pedido?> ObtenerPorIdAsync(int id) =>
        _db.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Usuario)
            .Include(p => p.Pago)
            .Include(p => p.Detalles).ThenInclude(d => d.Producto!).ThenInclude(pr => pr.Receta)
            .FirstOrDefaultAsync(p => p.IdPedido == id);

    public Task<bool> ExisteClienteAsync(int id) => _db.Clientes.AnyAsync(c => c.IdCliente == id);

    public Task<bool> ExisteUsuarioAsync(int id) => _db.Usuarios.AnyAsync(u => u.IdUsuario == id);

    public void Agregar(Pedido pedido) => _db.Pedidos.Add(pedido);

    public Task GuardarAsync() => _db.SaveChangesAsync();
}
