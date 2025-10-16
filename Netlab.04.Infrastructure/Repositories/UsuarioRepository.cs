using Azure.Core;
using Netlab.Domain.DTOs;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Infrastructure.Database;
using NPoco;
using System.Net;
using System.Text;

namespace Netlab.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IDatabaseFactory _databaseFactory;

    public UsuarioRepository(IDatabaseFactory databaseFactory)
    {
        _databaseFactory = databaseFactory;
    }

    public async Task<Usuario?> GetByLoginAsync(AuthRequest request)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.SingleOrDefaultAsync<Usuario>(
            "EXEC pNLS_LoginUsuario @0, @1, @2",
            request.Login,
            request.Password,
            request.IPAddress
        );
    }

    public async Task<List<EstablecimientoPerfil>> ObtenerEstablecimientoUsuario(int IdUsuario)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.FetchAsync<EstablecimientoPerfil>(@"
                SELECT e.idEstablecimiento, e.nombre 
                FROM Establecimiento e
                INNER JOIN UsuarioEstablecimiento ue ON e.idEstablecimiento = ue.idEstablecimiento
                WHERE ue.idUsuario = @0 and ue.estado = 1 AND e.estado = 1", IdUsuario);
    }

    public async Task<List<Rol>> ObtenerRolesUsuario(int IdUsuario)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.FetchAsync<Rol>(@"
                SELECT r.idRol, r.nombre 
                FROM Rol r
                INNER JOIN UsuarioRol ur ON ur.idRol = r.idRol
                WHERE ur.idUsuario = @0 AND r.estado = 1", IdUsuario);
    }

    public async Task<List<Examen>> ObtenerExamenesUsuario(int IdUsuario)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.FetchAsync<Examen>(@"
                SELECT ex.idExamen, ex.nombre, uee.idTipo 
                FROM Examen ex
                INNER JOIN UsuarioEnfermedadExamen uee ON ex.idExamen = uee.idExamen
                WHERE uee.idUsuario = @0 and uee.estado = 1 AND ex.estado = 1", IdUsuario);
    }

    public async Task<List<Menu>> ObtenerMenusUsuario(int IdUsuario)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.FetchAsync<Menu>(
            "EXEC pNLS_MenuItemsByUser @0",
            IdUsuario
        );
    }






    //public Usuario? ObtenerPorLogin(string login)
    //{
    //    using var db = _databaseFactory.GetDatabase();

    //    var usuario = db.FirstOrDefault<Usuario>("SELECT * FROM Usuario WHERE login = @0 AND estado = 1", login);

    //    if (usuario == null) return null;

    //    var roles = db.Fetch<Rol>(@"
    //            SELECT r.idRol, r.nombre 
    //            FROM Rol r
    //            INNER JOIN UsuarioRol ur ON ur.idRol = r.idRol
    //            WHERE ur.idUsuario = @0 AND r.estado = 1", usuario.IdUsuario);

    //    usuario.Roles = roles;
    //    return usuario;
    //}





    //public async Task<List<User>> ObtenerUsuarios(User usuario)
    //{
    //    using var db = _databaseFactory.GetDatabase();
    //    var response = await db.FetchAsync<User>
    //        (
    //            "EXEC pNLS_Usuario @0, @1, @2, @3, @4",
    //            usuario.LOGIN,
    //            usuario.APELLIDOPATERNO,
    //            usuario.APELLIDOMATERNO,
    //            usuario.NOMBRES,
    //            usuario.DOCUMENTOIDENTIDAD
    //        );
    //    return response.ToList();
    //}
    //public async Task<int> ExisteLogin(string login)
    //{
    //    using var db = _databaseFactory.GetDatabase();
    //    var response = await db.SingleAsync<User>
    //        (
    //            "EXEC pNLS_UsuarioByLogin @0", login
    //        );
    //    return response.IDUSUARIO;
    //}

    //public async Task<string> RegistrarUsuario(User usurio)
    //{
    //    using var db = _databaseFactory.GetDatabase();
    //    string sql = "EXEC pNLI_Usuario @0, @1, @2, @3, @4, @5, @6, @7, @8, @9, ";
    //    sql += "@10, @11, @12, @13, @14, @15, @16, @17, @18, @19";

    //    var response = await db.ExecuteScalarAsync<string>
    //        (
    //            sql,
    //            new object[] {
    //                                usurio.LOGIN,
    //                                usurio.DOCUMENTOIDENTIDAD,
    //                                usurio.APELLIDOPATERNO,
    //                                usurio.APELLIDOMATERNO,
    //                                usurio.NOMBRES,
    //                                usurio.CODIGOCOLEGIO,
    //                                usurio.RNE,
    //                                usurio.CARGO,
    //                                usurio.CORREO,
    //                                usurio.ESTATUS,
    //                                usurio.IDUSUARIOREGISTRO,
    //                                usurio.TELEFONOCONTACTO,
    //                                usurio.TIEMPOCADUCIDAD,
    //                                usurio.IDPROFESION,
    //                                usurio.IDTIPOUSUARIO,
    //                                usurio.IDCOMPONENTE,
    //                                usurio.IDTIPOACCESO,
    //                                usurio.IDNIVELAPROBACION,
    //                                usurio.FIRMADIGITAL,
    //                                usurio.CONTRASENIA
    //            }
    //        );
    //    return response;
    //}

    //public async Task EditarUsuario(User usurio)
    //{
    //    using var db = _databaseFactory.GetDatabase();
    //    string sql = "EXEC pNLU_Usuario @0, @1, @2, @3, @4, @5, @6, @7, @8, @9, ";
    //    sql += "@10, @11, @12, @13, @14, @15, @16, @17, @18, @19, @20, @21";

    //    var response = await db.ExecuteAsync
    //        (
    //            sql,
    //            new object[] {
    //                                usurio.LOGIN,
    //                                usurio.DOCUMENTOIDENTIDAD,
    //                                usurio.APELLIDOPATERNO,
    //                                usurio.APELLIDOMATERNO,
    //                                usurio.NOMBRES,
    //                                usurio.CODIGOCOLEGIO,
    //                                usurio.RNE,
    //                                usurio.CARGO,
    //                                usurio.CORREO,
    //                                usurio.ESTATUS,
    //                                usurio.IDUSUARIOREGISTRO,
    //                                usurio.TELEFONOCONTACTO,
    //                                usurio.TIEMPOCADUCIDAD,
    //                                usurio.IDPROFESION,
    //                                usurio.IDTIPOUSUARIO,
    //                                usurio.IDCOMPONENTE,
    //                                usurio.IDTIPOACCESO,
    //                                usurio.IDNIVELAPROBACION,
    //                                usurio.FIRMADIGITAL,
    //                                usurio.CONTRASENIA
    //            }
    //        );
    //}

    //public async Task<User> ObtenerUsuario(int IdUsuario)
    //{
    //    using var db = _databaseFactory.GetDatabase();
    //    return await db.Query<User>()
    //                    .Where(x => x.IDUSUARIO == IdUsuario).FirstOrDefaultAsync();
    //}







    //public async Task<int> RegistrarUsuario(User usuario)
    //{
    //    using var db = _databaseFactory.GetDatabase();
    //    await db.InsertAsync(usuario);
    //    return usuario.IDUSUARIO;
    //}
}