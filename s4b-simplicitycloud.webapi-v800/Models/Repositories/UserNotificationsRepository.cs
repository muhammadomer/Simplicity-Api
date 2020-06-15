using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.Commons;
using Microsoft.VisualBasic;
using SimplicityOnlineWebApi.DAL;
using Newtonsoft.Json;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using TheArtOfDev.HtmlRenderer.Core.Entities;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Drawing;


namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class UserNotificationsRepository: IUserNotificationsRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public UserNotificationsRepository()
        {
            
        }

        public ResponseModel InsertFirebaseToken(HttpRequest request, string token)
        {
            NotificationsToken result = new NotificationsToken();
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        UserNotificationsDB noteDB = new UserNotificationsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        NotificationsToken obj = new NotificationsToken();
                        long sequence = -1;
                        obj.FirebaseToken = token;      
                        obj.UserId = Convert.ToInt32(request.Headers["UserId"]);
                        obj.CreatedBy = Convert.ToInt32(request.Headers["UserId"]);
                        obj.DateCreated = DateTime.Now;
                        List<NotificationsToken> exist_token = noteDB.getTokenList(token);
                        if(exist_token.Count==0)
                        {
                            if (noteDB.insertFirebaseToken(out sequence, obj))
                            {
                                result = obj;
                                result.Sequence = sequence;
                                returnValue.TheObject = result;
                                returnValue.IsSucessfull = true;
                            }
                        }else
                        {
                            result = exist_token[0];
                            result.Sequence = exist_token[0].Sequence;
                            returnValue.TheObject = result;
                            returnValue.IsSucessfull = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }


        public bool DeleteUserToken(HttpRequest request,  string fb_token)
        {
            bool returnValue = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        UserNotificationsDB noteDB = new UserNotificationsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        if (noteDB.deleteByToken(userId, fb_token))
                        {
                            returnValue = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }


        public ResponseModel UpdateNotification(HttpRequest request, UserNotifications obj)
        {   
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        UserNotificationsDB noteDB = new UserNotificationsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        obj.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
                        obj.DateLastAmended = DateTime.Now;
                        if (noteDB.updateNotification(obj))
                        {   
                            returnValue.TheObject = obj;
                            returnValue.IsSucessfull = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public ResponseModel GetUserNotificationList(HttpRequest request, bool isOnlyNonReadNotification)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        UserNotificationsDB noteDB = new UserNotificationsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        returnValue.TheObject = noteDB.getUserNotificationList(userId, isOnlyNonReadNotification);
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = noteDB.ErrorMessage;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

    }
}
