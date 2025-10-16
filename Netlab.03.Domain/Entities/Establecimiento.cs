using NPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Entities
{
    [TableName("Establecimiento")]
    [PrimaryKey("IDESTABLECIMIENTO")]
    public class Establecimiento
    {
        public int IDESTABLECIMIENTO { get; set; }
        public int CODIGOINSTITUCION { get; set; }
        public string CODIGOUNICO { get; set; }
        public string NOMBRE { get; set; }
        public string CLASIFICACION { get; set; }
        public string UBIGEO { get; set; }
        public string DIRECCION { get; set; }
        public string IDDISA { get; set; }
        public string IDRED { get; set; }
        public string IDMICRORED { get; set; }
        public int IDCATEGORIA { get; set; }
        public string LATITUD { get; set; }
        public string LONGITUD { get; set; }
        public int TIPO { get; set; }
        public string CORREOELECTRONICO { get; set; }
        public string WEBSITE { get; set; }
        public byte[] LOGOREG { get; set; }
        public byte[] LOGO { get; set; }
        public byte[] SELLO { get; set; }
        public int ESTADO { get; set; }
        public DateTime FECHAREGISTRO { get; set; }
        public int IDUSUARIOREGISTRO { get; set; }
        public DateTime FECHAEDICION { get; set; }
        public int IDUSUARIOEDICION { get; set; }
        public int IDLABORATORIO { get; set; }
        public string RUC { get; set; }
        public string NOMBREANTERIOR { get; set; }
        public DateTime FECHAVIGENCIAFIN { get; set; }
        public byte[] LOGOREGIONALANTERIOR { get; set; }
        public DateTime LOGOREGIONALFECHAVIGENCIAFIN { get; set; }
        public byte[] LOGOANTERIOR { get; set; }
        public DateTime LOGOFECHAVIGENCIAFIN { get; set; }
        public int IDJURISDICCION { get; set; }
        public int PEEC { get; set; }
        public byte[] SELLOANTERIOR { get; set; }
        public DateTime SELLOFECHAVIGENCIAFIN { get; set; }
        public string CODIGOEESS { get; set; }
        public int IDUNIDAD { get; set; }
        public string IDHISDISA { get; set; }
        public DateTime FECHAVIGENCIAHISDISA { get; set; }
        public string IDHISRED { get; set; }
        public DateTime FECHAVIGENCIAHISRED { get; set; }
        public string IDHISMICRORED { get; set; }
        public DateTime FECHAVIGENCIAHISMICRORED { get; set; }
        public string DIRECCIONANTERIOR { get; set; }
        public DateTime FECHAVIGENCIADDIRECCION { get; set; }

    }

    public class Ubigeo : General
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
    }

    public class EstablecimientoResponse
    {
        public int CODIGOINSTITUCION { get; set; }
        public string NOMBREINSTITUCION { get; set; }
        public string IDDISA { get; set; }
        public string NOMBREDISA { get; set; }
        public string IDRED {  get; set; }
        public string NOMBRERED { get; set; }
        public string IDMICRORED { get; set; }
        public string NOMBREMICRORED { get; set; }
        public int IDESTABLECIMIENTO { get; set; }
        public string CODIGOUNICO { get; set; }
        public string NOMBRE { get; set; }
    }

    [TableName("Institucion")]
    [PrimaryKey("CODIGOINSTITUCION")]
    public class Institucion : General
    {
        public int CODIGOINSTITUCION { get; set; }
        public string NOMBREINSTITUCION { get; set; }
        public string DESCRIPCION { get; set; }
    }

    [TableName("Disa")]
    [PrimaryKey("IDDISA")]
    public class Disa : General
    {
        public string IDDISA { get; set; }
        public string NOMBREDISA { get; set; }
    }

    [TableName("Red")]
    [PrimaryKey("IDRED")]
    public class Red : General
    {
        public string IDDISA { get; set; }
        public string IDRED { get; set; }
        public string NOMBRERED { get; set; }
    }

    [TableName("MicroRed")]
    [PrimaryKey("IDMICRORED")]
    public class MicroRed : General
    {
        public string IDDISA { get; set; }
        public string IDRED { get; set; }
        public string IDMICRORED { get; set; }
        public string NOMBREMICRORED { get; set; }
    }
}
