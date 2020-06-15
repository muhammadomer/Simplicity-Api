using System;

namespace SimplicityOnlineBLL.Entities
{
    public class StockDetails
    {

        public string StockCode { get; set; }
        public long? EntityId { get; set; }
        public double StockReorderLevel { get; set; }
        public double StockReorderAmount { get; set; }
        public double StockMinReorderAmount { get; set; }
        public double StockQuantityAvail { get; set; }
        public double StockQuantityOnOrder { get; set; }
        public double StockPriceSale { get; set; }
        public double StockShippingWeight { get; set; }
        public string StockBinLocation { get; set; }
        public double StockAmtLabour { get; set; }
        public double StockLabourHours { get; set; }
        public double StockMaterialWaste { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }

    }
}