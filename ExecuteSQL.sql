USE [master]
GO


IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'Work-Test')
BEGIN
    CREATE DATABASE [Work-Test]
END
GO

ALTER DATABASE [Work-Test] SET COMPATIBILITY_LEVEL = 160
GO

USE [Work-Test]
GO

--2. CREACIÓN DE TABLAS 

-- Tabla Cliente
CREATE TABLE [dbo].[Cliente](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Telefono] [nvarchar](20) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

-- Tabla Proveedor
CREATE TABLE [dbo].[Proveedor](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Telefono] [nvarchar](20) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

-- Tabla Usuario
CREATE TABLE [dbo].[Usuario](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Rol] [nvarchar](20) NOT NULL,
	[Nombre] [nvarchar](100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

-- Tabla Producto
CREATE TABLE [dbo].[Producto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Descripcion] [nvarchar](255) NULL,
	[PrecioUnitario] [decimal](10, 2) NOT NULL,
	[Stock] [int] NOT NULL DEFAULT ((0)),
    PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

-- Tabla Compra
CREATE TABLE [dbo].[Compra](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProveedorId] [int] NOT NULL,
	[Fecha] [datetime] NOT NULL DEFAULT (getdate()),
	[UsuarioId] [int] NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

-- Tabla CompraDetalle
CREATE TABLE [dbo].[CompraDetalle](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompraId] [int] NOT NULL,
	[ProductoId] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[PrecioCompra] [decimal](10, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

-- Tabla Venta
CREATE TABLE [dbo].[Venta](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClienteId] [int] NOT NULL,
	[Fecha] [datetime] NOT NULL DEFAULT (getdate()),
	[UsuarioId] [int] NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

-- Tabla VentaDetalle
CREATE TABLE [dbo].[VentaDetalle](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VentaId] [int] NOT NULL,
	[ProductoId] [int] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[PrecioVenta] [decimal](10, 2) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
) ON [PRIMARY]
GO

/****** 3. INSERCIÓN DE DATOS INICIALES ******/

SET IDENTITY_INSERT [dbo].[Cliente] ON 
INSERT [dbo].[Cliente] ([Id], [Nombre], [Email], [Telefono]) VALUES (1, N'Juan Pérez', N'juan.perez@email.com', N'555-2001')
SET IDENTITY_INSERT [dbo].[Cliente] OFF
GO

SET IDENTITY_INSERT [dbo].[Proveedor] ON 
INSERT [dbo].[Proveedor] ([Id], [Nombre], [Email], [Telefono]) VALUES (1, N'Proveedor ABC', N'contacto@abc.com', N'555-1001')
SET IDENTITY_INSERT [dbo].[Proveedor] OFF
GO

SET IDENTITY_INSERT [dbo].[Usuario] ON 
INSERT [dbo].[Usuario] ([Id], [Email], [Password], [Rol], [Nombre]) VALUES (1, N'admin@empresa.com', N'123456', N'Administrador', N'Admin Principal')
INSERT [dbo].[Usuario] ([Id], [Email], [Password], [Rol], [Nombre]) VALUES (2, N'codecastle@gmail.com', N'test123', N'Vendedor', N'Testeo1')
SET IDENTITY_INSERT [dbo].[Usuario] OFF
GO

SET IDENTITY_INSERT [dbo].[Producto] ON 
INSERT [dbo].[Producto] ([Id], [Nombre], [Descripcion], [PrecioUnitario], [Stock]) VALUES (1, N'Laptop Dell', N'Laptop i7 16GB', CAST(1200.00 AS Decimal(10, 2)), 9)
SET IDENTITY_INSERT [dbo].[Producto] OFF
GO

/****** 4. RELACIONES Y RESTRICCIONES ******/

ALTER TABLE [dbo].[Compra] WITH CHECK ADD FOREIGN KEY([ProveedorId]) REFERENCES [dbo].[Proveedor] ([Id])
GO
ALTER TABLE [dbo].[Compra] WITH CHECK ADD FOREIGN KEY([UsuarioId]) REFERENCES [dbo].[Usuario] ([Id])
GO
ALTER TABLE [dbo].[CompraDetalle] WITH CHECK ADD FOREIGN KEY([ProductoId]) REFERENCES [dbo].[Producto] ([Id])
GO
ALTER TABLE [dbo].[CompraDetalle] WITH CHECK ADD CONSTRAINT [FK_CompraDetalle_Compra_Cascade] FOREIGN KEY([CompraId]) REFERENCES [dbo].[Compra] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Venta] WITH CHECK ADD FOREIGN KEY([ClienteId]) REFERENCES [dbo].[Cliente] ([Id])
GO
ALTER TABLE [dbo].[Venta] WITH CHECK ADD FOREIGN KEY([UsuarioId]) REFERENCES [dbo].[Usuario] ([Id])
GO
ALTER TABLE [dbo].[VentaDetalle] WITH CHECK ADD FOREIGN KEY([ProductoId]) REFERENCES [dbo].[Producto] ([Id])
GO
ALTER TABLE [dbo].[VentaDetalle] WITH CHECK ADD FOREIGN KEY([VentaId]) REFERENCES [dbo].[Venta] ([Id])
GO
ALTER TABLE [dbo].[Producto] WITH CHECK ADD CONSTRAINT [CK_Producto_Stock] CHECK (([Stock]>=(0)))
GO

PRINT 'Base de datos Work-Test creada y configurada correctamente.';
GO