using System;

namespace SimplicityOnlineBLL.Entities
{
    public class OrdersMeHeader
    {

        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public string MeUserText1 { get; set; }
        public long? MeUserCombo1 { get; set; }
        public long? MeScheduleCount { get; set; }
        public bool FlgMeFinalised { get; set; }
        public string MeNotes { get; set; }
        public bool FlgMeEnquiry { get; set; }
        public DateTime? DateMeEnquiry { get; set; }
        public bool FlgMeTenderReceived { get; set; }
        public DateTime? DateMeTenderReceived { get; set; }
        public bool FlgMeTenderDue { get; set; }
        public DateTime? DateMeTenderDue { get; set; }
        public bool FlgMeTenderSent { get; set; }
        public DateTime? DateMeTenderSent { get; set; }
        public bool FlgMeApproved { get; set; }
        public DateTime? DateMeApproved { get; set; }
        public bool FlgMeUserDate1 { get; set; }
        public DateTime? DateMeUserDate1 { get; set; }
        public string MeProjectTitle { get; set; }
        public bool FlgSpecShowClient { get; set; }
        public bool FlgSpecShowJobAddress { get; set; }
        public int CategorySequence { get; set; }
        public string SpecNotes { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public RefOrdTenderCategories RefOrdTenderCategories { get; set; }
        public Orders Order { get; set; }

    }
}