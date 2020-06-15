using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class RefNaturalForm
    {
        public long? FormSequence { get; set; }
        public bool FlgDeleted { get; set; }
        public bool FlgDefault { get; set; }
        public int DefaultId { get; set; }
        public bool FlgPreferred { get; set; }
        public long? RowIndex { get; set; }
        public string FormId { get; set; }
        public string FormDesc { get; set; }
        public long? CategorySequence { get; set; }
        public string CategoryDesc { get; set; }
        public bool FlgClientSpecific { get; set; }
        public long? ClientId { get; set; }
        public RefNaturalFormCategories RefNaturalformCategory { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}
