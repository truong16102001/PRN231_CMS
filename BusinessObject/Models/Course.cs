using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        public string? Image { get; set; }
        public int? CreatorId { get; set; }
        public DateTime? CreatedTime { get; set; }

        public virtual User? Creator { get; set; }
        [JsonIgnore]
        public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; }
    }
}
