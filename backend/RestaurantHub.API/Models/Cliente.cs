using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantHub.API.Models;

[Table("CLIENTE")]
public class Cliente
{
    [Column("id_cliente")] public int IdCliente { get; set; }
    [Column("nombre")] public string Nombre { get; set; } = "";
    [Column("apellido")] public string Apellido { get; set; } = "";
    [Column("telefono")] public string? Telefono { get; set; }
    [Column("correo")] public string? Correo { get; set; }
}
