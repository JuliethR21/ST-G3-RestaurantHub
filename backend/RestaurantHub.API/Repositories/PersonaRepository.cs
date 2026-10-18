using Microsoft.EntityFrameworkCore;
using RestaurantHub.API.Data;
using RestaurantHub.API.Models;

namespace RestaurantHub.API.Repositories;

// Consultas de solo lectura sobre clientes y usuarios, usadas al registrar pedidos
public class PersonaRepository
{
    private readonly RestaurantHubDbContext _db;

    public PersonaRepository(RestaurantHubDbContext db) => _db = db;

    public Task<List<Cliente>> ListarClientesAsync() =>
        _db.Clientes.AsNoTracking().OrderBy(c => c.Nombre).ThenBy(c => c.Apellido).ToListAsync();

    public Task<List<Usuario>> ListarUsuariosAsync() =>
        _db.Usuarios.AsNoTracking().OrderBy(u => u.Nombre).ThenBy(u => u.Apellido).ToListAsync();
}
