using System;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.ClientInvoice.Entities
{
    public class StatementListResponse
    {
        public List<StatementResponse> data { get; set; }
        public decimal? openingBalance { get; set; }
        public decimal? closingBalance { get; set; }
    }

    public class StatementResponse
    {
        public long srNo { get; set; }
        public string jobRef { get; set; }
        public string jobAddress { get; set; }
        public string client { get; set; }
        public string invoicenoOrItemRef { get; set; }
        public string entryType { get; set; }
        public string sequence { get; set; }
        public string date { get; set; }
        public string refText { get; set; }
        public decimal? firstBalance { get; set; }
        public decimal? secondBalance { get; set; }
        public decimal? balance { get; set; }


        public StatementResponse()
        {
        }

        public StatementResponse(int _srNo, string _jobRef, string _jobAddress, string _client, string _invoicenoOrItemRef, string _entryType, string _sequence, string _date, string _refText, decimal? _firstBalance, decimal? _secondBalance, decimal? _balance)
        {
            srNo = _srNo;
            jobRef = _jobRef;
            jobAddress = _jobAddress;
            client = _client;
            invoicenoOrItemRef = _invoicenoOrItemRef;
            entryType = _entryType;
            sequence = _sequence;
            date = _date;
            refText = _refText;
            firstBalance = _firstBalance;
            secondBalance = _secondBalance;
            balance = _balance;
        }
    }

    public class StatementDBResponse
    {
        public string jobRef { get; set; }
        public string jobAddress { get; set; }
        public string client { get; set; }
        public string entryType { get; set; }
        public DateTime? entryDate { get; set; }
        public decimal? entryAmtTotal { get; set; }
        public string entryDetails { get; set; }
        public string invoicenoOrItemRef { get; set; }
        public string sequence { get; set; }
    }
}
