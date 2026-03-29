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
                .Include(v => v.Cliente)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .OrderByDescending(v => v.Fecha)
                .ToListAsync();
        }

        // ✅ ESTE ES EL MÉTODO QUE TE FALTABA Y POR ESO EL CONTROLADOR DABA ERROR
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
                // Dentro de RegistrarVenta
                var venta = new Venta
                {
                    ClienteId = dto.ClienteId,
                    Fecha = DateTime.Now,
                    UsuarioId = dto.UsuarioId // <--- Ahora usamos el ID real del DTO
                };

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                foreach (var item in dto.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(item.ProductoId);

                    if (producto == null) throw new Exception($"Producto {item.ProductoId} no existe");
                    if (producto.Stock < item.Cantidad) throw new Exception($"Stock insuficiente para {producto.Nombre}");

                    // Descontar Stock
                    producto.Stock -= item.Cantidad;
                    _context.Productos.Update(producto);

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
                return "Venta registrada correctamente";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return $"Error: {ex.Message}";
            }
        }
    }
}