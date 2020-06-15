using System;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class SubConPoHeader
    {
        public long? Sequence { get; set; }
        public bool FlgEformsImport { get; set; }
        public long? EformsImportId { get; set; }
        public bool FlgPoPlaced { get; set; }
        public int PoType { get; set; }
        public string PORef { get; set; }
        public string CustomerRef { get; set; }
        public long? JobSequence { get; set; }
        public long? EntityId { get; set; }
        public string EntityAddress { get; set; }
        public string EntityTelephone { get; set; }
        public DateTime? PODate { get; set; }
        public string AddressInvoice { get; set; }
        public string AddressDelivery { get; set; }
        public double PoAmtMat { get; set; }
        public double PoAmtLab { get; set; }
        public double PoAmtDiscount { get; set; }
        public double PoAmtShipping { get; set; }
        public double PoAmtSubtotal { get; set; }
        public double PoAmtVat { get; set; }
        public double PoAmtTotal { get; set; }
        public long? RequestedId { get; set; }
        public string VehicleReg { get; set; }
        public string poNotes { get; set; }
        public DateTime? RequiredByDate { get; set; }
        public bool FlgDispatchDate { get; set; }
        public DateTime? DateDespatchDate { get; set; }
        public string OrderedBy { get; set; }
        public int POStatus { get; set; }
        public string UserField01 { get; set; }
        public string UserField02 { get; set; }
        public string UserField03 { get; set; }
        public string UserField04 { get; set; }
        public string UserField05 { get; set; }
        public string UserField06 { get; set; }
        public string UserField07 { get; set; }
        public string UserField08 { get; set; }
        public string UserField09 { get; set; }
        public string UserField10 { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public List<SubConPOItems> SubConPOItems { get; set; }
    }
}