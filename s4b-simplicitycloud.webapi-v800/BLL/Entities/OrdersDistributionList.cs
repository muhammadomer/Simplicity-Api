using System;

namespace SimplicityOnlineBLL.Entities
{
    public class OrdersDistributionList
    {

        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public long? JobSequence { get; set; }
        public string EmailName { get; set; }
        public string EmailAddress { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }


    }
}