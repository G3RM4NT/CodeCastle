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
    public DbSet<CompraDetalle> CompraDetalles { get; set;}

    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Venta> Ventas { get; set; }
    public DbSet<VentaDetalle> VentaDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Mapeo de nombres de tablas (Singular como en tu DB)
        modelBuilder.Entity<Producto>().ToTable("Producto");
        modelBuilder.Entity<Proveedor>().ToTable("Proveedor");
        modelBuilder.Entity<Cliente>().ToTable("Cliente");
        modelBuilder.Entity<Compra>().ToTable("Compra");
        modelBuilder.Entity<CompraDetalle>().ToTable("CompraDetalle");
        modelBuilder.Entity<Venta>().ToTable("Venta");
        modelBuilder.Entity<VentaDetalle>().ToTable("VentaDetalle");
        modelBuilder.Entity<Usuario>().ToTable("Usuario");

        // 2. Configuración de Relación: Compra -> CompraDetalle
        modelBuilder.Entity<CompraDetalle>(entity =>
        {
            entity.HasOne(d => d.Compra)
                  .WithMany(c => c.CompraDetalles) // Coincide con List en Compra.cs
                  .HasForeignKey(d => d.CompraId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 3. Configuración de Relación: Venta -> VentaDetalle
        modelBuilder.Entity<VentaDetalle>(entity =>
        {
            entity.HasOne(d => d.Venta)
                  .WithMany(v => v.Detalles) // Coincide con List en Venta.cs
                  .HasForeignKey(d => d.VentaId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}