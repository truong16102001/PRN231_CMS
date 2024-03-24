using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class UploadAddUpdateDTO
    {
        public int? UploadId { get; set; }
        public int? RegistrationId { get; set; }
        public int? FileId { get; set; }
        public string? UploadName { get; set; }
        public string? UploadDescription { get; set; }
        public DateTime? UploadTime { get; set; }
        public bool? IsHide { get; set; }

    }
}
