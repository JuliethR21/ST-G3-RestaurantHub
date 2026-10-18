using RestaurantHub.API.DTOs;
using RestaurantHub.API.Repositories;

namespace RestaurantHub.API.Services;

public class IngredienteService
{
    private readonly IngredienteRepository _ingredientes;

    public IngredienteService(IngredienteRepository ingredientes) => _ingredientes = ingredientes;

    public async Task<List<IngredienteDto>> ListarAsync(bool soloBajoMinimo) =>
        (await _ingredientes.ListarAsync(soloBajoMinimo))
            .Select(i => new IngredienteDto(
                i.IdIngrediente, i.Nombre, i.UnidadMedida, i.StockActual, i.StockMinimo, i.StockActual <= i.StockMinimo))
            .ToList();

    public async Task<List<AlertaDto>> ListarAlertasPendientesAsync() =>
        (await _ingredientes.ListarAlertasPendientesAsync())
            .Select(a => new AlertaDto(
                a.IdAlerta, a.IdIngrediente, a.Ingrediente?.Nombre ?? "",
                a.Ingrediente?.StockActual ?? 0, a.Ingrediente?.StockMinimo ?? 0, a.Fecha, a.Estado))
            .ToList();
}
