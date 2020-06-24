using System;

namespace SimplicityOnlineBLL.Entities
{
    public class S4bCheckLogonAuditFails
    {
        public long Sequence { get; set; }
		public long joinSequence { get; set; }
		public int CheckId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}