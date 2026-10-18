using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantHub.API.Models;

[Table("PEDIDO")]
public class Pedido
{
    [Column("id_pedido")] public int IdPedido { get; set; }
    [Column("id_cliente")] public int IdCliente { get; set; }
    [Column("id_usuario")] public int IdUsuario { get; set; }
    [Column("fecha")] public DateTime Fecha { get; set; }
    [Column("estado")] public string Estado { get; set; } = "";
    [Column("total")] public decimal Total { get; set; }

    public Cliente? Cliente { get; set; }
    public Usuario? Usuario { get; set; }
    public List<DetallePedido> Detalles { get; set; } = new();
    public Pago? Pago { get; set; }
}
