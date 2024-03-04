using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Section
    {
        public Section()
        {
            FileUploads = new HashSet<FileUpload>();
        }

        public int SectionId { get; set; }
        public int? RegistrationId { get; set; }
        public string? SectionName { get; set; }
        public string? SectionDescription { get; set; }
        public int? SectionOrder { get; set; }

        public virtual CourseRegistration? Registration { get; set; }
        public virtual ICollection<FileUpload> FileUploads { get; set; }
    }
}
