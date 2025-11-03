using Netlab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.BusinessObjects.Usuario
{
    public class UsuarioPerfilInput
    {
        public int TIPOSOLICITUD { get; set; }
        public int IDESTABLECIMIENTO { get; set; }
        public int TIPODOCUMENTO { get; set; }
        public string NUMERODOCUMENTO { get; set; } = string.Empty;
        public string APELLIDOPATERNO { get; set; } = string.Empty;
        public string APELLIDOMATERNO { get; set; } = string.Empty;
        public string NOMBRE { get; set; } = string.Empty;
        public string CORREOELECTRONICO { get; set; } = string.Empty;
        public string CELULAR { get; set; } = string.Empty;
        public string CONDICIONLABORAL { get; set; } = string.Empty;
        public string CARGO { get; set; } = string.Empty;
        public int IDCOMPONENTE { get; set; }
        public int IDPROFESION { get; set; }
        public int NUMEROCOLEGIATURA { get; set; }
        public SolicitudUsuarioPerfil perfil { get; set; } = new SolicitudUsuarioPerfil();
    }
}
