using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantHub.API.Models;

[Table("FACTURA")]
public class Factura
{
    [Column("id_factura")] public int IdFactura { get; set; }
    [Column("id_pedido")] public int IdPedido { get; set; }
    [Column("fecha")] public DateTime Fecha { get; set; }
    [Column("total")] public decimal Total { get; set; }
}
