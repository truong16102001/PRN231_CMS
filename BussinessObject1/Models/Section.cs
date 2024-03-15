using System;
using System.Collections.Generic;

namespace BussinessObject1.Models
{
    public partial class Section
    {
        public Section()
        {
            Uploads = new HashSet<Upload>();
        }

        public int SectionId { get; set; }
        public int? RegistrationId { get; set; }
        public string? SectionName { get; set; }
        public string? SectionDescription { get; set; }
        public int? SectionOrder { get; set; }

        public virtual CourseRegistration? Registration { get; set; }
        public virtual ICollection<Upload> Uploads { get; set; }
    }
}
