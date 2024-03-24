using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public partial class CourseRegistration
    {
        public CourseRegistration()
        {
            Uploads = new HashSet<Upload>();
        }

        public int RegistrationId { get; set; }
        public int? CourseId { get; set; }
        public int? UserId { get; set; }
        public DateTime? RegistedTime { get; set; }
        public string? EditedCourseName { get; set; }
        public string? EditedCourseDescription { get; set; }

        public virtual Course? Course { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<Upload> Uploads { get; set; }
    }
}
