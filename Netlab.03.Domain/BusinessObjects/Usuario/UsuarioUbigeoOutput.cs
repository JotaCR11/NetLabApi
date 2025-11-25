using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.BusinessObjects.Usuario
{
    public class UsuarioUbigeoOutput
    {
        public int IdUsuario {  get; set; }
        public int IdEstablecimiento { get; set; }
        public string nombre { get; set; }
        public string ubigeo { get; set; }
        public UsuarioUbigeoOutput()
        {
            IdUsuario = 0;
            IdEstablecimiento = 0;
            nombre = string.Empty;
            ubigeo = string.Empty;
        }
    }
}
