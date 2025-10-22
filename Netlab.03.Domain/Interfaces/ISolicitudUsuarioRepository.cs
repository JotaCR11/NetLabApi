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
        Task<int> RegistraCodigoValidacionCorreo(SolicitudUsuarioCorreoValidacion solicitudUsuarioCorreoValidacion);
        Task<SolicitudUsuarioCorreoValidacion> ObtenerDatosValidacionCorreo(string documentoIdentidad, string email, string codigo);
        Task<int> ActualizaDatoCodigoValidacion(SolicitudUsuarioCorreoValidacion solicitudUsuarioCorreoValidacion);
        Task<List<Enfermedad>> ListaEnfermedad(string nombre);

        Task<int> RegistrarSolicitud(SolicitudUsuario solicitudUsuario);
        Task<int> RegistrarSolicitudRol(SolicitudUsuarioRol solicitudUsuarioRol);
        Task<int> RegistrarSolicitudRolExamen(SolicitudUsuarioRolExamen solicitudUsuarioRolExamen);
        Task<int> RegistroFormularioPDF(int IdSolicitud, byte[] file);


        Task<List<SoliciudUsuarioExamen>> ListaExamenPorEnfermedad(int IdEnfermedad, string nombre);
    }
}
