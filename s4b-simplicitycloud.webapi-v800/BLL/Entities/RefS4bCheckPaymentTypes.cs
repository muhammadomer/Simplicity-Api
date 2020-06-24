using System;

namespace SimplicityOnlineBLL.Entities
{
    public class RefS4bCheckPaymentTypes
    {
        public int PaymentType { get; set; }
		public bool FlgDeleted { get; set; }
        public int RowIndex { get; set; }
        public string PaymentDesc { get; set; }
		public bool FlgStart { get; set; }
		public bool FlgEnd { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}