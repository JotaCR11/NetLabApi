using Azure.Core;
using Netlab.Domain.BusinessObjects.SolicitudUsuario;
using Netlab.Domain.BusinessObjects.Usuario;
using Netlab.Domain.DTOs;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Infrastructure.Database;
using NPoco;
using System.Data;
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
        return await db.FetchAsync<EstablecimientoPerfil>("EXEC pNLS_EstablecimientoPorIdUsuario @0", IdUsuario);
    }

    public async Task<List<Rol>> ObtenerRolesUsuario(int IdUsuario)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.FetchAsync<Rol>("EXEC pNLS_RolesPorIdUsuario @0", IdUsuario);
    }

    public async Task<List<EnfermedadExamen>> ObtenerExamenesUsuario(int IdUsuario)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.FetchAsync<EnfermedadExamen>("EXEC pNLS_ExamenesPorIdUsuario @0", IdUsuario);
    }

    public async Task<List<Menu>> ObtenerMenusUsuario(int IdUsuario)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.FetchAsync<Menu>(
            "EXEC pNLS_MenuItemsByUser @0",
            IdUsuario
        );
    }

    public async Task<List<User>> ObtenerUsuarioPorDocumentoIdentidad(string documentoIdentidad)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.QueryAsync<User>()
            .Where(x => x.DOCUMENTOIDENTIDAD.Equals(documentoIdentidad)).ToList();
    }

    public async Task<UsuarioIndicador?> ObtenerCantidadTotalUsuario()
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.SingleOrDefaultAsync<UsuarioIndicador>(
            "EXEC pNLS_ListaIndicadorCantidadUsuario"
        );
    }

    public async Task<List<UsuarioAtencionOutput>> ObtenerListaAtenciones(UsuarioAtencionInput input)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.FetchAsync<UsuarioAtencionOutput>(
            "EXEC pNLS_ListaHistorialAtencionSolicitudUsuario @0,@1,@2,@3,@4,@5",
            input.texto,
            input.estatus,
            input.ordenamiento,
            input.tamnaño,
            input.pagina,
            input.total
        );
    }

    public async Task<List<UsuarioUbigeoOutput>> ObtenerListaUbigeoUsuario()
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.FetchAsync<UsuarioUbigeoOutput>(
            "EXEC pNLS_ListaUbigeoUsuariosActivos"
        );
    }

    public async Task<List<UsuarioDetalleAtencionOutput>> ObtenerListaDetalleAtenciones(UsuarioAtencionInput input)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.FetchAsync<UsuarioDetalleAtencionOutput>(
            "EXEC pNLS_ListaDetalleAtencionSolicitudUsuario @0,@1,@2,@3,@4,@5",
            input.texto,
            input.estatus,
            input.ordenamiento,
            input.tamnaño,
            input.pagina,
            input.total
        );
    }

    public async Task<UsuarioAprobadoOutput> AprobarSolicitudUsuario(int IdSolicitudUsuario, int IdUsuarioAtencion)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.SingleOrDefaultAsync<UsuarioAprobadoOutput>(
            "EXEC pNLI_AprobarSolicitudUsuario @IdSolicitudUsuario, @IdUsuarioAtencion",
            new { IdSolicitudUsuario, IdUsuarioAtencion }
        );
    }

    public async Task<List<UsuarioAtencionOutput>> ObtenerListaPendienteSolicitudUsuario(UsuarioAtencionInput input)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.FetchAsync<UsuarioAtencionOutput>(
            "EXEC pNLS_ListaPendienteSolicitudUsuario @0,@1,@2,@3,@4,@5",
            input.texto,
            input.estatus,
            input.ordenamiento,
            input.tamnaño,
            input.pagina,
            input.total
        );
    }

    public async Task<List<UsuarioAtencionOutput>> IndicadorAtencionSolicitudUsuario(int anio)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.FetchAsync<UsuarioAtencionOutput>(
            "EXEC pNLS_IndicadorAtencionSolicitudUsuario @0",
            anio
        );
    }

    public async Task<UsuarioRechazadoOutput> RechazarSolicitudUsuario(SolicitudUsuarioRechazo rechazo)
    {
        using var db = _databaseFactory.GetDatabase();
        return await db.SingleOrDefaultAsync<UsuarioRechazadoOutput>(
            "EXEC pNLU_RechazarSolicitudUsuario @0,@1,@2,@3",
            rechazo.IdSolicitudUsuario,
            rechazo.IdMotivo,
            rechazo.Observacion,
            rechazo.IdUsuarioRegistro
        );
    }
}