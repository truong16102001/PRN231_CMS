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
    }
}
