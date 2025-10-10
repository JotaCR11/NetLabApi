using Azure.Core;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Infrastructure.Database;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Netlab.Infrastructure.Repositories
{
    public class SolicitudUsuarioRepository : ISolicitudUsuarioRepository
    {
        private readonly IDatabaseFactory _databaseFactory;

        public SolicitudUsuarioRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public async Task<SolicitudUsuario> RegistrarSolicitudUsuario(SolicitudUsuario solicitudUsuario)
        {
            using var db = _databaseFactory.GetDatabase();
            return await db.SingleOrDefaultAsync<SolicitudUsuario>
                (
                    "EXEC pNLI_SolicitudUsuario @0",
                    JsonSerializer.Serialize(solicitudUsuario)
                );
        }

        public async Task<List<Establecimiento>> ObtenerEstablecimientoPorNombre(string nombre)
        {
            using var db = _databaseFactory.GetDatabase();
            var establecimiento = await db.Query<Establecimiento>().Where(x => x.Nombre.Contains(nombre) && x.IdLabIns != 2).ToListAsync();
            return establecimiento;
        }
    }
}
