using System;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class RefS4bCheckTypes
    {
        public long Sequence { get; set; }
		public bool FlgDeleted { get; set; }
        public int CheckType { get; set; }
        public int CheckId { get; set; }
        public long RowIndex { get; set; }
        public string CheckDesc { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }

	public class S4bQuestionList
	{
		public List<RefS4bCheckTypes> RefS4bCheckTypes { get; set; }
		public List<RefS4bCheckPaymentTypes> RefS4bCheckPaymentTypes { get; set; }

	}
}