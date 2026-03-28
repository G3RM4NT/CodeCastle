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

        public async Task<string> RegistrarVenta(VentaDTO dto)
        {
            // 🔥 VALIDAR STOCK
            foreach (var item in dto.Detalles)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);

                if (producto == null)
                    return $"Producto {item.ProductoId} no existe";

                if (producto.Stock < item.Cantidad)
                    return $"Stock insuficiente para el producto {producto.Nombre}";
            }

            // 🧾 CREAR VENTA
            var venta = new Venta
            {
                ClienteId = dto.ClienteId,
                Fecha = DateTime.Now
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            //  DETALLES + DESCONTAR STOCK
            foreach (var item in dto.Detalles)
            {
                var detalle = new VentaDetalle
                {
                    VentaId = venta.Id,
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioVenta = item.PrecioVenta
                };

                _context.VentaDetalles.Add(detalle);

                var producto = await _context.Productos.FindAsync(item.ProductoId);
                producto.Stock -= item.Cantidad; // 💣 DESCUENTA STOCK
            }

            await _context.SaveChangesAsync();

            return "Venta registrada correctamente 🔥";
        }
    }
}