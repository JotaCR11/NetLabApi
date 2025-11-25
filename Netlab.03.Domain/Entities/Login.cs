using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Entities
{
    public class Login
    {
        public string LOGIN {  get; set; }
        public string CONTRASENIA { get; set; }
    }

    public class LoginResponse
    {
        public string TOKEN { get; set; }
        public Usuario USUARIO { get; set; }
        public List<EstablecimientoPerfil> ESTABLECIMIENTOS { get; set; }
    }
}
