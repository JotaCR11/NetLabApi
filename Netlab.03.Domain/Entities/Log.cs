using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Entities
{
    [PrimaryKey("IdLogAcceso", AutoIncrement = true)]
    public class LogAcceso
    {
        public int IdLogAcceso { get; set; }
        public int IdUsuario { get; set; }
        public string Ruta { get; set; }
        public string Metodo { get; set; }
        public DateTime Fecha { get; set; }
        public string IpCliente { get; set; }
        public bool EsExitoso { get; set; }
        public string Mensaje { get; set; }
        public string Request { get; set; }
        public string StackTrace { get; set; }
        public int StatusCode { get; set; }
    }

    public class LogError
    {
        public int IdLogError { get; set; }
        public int IdUsuario { get; set; }
        public string Endpoint { get; set; }
        public string Mensaje { get; set; }
        public string StacTrace { get; set; }
    }
}
