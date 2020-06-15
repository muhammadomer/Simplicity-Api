using SimplicityOnlineBLL.Entities;
using Microsoft.AspNetCore.Http;
using System;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface ITimeAndAttendanceRepository : IRepository
    {
        ResponseModel GetBudget(HttpRequest request, int yearValue, int teamId, int locationId);
        ResponseModel GetRosterDetails(HttpRequest request, DateTime? entryDate1, DateTime? entryDate2, long prReference, long summarySequence);
        ResponseModel GetRevenue(HttpRequest request, int teamId, int locationId, DateTime? entryDate1, DateTime? entryDate2);
        ResponseModel UpdateBudget(HttpRequest request, RequestModel reqModel);
        ResponseModel UpdateRevenue(HttpRequest request, RequestModel reqModel);
    }
}
