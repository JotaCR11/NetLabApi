using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.DTOs
{
    public class DatosCorreo
    {
        public string NombreDestino { get; set; }
        public string Codigo { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Archivo { get; set; }
        public DatosCorreo() 
        { 
            NombreDestino = string.Empty;
            Codigo = string.Empty;
            Login = string.Empty;
            Password = string.Empty;
            Archivo = string.Empty;
        }

    }
}
