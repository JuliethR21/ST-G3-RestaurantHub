using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantHub.API.Models;

[Table("PRODUCTO")]
public class Producto
{
    [Column("id_producto")] public int IdProducto { get; set; }
    [Column("id_categoria")] public int IdCategoria { get; set; }
    [Column("nombre")] public string Nombre { get; set; } = "";
    [Column("descripcion")] public string? Descripcion { get; set; }
    [Column("precio")] public decimal Precio { get; set; }
    [Column("disponible")] public bool Disponible { get; set; } = true;

    public Categoria? Categoria { get; set; }
    public List<ProductoIngrediente> Receta { get; set; } = new();
}
