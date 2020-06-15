using System;

namespace SimplicityOnlineBLL.Entities
{
    public class OrderBillItems
    {
        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public long? BillSequence { get; set; }
        public long? ItemSequence { get; set; }
        public bool FlgTextLine { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public double PcentPayment { get; set; }
        public double AmountPayment { get; set; }
        public bool FlgDiscounted { get; set; }
        public double PcentDiscount { get; set; }
        public double AmountDiscount { get; set; }
        public bool FlgRetention { get; set; }
        public double PcentRetention { get; set; }
        public double AmountRetention { get; set; }
        public double AmountSubTotal { get; set; }
        public double PcentVat { get; set; }
        public double AmountVat { get; set; }
        public double PcentCis { get; set; }
        public double AmountCis { get; set; }
        public string SageNominalCode { get; set; }
        public string SageTaxCode { get; set; }
        public string SageBankCode { get; set; }
        public string CostCentreId { get; set; }
        public int ItemType { get; set; }
        public double ItemQty { get; set; } //non db-column
        public string ItemUnits { get; set; } //non db-column
        public double ItemAmountBalance { get; set; } //non db-column
        public double ItemAmountTotal { get; set; }
       
       
    }   
}