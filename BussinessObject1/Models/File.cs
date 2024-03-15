using System;
using System.Collections.Generic;

namespace BussinessObject1.Models
{
    public partial class File
    {
        public File()
        {
            Uploads = new HashSet<Upload>();
        }

        public int FileId { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public byte[]? FileContent { get; set; }
        public string? FileUrl { get; set; }

        public virtual ICollection<Upload> Uploads { get; set; }
    }
}
