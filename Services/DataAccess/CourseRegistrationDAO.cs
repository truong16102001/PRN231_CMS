using BusinessObject.DTO;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.DataAccess
{
    internal class CourseRegistrationDAO
    {
        internal static async Task<IEnumerable<CourseRegistration>> GetListOfRegistration()
        {
            var list = new List<CourseRegistration>();
            using (var context = new PRN231_DemoCMSContext())
            {
                list = context.CourseRegistrations.ToList();
            }
            return list ?? new();
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
