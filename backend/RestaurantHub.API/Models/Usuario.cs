using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantHub.API.Models;

[Table("USUARIO")]
public class Usuario
{
    [Column("id_usuario")] public int IdUsuario { get; set; }
    [Column("nombre")] public string Nombre { get; set; } = "";
    [Column("apellido")] public string Apellido { get; set; } = "";
    [Column("correo")] public string Correo { get; set; } = "";
    [Column("contrasena")] public string Contrasena { get; set; } = "";
    [Column("rol")] public string Rol { get; set; } = "";
}
