using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Entities
{
    public class ReniecResponse
    {
        public string Dni { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string ApellidoMaterno { get; set; } = string.Empty;
    }

    public class ReniecServiceCredenciales
    {
        public string Url { get; set; } = string.Empty;
        public string app { get; set; } = string.Empty;
        public string usuario { get; set; } = string.Empty;
        public string clave { get; set; } = string.Empty;
    }
}
