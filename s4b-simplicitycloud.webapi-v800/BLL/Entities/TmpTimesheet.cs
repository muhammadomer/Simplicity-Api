using System;

namespace SimplicityOnlineBLL.Entities
{
    public class TmpTimesheet
    {
        public long? Sequence { get; set; }
        public string ImpRef { get; set; }
        public int DataStatus { get; set; }
        public string UncWebSessionId { get; set; }
        public string RowEmployeeName { get; set; }
        public string RowDesc { get; set; }
        public string RowDesc2 { get; set; }
        public string RowDesc3 { get; set; }
        public DateTime? DateRowStartTime { get; set; }
        public DateTime? DateRowFinishTime { get; set; }
        public double RowTimeTotal { get; set; }
        public string RowPymtType { get; set; }
        public string RowNotes { get; set; }
        public DateTime? DateRowDate { get; set; }
        public string RowJobRef { get; set; }
        public bool FlgJobRefValid { get; set; }
        public bool FlgLessBreakTime { get; set; }
        public long? JobSequence { get; set; }
        public bool FlgPayrollEntry { get; set; }
        public string RowAssetName { get; set; }
        public long? EntityId { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }

        public string StartTimeLocation { get; set; }
        public string FinishTimeLocation { get; set; }
        public double DeSequence { get; set; }

        public long? UserId { get; set; }
    }
}