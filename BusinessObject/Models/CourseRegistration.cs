using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class CourseRegistration
    {
        public CourseRegistration()
        {
            Sections = new HashSet<Section>();
        }

        public int RegistrationId { get; set; }
        public int? CourseId { get; set; }
        public int? UserId { get; set; }
        public DateTime? RegistedTime { get; set; }
        public string? EditedCourseName { get; set; }
        public string? EditedCourseDescription { get; set; }

        public virtual Course? Course { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Section> Sections { get; set; }
    }
}
