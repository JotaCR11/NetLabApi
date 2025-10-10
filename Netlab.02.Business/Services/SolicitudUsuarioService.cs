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
        Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorNombre(string nombre);
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
        public async Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorNombre(string nombre)
        {
            var establecimiento = await _solicitudRepo.ObtenerEstablecimientoPorNombre(nombre);
            return establecimiento.Select(e => new EstablecimientoResponse
            {
                IdEstablecimiento = e.IdEstablecimiento,
                Nombre = e.Nombre
            }).ToList();
        }
    }
}
