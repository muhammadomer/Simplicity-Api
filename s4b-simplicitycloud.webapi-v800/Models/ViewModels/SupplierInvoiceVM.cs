using SimplicityOnlineWebApi.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models
{
    public class SupplierInvoiceVM
    {
        public SupplierInvoiceVM()
        {
            InvoiceLines = new List<SupplierInvoiceItemsVM>();
        }
        public long Sequence { get; set; }
        public string TransType { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string SageACRef { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime ItemisedDate { get; set; }
        public string SubTotal { get; set; }
        public string VAT { get; set; }
        public string Total { get; set; }
        public string InvoicePic { get; set; }
        public string FileNameCabId { get; set; }
        public long? EntityId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Approved { get; set; }
        public string RossumPONo { get; set; }
        public string JobReference { get; set; }
        public string JobManager { get; set; }
        public int ContactId { get; set; }
        public bool ItemisedRef { get; set; }
        public bool FlgInvoiceCreated { get; set; }
        public int SageId { get; set; }
        public double SumAmtMain { get; set; }
        public double SumAmtLabour { get; set; }
        public double SumAmtDiscount { get; set; }
        public double SumAmtSubTotal { get; set; }
        public bool FlgIncVAT { get; set; }
        public double SumAmtVAT { get; set; }
        public bool FlgAddTax { get; set; }
        public double SumAmtTax { get; set; }
        public double SumAmtTotal { get; set; }
        public string ItemisedDetail { get; set; }
        public string SagBankCode { get; set; }
        public bool FlgChecked { get; set; }
        public DateTime? DateChecked { get; set; }
        public int CreatedBy { get; set; }
        public int? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public long? RossumFileSequence { get; set; }
        public string RossumPurchaseOrderoNo { get; set; }
        public string RossumDeliveryNotNo { get; set; }
        public string SupplierName { get; set; }
        public string SupplierType { get; set; }
        public bool IsConvertToInvoice { get; set; }
        public List<SupplierInvoiceItemsVM> InvoiceLines { get; set; }
    }
    public class SupplierInvoiceItemsVM
    {
        public long Sequence { get; set; }
        public long InvoiceSequence { get; set; }
        public int ItemJoinType { get; set; }
        public DateTime ItemDate { get; set; }
        public double ItemQuantity { get; set; }
        public string ItemRef { get; set; }
        public string StockCode { get; set; }
        public string ItemDesc { get; set; }
        public string ItemUnit { get; set; }
        public int AssetSequence { get; set; }
        public double ItemAmt { get; set; }
        public double ItemAmtLabour { get; set; }
        public double ItemAmtTax { get; set; }
        public bool FlgItemDiscounted { get; set; }
        public double ItemDiscountPercent { get; set; }
        public double ItemAmtDiscount { get; set; }
        public double ItemAmtSubTotal { get; set; }
        public double ItemVATPercent { get; set; }
        public double ItemAmtVAT { get; set; }
        public double ItemAmtTotal { get; set; }
        public double ItemRetentionPercent { get; set; }
        public double ItemAmtRetention { get; set; }
        public double ItemAmtRetentionPaid { get; set; }
        public string ItemVoucherRef { get; set; }
        public string ItemRecipientName { get; set; }
        public string ItemReceiverName { get; set; }
        public string SageNominalCode { get; set; }
        public string SageTaxCode { get; set; }
        public string SageBankCode { get; set; }
        public string CostCentreId { get; set; }
        public int TelSequence { get; set; }
        public int JobSequence { get; set; }
        public bool FlgJobSeqExclude { get; set; }
        public int ItemType { get; set; }
        public int AccomSequence { get; set; }
        public bool FlgChecked { get; set; }
        public int ImportType { get; set; }
        public int ImportTypeSequence { get; set; }
        public string ImportTypeRef { get; set; }
        public string ImportTypeDescription { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public string CostCodeDesc { get; set; }
        public string VehicleReg { get; set; }
        public string TelRefDesc { get; set; }
    }
}

