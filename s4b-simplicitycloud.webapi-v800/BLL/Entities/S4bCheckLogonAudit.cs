using System;

namespace SimplicityOnlineBLL.Entities
{
    public class S4bCheckLogonAudit
    {
        public long Sequence { get; set; }
		public bool FlgPassed { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}