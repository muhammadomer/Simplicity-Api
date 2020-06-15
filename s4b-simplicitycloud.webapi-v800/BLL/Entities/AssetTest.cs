using SimplicityOnlineWebApi.BLL.Entities;
using System;

namespace SimplicityOnlineBLL.Entities
{
    public class AssetTestH
    {
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public long? AssetSequence { get; set; }
        public long? TypeSequence { get; set; }
        public long? CheckTypeSequence { get; set; }
        public long? EntityId { get; set; }
        public DateTime? DateCheck { get; set; }
        public bool FlgLocked { get; set; }
        public bool FlgComplete { get; set; }
        public long? TestPassOrFail { get; set; }
        public string CheckLocation { get; set; }
        public long? EngineHours { get; set; }       
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public string NameShort { get; set; }
    }

    public class AssetTestI
    {
        public long? Sequence { get; set; }
        public long? JoinSequence { get; set; }
        public long? AssetSequence { get; set; }
        public long? TestItemId { get; set; }
        public string InputDoneBy { get; set; }
        public string InputComments { get; set; }
        public DateTime? DateInputDone { get; set; }
        public string InputCheckedBy { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public AssetTestItems assetTestItems { get; set; }
        public AssetTestActionTypes assetTestActionTypes { get; set; }
    }

    public class AssetTestItems
    {
        public long? Test_item_id { get; set; }
        public bool FlgDeleted { get; set; }
        public long? TestListId { get; set; }
        public long? InputTypeId { get; set; }
        public bool FlgLabel { get; set; }
        public long? RowIndex { get; set; }
        public string TestItemCode { get; set; }
        public string TestItemDescription { get; set; }
        public long? ActionTypeId { get; set; }
        public string TestItemInstruction { get; set; }
        public string TestItemLocation { get; set; }
        public string TestItemCriteria { get; set; }
    }

    public class AssetTestActionTypes
    {
        public long? ActionTypeId { get; set; }
        public string ActionTypeDescription { get; set; }
        public bool FlgDeleted { get; set; }
        public long? RowIndex { get; set; }
    }
}