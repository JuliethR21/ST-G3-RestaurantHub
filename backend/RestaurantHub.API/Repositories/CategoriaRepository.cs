using Microsoft.EntityFrameworkCore;
using RestaurantHub.API.Data;
using RestaurantHub.API.Models;

namespace RestaurantHub.API.Repositories;

public class CategoriaRepository
{
    private readonly RestaurantHubDbContext _db;

    public CategoriaRepository(RestaurantHubDbContext db) => _db = db;

    public Task<List<Categoria>> ListarAsync() =>
        _db.Categorias.AsNoTracking().OrderBy(c => c.Nombre).ToListAsync();

    public Task<Categoria?> ObtenerPorIdAsync(int id) =>
        _db.Categorias.FirstOrDefaultAsync(c => c.IdCategoria == id);

    public Task<bool> ExisteAsync(int id) =>
        _db.Categorias.AnyAsync(c => c.IdCategoria == id);

    public Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null) =>
        _db.Categorias.AnyAsync(c => c.Nombre == nombre && c.IdCategoria != excluirId);

    public Task<bool> TieneProductosAsync(int id) =>
        _db.Productos.AnyAsync(p => p.IdCategoria == id);

    public void Agregar(Categoria categoria) => _db.Categorias.Add(categoria);

    public void Eliminar(Categoria categoria) => _db.Categorias.Remove(categoria);

    public Task GuardarAsync() => _db.SaveChangesAsync();
}
