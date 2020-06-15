using System;

namespace SimplicityOnlineBLL.Entities
{
    public class SubConPOItems
    {
        public long? Sequence { get; set; }
        public long? POSequence { get; set; }
        public long? EntityId { get; set; }
        public int ItemType { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string ItemUnit { get; set; }
        public double ItemQuantity { get; set; }
        public double ItemAmountMat { get; set; }
        public double ItemAmountLabour { get; set; }
        public double ItemAmountNet { get; set; }
        public bool FlgItemDiscount { get; set; }
        public double ItemDiscountPcent { get; set; }
        public double ItemDiscountAmount { get; set; }
        public double ItemSubtotal { get; set; }
        public bool FlgItemVat { get; set; }
        public double ItemVatPcent { get; set; }
        public double ItemVatAmount { get; set; }
        public double ItemTotal { get; set; }
        public double ItemHours { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}