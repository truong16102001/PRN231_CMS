using BusinessObject.DTO;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface IRegistrationRepository
    {
        Task<IEnumerable<CourseRegistration>> GetCourseRegistrations();
        Task<bool> RegisterCourse(RegistrationAddUpdateDTO courseRegistration);
    }
}
