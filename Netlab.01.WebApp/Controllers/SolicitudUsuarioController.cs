using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Netlab.Business.Services;
using Netlab.Domain.Entities;

namespace Netlab.WebApp.Controllers
{
    [Route("api/solicitud")]
    [ApiController]
    public class SolicitudUsuarioController : ControllerBase
    {
        private readonly ISolicitudUsuarioService _solicitudService;
        public SolicitudUsuarioController(ISolicitudUsuarioService solicitudService)
        {
            _solicitudService = solicitudService;        
        }

        [HttpPost("registrarsolicitud")]
        public async Task<IActionResult> RegistrarSolicitudUsuario([FromBody] SolicitudUsuario request)
        {
            var response = await _solicitudService.RegistrarSolicitudUsuario(request);
            return Ok(response);
        }

        [HttpGet("eessnorenipress")]
        public async Task<IActionResult> ObtenerEstablecimientoPorNombre(string nombre)
        {
            var response = await _solicitudService.ObtenerEstablecimientoPorNombre(nombre);
            return Ok(response);
        }
    }
}
