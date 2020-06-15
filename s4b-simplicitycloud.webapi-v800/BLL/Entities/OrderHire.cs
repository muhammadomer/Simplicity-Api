using System;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class OrderHire
    {
        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public int HireType { get; set; }
        public string JobRef { get; set; }
        public string JobAddress { get; set; }
        public string JobCostCenter { get; set; }
        public long? AssetSequence { get; set; }
        public long? POISequence { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public double ItemQuantity { get; set; }
        public string ContractRef { get; set; }
        public bool FlgChargeable { get; set; }
        public int RateType { get; set; }
        public DateTime? DateHireStart { get; set; }
        public DateTime? DateHireEnd { get; set; }
        public bool FlgHalfDay { get; set; }
        public long? NumberOfDays { get; set; }
        public long? NumberOfWeeks { get; set; }
        public double TotalDays { get; set; }
        public bool FlgRretruned { get; set; }
        public DateTime? DateReturned { get; set; }
        public string EndRreferenece { get; set; }
        public bool FlgEextendHire { get; set; }
        public string ExtendHireRef { get; set; }
        public bool FlgDamaged { get; set; }
        public DateTime? DateDamaged { get; set; }
        public int? DamageType { get; set; }
        public string DamageTypeDesc { get; set; }
        public double HireDayRate { get; set; }
        public double HireTotal { get; set; }
        public double HireTfrCostsTotal { get; set; }
        public string HireNotes { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public bool FlgDeleted { get; set; }
        public long? AssetId { get; set; }
        public long? ItemModel { get; set; }
        public string AssetCategory { get; set; }
        public string ItemDesc { get; set; }
        public string SupplierName { get; set; }
        public string SupplierPORef { get; set; }
        public string SupplierAddress { get; set; }
        public string Location { get; set; }


    }

    
}