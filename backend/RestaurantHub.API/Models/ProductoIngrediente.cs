using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantHub.API.Models;

// Receta: cantidad de un ingrediente necesaria para preparar una unidad del producto
[Table("PRODUCTO_INGREDIENTE")]
public class ProductoIngrediente
{
    [Column("id_producto")] public int IdProducto { get; set; }
    [Column("id_ingrediente")] public int IdIngrediente { get; set; }
    [Column("cantidad")] public decimal Cantidad { get; set; }

    public Producto? Producto { get; set; }
    public Ingrediente? Ingrediente { get; set; }
}
