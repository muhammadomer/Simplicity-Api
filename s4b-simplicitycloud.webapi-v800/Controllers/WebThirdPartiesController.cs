using SimplicityOnlineWebApi.Models.Interfaces;
using System.Collections.Generic;
using SimplicityOnlineBLL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class WebThirdPartiesController : Controller
    {
        private readonly IWebThirdPartiesRepository WebThirdPartiesRepository;
        public WebThirdPartiesController(IWebThirdPartiesRepository webThirdPartiesRepository)
        { this.WebThirdPartiesRepository =webThirdPartiesRepository; }

        // POST api/values
        [HttpPost]
        [ActionName("Login")]
        [Route("[action]")]
        public WebThirdParties Login([FromBody]WebThirdParties webUser)
        {
            WebThirdParties returnedWebUser = WebThirdPartiesRepository.Login(webUser.UserLogon, webUser.WebEnable, webUser.ProjectId, HttpContext.Response);
            if (returnedWebUser == null)
            {
                returnedWebUser = new WebThirdParties();
                returnedWebUser.IsSucessfull = false;
                returnedWebUser.ProjectId = webUser.ProjectId;
                returnedWebUser.Message = "Invalid User Name / Password. Please try again.";
                return returnedWebUser;
            }
            return returnedWebUser;
        }

        [HttpGet]
        [ActionName("GetAllUsers")]
        [Route("[action]")]
        public List<WebThirdParties>  GetAllUsers()
        {
            List<WebThirdParties> returnedWebUsers = WebThirdPartiesRepository.GetAllUsers(Request, HttpContext.Response);
            if (returnedWebUsers == null || returnedWebUsers.Count==0)
            {
                WebThirdParties returnedWebUser = new WebThirdParties();
                returnedWebUser.IsSucessfull = false;
                //returnedWebUser.ProjectId = webUser.ProjectId;
                returnedWebUser.Message = "Invalid User Name / Password. Please try again.";
                returnedWebUsers = new List<WebThirdParties>();
                returnedWebUsers.Add(returnedWebUser);
                return returnedWebUsers;
            }
            return returnedWebUsers;
        }

        [HttpPost]
        [ActionName("AddThirdPartyUser")]
        [Route("[action]")]
        public WebThirdParties AddThirdPartyUser([FromBody]WebThirdParties Thirdparty)
        {
            WebThirdParties returnedWebUser = WebThirdPartiesRepository.AddUThirsPartyUser(Thirdparty, HttpContext.Request);
            if (returnedWebUser == null)
            {
                return returnedWebUser;
            }
            return returnedWebUser;
        }

        [HttpGet]
        [ActionName("GetUsersById")]
        [Route("[action]")]
        public WebThirdParties GetUsersById(int UserId)
        {
            WebThirdParties returnedWebUsers = WebThirdPartiesRepository.GetUsersByUserId(UserId, Request, HttpContext.Response);
          
            if (returnedWebUsers == null)
            {
                return returnedWebUsers;
            }
            return returnedWebUsers;
        }

        [HttpPost]
        [ActionName("UpdateUser")]
        [Route("[action]")]
        public WebThirdParties UpdateUser([FromBody]WebThirdParties webUser)
        {
            WebThirdParties returnedWebUser = WebThirdPartiesRepository.UpdateUser(webUser, HttpContext.Request);
            if (returnedWebUser == null)
            {
                return returnedWebUser;
            }
            return returnedWebUser;
        }

        [HttpGet]
        [ActionName("UniqueUser")]
        [Route("[action]")]
        public bool UniqueUser(string webUser)
        {
           List<WebThirdParties> returnedWebUser = WebThirdPartiesRepository.CheckUserAsUniq(webUser, HttpContext.Request);
            if (returnedWebUser == null || returnedWebUser.Count==0)
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        [ActionName("GetAllUserByEntityId")]
        [Route("[action]")]
        public List<WebThirdParties> GetAllUserByEntityId(long webUser)
        {
            List<WebThirdParties> returnedWebUser = WebThirdPartiesRepository.GetAllUsersByEntityId(webUser,Request,HttpContext.Response);
            if (returnedWebUser == null || returnedWebUser.Count == 0)
            {
                return returnedWebUser;
            }
            return returnedWebUser;
        }

        [HttpGet]
        [ActionName("GetThirdPartyHomePage")]
        [Route("[action]")]
        public string GetThirdPartyHomePage()
        {
            return WebThirdPartiesRepository.GetThirdPartyHomePageUrl(Request);
        }

        [HttpPost]
        [ActionName("DeleteUser")]
        [Route("[action]")]
        public bool DeleteUser([FromBody]WebThirdParties webUser)
        {
            bool returnedWebUser = WebThirdPartiesRepository.DeleteUser(webUser, Request);
            if (returnedWebUser == true)
            {
                return returnedWebUser;
            }
            return returnedWebUser;
        }

        [HttpPost]
        [ActionName("ChangePassword")]
        [Route("[action]")]
        public ResponseModel ChangePassword([FromBody]WebThirdParties webUser)
        {
            return WebThirdPartiesRepository.ChangePassword(webUser, Request);
        }
    }
}
