--CREACIÓN DE LA BASE DE DATOS

IF DB_ID('RestaurantHubDB') IS NULL
BEGIN
    CREATE DATABASE RestaurantHubDB;
END
GO

USE RestaurantHubDB;
GO

-- Eliminacion de las tablas existentes (en orden inverso a sus dependencias)
-- para poder ejecutar el script nuevamente desde cero
DROP TABLE IF EXISTS ALERTA_REABASTECIMIENTO;
DROP TABLE IF EXISTS PRODUCTO_INGREDIENTE;
DROP TABLE IF EXISTS PREDICCION_DEMANDA;
DROP TABLE IF EXISTS FACTURA;
DROP TABLE IF EXISTS PAGO;
DROP TABLE IF EXISTS DETALLE_PEDIDO;
DROP TABLE IF EXISTS PEDIDO;
DROP TABLE IF EXISTS INVENTARIO;
DROP TABLE IF EXISTS INGREDIENTE;
DROP TABLE IF EXISTS PRODUCTO;
DROP TABLE IF EXISTS CATEGORIA;
DROP TABLE IF EXISTS CLIENTE;
DROP TABLE IF EXISTS USUARIO;
GO

-- Creacion de la tabla "USUARIO"
CREATE TABLE USUARIO (
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50) NOT NULL,
    correo VARCHAR(100) NOT NULL UNIQUE,
    contrasena VARCHAR(255) NOT NULL,
    rol VARCHAR(20) NOT NULL
);

-- Creacion de la tabla "CLIENTE"
CREATE TABLE CLIENTE (
    id_cliente INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50) NOT NULL,
    telefono VARCHAR(20),
    correo VARCHAR(100)
);

-- Creacion de la tabla "CATEGORIA"
CREATE TABLE CATEGORIA (
    id_categoria INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE,
    descripcion VARCHAR(150)
);

-- Creacion de la tabla "PRODUCTO"
CREATE TABLE PRODUCTO (
    id_producto INT IDENTITY(1,1) PRIMARY KEY,
    id_categoria INT NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(200),
    precio DECIMAL(10,2) NOT NULL,
    disponible BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_PRODUCTO_CATEGORIA
        FOREIGN KEY (id_categoria)
        REFERENCES CATEGORIA(id_categoria),

    CONSTRAINT CK_PRODUCTO_PRECIO
        CHECK (precio >= 0)
);

-- Creacion de la tabla "INGREDIENTE"
CREATE TABLE INGREDIENTE (
    id_ingrediente INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    unidad_medida VARCHAR(30) NOT NULL,
    stock_actual DECIMAL(10,2) NOT NULL,
    stock_minimo DECIMAL(10,2) NOT NULL,

    CONSTRAINT CK_INGREDIENTE_STOCK_ACTUAL
        CHECK (stock_actual >= 0),

    CONSTRAINT CK_INGREDIENTE_STOCK_MINIMO
        CHECK (stock_minimo >= 0)
);

-- Creacion de la tabla "PRODUCTO_INGREDIENTE" (receta de cada producto)
CREATE TABLE PRODUCTO_INGREDIENTE (
    id_producto INT NOT NULL,
    id_ingrediente INT NOT NULL,
    cantidad DECIMAL(10,2) NOT NULL,

    CONSTRAINT PK_PRODUCTO_INGREDIENTE
        PRIMARY KEY (id_producto, id_ingrediente),

    CONSTRAINT FK_RECETA_PRODUCTO
        FOREIGN KEY (id_producto)
        REFERENCES PRODUCTO(id_producto),

    CONSTRAINT FK_RECETA_INGREDIENTE
        FOREIGN KEY (id_ingrediente)
        REFERENCES INGREDIENTE(id_ingrediente),

    CONSTRAINT CK_RECETA_CANTIDAD
        CHECK (cantidad > 0)
);

-- Creacion de la tabla "PEDIDO"
CREATE TABLE PEDIDO (
    id_pedido INT IDENTITY(1,1) PRIMARY KEY,
    id_cliente INT NOT NULL,
    id_usuario INT NOT NULL,
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    estado VARCHAR(30) NOT NULL,
    total DECIMAL(10,2) NOT NULL,

    CONSTRAINT FK_PEDIDO_CLIENTE
        FOREIGN KEY (id_cliente)
        REFERENCES CLIENTE(id_cliente),

    CONSTRAINT FK_PEDIDO_USUARIO
        FOREIGN KEY (id_usuario)
        REFERENCES USUARIO(id_usuario),

    CONSTRAINT CK_PEDIDO_TOTAL
        CHECK (total >= 0)
);

-- Creacion de la tabla "DETALLE_PEDIDO"
CREATE TABLE DETALLE_PEDIDO (
    id_detalle INT IDENTITY(1,1) PRIMARY KEY,
    id_pedido INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,

    CONSTRAINT FK_DETALLE_PEDIDO
        FOREIGN KEY (id_pedido)
        REFERENCES PEDIDO(id_pedido),

    CONSTRAINT FK_DETALLE_PRODUCTO
        FOREIGN KEY (id_producto)
        REFERENCES PRODUCTO(id_producto),

    CONSTRAINT CK_DETALLE_CANTIDAD
        CHECK (cantidad > 0),

    CONSTRAINT CK_DETALLE_PRECIO
        CHECK (precio_unitario >= 0),

    CONSTRAINT CK_DETALLE_SUBTOTAL
        CHECK (subtotal >= 0)
);

-- Creacion de la tabla "PAGO"
CREATE TABLE PAGO (
    id_pago INT IDENTITY(1,1) PRIMARY KEY,
    id_pedido INT NOT NULL UNIQUE,
    fecha_pago DATETIME NOT NULL DEFAULT GETDATE(),
    monto DECIMAL(10,2) NOT NULL,
    metodo_pago VARCHAR(30) NOT NULL,
    estado VARCHAR(20) NOT NULL,

    CONSTRAINT FK_PAGO_PEDIDO
        FOREIGN KEY (id_pedido)
        REFERENCES PEDIDO(id_pedido),

    CONSTRAINT CK_PAGO_MONTO
        CHECK (monto >= 0)
);

-- Creacion de la tabla "FACTURA"
CREATE TABLE FACTURA (
    id_factura INT IDENTITY(1,1) PRIMARY KEY,
    id_pedido INT NOT NULL UNIQUE,
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    total DECIMAL(10,2) NOT NULL,

    CONSTRAINT FK_FACTURA_PEDIDO
        FOREIGN KEY (id_pedido)
        REFERENCES PEDIDO(id_pedido),

    CONSTRAINT CK_FACTURA_TOTAL
        CHECK (total >= 0)
);

-- Creacion de la tabla "PREDICCION_DEMANDA"
CREATE TABLE PREDICCION_DEMANDA (
    id_prediccion INT IDENTITY(1,1) PRIMARY KEY,
    id_producto INT NOT NULL,
    fecha DATETIME NOT NULL,
    cantidad_estimada INT NOT NULL,

    CONSTRAINT FK_PREDICCION_PRODUCTO
        FOREIGN KEY (id_producto)
        REFERENCES PRODUCTO(id_producto),

    CONSTRAINT CK_PREDICCION_CANTIDAD
        CHECK (cantidad_estimada >= 0)
);

-- Creacion de la tabla "ALERTA_REABASTECIMIENTO"
CREATE TABLE ALERTA_REABASTECIMIENTO (
    id_alerta INT IDENTITY(1,1) PRIMARY KEY,
    id_ingrediente INT NOT NULL,
    fecha DATETIME NOT NULL DEFAULT GETDATE(),
    estado VARCHAR(20) NOT NULL,

    CONSTRAINT FK_ALERTA_INGREDIENTE
        FOREIGN KEY (id_ingrediente)
        REFERENCES INGREDIENTE(id_ingrediente)
);
GO

-- Registro de datos de prueba
INSERT INTO USUARIO
    (nombre, apellido, correo, contrasena, rol)
VALUES
    ('Carlos', 'Rodriguez', 'carlos@restauranthub.com', '123456', 'ADMIN'),
    ('Laura', 'Martinez', 'laura@restauranthub.com', '123456', 'EMPLEADO');

INSERT INTO CLIENTE
    (nombre, apellido, telefono, correo)
VALUES
    ('Juan', 'Perez', '3001234567', 'juan@gmail.com'),
    ('Maria', 'Gomez', '3019876543', 'maria@gmail.com'),
    ('Andres', 'Lopez', '3105556677', 'andres@gmail.com');

INSERT INTO CATEGORIA
    (nombre, descripcion)
VALUES
    ('Hamburguesas', 'Hamburguesas y productos relacionados'),
    ('Bebidas', 'Bebidas frias y calientes'),
    ('Acompañamientos', 'Productos para acompañar los platos principales');

INSERT INTO PRODUCTO
    (id_categoria, nombre, descripcion, precio, disponible)
VALUES
    (1, 'Hamburguesa Clasica', 'Hamburguesa con carne, queso, tomate y lechuga', 18000.00, 1),
    (1, 'Hamburguesa Especial', 'Hamburguesa con doble carne y queso', 25000.00, 1),
    (2, 'Gaseosa', 'Bebida gaseosa personal', 5000.00, 1),
    (2, 'Limonada', 'Limonada natural', 7000.00, 1),
    (3, 'Papas Fritas', 'Porcion de papas fritas', 8000.00, 1);

INSERT INTO INGREDIENTE
    (nombre, unidad_medida, stock_actual, stock_minimo)
VALUES
    ('Pan de hamburguesa', 'UNIDAD', 40, 10),
    ('Carne de res', 'GRAMO', 6000, 1500),
    ('Queso', 'TAJADA', 60, 15),
    ('Tomate', 'GRAMO', 3000, 500),
    ('Lechuga', 'GRAMO', 2000, 300),
    ('Papa', 'GRAMO', 10000, 2000),
    ('Limon', 'UNIDAD', 30, 10),
    ('Azucar', 'GRAMO', 2000, 500),
    ('Gaseosa personal', 'UNIDAD', 8, 10);

-- Receta de cada producto: cantidad de ingrediente para preparar una unidad
INSERT INTO PRODUCTO_INGREDIENTE
    (id_producto, id_ingrediente, cantidad)
VALUES
    (1, 1, 1), (1, 2, 150), (1, 3, 1), (1, 4, 30), (1, 5, 20),
    (2, 1, 1), (2, 2, 300), (2, 3, 2), (2, 4, 30), (2, 5, 20),
    (3, 9, 1),
    (4, 7, 2), (4, 8, 30),
    (5, 6, 250);

INSERT INTO PEDIDO
    (id_cliente, id_usuario, fecha, estado, total)
VALUES
    (1, 1, GETDATE(), 'PENDIENTE', 31000.00),
    (2, 2, GETDATE(), 'PREPARANDO', 30000.00),
    (3, 2, GETDATE(), 'ENTREGADO', 33000.00);

INSERT INTO DETALLE_PEDIDO
    (id_pedido, id_producto, cantidad, precio_unitario, subtotal)
VALUES
    (1, 1, 1, 18000.00, 18000.00),
    (1, 3, 1, 5000.00, 5000.00),
    (1, 5, 1, 8000.00, 8000.00),

    (2, 2, 1, 25000.00, 25000.00),
    (2, 3, 1, 5000.00, 5000.00),

    (3, 1, 1, 18000.00, 18000.00),
    (3, 4, 1, 7000.00, 7000.00),
    (3, 5, 1, 8000.00, 8000.00);

INSERT INTO PAGO
    (id_pedido, fecha_pago, monto, metodo_pago, estado)
VALUES
    (1, GETDATE(), 31000.00, 'EFECTIVO', 'PAGADO'),
    (3, GETDATE(), 33000.00, 'TARJETA', 'PAGADO');

INSERT INTO FACTURA
    (id_pedido, fecha, total)
VALUES
    (1, GETDATE(), 31000.00),
    (3, GETDATE(), 33000.00);

INSERT INTO PREDICCION_DEMANDA
    (id_producto, fecha, cantidad_estimada)
VALUES
    (1, DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), 25),
    (2, DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), 12),
    (3, DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), 30),
    (4, DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), 15),
    (5, DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), 28);

INSERT INTO ALERTA_REABASTECIMIENTO
    (id_ingrediente, fecha, estado)
VALUES
    (9, GETDATE(), 'PENDIENTE');
GO

--- Consulta de prueba

SELECT
    P.id_pedido,
    C.nombre + ' ' + C.apellido AS cliente,
    U.nombre + ' ' + U.apellido AS usuario,
    P.fecha,
    P.estado,
    P.total
FROM PEDIDO P
INNER JOIN CLIENTE C
    ON P.id_cliente = C.id_cliente
INNER JOIN USUARIO U
    ON P.id_usuario = U.id_usuario;

SELECT
    DP.id_pedido,
    PR.nombre AS producto,
    DP.cantidad,
    DP.precio_unitario,
    DP.subtotal
FROM DETALLE_PEDIDO DP
INNER JOIN PRODUCTO PR
    ON DP.id_producto = PR.id_producto
ORDER BY DP.id_pedido;

SELECT
	PR.id_producto,
	PR.nombre AS producto,
	C.nombre AS categoria,
	PR.precio,
	PR.disponible
FROM PRODUCTO PR
INNER JOIN CATEGORIA C
	ON PR.id_categoria = C.id_categoria
ORDER BY PR.nombre;

-- Receta de cada producto
SELECT
	PR.nombre AS producto,
	I.nombre AS ingrediente,
	PI.cantidad,
	I.unidad_medida
FROM PRODUCTO_INGREDIENTE PI
INNER JOIN PRODUCTO PR
	ON PI.id_producto = PR.id_producto
INNER JOIN INGREDIENTE I
	ON PI.id_ingrediente = I.id_ingrediente
ORDER BY PR.nombre, I.nombre;

-- Ingredientes con existencias en o por debajo del minimo
SELECT
	I.id_ingrediente,
	I.nombre AS ingrediente,
	I.stock_actual,
	I.stock_minimo,
	I.unidad_medida
FROM INGREDIENTE I
WHERE I.stock_actual <= I.stock_minimo;

-- Pedidos facturados con su pago
SELECT
	F.id_factura,
	F.id_pedido,
	F.fecha,
	F.total,
	PA.metodo_pago,
	PA.estado AS estado_pago
FROM FACTURA F
INNER JOIN PAGO PA
	ON F.id_pedido = PA.id_pedido;

-- Alertas de reabastecimiento pendientes
SELECT
	A.id_alerta,
	I.nombre AS ingrediente,
	I.stock_actual,
	I.stock_minimo,
	A.fecha,
	A.estado
FROM ALERTA_REABASTECIMIENTO A
INNER JOIN INGREDIENTE I
	ON A.id_ingrediente = I.id_ingrediente
WHERE A.estado = 'PENDIENTE';

-- Prediccion de demanda por producto
SELECT
	PD.fecha,
	PR.nombre AS producto,
	PD.cantidad_estimada
FROM PREDICCION_DEMANDA PD
INNER JOIN PRODUCTO PR
	ON PD.id_producto = PR.id_producto
ORDER BY PD.fecha, PR.nombre;
