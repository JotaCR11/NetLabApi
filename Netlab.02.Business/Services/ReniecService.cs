using Microsoft.Extensions.Configuration;
using Netlab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Business.Services
{
    public class ReniecService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ReniecService> _logger;

        public ReniecService(IConfiguration config, ILogger<ReniecService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<PersonaResponse?> ObtenerDatosReniecAsync(string dni)
        {
            var client = new ServiceConsultaDniClient(
                ServiceConsultaDniClient.EndpointConfiguration.ServiceConsultaDniSoap12
            );

            // Autenticación si RENIEC exige credenciales
            var usuario = _config["Apis:Reniec:Usuario"];
            var clave = _config["Apis:Reniec:Clave"];

            try
            {
                var response = await client.consultarDniAsync(usuario, clave, dni);

                if (response == null || response.@return == null)
                    return null;

                return new PersonaResponse
                {
                    Dni = dni,
                    Nombres = response.@return.nombres,
                    ApellidoPaterno = response.@return.apellidoPaterno,
                    ApellidoMaterno = response.@return.apellidoMaterno,
                    Direccion = response.@return.direccion,
                    Sexo = response.@return.sexo
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consultando RENIEC SOAP");
                return null;
            }
            finally
            {
                await client.CloseAsync();
            }
        }
    }
}
