using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class UserNotificationController : Controller
    {

        private readonly IUserNotificationsRepository UserNotificationsRepository;
        public UserNotificationController(IUserNotificationsRepository userNotificationsRepository)
        {
            this.UserNotificationsRepository =userNotificationsRepository;
        }

        [HttpPost]
        [ActionName("InsertToken")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult InsertFirebaseToken(string fb_token)
        {
            return new ObjectResult(UserNotificationsRepository.InsertFirebaseToken(Request, fb_token));
        }

        [HttpPost]
        [ActionName("DeleteToken")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult DeleteFirebaseToken(string fb_token)
        {
            bool result = false;
            result = UserNotificationsRepository.DeleteUserToken(Request, fb_token);
            return new ObjectResult(new { result });
        }

        [HttpPost]
        [ActionName("UpdateNotification")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateNotification([FromBody]UserNotifications userNotification)
        {
            return new ObjectResult(UserNotificationsRepository.UpdateNotification(Request, userNotification));
        }

        [HttpGet]
        [ActionName("GetUserNotificationList")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetUserNotificationList(bool isonlyNonReadNotification)
        {
            return new ObjectResult(UserNotificationsRepository.GetUserNotificationList(Request, isonlyNonReadNotification));
        }

    }
}
