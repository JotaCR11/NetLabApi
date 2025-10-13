using Azure.Core;
using Netlab.Domain.DTOs;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Infrastructure.Database;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

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

        public async Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorTexto(string texto)
        {
            using var db = _databaseFactory.GetDatabase();
            return await db.FetchAsync<EstablecimientoResponse>(
                "EXEC pNLS_SolicitudUsuarioEstablecimientoTexto @0",
                texto);
        }

        public async Task<PerfilUsuarioResponse> ObtenerPerfilUsuario(string documentoIdentidad)
        {
            using var db = _databaseFactory.GetDatabase();
            return await db.SingleOrDefaultAsync<PerfilUsuarioResponse>(
                "EXEC pNLS_SolicitudUsuarioGetDatosUsuario @0",
                documentoIdentidad);
        }
    }
}
