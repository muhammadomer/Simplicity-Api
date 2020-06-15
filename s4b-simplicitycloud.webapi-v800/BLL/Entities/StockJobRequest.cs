using System;

namespace SimplicityOnlineBLL.Entities
{
    public class StockJobRequest
    {
        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public long? JoinSequence { get; set; }
        public string TransType { get; set; }
        public long? EntityId { get; set; }
        public string StockCode { get; set; }
        public string StockUnit { get; set; }
        public string StockDesc { get; set; }
        public double StockQuantity { get; set; }
        public double StockAmountEst { get; set; }
        public DateTime? StockRequestedDate { get; set; }
        public DateTime? DateStockRequired { get; set; }
        public bool FlgStockOrdered { get; set; }
        public DateTime? StockOrderedDate { get; set; }
        public bool FlgStockReceived { get; set; }
        public DateTime? StockReceivedDate { get; set; }
        public bool FlgSorDrillDown { get; set; }
        public string SorItemCode { get; set; }
        public int ItemType { get; set; }
        public double ItemHours { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}