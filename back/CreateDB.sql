IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AA1')
BEGIN
    CREATE DATABASE AA1;
END
GO

USE AA1;
GO

-- 1. Eliminar todas las tablas en orden inverso a su creación para no romper las Foreign Keys
DROP TABLE IF EXISTS MANTENIMIENTOS;
DROP TABLE IF EXISTS MATERIALES;
DROP TABLE IF EXISTS RESERVAS;
DROP TABLE IF EXISTS PISTAS;
DROP TABLE IF EXISTS USUARIOS;
GO

-- 2. Recrear las tablas con IDENTITY
CREATE TABLE USUARIOS (
    idUsuario INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50),
    apellido VARCHAR(50),
    telefono INT,
    direccion VARCHAR(100),
    fechaNac DATE
);
GO

CREATE TABLE PISTAS (
    idPista INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50),
    tipo VARCHAR(30),
    direccion VARCHAR(255),
    activa BIT,
    precioHora INT
);
GO

CREATE TABLE RESERVAS (
    idReserva INT PRIMARY KEY IDENTITY(1,1),
    idUsuario INT,
    idPista INT,
    fecha DATE,
    horas INT,
    precio INT,
    FOREIGN KEY (idUsuario) REFERENCES USUARIOS(idUsuario),
    FOREIGN KEY (idPista) REFERENCES PISTAS(idPista)
);
GO

CREATE TABLE MATERIALES (
    idMaterial INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50),
    cantidad INT,
    disponibilidad BIT,
    idPista INT,
    fechaAct DATE,
    FOREIGN KEY (idPista) REFERENCES PISTAS(idPista)
);
GO

CREATE TABLE MANTENIMIENTOS (
    idMantenimiento INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50),
    tlfn INT,
    cif INT,
    idPista INT,
    correo VARCHAR(100),
    FOREIGN KEY (idPista) REFERENCES PISTAS(idPista)
);
GO

-- 3. Insertar datos de ejemplo
INSERT INTO USUARIOS (nombre, apellido, telefono, direccion, fechaNac) 
VALUES ('Ana', 'García', 600123456, 'Calle Mayor 10', '1990-05-15'),
       ('Luis', 'Martínez', 600654321, 'Av. Goya 22', '1985-11-30');
GO

INSERT INTO PISTAS (nombre, tipo, direccion, activa, precioHora) 
VALUES ('Pista Central', 'Tenis', 'Pista de tierra batida', 1, 25),
       ('Pista Norte', 'Padel', 'Pista cubierta', 1, 20);
GO

INSERT INTO RESERVAS (idUsuario, idPista, fecha, horas, precio) 
VALUES (2, 1, '2025-11-20', 1, 20),
       (1, 2, '2025-11-21', 3, 28),
       (1, 2, '2025-12-01', 1, 18);
GO

INSERT INTO MATERIALES (nombre, cantidad, disponibilidad, idPista, fechaAct) 
VALUES ('Raquetas', 10, 1, 1, '2025-01-20'),
       ('Pelotas', 30, 1, 2, '2023-05-05');
GO

INSERT INTO MANTENIMIENTOS (nombre, tlfn, cif, idPista, correo) 
VALUES ('Revisión red', 152847563, 258, 1, 'mantenimiento@club.com'),
       ('Cambio focos', 611259566, 364, 2, 'soporte@club.com');
GO