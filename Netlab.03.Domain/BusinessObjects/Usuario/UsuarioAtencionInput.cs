using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.BusinessObjects.Usuario
{
    public class UsuarioAtencionInput
    {
        public string texto {  get; set; }
        public int estado { get; set; }
        public string ordenamiento { get; set; }
        public int tamnaño { get; set; }
        public int pagina { get; set; }
        public int total { get; set; }

        public UsuarioAtencionInput()
        {
            texto = string.Empty;
            estado = 0;
            ordenamiento = string.Empty;
            tamnaño = 0;
            pagina = 0;
            total = 0;
        }
    }

    public class UsuarioAtencionOutput
    {
        public int IdSolicitudUsuario { get; set; }
        public string FechaSolicitud { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombre {  get; set; }
        public string EESS {  get; set; }
        public string Estado { get; set; }
        public string TipoSolicitud { get; set; }
        public string Observaciones {  get; set; }

        public UsuarioAtencionOutput()
        {
            IdSolicitudUsuario = 0;
            FechaSolicitud = string.Empty;
            NumeroDocumento = string.Empty;
            Nombre = string.Empty;
            EESS = string.Empty;   
            Estado = string.Empty;
            TipoSolicitud = string.Empty;
            Observaciones = string.Empty;
        }
    }
}
