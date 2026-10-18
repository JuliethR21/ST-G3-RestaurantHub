using System.ComponentModel.DataAnnotations;

namespace RestaurantHub.API.DTOs;

public record CategoriaDto(int IdCategoria, string Nombre, string? Descripcion);

public class GuardarCategoriaDto
{
    [Required, StringLength(50)]
    public string Nombre { get; set; } = "";

    [StringLength(150)]
    public string? Descripcion { get; set; }
}
