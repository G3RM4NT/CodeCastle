using Microsoft.AspNetCore.Mvc;
using WorkTestAPI.Models;
using WorkTestAPI.Services;

namespace WorkTestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _service;

        public ProductoController(IProductoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => Ok(await _service.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _service.GetById(id);
            if (producto == null) return NotFound();
            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
            => Ok(await _service.Create(producto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Producto producto)
            => Ok(await _service.Update(id, producto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);
            if (!result) return NotFound();
            return Ok();
        }
    }
}