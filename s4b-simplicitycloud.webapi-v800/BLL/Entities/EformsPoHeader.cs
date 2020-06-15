using System;

namespace SimplicityOnlineBLL.Entities
{
    public class EformsPoHeader
    {
                 
            public long? Sequence { get; set; }
            public bool FlgDeleted { get; set; }
            public int DataType { get; set; }
            public string NfsSubmitNo { get; set; }
            public string NfsSubmitTimeStamp { get; set; }
            public string ImpRef { get; set; }
            public int FormType { get; set; }
            public string JobRef { get; set; }
            public bool FlgValidJobRef { get; set; }
            public long? JobSequence { get; set; }
            public string SupplierShortName { get; set; }
            public bool FlgValidSupplierShortName { get; set; }
            public long? SupplierId { get; set; }
            public long? SupplierMultiAddId { get; set; }
            public string SupplierEmail { get; set; }
            public string AttentionOf { get; set; }
            public string NfPoRef { get; set; }
            public DateTime? DatePoDate { get; set; }
            public DateTime? RequiredByDate { get; set; }
            public bool FlgDeliverToSite { get; set; }
            public string OrderedByShortName { get; set; }
            public bool FlgValidOrderedByShortName { get; set; }
            public long? OrderedById { get; set; }
            public string RequestedByShortName { get; set; }
            public bool FlgValidRequestedByShortName { get; set; }
            public long? RequestedById { get; set; }
            public string PoAddressInvoice { get; set; }
            public string PoNotes { get; set; }
            public long? PoVoTypeSequence { get; set; }
            public string VoRef { get; set; }
            public long? OrderId { get; set; }
            public double OrderAmount { get; set; }
            public double OrderDiscountAmount { get; set; }
            public double OrderShippingAmount { get; set; }
            public double OrderSubtotalAmount { get; set; }
            public double OrderVatAmount { get; set; }
            public double OrderTotalAmount { get; set; }
            public bool FlgOtherIssue { get; set; }
            public long? CreatedBy { get; set; }
            public DateTime? DateCreated { get; set; }
            public long? LastAmendedBy { get; set; }
            public DateTime? DateLastAmended { get; set; }
     }
}