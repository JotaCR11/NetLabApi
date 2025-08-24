
use Netlab_Nuevo
go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Usuario](
	[idUsuario] [int] IDENTITY(1,1) NOT NULL,
	[login] [varchar](50) NULL,
	[idTipoUsuario] [int] NULL,
	[idProfesion] [int] NULL,
	[documentoIdentidad] [varchar](9) NULL,
	[apellidoPaterno] [varchar](100) NULL,
	[apellidoMaterno] [varchar](100) NULL,
	[nombres] [varchar](100) NULL,
	[iniciales] [varchar](100) NULL,
	[codigoColegio] [varchar](15) NULL,
	[RNE] [varchar](10) NULL,
	[cargo] [varchar](200) NULL,
	[telefonoContacto] [varchar](20) NULL,
	[correo] [varchar](300) NULL,
	[contrasenia] [varbinary](256) NULL,
	[firmadigital] [varbinary](500) NULL,
	[fechaIngreso] [datetime] NULL,
	[tiempoCaducidad] [int] NULL,
	[fechaUltimoAcceso] [datetime] NULL,
	[fechaCaducidad] [datetime] NULL,
	[estatus] [int] NULL,
	[estado] [int] NOT NULL CONSTRAINT [DF_Usuario_estado]  DEFAULT ((1)),
	[fechaRegistro] [datetime] NOT NULL CONSTRAINT [DF_Usuario_fechaRegistro]  DEFAULT (getdate()),
	[idUsuarioRegistro] [int] NULL,
	[fechaEdicion] [datetime] NULL,
	[idUsuarioEdicion] [int] NULL,
	[idComponente] [int] NULL,
	[idTipoAcceso] [int] NULL,
	[idNivelAprobacion] [int] NULL,
	[fechaRenovacion] [datetime] NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[idUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rol](
	[idRol] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](500) NULL,
	[descripcion] [varchar](1000) NULL,
	[tipo] [int] NULL,
	[estado] [int] NOT NULL CONSTRAINT [DF_Rol_estado]  DEFAULT ((1)),
	[fechaRegistro] [datetime] NOT NULL CONSTRAINT [DF_Rol_fechaRegistro]  DEFAULT (getdate()),
	[idUsuarioRegistro] [int] NULL,
	[fechaEdicion] [datetime] NULL,
	[idUsuarioEdicion] [int] NULL,
	[idCategoriaUsuario] [int] NULL DEFAULT ((1)),
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[idRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioAreaProcesamiento](
	[idUsuario] [int] NOT NULL,
	[idAreaProcesamiento] [int] NOT NULL,
	[estado] [int] NULL CONSTRAINT [DF_UsuarioAreaProcesamiento_estado]  DEFAULT ((1)),
	[fechaRegistro] [datetime] NULL CONSTRAINT [DF_UsuarioAreaProcesamiento_fechaRegistro]  DEFAULT (getdate()),
	[idUsuarioRegistro] [int] NULL,
	[fechaEdicion] [datetime] NULL,
	[idUsuarioEdicion] [int] NULL,
 CONSTRAINT [PK_UsuarioAreaProcesamiento] PRIMARY KEY CLUSTERED 
(
	[idUsuario] ASC,
	[idAreaProcesamiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioEnfermedadExamen](
	[idUsuario] [int] NOT NULL,
	[idUsuarioEnfermedadExamen] [int] NOT NULL,
	[idEnfermedad] [int] NOT NULL,
	[idExamen] [uniqueidentifier] NOT NULL,
	[estado] [int] NULL,
	[fechaRegistro] [datetime] NULL,
	[idUsuarioRegistro] [int] NULL,
	[fechaEdicion] [datetime] NULL,
	[idUsuarioEdicion] [int] NULL,
	[idTipo] [int] NULL,
 CONSTRAINT [PK_UsuarioEnfermedadExamen] PRIMARY KEY CLUSTERED 
(
	[idUsuario] ASC,
	[idUsuarioEnfermedadExamen] ASC,
	[idEnfermedad] ASC,
	[idExamen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioEstablecimiento](
	[idUsuario] [int] NOT NULL,
	[codigoInstitucion] [int] NULL,
	[idDISA] [nchar](2) NULL,
	[idRed] [nchar](2) NULL,
	[idMicroRed] [nchar](2) NULL,
	[idEstablecimiento] [int] NULL,
	[estado] [int] NULL CONSTRAINT [DF_UsuarioEstablecimiento_estado]  DEFAULT ((1)),
	[fechaRegistro] [datetime] NULL CONSTRAINT [DF_UsuarioEstablecimiento_fechaRegistro]  DEFAULT (getdate()),
	[idUsuarioRegistro] [int] NULL,
	[fechaEdicion] [datetime] NULL,
	[idUsuarioEdicion] [int] NULL
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuarioRol](
	[idUsuario] [int] NOT NULL,
	[idRol] [int] NOT NULL,
	[estado] [int] NOT NULL CONSTRAINT [DF_UsuarioRol_estado]  DEFAULT ((1)),
	[fechaRegistro] [datetime] NOT NULL CONSTRAINT [DF_UsuarioRol_fechaRegistro]  DEFAULT (getdate()),
	[idUsuarioRegistro] [int] NULL,
	[fechaEdicion] [datetime] NULL,
	[idUsuarioEdicion] [int] NULL,
 CONSTRAINT [PK_UsuarioRol] PRIMARY KEY CLUSTERED 
(
	[idUsuario] ASC,
	[idRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Menu](
	[idMenu] [int] NOT NULL,
	[nombre] [varchar](300) NULL,
	[descripcion] [varchar](1000) NULL,
	[URL] [varchar](2000) NULL,
	[idMenuPadre] [int] NULL,
	[orden] [int] NULL,
	[estado] [int] NOT NULL CONSTRAINT [DF_Menu_estado]  DEFAULT ((1)),
	[fechaRegistro] [datetime] NOT NULL CONSTRAINT [DF_Menu_fechaRegistro]  DEFAULT (getdate()),
	[idUsuarioRegistro] [int] NULL,
	[fechaEdicion] [datetime] NULL,
	[idUsuarioEdicion] [int] NULL,
	[icon] [varchar](50) NULL,
 CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED 
(
	[idMenu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuRol](
	[idMenu] [int] NOT NULL,
	[idRol] [int] NOT NULL,
	[estado] [int] NOT NULL CONSTRAINT [DF_MenuRol_estado]  DEFAULT ((1)),
	[fechaRegistro] [datetime] NOT NULL CONSTRAINT [DF_MenuRol_fechaRegistro]  DEFAULT (getdate()),
	[idUsuarioRegistro] [int] NULL,
	[fechaEdicion] [datetime] NULL,
	[idUsuarioEdicion] [int] NULL,
 CONSTRAINT [PK_MenuRol] PRIMARY KEY CLUSTERED 
(
	[idMenu] ASC,
	[idRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE LogAcceso (
    IdLogAcceso INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    Ruta NVARCHAR(500),
    Metodo NVARCHAR(10),
    Fecha DATETIME DEFAULT GETDATE(),
    IpCliente NVARCHAR(100),
    EsExitoso BIT,
    Mensaje NVARCHAR(MAX),
	[Request] NVARCHAR(1000),
    [StackTrace] NVARCHAR(MAX),
	StatusCode INT
);

