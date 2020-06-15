using System;

namespace SimplicityOnlineBLL.Entities
{
    public class StockGroup
    {
        public long? EntityId { get; set; }
        public bool FlgHidden { get; set; }
        public string GroupCode { get; set; }
        public int TreeviewLevel { get; set; }
        public string ParentCode { get; set; }
        public string GroupDesc { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}