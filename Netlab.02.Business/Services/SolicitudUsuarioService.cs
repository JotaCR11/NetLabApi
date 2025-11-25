using Azure;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Netlab.Domain.BusinessObjects.SolicitudUsuario;
using Netlab.Domain.BusinessObjects.Usuario;
using Netlab.Domain.DTOs;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Helper;
using Netlab.Infrastructure.Database;
using Netlab.Infrastructure.ServicioReniec;
using NPoco;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Netlab.Business.Services
{
    public interface ISolicitudUsuarioService
    {
        Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorCodigoUnico(string codigoUnico);
        Task<List<EstablecimientoResponse>> ObtenerEstablecimientoSinCodigoUnico();
        EstablecimientoCSV LeerCsv(string codigoUnico);
        Task<int> RegistrarEstablecimiento(EstablecimientoCSV establecimientocsv);
        Task<UsuarioPerfilOut> ObtenerPerfilUsuario(string documentoIdentidad);
        Task<(bool Exito, string MensajeError)> EnviarCodigoAsync(string documentoIdentidad, string email, string nombre);
        Task<string> ValidarCodigoAsync(string documentoIdentidad, string email, string codigo);
        Task<List<Enfermedad>> ListaEnfermedad();
        Task<List<SoliciudUsuarioExamen>> ListaExamenPorEnfermedad(int IdEnfermedad);
        Task<SolicitudUsuarioResponse> RegistrarSolicitudUsuario(UsuarioPerfilInput solicitudUsuario);
        Task<ArchivoInput> RegistroFormularioPDF(ArchivoInput file);
        Task<SolicitudUsuario> ObtenerDatosSolicitudAsync(int idSolicitudUsuario);
        Task<EstadoSolicitud> EstadoSolicitudUsuario(string CodigoSolicitud);
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

        public async Task<UsuarioPerfilOut> ObtenerPerfilUsuario(string documentoIdentidad)
        {
            var listaUsuario = new List<User>();
            var usuario = new User();
            listaUsuario = await _userRepo.ObtenerUsuarioPorDocumentoIdentidad(documentoIdentidad);
            var roles = new List<Rol>();
            var examenes = new List<EnfermedadExamen>();
            var perfil = new SolicitudUsuarioPerfil();
            if (listaUsuario.Count() > 0)
            {
                usuario = listaUsuario.OrderByDescending(x => x.FECHAREGISTRO).FirstOrDefault();
                roles = await _userRepo.ObtenerRolesUsuario(usuario.IDUSUARIO);
                examenes = await _userRepo.ObtenerExamenesUsuario(usuario.IDUSUARIO);

                perfil.ordenaExamenes = (usuario.IDTIPOUSUARIO == 4)? true : false;
                for (int i = 0; i < roles.Count; i++)
                {
                    perfil.digitador = (roles[i].IdRol == 18) ? true : perfil.digitador;
                    perfil.recepcionista = (roles[i].IdRol == 5 || roles[i].IdRol == 38) ? true : perfil.recepcionista;
                    perfil.medicoVIH = (roles[i].IdRol == 85) ? true : perfil.medicoVIH;
                    perfil.preanalisis = (roles[i].IdRol == 99) ? true : perfil.preanalisis;
                    perfil.analisis = (roles[i].IdRol == 13 || roles[i].IdRol == 17) ? true : perfil.analisis;
                    perfil.verificador = (roles[i].IdRol == 7) ? true : perfil.verificador;
                    perfil.consultaResultados = (roles[i].IdRol == 30 || roles[i].IdRol == 50 || roles[i].IdRol == 74) ? true : perfil.consultaResultados;
                    perfil.descargaResultados = (roles[i].IdRol == 49 || roles[i].IdRol == 50 || roles[i].IdRol == 47) ? true : perfil.descargaResultados;
                }

                perfil.examenesPreanalisis = examenes.Select(e => new EnfermedadExamen { idExamen = e.idExamen, Examen = e.Examen, idTipo = e.idTipo })
                                                     .Where(x => x.idTipo == 5 || x.idTipo == 1 || x.idTipo == 3).ToList();
                perfil.examenesAnalisis = examenes.Select(e => new EnfermedadExamen { idExamen = e.idExamen, Examen = e.Examen, idTipo = e.idTipo })
                                                     .Where(x => x.idTipo == 1 || x.idTipo == 3).ToList();
                perfil.examenesVerificador = examenes.Select(e => new EnfermedadExamen { idExamen = e.idExamen, Examen = e.Examen, idTipo = e.idTipo })
                                                     .Where(x => x.idTipo == 2 || x.idTipo == 3).ToList();

                perfil.enfermedadesResultados = examenes
                    .GroupBy(e => e.idEnfermedad)
                    .Select(g => g.First())
                    .Select(e => new EnfermedadExamen { idEnfermedad = e.idEnfermedad,Enfermedad = e.Enfermedad})
                    .ToList();
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
            return new UsuarioPerfilOut
            {
                    USUARIO = usuario,
                    Perfil = perfil
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

        public async Task<SolicitudUsuarioResponse> RegistrarSolicitudUsuario(UsuarioPerfilInput input)
        {
            bool exito = false;
            string error = string.Empty;
            var solicitudUsuario = new SolicitudUsuario();
            solicitudUsuario = await ConvertTramaToSolicitudUsuario(input);

            try
            {
                if (!string.IsNullOrEmpty(solicitudUsuario.NUMERODOCUMENTO))
                {
                    solicitudUsuario.CODIGOSOLICITUD = "S" + solicitudUsuario.NUMERODOCUMENTO
                                                        + DateTime.Now.ToString("yyMMdd")
                                                        + DateTime.Now.ToString("hhmmss");

                    solicitudUsuario.ESTATUS = 0;
                    solicitudUsuario.ESTADO = 1;
                    solicitudUsuario.FECHAREGISTRO = DateTime.Now;
                    solicitudUsuario.IDSOLICITUDUSUARIO = await _solicitudRepo.RegistrarSolicitud(solicitudUsuario);
                }
                if (solicitudUsuario.IDSOLICITUDUSUARIO > 0)
                {
                    if (solicitudUsuario.LISTASOLICITUDUSUARIOROL.Count > 0)
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
                    }
                    else
                    {
                        throw new Exception("No se encontraron roles.");
                    }

                    (exito, error) = await _emailService.EnviarCorreoAsync(
                                           "Registro de solicitud Netlab",
                                           await PlantillaCorreo("RegistroSolicitud.html", solicitudUsuario.NOMBRE, solicitudUsuario.CODIGOSOLICITUD),
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
                await _solicitudRepo.DeleteRollback(solicitudUsuario.IDSOLICITUDUSUARIO);
                solicitudUsuario.IDSOLICITUDUSUARIO = 0;
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

        public Task<SolicitudUsuario> ConvertTramaToSolicitudUsuario(UsuarioPerfilInput input)
        {
            var solicitud = new SolicitudUsuario()
            {
                TIPOSOLICITUD = input.TIPOSOLICITUD,
                IDESTABLECIMIENTO = input.IDESTABLECIMIENTO,
                TIPODOCUMENTO = input.TIPODOCUMENTO,
                NUMERODOCUMENTO = input.NUMERODOCUMENTO,
                APELLIDOPATERNO = input.APELLIDOPATERNO,
                APELLIDOMATERNO = input.APELLIDOMATERNO,
                NOMBRE = input.NOMBRE,
                CORREOELECTRONICO = input.CORREOELECTRONICO,
                CELULAR = input.CELULAR,
                CONDICIONLABORAL = input.CONDICIONLABORAL,
                CARGO = input.CARGO,
                IDCOMPONENTE = input.IDCOMPONENTE,
                IDPROFESION = input.IDPROFESION,
                NUMEROCOLEGIATURA = input.NUMEROCOLEGIATURA,
                ORDENAEXAMEN = input.perfil.ordenaExamenes,
                COMITEEXPERTO = input.perfil.comiteDeExpertos,
                nombreautorizado = input.nombreautorizado,
                cargoautorizado = input.cargoautorizado
            };

            if (input.perfil.digitador)
            {
                var solicitudUsuarioRol = new SolicitudUsuarioRol();
                solicitudUsuarioRol.IDROL = 18;
                solicitud.LISTASOLICITUDUSUARIOROL.Add(solicitudUsuarioRol);
            }
            if (input.perfil.recepcionista)
            {
                if (solicitud.IDESTABLECIMIENTO == 991)
                {
                    var solicitudUsuarioRol = new SolicitudUsuarioRol();
                    solicitudUsuarioRol.IDROL = 38;
                    solicitud.LISTASOLICITUDUSUARIOROL.Add(solicitudUsuarioRol);
                }
                else
                {
                    var solicitudUsuarioRol = new SolicitudUsuarioRol();
                    solicitudUsuarioRol.IDROL = 5;
                    solicitud.LISTASOLICITUDUSUARIOROL.Add(solicitudUsuarioRol);
                }
            }
            if (input.perfil.medicoVIH)
            {
                var solicitudUsuarioRol = new SolicitudUsuarioRol();
                solicitudUsuarioRol.IDROL = 77;
                solicitud.LISTASOLICITUDUSUARIOROL.Add(solicitudUsuarioRol);
            }
            if (input.perfil.preanalisis)
            {
                var solicitudUsuarioRol = new SolicitudUsuarioRol();
                solicitudUsuarioRol.IDROL = 85;

                for (int i = 0; i < input.perfil.examenesPreanalisis.Count; i++)
                {
                    var solicitudUsuarioRolExamen = new SolicitudUsuarioRolExamen();
                    solicitudUsuarioRolExamen.IDENFERMEDAD = input.perfil.examenesPreanalisis[i].idEnfermedad;
                    solicitudUsuarioRolExamen.IDEXAMEN = input.perfil.examenesPreanalisis[i].idExamen;
                    solicitudUsuarioRol.LISTASOLICITUDUSUARIOROLEXAMEN.Add(solicitudUsuarioRolExamen);
                }
                solicitud.LISTASOLICITUDUSUARIOROL.Add(solicitudUsuarioRol);
            }
            if (input.perfil.analisis)
            {
                var solicitudUsuarioRol = new SolicitudUsuarioRol();
                solicitudUsuarioRol.IDROL = 17;

                for (int i = 0; i < input.perfil.examenesAnalisis.Count; i++)
                {
                    var solicitudUsuarioRolExamen = new SolicitudUsuarioRolExamen();
                    solicitudUsuarioRolExamen.IDENFERMEDAD = input.perfil.examenesAnalisis[i].idEnfermedad;
                    solicitudUsuarioRolExamen.IDEXAMEN = input.perfil.examenesAnalisis[i].idExamen;
                    solicitudUsuarioRol.LISTASOLICITUDUSUARIOROLEXAMEN.Add(solicitudUsuarioRolExamen);
                }
                solicitud.LISTASOLICITUDUSUARIOROL.Add(solicitudUsuarioRol);
            }
            if (input.perfil.verificador)
            {
                var solicitudUsuarioRol = new SolicitudUsuarioRol();
                solicitudUsuarioRol.IDROL = 7;

                for (int i = 0; i < input.perfil.examenesVerificador.Count; i++)
                {
                    var solicitudUsuarioRolExamen = new SolicitudUsuarioRolExamen();
                    solicitudUsuarioRolExamen.IDENFERMEDAD = input.perfil.examenesVerificador[i].idEnfermedad;
                    solicitudUsuarioRolExamen.IDEXAMEN = input.perfil.examenesVerificador[i].idExamen;
                    solicitudUsuarioRol.LISTASOLICITUDUSUARIOROLEXAMEN.Add(solicitudUsuarioRolExamen);
                }
                solicitud.LISTASOLICITUDUSUARIOROL.Add(solicitudUsuarioRol);
            }
            if (input.perfil.consultaResultados)
            {
                var solicitudUsuarioRol = new SolicitudUsuarioRol();
                solicitudUsuarioRol.IDROL = 30;

                for (int i = 0; i < input.perfil.enfermedadesResultados.Count; i++)
                {
                    var solicitudUsuarioRolExamen = new SolicitudUsuarioRolExamen();
                    solicitudUsuarioRolExamen.IDENFERMEDAD = input.perfil.enfermedadesResultados[i].idEnfermedad;
                    solicitudUsuarioRolExamen.IDEXAMEN = null;
                    solicitudUsuarioRol.LISTASOLICITUDUSUARIOROLEXAMEN.Add(solicitudUsuarioRolExamen);
                }
                solicitud.LISTASOLICITUDUSUARIOROL.Add(solicitudUsuarioRol);
            }
            if (input.perfil.descargaResultados)
            {
                var solicitudUsuarioRol = new SolicitudUsuarioRol();
                solicitudUsuarioRol.IDROL = 49;

                for (int i = 0; i < input.perfil.enfermedadesResultados.Count; i++)
                {
                    var solicitudUsuarioRolExamen = new SolicitudUsuarioRolExamen();
                    solicitudUsuarioRolExamen.IDENFERMEDAD = input.perfil.enfermedadesResultados[i].idEnfermedad;
                    solicitudUsuarioRolExamen.IDEXAMEN = null;
                    solicitudUsuarioRol.LISTASOLICITUDUSUARIOROLEXAMEN.Add(solicitudUsuarioRolExamen);
                }
                solicitud.LISTASOLICITUDUSUARIOROL.Add(solicitudUsuarioRol);
            }

            return Task.FromResult(solicitud); 
        }

        public async Task<EstadoSolicitud> EstadoSolicitudUsuario(string CodigoSolicitud)
        {
            return await _solicitudRepo.EstadoSolicitudUsuario(CodigoSolicitud);
        }
    }
}

