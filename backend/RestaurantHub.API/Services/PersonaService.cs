using RestaurantHub.API.DTOs;
using RestaurantHub.API.Repositories;

namespace RestaurantHub.API.Services;

public class PersonaService
{
    private readonly PersonaRepository _personas;

    public PersonaService(PersonaRepository personas) => _personas = personas;

    public async Task<List<ClienteDto>> ListarClientesAsync() =>
        (await _personas.ListarClientesAsync())
            .Select(c => new ClienteDto(c.IdCliente, c.Nombre, c.Apellido, c.Telefono, c.Correo))
            .ToList();

    public async Task<List<UsuarioDto>> ListarUsuariosAsync() =>
        (await _personas.ListarUsuariosAsync())
            .Select(u => new UsuarioDto(u.IdUsuario, u.Nombre, u.Apellido, u.Correo, u.Rol))
            .ToList();
}
