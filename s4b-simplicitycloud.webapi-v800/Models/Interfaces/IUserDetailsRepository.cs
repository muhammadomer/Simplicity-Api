using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IUserDetailsRepository : IRepository
    {
        //UserDetails Login(string userLogon, string userEnable, string ProjectId,long nbrOfAllowedUsers);
      UserDetails Login(string userLogon, string userEnable, string ProjectId);
      UserDetails Login(int userId, string projectId, HttpResponse Response);
        bool LogOut(HttpRequest Request, HttpResponse Response);
        List<UserDetails> GetAllUsers(HttpRequest Request, HttpResponse Response);
        List<UserDetails> GetUsersByUserName(string userName, HttpRequest request, HttpResponse response);
        List<UserDetails> GetUsersByUserId(long userId, HttpRequest request, HttpResponse response);
        ResponseModel AddUser(UserDetails userName, HttpRequest request);
        ResponseModel UpdateUser(UserDetails userName, HttpRequest request);
        List<UserDetails> CheckUserAsUniq(string userName,long userId, HttpRequest request);
        UserDetails DisableOrEnable(UserDetails userName, HttpRequest request);
        UserDetails CheckUserLogon(UserDetails userName, HttpRequest request);
        UserDetails CheckPassword(UserDetails userName);
        UserDetails ResetPassword(UserDetails userName);
        long GetTotalUsers(HttpRequest request);
        UserDetails GetUserByEmail(UserDetails user);
        UserDetails CheckForgotPasswordString(UserDetails userName);
        UserDetails RecoverForgotPassword(UserDetails userName);
        UserDetails GetUserByUserId(int userId, HttpRequest request);
    }
}
