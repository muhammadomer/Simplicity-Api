using System;

namespace SimplicityOnlineBLL.Entities
{
    public class OrderClientAudit
    {

        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public long? JobClientId { get; set; }
        public long? PhaseType { get; set; }
        public bool FlgPhaseRef { get; set; }
        public string PhaseRef { get; set; }
        public bool FlgPhaseStart { get; set; }
        public DateTime? DatePhaseStart { get; set; }
        public string PhaseStartDesc { get; set; }
        public bool FlgPhaseFinish { get; set; }
        public DateTime? DatePhaseFinish { get; set; }
        public string PhaseFinishDesc { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}