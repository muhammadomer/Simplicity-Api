using System;

namespace SimplicityOnlineBLL.Entities
{
    public class OrdersNotes
    {

        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public string OrderNotes { get; set; }
        public string UserName { get; set; }
        public string UserLogon { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }

    }
}