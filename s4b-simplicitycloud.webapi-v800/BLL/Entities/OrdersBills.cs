using System;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class OrdersBills
    {

        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public string BillRef { get; set; }
        public string JobRef { get; set; }
        public long? ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientRef { get; set; }
        public string ClientParent { get; set; }
        public long? EntityJoinId { get; set; }
        public bool FlgParentOverride { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public double AmountInitial { get; set; }
        public double AmountDiscount { get; set; }
        public double PcentRetention { get; set; }
        public double AmountRetention { get; set; }
        public double AmountSubTotal { get; set; }
        public double AmountVat { get; set; }
        public double AmountCis { get; set; }
        public double AmountTotal { get; set; }
        public DateTime? JobDate { get; set; }
        public bool FlgJobDateStart { get; set; }
        public DateTime? JobDateStart { get; set; }
        public bool FlgJobDateFinish { get; set; }
        public DateTime? JobDateFinish { get; set; }
        public string MaillingAddress { get; set; }
        public string EmailAddress { get; set; }
        public string Footnote { get; set; }
        public bool FlgRequestMade { get; set; }
        public DateTime? RequestMadeDate { get; set; }
        public bool FlgSetToProforma { get; set; }
        public DateTime? SetToProformaDate { get; set; }
        public long? SageId { get; set; }
        public bool FlgSetToInvoice { get; set; }
        public DateTime? SetToInvoiceDate { get; set; }
        public long? InvoiceIndex { get; set; }
        public bool FlgHasAVatInv { get; set; }
        public bool FlgIsVatInv { get; set; }
        public long? JoinBillSequence { get; set; }
        public long? RciId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public bool FlgArchive { get; set; }
        public double OutstandingAmt { get; set; }
        public double AllocatedAmt { get; set; }
        public string EntryType { get; set; }
        public string Prospect { get; set; }
        public string NameSage { get; set; }
        public string PaymentType { get; set; }
        public List<OrderBillItems> OrderBillItems { get; set; }
    }

    public class SaleInvoices
    {
        public long? Sequence { get; set; }
        public string JobReference { get; set; }
        public string ClientRef { get; set; }
        public string ClientName { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? DateRaised { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string JobAddress { get; set; }
        public string JobDescription { get; set; }
        public string JobCostCentre { get; set; }
        public string TradeCode { get; set; }
        public string WorkCenter { get; set; }
        public double AmountInitial { get; set; }
        public double AmountDiscount { get; set; }
        public double PcentRetention { get; set; }
        public double AmountRetention { get; set; }
        public double AmountSubTotal { get; set; }
        public double AmountVat { get; set; }
        public double AmountCis { get; set; }
        public double AmountTotal { get; set; }
        public string Footnote { get; set; }
        public string FooterHtml { get; set; }
        public string HeaderHtml { get; set; }
        public string VatReg { get; set; }
        public List<OrderBillItems> OrderBillItems { get; set; }
    }


}