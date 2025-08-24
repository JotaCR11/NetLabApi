using Netlab.Domain.DTOs;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Helper;
using Netlab.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Netlab.Business.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(AuthRequest request);
}

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly ILogAccesoRepository _logAccesoRepository;
    private readonly string _jwtKey;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;

    public AuthService(IUsuarioRepository usuarioRepo, IConfiguration config, ILogAccesoRepository logAccesoRepository)
    {
        _usuarioRepo = usuarioRepo;
        _jwtKey = config["Jwt:Key"];
        _jwtIssuer = config["Jwt:Issuer"] ?? "Netlab";
        _jwtAudience = config["Jwt:Audience"] ?? "InterOpNetLabvUsuarios";
        _logAccesoRepository = logAccesoRepository;
    }
    /*
    public string? Login(string login, string password)
    {
        var user = _usuarioRepo.ObtenerPorLogin(login);
        if (user == null || !Seguridad.VerifyPassword(password, user.Contrasenia))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtKey);

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, user.Login)
            };

        foreach (var rol in user.Roles)
            claims.Add(new Claim(ClaimTypes.Role, rol.Nombre));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtIssuer,
            Audience = _jwtAudience
        };

        var keyst = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    */

    public async Task<AuthResponse> LoginAsync(AuthRequest request)
    {
        var user = await _usuarioRepo.GetByLoginAsync(request);
        //if (user == null) return null;

        /*
         Respuesta SP:
            6: Credenciales válidas
            5: Contraseña incorrecta
            4: Usuario desactivado
            3: Clave caducada
            2: Usuario nuevo
         */
        if (user.Respuesta != 6)
        {
            //var logError = new LogAcceso
            //{
            //    IdUsuario = 0,
            //    Ruta = "/api/auth/login",
            //    Metodo = "POST",
            //    Fecha = DateTime.Now,
            //    //IpCliente = ,
            //    EsExitoso = false,
            //    Mensaje = "Credenciales inválidas",
            //    Request = JsonSerializer.Serialize(request)
            //};

            //await _logAccesoRepository.RegistrarLogAsync(logError);
            return null;
        }


        var roles = await _usuarioRepo.ObtenerRolesAsync(user.IdUsuario);
        //var log = new LogAcceso
        //{
        //    IdUsuario = user.IdUsuario,
        //    Ruta = "/api/auth/login",
        //    Metodo = "POST",
        //    Fecha = DateTime.Now,
        //    EsExitoso = true,
        //    Mensaje = "Credenciales válidas",
        //    Request = JsonSerializer.Serialize(request)
        //};

        //await _logAccesoRepository.RegistrarLogAsync(log);

        return new AuthResponse
        {
            Token = GenerateJwtToken(user, roles),
            NombreUsuario = $"{user.Nombres} {user.ApellidoPaterno} {user.ApellidoMaterno}".Trim(),
            Roles = roles.ToArray()
        };
    }

    private string GenerateJwtToken(Usuario user, List<string> roles)
    {
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Login),
        new Claim("idUsuario", user.IdUsuario.ToString()),
        new Claim(ClaimTypes.Email, user.Correo ?? ""),
        new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString())
    };

        //foreach (var rol in roles)
        //{
        //    claims.Add(new Claim(ClaimTypes.Role, rol));
        //}

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtIssuer,
            audience: _jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(20),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
