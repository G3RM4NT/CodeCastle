using Microsoft.AspNetCore.Mvc;
using WorkTestAPI.DTOS;
using WorkTestAPI.Services;

namespace WorkTestAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            var token = _service.Login(dto);

            if (token == null)
                return Unauthorized("Credenciales inválidas");

            return Ok(new { token });
        }

        // --- MÉTODO AÑADIDO PARA REGISTRO ---
        [HttpPost("register")]
        public IActionResult Register(UsuarioRegistroDTO dto)
        {
            var resultado = _service.Register(dto);

            if (!resultado)
                return BadRequest("El correo ya está registrado o los datos son inválidos");

            return Ok(new { message = "Usuario registrado correctamente" });
        }
    }
}