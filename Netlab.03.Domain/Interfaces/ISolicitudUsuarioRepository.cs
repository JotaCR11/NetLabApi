using Netlab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Interfaces
{
    public interface ISolicitudUsuarioRepository
    {
        Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorTexto(string texto);
        Task<int> RegistrarEstablecimiento(EstablecimientoCSV establecimientocsv);
        Task<SolicitudUsuario> RegistrarSolicitudUsuario(SolicitudUsuario solicitudUsuario); 
        Task<PerfilUsuarioResponse> ObtenerPerfilUsuario(string documentoIdentidad);
    }
}
