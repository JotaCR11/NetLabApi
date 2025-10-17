using Netlab.Domain.DTOs;
using Netlab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByLoginAsync(AuthRequest request);
        Task<List<EstablecimientoPerfil>> ObtenerEstablecimientoUsuario(int IdUsuario);
        Task<List<Rol>> ObtenerRolesUsuario(int IdUsuario);
        Task<List<Examen>> ObtenerExamenesUsuario(int IdUsuario);
        Task<List<Menu>> ObtenerMenusUsuario(int IdUsuario);
        Task<List<User>> ObtenerUsuarioPorDocumentoIdentidad(string documentoIdentidad);





        //Task<List<string>> ObtenerRolesAsync(int idUsuario);
        //Task<List<User>> ObtenerUsuarios(User usuario);
        //Task<int> ExisteLogin(string login);
        //Task<int> RegistrarUsuario(User usurio);
        //Task EditarUsuario(User usurio);
        //Task<User> ObtenerUsuario(int IdUsuario);



    }
}
