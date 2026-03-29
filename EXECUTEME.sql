USE [master]
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Work-Test')
BEGIN
    CREATE DATABASE [Work-Test]
END
GO

USE [Work-Test]
GO

IF OBJECT_ID('[dbo].[Usuario]', 'U') IS NULL
BEGIN
CREATE TABLE [dbo].[Usuario](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Email] [nvarchar](100) NOT NULL,
    [Password] [nvarchar](255) NOT NULL,
    [Rol] [nvarchar](20) NOT NULL,
    [Nombre] [nvarchar](100) NULL
)
END
GO

IF OBJECT_ID('[dbo].[Cliente]', 'U') IS NULL
BEGIN
CREATE TABLE [dbo].[Cliente](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Nombre] [nvarchar](100) NOT NULL,
    [Email] [nvarchar](100) NULL,
    [Telefono] [nvarchar](20) NULL
)
END
GO

IF OBJECT_ID('[dbo].[Proveedor]', 'U') IS NULL
BEGIN
CREATE TABLE [dbo].[Proveedor](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Nombre] [nvarchar](100) NOT NULL,
    [Email] [nvarchar](100) NULL,
    [Telefono] [nvarchar](20) NULL
)
END
GO

IF OBJECT_ID('[dbo].[Producto]', 'U') IS NULL
BEGIN
CREATE TABLE [dbo].[Producto](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Nombre] [nvarchar](100) NOT NULL,
    [Descripcion] [nvarchar](255) NULL,
    [PrecioUnitario] [decimal](10, 2) NOT NULL,
    [Stock] [int] NOT NULL DEFAULT 0,
    CONSTRAINT [CK_Producto_Stock] CHECK ([Stock] >= 0)
)
END
GO

IF OBJECT_ID('[dbo].[Compra]', 'U') IS NULL
BEGIN
CREATE TABLE [dbo].[Compra](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ProveedorId] [int] NOT NULL FOREIGN KEY REFERENCES [dbo].[Proveedor](Id),
    [Fecha] [datetime] NOT NULL DEFAULT GETDATE(),
    [UsuarioId] [int] NULL FOREIGN KEY REFERENCES [dbo].[Usuario](Id)
)
END
GO

IF OBJECT_ID('[dbo].[CompraDetalle]', 'U') IS NULL
BEGIN
CREATE TABLE [dbo].[CompraDetalle](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [CompraId] [int] NOT NULL,
    [ProductoId] [int] NOT NULL FOREIGN KEY REFERENCES [dbo].[Producto](Id),
    [Cantidad] [int] NOT NULL,
    [PrecioCompra] [decimal](10, 2) NOT NULL,
    CONSTRAINT [FK_CompraDetalle_Compra_Cascade] FOREIGN KEY([CompraId]) REFERENCES [dbo].[Compra](Id) ON DELETE CASCADE
)
END
GO

IF OBJECT_ID('[dbo].[Venta]', 'U') IS NULL
BEGIN
CREATE TABLE [dbo].[Venta](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ClienteId] [int] NOT NULL FOREIGN KEY REFERENCES [dbo].[Cliente](Id),
    [Fecha] [datetime] NOT NULL DEFAULT GETDATE(),
    [UsuarioId] [int] NULL FOREIGN KEY REFERENCES [dbo].[Usuario](Id)
)
END
GO

IF OBJECT_ID('[dbo].[VentaDetalle]', 'U') IS NULL
BEGIN
CREATE TABLE [dbo].[VentaDetalle](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [VentaId] [int] NOT NULL FOREIGN KEY REFERENCES [dbo].[Venta](Id) ON DELETE CASCADE,
    [ProductoId] [int] NOT NULL FOREIGN KEY REFERENCES [dbo].[Producto](Id),
    [Cantidad] [int] NOT NULL,
    [PrecioVenta] [decimal](10, 2) NOT NULL
)
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Usuario])
BEGIN
    SET IDENTITY_INSERT [dbo].[Usuario] ON 
    INSERT [dbo].[Usuario] ([Id], [Email], [Password], [Rol], [Nombre]) VALUES 
    (3, N'AdminCode@gmail.com', N'Test12345', N'Administrador', N'William Gonzalez'),
    (4, N'gonzagerman924@gmail.com', N'Test123', N'Vendedor', N'German Gonzalez'),
    (5, N'manuel@gmail.com', N'12345', N'Vendedor', N'Manuel Mejia'),
    (6, N'Blanca@gmail.com', N'blanca123', N'Vendedor', N'Blanca Ester')
    SET IDENTITY_INSERT [dbo].[Usuario] OFF
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Cliente])
BEGIN
    SET IDENTITY_INSERT [dbo].[Cliente] ON 
    INSERT [dbo].[Cliente] ([Id], [Nombre], [Email], [Telefono]) VALUES 
    (1, N'Juan Pérez', N'juan.perez@email.com', N'555-2001'),
    (2, N'María García', N'maria.garcia@email.com', N'555-2002'),
    (3, N'Carlos Rodríguez', N'carlos.rod@email.com', N'555-2003'),
    (4, N'Ana Martínez', N'ana.mtz@email.com', N'555-2004'),
    (5, N'Luis Hernández', N'luis.hndz@email.com', N'555-2005'),
    (6, N'Elena Gómez', N'elena.gomez@email.com', N'555-2006'),
    (7, N'Roberto Díaz', N'roberto.diaz@email.com', N'555-2007'),
    (8, N'Sofía López', N'sofia.lopez@email.com', N'555-2008')
    SET IDENTITY_INSERT [dbo].[Cliente] OFF
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Proveedor])
BEGIN
    SET IDENTITY_INSERT [dbo].[Proveedor] ON 
    INSERT [dbo].[Proveedor] ([Id], [Nombre], [Email], [Telefono]) VALUES 
    (1, N'Proveedor ABC', N'contacto@abc.com', N'555-1001'),
    (2, N'Distribuidora Global', N'contacto@globaldist.com', N'555-3001'),
    (3, N'TecnoSuministros S.A.', N'ventas@tecnosumi.com', N'555-3002'),
    (4, N'Importaciones Prime', N'info@primeimport.com', N'555-3003'),
    (5, N'Logística del Norte', N'logistica@norte.com', N'555-3004'),
    (6, N'Soluciones Industriales', N'soporte@solind.com', N'555-3005'),
    (7, N'Mercado Mayorista', N'pedidos@mercamayor.com', N'555-3006'),
    (8, N'Suministros Alpha', N'alpha@suministros.com', N'555-3007'),
    (9, N'Corporación Delta', N'admin@deltacorp.com', N'555-3008')
    SET IDENTITY_INSERT [dbo].[Proveedor] OFF
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[Producto])
BEGIN
    SET IDENTITY_INSERT [dbo].[Producto] ON 
    INSERT [dbo].[Producto] ([Id], [Nombre], [Descripcion], [PrecioUnitario], [Stock]) VALUES 
    (1, N'Laptop Dell', N'Laptop i7 16GB', 1200.00, 8),
    (31, N'ff', N'fff', 12.00, 1),
    (36, N'PS3', N'CONSOLA VIEJA', 200.00, 6),
    (37, N'PS4', N'Consola de videojuegos', 350.00, 1),
    (38, N'memoria ram', N'memoria ram pc', 150.00, 5)
    SET IDENTITY_INSERT [dbo].[Producto] OFF
END
GO