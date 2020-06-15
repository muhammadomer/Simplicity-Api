using System.Collections.Generic;

namespace SimplicityOnlineWebApi.ClientInvoice.Entities
{
    public class InvoiceDetailPdfResponse
    {
        #region Comman Data

        public string invoice_detail_title { get; set; } = "";
        public string company_address { get; set; } = "";
        public string invoice_date { get; set; } = "";
        public string client { get; set; } = "";
        public string job_ref { get; set; } = "";
        public string job_address { get; set; } = "";
        public string start_date { get; set; } = "";
        public string end_date { get; set; } = "";
        public string your_ref { get; set; } = "";
        public string trade_code { get; set; } = "";
        public string foot_note { get; set; } = "";

        public decimal? amount_sub_total { get; set; } = 0;
        public decimal? orders_bills_amount_vat { get; set; } = 0;
        public decimal? amount_total { get; set; } = 0;
        public decimal? paid_to_date { get; set; } = 0;

        public string paymement_item_display { get; set; } = "";
        public string paymement_logo { get; set; } = "";

        #endregion

        #region Invoice Payments

        public List<InvoiceDetailItemsPdfResponse> invoice_items { get; set; }
        public List<InvoiceDetailPaymentPdfResponse> invoice_payments { get; set; }

        #endregion
    }

    public class InvoiceDetailItemsPdfResponse
    {
        public string item_desc { get; set; } = "";
        public string item_code { get; set; } = "";
        public string item_units { get; set; } = "";
        public int item_quantity { get; set; } = 0;
        public decimal? amount_payment { get; set; } = 0;        
    }

    public class InvoiceDetailPaymentPdfResponse
    {
        public string entry_type { get; set; } = "";
        public string invoice_no_or_item_ref { get; set; } = "";
        public string entry_date { get; set; } = "";
        public decimal? entry_amt_allocated { get; set; } = 0;
        public string invoice_no { get; set; } = "";
    }
}
