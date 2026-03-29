using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using WorkTestAPI.Data;
using WorkTestAPI.Repositories;
using WorkTestAPI.Services;

namespace WorkTestAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- AGREGADO: Configuración de CORS ---
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });
            // ---------------------------------------

            // 1.  Conexión a SQL Server
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2.  Controladores y Configuración de JSON (Evita ciclos infinitos)
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // 3.  Configuración de Autenticación JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
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
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            // 4.  Swagger con soporte para JWT (El botón "Authorize")
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WorkTestAPI", Version = "v1" });

                // Definir el esquema de seguridad
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingresa: Bearer [tu_token]"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // 5.  Inyección de Dependencias
            builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
            builder.Services.AddScoped<IProductoService, ProductoService>();
            builder.Services.AddScoped<AuthService>(); // Agregado para el Login
            builder.Services.AddScoped<CompraService>();
            builder.Services.AddScoped<VentaService>();

            var app = builder.Build();

            // 6.  Pipeline de la aplicación
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // --- AGREGADO: Habilitar CORS en el pipeline ---
            app.UseCors("AllowAll");
            // -----------------------------------------------

            app.UseHttpsRedirection();

            // IMPORTANTE: Authentication debe ir ANTES de Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            //  Debug (opcional)
            AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
            {
                Console.WriteLine(eventArgs.Exception.ToString());
            };

            app.Run();
        }
    }
}