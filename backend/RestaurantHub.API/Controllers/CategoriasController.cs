using Microsoft.AspNetCore.Mvc;
using RestaurantHub.API.DTOs;
using RestaurantHub.API.Services;

namespace RestaurantHub.API.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly CategoriaService _servicio;

    public CategoriasController(CategoriaService servicio) => _servicio = servicio;

    [HttpGet]
    public Task<List<CategoriaDto>> Listar() => _servicio.ListarAsync();

    [HttpGet("{id:int}")]
    public Task<CategoriaDto> Obtener(int id) => _servicio.ObtenerAsync(id);

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Crear(GuardarCategoriaDto datos)
    {
        var categoria = await _servicio.CrearAsync(datos);
        return CreatedAtAction(nameof(Obtener), new { id = categoria.IdCategoria }, categoria);
    }

    [HttpPut("{id:int}")]
    public Task<CategoriaDto> Actualizar(int id, GuardarCategoriaDto datos) => _servicio.ActualizarAsync(id, datos);

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _servicio.EliminarAsync(id);
        return NoContent();
    }
}
