using Netlab.Business.Services;
using Netlab.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Netlab.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request)
        {
            string? ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                            ?? HttpContext.Connection.RemoteIpAddress?.ToString();

            if (ipAddress == "::1")
                ipAddress = "127.0.0.1";

            request.IPAddress = ipAddress;
            var response = await _authService.LoginAsync(request);
            if (response == null || string.IsNullOrEmpty(response.Token)) return Unauthorized(new {message = "Credenciales inválidas" });

            return Ok(new { response.Token, response.NombreUsuario });
        }
    }

    public class LoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
