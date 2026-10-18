using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantHub.API.Models;

[Table("ALERTA_REABASTECIMIENTO")]
public class AlertaReabastecimiento
{
    [Column("id_alerta")] public int IdAlerta { get; set; }
    [Column("id_ingrediente")] public int IdIngrediente { get; set; }
    [Column("fecha")] public DateTime Fecha { get; set; }
    [Column("estado")] public string Estado { get; set; } = "";

    public Ingrediente? Ingrediente { get; set; }
}
