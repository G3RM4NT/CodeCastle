using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkTestAPI.Data;
using Microsoft.AspNetCore.Authorization; // <--- AGREGADO

namespace WorkTestAPI.Controllers
{
    [Authorize] // <--- Requiere estar logueado para cualquier reporte
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReporteController(AppDbContext context)
        {
            _context = context;
        }

        // 📦 STOCK EN TIEMPO REAL - Acceso: Ambos
        [HttpGet("stock")]
        public async Task<IActionResult> GetStock()
        {
            var productos = await _context.Productos
                .Select(p => new
                {
                    p.Id,
                    p.Nombre,
                    p.Stock,
                    p.PrecioUnitario
                })
                .ToListAsync();

            return Ok(productos);
        }

        // 💰 REPORTE VENTAS - Acceso: Ambos
        [HttpGet("ventas")]
        public async Task<IActionResult> GetVentasPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            var ventas = await _context.Ventas
                .Where(v => v.Fecha >= fechaInicio && v.Fecha <= fechaFin)
                .Select(v => new
                {
                    v.Id,
                    v.Fecha,
                    v.ClienteId,
                    Total = _context.VentaDetalles
                        .Where(d => d.VentaId == v.Id)
                        .Sum(d => d.Cantidad * d.PrecioVenta)
                })
                .ToListAsync();

            return Ok(ventas);
        }
    }
}