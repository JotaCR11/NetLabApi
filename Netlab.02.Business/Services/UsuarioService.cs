
using Azure;
using Microsoft.AspNetCore.WebUtilities;
using Netlab.Domain.BusinessObjects.SolicitudUsuario;
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
using System.Windows.Markup;

namespace Netlab.Business.Services
{
    public interface IUsuarioService
    {
        Task<LoginResponse> LoginUsuario(AuthRequest login);
        Task<UsuarioIndicador?> ObtenerCantidadTotalUsuario();
        Task<List<UsuarioAtencionOutput>> ObtenerListaAtenciones(UsuarioAtencionInput input);
        Task<List<UsuarioUbigeoOutput>> ObtenerListaUbigeoUsuario();
        Task<List<UsuarioDetalleAtencionOutput>> ObtenerListaDetalleAtenciones(UsuarioAtencionInput input);
        Task<UsuarioAprobadoOutput> AprobarSolicitudUsuario(int IdSolicitudUsuario);
    }
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _userRepo;
        private readonly IEmailService _emailService;
        //private readonly AuthService _authService;

        public UsuarioService(IUsuarioRepository userRepo, IEmailService emailService)
        {
            _userRepo = userRepo;
            _emailService = emailService;
        }

        public async Task<LoginResponse> LoginUsuario(AuthRequest login)
        {
            var usuario = await _userRepo.GetByLoginAsync(login);
            var roles = new List<Rol>();
            var examenes = new List<EnfermedadExamen>();
            var establecimientos = new List<EstablecimientoPerfil>();
            var token = string.Empty;
            LoginResponse loginResponse = new LoginResponse();

            if (usuario.Respuesta == 6)
            {
                establecimientos = await _userRepo.ObtenerEstablecimientoUsuario(usuario.IdUsuario);

                foreach (var item in establecimientos)
                {
                    item.ROLES = new List<Rol>();
                    item.ROLES = await _userRepo.ObtenerRolesUsuario(usuario.IdUsuario);
                    item.EXAMENES = new List<EnfermedadExamen>();
                    item.EXAMENES = await _userRepo.ObtenerExamenesUsuario(usuario.IdUsuario);
                    var menuList = new List<Menu>();
                    menuList = await _userRepo.ObtenerMenusUsuario(usuario.IdUsuario);
                    item.MENUS = new List<Menu>();
                    item.MENUS = ConstruirJerarquia(menuList);
                }
                loginResponse = new LoginResponse()
                {
                    TOKEN = token,
                    USUARIO = usuario,
                    ESTABLECIMIENTOS = establecimientos
                };
            }

            return loginResponse;
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

        public async Task<UsuarioAprobadoOutput> AprobarSolicitudUsuario(int IdSolicitudUsuario)
        {
            bool exito = false;
            string error = string.Empty;

            var response = await _userRepo.AprobarSolicitudUsuario(IdSolicitudUsuario,190);
            string archivo = (response.TipoSolicitud == 1) ? "CreacionSolicitudUsuario.html" : "RenovacionSolicitudUsuario.html";
            var correo = new DatosCorreo()
            {
                Archivo = archivo,
                NombreDestino = response.NombreUsuario,
                Login = response.Login,
                Password = response.Password
            };
            (exito, error) = await _emailService.EnviarCorreoAsync("Datos de acceso - netlab 2.0", await PlantillaCorreo(correo), response.CorreoElectronico);
            return response;
        }

        public async Task<string> PlantillaCorreo(DatosCorreo correo)
        {
            string html = await EmailService.CargarPlantillaAsync(
                correo.Archivo,
                new Dictionary<string, string>
                {
                    { "NombreDestino", correo.NombreDestino },
                    { "Login", correo.Login },
                    { "Password", correo.Password }
                }
            );
            return html;
        }
    }
}
