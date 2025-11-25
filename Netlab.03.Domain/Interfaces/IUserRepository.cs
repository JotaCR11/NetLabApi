using Netlab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<Usuario>> ObtenerUsuarios(Usuario usuario);
        Task<int> ExisteLogin(string login);
        Task<string> RegistrarUsuario(Usuario usurio);
        Task EditarUsuario(Usuario usurio);
    }
}
