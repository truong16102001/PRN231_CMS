using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces
{
    public interface ICourseRepository
    {
        Task<bool> AddCourse(Course newCourse);
        Task<Course> GetCourseById(int courseId);
        Task<IEnumerable<Course>> GetCourses();


    }
}
