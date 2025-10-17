using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Netlab.Business.Services;
using Netlab.Domain.Entities;

namespace Netlab.WebApp.Controllers
{
    [Route("api/solicitud")]
    [ApiController]
    [Authorize]
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

        [HttpGet("obtenerestablecimiento")]
        public async Task<IActionResult> ObtenerEstablecimientoPorTexto(string request)
        {
            var response = await _solicitudService.ObtenerEstablecimientoPorTexto(request);
            return Ok(response);
        }

        [HttpGet("obtenerdatosusuario")]
        public async Task<IActionResult> ObtenerPerfilUsuario(string documentoIdentidad)
        {
            var response = await _solicitudService.ObtenerPerfilUsuario(documentoIdentidad);
            return Ok(response);
        }

        [HttpPost("registrarsolicitud")]
        public async Task<IActionResult> RegistrarSolicitudUsuario([FromBody] SolicitudUsuario request)
        {
            var response = await _solicitudService.RegistrarSolicitudUsuario(request);
            return Ok(response);
        }
    }
}
