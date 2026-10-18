using Microsoft.AspNetCore.Mvc;
using RestaurantHub.API.DTOs;
using RestaurantHub.API.Services;

namespace RestaurantHub.API.Controllers;

[ApiController]
[Route("api/pedidos")]
public class PedidosController : ControllerBase
{
    private readonly PedidoService _servicio;

    public PedidosController(PedidoService servicio) => _servicio = servicio;

    // Historial con filtros opcionales: /api/pedidos?estado=PENDIENTE&desde=2026-10-01&hasta=2026-10-31
    [HttpGet]
    public Task<List<PedidoResumenDto>> Listar(string? estado, DateTime? desde, DateTime? hasta) =>
        _servicio.ListarAsync(estado, desde, hasta);

    [HttpGet("{id:int}")]
    public Task<PedidoDto> Obtener(int id) => _servicio.ObtenerAsync(id);

    [HttpPost]
    public async Task<ActionResult<PedidoDto>> Crear(CrearPedidoDto datos)
    {
        var pedido = await _servicio.CrearAsync(datos);
        return CreatedAtAction(nameof(Obtener), new { id = pedido.IdPedido }, pedido);
    }

    [HttpPatch("{id:int}/estado")]
    public Task<PedidoDto> CambiarEstado(int id, CambiarEstadoPedidoDto datos) =>
        _servicio.CambiarEstadoAsync(id, datos.Estado);
}
