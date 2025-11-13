using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.BusinessObjects.SolicitudUsuario
{
    public class EnfermedadExamen
    {
        public int idEnfermedad {  get; set; }
        public string Enfermedad { get; set; }
        public Guid? idExamen { get; set; }
        public string Examen { get; set; }
        public int idTipo { get; set; }
        public EnfermedadExamen()
        {
            idEnfermedad = 0;
            Enfermedad = string.Empty;
            idExamen = Guid.Empty;
            Examen = string.Empty;
            idTipo = 0;
        }
    }
    
}
