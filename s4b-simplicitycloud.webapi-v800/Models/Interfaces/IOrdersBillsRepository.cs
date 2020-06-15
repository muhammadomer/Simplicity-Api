using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
   public interface IOrdersBillsRepository : IRepository
    {
        ResponseModel CreateInvoiceRequest(HttpRequest request, OrdersBills obj);
        ResponseModel CreateRequestForPayment(HttpRequest request, OrdersBills obj);
        ResponseModel UpdateOrdersBillsBySequence(HttpRequest request, OrdersBills obj);
        ResponseModel ConvertToInvoice(HttpRequest request, OrdersBills obj);
        ResponseModel BatchConvertToInvoice(HttpRequest request, List<OrdersBills> obj);
        ResponseModel SaveInvoice(HttpRequest request, OrdersBills obj);
        ResponseModel GetOrderBillItemsForInvoicingByJobSequence(HttpRequest Request, long jobSequence);
        ResponseModel GetOrderBillsForEditingBySequence(HttpRequest Request, long billSequence,long jobSequence);
        OrdersBills GetOrderBillsBySequence(HttpRequest Request, long sequence);
        ResponseModel GetOrdersBillByJobSequenceAndType(HttpRequest Request, long jobSequence,string type);
        ResponseModel GetOrdersBillInvoiceBySequence(HttpRequest Request, long billSequence);
        ResponseModel GetApplicationForPaymentsAndInvoicesByJobSequence(HttpRequest Request,long jobSequence );
        ResponseModel GetSaleInvoiceBySequence(HttpRequest Request, long billSequence);
        ResponseModel GetListOfSaleInvoices(HttpRequest Request, DateTime? fromDate, DateTime? toDate);
        ResponseModel GetListOfAppForPayments(HttpRequest Request, ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate);
        ResponseModel DownloadPDF(HttpRequest request, string htmlString);
        List<OrdersBills> GetOrdersBillByJobSequence(HttpRequest Request, long jobSequence);
        OrdersBills update(HttpRequest request, OrdersBills obj);
    }
}
