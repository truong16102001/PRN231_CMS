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
        internal static async Task<bool> Add(Course newCourse)
        {
            try
            {
                using (var context = new PRN231_DemoCMSContext())
                {
                    // Add the new course to the context
                    context.Courses.Add(newCourse);

                    // Save changes to the database
                    await context.SaveChangesAsync();

                    return context.SaveChanges() > 0; 
                }
            }
            catch (Exception ex)
            {
                // Log the error here
                Console.WriteLine("Error adding course: " + ex.Message);
                return false; // Return false if there's an error adding the course
            }
        }

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
                courses = context.Courses.Include(c => c.Creator).ToList();
            }
            return courses;
        }
    }
}
