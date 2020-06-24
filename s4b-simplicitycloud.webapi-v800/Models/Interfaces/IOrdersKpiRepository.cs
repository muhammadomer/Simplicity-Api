using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using System;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IOrdersKpiRepository : IRepository
    {
        ResponseModel GetOutstandingKpiOrderList(HttpRequest request, ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate);
        ResponseModel GetSuccessKpiOrderList(HttpRequest request, ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate);
    }
}
