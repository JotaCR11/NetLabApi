
using Azure;
using Microsoft.AspNetCore.WebUtilities;
using Netlab.Domain.BusinessObjects.Usuario;
using Netlab.Domain.DTOs;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Netlab.Business.Services
{
    public interface IUsuarioService
    {
        Task<LoginResponse> LoginUsuario(AuthRequest login);
        Task<UsuarioIndicador?> ObtenerCantidadTotalUsuario();
        Task<List<UsuarioAtencionOutput>> ObtenerListaAtenciones(UsuarioAtencionInput input);
        Task<List<UsuarioUbigeoOutput>> ObtenerListaUbigeoUsuario();
        Task<List<UsuarioDetalleAtencionOutput>> ObtenerListaDetalleAtenciones(UsuarioAtencionInput input);
        //Task<List<User>> ObtenerUsuarios(User usuario);
        //Task<bool> ExisteLogin(string login);
        ////Task RegistrarUsuario(User usurio);
        //Task EditarUsuario(User usurio);
        //Task<User> ObtenerPerfilUsuario(int IdUsuario);

    }
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _userRepo;
        //private readonly EmailService _emailService;
        //private readonly AuthService _authService;

        public UsuarioService(IUsuarioRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<LoginResponse> LoginUsuario(AuthRequest login)
        {
            var usuario = await _userRepo.GetByLoginAsync(login);
            var roles = new List<Rol>();
            var examenes = new List<Examen>();
            var establecimientos = new List<EstablecimientoPerfil>();
            var token = string.Empty;

            if (usuario != null)
            {
                establecimientos = await _userRepo.ObtenerEstablecimientoUsuario(usuario.IdUsuario);

                foreach (var item in establecimientos)
                {
                    item.ROLES = new List<Rol>();
                    item.ROLES = await _userRepo.ObtenerRolesUsuario(usuario.IdUsuario);
                    item.EXAMENES = new List<Examen>();
                    item.EXAMENES = await _userRepo.ObtenerExamenesUsuario(usuario.IdUsuario);
                    var menuList = new List<Menu>();
                    menuList = await _userRepo.ObtenerMenusUsuario(usuario.IdUsuario);
                    item.MENUS = new List<Menu>();
                    item.MENUS = ConstruirJerarquia(menuList);
                }
            }
            return new LoginResponse
            {
                TOKEN = token,
                USUARIO = usuario,
                ESTABLECIMIENTOS = establecimientos
            };
        }

        public static List<Menu> ConstruirJerarquia(List<Menu> menus)
        {
            // Clonar todos los elementos para evitar referencias compartidas
            var todos = menus
                .Select(m => new Menu
                {
                    IdMenu = m.IdMenu,
                    Nombre = m.Nombre,
                    IdMenuPadre = m.IdMenuPadre,
                    URL = m.URL,
                    Hijos = new List<Menu>()
                })
                .ToList();

            // Crear un diccionario para rápido acceso por id
            var mapa = todos.ToDictionary(x => x.IdMenu);

            // Lista de raíces (padres con idMenuPadre = "0")
            var raices = new List<Menu>();

            foreach (var menu in todos)
            {
                if (menu.IdMenuPadre == 0 || !mapa.ContainsKey(menu.IdMenuPadre))
                {
                    raices.Add(menu);
                }
                else
                {
                    // Agregar este menú como hijo del padre correspondiente
                    mapa[menu.IdMenuPadre].Hijos.Add(menu);
                }
            }
            return raices;
        }

        public async Task<UsuarioIndicador?> ObtenerCantidadTotalUsuario()
        {
            return await _userRepo.ObtenerCantidadTotalUsuario();
        }

        public async Task<List<UsuarioAtencionOutput>> ObtenerListaAtenciones(UsuarioAtencionInput input)
        {
            return await _userRepo.ObtenerListaAtenciones(input);
        }

        public async Task<List<UsuarioUbigeoOutput>> ObtenerListaUbigeoUsuario()
        {
            return await _userRepo.ObtenerListaUbigeoUsuario();
        }

        public async Task<List<UsuarioDetalleAtencionOutput>> ObtenerListaDetalleAtenciones(UsuarioAtencionInput input)
        {
            return await _userRepo.ObtenerListaDetalleAtenciones(input);
        }


        //public async Task<List<User>> ObtenerUsuarios(User usuario)
        //{
        //    return await _userRepo.ObtenerUsuarios(usuario);
        //}
        //public async Task<bool> ExisteLogin(string login)
        //{
        //    var response = await _userRepo.ExisteLogin(login);
        //    return (response > 0) ? true : false;
        //}
        //public async Task RegistrarUsuario(User usurio)
        //{
        //    var response = await _userRepo.RegistrarUsuario(usurio);
        //    if (response.Length > 1)
        //    {
        //        string asunto = "Datos de acceso - Netlab 2.0";
        //        await _emailService.EnviarCorreoAsync(asunto, response);
        //    }
        //}
        //public async Task EditarUsuario(User usurio)
        //{
        //    await _userRepo.EditarUsuario(usurio);
        //    string asunto = "Datos de acceso - Netlab 2.0";
        //    string mensaje = "Estimado(a) usuario: " + usurio.NOMBRES + " " + usurio.APELLIDOPATERNO + " se renovó su cuenta de usuario.";
        //    await _emailService.EnviarCorreoAsync(asunto, mensaje);
        //}

        //public async Task<User> ObtenerPerfilUsuario(int IdUsuario)
        //{
        //    var usuario = await _userRepo.ObtenerUsuario(IdUsuario);
        //    var rol = await _userRepo.ObtenerRolesUsuario(IdUsuario);
        //    var examen = await _userRepo.ObtenerExamenesUsuario(IdUsuario);
        //    var establecimiento = await _userRepo.ObtenerEstablecimientoUsuario(IdUsuario);
        //    usuario.PERFILUSUARIO = new List<Perfil>();

        //    for (int i = 0; i < establecimiento.Count; i++)
        //    {
        //        var perfil = new Perfil
        //        {
        //            idEstablecimiento = establecimiento[i].IDESTABLECIMIENTO,
        //            rol = new List<Rol>(),
        //            examen = new List<Examen>()
        //        };
        //        for (int r = 0; r < rol.Count; r++)
        //        {
        //            perfil.rol.Add(new Rol
        //            {
        //                IdRol = rol[r].IdRol,
        //                Nombre = rol[r].Nombre
        //            });
        //        }
        //        for (int e = 0; e < examen.Count; e++)
        //        {
        //            perfil.examen.Add(new Examen
        //            {
        //                IdExamen = examen[e].IdExamen,
        //                Nombre = examen[e].Nombre
        //            });
        //        }
        //        usuario.PERFILUSUARIO.Add(perfil);
        //    }
        //    return usuario;        
        //}


    }
}
