
using Azure;
using Microsoft.AspNetCore.WebUtilities;
using Netlab.Domain.DTOs;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Business.Services
{
    public interface IUsuarioService
    {
        Task<List<User>> ObtenerUsuarios(User usuario);
        Task<bool> ExisteLogin(string login);
        //Task RegistrarUsuario(User usurio);
        Task EditarUsuario(User usurio);
        Task<User> ObtenerPerfilUsuario(int IdUsuario);
        Task<bool> ValidaLogin(AuthRequest login);
    }
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _userRepo;
        private readonly EmailService _emailService;

        public UsuarioService(IUsuarioRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<List<User>> ObtenerUsuarios(User usuario)
        {
            return await _userRepo.ObtenerUsuarios(usuario);
        }
        public async Task<bool> ExisteLogin(string login)
        {
            var response = await _userRepo.ExisteLogin(login);
            return (response > 0) ? true : false;
        }
        //public async Task RegistrarUsuario(User usurio)
        //{
        //    var response = await _userRepo.RegistrarUsuario(usurio);
        //    if (response.Length > 1)
        //    {
        //        string asunto = "Datos de acceso - Netlab 2.0";
        //        await _emailService.EnviarCorreoAsync(asunto, response);
        //    }
        //}
        public async Task EditarUsuario(User usurio)
        {
            await _userRepo.EditarUsuario(usurio);
            string asunto = "Datos de acceso - Netlab 2.0";
            string mensaje = "Estimado(a) usuario: " + usurio.NOMBRES + " " + usurio.APELLIDOPATERNO + " se renovó su cuenta de usuario.";
            await _emailService.EnviarCorreoAsync(asunto, mensaje);
        }

        public async Task<User> ObtenerPerfilUsuario(int IdUsuario)
        {
            var usuario = await _userRepo.ObtenerUsuario(IdUsuario);
            var rol = await _userRepo.ObtenerRolesUsuario(IdUsuario);
            var examen = await _userRepo.ObtenerExamenesUsuario(IdUsuario);
            var establecimiento = await _userRepo.ObtenerEstablecimientoUsuario(IdUsuario);
            usuario.PERFILUSUARIO = new List<Perfil>();

            for (int i = 0; i < establecimiento.Count; i++)
            {
                var perfil = new Perfil
                {
                    idEstablecimiento = establecimiento[i].IDESTABLECIMIENTO,
                    rol = new List<Rol>(),
                    examen = new List<Examen>()
                };
                for (int r = 0; r < rol.Count; r++)
                {
                    perfil.rol.Add(new Rol
                    {
                        IdRol = rol[r].IdRol,
                        Nombre = rol[r].Nombre
                    });
                }
                for (int e = 0; e < examen.Count; e++)
                {
                    perfil.examen.Add(new Examen
                    {
                        IdExamen = examen[e].IdExamen,
                        Nombre = examen[e].Nombre
                    });
                }
                usuario.PERFILUSUARIO.Add(perfil);
            }
            return usuario;        
        }

        public async Task<bool> ValidaLogin(AuthRequest login)
        {
            var inputBytes = Encoding.UTF8.GetBytes(login.Password);
            var hashBytes = System.Security.Cryptography.SHA256.HashData(inputBytes);
            var _login = new loginInput()
            {
                login = login.Login,
                password = hashBytes
            };
            var validalogin = false;
            var response = await _userRepo.ValidaLogin(_login);
            if (response != null)
            {
                validalogin = true;
            }
            return validalogin;
        }
    }
}
