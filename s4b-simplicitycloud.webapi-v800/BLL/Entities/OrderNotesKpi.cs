using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineBLL.Entities
{
    public class OrderNotesKpi
    {
        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public string KpiNotes { get; set; }
        public string UserName { get; set; }
        public string UserLogOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}
