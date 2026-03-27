using WorkTestAPI.Data;
using WorkTestAPI.DTOS;
using WorkTestAPI.Models;

namespace WorkTestAPI.Services
{
    public class CompraService
    {
        private readonly AppDbContext _context;

        public CompraService(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarCompra(CompraDTO dto)
        {
            var compra = new Compra
            {
                ProveedorId = dto.ProveedorId,
                Fecha = DateTime.Now
            };

            _context.Compras.Add(compra);
            await _context.SaveChangesAsync();

            foreach (var item in dto.Detalles)
            {
                var detalle = new CompraDetalle
                {
                    CompraId = compra.Id,
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioCompra = item.PrecioCompra
                };

                _context.CompraDetalles.Add(detalle);

              
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto != null)
                {
                    producto.Stock += item.Cantidad;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}