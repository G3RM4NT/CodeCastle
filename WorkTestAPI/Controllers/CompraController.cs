using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkTestAPI.Data;
using WorkTestAPI.DTOS;
using WorkTestAPI.Models;
using WorkTestAPI.Services;

namespace WorkTestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompraController : ControllerBase
    {
        private readonly CompraService _service;
        private readonly AppDbContext _context;

        public CompraController(CompraService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        // 🔍 GET: api/compra (todas las compras)
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var compras = await _context.Compras
     .Include(c => c.Proveedor)
     .Include(c => c.Detalles)
     .ThenInclude(d => d.Producto)
     .ToListAsync();

            return Ok(compras);
        }

        // 🔍 GET by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var compra = await _context.Compras
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (compra == null)
                return NotFound();

            return Ok(compra);
        }

        // ➕ POST (crear compra)
        [HttpPost]
        public async Task<IActionResult> CrearCompra(CompraDTO dto)
        {
            await _service.RegistrarCompra(dto);
            return Ok("Compra registrada correctamente ");
        }

        // ❌ DELETE (opcional)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var compra = await _context.Compras.FindAsync(id);

            if (compra == null)
                return NotFound();

            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();

            return Ok("Compra eliminada");
        }
    }
}