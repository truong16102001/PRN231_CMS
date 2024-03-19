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
    internal class CourseRegistrationDAO
    {
        public static async Task<IEnumerable<CourseRegistration>> GetListOfRegistration()
        {
            using (var context = new PRN231_DemoCMSContext())
            {
                // Sử dụng Include để tải thông tin của Course và User cho mỗi CourseRegistration
                var list = await context.CourseRegistrations
                    .Include(cr => cr.Course)
                    .Include(cr => cr.User)
                    .ToListAsync();

                return list;
            }
        }

        internal static async Task<bool> RegisterCourse(RegistrationAddUpdateDTO courseRegistration)
        {
            using (var context = new PRN231_DemoCMSContext())
            {
                CourseRegistration pub = new CourseRegistration
                {
                    CourseId= courseRegistration.CourseId,
                    UserId= courseRegistration.UserId,
                    RegistedTime= courseRegistration.RegistedTime,
                    EditedCourseName= courseRegistration.EditedCourseName,
                    EditedCourseDescription= courseRegistration.EditedCourseDescription
                };
                await context.CourseRegistrations.AddAsync(pub);
                return await context.SaveChangesAsync() > 0;
            }
        }

      
    }
}
