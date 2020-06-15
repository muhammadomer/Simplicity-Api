using System;

namespace SimplicityOnlineBLL.Entities
{
    public class OrderStatusAudit
    {

        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public long? StatusType { get; set; }
        public bool FlgJobClientId { get; set; }
        public long? JobClientId { get; set; }
        public bool FlgStatusRef { get; set; }
        public string StatusRef { get; set; }
        public DateTime? DateStatusRef { get; set; }
        public string StatusRef2 { get; set; }
        public string StatusDesc { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}