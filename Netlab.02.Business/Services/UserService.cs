
using Netlab.Domain.Entities;
using Netlab.Domain.Interfaces;
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
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

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
    }
}
