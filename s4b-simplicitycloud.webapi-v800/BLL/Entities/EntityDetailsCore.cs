using System;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class EntityDetailsCore
    {
        public long? EntityId { get; set; }
        public bool FlgDeleted { get; set; }
        public bool FlgEntityOnHold { get; set; }
        public bool FlgContactManager { get; set; }
        public long? ClientType { get; set; }
        public bool FlgInvoicingClient { get; set; }
        public bool FlgEntityApproved { get; set; }
        public bool FlgEntityJoin { get; set; }
        public long? EntityJoinId { get; set; }
        public long? EntityApprovedStatus { get; set; }
        public long? EntityPymtType { get; set; }
        public string EntityPymtTypeDesc { get; set; }
        public bool FlgEformsPreferred { get; set; }
        public string NameShort { get; set; }
        public string NameLong { get; set; }
        public long? SageId { get; set; }
        public bool FlgSageTurnOn { get; set; }
        public string NameSage { get; set; }
        public string NameTitle { get; set; }
        public string NameInitilas { get; set; }
        public string NameForename { get; set; }
        public string NameSurname { get; set; }
        public string AddressNo { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string AddressPostCode { get; set; }
        public string AddressFull { get; set; }
        public string Telephone { get; set; }
        public string TelExt { get; set; }
        public string TelFax { get; set; }
        public string TelMobile { get; set; }
        public string TelWork { get; set; }
        public string Email { get; set; }
        public string PropertyEpn { get; set; }
        public string PropertyUpn { get; set; }
        public string EntityDetails { get; set; }
        public string SupplementaryEntityDetails { get; set; }
        public bool FlgSupAddressHeld { get; set; }
        public bool FlgClientCheck { get; set; }
        public long? UserListId { get; set; }
        public long? UserListId2 { get; set; }
        public long? UserListId3 { get; set; }
        public object UserNumericField1 { get; set; }
        public bool FlgUserField1 { get; set; }
        public bool FlgUserField2 { get; set; }
        public bool FlgUserField3 { get; set; }
        public bool FlgUserField4 { get; set; }
        public string UserTextField1 { get; set; }
        public string UserTextField2 { get; set; }
        public string UserTextField3 { get; set; }
        public string UserTextField4 { get; set; }
        public bool FlgUserDateField1 { get; set; }
        public DateTime? DateUserDateField1 { get; set; }
        public bool FlgUserDateField2 { get; set; }
        public DateTime? DateUserDateField2 { get; set; }
        public bool FlgUserDateField3 { get; set; }
        public DateTime? DateUserDateField3 { get; set; }
        public bool FlgUserDateField4 { get; set; }
        public DateTime? DateUserDateField4 { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public long? ParentEntityId { get; set; }
        public string ParentNameShort { get; set; }
        public List<EntityDetailsSupplementary> EntityDetailsSupplementaryList { get; set; }
        public string SupplementaryDetails { get; set; }
        public string PropertyType { get; set; }
        public string PropertyStatus { get; set; }
        public string SageNominalCode { get; set; }
        public string SageDefaultTaxCode { get; set; }
        public string SageVatNumber { get; set; }
        public string WebAddress { get; set; }
        public string Client { get; set; }
        public EdcGdpr EdcGdpr { get; set; }
        public int UserAccepts { get; set; }
        public DateTime? DateUserAccepts { get; set; }
        public int AcceptsType { get; set; }
        public int ContactByPost { get; set; }
        public int ContactByEmail { get; set; }
        public int ContactByPhone { get; set; }
        public int ContactBySMS { get; set; }
        public string Type { get; set; }
        public List<EntityDetailsNotes> EntityDetailNotes { get; set; }
        public EdcCloud EdcCloudFields { get; set; }
    }

    public class EdcCloud
    {
        public string ContactInvoiceCabId { get; set; }
        public string ContactPOCabId { get; set; }
        public string ContactDNCabId { get; set; }
        public string RossumContactName { get; set; }
    }

    public class EntityDetailsCoreMin
    {
        public long? EntityId { get; set; }
        public long? EntityJoinId { get; set; }
        public string NameShort { get; set; }
        public string NameLong { get; set; }
        public string NameForename { get; set; }
        public string NameSurname { get; set; }
        public string ParentName { get; set; }
        public string AddressPostCode { get; set; }
        public string AddressFull { get; set; }
        public string Telephone { get; set; }
        public string TelExt { get; set; }
        public string TelFax { get; set; }
        public string TelMobile { get; set; }
        public string TelWork { get; set; }
        public string Email { get; set; }
        public string TransTypeDesc { get; set; }
        public bool FlgEntityApproved { get; set; }
        public bool FlgInvoicingClient { get; set; }
		public bool FlgEntityOnHold { get; set; }

	}

    public class EntityDetailsCoreAddress
    {
        public long? EntityId { get; set; }
        public bool FlgDeleted { get; set; }
        public long? ClientType { get; set; }
        public long? EntityJoinId { get; set; }
        public string NameShort { get; set; }
        public string NameLong { get; set; }
        public string NameTitle { get; set; }
        public string AddressNo { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressLine5 { get; set; }
        public string AddressPostCode { get; set; }
        public string AddressFull { get; set; }
        public string Telephone { get; set; }
        public string TelExt { get; set; }
        public string TelFax { get; set; }
        public string TelMobile { get; set; }
        public string TelWork { get; set; }
        public string Email { get; set; }
        public string PropertyEpn { get; set; }
        public string PropertyUpn { get; set; }
        public string EntityDetails { get; set; }
        public string PropertyType { get; set; }
        public string PropertyStatus { get; set; }
        public string Client { get; set; }
    }

}