using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims; // <--- Necesario para ClaimTypes
using System.Text;
using Microsoft.EntityFrameworkCore; // <--- Agregado por seguridad
using WorkTestAPI.Data;
using WorkTestAPI.DTOS;
using WorkTestAPI.Models; // <--- ASEGÚRATE de tener este para reconocer la clase Usuario

namespace WorkTestAPI.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public string? Login(LoginDTO dto)
        {
            // Si te marca error en .Usuarios, revisa si en AppDbContext lo llamaste Usuarios o Users
            var user = _context.Usuarios
                .FirstOrDefault(u => u.Email == dto.Email && u.Password == dto.Password);

            if (user == null)
                return null;

            // ClaimTypes ahora debería reconocerse por el using System.Security.Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Rol ?? "Administrador"), // Evita nulos en el Rol
                new Claim("Id", user.Id.ToString()) // Siempre es útil tener el ID en el token
            };

            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey)) throw new Exception("JWT Key no configurada");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}