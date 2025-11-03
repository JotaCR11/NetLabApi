using Netlab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.BusinessObjects.Usuario
{
    public class UsuarioPerfilOut
    {
        public User USUARIO { get; set; } = new User();
        public SolicitudUsuarioPerfil Perfil { get; set; } = new SolicitudUsuarioPerfil();
    }

}
