using Netlab.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Domain.Interfaces
{
    public interface IRegistrarNotiWebRepository
    {
        Task<DatoNotiwebNetlab> RegistrarNotificacionWebAsync(DatoNotiwebNetlab request);
        Task<LogTestCDC> TestRegistroCDCAsync(LogTestCDC request);
    }
}
