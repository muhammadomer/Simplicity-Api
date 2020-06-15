using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.Models
{
    public class DocumentUploadModel
    {
        public string FileName { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
        public string FileNameCabId { get; set; }
    }
}
