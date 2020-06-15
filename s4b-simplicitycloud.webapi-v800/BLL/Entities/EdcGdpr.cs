using System;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class EdcGdpr
    {

        public long? EntityId { get; set; }
        public long? UserAccepts { get; set; }
        public DateTime? DateUserAccepts { get; set; }
        public int AcceptsType { get; set; }
        public string NoReason { get; set; }
        public int ContactByPost { get; set; }
        public int ContactByEmail { get; set; }
        public int ContactByPhone { get; set; }
        public int ContactBySms{ get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }

   

}