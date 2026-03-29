using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using WorkTestAPI.Data;
using WorkTestAPI.Repositories;
using WorkTestAPI.Services;
using WorkTestAPI.Models; // Asegúrate de que esto apunte a donde están tus clases Usuario, Producto, etc.

namespace WorkTestAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- Configuración de CORS ---
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

            // 1. Conexión a SQL Server
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Controladores y Configuración de JSON
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // 3. Configuración de Autenticación JWT
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

            // 4. Swagger con soporte para JWT
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WorkTestAPI", Version = "v1" });
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
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] {}
                    }
                });
            });

            // 5. Inyección de Dependencias
            builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
            builder.Services.AddScoped<IProductoService, ProductoService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<CompraService>();
            builder.Services.AddScoped<VentaService>();

            var app = builder.Build();

            // ==========================================================
            // LÓGICA DE CREACIÓN DE TABLAS E INSERTS AUTOMÁTICOS
            // ==========================================================
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();

                    // Crea la base de datos y las tablas si no existen
                    context.Database.EnsureCreated();

                    // --- INSERTS DE USUARIOS ---
                    if (!context.Usuarios.Any())
                    {
                        context.Usuarios.AddRange(
                            new Usuario { Email = "AdminCode@gmail.com", Password = "Test12345", Rol = "Administrador", Nombre = "William Gonzalez" },
                            new Usuario { Email = "gonzagerman924@gmail.com", Password = "Test123", Rol = "Vendedor", Nombre = "German Gonzalez" },
                            new Usuario { Email = "manuel@gmail.com", Password = "12345", Rol = "Vendedor", Nombre = "Manuel Mejia" },
                            new Usuario { Email = "Blanca@gmail.com", Password = "blanca123", Rol = "Vendedor", Nombre = "Blanca Ester" }
                        );
                    }

                    // --- INSERTS DE CLIENTES ---
                    if (!context.Clientes.Any())
                    {
                        context.Clientes.AddRange(
                            new Cliente { Nombre = "Juan Pérez", Email = "juan.perez@email.com", Telefono = "555-2001" },
                            new Cliente { Nombre = "María García", Email = "maria.garcia@email.com", Telefono = "555-2002" },
                            new Cliente { Nombre = "Carlos Rodríguez", Email = "carlos.rod@email.com", Telefono = "555-2003" }
                        );
                    }

                    // --- INSERTS DE PROVEEDORES ---
                    if (!context.Proveedores.Any())
                    {
                        context.Proveedores.AddRange(
                            new Proveedor { Nombre = "Proveedor ABC", Email = "contacto@abc.com", Telefono = "555-1001" },
                            new Proveedor { Nombre = "Corporación Delta", Email = "admin@deltacorp.com", Telefono = "555-3008" }
                        );
                    }

                    // --- INSERTS DE PRODUCTOS ---
                    if (!context.Productos.Any())
                    {
                        context.Productos.AddRange(
                            new Producto { Nombre = "Laptop Dell", Descripcion = "Laptop i7 16GB", PrecioUnitario = 1200, Stock = 8 },
                            new Producto { Nombre = "PS3", Descripcion = "Consola vieja", PrecioUnitario = 200, Stock = 6 },
                            new Producto { Nombre = "PS4", Descripcion = "Consola", PrecioUnitario = 350, Stock = 1 }
                        );
                    }

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ocurrió un error al crear o sembrar la base de datos.");
                }
            }
            // ==========================================================

            // 6. Pipeline de la aplicación
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                // Habilitamos Swagger en producción para que puedas probarlo en Railway
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}