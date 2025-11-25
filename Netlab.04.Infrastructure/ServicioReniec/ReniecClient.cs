using Microsoft.Extensions.Options;
using Netlab.Domain.Entities;
using ReniecService;
using System.ServiceModel;

namespace Netlab.Infrastructure.ServicioReniec
{
    public class ReniecClient : IReniecClient
    {
        private readonly serviciomqSoapClient _client;
        private readonly ReniecServiceCredenciales _options;

        public ReniecClient(IOptions<ReniecServiceCredenciales> options)
        {
            _options = options.Value;

            var binding = new BasicHttpBinding
            {
                MaxReceivedMessageSize = 65536 * 10,
                Security = new BasicHttpSecurity
                {
                    Mode = BasicHttpSecurityMode.None // ← Permite HTTP
                },
            };

            var endpoint = new EndpointAddress(_options.Url);
            _client = new serviciomqSoapClient(binding, endpoint);
        }

        public async Task<ReniecResponse> ObtenerDatosPorDniAsync(string dni)
        {
            Credencialmq cm = new Credencialmq
            {
                app = _options.app,
                usuario = _options.usuario,
                clave = _options.clave
            };

            var response = await _client.obtenerDatosBasicosAsync(cm,dni);

            return new ReniecResponse
            {
                Dni = response.obtenerDatosBasicosResult[22],
                Nombres = response.obtenerDatosBasicosResult[5],
                ApellidoPaterno = response.obtenerDatosBasicosResult[2],
                ApellidoMaterno = ApellidoMaterno(response.obtenerDatosBasicosResult)
            };
        }

        public string ApellidoMaterno(string[] datos)
        {
            string apellidoMaterno = datos[3];
            //apellido de casada
            if (datos[4] == "SIN DATOS")
            {
                datos[4] = "";
            }
            if (datos[4] != "")
            {
                apellidoMaterno = datos[3] + " " + datos[4];

                if (datos[3] == "SIN DATOS")
                {
                    apellidoMaterno = datos[4];
                }
            }
            
            return apellidoMaterno;
        }
    }
}
