using System;

namespace SimplicityOnlineBLL.Entities
{
    public class StockList
    {
        public string StockCode { get; set; }
        public long? EntityId { get; set; }
        public long? StockTypeId { get; set; }
        public string StockUnits { get; set; }
        public double StockCostPrice { get; set; }
        public DateTime? DateCpLastAmended { get; set; }
        public string StockBarCode { get; set; }
        public long? SageId { get; set; }
        public string SageNominalCode { get; set; }
        public string SageTaxCode { get; set; }
        public byte[] StockPicture { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }

    }
}