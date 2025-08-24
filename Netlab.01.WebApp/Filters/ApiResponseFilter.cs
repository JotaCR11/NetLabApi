using Netlab.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Netlab.WebApp.Filters
{
    public class ApiResponseFilter : IAsyncResultFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // No hacemos nada antes de ejecutar la acción
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value is IApiResponseMarker) // marker interface
            {
                await next();
                return;
            }

            //var result = context.Result;

            // Obtenemos el StatusCode del contexto
            //var statusCode = context.HttpContext.Response.StatusCode;

            //// Calculamos success en base al statusCode

            //object data = null;
            //string message = "Operación exitosa";

            //if (result is ObjectResult objectResult)
            //{
            //    data = objectResult.Value;
            //    statusCode = objectResult.StatusCode ?? statusCode;

            //    if (statusCode >= 400)
            //    {
            //        message = "Ocurrió un error";
            //    }
            //}

            //bool success = statusCode >= 200 && statusCode < 300;

            //var apiResponse = new
            //{
            //    statusCode,
            //    success,
            //    message,
            //    data,
            //    timestamp = DateTime.UtcNow,
            //    errors = new string[] { }
            //};

            //context.Result = new ObjectResult(apiResponse)
            //{
            //    StatusCode = statusCode
            //};
            if (context.Result is ObjectResult result)
            {
                var apiResponse = new ApiResponse<object>
                {
                    StatusCode = result.StatusCode ?? StatusCodes.Status200OK,
                    Success = result.StatusCode >= 200 && result.StatusCode < 300,
                    Message = result.StatusCode == 200 ? "Operación exitosa" : "Ocurrió un error",
                    Data = result.Value,
                    Timestamp = DateTime.UtcNow
                };

                context.Result = new ObjectResult(apiResponse)
                {
                    StatusCode = result.StatusCode
                };
            }

            await next();
        }
        /*
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                var statusCode = objectResult.StatusCode ?? 200;

                var response = new ApiResponse<object>
                {
                    StatusCode = statusCode,
                    Message = GetDefaultMessage(statusCode),
                    Data = objectResult.Value,
                    Errors = statusCode >= 400 ? new List<string> { objectResult.Value?.ToString() ?? "Error" } : new List<string>()
                };

                context.Result = new ObjectResult(response)
                {
                    StatusCode = statusCode
                };
            }
            else if (context.Result is StatusCodeResult statusCodeResult)
            {
                var statusCode = statusCodeResult.StatusCode;

                var response = new ApiResponse<object>
                {
                    StatusCode = statusCode,
                    Message = GetDefaultMessage(statusCode),
                    Data = null
                };

                context.Result = new ObjectResult(response)
                {
                    StatusCode = statusCode
                };
            }
        }
        

        private string GetDefaultMessage(int statusCode) =>
            statusCode switch
            {
                200 => "Operación exitosa",
                201 => "Recurso creado correctamente",
                400 => "Solicitud inválida",
                401 => "No autorizado",
                403 => "Prohibido",
                404 => "No encontrado",
                500 => "Error interno del servidor",
                _ => "Resultado de la operación"
            };
        */
    }
}
