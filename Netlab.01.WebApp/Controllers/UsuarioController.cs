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
    //[Authorize]
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

        [HttpGet("listahistorialatenciones")]
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

        [HttpGet("listadetalleatenciones")]
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

        //[HttpGet("obtenerusuario")]
        //public async Task<IActionResult> ObtenerUsuario([FromBody] User request)
        //{
        //    var response = await _userService.ObtenerUsuarios(request);
        //    return Ok(response);
        //}

        ////[HttpPost("registrarusuario")]
        ////public async Task RegistrarUsuario([FromBody] User request)
        ////{
        ////    await _userService.RegistrarUsuario(request);
        ////}

        //[HttpPost("editarusuario")]
        //public async Task EditarUsuario([FromBody] User request)
        //{
        //    await _userService.EditarUsuario(request);
        //}

        //[HttpGet("buscarusuarioid")]
        //public async Task<IActionResult> ObtenerUsuario(int IdUsuario)
        //{
        //    var response = await _userService.ObtenerPerfilUsuario(IdUsuario);
        //    return Ok(response);
        //}

        //[HttpPost("registrarusuario")]
        //public async Task RegistrarUsuario([FromBody] User request)
        //{

        //}

    }
}
