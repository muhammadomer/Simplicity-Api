using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
   public interface IOrderHireRepository : IRepository
    {
        ResponseModel GetListOfOrderHire(HttpRequest Request,ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate,int hireType);
        ResponseModel GetOrderHireBySequence(HttpRequest Request, int sequence);
        ResponseModel GetAssetSelectedForDateRange(HttpRequest Request, long assetsequence, DateTime? fromDate, DateTime? toDate);
        OrderHire Insert(HttpRequest request, OrderHire obj);
        ResponseModel UpdateBySequence(HttpRequest request, OrderHire obj);
        List<OrderHire> GetOrdHireForReportByDate(HttpRequest Request, DateTime? fromDate, DateTime? toDate);
    }
}
