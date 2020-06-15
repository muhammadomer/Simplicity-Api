
using System;
//using System.Net.Http;
using System.Net;
using SimplicityOnlineWebApi.Models.Repositories;
using SimplicityOnlineBLL.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

//using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.Commons
{
    public class ValidateRequestStateAttribute : ActionFilterAttribute
    {
        //private ILogger<UserDetailsRepository> LOGGER;

        public ValidateRequestStateAttribute()
        {
            //LOGGER = _LOGGER;
        }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            try
            {
                string projectId = actionContext.HttpContext.Request.Headers["ProjectId"].ToString().ToUpper();
                string accessToken = actionContext.HttpContext.Request.Headers["Token"];
                string userId = actionContext.HttpContext.Request.Headers["UserId"];
                UserSessionsRepository userSession = new UserSessionsRepository();
                string message = "";
                //LOGGER.LogDebug("Validating session for Project Id: " + projectId + " and User Id: " + userId);
                if (!userSession.IsValidSession(Convert.ToInt32(userId), accessToken, projectId, out message))
                {
                    //LOGGER.LogError("Invalid session for Project Id: " + projectId + " and User Id: " + userId);
                    actionContext.Result = new UnauthorizedResult();
                }
            }
            catch (Exception ex)
            {
                actionContext.Result = new BadRequestObjectResult("Error occured while validating session. " +
                                         ex.Message + "\n\nPlease try again.");
                //LOGGER.LogError("Error occured while validating session. " + ex.Message);
            };
        }
    }
}
