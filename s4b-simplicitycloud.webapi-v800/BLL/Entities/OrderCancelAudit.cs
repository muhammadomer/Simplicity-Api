using System.Collections.Generic;
using SimplicityOnlineWebApi.Models;
using System;

namespace SimplicityOnlineBLL.Entities
{
    public class OrderCancelAudit : ResponseModel
    {
        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public string CancelReference { get; set; }
        public string CancelNotes { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? LastAmendedDate { get; set; }
    }
   
}
