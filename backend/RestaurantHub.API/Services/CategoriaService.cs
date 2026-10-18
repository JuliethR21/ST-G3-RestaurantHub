using RestaurantHub.API.DTOs;
using RestaurantHub.API.Exceptions;
using RestaurantHub.API.Models;
using RestaurantHub.API.Repositories;

namespace RestaurantHub.API.Services;

public class CategoriaService
{
    private readonly CategoriaRepository _categorias;

    public CategoriaService(CategoriaRepository categorias) => _categorias = categorias;

    public async Task<List<CategoriaDto>> ListarAsync() =>
        (await _categorias.ListarAsync()).Select(ADto).ToList();

    public async Task<CategoriaDto> ObtenerAsync(int id) =>
        ADto(await BuscarAsync(id));

    public async Task<CategoriaDto> CrearAsync(GuardarCategoriaDto datos)
    {
        var nombre = datos.Nombre.Trim();
        if (await _categorias.ExisteNombreAsync(nombre))
            throw new ReglaNegocioException($"Ya existe una categoría llamada '{nombre}'.");

        var categoria = new Categoria { Nombre = nombre, Descripcion = datos.Descripcion };
        _categorias.Agregar(categoria);
        await _categorias.GuardarAsync();
        return ADto(categoria);
    }

    public async Task<CategoriaDto> ActualizarAsync(int id, GuardarCategoriaDto datos)
    {
        var categoria = await BuscarAsync(id);
        var nombre = datos.Nombre.Trim();
        if (await _categorias.ExisteNombreAsync(nombre, id))
            throw new ReglaNegocioException($"Ya existe una categoría llamada '{nombre}'.");

        categoria.Nombre = nombre;
        categoria.Descripcion = datos.Descripcion;
        await _categorias.GuardarAsync();
        return ADto(categoria);
    }

    public async Task EliminarAsync(int id)
    {
        var categoria = await BuscarAsync(id);
        if (await _categorias.TieneProductosAsync(id))
            throw new ReglaNegocioException("No se puede eliminar una categoría que tiene productos asociados.");

        _categorias.Eliminar(categoria);
        await _categorias.GuardarAsync();
    }

    private async Task<Categoria> BuscarAsync(int id) =>
        await _categorias.ObtenerPorIdAsync(id)
        ?? throw new NoEncontradoException($"No existe la categoría {id}.");

    private static CategoriaDto ADto(Categoria c) => new(c.IdCategoria, c.Nombre, c.Descripcion);
}
