using System;

namespace SimplicityOnlineBLL.Entities
{
    public class StockJobReqHeader
    {
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public long? JobSequence { get; set; }
        public bool FlgAuthorised { get; set; }
        public long? AuthorisedBy { get; set; }
        public DateTime? DateAuthorised { get; set; }
        public int PoType { get; set; }
        public bool FlgPoPlaced { get; set; }
        public long? PoSequence { get; set; }
        public string UserField01 { get; set; }
        public string UserField02 { get; set; }
        public string UserField03 { get; set; }
        public string UserField04 { get; set; }
        public string UserField05 { get; set; }
        public string UserField06 { get; set; }
        public string UserField07 { get; set; }
        public string UserField08 { get; set; }
        public string UserField09 { get; set; }
        public string UserField10 { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}