namespace SimplicityOnlineWebApi.ClientInvoice.Entities
{
    public class InvoiceListResponse<T>
    {
        public InvoiceListResponse()
        {
        }

        public InvoiceListResponse(T[] _data, int _totalRecords, int _currenPage, int _pageSize, int _currenPageRecords)
        {
            data = _data;
            totalRecords = _totalRecords;
            currenPage = _currenPage;
            pageSize = _pageSize;
            currenPageRecords = _currenPageRecords;
        }

        public T[] data { get; set; } = null;
        public SummaryTotalResponse summary { get; set; } = null;
        public int totalRecords { get; set; }
        public int currenPage { get; set; }
        public int pageSize { get; set; }
        public int currenPageRecords { get; set; }
    }
    
    public class SummaryTotalResponse
    {
        public InvoiceTotalResponse pageTotal { get; set; }
        public InvoiceTotalResponse allPageTotal { get; set; }

        public SummaryTotalResponse()
        {
        }
        public SummaryTotalResponse(InvoiceTotalResponse _pageTotal, InvoiceTotalResponse _allPageTotal)
        {
            pageTotal = _pageTotal;
            allPageTotal = _allPageTotal;
        }
    }
    
    public class InvoiceTotalResponse
    {
        public decimal? netTotal { get; set; }
        public decimal? vatTotal { get; set; }
        public decimal? grossTotal { get; set; }
        public decimal? osTotal { get; set; }
    }
    
    public class InvoiceResponse
    {
        public long entityId { get; set; }
        public string clientName { get; set; }
        public string jobRef { get; set; }
        public string jobAddress { get; set; }
        public long sequence { get; set; }
        public string invoiceNo { get; set; }
        public string invoiceDate { get; set; }
        public decimal? amountSubTotal { get; set; }
        public decimal? amountTotal { get; set; }
        public decimal? amountVat { get; set; }
        public decimal? entryAmtTotal { get; set; }
        public decimal? entryAmtAllocated { get; set; }
        public decimal? amtOutstanding { get; set; }
        public string inv { get; set; }
        public string invStatus { get; set; }
        public bool flgSettled { get; set; }
    }
}
