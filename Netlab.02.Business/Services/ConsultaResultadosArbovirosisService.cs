using Netlab.Domain.DTOs;
using Netlab.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Business.Services
{
    public interface IConsultaResultadosArbovirosisService
    {
        Task<List<ResultadosArbovirosis>> ObtenerResultadosArbovirosisPorDni(string dni);
    }

    public class ConsultaResultadosArbovirosisService : IConsultaResultadosArbovirosisService
    {
        private readonly IConsultaResultadosArbovirosisRepository _repository;
        public ConsultaResultadosArbovirosisService(IConsultaResultadosArbovirosisRepository consultaResultadosArbovirosisRepository)
        {
            _repository = consultaResultadosArbovirosisRepository;
        }
        public Task<List<ResultadosArbovirosis>> ObtenerResultadosArbovirosisPorDni(string dni) { 
            return _repository.ObtenerResultadosArbovirosisPorDni(dni);
        }
    }
}
