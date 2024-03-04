using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Course
    {
        public Course()
        {
            CourseRegistrations = new HashSet<CourseRegistration>();
        }

        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? CourseDescription { get; set; }

        public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; }
    }
}
