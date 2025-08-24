using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Entities
{
    public class General
    {
        public int Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdUsuarioRegistro { get; set; }
        public DateTime FechaEdicion { get; set; }
        public int IdUsuarioEdicion { get; set; }
        public Usuario UsuarioRegistro { get; set; }

        public string EstadoDesc => Estado == 1 ? "Activo" : "Inactivo";

        public bool EstadoCheck
        {
            get { return Estado == 1; }
            set
            {
                Estado = value ? 1 : 0;
            }
        }
    }
}
