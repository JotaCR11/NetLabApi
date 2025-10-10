using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Entities
{
    public class Establecimiento : General
    {
        public int IdEstablecimiento { get; set; }
        public string CodigoInstitucion { get; set; }
        public string CodigoUnico { get; set; }
        public string Clasificacion { get; set; }
        public string Nombre { get; set; }
        public string Ubigeo { get; set; }
        public string Direccion { get; set; }
        public string CodigoDisa { get; set; }
        public string CodigoRed { get; set; }
        public string CodigoMicroRed { get; set; }
        public int CodigoCategoria { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Correoelectronico { get; set; }
        public Ubigeo eUbigeo { get; set; } //revisar con query
        public string Website { get; set; }
        public int IdLabIns { get; set; }
        public int IdCategoria { get; set; }
        public string Telefono { get; set; }
        public string NombreDisa { get; set; }
        //public string? Ubigeo { get; set; } //revisar con query
        public string LogoRegional { get; set; }
        public string Logo { get; set; }
        public string Sello { get; set; }
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
        public string NOMBRE { get; set; }
    }
}
