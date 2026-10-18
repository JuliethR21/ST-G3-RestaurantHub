using Microsoft.AspNetCore.Mvc;
using RestaurantHub.API.DTOs;
using RestaurantHub.API.Services;

namespace RestaurantHub.API.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly PersonaService _servicio;

    public UsuariosController(PersonaService servicio) => _servicio = servicio;

    [HttpGet]
    public Task<List<UsuarioDto>> Listar() => _servicio.ListarUsuariosAsync();
}
