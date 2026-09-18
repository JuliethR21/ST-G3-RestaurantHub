--CREACIÓN DE LA BASE DE DATOS

IF DB_ID('RestaurantHubDB') IS NULL
BEGIN
    CREATE DATABASE RestaurantHubDB;
END

USE RestaurantHubDB;

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
    estado VARCHAR(20) NOT NULL,

    CONSTRAINT FK_PRODUCTO_CATEGORIA
        FOREIGN KEY (id_categoria)
        REFERENCES CATEGORIA(id_categoria),

    CONSTRAINT CK_PRODUCTO_PRECIO
        CHECK (precio >= 0)
);

-- Creacion de la tabla "INVENTARIO"
CREATE TABLE INVENTARIO (
	id_inventario INT IDENTITY(1,1) PRIMARY KEY,
	id_producto INT NOT NULL UNIQUE,
	stock_actual INT NOT NULL,
	stock_minimo INT NOT NULL,
	fecha_actualizacion DATETIME NOT NULL,

	CONSTRAINT FK_INVENTARIO_PRODUCTO
		FOREIGN KEY (id_producto)
		REFERENCES PRODUCTO(id_producto),

	CONSTRAINT CK_INVENTARIO_STOCK_ACTUAL
		CHECK (stock_actual >= 0),

	CONSTRAINT CK_INVENTARIO_STOCK_MINIMO CHECK (stock_minimo >= 0)
);

-- Creacion de la tabla "PEDIDO"
CREATE TABLE PEDIDO (
    id_pedido INT IDENTITY(1,1) PRIMARY KEY,
    id_cliente INT NOT NULL,
    id_usuario INT NOT NULL,
    fecha DATETIME NOT NULL,
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
    fecha_pago DATETIME NOT NULL,
    monto DECIMAL(10,2) NOT NULL,
    metodo_pago VARCHAR(30) NOT NULL,
    estado VARCHAR(20) NOT NULL,

    CONSTRAINT FK_PAGO_PEDIDO
        FOREIGN KEY (id_pedido)
        REFERENCES PEDIDO(id_pedido),

    CONSTRAINT CK_PAGO_MONTO
        CHECK (monto >= 0)
);

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
    (id_categoria, nombre, descripcion, precio, estado)
VALUES
    (1, 'Hamburguesa Clasica', 'Hamburguesa con carne, queso, tomate y lechuga', 18000.00, 'DISPONIBLE'),
    (1, 'Hamburguesa Especial', 'Hamburguesa con doble carne y queso', 25000.00, 'DISPONIBLE'),
    (2, 'Gaseosa', 'Bebida gaseosa personal', 5000.00, 'DISPONIBLE'),
    (2, 'Limonada', 'Limonada natural', 7000.00, 'DISPONIBLE'),
    (3, 'Papas Fritas', 'Porcion de papas fritas', 8000.00, 'DISPONIBLE');

INSERT INTO INVENTARIO
	(id_producto, stock_actual, stock_minimo, fecha_actualizacion)
VALUES
	(1, 20, 5, GETDATE()),
	(2, 15, 5, GETDATE()),
	(3, 50, 10, GETDATE()),
	(4, 30, 8, GETDATE()),
	(5, 25, 5, GETDATE());

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
	PR.estado,
	I.stock_actual,
	I.stock_minimo,
	I.fecha_actualizacion
FROM PRODUCTO PR
INNER JOIN CATEGORIA C
	ON PR.id_categoria = C.id_categoria
INNER JOIN INVENTARIO I
	ON PR.id_producto = I.id_producto
ORDER BY PR.nombre;

SELECT
	PR.id_producto,
	PR.nombre AS producto,
	I.stock_actual,
	I.stock_minimo
FROM PRODUCTO PR
INNER JOIN INVENTARIO I
	ON PR.id_producto = I.id_producto
WHERE I.stock_actual <= I.stock_minimo;