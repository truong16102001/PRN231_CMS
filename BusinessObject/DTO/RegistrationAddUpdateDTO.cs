using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class RegistrationAddUpdateDTO
    {
        public int RegistrationId { get; set; }
        public int? CourseId { get; set; }
        public int? UserId { get; set; }
        public DateTime? RegistedTime { get; set; }
        public string? EditedCourseName { get; set; }
        public string? EditedCourseDescription { get; set; }
    }
}
