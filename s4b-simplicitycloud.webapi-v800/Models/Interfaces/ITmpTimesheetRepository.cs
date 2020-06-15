using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface ITmpTimesheetRepository : IRepository
    {
        TmpTimesheet insert(HttpRequest Request, TmpTimesheet obj);
        ResponseModel CreateTimeSheetData(HttpRequest Request, TmpTimesheet obj);
        List<AppointmentTimeEntries> GetAllTimeEntriesByDate(HttpRequest request, DateTime? appStartDate);
    }
}
