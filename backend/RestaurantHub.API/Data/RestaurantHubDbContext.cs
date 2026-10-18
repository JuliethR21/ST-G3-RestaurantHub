using Microsoft.EntityFrameworkCore;
using RestaurantHub.API.Models;

namespace RestaurantHub.API.Data;

public class RestaurantHubDbContext : DbContext
{
    public RestaurantHubDbContext(DbContextOptions<RestaurantHubDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Ingrediente> Ingredientes => Set<Ingrediente>();
    public DbSet<ProductoIngrediente> ProductoIngredientes => Set<ProductoIngrediente>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<DetallePedido> DetallesPedido => Set<DetallePedido>();
    public DbSet<Pago> Pagos => Set<Pago>();
    public DbSet<Factura> Facturas => Set<Factura>();
    public DbSet<PrediccionDemanda> PrediccionesDemanda => Set<PrediccionDemanda>();
    public DbSet<AlertaReabastecimiento> AlertasReabastecimiento => Set<AlertaReabastecimiento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Claves primarias (los nombres id_xxx no siguen la convención de EF)
        modelBuilder.Entity<Usuario>().HasKey(u => u.IdUsuario);
        modelBuilder.Entity<Cliente>().HasKey(c => c.IdCliente);
        modelBuilder.Entity<Categoria>().HasKey(c => c.IdCategoria);
        modelBuilder.Entity<Producto>().HasKey(p => p.IdProducto);
        modelBuilder.Entity<Ingrediente>().HasKey(i => i.IdIngrediente);
        modelBuilder.Entity<ProductoIngrediente>().HasKey(pi => new { pi.IdProducto, pi.IdIngrediente });
        modelBuilder.Entity<Pedido>().HasKey(p => p.IdPedido);
        modelBuilder.Entity<DetallePedido>().HasKey(d => d.IdDetalle);
        modelBuilder.Entity<Pago>().HasKey(p => p.IdPago);
        modelBuilder.Entity<Factura>().HasKey(f => f.IdFactura);
        modelBuilder.Entity<PrediccionDemanda>().HasKey(p => p.IdPrediccion);
        modelBuilder.Entity<AlertaReabastecimiento>().HasKey(a => a.IdAlerta);

        // Relaciones
        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Categoria).WithMany(c => c.Productos)
            .HasForeignKey(p => p.IdCategoria);

        modelBuilder.Entity<ProductoIngrediente>()
            .HasOne(pi => pi.Producto).WithMany(p => p.Receta)
            .HasForeignKey(pi => pi.IdProducto);

        modelBuilder.Entity<ProductoIngrediente>()
            .HasOne(pi => pi.Ingrediente).WithMany()
            .HasForeignKey(pi => pi.IdIngrediente);

        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Cliente).WithMany()
            .HasForeignKey(p => p.IdCliente);

        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Usuario).WithMany()
            .HasForeignKey(p => p.IdUsuario);

        modelBuilder.Entity<DetallePedido>()
            .HasOne<Pedido>().WithMany(p => p.Detalles)
            .HasForeignKey(d => d.IdPedido);

        modelBuilder.Entity<DetallePedido>()
            .HasOne(d => d.Producto).WithMany()
            .HasForeignKey(d => d.IdProducto);

        modelBuilder.Entity<Pago>()
            .HasOne<Pedido>().WithOne(p => p.Pago)
            .HasForeignKey<Pago>(p => p.IdPedido);

        modelBuilder.Entity<Factura>()
            .HasOne<Pedido>().WithOne()
            .HasForeignKey<Factura>(f => f.IdPedido);

        modelBuilder.Entity<PrediccionDemanda>()
            .HasOne<Producto>().WithMany()
            .HasForeignKey(p => p.IdProducto);

        modelBuilder.Entity<AlertaReabastecimiento>()
            .HasOne(a => a.Ingrediente).WithMany()
            .HasForeignKey(a => a.IdIngrediente);

        // Todas las columnas decimales de la base son DECIMAL(10,2)
        foreach (var propiedad in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal)))
        {
            propiedad.SetPrecision(10);
            propiedad.SetScale(2);
        }
    }
}
