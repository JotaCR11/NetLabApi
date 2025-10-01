using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Entities
{
    [TableName("Usuario")]
    [PrimaryKey("idUsuario", AutoIncrement = true)]
    public class User
    {
        public int IDUSUARIO { get; set; }
        public string LOGIN { get; set; }
        public int IDTIPOUSUARIO { get; set; }
        public int IDPROFESION { get; set; }
        public string DOCUMENTOIDENTIDAD { get; set; }
        public string APELLIDOPATERNO { get; set; }
        public string APELLIDOMATERNO { get; set; }
        public string NOMBRES { get; set; }
        public string INICIALES { get; set; }
        public string CODIGOCOLEGIO { get; set; }
        public string RNE { get; set; }
        public string CARGO { get; set; }
        public string TELEFONOCONTACTO { get; set; }
        public string CORREO { get; set; }
        public byte[] CONTRASENIA { get; set; }
        public byte[] FIRMADIGITAL { get; set; }
        public DateTime FECHAINGRESO { get; set; }
        public int TIEMPOCADUCIDAD { get; set; }
        public DateTime FECHAULTIMOACCESO { get; set; }
        public DateTime FECHACADUCIDAD { get; set; }
        public int ESTATUS { get; set; }
        public int ESTADO { get; set; }
        public DateTime FECHAREGISTRO { get; set; }
        public int IDUSUARIOREGISTRO { get; set; }
        public DateTime FECHAEDICION { get; set; }
        public int IDUSUARIOEDICION { get; set; }
        public int IDCOMPONENTE { get; set; }
        public int IDTIPOACCESO { get; set; }
        public int IDNIVELAPROBACION { get; set; }
        public DateTime FECHARENOVACION { get; set; }
        public DateTime FECHAINACTIVACION { get; set; }
        public int MOTIVOINACTIVACION { get; set; }
        public string OTROMOTIVOINACTIVACION { get; set; }
        public int USUARIOINACTIVACION { get; set; }
    }

    [TableName("AreaProcesamiento")]
    [PrimaryKey("idAreaProcesamiento", AutoIncrement = true)]
    public class AreaProcesamiento
    {
        public int IdAreaProcesamiento { get; set; }
        public string Nombre { get; set; }
    }
}
