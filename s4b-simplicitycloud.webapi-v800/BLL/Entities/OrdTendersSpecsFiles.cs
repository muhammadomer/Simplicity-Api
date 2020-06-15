using System;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class OrdTendersSpecsFiles
    {
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public long? JoinSequence { get; set; }
        public string FileDesc { get; set; }
        public string FileCabId { get; set; }
        public string FileDir { get; set; }
        public string FileName { get; set; }
        public bool FlgUploadComplete { get; set; }
        public long? GuId { get; set; }
        public int VersionNo { get; set; }
        public bool FlgNotLatestVersion { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}