using System;

namespace SimplicityOnlineBLL.Entities
{
    public class Cld_Ord_Labels_Files
    {
        public long? Sequence { get; set; }
        public long? JobSequence { get; set; } // un_orders sequence
        public int OiSequence { get; set; } // un_order_items sequence
        public int HeaderSequence { get; set; }
        public long? JoinSequence { get; set; } //OI_FireProtection_I Sequence
        public bool FlgDeleted { get; set; }
        public string FileNameAndPath { get; set; }
        public DateTime? FileDate { get; set; }
        public string FileDesc { get; set; }
        public string ImageURL { get; set; }
        public string LogoURL { get; set; }        
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public bool FlgIsBase64Img { get; set; }
        public string Base64Img { get; set; }
        public string ImageName { get; set; }
        public string ImageUser { get; set; }

        public long? AddInfoSequence { get; set; }
        public string AddInfo { get; set; }
        public string FolderNames { get; set; }
        public string DriveFileId { get; set; }
    }
}
