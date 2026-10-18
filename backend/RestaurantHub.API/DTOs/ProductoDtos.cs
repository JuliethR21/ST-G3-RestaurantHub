using System.ComponentModel.DataAnnotations;

namespace RestaurantHub.API.DTOs;

public record ProductoDto(
    int IdProducto,
    string Nombre,
    string? Descripcion,
    decimal Precio,
    bool Disponible,
    int IdCategoria,
    string Categoria);

public record IngredienteRecetaDto(int IdIngrediente, string Ingrediente, decimal Cantidad, string UnidadMedida);

public record ProductoDetalleDto(
    int IdProducto,
    string Nombre,
    string? Descripcion,
    decimal Precio,
    bool Disponible,
    int IdCategoria,
    string Categoria,
    List<IngredienteRecetaDto> Receta);

public class GuardarProductoDto
{
    [Required]
    public int IdCategoria { get; set; }

    [Required, StringLength(100)]
    public string Nombre { get; set; } = "";

    [StringLength(200)]
    public string? Descripcion { get; set; }

    [Range(0, 99999999.99, ErrorMessage = "El precio no puede ser negativo.")]
    public decimal Precio { get; set; }

    public bool Disponible { get; set; } = true;
}
