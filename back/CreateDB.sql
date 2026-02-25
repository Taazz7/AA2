IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AA1')
BEGIN
    CREATE DATABASE AA1;
END

GO

USE AA1;

GO

-- 1. Eliminar tablas en orden para evitar errores de claves ajenas
DROP TABLE IF EXISTS AA1.dbo.MANTENIMIENTOS;
DROP TABLE IF EXISTS AA1.dbo.MATERIALES;
DROP TABLE IF EXISTS AA1.dbo.RESERVAS;
DROP TABLE IF EXISTS AA1.dbo.PISTAS;
DROP TABLE IF EXISTS AA1.dbo.USUARIOS;

GO

-- 2. Recrear las tablas
CREATE TABLE AA1.dbo.USUARIOS (
    idUsuario INT PRIMARY KEY IDENTITY(1,1),
    usuario VARCHAR(100) UNIQUE NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    telefono INT,
    contraseña VARCHAR(100),
    rol VARCHAR(20) NOT NULL
);

GO

CREATE TABLE AA1.dbo.PISTAS (
    idPista INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50),
    tipo VARCHAR(30),
    direccion VARCHAR(255),
    activa BIT,
    precioHora INT
);

GO

CREATE TABLE AA1.dbo.RESERVAS (
    idReserva INT PRIMARY KEY IDENTITY(1,1),
    idUsuario INT,
    idPista INT,
    fecha DATE,
    horas INT,
    precio INT,
    FOREIGN KEY (idUsuario) REFERENCES AA1.dbo.USUARIOS(idUsuario),
    FOREIGN KEY (idPista) REFERENCES AA1.dbo.PISTAS(idPista)
);

GO

CREATE TABLE AA1.dbo.MATERIALES (
    idMaterial INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50),
    cantidad INT,
    disponibilidad BIT,
    idPista INT,
    fechaAct DATE,
    FOREIGN KEY (idPista) REFERENCES AA1.dbo.PISTAS(idPista)
);

GO

CREATE TABLE AA1.dbo.MANTENIMIENTOS (
    idMantenimiento INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50),
    tlfn INT,
    cif INT,
    idPista INT,
    correo VARCHAR(100),
    FOREIGN KEY (idPista) REFERENCES AA1.dbo.PISTAS(idPista)
);

GO

-- 3. Insertar datos de ejemplo usando rutas absolutas
INSERT INTO AA1.dbo.USUARIOS (usuario, email, telefono, contraseña, rol) 
VALUES ('admin', 'admin@pistas.com', 600000000, 'admin123', 'admin'),
       ('Javy', 'admin2@pistas.com', 600700000, '1234', 'admin'),
       ('user', 'user@pistas.com', 611223344, 'user123', 'user');

GO

INSERT INTO AA1.dbo.PISTAS (nombre, tipo, direccion, activa, precioHora) 
VALUES ('Pista Central', 'Tenis', 'Pista de tierra batida', 1, 25),
       ('Pista Norte', 'Padel', 'Pista cubierta', 1, 20);

GO

INSERT INTO AA1.dbo.RESERVAS (idUsuario, idPista, fecha, horas, precio) 
VALUES (2, 1, '2025-11-20', 1, 20),
       (1, 2, '2025-11-21', 3, 28),
       (1, 2, '2025-12-01', 1, 18);

GO

INSERT INTO AA1.dbo.MATERIALES (nombre, cantidad, disponibilidad, idPista, fechaAct) 
VALUES ('Raquetas', 10, 1, 1, '2025-01-20'),
       ('Pelotas', 30, 1, 2, '2023-05-05');

GO

INSERT INTO AA1.dbo.MANTENIMIENTOS (nombre, tlfn, cif, idPista, correo) 
VALUES ('Revisión red', 152847563, 258, 1, 'mantenimiento@club.com'),
       ('Cambio focos', 611259566, 364, 2, 'soporte@club.com');

GO
