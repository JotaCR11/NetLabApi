using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Business.Services
{
    public interface IRegistrarNotiWebService
    {
        Task<DatoNotiwebNetlabResponse> RegistrarAsync(DatoNotiwebNetlab request);
        Task<LogTestCDC> TestRegistroCDCAsync(TestRequest request);
    }

    public class RegistrarNotiWebService : IRegistrarNotiWebService
    {
        private readonly IRegistrarNotiWebRepository _registrarRepo;
        public RegistrarNotiWebService(IRegistrarNotiWebRepository registrarRepo)
        {
            _registrarRepo = registrarRepo;
        }

        public async Task<DatoNotiwebNetlabResponse> RegistrarAsync(DatoNotiwebNetlab request)
        {
            request.codigo_INS = Guid.NewGuid().ToString();
            request.Estado = 0;
            await _registrarRepo.RegistrarNotificacionWebAsync(request);

            return new DatoNotiwebNetlabResponse
            {
                responseId = request.Id,
                codigo_cdc = request.codigo_cdc,
                codigo_INS = request.codigo_INS
            };
        }

        public async Task<LogTestCDC> TestRegistroCDCAsync(TestRequest request)
        {
            LogTestCDC nuevaTrama = new LogTestCDC
            {
                Trama = System.Text.Json.JsonSerializer.Serialize(request),
                FechaRegistro = DateTime.UtcNow
            };

            await _registrarRepo.TestRegistroCDCAsync(nuevaTrama);

            return nuevaTrama;
        }
    }
}
