using Microsoft.EntityFrameworkCore;
using WorkTestAPI.Models;

namespace WorkTestAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Producto> Productos { get; set; }
    public DbSet<Proveedor> Proveedores { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Compra> Compras { get; set; }
    public DbSet<CompraDetalle> CompraDetalles { get; set; }
    public DbSet<Venta> Ventas { get; set; }
    public DbSet<VentaDetalle> VentaDetalles { get; set; }

    // AGREGA ESTA PARTE PARA MAPEAR LOS NOMBRES CORRECTOS
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeo manual a los nombres en singular de tu imagen
        modelBuilder.Entity<Producto>().ToTable("Producto");
        modelBuilder.Entity<Proveedor>().ToTable("Proveedor");
        modelBuilder.Entity<Cliente>().ToTable("Cliente");
        modelBuilder.Entity<Compra>().ToTable("Compra");
        modelBuilder.Entity<CompraDetalle>().ToTable("CompraDetalle");
        modelBuilder.Entity<Venta>().ToTable("Venta");
        modelBuilder.Entity<VentaDetalle>().ToTable("VentaDetalle");
    }
}