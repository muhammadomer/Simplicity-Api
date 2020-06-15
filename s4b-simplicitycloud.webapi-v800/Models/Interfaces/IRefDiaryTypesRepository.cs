using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IRefDiaryTypesRepository : IRepository
    {
        List<RefDiaryAppTypes> GetAllDiaryAppTypes(HttpRequest request, HttpResponse response);
        RefDiaryAppTypes AddDiaryAppTypes(RefDiaryAppTypes Obj, HttpRequest request);
        List<RefDiaryAppTypes> GetDiaryAppTypesById(long TypeCode, HttpRequest Request, HttpResponse Response);
        RefDiaryAppTypes UpdateDiaryAppTypes(RefDiaryAppTypes DiaryAppType, HttpRequest request);

    }
}
