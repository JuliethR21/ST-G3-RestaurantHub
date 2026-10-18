using Microsoft.AspNetCore.Mvc;
using RestaurantHub.API.DTOs;
using RestaurantHub.API.Services;

namespace RestaurantHub.API.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductosController : ControllerBase
{
    private readonly ProductoService _servicio;

    public ProductosController(ProductoService servicio) => _servicio = servicio;

    // Catálogo con filtros opcionales: /api/productos?nombre=hamb&idCategoria=1&disponible=true
    [HttpGet]
    public Task<List<ProductoDto>> Listar(string? nombre, int? idCategoria, bool? disponible) =>
        _servicio.ListarAsync(nombre, idCategoria, disponible);

    [HttpGet("{id:int}")]
    public Task<ProductoDetalleDto> Obtener(int id) => _servicio.ObtenerAsync(id);

    [HttpPost]
    public async Task<ActionResult<ProductoDetalleDto>> Crear(GuardarProductoDto datos)
    {
        var producto = await _servicio.CrearAsync(datos);
        return CreatedAtAction(nameof(Obtener), new { id = producto.IdProducto }, producto);
    }

    [HttpPut("{id:int}")]
    public Task<ProductoDetalleDto> Actualizar(int id, GuardarProductoDto datos) => _servicio.ActualizarAsync(id, datos);

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Desactivar(int id)
    {
        await _servicio.DesactivarAsync(id);
        return NoContent();
    }
}
