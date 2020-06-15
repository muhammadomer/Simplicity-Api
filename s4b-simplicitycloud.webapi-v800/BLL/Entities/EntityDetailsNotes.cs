using System;

namespace SimplicityOnlineBLL.Entities
{
    public class EntityDetailsNotes
    {

        public long? Sequence { get; set; }
        public long? EntityId { get; set; }
        public string EntityNotes { get; set; }
        public string UserName { get; set; }
        public string UserLogon { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }

    }
}