using Netlab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Interfaces
{
    public interface ILogAccesoRepository
    {
        Task RegistrarLogAsync(LogAcceso log);
    }
}
