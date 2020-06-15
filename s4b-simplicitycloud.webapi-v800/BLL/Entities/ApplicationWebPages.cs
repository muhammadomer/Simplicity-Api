using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class ApplicationWebPages
    {
        public long?  Sequence { get; set; }
        public string PageGeneticLabel { get; set; }
        public string PageCustomizeLabel  { get; set; }
        public string PageUrl { get; set; }
        public bool  IsVisible { get; set; }
        public bool IsToolbar { get; set; }
        public int SortOrder { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? dateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}
