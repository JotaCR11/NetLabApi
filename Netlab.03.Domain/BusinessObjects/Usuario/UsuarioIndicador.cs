using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.BusinessObjects.Usuario
{
    public class UsuarioIndicador
    {
        public int TotalActivos { get; set; }
        public int TotalCaducados { get; set; }
        public int TotalInactivos { get; set; }
        public int TotalPendientes {  get; set; }

        public UsuarioIndicador()
        {
            TotalActivos = 0;
            TotalCaducados = 0;
            TotalInactivos = 0;
            TotalPendientes = 0;
        }
    }

    public class IndicadorAtencionSolicitudUsuario
    {
        public string MES { get; set; }
        public int SOLICITUDES_ATENDIDAS { get; set; }
        public int SOLICITUDES_RECHAZADAS { get; set; }
        public int SOLICITUDES_APROBADAS { get; set; }
        public List<UsuarioAtencionOutput> DATA { get; set; }
        public IndicadorAtencionSolicitudUsuario()
        {
            MES = string.Empty;
            SOLICITUDES_ATENDIDAS = 0;
            SOLICITUDES_RECHAZADAS = 0;
            SOLICITUDES_APROBADAS = 0;
            DATA = new List<UsuarioAtencionOutput>();
        }
    }
}
