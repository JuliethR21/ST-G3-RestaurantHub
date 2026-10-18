using Microsoft.AspNetCore.Mvc;
using RestaurantHub.API.DTOs;
using RestaurantHub.API.Services;

namespace RestaurantHub.API.Controllers;

[ApiController]
[Route("api/ingredientes")]
public class IngredientesController : ControllerBase
{
    private readonly IngredienteService _servicio;

    public IngredientesController(IngredienteService servicio) => _servicio = servicio;

    // Inventario de ingredientes; con ?bajoMinimo=true solo los que están en o por debajo del mínimo
    [HttpGet]
    public Task<List<IngredienteDto>> Listar(bool bajoMinimo = false) => _servicio.ListarAsync(bajoMinimo);

    [HttpGet("alertas")]
    public Task<List<AlertaDto>> Alertas() => _servicio.ListarAlertasPendientesAsync();
}
