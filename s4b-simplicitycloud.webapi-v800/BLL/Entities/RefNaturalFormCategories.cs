using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class RefNaturalFormCategories
    {
        public long? CategorySequence { get; set; }
        public bool FlgDeleted { get; set; }
        public long? RowIndex { get; set; }
        public bool FlgCompulsory { get; set; }
        public string CategoryDesc { get; set; }
        public string HyperlinkText { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}
