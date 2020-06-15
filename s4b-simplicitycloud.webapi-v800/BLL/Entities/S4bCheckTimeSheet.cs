using System;

namespace SimplicityOnlineBLL.Entities
{
    public class S4bCheckTimeSheet
    {
        public long Sequence { get; set; }
		public bool FlgDeleted { get; set; }
		public long JobSequence { get; set; }
		public long DeSequence { get; set; }
        public int PaymentType { get; set; }
		public DateTime? DateStartDate { get; set; }
		public DateTime? DateStartTime { get; set; }
		public string StartTimeLocation { get; set; }
		public DateTime? DateFinishTime { get; set; }
		public string FinishTimeLocation { get; set; }
		public Double RowTimeTotal { get; set; }
		public string RowNotes { get; set; }
		public bool FlgImported { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}