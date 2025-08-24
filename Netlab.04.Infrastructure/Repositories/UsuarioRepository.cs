using Netlab.Domain.DTOs;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Infrastructure.Database;
using NPoco;
using System.Net;

namespace Netlab.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IDatabaseFactory _databaseFactory;

    public UsuarioRepository(IDatabaseFactory databaseFactory)
    {
        _databaseFactory = databaseFactory;
    }

    public Usuario? ObtenerPorLogin(string login)
    {
        using var db = _databaseFactory.GetDatabase();

        var usuario = db.FirstOrDefault<Usuario>("SELECT * FROM Usuario WHERE login = @0 AND estado = 1", login);

        if (usuario == null) return null;

        var roles = db.Fetch<Rol>(@"
                SELECT r.idRol, r.nombre 
                FROM Rol r
                INNER JOIN UsuarioRol ur ON ur.idRol = r.idRol
                WHERE ur.idUsuario = @0 AND r.estado = 1", usuario.IdUsuario);

        usuario.Roles = roles;
        return usuario;
    }

    public async Task<Usuario?> GetByLoginAsync(AuthRequest request)
    {
        using var db = _databaseFactory.GetDatabase();
        //var sql = @"
        //SELECT TOP 1 *
        //FROM Usuario
        //WHERE [login] = @0
        //  AND PWDCOMPARE(@1, contrasenia) = 1";
        return await db.SingleOrDefaultAsync<Usuario>(
            "EXEC pNLS_LoginUsuario @0, @1, @2",
            request.Login,
            request.Password,
            request.IPAddress
        );

        //return await db.SingleOrDefaultAsync<Usuario>(sql, request.Login, request.Password);
    }

    public async Task<List<string>> ObtenerRolesAsync(int idUsuario)
    {
        using var db = _databaseFactory.GetDatabase();

        var sql = @"SELECT R.nombre 
                FROM UsuarioRol UR
                INNER JOIN Rol R ON UR.idRol = R.idRol
                WHERE UR.idUsuario = @0";

        var roles = await db.FetchAsync<string>(sql, idUsuario);

        return roles;
    }
}
