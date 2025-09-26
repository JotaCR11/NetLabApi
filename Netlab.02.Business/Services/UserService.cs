
using Azure;
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
using Netlab.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netlab.Business.Services
{
    public interface IUserService
    {
        Task<List<User>> ObtenerUsuarios(User usuario);
        Task<bool> ExisteLogin(string login);
        Task RegistrarUsuario(User usurio);
        Task EditarUsuario(User usurio);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly EmailService _emailService;

        public UserService(IUserRepository userRepo)
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
        public async Task RegistrarUsuario(User usurio)
        {
            var response = await _userRepo.RegistrarUsuario(usurio);
            if (response.Length > 1)
            {
                string asunto = "Datos de acceso - Netlab 2.0";
                await _emailService.EnviarCorreoAsync(asunto, response);
            }
        }
        public async Task EditarUsuario(User usurio)
        {
            await _userRepo.EditarUsuario(usurio);
            string asunto = "Datos de acceso - Netlab 2.0";
            string mensaje = "Estimado(a) usuario: " + usurio.NOMBRES + " " + usurio.APELLIDOPATERNO + " se renovó su cuenta de usuario.";
            await _emailService.EnviarCorreoAsync(asunto, mensaje);
        }
    }
}
