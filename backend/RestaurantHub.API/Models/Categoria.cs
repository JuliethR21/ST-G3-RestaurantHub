using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantHub.API.Models;

[Table("CATEGORIA")]
public class Categoria
{
    [Column("id_categoria")] public int IdCategoria { get; set; }
    [Column("nombre")] public string Nombre { get; set; } = "";
    [Column("descripcion")] public string? Descripcion { get; set; }

    public List<Producto> Productos { get; set; } = new();
}
