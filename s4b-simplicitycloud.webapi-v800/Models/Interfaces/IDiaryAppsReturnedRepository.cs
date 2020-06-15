using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IDiaryAppsReturnedRepository : IRepository
    {
        ResponseModel AddDiaryAppReturnedWithOrderStatus(HttpRequest request, DiaryAppsReturned diaryAppReturned);
    }
}
