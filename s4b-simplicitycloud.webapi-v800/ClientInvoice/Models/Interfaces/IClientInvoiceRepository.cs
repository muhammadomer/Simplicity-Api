using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.ClientInvoice.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.ClientInvoice.Models.Interfaces
{
    public interface IClientInvoiceRepository : IRepository
    {
        BankDetailsCompanyNoVatNoResponse GetBankDetailForClient(string projectId, int sequnceId, string invoiceNo);

        List<JobRefAutoCompleteResponse> GetJobRefAutoComplete(HttpRequest request, string searchText, int itemCount);

        List<NamedModel> GetCompanyAutoComplete(HttpRequest request, string searchText, int itemCount);

        List<NamedModel> GetShowTypesData(HttpRequest request);

        ListResponse<CompanyAgedDabtorResponse> GetAllAgedDabutors(HttpRequest request, ClientPageListRequest listRequest);

        StatementListResponse GetAllStatement(string projectId, Parameters parameters);

        InvoiceListResponse<InvoiceResponse> GetAllInvoices(HttpRequest request, ClientPageListRequest listRequest, Parameters parameters);

        InvoiceDetailResponse GetInvoiceDetail(string projectId, int sequenceId, string invoiceNo);

        InvoiceDetailPdfResponse GetInvoiceDetailPdfData(string projectId, int sequenceId, string invoiceNo);

        LicenseInformation GetLicenseInfo(string projectId);

        string GetBasePath();
    }
}
