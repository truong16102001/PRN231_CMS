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

namespace Infrastructures.Services
{
    public class UserRepository : IUserRepository
    {
        public async Task GetUserById(int? userId) => await UserDAO.GetUserByUserId(userId);
       

        public async Task<User> Login(LoginModel req) => await UserDAO.Login(req);

    }
}
