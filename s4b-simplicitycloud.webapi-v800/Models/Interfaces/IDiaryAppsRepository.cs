using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IDiaryAppsRepository : IRepository
    {
        List<DiaryApps> GetDiaryAppsByDate(DateTime? appStartDate, DateTime? appEndDate, HttpRequest Request, HttpResponse Response);
        ResponseModel CreateDiaryApps(DiaryApps diaryApp, HttpRequest request);
        DiaryApps UpdateDiaryApp(DiaryApps diaryApp, HttpRequest request);
        Boolean DeleteDiaryApp(DiaryApps diaryApp, HttpRequest request);
        List<DiaryApps> GetAppoinmentsByAppDateAndUserId(DateTime? date, HttpRequest request);
        DiaryApps GetDiaryAppsBySequence(long sequence, HttpRequest request, HttpResponse response);
        List<DiaryApps> GetDiaryAppsThirdPartyByEntityId(long entityId, HttpRequest request, HttpResponse response);
        ResponseModel GetUserAppointmentsByAppDate(HttpRequest request, DateTime? date);        
        List<DiaryApps> GetDiaryAppsByDateAndJobRef(DateTime? appointmentStartDate, DateTime? appointmentEndDate, string jobRef, HttpRequest request, HttpResponse response);
        List<DiaryAppsHistory> GetDiaryAppsByJobSequence(HttpRequest request, HttpResponse response, long jobSequence);

        ResponseModel GetUserAppointmentsByAppDateForTimeEntry(HttpRequest request, DateTime? appStartDate);
    }
}
