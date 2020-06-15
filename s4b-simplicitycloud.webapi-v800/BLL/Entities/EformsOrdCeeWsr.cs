using System;

namespace SimplicityOnlineBLL.Entities
{
    public class EformsOrdCeeWsr
    {

        public long? Sequence { get; set; }
        public string FormId { get; set; }
        public string FormSubmissionId { get; set; }
        public string FormTimeStamp { get; set; }
        public long? JobSequence { get; set; }
        public long? RowNo { get; set; }
        public string RowDesc { get; set; }
        public string RowRefNo { get; set; }
        public DateTime? DateRowSampleDate { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}