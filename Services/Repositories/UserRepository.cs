using BusinessObject.DTO;
using BusinessObject.Models;
using Infrastructures.DataAccess;
using Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<bool> AddUserAsync(UserAddEditDTO userEditDTO)
        {
            return await UserDAO.Add(userEditDTO);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await UserDAO.CheckEmailExist(email);
        }

        public async Task GetUserById(int? userId) => await UserDAO.GetUserByUserId(userId);


        public async Task<User> Login(LoginModel req) => await UserDAO.Login(req);

        public async Task<bool> UpdateUserAsync(UserEditDTO userEditDTO)
        {
            return await UserDAO.UpdateProfile(userEditDTO);
        }
    }
}
