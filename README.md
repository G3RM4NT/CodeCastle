
# Work-Test-Code-Castle-
Sistema de gestión de ventas desarrollado como prueba técnica con Angular en el frontend y .NET en el backend.

Creación de sistema de gestión de ventas
desarrollado como prueba tecnica con
angular en el frontend y.net en el
backend.

Parte 1 (Toma de requerimientos)

Analice lo que se pide en la evaluación
y comprendi la estructura del proyecto
detalladamente.

Parte 2 (Estructura de datos)

Generalmente por lo personal, prefiero
comenzar por la base de datos ya que me
ayuda a manejar mas la logica y estructura 
de datos, dentro del archivo del query 
se encuentran cada consulta comentada .

Parte 2.5(Instalación de frameworks)
1. Microsoft.EntityFrameworkCore.SqlServer (8.0.0)
Este es el conector. Es el que le permite a tu código de C# "hablar" con una base de datos de SQL Server. Sin este paquete, Entity Framework no sabría cómo traducir tus consultas de C# al lenguaje SQL que entiende tu base de datos.

2. Microsoft.EntityFrameworkCore.Tools (8.0.0)
Estas son las herramientas de consola. Se usan principalmente para gestionar las Migraciones. Gracias a este paquete puedes usar comandos como Add-Migration y Update-Database dentro de Visual Studio para que tus tablas se creen o actualicen automáticamente.

3. Microsoft.EntityFrameworkCore.Design (8.0.0)
Este paquete trabaja en conjunto con el de Tools. Contiene la lógica necesaria para que el motor de Entity Framework pueda generar código automáticamente durante el tiempo de diseño (por ejemplo, cuando creas los archivos de migración basados en tus modelos).

4. Swashbuckle.AspNetCore (6.5.0)
Este es el paquete que genera Swagger. Es el responsable de crear esa página web azul y verde que usas para probar tus métodos GET y POST. Básicamente, lee tu código y genera una documentación interactiva para que no tengas que usar herramientas externas para probar la API.


Parte 3 (Conexión Base de datos y Creación de WEB-API)
Instalación de frameworks
Conexión de base de datos y realice los modelos 
Utilicé una arquitectura en capas aplicando el patrón Repository y Service para separar la lógica de negocio del acceso a datos, logrando un código más mantenible, escalable y testeable.
Realice el controlador de productos y hice
las pruebas de metodos con postman.
Configuré el mapeo en OnModelCreating para evitar la pluralización automática de Entity Framework y respetar la estructura existente de la base de datos
