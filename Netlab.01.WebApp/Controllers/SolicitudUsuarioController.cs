using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Netlab.Business.Services;
using Netlab.Domain.DTOs;
using Netlab.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Netlab.WebApp.Controllers
{
    [Route("api/solicitud")]
    [ApiController]
    //[Authorize]
    public class SolicitudUsuarioController : ControllerBase
    {
        private readonly ISolicitudUsuarioService _solicitudService;

        public SolicitudUsuarioController(ISolicitudUsuarioService solicitudService)
        {
            _solicitudService = solicitudService;        
        }

        [HttpGet("obtenerestablecimientoporcodigo")]
        public async Task<IActionResult> ObtenerEstablecimientoPorCodigoUnico(string request)
        {
            var response = await _solicitudService.ObtenerEstablecimientoPorCodigoUnico(request);
            return Ok(response);
        }

        [HttpGet("obtenerestablecimientosincodigo")]
        public async Task<IActionResult> ObtenerEstablecimientoSinCodigo()
        {
            var response = await _solicitudService.ObtenerEstablecimientoSinCodigoUnico();
            return Ok(response);
        }

        [HttpGet("obtenerdatosusuario")]
        public async Task<IActionResult> ObtenerPerfilUsuario(string documentoIdentidad)
        {
            var response = await _solicitudService.ObtenerPerfilUsuario(documentoIdentidad);
            return Ok(response);
        }

        [HttpPost("validacorreo")]
        public async Task<IActionResult> EnviarCodigo(string documentoIdentidad, string email, string nombre)
        {
            var (exito, error) = await _solicitudService.EnviarCodigoAsync(documentoIdentidad, email,nombre);
            if (exito)
            {
                var response = new ApiResponse<bool>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Correo enviado correctamente",
                    Data = true
                };

                return Ok(response);
            }
            else { 
                var errorResponse = new ApiResponse<bool>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Success = false,
                    Message = "Error al enviar el correo",
                    Data = false,
                    Errors = new List<string> { error }
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
                }
            }

        [HttpPost("verificacodigoseguridad")]
        public async Task<IActionResult> ValidaCodigoSeguridad(string documentoIdentidad, string email, string codigo)
        {
            var error = await _solicitudService.ValidarCodigoAsync(documentoIdentidad,email, codigo);
            if (string.IsNullOrEmpty(error))
            {
                var response = new ApiResponse<bool>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Success = true,
                    Message = error,
                    Data = true
                };
                return Ok(response);
            }
            else
            {
                var errorResponse = new ApiResponse<bool>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Success = false,
                    Message = error,
                    Data = false
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
            
        }

        [HttpGet("listaenfermedad")]
        public async Task<IActionResult> ListaEnfermedad(string nombre)
        {
            var response = await _solicitudService.ListaEnfermedad(nombre);
            return Ok(response);
        }

        [HttpGet("listaexamen")]
        public async Task<IActionResult> ListaExamenPorEnfermedad(int IdEnfermedad, string nombre)
        {
            var response = await _solicitudService.ListaExamenPorEnfermedad(IdEnfermedad,nombre);
            return Ok(response);
        }

        [HttpPost("registrarsolicitud")]
        public async Task<IActionResult> RegistrarSolicitudUsuario([FromBody] SolicitudUsuario request)
        {
            var solicitud = await _solicitudService.RegistrarSolicitudUsuario(request);
            if (solicitud.SOLICITUDUSUARIO.IDSOLICITUDUSUARIO > 0)
            {
                var response = new ApiResponse<SolicitudUsuarioResponse>
                {
                    StatusCode = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Solicitud generada correctamente",
                    Data = solicitud
                };
                return Ok(response);
            }
            else
            {
                var errorResponse = new ApiResponse<bool>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Success = false,
                    Message = "Error al generar la solicitud",
                    Data = false

                };
                return Ok(errorResponse);
            }
        }
        [HttpPost("uploadpdf")]
        public async Task<IActionResult> SubirPdfSolicitud(ArchivoInput file)
        {
            var response = await _solicitudService.RegistroFormularioPDF(file);
            return Ok(response);
        }

        
    }
}
