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

        internal static async Task<bool> Add(UserAddEditDTO userEditDTO)
        {
            User user = new User
            {
                FullName = userEditDTO.FullName,
                Email = userEditDTO.Email,
                Password = userEditDTO.Password,
                Role = "teacher"
            };
            try
            {
                using (var context = new PRN231_DemoCMSContext())
                {
                    // Add the new course to the context
                    context.Users.Add(user);

                    return await context.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the error here
                Console.WriteLine("Error adding user: " + ex.Message);
                return false; // Return false if there's an error adding the course
            }
        }

        internal static async Task<bool> CheckEmailExist(string email)
        {
            using (var context = new PRN231_DemoCMSContext())
            {
                // Kiểm tra xem có bất kỳ người dùng nào có địa chỉ email như vậy trong cơ sở dữ liệu hay không
                bool emailExists = await context.Users.AnyAsync(a => a.Email == email);

                return emailExists;
            }
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

        internal static async Task<bool> UpdateProfile(UserEditDTO userEditDTO)
        {
            using (var context = new PRN231_DemoCMSContext())
            {
                // Kiểm tra xem email mới đã tồn tại trong cơ sở dữ liệu chưa
                var emailExists = await context.Users.AnyAsync(u => u.Email == userEditDTO.Email && u.UserId != userEditDTO.UserId);

                if (emailExists)
                {
                    return false;
                }

                // Tiếp tục cập nhật thông tin người dùng nếu không có vấn đề với email
                var existingUser = await context.Users.FirstOrDefaultAsync(u => u.UserId == userEditDTO.UserId);

                if (existingUser != null)
                {
                    existingUser.FullName = userEditDTO.FullName;
                    existingUser.Email = userEditDTO.Email;

                    // Cập nhật thông tin người dùng trong context và lưu vào cơ sở dữ liệu
                    context.Users.Update(existingUser);
                    await context.SaveChangesAsync();
                    return true;

                }
                else
                {
                    throw new Exception("User not found."); // Hoặc xử lý theo nhu cầu của bạn khi không tìm thấy người dùng
                }
            }
        }
    }
}
