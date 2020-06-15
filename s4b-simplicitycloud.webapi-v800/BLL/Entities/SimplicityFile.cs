using System.IO;

namespace SimplicityOnlineBLL.Entities
{
    public class SimplicityFile
    {
        public string FileName { get; set; }
        public bool Isbase64 { get; set; }
        public string Base64String { get; set; }
        public MemoryStream MemStream { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
    }
}
