using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Entities
{
    public class SolicitudUsuarioResponse
    {
        public string ERROR { get; set; } = string.Empty;
        public bool ENVIOCORREO { get; set; } = false;
        public SolicitudUsuario SOLICITUDUSUARIO { get; set; } = new SolicitudUsuario();
    }

    [TableName("SolicitudUsuario")]
    [PrimaryKey("IdSolicitudUsuario", AutoIncrement = true)]
    public class SolicitudUsuario
    {
        public int IDSOLICITUDUSUARIO { get; set; }
        public int TIPOSOLICITUD { get; set; }
        public int IDESTABLECIMIENTO { get; set; }
        public int TIPODOCUMENTO { get; set; }
        public string NUMERODOCUMENTO { get; set; } = string.Empty;
        public string APELLIDOPATERNO { get; set; } = string.Empty;
        public string APELLIDOMATERNO { get; set; } = string.Empty;
        public string NOMBRE { get; set; } = string.Empty;
        public string CORREOELECTRONICO { get; set; } = string.Empty;
        public string CELULAR { get; set; } = string.Empty;
        public int IDCONDICIONLABORAL { get; set; }
        public int IDCARGO { get; set; }
        public int IDCOMPONENTE { get; set; }
        public int IDPROFESION { get; set; }
        public int NUMEROCOLEGIATURA { get; set; }
        public bool ORDENAEXAMEN { get; set; }
        public bool COMITEEXPERTO { get; set; }
        public string CODIGOSOLICITUD { get; set; } = string.Empty;
        public byte[]? ARCHIVO { get; set; } = new byte[0];
        public int? IDUSUARIOATENCION { get; set; }
        public DateTime? FECHAATENCION { get; set; }
        public int? ESTATUS { get; set; } 
        public int? ESTADO { get; set; } 
        public DateTime? FECHAREGISTRO { get; set; } 
        public int? IDUSUARIOEDICION { get; set; }
        public DateTime? FECHAEDICION { get; set; }
        public List<SolicitudUsuarioRol> LISTASOLICITUDUSUARIOROL { get; set; } = new List<SolicitudUsuarioRol>();
    }

    [TableName("SolicitudUsuarioRol")]
    [PrimaryKey("IdSolicitudUsuarioRol", AutoIncrement = true)]
    public class SolicitudUsuarioRol
    {
        public int IDSOLICITUDUSUARIOROL { get; set; }
        public int IDSOLICITUDUSUARIO { get; set; }
        public int IDROL { get; set; }
        public List<SolicitudUsuarioRolExamen> LISTASOLICITUDUSUARIOROLEXAMEN { get; set; } = new List<SolicitudUsuarioRolExamen>();
        public int? ESTADO { get; set; }
        public DateTime? FECHAREGISTRO { get; set; }
        public int? IDUSUARIOEDICION { get; set; }
        public DateTime? FECHAEDICION { get; set; }
    }

    [TableName("SolicitudUsuarioRolExamen")]
    [PrimaryKey("IdSolicitudUsuarioRol", AutoIncrement = false)]
    public class SolicitudUsuarioRolExamen
    {
        public int IDSOLICITUDUSUARIOROL { get; set; }
        public int IDENFERMEDAD {  get; set; }
        public Guid? IDEXAMEN { get; set; } = Guid.Empty;
        public int? ESTADO { get; set; }
        public DateTime? FECHAREGISTRO { get; set; }
        public int? IDUSUARIOEDICION { get; set; }
        public DateTime? FECHAEDICION { get; set; }
    }

    public class PerfilUsuarioResponse
    {
        public User USUARIO {  get; set; } = new User();
        public List<Rol> ROL { get; set; } = new List<Rol>();
        public List<Examen> EXAMEN { get; set; } = new List<Examen>();
    }

    public class EstablecimientoCSV
    {
        public string INSTITUCION { get; set; } = string.Empty;
        public string COD_IPRESS { get; set; } = string.Empty;
        public string NOMBRE { get; set; } = string.Empty;
        public string CLASIFICACION { get; set; } = string.Empty;
        public string TIPO_ESTABLECIMIENTO { get; set; } = string.Empty;
        public string DEPARTAMENTO { get; set; } = string.Empty;
        public string PROVINCIA { get; set; } = string.Empty;
        public string DISTRITO { get; set; } = string.Empty;
        public string UBIGEO { get; set; } = string.Empty;
        public string DIRECCION { get; set; } = string.Empty;
        public string CO_DISA { get; set; } = string.Empty;
        public string COD_RED { get; set; } = string.Empty;
        public string COD_MICRORRED { get; set; } = string.Empty;
        public string DISA { get; set; } = string.Empty;
        public string RED { get; set; } = string.Empty;
        public string MICRORED { get; set; } = string.Empty;
        public string COD_UE { get; set; } = string.Empty;
        public string UNIDAD_EJECUTORA { get; set; } = string.Empty;
        public string CATEGORIA { get; set; } = string.Empty;
        public string TELEFONO { get; set; } = string.Empty;
        public string HORARIO { get; set; } = string.Empty;
        public string INICIO_ACTIVIDAD { get; set; } = string.Empty;
        public string ESTADO { get; set; } = string.Empty;
        public string SITUACION { get; set; } = string.Empty;
        public string CONDICION { get; set; } = string.Empty;
        public string NORTE { get; set; } = string.Empty;
        public string ESTE { get; set; } = string.Empty;
        public string IMAGEN_1 { get; set; } = string.Empty;
        public string FE_ACT_IMAGEN_1 { get; set; } = string.Empty;
        public string IMAGEN_2 { get; set; } = string.Empty;
        public string FE_ACT_IMAGEN_2 { get; set; } = string.Empty;
        public string IMAGEN_3 { get; set; } = string.Empty;
        public string FE_ACT_IMAGEN_3 { get; set; } = string.Empty;
    }

    public class SolicitudUsuarioCorreoValidacion
    {
        public int Id { get; set; }
        public string DocumentoIdentidad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public DateTime? FechaGeneracion { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public bool Usado { get; set; }
        public DateTime? FechaUso { get; set; }
    }

    public class Enfermedad
    {
        public int IdEnfermedad { get; set; }
        public string snomed { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string patogeno { get; set; } = string.Empty;
        public int estado { get; set; }
        public DateTime? fechaRegistro { get; set; }
        public int idUsuarioRegistro { get; set; }
        public DateTime? fechaEdicion { get; set; }
        public int idUsuarioEdicion { get; set; }
    }

    public class SoliciudUsuarioExamen
    {
        public Guid idExamen { get; set; }
        public string nombre { get; set; } = string.Empty;   
        public int ipruebarapida { get; set; }
        public int idExamenAgrupado { get; set; }
        public int estado { get; set; }
    }
    
    public class ArchivoInput
    {
        public int IdSolicitudUsuario { get; set; }
        public byte[] archivo { get; set; } = new byte[0];
        public bool upload {  get; set; } = false;
    }
}
