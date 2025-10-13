using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Business.Services
{
    public interface ISolicitudUsuarioService
    {
        Task<SolicitudUsuarioResponse> RegistrarSolicitudUsuario(SolicitudUsuario solicitudUsuario);
        Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorTexto(string texto);
        Task<PerfilUsuarioResponse> ObtenerPerfilUsuario(string documentoIdentidad);
    }

    public class SolicitudUsuarioService : ISolicitudUsuarioService
    {
        private readonly ISolicitudUsuarioRepository _solicitudRepo;
        public SolicitudUsuarioService(ISolicitudUsuarioRepository solicitudRepo)
        {
            _solicitudRepo = solicitudRepo;
        }

        public async Task<SolicitudUsuarioResponse> RegistrarSolicitudUsuario(SolicitudUsuario solicitudUsuario)
        {
            var solicitud = await _solicitudRepo.RegistrarSolicitudUsuario(solicitudUsuario);
            
            return new SolicitudUsuarioResponse
            {
                RESPONSEID = solicitud.IDSOLICITUDUSUARIO,
                CODIGOSOLICITUD = solicitud.CODIGOSOLICITUD
            };
        }
        public async Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorTexto(string texto)
        {
            var response = await _solicitudRepo.ObtenerEstablecimientoPorTexto(texto);
            return response;
        }

        public async Task<PerfilUsuarioResponse> ObtenerPerfilUsuario(string documentoIdentidad)
        {
            var response = await _solicitudRepo.ObtenerPerfilUsuario(documentoIdentidad);
            return response;
        }
    }
}
