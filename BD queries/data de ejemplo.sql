-- Roles
INSERT INTO Rol (nombre, descripcion, tipo)
VALUES ('Administrador', 'Acceso completo', 1),
       ('Usuario', 'Acceso limitado', 2);

-- Menús
INSERT INTO Menu (idMenu, nombre, descripcion, URL, orden, icon)
VALUES (1, 'Dashboard', 'Panel principal', '/dashboard', 1, 'dashboard'),
       (2, 'Reportes', 'Vista de reportes', '/reportes', 2, 'bar-chart');

-- Relacionar Menú con Roles
INSERT INTO MenuRol (idMenu, idRol)
VALUES (1, 1), -- Dashboard -> Administrador
       (2, 1), -- Reportes -> Administrador
       (1, 2); -- Dashboard -> Usuario

-- Usuarios
INSERT INTO Usuario (login, contrasenia, nombres, apellidoPaterno, apellidoMaterno, correo)
VALUES 
('admin', CONVERT(varbinary(256), HASHBYTES('SHA2_256', 'admin123')), 'Admin', 'Principal', '', 'admin@demo.com'),
('usuario1', CONVERT(varbinary(256), HASHBYTES('SHA2_256', 'usuario123')), 'Usuario', 'Prueba', '', 'usuario1@demo.com');

-- Asignar Roles a Usuarios
INSERT INTO UsuarioRol (idUsuario, idRol)
VALUES 
(1, 1), -- admin → Administrador
(2, 2); -- usuario1 → Usuario
