using SimplicityOnlineBLL.Entities;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.BLL.Entities;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.Models.ViewModels;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
   public interface ISupplierInvoiceRepository : IRepository
    {
        ResponseModel GetRossumUnfinalizedInvoices(RequestHeaderModel header, ClientRequest clientRequest);
        ResponseModel GetUnfinalizedInvoices(RequestHeaderModel header, ClientRequest clientRequest);
        ResponseModel SaveInvoice(InvoiceItemised invoice, RequestHeaderModel header);
        SupplierInvoiceVM GetInvoiceByInvNo(string invoiceNo, RequestHeaderModel header);
        ResponseModel GetItemisedInvoice(RequestHeaderModel header, long invoiceSequence);
        ResponseModel GetVehicle(RequestHeaderModel header);
        ResponseModel GetItemTel(RequestHeaderModel header);
        ResponseModel GetCostCode(RequestHeaderModel header);
        SageViewModel GetSageDetail(RequestHeaderModel header);
        ResponseModel UpdateInvoiceSupplier(InvoiceItemised invoice, RequestHeaderModel header);
        SupplierInvoiceVM GetInvoiceBySequenceNo(long sequenceNo, RequestHeaderModel header);
        string GetJobRefByPO(long PONo, RequestHeaderModel header);
    }
}
