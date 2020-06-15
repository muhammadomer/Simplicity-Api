using Microsoft.AspNetCore.Http;

using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class WebViewerRepository : IWebViewerRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public WebViewerRepository()
        {
            
        }

        public WebViewer Login(string userLogon, string userEnable, string projectId, HttpResponse Response)
        {
            bool validRequest = false;
            WebViewer webViewer = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        if (settings != null)
                        {
                            validRequest = true;
                            WebViewerDB userDB = new WebViewerDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                            //settings.DatabaseType
                            webViewer = userDB.getUserByUserLogonAndEnable(userLogon, userEnable);
                            if (webViewer == null)
                            {
                                Response.Headers["message"] = "Invalid User Name or Password.";
                            }
                            else if (webViewer.FlgDeleted)
                            {
                                Response.Headers["message"] = "User has been deleted or locked.";
                            }
                            else
                            {
                                UserSessionsRepository userSession = new UserSessionsRepository();
                                string token = userSession.CreateUserSession(webViewer.ViewerId, webViewer.ViewerLogon, projectId);
                                webViewer.ProjectId = projectId;
                                if (token != "")
                                {
                                    webViewer.StatusCode = (int)HttpStatusCode.OK;
                                    webViewer.Token = token;
                                    webViewer.IsSucessfull = true;
                                    //if successfull then load menues
                                    ApplicationWebPagesDB webPageDb = new ApplicationWebPagesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                    webViewer.ApplicationWebPagesList = webPageDb.GetAllApplicationWebPages();
                                    //Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(user));
                                }
                                else
                                {
                                    webViewer.ApplicationWebPagesList = null;
                                    webViewer.IsSucessfull = false;
                                    webViewer.Message = "User has been deleted or locked.";
                                }
                            }
                        }
                    }
                }
                if (!validRequest)
                {
                    Response.Headers["message"] = "Invalid Project Id";
                }
            }
            catch (Exception ex)
            {
            }
            return webViewer;
        }

        //public List<WebThirdParties> GetAllUsers(HttpRequest Request, HttpResponse Response)
        //{
        //    List<WebThirdParties> returnValue = null;
        //    try
        //    {
        //        string projectId = Request.Headers["ProjectId"];
        //        int userId = Int32.Parse(Request.Headers["WebId"]);
        //        string token = Request.Headers["token"];
        //        if (!string.IsNullOrWhiteSpace(projectId))
        //        {
        //            ProjectSettings settings = Configs.settings[projectId];
        //            if (settings != null)
        //            {
        //                WebThirdPartiesDB userDB = new WebThirdPartiesDB(Utilities.getDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
        //                returnValue = userDB.getAllUsers();
        //                if (returnValue == null)
        //                {
        //                    Response.Headers["message"] = "No User Found.";
        //                }
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Response.Headers["message"] = "Exception occured while getting all Users. " + ex.Message;
        //    }
        //    return returnValue;
        //}
        //public List<WebThirdParties> GetAllUsersByEntityId(long EntityId,HttpRequest Request, HttpResponse Response)
        //{
        //    List<WebThirdParties> returnValue = null;
        //    try
        //    {
        //        string projectId = Request.Headers["ProjectId"];
        //        if (!string.IsNullOrWhiteSpace(projectId))
        //        {
        //            ProjectSettings settings = Configs.settings[projectId];
        //            if (settings != null)
        //            {
        //                WebThirdPartiesDB userDB = new WebThirdPartiesDB(Utilities.getDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
        //                returnValue = userDB.getAllByEntity(EntityId);
        //                if (returnValue == null)
        //                {
        //                    Response.Headers["message"] = "No User Found.";
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Headers["message"] = "Exception occured while getting all Users. " + ex.Message;
        //    }
        //    return returnValue;
        //}

        //public string GetThirdPartyHomePageUrl(HttpRequest Request)
        //{
        //    string returnValue = "";
        //    try
        //    {
        //        string projectId = Request.Headers["ProjectId"];
        //        if (!string.IsNullOrWhiteSpace(projectId))
        //        {
        //            ProjectSettings settings = Configs.settings[projectId];
        //            if (settings != null)
        //            {
        //                CldSettingsRepository cldSettingsRepository = new CldSettingsRepository();
        //                returnValue = cldSettingsRepository.GetThirdPartyHomePageUrl(Request);
        //                if (returnValue == null)
        //                {
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Response.Headers["message"] = "Exception occured while getting all Users. " + ex.Message;
        //    }
        //    return returnValue;
        //}

        //public List<WebThirdParties> GetUsersByUserName(string userName, HttpRequest Request, HttpResponse Response)
        //{
        //    List<WebThirdParties> returnValue = null;
        //    try
        //    {
        //        string projectId = Request.Headers["ProjectId"];
        //        if (!string.IsNullOrWhiteSpace(projectId))
        //        {
        //            ProjectSettings settings = Configs.settings[projectId];
        //            if (settings != null)
        //            {
        //                WebThirdPartiesDB userDB = new WebThirdPartiesDB(Utilities.getDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
        //                returnValue = userDB.GetUsersByUserName(userName);
        //                if (returnValue == null)
        //                {
        //                    Response.Headers["message"] = "No User Found.";
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
        //    }
        //    return returnValue;
        //}

        //public bool LogOut(HttpRequest Request, HttpResponse Response)
        //{
        //    bool validRequest = false;
        //    bool returnValue = false;
        //    try
        //    {
        //        string projectId = Request.Headers["ProjectId"];
        //        int userId = Int32.Parse(Request.Headers["UserId"]);
        //        string token = Request.Headers["token"];
        //        if (!string.IsNullOrWhiteSpace(projectId))
        //        {
        //            ProjectSettings settings = Configs.settings[projectId];
        //            if (settings != null)
        //            {
        //                validRequest = true;
        //                UserSessionsDB userSession = new UserSessionsDB(Utilities.getDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
        //                if (userSession.updateUserSessionExpiry(token, userId, DateTime.Now))
        //                {
        //                    Response.StatusCode = (int)HttpStatusCode.OK;
        //                    Response.Headers["message"] = "User has been logged out,";
        //                    returnValue = true;
        //                }
        //                else
        //                {
        //                    Response.Headers["message"] = "Error occured while logging user out.";
        //                }
        //            }
        //        }
        //        if (!validRequest)
        //        {
        //            Response.Headers["message"] = "Invalid Project Id";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return returnValue;
        //}

        //public WebThirdParties AddUThirsPartyUser(WebThirdParties userName, HttpRequest request)
        //{
        //    WebThirdParties User = null;
        //    try
        //    {
        //        string projectId = request.Headers["ProjectId"];
        //        if (!string.IsNullOrWhiteSpace(projectId))
        //        {
        //            ProjectSettings settings = Configs.settings[projectId];
        //            if (settings != null)
        //            {
        //                WebThirdPartiesDB userDB = new WebThirdPartiesDB(Utilities.getDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
        //                User = userDB.AddUser(userName);
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        User = null;
        //    }
        //    return User;
        //}

        //public WebThirdParties UpdateUser(WebThirdParties userName, HttpRequest request)
        //{
        //    WebThirdParties User = null;
        //    try
        //    {
        //        string projectId = request.Headers["ProjectId"];
        //        if (!string.IsNullOrWhiteSpace(projectId))
        //        {
        //            ProjectSettings settings = Configs.settings[projectId];
        //            if (settings != null)
        //            {
        //                WebThirdPartiesDB userDB = new WebThirdPartiesDB(Utilities.getDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
        //                User = userDB.UpdateUser(userName);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        User = null;
        //    }
        //    return User;
        //}

        //public List<WebThirdParties> CheckUserAsUniq(string userName, HttpRequest request)
        //{
        //    List<WebThirdParties> User = new List<WebThirdParties>();
        //    try
        //    {
        //        string projectId = request.Headers["ProjectId"];
        //        if (!string.IsNullOrWhiteSpace(projectId))
        //        {
        //            ProjectSettings settings = Configs.settings[projectId];
        //            if (settings != null)
        //            {
        //                WebThirdPartiesDB userDB = new WebThirdPartiesDB(Utilities.getDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
        //                User = userDB.CheckUserAsUniq(userName);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        User = new List<WebThirdParties>();
        //    }
        //    return User;
        //}

        //public bool DeleteUser(WebThirdParties userName, HttpRequest request)
        //{
        //    bool user = false;
        //    try
        //    {
        //        string projectId = request.Headers["ProjectId"];
        //        if (!string.IsNullOrWhiteSpace(projectId))
        //        {
        //            ProjectSettings settings = Configs.settings[projectId];
        //            if (settings != null)
        //            {
        //                WebThirdPartiesDB userDB = new WebThirdPartiesDB(Utilities.getDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
        //                user = userDB.DeleteUser(userName);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        user = false;
        //    }
        //    return user;
        //}
        //public WebThirdParties GetUsersByUserId(int UserId, HttpRequest Request, HttpResponse Response)
        //{
        //    WebThirdParties returnValue = null;
        //    try
        //    {
        //        string projectId = Request.Headers["ProjectId"];
        //        if (!string.IsNullOrWhiteSpace(projectId))
        //        {
        //            ProjectSettings settings = Configs.settings[projectId];
        //            if (settings != null)
        //            {
        //                WebThirdPartiesDB userDB = new WebThirdPartiesDB(Utilities.getDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
        //                returnValue = userDB.GetUsersByUserId(UserId);
        //                if (returnValue == null)
        //                {
        //                    Response.Headers["message"] = "No User Found.";
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
        //    }
        //    return returnValue;
        //}

        //public ResponseModel ChangePassword(WebThirdParties webUser, HttpRequest request)
        //{
        //    ResponseModel returnValue = new ResponseModel();
        //    try
        //    {
        //        string projectId = request.Headers["ProjectId"];
        //        if (!string.IsNullOrWhiteSpace(projectId))
        //        {
        //            ProjectSettings settings = Configs.settings[projectId];
        //            if (settings != null)
        //            {
        //                WebThirdPartiesDB userDB = new WebThirdPartiesDB(Utilities.getDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
        //                WebThirdParties webUserDB = userDB.getUserByUserLogonAndEnable(webUser.UserName, webUser.WebEnable);
        //                if (webUserDB == null)
        //                {
        //                    returnValue.Message = "User could not be verified. Please double check your current Password. " + userDB.ErrorMessage;
        //                }
        //                else
        //                {
        //                    if (webUser.WebEnableNew.Equals(webUser.WebEnableNewConfirm))
        //                    {
        //                        WebThirdParties webUserUpdated = userDB.UpdateUserPassword(webUser);
        //                        if (webUserUpdated == null)
        //                        {
        //                            returnValue.Message = "Unable to Change User Password. " + userDB.ErrorMessage;
        //                        }
        //                        else
        //                        {
        //                            returnValue.Message = "Password has been changed. ";
        //                            returnValue.IsSucessfull = true;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        returnValue.Message = "New Passwords do not match. ";
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        returnValue.Message = "Exception occured while Changing Password for Third Party User. " + ex.Message;
        //    }
        //    return returnValue;

        //}
    }
}
