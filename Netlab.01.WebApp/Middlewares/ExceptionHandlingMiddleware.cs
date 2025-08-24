using Netlab.Domain.DTOs;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Netlab.WebApp.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, ILogAccesoRepository logRepo)
        {
            var originalBodyStream = context.Response.Body;

            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;
            String requestBody = await LeerCuerpoRequestAsync(context);
            try
            {
                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                string responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                string stackTrace = null;
                if (!string.IsNullOrWhiteSpace(responseText) && responseText.Contains("\"errors\""))
                {
                    try
                    {
                        var jsonDoc = JsonDocument.Parse(responseText);
                        if (jsonDoc.RootElement.TryGetProperty("errors", out var errorsProp) && errorsProp.ValueKind == JsonValueKind.Array)
                        {
                            var errores = errorsProp.EnumerateArray().Select(e => e.GetString());
                            stackTrace = string.Join("; ", errores);
                        }
                    }
                    catch (Exception parseEx)
                    {
                        _logger.LogWarning(parseEx, "No se pudo parsear la respuesta para extraer errores");
                    }
                }

                var mensaje = context.Response.StatusCode >= 200 && context.Response.StatusCode < 300
                    ? "Request procesado correctamente"
                    : $"Error {context.Response.StatusCode}";

                await RegistrarLogAccesoAsync(context, logRepo, context.Response.StatusCode >= 200 && context.Response.StatusCode < 300, mensaje, stackTrace, requestBody);
            }
            catch (Exception ex)
            {
                //await HandleExceptionAsync(context, ex);

                var mensaje = $"Error interno en request: {ex.Message}";
                await RegistrarLogAccesoAsync(context, logRepo, false, mensaje, ex.StackTrace, requestBody);

                throw;
            }
            finally
            {
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = new ApiResponse<object>
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "Ha ocurrido un error inesperado.",
                Data = null,
                Errors = new List<string> { ex.Message }
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }

        private async Task RegistrarLogAccesoAsync(HttpContext context, ILogAccesoRepository logRepo, bool exito, string mensaje, string stackTrace = null, string requestBody = null)
        {
            try
            {
                var ruta = context.Request.Path;
                var metodo = context.Request.Method;
                var ip = context.Connection.RemoteIpAddress?.ToString();
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
                    Request = requestBody,
                    StatusCode = statusCodeResponse
                };

                await logRepo.RegistrarLogAsync(log);
            }
            catch (Exception ex)
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
