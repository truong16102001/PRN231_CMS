using BusinessObject.DTO;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.DataAccess
{
    public class UploadDAO
    {
        internal static async Task<bool> Add(Upload newCourse)
        {
            try
            {
                using (var context = new PRN231_DemoCMSContext())
                {
                    // Add the new course to the context
                    context.Uploads.Add(newCourse);

                    // Save changes to the database
                    await context.SaveChangesAsync();

                    return context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the error here
                Console.WriteLine("Error adding upload: " + ex.Message);
                return false; // Return false if there's an error adding the course
            }
        }

        internal static async Task<Upload> GetById(int uploadId)
        {
            try
            {
                using (var context = new PRN231_DemoCMSContext())
                {
                    var upload = await context.Uploads.FirstOrDefaultAsync(u => u.UploadId == uploadId);
                    return upload;
                }
            }
            catch (Exception ex)
            {
                // Log the error here
                Console.WriteLine("Error get upload by id: " + ex.Message);
                throw; // Re-throw the exception to propagate it upwards
            }
        }


        internal static async Task<IEnumerable<Upload>> GetByRegistrationId(int registrationId)
        {
            try
            {
                using (var context = new PRN231_DemoCMSContext())
                {
                    var uploads = await context.Uploads.Where(u => u.RegistrationId == registrationId).ToListAsync();
                    return uploads;
                }
            }
            catch (Exception ex)
            {
                // Log the error here
                Console.WriteLine("Error retrieving uploads: " + ex.Message);
                throw; // Re-throw the exception to propagate it upwards
            }
        }

        internal static async Task<bool> Update(UploadAddUpdateDTO uploadAddUpdateDTO)
        {
            using (var context = new PRN231_DemoCMSContext())
            {
                var existingUser = await context.Uploads.FirstOrDefaultAsync(u => u.UploadId == uploadAddUpdateDTO.UploadId);

                if (existingUser != null)
                {
                    existingUser.RegistrationId = uploadAddUpdateDTO.RegistrationId;
                    existingUser.UploadName = uploadAddUpdateDTO.UploadName;
                    existingUser.UploadDescription = uploadAddUpdateDTO.UploadDescription;
                    existingUser.IsHide = uploadAddUpdateDTO.IsHide;
                    existingUser.UploadTime = DateTime.UtcNow;
                    // Cập nhật thông tin người dùng trong context và lưu vào cơ sở dữ liệu
                    context.Uploads.Update(existingUser);
                    await context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new Exception("Upload not found."); // Hoặc xử lý theo nhu cầu của bạn khi không tìm thấy người dùng
                }
            }
        }
    }
}
