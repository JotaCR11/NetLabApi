using Netlab.Domain.BusinessObjects.SolicitudUsuario;
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
        Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorCodigoUnico(string CodigoUnico);
        Task<List<EstablecimientoResponse>> ObtenerEstablecimientoSinCodigoUnico();
        Task<int> RegistrarEstablecimiento(EstablecimientoCSV establecimientocsv);
        Task<int> RegistraCodigoValidacionCorreo(SolicitudUsuarioCorreoValidacion solicitudUsuarioCorreoValidacion);
        Task<SolicitudUsuarioCorreoValidacion> ObtenerDatosValidacionCorreo(string documentoIdentidad, string email, string codigo);
        Task<int> ActualizaDatoCodigoValidacion(SolicitudUsuarioCorreoValidacion solicitudUsuarioCorreoValidacion);
        Task<List<Enfermedad>> ListaEnfermedad();
        Task<List<Enfermedad>> ListaEnfermedadNetlab1();
        Task<int> RegistrarSolicitud(SolicitudUsuario solicitudUsuario);
        Task<int> RegistrarSolicitudRol(SolicitudUsuarioRol solicitudUsuarioRol);
        Task<int> RegistrarSolicitudRolExamen(SolicitudUsuarioRolExamen solicitudUsuarioRolExamen);
        Task<List<SoliciudUsuarioExamen>> ListaExamenPorEnfermedad(int IdEnfermedad);
        Task<int> RegistroFormularioPDF(ArchivoInput file);
        Task<(SolicitudUsuario, List<SolicitudUsuarioRol>, List<SolicitudUsuarioRolExamen>)> ObtenerDatosSolicitudAsync(int idSolicitudUsuario);
        Task<EstadoSolicitud> EstadoSolicitudUsuario(string CodigoSolicitud);
        Task<int> DeleteRollback(int idSolicitudUsuario);
    }
}
