using Azure;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Netlab.Domain.DTOs;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Helper;
using Netlab.Infrastructure.Database;
using Netlab.Infrastructure.ServicioReniec;
using System;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Netlab.Business.Services
{
    public interface ISolicitudUsuarioService
    {
        Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorCodigoUnico(string codigoUnico);
        Task<List<EstablecimientoResponse>> ObtenerEstablecimientoSinCodigoUnico();
        EstablecimientoCSV LeerCsv(string codigoUnico);
        Task<int> RegistrarEstablecimiento(EstablecimientoCSV establecimientocsv);
        Task<PerfilUsuarioResponse> ObtenerPerfilUsuario(string documentoIdentidad);
        Task<(bool Exito, string MensajeError)> EnviarCodigoAsync(string documentoIdentidad, string email, string nombre);
        Task<string> ValidarCodigoAsync(string documentoIdentidad, string email, string codigo);
        Task<List<Enfermedad>> ListaEnfermedad();
        Task<List<SoliciudUsuarioExamen>> ListaExamenPorEnfermedad(int IdEnfermedad);
        Task<SolicitudUsuarioResponse> RegistrarSolicitudUsuario(SolicitudUsuario solicitudUsuario);
        Task<ArchivoInput> RegistroFormularioPDF(ArchivoInput file);
        Task<SolicitudUsuario> ObtenerDatosSolicitudAsync(int idSolicitudUsuario);
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
            var response = await _solicitudRepo.ObtenerEstablecimientoPorCodigoUnico(codigoUnico);
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
                        response = await _solicitudRepo.ObtenerEstablecimientoPorCodigoUnico(codigoUnico);
                    }
                }
            }
            return response;
        }

        public async Task<List<EstablecimientoResponse>> ObtenerEstablecimientoSinCodigoUnico()
        {
            var response = await _solicitudRepo.ObtenerEstablecimientoSinCodigoUnico();
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

        public async Task<(bool Exito, string MensajeError)> EnviarCodigoAsync(string documentoIdentidad, string email, string nombre)
        {
            int registroCodigo = 0;
            bool exito = false;
            string error = string.Empty;
            var solicitudUsuarioCorreoValidacion = new SolicitudUsuarioCorreoValidacion()
            {
                DocumentoIdentidad = documentoIdentidad,
                Email = email,
                Codigo = new Random().Next(100000, 999999).ToString(),
                FechaGeneracion = DateTime.Now,
                FechaExpiracion = DateTime.Now.AddMinutes(10)
            };
            registroCodigo = await _solicitudRepo.RegistraCodigoValidacionCorreo(solicitudUsuarioCorreoValidacion);

            if (registroCodigo > 0)
            {
                (exito, error) = await _emailService.EnviarCorreoAsync("Codigo de verificacion",
                                                    await PlantillaCorreo("ConfirmacionCorreo.html", nombre, solicitudUsuarioCorreoValidacion.Codigo),
                                                    solicitudUsuarioCorreoValidacion.Email);
            }
            return (exito, error);
        }

        public async Task<string> ValidarCodigoAsync(string documentoIdentidad, string email, string codigo)
        {
            int validaCodigo = 0;
            string error = string.Empty;
            var response = await _solicitudRepo.ObtenerDatosValidacionCorreo(documentoIdentidad, email, codigo);
            if (response != null)
            {
                if (response.FechaExpiracion > DateTime.Now)
                {
                    var solicitudUsuarioCorreoValidacion = new SolicitudUsuarioCorreoValidacion()
                    {
                        Id = response.Id,
                        DocumentoIdentidad = response.DocumentoIdentidad,
                        Email = response.Email,
                        Codigo = response.Codigo,
                        FechaGeneracion = response.FechaGeneracion,
                        FechaExpiracion = response.FechaExpiracion,
                        Usado = true,
                        FechaUso = DateTime.Now
                    };
                    validaCodigo = await _solicitudRepo.ActualizaDatoCodigoValidacion(solicitudUsuarioCorreoValidacion);
                }
                else
                {
                    error ="Código Expirado.";
                }
            }
            else
            {
                error = "Código ingresado es inválido.";
            }
            return error;
        }

        public async Task<List<Enfermedad>> ListaEnfermedad()
        {
            var enfermedadNet2 = await _solicitudRepo.ListaEnfermedad();
            var enfermedadNet1 = await _solicitudRepo.ListaEnfermedadNetlab1();
            return enfermedadNet2.Union(enfermedadNet1).OrderBy(x => x.Nombre).ToList();
        }

        public async Task<List<SoliciudUsuarioExamen>> ListaExamenPorEnfermedad(int IdEnfermedad)
        {
            return await _solicitudRepo.ListaExamenPorEnfermedad(IdEnfermedad);
        }

        public async Task<SolicitudUsuarioResponse> RegistrarSolicitudUsuario(SolicitudUsuario solicitudUsuario)
        {
            bool exito = false;
            string error = string.Empty;
            
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (!string.IsNullOrEmpty(solicitudUsuario.NUMERODOCUMENTO))
                    {
                        solicitudUsuario.CODIGOSOLICITUD = "S" + solicitudUsuario.NUMERODOCUMENTO
                                                            + DateTime.Now.ToString("yyMMdd")
                                                            + DateTime.Now.ToString("hhmmss");

                        solicitudUsuario.ESTATUS = 1;
                        solicitudUsuario.ESTADO = 1;
                        solicitudUsuario.FECHAREGISTRO = DateTime.Now;
                        solicitudUsuario.IDSOLICITUDUSUARIO = await _solicitudRepo.RegistrarSolicitud(solicitudUsuario);
                    }

                    if (solicitudUsuario.IDSOLICITUDUSUARIO > 0)
                    {
                        for (int i = 0; i < solicitudUsuario.LISTASOLICITUDUSUARIOROL.Count; i++)
                        {
                            solicitudUsuario.LISTASOLICITUDUSUARIOROL[i].ESTADO = 1;
                            solicitudUsuario.LISTASOLICITUDUSUARIOROL[i].FECHAREGISTRO = DateTime.Now;
                            solicitudUsuario.LISTASOLICITUDUSUARIOROL[i].IDSOLICITUDUSUARIO = solicitudUsuario.IDSOLICITUDUSUARIO;
                            solicitudUsuario.LISTASOLICITUDUSUARIOROL[i].IDSOLICITUDUSUARIOROL = await _solicitudRepo.RegistrarSolicitudRol(solicitudUsuario.LISTASOLICITUDUSUARIOROL[i]);

                            if (solicitudUsuario.LISTASOLICITUDUSUARIOROL[i].IDSOLICITUDUSUARIOROL > 0)
                            {
                                int RegistroExamen = 0;
                                for (int j = 0; j < solicitudUsuario.LISTASOLICITUDUSUARIOROL[i].LISTASOLICITUDUSUARIOROLEXAMEN.Count; j++)
                                {
                                    solicitudUsuario.LISTASOLICITUDUSUARIOROL[i].LISTASOLICITUDUSUARIOROLEXAMEN[j].ESTADO = 1;
                                    solicitudUsuario.LISTASOLICITUDUSUARIOROL[i].LISTASOLICITUDUSUARIOROLEXAMEN[j].FECHAREGISTRO = DateTime.Now;
                                    solicitudUsuario.LISTASOLICITUDUSUARIOROL[i].LISTASOLICITUDUSUARIOROLEXAMEN[j].IDSOLICITUDUSUARIOROL = solicitudUsuario.LISTASOLICITUDUSUARIOROL[i].IDSOLICITUDUSUARIOROL;
                                    if (solicitudUsuario.LISTASOLICITUDUSUARIOROL[i].LISTASOLICITUDUSUARIOROLEXAMEN[j].IDENFERMEDAD > 0)
                                    {
                                        RegistroExamen = await _solicitudRepo.RegistrarSolicitudRolExamen(solicitudUsuario.LISTASOLICITUDUSUARIOROL[i].LISTASOLICITUDUSUARIOROLEXAMEN[j]);
                                        if (RegistroExamen == 0)
                                        {
                                            throw new Exception("Error al registrar examen.");
                                        }
                                    } 
                                }
                            }
                            else
                            {
                                throw new Exception("Error al registrar rol.");
                            }
                        }
                        scope.Complete();

                        (exito, error) = await _emailService.EnviarCorreoAsync(
                                           "Registro de solicitud Netlab",
                                           await PlantillaCorreo("RegistroSolicitud.html",solicitudUsuario.NOMBRE, solicitudUsuario.CODIGOSOLICITUD),
                                           solicitudUsuario.CORREOELECTRONICO);
                    }
                    else
                    {
                        throw new Exception("Error al registrar solicitud.");
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    solicitudUsuario.IDSOLICITUDUSUARIO = 0;
                }
            }
            
            return new SolicitudUsuarioResponse
            {
                ERROR = error,
                ENVIOCORREO = exito,
                SOLICITUDUSUARIO = solicitudUsuario
            };
        }
        
        public async Task<string> PlantillaCorreo(string archivo, string nombreSolicitante, string dato)
        {
            string html = await EmailService.CargarPlantillaAsync(
                archivo,
                new Dictionary<string, string>
                {
                    { "NOMBRE_USUARIO", nombreSolicitante.ToUpper() },
                    { "CODIGO", dato.ToUpper() }
                }
            );
            return html;
        }

        public async Task<ArchivoInput> RegistroFormularioPDF(ArchivoInput file)
        {
            int Id = await _solicitudRepo.RegistroFormularioPDF(file);
            if (Id > 0)
            {
                file.upload = true;
            }
            return file;
        }

        public async Task<SolicitudUsuario> ObtenerDatosSolicitudAsync(int idSolicitudUsuario)
        {
            var solicitud = new SolicitudUsuario();
            var roles = new List<SolicitudUsuarioRol>();
            var examenes = new List<SolicitudUsuarioRolExamen>();

            (solicitud,roles,examenes) = await _solicitudRepo.ObtenerDatosSolicitudAsync(idSolicitudUsuario);
            solicitud.LISTASOLICITUDUSUARIOROL = new List<SolicitudUsuarioRol>();
            solicitud.LISTASOLICITUDUSUARIOROL = roles;

            foreach (var rol in solicitud.LISTASOLICITUDUSUARIOROL)
            {
                rol.LISTASOLICITUDUSUARIOROLEXAMEN = examenes
                    .Where(e => e.IDSOLICITUDUSUARIOROL == rol.IDSOLICITUDUSUARIOROL)
                    .ToList();
            }
            return solicitud;
        }
    }
}

