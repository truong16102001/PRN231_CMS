using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.DataAccess
{
    public class CourseDAO
    {
        internal static async Task<Course> GetById(int courseId)
        {
            using (var context = new PRN231_DemoCMSContext())
            {
                var course = await context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);
                return course;
            }
        }



        internal static async Task<IEnumerable<Course>> GetListOfCourse()
        {
            var courses = new List<Course>();
            using (var context = new PRN231_DemoCMSContext())
            {
                courses = context.Courses.ToList();
            }
            return courses;
        }
    }
}
