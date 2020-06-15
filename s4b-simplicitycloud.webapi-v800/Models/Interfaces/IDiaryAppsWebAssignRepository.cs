using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.BLL.Entities;
using Microsoft.AspNetCore.Http;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IDiaryAppsWebAssignRepository : IRepository
    {
        List<DiaryAppsWebAssign> GetAllWebAssignApp(HttpRequest Request, HttpResponse Response);
        DiaryAppsWebAssign AddWebAssignObject(DiaryAppsWebAssign WebAssignObj, HttpRequest request);
        DiaryAppsWebAssign UpdateWebAssignObject(DiaryAppsWebAssign WebAssignObj, HttpRequest request);
        DiaryAppsWebAssign IsWebAssignAppExists(DiaryAppsWebAssign webAssign, HttpRequest Request);
        List<DiaryAppsWebAssign> GetThirdPartyApp(DiaryAppsWebAssign webAssign, HttpRequest Request);
        DiaryAppsWebAssign UpdateWebAssignByCriteria(DiaryAppsWebAssign WebAssignObj, HttpRequest request);
    }
}
