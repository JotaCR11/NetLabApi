using Netlab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Infrastructure.ServicioReniec
{
    public interface IReniecClient
    {
        Task<ReniecResponse> ObtenerDatosPorDniAsync(string dni);
    }
}
