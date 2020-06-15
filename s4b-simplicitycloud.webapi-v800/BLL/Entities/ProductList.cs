namespace SimplicityOnlineBLL.Entities
{
    public class ProductList
    {
        public long? GroupId { get; set; }
        public string TransType { get; set; }
        public string ProductCode { get; set; }
        public string ProductUnits { get; set; }
        public double AmountLabour { get; set; }
        public double AmountPlant { get; set; }
        public double AmountMaterials { get; set; }
        public double AmountTotal { get; set; }
        public string ProductVam { get; set; }
        public string ProductCostCentre { get; set; }
        public long? SageId { get; set; }
        public string ProductSageNominalCode { get; set; }
        public string ProductSageTaxCode { get; set; }
        public string ProductOutsDia { get; set; }
        public string ProductDOrW { get; set; }
        public string ProductWeight { get; set; }
        public string ProductLength { get; set; }
        public string ProductWidth { get; set; }
        public string ProductHeight { get; set; }
        public string ProductAreaMin { get; set; }
        public string ProductAreaMax { get; set; }
        public string ProductTypeId { get; set; }
        public long? CreatedBy { get; set; }
        public string DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public string DateLastAmended { get; set; }
    }
}