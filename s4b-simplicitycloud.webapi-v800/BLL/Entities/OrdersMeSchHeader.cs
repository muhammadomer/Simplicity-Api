using System;

namespace SimplicityOnlineBLL.Entities
{
    public class OrdersMeSchHeader
    {

        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public long? joinSequence { get; set; }
        public bool FlgDeleted { get; set; }
		public bool FlgFinalised { get; set; }
		public string MeVersionNo { get; set; }
        public DateTime? DateMeVersionNo { get; set; }
		public string MeVersionOption { get; set; }
		public string MeVersionNotes { get; set; }
        public int CeeItemCount { get; set; }
        public int PackSequence { get; set; }
        public bool FlgOrdTender { get; set; }
        public DateTime? DateMeApproved { get; set; }
        public bool FlgMeUserDate1 { get; set; }
        public DateTime? DateOrderTender { get; set; }
       
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }

    }
}