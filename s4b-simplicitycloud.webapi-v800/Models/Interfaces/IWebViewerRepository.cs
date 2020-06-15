using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IWebViewerRepository : IRepository
    {
        WebViewer Login(string webLogon, string webEnable, string ProjectId, HttpResponse Response);
        //List<WebViewer> GetAllUsers(HttpRequest Request, HttpResponse Response);
        //WebViewer AddUThirsPartyUser(WebViewer Obj, HttpRequest request);
        //List<WebViewer> GetUsersByUserName(string userName, HttpRequest Request, HttpResponse Response);
        //WebViewer GetUsersByUserId(int UserId, HttpRequest Request, HttpResponse Response);
        //WebViewer UpdateUser(WebViewer userName, HttpRequest request);
        //List<WebViewer> CheckUserAsUniq(string userName, HttpRequest request);
        //List<WebViewer> GetAllUsersByEntityId(long EntityId, HttpRequest Request, HttpResponse Response);
        //bool DeleteUser(WebViewer userName, HttpRequest request);
        //string GetThirdPartyHomePageUrl(HttpRequest Request);
        //ResponseModel ChangePassword(WebViewer webUser, HttpRequest request);
    }
}
