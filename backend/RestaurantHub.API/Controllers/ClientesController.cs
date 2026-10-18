using Microsoft.AspNetCore.Mvc;
using RestaurantHub.API.DTOs;
using RestaurantHub.API.Services;

namespace RestaurantHub.API.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly PersonaService _servicio;

    public ClientesController(PersonaService servicio) => _servicio = servicio;

    [HttpGet]
    public Task<List<ClienteDto>> Listar() => _servicio.ListarClientesAsync();
}
