using Netlab.Domain.BusinessObjects.SolicitudUsuario;
using Netlab.Domain.BusinessObjects.Usuario;
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
        Task<List<EnfermedadExamen>> ObtenerExamenesUsuario(int IdUsuario);
        Task<List<Menu>> ObtenerMenusUsuario(int IdUsuario);
        Task<List<User>> ObtenerUsuarioPorDocumentoIdentidad(string documentoIdentidad);
        Task<UsuarioIndicador?> ObtenerCantidadTotalUsuario();
        Task<List<UsuarioAtencionOutput>> ObtenerListaAtenciones(UsuarioAtencionInput input);
        Task<List<UsuarioUbigeoOutput>> ObtenerListaUbigeoUsuario();
        Task<List<UsuarioDetalleAtencionOutput>> ObtenerListaDetalleAtenciones(UsuarioAtencionInput input);
        Task<UsuarioAprobadoOutput> AprobarSolicitudUsuario(int IdSolicitudUsuario, int IdUsuarioAtencion);

    }
}
