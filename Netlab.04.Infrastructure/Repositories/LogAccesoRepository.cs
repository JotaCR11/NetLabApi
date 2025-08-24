using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Infrastructure.Repositories
{
    
    public class LogAccesoRepository : ILogAccesoRepository
    {
        private readonly IDatabaseFactory _dbFactory;

        public LogAccesoRepository(IDatabaseFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task RegistrarLogAsync(LogAcceso log)
        {
            using var db = _dbFactory.GetDatabase();
            await db.InsertAsync(log);
        }
    }
}
