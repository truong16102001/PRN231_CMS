using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class File
    {
        public File()
        {
            FileUploads = new HashSet<FileUpload>();
        }

        public int FileId { get; set; }
        public string FileName { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public byte[]? FileContent { get; set; }
        public string? FileUrl { get; set; }

        public virtual ICollection<FileUpload> FileUploads { get; set; }
    }
}
