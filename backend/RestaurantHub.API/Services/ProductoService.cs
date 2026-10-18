using RestaurantHub.API.DTOs;
using RestaurantHub.API.Exceptions;
using RestaurantHub.API.Models;
using RestaurantHub.API.Repositories;

namespace RestaurantHub.API.Services;

public class ProductoService
{
    private readonly ProductoRepository _productos;
    private readonly CategoriaRepository _categorias;

    public ProductoService(ProductoRepository productos, CategoriaRepository categorias)
    {
        _productos = productos;
        _categorias = categorias;
    }

    public async Task<List<ProductoDto>> ListarAsync(string? nombre, int? idCategoria, bool? disponible) =>
        (await _productos.ListarAsync(nombre, idCategoria, disponible)).Select(ADto).ToList();

    public async Task<ProductoDetalleDto> ObtenerAsync(int id) =>
        ADetalleDto(await BuscarAsync(id));

    public async Task<ProductoDetalleDto> CrearAsync(GuardarProductoDto datos)
    {
        await ValidarCategoriaAsync(datos.IdCategoria);

        var producto = new Producto();
        Copiar(datos, producto);
        _productos.Agregar(producto);
        await _productos.GuardarAsync();
        return await ObtenerAsync(producto.IdProducto);
    }

    public async Task<ProductoDetalleDto> ActualizarAsync(int id, GuardarProductoDto datos)
    {
        var producto = await BuscarAsync(id);
        await ValidarCategoriaAsync(datos.IdCategoria);

        Copiar(datos, producto);
        await _productos.GuardarAsync();
        return await ObtenerAsync(id);
    }

    // No se borra el producto para no perder el historial de pedidos: solo se marca como no disponible
    public async Task DesactivarAsync(int id)
    {
        var producto = await BuscarAsync(id);
        producto.Disponible = false;
        await _productos.GuardarAsync();
    }

    private async Task ValidarCategoriaAsync(int idCategoria)
    {
        if (!await _categorias.ExisteAsync(idCategoria))
            throw new NoEncontradoException($"No existe la categoría {idCategoria}.");
    }

    private async Task<Producto> BuscarAsync(int id) =>
        await _productos.ObtenerPorIdAsync(id)
        ?? throw new NoEncontradoException($"No existe el producto {id}.");

    private static void Copiar(GuardarProductoDto datos, Producto producto)
    {
        producto.IdCategoria = datos.IdCategoria;
        producto.Nombre = datos.Nombre.Trim();
        producto.Descripcion = datos.Descripcion;
        producto.Precio = datos.Precio;
        producto.Disponible = datos.Disponible;
    }

    private static ProductoDto ADto(Producto p) =>
        new(p.IdProducto, p.Nombre, p.Descripcion, p.Precio, p.Disponible, p.IdCategoria, p.Categoria?.Nombre ?? "");

    private static ProductoDetalleDto ADetalleDto(Producto p) =>
        new(p.IdProducto, p.Nombre, p.Descripcion, p.Precio, p.Disponible, p.IdCategoria, p.Categoria?.Nombre ?? "",
            p.Receta.Select(r => new IngredienteRecetaDto(
                r.IdIngrediente, r.Ingrediente?.Nombre ?? "", r.Cantidad, r.Ingrediente?.UnidadMedida ?? "")).ToList());
}
