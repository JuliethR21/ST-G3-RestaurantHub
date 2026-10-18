namespace RestaurantHub.API.DTOs;

public record IngredienteDto(
    int IdIngrediente,
    string Nombre,
    string UnidadMedida,
    decimal StockActual,
    decimal StockMinimo,
    bool BajoMinimo);

public record AlertaDto(
    int IdAlerta,
    int IdIngrediente,
    string Ingrediente,
    decimal StockActual,
    decimal StockMinimo,
    DateTime Fecha,
    string Estado);
