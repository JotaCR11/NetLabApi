using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Netlab.WebApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PruebaAccesoController : ControllerBase
    {
        // ✅ Solo autenticación necesaria
        [HttpGet("libre")]
        public IActionResult Libre()
        {
            return Ok("Este método está disponible para cualquier usuario autenticado.");
        }

        // ✅ Solo para el rol Admin
        [HttpGet("solo-admin")]
        [Authorize(Roles = "Administrador")]
        public IActionResult SoloAdmin()
        {
            return Ok("Este método solo es accesible por usuarios con rol Admin.");
        }

        // ✅ Solo para el rol Analista
        [HttpGet("solo-analista")]
        [Authorize(Roles = "Usuario")]
        public IActionResult SoloAnalista()
        {
            return Ok("Este método solo es accesible por usuarios con rol Analista.");
        }
    }
}
