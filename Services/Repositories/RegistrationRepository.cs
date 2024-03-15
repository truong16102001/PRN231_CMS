using BusinessObject.DTO;
using BusinessObject.Models;
using Infrastructures.DataAccess;
using Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        public async Task<IEnumerable<CourseRegistration>> GetCourseRegistrations()
        {
            return await CourseRegistrationDAO.GetListOfRegistration();
        }

        public async Task<bool> RegisterCourse(RegistrationAddUpdateDTO courseRegistration)
        {
            return await CourseRegistrationDAO.RegisterCourse(courseRegistration);
        }
    }
}
