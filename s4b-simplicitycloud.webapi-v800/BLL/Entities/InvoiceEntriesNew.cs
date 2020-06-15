using System;

namespace SimplicityOnlineBLL.Entities
{
    public class InvoiceEntriesNew
    {

        public long? Sequence { get; set; }
        public bool FlgCancelled { get; set; }
        public bool FlgSysEntry { get; set; }
        public string TransType { get; set; }
        public string EntryType { get; set; }
        public string InvoicenoOrItemref { get; set; }
        public long? BillSequence { get; set; }
        public long? JobSequence { get; set; }
        public long? ContactId { get; set; }
        public long? EntitySubId { get; set; }
        public DateTime EntryDate { get; set; }
        public double EntryAmtOrMat { get; set; }
        public double EntryAmtLabour { get; set; }
        public double EntryAmtDiscounted { get; set; }
        public double EntryAmtSubtotal { get; set; }
        public bool FlgAddVat { get; set; }
        public double EntryAmtVat { get; set; }
        public bool FlgTaxRequired { get; set; }
        public double EntryTaxRate { get; set; }
        public double EntryAmtTax { get; set; }
        public double EntryAmtTotalMat { get; set; }
        public double EntryAmtTotalLabour { get; set; }
        public double EntryAmtTotal { get; set; }
        public string EntryDetails { get; set; }
        public string EntryVoucherRef { get; set; }
        public string EntryExtraInfo { get; set; }
        public bool FlgCardDetails { get; set; }
        public long? CardDetailsId { get; set; }
        public bool FlgCardPayeeIsEntity { get; set; }
        public long? SageId { get; set; }
        public double EntryAmtAllocated { get; set; }
        public double EntryAmtAllocatedLabour { get; set; }
        public bool FlgSettled { get; set; }
        public bool FlgSubEntryRow { get; set; }
        public long? SubEntryJoinDrSequence { get; set; }
        public long? SubEntryJoinCrSequence { get; set; }
        public double SubEntryAmt { get; set; }
        public string SubEntryDetails { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}