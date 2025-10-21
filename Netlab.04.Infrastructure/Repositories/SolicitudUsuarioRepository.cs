using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Infrastructure.Database;
using NPoco;
using System.Text.Json;

namespace Netlab.Infrastructure.Repositories
{
    public class SolicitudUsuarioRepository : ISolicitudUsuarioRepository
    {
        private readonly IDatabaseFactory _databaseFactory;

        public SolicitudUsuarioRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public async Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorTexto(string texto)
        {
            using var db = _databaseFactory.GetDatabase();
            return await db.FetchAsync<EstablecimientoResponse>(
                "EXEC pNLS_SolicitudUsuarioEstablecimientoTexto @0",
                texto);
        }
        public async Task<int> RegistrarEstablecimiento(EstablecimientoCSV establecimientocsv)
        {
            using var db = _databaseFactory.GetDatabase();
            int IdEstablecimiento = await db.ExecuteScalarAsync<int>
                (
                "EXEC pNLI_SolicitudUsuarioEstablecimiento @0,@1,@2,@3,@4,@5,@6,@7,@8," +
                                                           "@9,@10,@11,@12,@13,@14",
                new object[] 
                    {
                        establecimientocsv.INSTITUCION,
                        establecimientocsv.COD_IPRESS,
                        establecimientocsv.NOMBRE,
                        establecimientocsv.CLASIFICACION,
                        establecimientocsv.DEPARTAMENTO,
                        establecimientocsv.PROVINCIA,
                        establecimientocsv.DISTRITO,
                        establecimientocsv.UBIGEO,
                        establecimientocsv.DIRECCION,
                        establecimientocsv.DISA,
                        establecimientocsv.RED,
                        establecimientocsv.MICRORED,
                        establecimientocsv.CATEGORIA,
                        establecimientocsv.NORTE,
                        establecimientocsv.ESTE
                    }
                );
            return IdEstablecimiento;
        }
        
        public async Task<PerfilUsuarioResponse> ObtenerPerfilUsuario(string documentoIdentidad)
        {
            using var db = _databaseFactory.GetDatabase();
            return await db.SingleOrDefaultAsync<PerfilUsuarioResponse>(
                "EXEC pNLS_SolicitudUsuarioGetDatosUsuario @0",
                documentoIdentidad);
        }

        public async Task<int> RegistraCodigoValidacionCorreo(SolicitudUsuarioCorreoValidacion solicitudUsuarioCorreoValidacion)
        {
            using var db = _databaseFactory.GetDatabase();
            return (int)await db.InsertAsync(solicitudUsuarioCorreoValidacion);
        }

        public async Task<SolicitudUsuarioCorreoValidacion?> ObtenerDatosValidacionCorreo(string documentoIdentidad,string email, string codigo)
        {
            using var db = _databaseFactory.GetDatabase();
            return await db.QueryAsync<SolicitudUsuarioCorreoValidacion>()
                .Where(x => x.DocumentoIdentidad.Equals(documentoIdentidad)
                        && x.Email.Equals(email)
                        && x.Codigo.Equals(codigo)
                )
                .OrderByDescending(p => p.FechaGeneracion)
                .FirstOrDefault();
        }

        public async Task<int> ActualizaDatoCodigoValidacion(SolicitudUsuarioCorreoValidacion solicitudUsuarioCorreoValidacion)
        {
            using var db = _databaseFactory.GetDatabase();
            return await db.UpdateAsync(solicitudUsuarioCorreoValidacion);
        }

        public async Task<List<Enfermedad>> ListaEnfermedad(string nombre)
        {
            using var db = _databaseFactory.GetDatabase();
            return await db.QueryAsync<Enfermedad>()
                        .Where(x=>x.Nombre.Contains(nombre)).ToList();
        }

        public async Task<List<SoliciudUsuarioExamen>> ListaExamenPorEnfermedad(int IdEnfermedad, string nombre)
        {
            using var db = _databaseFactory.GetDatabase();
            var response = await db.FetchAsync<SoliciudUsuarioExamen>(
                "EXEC pNLS_ExamenesPorEnfermedad @0",
                IdEnfermedad
                );
            return response.Where(x => x.nombre.Contains(nombre.ToUpper())).ToList();
        }

        public async Task<SolicitudUsuario> RegistrarSolicitudUsuario(SolicitudUsuario solicitudUsuario)
        {
            using var db = _databaseFactory.GetDatabase();
            var json = JsonSerializer.Serialize(solicitudUsuario);
            return await db.SingleOrDefaultAsync<SolicitudUsuario>
                (
                    "EXEC pNLI_SolicitudUsuario @0",
                    JsonSerializer.Serialize(solicitudUsuario)
                );
        }
    }
}
