using System;

namespace SimplicityOnlineBLL.Entities
{
    public class AttfOrdDocsMasters
    {
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public bool FlgHide { get; set; }
        public long? JobSequence { get; set; }
        public string FileName { get; set; }
        public string FileVersionNo { get; set; }
        public string DateFileVersionNo { get; set; }
        public string FileVersionOption { get; set; }
        public bool FlgFileVo { get; set; }
        public string FileNotes { get; set; }
        public string FilePathAndName { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }

    public class AttfOrdDocsMastersFile
    {
        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public SimplicityFile OrderDocFile { get; set; }
    }

}