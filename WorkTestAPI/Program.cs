using Microsoft.EntityFrameworkCore;
using WorkTestAPI.Data;
using WorkTestAPI.Repositories;
using WorkTestAPI.Services;
using System.Text.Json.Serialization; // <--- SE AGREGÓ ESTO
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WorkTestAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 🔌 Conexión a SQL Server
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 📦 Servicios y Configuración de JSON (PARA EVITAR CICLOS)
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Esto soluciona el error "A possible object cycle was detected"
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // 🧪 Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Inyección de dependencias
            builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
            builder.Services.AddScoped<IProductoService, ProductoService>();
            builder.Services.AddScoped<CompraService>();
            builder.Services.AddScoped<VentaService>();

            builder.Services.AddScoped<AuthService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                        )
                    };
                });

            var app = builder.Build();

            // 🚀 Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

           

            app.MapControllers();

            app.UseAuthentication();
            app.UseAuthorization();

            // 🔍 Debug (opcional)
            AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
            {
                Console.WriteLine(eventArgs.Exception.ToString());
            };

            app.Run();
        }
    }
}