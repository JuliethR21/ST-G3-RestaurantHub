using Microsoft.EntityFrameworkCore;
using RestaurantHub.API.Data;
using RestaurantHub.API.Models;

namespace RestaurantHub.API.Repositories;

public class IngredienteRepository
{
    private readonly RestaurantHubDbContext _db;

    public IngredienteRepository(RestaurantHubDbContext db) => _db = db;

    public Task<List<Ingrediente>> ListarAsync(bool soloBajoMinimo)
    {
        var consulta = _db.Ingredientes.AsNoTracking();
        if (soloBajoMinimo)
            consulta = consulta.Where(i => i.StockActual <= i.StockMinimo);
        return consulta.OrderBy(i => i.Nombre).ToListAsync();
    }

    public Task<List<Ingrediente>> ObtenerPorIdsAsync(IEnumerable<int> ids) =>
        _db.Ingredientes.AsNoTracking().Where(i => ids.Contains(i.IdIngrediente)).ToListAsync();

    // Descuenta el stock en una sola sentencia UPDATE ... WHERE stock_actual >= cantidad.
    // Si otro pedido consumió el ingrediente al mismo tiempo, la condición falla y no se descuenta nada,
    // así el stock nunca queda negativo. Devuelve false si no había existencias suficientes.
    public async Task<bool> DescontarStockAsync(int idIngrediente, decimal cantidad)
    {
        var filas = await _db.Ingredientes
            .Where(i => i.IdIngrediente == idIngrediente && i.StockActual >= cantidad)
            .ExecuteUpdateAsync(s => s.SetProperty(i => i.StockActual, i => i.StockActual - cantidad));
        return filas == 1;
    }

    public Task ReintegrarStockAsync(int idIngrediente, decimal cantidad) =>
        _db.Ingredientes
            .Where(i => i.IdIngrediente == idIngrediente)
            .ExecuteUpdateAsync(s => s.SetProperty(i => i.StockActual, i => i.StockActual + cantidad));

    public Task<List<AlertaReabastecimiento>> ListarAlertasPendientesAsync() =>
        _db.AlertasReabastecimiento.AsNoTracking()
            .Include(a => a.Ingrediente)
            .Where(a => a.Estado == "PENDIENTE")
            .OrderByDescending(a => a.Fecha)
            .ToListAsync();

    public Task<bool> TieneAlertaPendienteAsync(int idIngrediente) =>
        _db.AlertasReabastecimiento.AnyAsync(a => a.IdIngrediente == idIngrediente && a.Estado == "PENDIENTE");

    public void AgregarAlerta(AlertaReabastecimiento alerta) => _db.AlertasReabastecimiento.Add(alerta);
}
