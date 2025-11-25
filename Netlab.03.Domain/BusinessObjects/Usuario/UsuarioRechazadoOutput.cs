using Netlab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.BusinessObjects.Usuario
{
    public class UsuarioRechazadoOutput
    {
        public string NombreUsuario { get; set; }
        public string CodigoSolicitud { get; set; }
        public string CorreoElectronico {  get; set; }
        public UsuarioRechazadoOutput()
        {
            NombreUsuario = string.Empty;
            CodigoSolicitud = string.Empty;
            CorreoElectronico = string.Empty;
        }
    }
}
