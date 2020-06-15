using System;
namespace SimplicityOnlineBLL.Entities
{
    public class PurchaseOrderItems
    {

        public long? Sequence { get; set; }
        public long? OrderId { get; set; }
        public int ItemImportType { get; set; }
        public long? RequestSequence { get; set; }
        public long? JobSequence { get; set; }
        public string JobRef { get; set; }
        public string JobAddress { get; set; }
        public string Supplier { get; set; }
        public string OrderRef { get; set; }
        public DateTime? OrderDate { get; set; }
        public string TransType { get; set; }
        public long? EntityId { get; set; }
        public int ItemType { get; set; }
        public double ItemHours { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string ItemUnit { get; set; }
        public double ItemQuantity { get; set; }
        public double ItemAmount { get; set; }
        public bool FlgItemDiscount { get; set; }
        public double ItemDiscountPcent { get; set; }
        public double ItemDiscountAmount { get; set; }
        public double ItemSubtotal { get; set; }
        public bool FlgItemVat { get; set; }
        public double ItemVatPcent { get; set; }
        public double ItemVatAmount { get; set; }
        public double ItemTotal { get; set; }

    }
}