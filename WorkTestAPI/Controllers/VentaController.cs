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
            // Validar campos obligatorios básicos
            if (dto == null || dto.ClienteId <= 0 || dto.Detalles == null || dto.Detalles.Count == 0)
            {
                return BadRequest(new { message = "Advertencia: completar campos obligatorios." });
            }

            try
            {
                var resultado = await _service.RegistrarVenta(dto);

                // Manejo de errores de lógica de negocio (Stock, existencia)
                if (resultado.Contains("insuficiente") || resultado.Contains("no existe"))
                {
                    return BadRequest(new { message = resultado });
                }

                // Si todo sale bien, retornamos un mensaje de éxito
                return Ok(new { message = "Venta registrada con éxito", detalle = resultado });
            }
            catch (Exception ex)
            {
                // Loguear el error (opcional)
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}