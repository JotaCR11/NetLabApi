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
        public int RESPONSEID { get; set; }
        public string CODIGOSOLICITUD { get; set; }
    }

    [TableName("SolicitudUsuario")]
    [PrimaryKey("IdSolicitudUsuario", AutoIncrement = true)]
    public class SolicitudUsuario
    {
        public int IDSOLICITUDUSUARIO { get; set; }
        public int TIPOSOLICITUD { get; set; }
        public int IDESTABLECIMIENTO { get; set; }
        public int TIPODOCUMENTO { get; set; }
        public string NUMERODOCUMENTO { get; set; }
        public string APELLIDOPATERNO { get; set; }
        public string APELLIDOMATERNO { get; set; }
        public string NOMBRE { get; set; }
        public string CORREOELECTRONICO { get; set; }
        public string CELULAR { get; set; }
        public int IDCONDICIONLABORAL { get; set; }
        public int IDCARGO { get; set; }
        public int IDCOMPONENTE { get; set; }
        public int IDPROFESION { get; set; }
        public int NUMEROCOLEGIATURA { get; set; }
        public bool ORDENAEXAMEN { get; set; }
        public bool COMITEEXPERTO { get; set; }
        public string CODIGOSOLICITUD { get; set; }
        public byte[] ARCHIVO { get; set; }
        public int IDUSUARIOATENCION { get; set; }
        public DateTime FECHAATENCION { get; set; }
        public int ESTATUS { get; set; }
        public int ESTADO { get; set; }
        public DateTime FECHAREGISTRO { get; set; }
        public int IDUSUARIOEDICION { get; set; }
        public DateTime FECHAEDICION { get; set; }
        public List<SolicitudUsuarioRol> LISTASOLICITUDUSUARIOROL {  get; set; }
    }

    [TableName("SolicitudUsuarioRol")]
    [PrimaryKey("IdSolicitudUsuarioRol", AutoIncrement = true)]
    public class SolicitudUsuarioRol
    {
        public int IDSOLICITUDUSUARIOROL { get; set; }
        public int IDSOLICITUDUSUARIO { get; set; }
        public int IDROL { get; set; }
        public List<SolicitudUsuarioRolExamen> LISTASOLICITUDUSUARIOROLEXAMEN { get; set; }
        public int ESTADO { get; set; }
        public DateTime FECHAREGISTRO { get; set; }
        public int IDUSUARIOEDICION { get; set; }
        public DateTime FECHAEDICION { get; set; }
    }

    [TableName("SolicitudUsuarioRolExamen")]
    [PrimaryKey("IdSolicitudUsuarioRol", AutoIncrement = true)]
    public class SolicitudUsuarioRolExamen
    {
        public int IDSOLICITUDUSUARIOROL { get; set; }
        public Guid IdExamen { get; set; }
        public int ESTADO { get; set; }
        public DateTime FECHAREGISTRO { get; set; }
        public int IDUSUARIOEDICION { get; set; }
        public DateTime FECHAEDICION { get; set; }
    }

    public class PerfilUsuarioResponse
    {
        public User USUARIO {  get; set; }
        public List<_Rol> ROL { get; set; }
        public List<Examen> EXAMEN { get; set; }
    }

    [TableName("Rol")]
    [PrimaryKey("idRol", AutoIncrement = true)]
    public class _Rol
    {
        public int IDROL { get; set; }
        public string NOMBRE { get; set; }
        //public List<Examen> EXAMEN { get; set; }
    }

    public class EstablecimientoCSV
    {
        public string INSTITUCION { get; set; }
        public string COD_IPRESS { get; set; }
        public string NOMBRE { get; set; }
        public string CLASIFICACION { get; set; }
        public string TIPO_ESTABLECIMIENTO { get; set; }
        public string DEPARTAMENTO { get; set; }
        public string PROVINCIA { get; set; }
        public string DISTRITO { get; set; }
        public string UBIGEO { get; set; }
        public string DIRECCION { get; set; }
        public string CO_DISA { get; set; }
        public string COD_RED { get; set; }
        public string COD_MICRORRED { get; set; }
        public string DISA { get; set; }
        public string RED { get; set; }
        public string MICRORED { get; set; }
        public string COD_UE { get; set; }
        public string UNIDAD_EJECUTORA { get; set; }
        public string CATEGORIA { get; set; }
        public string TELEFONO { get; set; }
        public string HORARIO { get; set; }
        public string INICIO_ACTIVIDAD { get; set; }
        public string ESTADO { get; set; }
        public string SITUACION { get; set; }
        public string CONDICION { get; set; }
        public string NORTE { get; set; }
        public string ESTE { get; set; }
        public string IMAGEN_1 { get; set; }
        public string FE_ACT_IMAGEN_1 { get; set; }
        public string IMAGEN_2 { get; set; }
        public string FE_ACT_IMAGEN_2 { get; set; }
        public string IMAGEN_3 { get; set; }
        public string FE_ACT_IMAGEN_3 { get; set; }
    }
}
