using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
   public interface IUserNotificationsRepository : IRepository
    {
        ResponseModel InsertFirebaseToken(HttpRequest request, string token);
        bool DeleteUserToken(HttpRequest request, string fb_token);
        ResponseModel UpdateNotification(HttpRequest request, UserNotifications obj);
        ResponseModel GetUserNotificationList(HttpRequest request, bool isOnlyNonReadNotification);
    }
}
