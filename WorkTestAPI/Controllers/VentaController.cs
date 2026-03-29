using Microsoft.AspNetCore.Mvc;
using WorkTestAPI.DTOS;
using WorkTestAPI.Services;

namespace WorkTestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {
        private readonly VentaService _service;

        // Solo inyectamos el servicio para mantener el controlador limpio
        public VentaController(VentaService service)
        {
            _service = service;
        }

        // 🔍 GET: api/Venta
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ventas = await _service.ObtenerTodas();
            return Ok(ventas);
        }

        // 🔍 GET: api/Venta/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var venta = await _service.ObtenerPorId(id);

            if (venta == null)
                return NotFound(new { message = $"La venta con ID {id} no existe." });

            return Ok(venta);
        }

        // ➕ POST: api/Venta
        [HttpPost]
        public async Task<IActionResult> CrearVenta([FromBody] VentaDTO dto)
        {
            // LOG DE DEPURACIÓN: Esto aparecerá en tu consola de Visual Studio
            Console.WriteLine($"Recibiendo venta para Cliente: {dto.ClienteId}");
            Console.WriteLine($"Cantidad de detalles recibidos: {dto?.Detalles?.Count ?? 0}");

            if (dto == null || dto.Detalles == null || dto.Detalles.Count == 0)
            {
                return BadRequest(new { message = "El servidor recibió la lista de productos VACÍA. Revisa los nombres en Angular." });
            }

            var resultado = await _service.RegistrarVenta(dto);

            if (resultado.Contains("Error"))
            {
                return StatusCode(500, new { message = resultado });
            }

            return Ok(new { message = resultado });
        }
    }
}