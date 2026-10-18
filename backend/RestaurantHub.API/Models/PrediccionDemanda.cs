using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantHub.API.Models;

[Table("PREDICCION_DEMANDA")]
public class PrediccionDemanda
{
    [Column("id_prediccion")] public int IdPrediccion { get; set; }
    [Column("id_producto")] public int IdProducto { get; set; }
    [Column("fecha")] public DateTime Fecha { get; set; }
    [Column("cantidad_estimada")] public int CantidadEstimada { get; set; }
}
