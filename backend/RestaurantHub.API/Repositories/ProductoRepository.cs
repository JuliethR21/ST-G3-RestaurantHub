using Microsoft.EntityFrameworkCore;
using RestaurantHub.API.Data;
using RestaurantHub.API.Models;

namespace RestaurantHub.API.Repositories;

public class ProductoRepository
{
    private readonly RestaurantHubDbContext _db;

    public ProductoRepository(RestaurantHubDbContext db) => _db = db;

    public Task<List<Producto>> ListarAsync(string? nombre, int? idCategoria, bool? disponible)
    {
        var consulta = _db.Productos.AsNoTracking().Include(p => p.Categoria).AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
            consulta = consulta.Where(p => p.Nombre.Contains(nombre));
        if (idCategoria.HasValue)
            consulta = consulta.Where(p => p.IdCategoria == idCategoria);
        if (disponible.HasValue)
            consulta = consulta.Where(p => p.Disponible == disponible);

        return consulta.OrderBy(p => p.Nombre).ToListAsync();
    }

    public Task<Producto?> ObtenerPorIdAsync(int id) =>
        _db.Productos
            .Include(p => p.Categoria)
            .Include(p => p.Receta).ThenInclude(r => r.Ingrediente)
            .FirstOrDefaultAsync(p => p.IdProducto == id);

    // Productos de un pedido junto con su receta, para calcular precios y consumo de ingredientes
    public Task<List<Producto>> ObtenerConRecetaAsync(IEnumerable<int> ids) =>
        _db.Productos
            .Include(p => p.Receta).ThenInclude(r => r.Ingrediente)
            .Where(p => ids.Contains(p.IdProducto))
            .ToListAsync();

    public void Agregar(Producto producto) => _db.Productos.Add(producto);

    public Task GuardarAsync() => _db.SaveChangesAsync();
}
