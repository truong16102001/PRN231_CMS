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
    public class CourseRepository : ICourseRepository
    {
        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await CourseDAO.GetListOfCourse();
        }
    }
}
