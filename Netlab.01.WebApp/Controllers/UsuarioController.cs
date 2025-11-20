using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netlab.Business.Services;
using Netlab.Domain.BusinessObjects.Usuario;
using Netlab.Domain.Entities;

namespace Netlab.WebApp.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _userService;
        private readonly ISolicitudUsuarioService _solicitud;
        public UsuarioController(IUsuarioService userService, ISolicitudUsuarioService solicitud)
        {
            _userService = userService;
            _solicitud = solicitud;
        }

        [HttpGet("listacantidadusuario")]
        public async Task<IActionResult> ObtenerListaCantidadUsuario()
        {
            var response = await _userService.ObtenerCantidadTotalUsuario();
            return Ok(response);
        }

        [HttpPost("listahistorialatenciones")]
        public async Task<IActionResult> ObtenerListaAtenciones([FromBody] UsuarioAtencionInput input)
        {
            var response = await _userService.ObtenerListaAtenciones(input);
            return Ok(response);
        }

        [HttpGet("listaubigeousuario")]
        public async Task<IActionResult> ObtenerListaUbigeoUsuario()
        {
            var response = await _userService.ObtenerListaUbigeoUsuario();
            return Ok(response);
        }

        [HttpPost("listadetalleatenciones")]
        public async Task<IActionResult> ObtenerListaDetalleAtenciones([FromBody] UsuarioAtencionInput input)
        {
            var response = await _userService.ObtenerListaDetalleAtenciones(input);
            return Ok(response);
        }

        [HttpGet("obtienedatosolicitudporid")]
        public async Task<IActionResult> ObtenerDatosSolicitudPorId(int IdSolicitudUsuario)
        {
            var response = await _solicitud.ObtenerDatosSolicitudAsync(IdSolicitudUsuario);
            return Ok(response);
        }

        [HttpPost("aprobarsolicitudusuario")]
        public async Task<IActionResult> AprobarSolicitudUsuario(int IdSolicitudUsuario)
        {
            var response = await _userService.AprobarSolicitudUsuario(IdSolicitudUsuario);
            return Ok(response);
        }

        [HttpPost("listahistorialatencionespendientes")]
        public async Task<IActionResult> ObtenerListaPendienteSolicitudUsuario([FromBody] UsuarioAtencionInput input)
        {
            var response = await _userService.ObtenerListaPendienteSolicitudUsuario(input);
            return Ok(response);
        }

        [HttpGet("indicadoressolicitudes/{anio}")]
        public async Task<IActionResult> IndicadorAtencionSolicitudUsuarios(int anio)
        {
            var response = await _userService.IndicadorAtencionSolicitudUsuarios(anio);
            return Ok(response);
        }

        [HttpPost("rechazarsolicitudusuario")]
        public async Task<IActionResult> RechazarSolicitudUsuario([FromBody] SolicitudUsuarioRechazo input)
        {
            var response = await _userService.RechazarSolicitudUsuario(input);
            return Ok(response);
        }
    }
}
