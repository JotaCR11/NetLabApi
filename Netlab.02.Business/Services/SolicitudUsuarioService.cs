using Azure;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Helper;
using Netlab.Infrastructure.ServicioReniec;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Business.Services
{
    public interface ISolicitudUsuarioService
    {
        Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorCodigoUnico(string codigoUnico);
        EstablecimientoCSV LeerCsv(string codigoUnico);
        Task<int> RegistrarEstablecimiento(EstablecimientoCSV establecimientocsv);
        Task<PerfilUsuarioResponse> ObtenerPerfilUsuario(string documentoIdentidad);
        Task EnviarCodigoAsync(string documentoIdentidad, string email);
        Task ValidarCodigoAsync(string documentoIdentidad, string email, string codigo);



        Task<SolicitudUsuarioResponse> RegistrarSolicitudUsuario(SolicitudUsuario solicitudUsuario);
        Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorTexto(string texto);
        
        
    }

    public class SolicitudUsuarioService : ISolicitudUsuarioService
    {
        private readonly ISolicitudUsuarioRepository _solicitudRepo;
        private readonly IUsuarioRepository _userRepo;
        private readonly IReniecClient _reniecClient;
        private readonly IEmailService _emailService;

        public SolicitudUsuarioService(ISolicitudUsuarioRepository solicitudRepo, IUsuarioRepository userRepo, 
                                        IReniecClient reniecClient, IEmailService emailService)
        {
            _solicitudRepo = solicitudRepo;
            _reniecClient = reniecClient;
            _userRepo = userRepo;
            _emailService = emailService;
        }

        public async Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorCodigoUnico(string codigoUnico)
        {
            var response = await _solicitudRepo.ObtenerEstablecimientoPorTexto(codigoUnico);
            if (response.Count == 0)
            {
                string _codigo = codigoUnico.TrimStart('0');
                var responseCsv = LeerCsv(_codigo);
                if (responseCsv != null)
                {
                    responseCsv.COD_IPRESS = codigoUnico;
                    int IdEstablecimiento = await _solicitudRepo.RegistrarEstablecimiento(responseCsv);
                    if (IdEstablecimiento > 0)
                    {
                        response = await _solicitudRepo.ObtenerEstablecimientoPorTexto(codigoUnico);
                    }
                }
            }
            return response;
        }

        public EstablecimientoCSV LeerCsv(string codigoUnico)
        {
            var rutaBase = AppContext.BaseDirectory;
            var rutaProyecto = Directory.GetParent(rutaBase)!.Parent!.Parent!.Parent!.Parent!.FullName;

            var rutaCsv = Path.Combine(
                rutaProyecto,
                "Netlab.04.Infrastructure",
                "Resources",
                "Ipress",
                "RENIPRESS_2025_v6.csv"
            );

            if (!File.Exists(rutaCsv))
                throw new FileNotFoundException($"No se encontró el archivo CSV en la ruta: {rutaCsv}");

            // Configuración del lector CSV
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                MissingFieldFound = null,
                BadDataFound = null,
                Delimiter = ","
            };

            using var reader = new StreamReader(rutaCsv);
            using var csv = new CsvReader(reader, config);

            var registros = csv.GetRecords<EstablecimientoCSV>().ToList();

            // Buscar coincidencias por el código único
            var encontrados = registros
                .Where(x => x.COD_IPRESS.Equals(codigoUnico))
                .FirstOrDefault();

            return encontrados;
        }

        public async Task<int> RegistrarEstablecimiento(EstablecimientoCSV establecimientocsv)
        {
            return await _solicitudRepo.RegistrarEstablecimiento(establecimientocsv);
        }

        public async Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorTexto(string texto)
        {
            var response = await _solicitudRepo.ObtenerEstablecimientoPorTexto(texto);
            return response;
        }

        public async Task<PerfilUsuarioResponse> ObtenerPerfilUsuario(string documentoIdentidad)
        {
            var listaUsuario = new List<User>();
            var usuario = new User();
            listaUsuario = await _userRepo.ObtenerUsuarioPorDocumentoIdentidad(documentoIdentidad);
            var roles = new List<Rol>();
            var examenes = new List<Examen>();
            if (listaUsuario.Count() > 0)
            {
                usuario = listaUsuario.OrderByDescending(x => x.FECHAREGISTRO).FirstOrDefault();
                roles = await _userRepo.ObtenerRolesUsuario(usuario.IDUSUARIO);
                examenes = await _userRepo.ObtenerExamenesUsuario(usuario.IDUSUARIO);
            }
            else
            {
                var persona = await _reniecClient.ObtenerDatosPorDniAsync(documentoIdentidad);
                if (persona != null)
                {
                    usuario.DOCUMENTOIDENTIDAD = documentoIdentidad;
                    usuario.APELLIDOPATERNO = persona.ApellidoPaterno;
                    usuario.APELLIDOMATERNO = persona.ApellidoMaterno;
                    usuario.NOMBRES = persona.Nombres;
                }
            }   
            return new PerfilUsuarioResponse
                {
                    USUARIO = usuario,
                    ROL = roles,
                    EXAMEN = examenes
                };
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

        public async Task EnviarCodigoAsync(string documentoIdentidad, string email)
        {
            var solicitudUsuarioCorreoValidacion = new SolicitudUsuarioCorreoValidacion()
            {
                DocumentoIdentidad = documentoIdentidad,
                Email = email,
                Codigo = new Random().Next(100000, 999999).ToString(),
                FechaGeneracion = DateTime.Now,
                FechaExpiracion = DateTime.Now.AddMinutes(10)
            };
            await _solicitudRepo.RegistraCodigoValidacionCorreo(solicitudUsuarioCorreoValidacion);

            await _emailService.EnviarCorreoAsync("Codigo de verificacion", 
                                                    solicitudUsuarioCorreoValidacion.Codigo, 
                                                    solicitudUsuarioCorreoValidacion.Email);
        }

        public async Task ValidarCodigoAsync(string documentoIdentidad, string email, string codigo)
        {
            var response = await _solicitudRepo.ObtenerDatosValidacionCorreo(documentoIdentidad, email, codigo);
            if (response != null)
            {
                if (response.FechaExpiracion > DateTime.Now)
                {
                    var solicitudUsuarioCorreoValidacion = new SolicitudUsuarioCorreoValidacion()
                    {
                        DocumentoIdentidad = response.DocumentoIdentidad,
                        Email = response.Email,
                        Codigo = response.Codigo,
                        FechaGeneracion = response.FechaGeneracion,
                        FechaExpiracion = response.FechaExpiracion,
                        Usado = true,
                        FechaUso = DateTime.Now
                    };
                    await _solicitudRepo.ActualizaDatoCodigoValidacion(solicitudUsuarioCorreoValidacion);
                }
                else
                {
                    throw new InvalidOperationException("Código Expirado.");
                }

            }
        }
    }
}
