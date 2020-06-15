using System;

namespace SimplicityOnlineBLL.Entities
{
    public class OrdersMeSchItems
    {

        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public long? joinSequence { get; set; }
		public long? ClonedFromSeq { get; set; }
		public int RowIndex { get; set; }
        public bool FlgRowLocked { get; set; }
		public bool FlgRowSelected { get; set; }
		public bool FlgRowIsText { get; set; }
		public int ItemType { get; set; }
		public long? AssetSequence { get; set; }
		public bool FlgRowIsSubtotal { get; set; }
		public int GroupId { get; set; }
		public string TransType { get; set; }
		public string ItemCode { get; set; }
		public string ItemDesc { get; set; }
		public string ItemUnits { get; set; }
		public double ItemQuantity { get; set; }
		public double ChgPcentLabour { get; set; }
		public double AmountLabour { get; set; }
		public double ChgPcentMaterials { get; set; }
		public double AmountMaterials { get; set; }
		public double ChgPcentPlant { get; set; }
		public double AmountPlant { get; set; }
		public string AdjCode { get; set; }
		public double ChgPcentAdj { get; set; }
		public string PriorityCode { get; set; }
		public double ChgPcentPriority { get; set; }
		public double ChgPcentValue { get; set; }
		public double AmountValue { get; set; }
		public double AmountTotal { get; set; }
		public double AmountBalance { get; set; }
		public DateTime? ItemVam { get; set; }
		public int AssignedTo { get; set; }
		public int VamCostSequence { get; set; }
		public double VamCostRate { get; set; }
		public int ProductOutsDia { get; set; }
		public int ProductdOrw { get; set; }
		public int ProductWeight { get; set; }
		public DateTime? ItemDueDate { get; set; }
		public bool FlgCompleted { get; set; }
		public bool FlgDocsRecd { get; set; }
		public int SupplierId { get; set; }
		public long? MeoiSectionSequence { get; set; }
		public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }

    }
}