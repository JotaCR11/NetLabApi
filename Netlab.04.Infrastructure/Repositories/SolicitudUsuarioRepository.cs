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
        public async Task<List<EstablecimientoResponse>> ObtenerEstablecimientoPorCodigoUnico(string CodigoUnico)
        {
            using var db = _databaseFactory.GetDatabase();
            return await db.FetchAsync<EstablecimientoResponse>(
                "EXEC pNLS_SolicitudUsuarioEstablecimientoCodigoUnico @0",
                CodigoUnico);
        }

        public async Task<List<EstablecimientoResponse>> ObtenerEstablecimientoSinCodigoUnico()
        {
            using var db = _databaseFactory.GetDatabase();
            return await db.FetchAsync<EstablecimientoResponse>(
                "EXEC pNLS_SolicitudUsuarioEstablecimientoSinCodigoUnico");
        }

        public async Task<int> RegistrarEstablecimiento(EstablecimientoCSV establecimientocsv)
        {
            using var db = _databaseFactory.GetDatabase();
            int IdEstablecimiento = await db.ExecuteScalarAsync<int>
                (
                "EXEC pNLI_SolicitudUsuarioEstablecimiento @0,@1,@2,@3,@4,@5,@6,@7,@8," +
                                                           "@9,@10,@11,@12,@13,@14,@15",
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
                        establecimientocsv.ESTE,
                        establecimientocsv.IMAGEN_3
                    }
                );
            return IdEstablecimiento;
        }
       
        public async Task<int> RegistraCodigoValidacionCorreo(SolicitudUsuarioCorreoValidacion solicitudUsuarioCorreoValidacion)
        {
            using var db = _databaseFactory.GetDatabase();
            var x = await db.InsertAsync(solicitudUsuarioCorreoValidacion);
            return Convert.ToInt32(x);
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

        //public async Task<SolicitudUsuario> RegistrarSolicitudUsuario(SolicitudUsuario solicitudUsuario)
        //{
        //    using var db = _databaseFactory.GetDatabase();
        //    var json = JsonSerializer.Serialize(solicitudUsuario);
        //    return await db.SingleOrDefaultAsync<SolicitudUsuario>
        //        (
        //            "EXEC pNLI_SolicitudUsuario @0",
        //            JsonSerializer.Serialize(solicitudUsuario)
        //        );
        //}

        public async Task<int> RegistrarSolicitud(SolicitudUsuario solicitudUsuario)
        {
            using var db = _databaseFactory.GetDatabase();
            return Convert.ToInt32(await db.InsertAsync(solicitudUsuario));
        }

        public async Task<int> RegistrarSolicitudRol(SolicitudUsuarioRol solicitudUsuarioRol)
        {
            using var db = _databaseFactory.GetDatabase();
            return Convert.ToInt32(await db.InsertAsync(solicitudUsuarioRol));
        }

        public async Task<int> RegistrarSolicitudRolExamen(SolicitudUsuarioRolExamen solicitudUsuarioRolExamen)
        {
            using var db = _databaseFactory.GetDatabase();
            return Convert.ToInt32(await db.InsertAsync(solicitudUsuarioRolExamen));
        }

        public async Task<int> RegistroFormularioPDF(ArchivoInput file)
        {
            using var db = _databaseFactory.GetDatabase();
            return await db.ExecuteAsync(
                "pNLU_UpdateFileSolicitudUsuario @0,@1"
                , file.IdSolicitudUsuario
                , file.archivo);
        }
    }
}
