using Netlab.Domain.DTOs;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Infrastructure.Database;
using NPoco;

namespace Netlab.Infrastructure.Repositories
{
    public class ConsultaResultadosArbovirosisRepository : IConsultaResultadosArbovirosisRepository
    {
        private readonly IDatabaseFactory _databaseFactory;
        public ConsultaResultadosArbovirosisRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public async Task<List<ResultadosArbovirosis>?> ObtenerResultadosArbovirosisPorDni(string dni)
        {
            using var db = _databaseFactory.GetDatabase();
            return await db.FetchAsync<ResultadosArbovirosis>(
                "EXEC pNLS_ConsultaResultadosDNIPrueba @0", 
                dni);
        }
    }
}