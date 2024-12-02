using RevOnRental.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevOnRental.Domain.Models
{
    public class BusinessDocument
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] FileContent { get; set; } // Store file content as byte array
        public DateTime UploadedDate { get; set; }
        public virtual Business Business { get; set; }

    }
}
