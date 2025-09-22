using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Netlab.Business.Services;
using Netlab.Domain.Entities;

namespace Netlab.WebApp.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsuarioController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("obtenerusuario")]
        public async Task<IActionResult> ObtenerUsuario([FromBody] User request)
        {
            var response = await _userService.ObtenerUsuarios(request);
            return Ok(response);
        }
    }
}
