using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class Cld_Ord_Labels
    {
        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public string TagNo { get; set; }

        public List<Cld_Ord_Labels_Files> OI_FireProtection_I_Images { get; set; }
    }
}
