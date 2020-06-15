using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IDashboardRepository : IRepository
    {
        ResponseModel GetDashboardView(HttpRequest request, DateTime date);
        ResponseModel GetDashboardViewForOrdersByOrderType(HttpRequest request, DateTime fromDate,DateTime toDate);
        ResponseModel GetDashboardViewForOrdersByOrderStatus(HttpRequest request, DateTime fromDate, DateTime toDate);
        ResponseModel GetDashboardViewForOrdersByJobStatus(HttpRequest request, DateTime fromDate, DateTime toDate);
        ResponseModel GetDashboardViewForSubmissionByTemplate(HttpRequest request, DateTime fromDate, DateTime toDate);
    }
}
