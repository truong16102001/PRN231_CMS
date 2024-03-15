using System;
using System.Collections.Generic;

namespace BussinessObject1.Models
{
    public partial class Upload
    {
        public int UploadId { get; set; }
        public int? SectionId { get; set; }
        public int? FileId { get; set; }
        public string? UploadName { get; set; }
        public string? UploadDescription { get; set; }
        public DateTime? UploadTime { get; set; }
        public bool? IsHide { get; set; }

        public virtual File? File { get; set; }
        public virtual Section? Section { get; set; }
    }
}
