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
}
