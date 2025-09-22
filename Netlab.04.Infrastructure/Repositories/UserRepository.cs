using Azure;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Infrastructure.Database;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseFactory _databaseFactory;

        public UserRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public async Task<List<User>> ObtenerUsuarios(User usuario)
        {
            using var db = _databaseFactory.GetDatabase();
            var response = await db.FetchAsync<User>
                (
                    "EXEC pNLS_Usuario @0, @1, @2, @3, @4",
                    usuario.LOGIN,
                    usuario.APELLIDOPATERNO,
                    usuario.APELLIDOMATERNO,
                    usuario.NOMBRES,
                    usuario.DOCUMENTOIDENTIDAD
                );
            return response.ToList();
        }
        public async Task<int> ExisteLogin(string login)
        {
            using var db = _databaseFactory.GetDatabase();
            var response = await db.SingleAsync<User>
                (
                    "EXEC pNLS_UsuarioByLogin @0",login
                );
            return response.IDUSUARIO;
        }

        public async Task<string> RegistrarUsuario(User usurio)
        {
            using var db = _databaseFactory.GetDatabase();
            string sql = "EXEC pNLI_Usuario @0, @1, @2, @3, @4, @5, @6, @7, @8, @9, ";
            sql += "@10, @11, @12, @13, @14, @15, @16, @17, @18, @19, @20";

            var response = await db.ExecuteScalarAsync<string>
                (
                    sql,
                    usurio.LOGIN,
                    usurio.DOCUMENTOIDENTIDAD,
                    usurio.APELLIDOPATERNO,
                    usurio.APELLIDOMATERNO,
                    usurio.NOMBRES,
                    usurio.CODIGOCOLEGIO,
                    usurio.RNE,
                    usurio.CARGO,
                    usurio.CORREO,
                    usurio.ESTATUS,
                    usurio.IDUSUARIOREGISTRO,
                    usurio.TELEFONOCONTACTO,
                    usurio.TIEMPOCADUCIDAD,
                    usurio.IDPROFESION,
                    usurio.IDTIPOUSUARIO,
                    usurio.IDNIVELAPROBACION,
                    usurio.FIRMADIGITAL,
                    usurio.CONTRASENIA
                );
            return response;
        }
    }
}