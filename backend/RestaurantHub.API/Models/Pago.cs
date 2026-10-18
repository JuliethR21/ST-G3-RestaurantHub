using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantHub.API.Models;

[Table("PAGO")]
public class Pago
{
    [Column("id_pago")] public int IdPago { get; set; }
    [Column("id_pedido")] public int IdPedido { get; set; }
    [Column("fecha_pago")] public DateTime FechaPago { get; set; }
    [Column("monto")] public decimal Monto { get; set; }
    [Column("metodo_pago")] public string MetodoPago { get; set; } = "";
    [Column("estado")] public string Estado { get; set; } = "";
}
