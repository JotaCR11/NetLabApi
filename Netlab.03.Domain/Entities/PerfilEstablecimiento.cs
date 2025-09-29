using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Entities
{
    public class PerfilEstablecimiento
    {
        public int idEstablecimiento {  get; set; }
        public List<Rol> rols { get; set; }
        public List<AreaProcesamiento> areaProcesamientos { get; set; }
        public List<Examen> examens {  get; set; }

    }
}
