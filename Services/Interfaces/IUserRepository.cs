using BusinessObject.DTO;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(UserAddEditDTO userEditDTO);
        Task<bool> EmailExistsAsync(string email);
        Task GetUserById(int? userId);
        Task<User> Login(LoginModel req);
        Task<bool> UpdateUserAsync(UserEditDTO userEditDTO);
    }
}
