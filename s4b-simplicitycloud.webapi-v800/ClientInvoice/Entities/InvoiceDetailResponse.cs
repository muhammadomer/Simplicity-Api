using System.Collections.Generic;

namespace SimplicityOnlineWebApi.ClientInvoice.Entities
{
    public class InvoiceDetailResponse
    {
        #region Comman Data

        public string paymentStatus { get; set; } = "";
        public string invoiceDetailMainTitlePretext { get; set; } = "";
        public string invoiceNo { get; set; } = "";
        public string invoiceDate { get; set; } = "";
        public string nameLong { get; set; } = "";
        public string jobRef { get; set; } = "";
        public string jobAddress { get; set; } = "";
        public string jobDateStart { get; set; } = "";
        public string jobDateFinish { get; set; } = "";
        public string jobClientRef { get; set; } = "";
        public string jobTradeCode { get; set; } = "";
        public string invaddr { get; set; } = "";
        public bool flgSetToInvoice { get; set; }
        public string footNote { get; set; } = "";        
        public string maillingAddress { get; set; }
        public string invoiceType { get; set; }

        public CompanyAddressWithDetailResponse companyAddressInDetail { get; set; }

        public decimal? paidToDate { get; set; } = 0;
        #endregion

        #region Totals
        public decimal? subtotalScheduledItems { get; set; } = 0;
        public decimal? invoiceAmount { get; set; } = 0;
        public decimal? discount { get; set; } = 0;
        public decimal? retentionTotal { get; set; } = 0;
        public decimal? subTotal { get; set; } = 0;
        public decimal? vat { get; set; } = 0;
        public decimal? invoiceTotal { get; set; } = 0;
        #endregion

        #region Invoice Payments        
        public List<InvoiceDetailItemsResponse> invoiceItems { get; set; }        
        public List<InvoiceDetailPaymentResponse> invoicePayments { get; set; }
        #endregion
    }
    
    public class InvoiceDetailItemsResponse
    {
        public bool flgRowIsText { get; set; }
        public string itemDesc { get; set; } = "";
        public string itemCode { get; set; } = "";
        public string itemUnits { get; set; } = "";
        public string itemQuantity { get; set; } = "";
        public decimal? amountPayment { get; set; } = 0;
        public decimal? amountVat { get; set; } = 0;
        public decimal? amountDiscount { get; set; } = 0;
        public decimal? amountSubTotal { get; set; } = 0;
        public decimal? amountRetention { get; set; } = 0;
    }

    public class InvoiceDetailPaymentResponse
    {
        public string entryType { get; set; } = "";
        public string invoicenoOrItemRef { get; set; } = "";
        public string entryDate { get; set; } = "";
        public decimal? entryAmtAllocated { get; set; } = 0;
        public string invoiceNo { get; set; } = "";        
    }
}
