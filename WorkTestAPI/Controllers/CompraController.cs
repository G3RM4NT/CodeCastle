using Microsoft.AspNetCore.Mvc;
using WorkTestAPI.Data;
using WorkTestAPI.DTOS;
using WorkTestAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization; // <--- AGREGADO

namespace WorkTestAPI.Controllers
{
    [Authorize(Roles = "Administrador")] // <--- SOLO el Admin entra a este controlador
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var compras = await _context.Compras
                .Include(c => c.Proveedor)
                .Include(c => c.CompraDetalles)
                    .ThenInclude(d => d.Producto)
                .ToListAsync();

            return Ok(compras);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var compra = await _context.Compras
                .Include(c => c.CompraDetalles)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (compra == null) return NotFound();
            return Ok(compra);
        }

        [HttpPost]
        public async Task<IActionResult> CrearCompra(CompraDTO dto)
        {
            try
            {
                await _service.RegistrarCompra(dto);
                return Ok("Compra registrada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra == null) return NotFound();

            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();

            return Ok("Compra y detalles eliminados");
        }
    }
}