using System;

namespace SimplicityOnlineBLL.Entities
{
    public class StockJobReceived
    {

        public long? Sequence { get; set; }
        public long? RequestSequence { get; set; }
        public string DeliveryRef { get; set; }
        public string TransType { get; set; }
        public long? EntityId { get; set; }
        public DateTime? StockRecievedDate { get; set; }
        public string StockCode { get; set; }
        public double StockQuantity { get; set; }
        public double StockAmount { get; set; }
        public bool FlgFromStockroom { get; set; }
        public long? JobSequence { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }

    }
}