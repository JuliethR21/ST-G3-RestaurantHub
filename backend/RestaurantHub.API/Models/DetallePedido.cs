using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantHub.API.Models;

[Table("DETALLE_PEDIDO")]
public class DetallePedido
{
    [Column("id_detalle")] public int IdDetalle { get; set; }
    [Column("id_pedido")] public int IdPedido { get; set; }
    [Column("id_producto")] public int IdProducto { get; set; }
    [Column("cantidad")] public int Cantidad { get; set; }
    [Column("precio_unitario")] public decimal PrecioUnitario { get; set; }
    [Column("subtotal")] public decimal Subtotal { get; set; }

    public Producto? Producto { get; set; }
}
