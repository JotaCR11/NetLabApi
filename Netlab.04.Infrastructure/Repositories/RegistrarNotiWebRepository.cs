using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Infrastructure.Database;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Infrastructure.Repositories
{
    public class RegistrarNotiWebRepository : IRegistrarNotiWebRepository
    {
        private readonly IDatabaseFactory _databaseFactory;
        public RegistrarNotiWebRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public async Task<DatoNotiwebNetlab> RegistrarNotificacionWebAsync(DatoNotiwebNetlab request)
        {
            using var db = _databaseFactory.GetDatabase();
            await db.InsertAsync(request);
            return request;
        }

        public async Task<LogTestCDC> TestRegistroCDCAsync(LogTestCDC request)
        {
            using var db = _databaseFactory.GetDatabase();
            await db.InsertAsync(request);
            return request;
        }
    }
}
