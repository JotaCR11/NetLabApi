using Netlab.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Interfaces
{
    public interface IConsultaResultadosArbovirosisRepository
    {
        Task<List<ResultadosArbovirosis>?> ObtenerResultadosArbovirosisPorDni(string dni);
    }
}
