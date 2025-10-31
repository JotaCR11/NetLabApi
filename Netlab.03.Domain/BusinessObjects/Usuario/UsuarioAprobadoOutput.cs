using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.BusinessObjects.Usuario
{
    public class UsuarioAprobadoOutput
    {
        public int IdUsuario { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string NombreUsuario { get; set; }
        public string CorreoElectronico { get; set; }
        public int TipoSolicitud {  get; set; }
        public int Netlab {  get; set; }

        public UsuarioAprobadoOutput()
        {
            IdUsuario = 0;
            Login = string.Empty;
            Password = string.Empty;
            NombreUsuario = string.Empty;
            CorreoElectronico = string.Empty;
            TipoSolicitud = 0;
            Netlab = 0;
        }
    }
}
