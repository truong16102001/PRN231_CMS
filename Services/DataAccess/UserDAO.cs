using BusinessObject.DTO;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.DataAccess
{
    public class UserDAO
    {
        public static async Task<User> Login(LoginModel req)
        {
            User? account;
            using (var context = new PRN231_DemoCMSContext())
            {
                account = await context.Users.Where(a => a.Email!.Equals(req.Email) && a.Password!.Equals(req.Password)).FirstOrDefaultAsync();
            }
            return account ?? new();
        }

        internal static async Task<User> GetUserByUserId(int? userId)
        {
            User? account;
            using (var context = new PRN231_DemoCMSContext())
            {
                account = await context.Users.Where(a => a.UserId == userId).FirstOrDefaultAsync();
            }

            return account ?? new();
        }
    }
}
