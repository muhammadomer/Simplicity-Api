using System;

namespace SimplicityOnlineBLL.Entities
{
    public class EFormsPOItems
    {                 
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public int DataType { get; set; }
        public string ImpRef { get; set; }
        public long? JoinSequence { get; set; }
        public int ItemType { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string ItemUnit { get; set; }
        public double ItemQuantity { get; set; }
        public double ItemAmtUnitPrice { get; set; }
        public double ItemAmtSubtotalBeforeDiscount { get; set; }
        public bool FlgItemDiscount { get; set; }
        public double ItemDiscountPcent { get; set; }
        public double ItemAmtDiscount { get; set; }
        public double ItemAmtSubtotal { get; set; }
        public bool FlgItemVat { get; set; }
        public double ItemVatPcent { get; set; }
        public double ItemAmtVat { get; set; }
        public double ItemAmtTotal { get; set; }
        public DateTime? DateItemDueDate { get; set; }
        public bool FlgDeliverToSite { get; set; }
        public bool FlgDeliveryNote { get; set; }
        public string DeliveryNoteRef { get; set; }
        public double DeliveryNoteQty { get; set; }
        public DateTime? DateDeliveryNote { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
     }
}