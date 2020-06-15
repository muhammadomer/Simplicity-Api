using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineBLL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class WebViewerController : Controller
    {
        private readonly IWebViewerRepository WebViewerRepository;
        public WebViewerController(IWebViewerRepository webViewerRepository)
        { this.WebViewerRepository =webViewerRepository; }

        // POST api/values
        [HttpPost]
        [ActionName("Login")]
        [Route("[action]")]
        public WebViewer Login([FromBody]WebViewer webUser)
        {
            WebViewer returnedWebUser = WebViewerRepository.Login(webUser.ViewerLogon, webUser.ViewerEnable, webUser.ProjectId, HttpContext.Response);
            if (returnedWebUser == null)
            {
                returnedWebUser = new WebViewer();
                returnedWebUser.IsSucessfull = false;
                returnedWebUser.ProjectId = webUser.ProjectId;
                returnedWebUser.Message = "Invalid User Name / Password. Please try again.";
                return returnedWebUser;
            }
            return returnedWebUser;
        }

        //    [HttpGet]
        //    [ActionName("GetAllUsers")]
        //    [Route("[action]")]
        //    public List<WebViewer>  GetAllUsers()
        //    {
        //        List<WebViewer> returnedWebUsers = WebViewerRepository.GetAllUsers(Request, HttpContext.Response);
        //        if (returnedWebUsers == null || returnedWebUsers.Count==0)
        //        {
        //            WebViewer returnedWebUser = new WebViewer();
        //            returnedWebUser.IsSucessfull = false;
        //            //returnedWebUser.ProjectId = webUser.ProjectId;
        //            returnedWebUser.Message = "Invalid User Name / Password. Please try again.";
        //            returnedWebUsers = new List<WebViewer>();
        //            returnedWebUsers.Add(returnedWebUser);
        //            return returnedWebUsers;
        //        }
        //        return returnedWebUsers;
        //    }

        //    [HttpPost]
        //    [ActionName("AddThirdPartyUser")]
        //    [Route("[action]")]
        //    public WebViewer AddThirdPartyUser([FromBody]WebViewer Thirdparty)
        //    {
        //        WebViewer returnedWebUser = WebViewerRepository.AddUThirsPartyUser(Thirdparty, HttpContext.Request);
        //        if (returnedWebUser == null)
        //        {
        //            return returnedWebUser;
        //        }
        //        return returnedWebUser;
        //    }

        //    [HttpGet]
        //    [ActionName("GetUsersById")]
        //    [Route("[action]")]
        //    public WebViewer GetUsersById(int UserId)
        //    {
        //        WebViewer returnedWebUsers = WebViewerRepository.GetUsersByUserId(UserId, Request, HttpContext.Response);

        //        if (returnedWebUsers == null)
        //        {
        //            return returnedWebUsers;
        //        }
        //        return returnedWebUsers;
        //    }

        //    [HttpPost]
        //    [ActionName("UpdateUser")]
        //    [Route("[action]")]
        //    public WebViewer UpdateUser([FromBody]WebViewer webUser)
        //    {
        //        WebViewer returnedWebUser = WebViewerRepository.UpdateUser(webUser, HttpContext.Request);
        //        if (returnedWebUser == null)
        //        {
        //            return returnedWebUser;
        //        }
        //        return returnedWebUser;
        //    }

        //    [HttpGet]
        //    [ActionName("UniqueUser")]
        //    [Route("[action]")]
        //    public bool UniqueUser(string webUser)
        //    {
        //       List<WebViewer> returnedWebUser = WebViewerRepository.CheckUserAsUniq(webUser, HttpContext.Request);
        //        if (returnedWebUser == null || returnedWebUser.Count==0)
        //        {
        //            return false;
        //        }
        //        return true;
        //    }

        //    [HttpGet]
        //    [ActionName("GetAllUserByEntityId")]
        //    [Route("[action]")]
        //    public List<WebViewer> GetAllUserByEntityId(long webUser)
        //    {
        //        List<WebViewer> returnedWebUser = WebViewerRepository.GetAllUsersByEntityId(webUser,Request,HttpContext.Response);
        //        if (returnedWebUser == null || returnedWebUser.Count == 0)
        //        {
        //            return returnedWebUser;
        //        }
        //        return returnedWebUser;
        //    }

        //    [HttpGet]
        //    [ActionName("GetThirdPartyHomePage")]
        //    [Route("[action]")]
        //    public string GetThirdPartyHomePage()
        //    {
        //        return WebViewerRepository.GetThirdPartyHomePageUrl(Request);
        //    }

        //    [HttpPost]
        //    [ActionName("DeleteUser")]
        //    [Route("[action]")]
        //    public bool DeleteUser([FromBody]WebViewer webUser)
        //    {
        //        bool returnedWebUser = WebViewerRepository.DeleteUser(webUser, Request);
        //        if (returnedWebUser == true)
        //        {
        //            return returnedWebUser;
        //        }
        //        return returnedWebUser;
        //    }

        //    [HttpPost]
        //    [ActionName("ChangePassword")]
        //    [Route("[action]")]
        //    public ResponseModel ChangePassword([FromBody]WebViewer webUser)
        //    {
        //        return WebViewerRepository.ChangePassword(webUser, Request);
        //    }
    }
}
