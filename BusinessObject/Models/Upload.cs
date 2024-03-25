using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
    public partial class Upload
    {
        public int UploadId { get; set; }
        public int? RegistrationId { get; set; }
        public int? FileId { get; set; }
        public string? UploadName { get; set; }
        public string? UploadDescription { get; set; }
        public DateTime? UploadTime { get; set; }
        public bool? IsHide { get; set; }
        public bool? IsGeneral { get; set; }

        public virtual File? File { get; set; }

        [JsonIgnore]
        public virtual CourseRegistration? Registration { get; set; }
    }
}
