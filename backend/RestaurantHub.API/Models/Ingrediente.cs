using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantHub.API.Models;

[Table("INGREDIENTE")]
public class Ingrediente
{
    [Column("id_ingrediente")] public int IdIngrediente { get; set; }
    [Column("nombre")] public string Nombre { get; set; } = "";
    [Column("unidad_medida")] public string UnidadMedida { get; set; } = "";
    [Column("stock_actual")] public decimal StockActual { get; set; }
    [Column("stock_minimo")] public decimal StockMinimo { get; set; }
}
