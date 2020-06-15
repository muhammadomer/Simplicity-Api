using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IRefDiaryAppRatesRepository : IRepository
    {
        RefDiaryAppRates AddDiaryAppRates(RefDiaryAppRates Obj, HttpRequest request);
        List<RefDiaryAppRates> GetDiaryAppRatesById(long TypeCode, HttpRequest Request, HttpResponse Response);
        List<RefDiaryAppRates> GetDiaryAppRates(HttpRequest Request, HttpResponse Response);
        RefDiaryAppRates UpdateDiaryAppRates(RefDiaryAppRates DiaryAppType, HttpRequest request);

    }
}
