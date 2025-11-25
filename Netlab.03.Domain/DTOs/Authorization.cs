using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.DTOs
{
    public class AuthRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string? IPAddress { get; set; }
    }

    public class AuthResponse
    {
        public string Token { get; set; }
        public string NombreUsuario { get; set; }
        //public string[] Roles { get; set; }
        // agregar mas propiedades del usuario/rol si es necesario a futuro.
    }
}
