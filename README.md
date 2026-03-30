WorkTestAPI - Sistema de Gestión Full Stack
Este proyecto es una solución integral diseñada para la gestión de ventas, compras y productos. El sistema se compone de una API REST desarrollada en .NET con arquitectura en capas y una interfaz web moderna en Angular.

Requisitos Previos

Antes de ejecutar el proyecto, asegúrese de contar con las siguientes herramientas instaladas:

Node.js versión 20.x o superior (requerido para Angular CLI)
Angular CLI
.NET 8 o superior
SQL Server

Importante:
El proyecto frontend está desarrollado con Angular 17+, el cual requiere obligatoriamente Node.js v20.19 o superior.
Si se utiliza una versión anterior (como Node 18), la aplicación no podrá ejecutarse y mostrará errores al iniciar con ng serve.

Tecnologías y Paquetes
Justificación Tecnológica
La elección de Angular para el frontend y SQL Server para la persistencia de datos se debe a que son las tecnologías que el desarrollador domina por completo, permitiendo una implementación eficiente, escalable y alineada con los estándares actuales de desarrollo.

Backend (.NET Core API)
Arquitectura: Estructura desacoplada mediante capas (Controllers, Services, Repositories, Data, Models, DTOs).

Paquetes NuGet Principales:

Microsoft.EntityFrameworkCore.SqlServer: Driver para la conexión con SQL Server.

Microsoft.EntityFrameworkCore.Tools: Permite ejecutar comandos de Entity Framework desde la consola.

Microsoft.EntityFrameworkCore.Design (8.0.0): Proporciona la lógica necesaria para que el motor de EF genere código automáticamente durante el tiempo de diseño para la creación de migraciones.

Swashbuckle.AspNetCore (6.5.0): Genera la documentación interactiva de Swagger, permitiendo visualizar y probar los métodos GET y POST de forma directa sin herramientas externas.

Microsoft.AspNetCore.Authentication.JwtBearer: Implementación de seguridad basada en tokens.

Frontend (Angular)
Framework: Angular 17+ utilizando una estructura modular.

Componentes clave: Interceptores para el manejo de tokens, Guards para protección de rutas y servicios especializados para el consumo de la API.

Instrucciones de Ejecución Local
1. Base de Datos (SQL Server)
Es fundamental ejecutar el script de base de datos incluido en la raíz del repositorio. Este archivo contiene la estructura y los registros esenciales (clientes, proveedores, productos y usuarios) necesarios para el funcionamiento del sistema.

Nota importante: Si no se ejecuta este script antes de iniciar la aplicación, el sistema no mostrará información en los listados de clientes ni de proveedores, y no será posible realizar el inicio de sesión ni probar los flujos de ventas y compras.

Pasos para configurar:

Abra SQL Server Management Studio o Azure Data Studio.

Cree una base de datos llamada Work-Test.

Ejecute el script proporcionado para generar las tablas e insertar los datos iniciales.

2. Configuración del Backend
Configurar la conexión: Edite el archivo appsettings.json con su cadena de conexión local apuntando a la base de datos Work-Test.

"DefaultConnection": "Server=.;Database=Work-Test;Trusted_Connection=True;TrustServerCertificate=True;"

Iniciar la API: Ejecute el comando dotnet run en la carpeta del proyecto.

3. Configuración del Frontend
Instalar dependencias: Ejecute el comando npm install.

Lanzar aplicación: Ejecute el comando ng serve.

Acceso: Abra el navegador en http://localhost:4200.

Supuestos Técnicos y de Negocio
Módulo de Reportes Avanzado: El reporte de ventas permite filtrar por rango de fechas y por cliente específico. Además, se incluye un módulo extra de reporte de compras para auditoría de ingresos.

Script de Inicialización: El ejecutable SQL asegura que el sistema cuente con la información necesaria para que todas las pantallas y filtros operen correctamente desde el primer uso.

Seguridad: Uso de JWT para proteger la información y asegurar que solo personal autorizado gestione el inventario y las ventas.

Arquitectura: Implementación de DTOs para garantizar que la comunicación entre la API y Angular sea segura y eficiente.


NOTA: Destacar que al crear la cuenta permite seleccionar los roles Vendedor y admin esto no deberia de ser asi pero se ha realizado para que sea mas practico en la prueba
al seleccionar admin tienen acceso total pero al seleccionar vendedor solo tienen acceso a ciertos modulos .
