using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.BusinessObjects.SolicitudUsuario
{
    public class EstadoSolicitud
    {
        public int IdSolicitudUsuario { get; set; }
        public string usuarioSolicitante { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime fechaCargaArchivo { get; set; }
        public Boolean ArchivoPendiente { get; set; }
        public Boolean SolicitudPendiente { get; set; }
        public Boolean SolicitudAprobada { get; set; }
        public Boolean SolicitudObservada { get; set; }
        public string UsuarioAtencion { get; set; }
        public string FechaAtencion { get; set; }
        public string Observacion { get; set; }
        public string Estado {  get; set; }

        public EstadoSolicitud()
        {
            IdSolicitudUsuario = 0;
            usuarioSolicitante = string.Empty;
            FechaRegistro = DateTime.MinValue;
            fechaCargaArchivo = DateTime.MinValue;
            ArchivoPendiente = false;
            SolicitudPendiente = false;
            SolicitudAprobada = false;
            SolicitudObservada = false;
            UsuarioAtencion = string.Empty;
            FechaAtencion = string.Empty;
            Observacion = string.Empty;
            Estado = string.Empty;
        }
    }
}
