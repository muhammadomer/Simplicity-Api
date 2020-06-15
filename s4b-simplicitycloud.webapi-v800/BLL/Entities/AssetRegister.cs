using SimplicityOnlineWebApi.BLL.Entities;
using System;

namespace SimplicityOnlineBLL.Entities
{
    public class AssetRegister
    {
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public string TransType { get; set; }
        public long? EntityId { get; set; }
        public long? ItemJoinDept { get; set; }
        public long? ItemJoinCategory { get; set; }
        public long? ItemJoinSupplementary { get; set; }
        public string ItemManufacturer { get; set; }
        public string AssetId { get; set; }
        public string ItemModel { get; set; }
        public string ItemSerialRef { get; set; }
        public string ItemExtraInfo { get; set; }
        public string ItemUserField1 { get; set; }
        public string ItemUserField2 { get; set; }
        public string ItemUserField3 { get; set; }
        public double ItemQuantity { get; set; }
        public DateTime? DateInstalled { get; set; }
        public DateTime? DateAcquired { get; set; }
        public DateTime? DateDisposed { get; set; }
        public double ItemValueBook { get; set; }
        public double ItemValueDepreciation { get; set; }
        public double ItemValueDisposal { get; set; }
        public string ItemDesc { get; set; }
        public string ItemAddress { get; set; }
        public bool FlgUseAddressId { get; set; }
        public long? ItemAddressId { get; set; }
        public long? ItemLocationJoinId { get; set; }
        public string ItemLocation { get; set; }
        public bool FlgItemChargeable { get; set; }
        public double ItemCostMaterialRate { get; set; }
        public double ItemCostLabourRate { get; set; }
		public double ItemCostAssetRateWeek { get; set; }
		public double ItemCostLabourRateWeek { get; set; }
		public bool FlgService { get; set; }
        public long? ServiceStartDay { get; set; }
        public long? ServiceStartMonth { get; set; }
        public long? ServiceRenewal { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public EntityDetailsCore EDC { get; set; }
        public AssetRegisterCats ARC { get; set; }
        public AssetRegisterDepts ARD { get; set; }
        public RefAssetRegisterSupplementaryTables Arst { get; set; }

    }

    public class AssetDetail
    {
        public long? DeSequence { get; set; }
        public DateTime? DateAppStart { get; set; }
        public string ResourceName { get; set; }
        public string JobRef { get; set; }
        public string JobAddress { get; set; }

    }
}