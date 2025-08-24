using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Helper;
using Microsoft.AspNetCore.Http;
using NPoco.fastJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.WebApp.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IEmailService emailService)
        {
            using var scope = context.RequestServices.CreateScope();
            var logRepo = scope.ServiceProvider.GetRequiredService<ILogAccesoRepository>();
            try
            {
                // Continuar con el pipeline
                await _next(context);

                bool esLogin = context.Request.Path.Value?.Contains("/api/auth/login", StringComparison.OrdinalIgnoreCase) == true;
                bool esAccesoNoPermitido = context.Response.StatusCode == StatusCodes.Status401Unauthorized ||
                                           context.Response.StatusCode == StatusCodes.Status403Forbidden;

                if(esLogin || esAccesoNoPermitido)
                {
                    var mensaje = context.Response.StatusCode switch
                    {
                        200 => "Login exitoso",
                        400 => "Credenciales inválidas",
                        401 => "Token inválido o no proporcionado",
                        403 => "No tienes permiso para acceder",
                        _ => "Acceso procesado"
                    };
                    // Registrar log de acceso
                    await RegistrarLogAccesoAsync(context, logRepo, context.Response.StatusCode == 200, mensaje);
                }
            }
            catch (Exception ex)
            {
                var mensaje = $"Error interno: {ex.Message}";
                // Registrar log con error
                await RegistrarLogAccesoAsync(context, logRepo, false, mensaje, ex.StackTrace);

                // Enviar correo de alerta
                await emailService.EnviarCorreoAsync("Prueba de envío Exception", mensaje);

                // Re-lanzar la excepción para que el sistema la capture si es necesario
                throw;
            }
        }

        private async Task RegistrarLogAccesoAsync(HttpContext context, ILogAccesoRepository logRepo, bool exito, string mensaje, string stackTrace = null)
        {
            try
            {
                var ruta = context.Request.Path;
                var metodo = context.Request.Method;
                var ip = context.Connection.RemoteIpAddress?.ToString();
                var body = await LeerCuerpoRequestAsync(context);
                var userIdClaim = context.User.FindFirst("idUsuario");

                int.TryParse(userIdClaim?.Value, out int idUsuario);
                if (idUsuario == 0)
                    idUsuario = -1; // visitante o usuario no autenticado

                var statusCodeResponse = context.Response.StatusCode;
                var log = new LogAcceso
                {
                    IdUsuario = idUsuario,
                    Ruta = ruta,
                    Metodo = metodo,
                    Fecha = DateTime.Now,
                    IpCliente = ip,
                    EsExitoso = exito,
                    Mensaje = mensaje,
                    StackTrace = stackTrace,
                    Request = body,
                    StatusCode = statusCodeResponse
                };

                await logRepo.RegistrarLogAsync(log);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error al registrar log de acceso");
                // No lanzar excepción si el log falla, para no interrumpir el flujo.
            }
        }

        private async Task<string> LeerCuerpoRequestAsync(HttpContext context)
        {
            context.Request.EnableBuffering(); // Permite volver a leer el stream
            context.Request.Body.Position = 0;

            using var reader = new StreamReader(context.Request.Body, encoding: Encoding.UTF8, leaveOpen: true);
            string body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            return body.Length > 1000 ? body.Substring(0, 1000) : body;
        }
    }
}
