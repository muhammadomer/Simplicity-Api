using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IWebThirdPartiesRepository : IRepository
    {
        WebThirdParties Login(string webLogon, string webEnable, string ProjectId, HttpResponse Response);
        List<WebThirdParties> GetAllUsers(HttpRequest Request, HttpResponse Response);
        WebThirdParties AddUThirsPartyUser(WebThirdParties Obj, HttpRequest request);
        List<WebThirdParties> GetUsersByUserName(string userName, HttpRequest Request, HttpResponse Response);
        WebThirdParties GetUsersByUserId(int UserId, HttpRequest Request, HttpResponse Response);
        WebThirdParties UpdateUser(WebThirdParties userName, HttpRequest request);
        List<WebThirdParties> CheckUserAsUniq(string userName, HttpRequest request);
        List<WebThirdParties> GetAllUsersByEntityId(long EntityId, HttpRequest Request, HttpResponse Response);
        bool DeleteUser(WebThirdParties userName, HttpRequest request);
        string GetThirdPartyHomePageUrl(HttpRequest Request);
        ResponseModel ChangePassword(WebThirdParties webUser, HttpRequest request);
    }
}
