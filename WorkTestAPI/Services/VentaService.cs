using Microsoft.EntityFrameworkCore;
using WorkTestAPI.Data;
using WorkTestAPI.DTOS;
using WorkTestAPI.Models;

namespace WorkTestAPI.Services
{
    public class VentaService
    {
        private readonly AppDbContext _context;

        public VentaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Venta>> ObtenerTodas()
        {
            return await _context.Ventas
                .Include(v => v.Cliente) // 🔥 IMPORTANTE para el reporte
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto) // 🔥 IMPORTANTE para ver nombres de productos
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();
        }

        public async Task<Venta?> ObtenerPorId(int id)
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<string> RegistrarVenta(VentaDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Validar Stock
                foreach (var item in dto.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(item.ProductoId);
                    if (producto == null) return $"Producto {item.ProductoId} no existe";
                    if (producto.Stock < item.Cantidad) return $"Stock insuficiente para {producto.Nombre}";
                }

                // 2. Crear Venta
                var venta = new Venta
                {
                    ClienteId = dto.ClienteId,
                    Fecha = DateTime.Now,
                    UsuarioId = 1
                };

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                // 3. Detalles + Descontar Stock
                foreach (var item in dto.Detalles)
                {
                    // Descontar Stock
                    var producto = await _context.Productos.FindAsync(item.ProductoId);
                    if (producto != null) producto.Stock -= item.Cantidad;

                    var detalle = new VentaDetalle
                    {
                        VentaId = venta.Id,
                        ProductoId = item.ProductoId,
                        Cantidad = item.Cantidad,
                        PrecioVenta = item.PrecioVenta
                    };
                    _context.VentaDetalles.Add(detalle);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return "Venta registrada correctamente 🔥";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return $"Error fatal: {ex.Message}";
            }
        }
    }
}