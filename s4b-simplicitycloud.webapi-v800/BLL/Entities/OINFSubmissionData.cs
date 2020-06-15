using System;

namespace SimplicityOnlineBLL.Entities
{
    public class OINFSubmissionData
    {
        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public long? DeSequence { get; set; }
        public DateTime? DatDe { get; set; }
        public string NfsSubmitNo { get; set; }
        public string NfsSubmitTs { get; set; }
        public bool FlgRowIsText { get; set; }
        public double ItemQuantity { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string ItemUnits { get; set; }
        public double AmountUnit { get; set; }
        public double AmountTotal { get; set; }
        public bool FlgThirdParty { get; set; }
        public int IdThirdParty { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}