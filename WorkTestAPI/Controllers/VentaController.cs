using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkTestAPI.Data;
using WorkTestAPI.DTOS;
using WorkTestAPI.Services;

namespace WorkTestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly VentaService _service;
        private readonly AppDbContext _context;

        public VentaController(VentaService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        // 🔍 GET ventas
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Detalles)
                .ToListAsync();

            return Ok(ventas);
        }

        // 🔍 GET by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var venta = await _context.Ventas
                .Where(v => v.Id == id)
                .FirstOrDefaultAsync();

            if (venta == null)
                return NotFound();

            return Ok(venta);
        }

        // ➕ POST
        [HttpPost]
        public async Task<IActionResult> CrearVenta(VentaDTO dto)
        {
            var resultado = await _service.RegistrarVenta(dto);

            if (resultado.Contains("insuficiente") || resultado.Contains("no existe"))
                return BadRequest(resultado);

            return Ok(resultado);
        }
    }
}