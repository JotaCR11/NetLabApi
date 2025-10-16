using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netlab.Business.Services;
using Netlab.Domain.Entities;

namespace Netlab.WebApp.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _userService;
        public UsuarioController(IUsuarioService userService)
        {
            _userService = userService;
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
