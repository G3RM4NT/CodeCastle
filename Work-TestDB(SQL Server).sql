
USE [Work-Test];
GO
-- Creación de la tabla de producto con un Check Constraint para
--que el campo stock no pueda tener valores negativos.

CREATE TABLE Producto (
    Id INT PRIMARY KEY IDENTITY(1,1),  Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255), PrecioUnitario DECIMAL(10,2) NOT NULL, Stock INT NOT NULL DEFAULT 0,
    CONSTRAINT CK_Producto_Stock CHECK (Stock >= 0)
);


-- Creación Tabla Proveedores

CREATE TABLE Proveedor (
  Id INT PRIMARY KEY IDENTITY(1,1),Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),Telefono NVARCHAR(20)
);


-- Creación Tabla Cliente

CREATE TABLE Cliente (
    Id INT PRIMARY KEY IDENTITY(1,1), Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100), Telefono NVARCHAR(20)
);

--Creación tabla usuario para manejar roles

CREATE TABLE Usuario (
    Id INT PRIMARY KEY IDENTITY(1,1), Email NVARCHAR(100) NOT NULL,
	Password NVARCHAR(255) NOT NULL,Rol NVARCHAR(20)
	NOT NULL 
);

--Tabla compra con relación en las tablas Proveedor y Usuario 

CREATE TABLE Compra (
    Id INT PRIMARY KEY IDENTITY(1,1), ProveedorId INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),   UsuarioId INT,
    FOREIGN KEY (ProveedorId) REFERENCES Proveedor(Id),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);

-- 
--Tabla Compra detalle con relación en las tablas compra y producto

CREATE TABLE CompraDetalle (
    Id INT PRIMARY KEY IDENTITY(1,1),CompraId INT NOT NULL,
    ProductoId INT NOT NULL, Cantidad INT NOT NULL,
    PrecioCompra DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (CompraId) REFERENCES Compra(Id),
    FOREIGN KEY (ProductoId) REFERENCES Producto(Id)
);


-- Tabla venta con relación en  tablas Cliente y Usuario

CREATE TABLE Venta (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClienteId INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    UsuarioId INT,
    FOREIGN KEY (ClienteId) REFERENCES Cliente(Id),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);


-- Tabla ventaDetalle con relación en tablas venta y producto
CREATE TABLE VentaDetalle (
    Id INT PRIMARY KEY IDENTITY(1,1), VentaId INT NOT NULL,
    ProductoId INT NOT NULL,  Cantidad INT NOT NULL,
    PrecioVenta DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (VentaId) REFERENCES Venta(Id),
    FOREIGN KEY (ProductoId) REFERENCES Producto(Id)
);

--Realizar un insert de prueba en cada tabla es una practica con la cual he trabajado
--en distintos proyectos para chequear que todo este bien y se relacione de manera correcta
--y es una luz verde y ahorra tiempo al ver el frontend y notar que si esta llamando los datos



USE [Work-Test];
GO

INSERT INTO Usuario (Email, Password, Rol) 
VALUES ('admin@empresa.com', '123456', 'Administrador');

INSERT INTO Proveedor (Nombre, Email, Telefono) 
VALUES ('Proveedor ABC', 'contacto@abc.com', '555-1001');

INSERT INTO Cliente (Nombre, Email, Telefono) 
VALUES ('Juan Pérez', 'juan.perez@email.com', '555-2001');

INSERT INTO Producto (Nombre, Descripcion, PrecioUnitario, Stock) 
VALUES ('Laptop Dell', 'Laptop i7 16GB', 1200.00, 10);

INSERT INTO Compra (ProveedorId, UsuarioId) 
VALUES (1, 1);

INSERT INTO CompraDetalle (CompraId, ProductoId, Cantidad, PrecioCompra) 
VALUES (1, 1, 5, 1100.00);

INSERT INTO Venta (ClienteId, UsuarioId) 
VALUES (1, 1);

INSERT INTO VentaDetalle (VentaId, ProductoId, Cantidad, PrecioVenta) 
VALUES (1, 1, 2, 1250.00);


