using System;
using System.Collections.Generic;
namespace SimplicityOnlineBLL.Entities
{
    public class S4bCheckAudit
    {
        public long Sequence { get; set; }
        public long CheckType { get; set; }
		public bool FlgPassed { get; set; }
		public long JobSequence { get; set; }
        public long DeSequence { get; set; }
		public bool FlgSelfIsolation { get; set; }
		public DateTime? DateSelfIsolation { get; set; }
		public string SelfIsolationNotes { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
		public List<S4bCheckAuditFails> S4bCheckAuditFails { get; set; }
	}
}