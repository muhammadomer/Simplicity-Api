using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly ILogger<UserDetailsRepository> LOGGER;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public UserDetailsRepository(ILogger<UserDetailsRepository> logger)
        {
            this.LOGGER = logger;
        }

        //public UserDetails Login(string userLogon, string userEnable, string projectId,long nbrOfAllowedUsers)
         public UserDetails Login(string userLogon, string userEnable, string projectId)
      {
            bool validRequest = false;
            UserDetails user = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        validRequest = true;
                        UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        //----Find nbr of Logged In Users ----
                        //Utilities.WriteLog("Checking nbr of logged in users");
                        //int nbrOfLoggedInUsers = userDB.getNbrOfLoggedInUsers();
                        //Utilities.WriteLog("Nbr Of Logged In Users:" + nbrOfLoggedInUsers);
                        //if(nbrOfLoggedInUsers >= nbrOfAllowedUsers)
                        //{
                        //   Utilities.WriteLog("Number of logged In users exceeds the allowed users in your license:" + nbrOfAllowedUsers);
                        //   Message = "Number of logged In users exceeds the allowed users in your license ";
                        //   return user;
                        //}
                        //---------------------------------
                        //LOGGER.LogDebug("Checking User Logon exist: " + userLogon);
						bool retValue = userDB.IsUserLogonExist(userLogon);
						if (retValue == false)
						{
							//LOGGER.LogError("User Logon does not exist");
							//Message = "Username is invalid";
						}
						else
						{
							user = userDB.getUserByUserLogonAndEnable(userLogon, userEnable);
							if (user == null)
							{
								//LOGGER.LogError(userDB.ErrorMessage);
								Message = "Password is invalid";
							}
							else if (user.FlgDeleted)
							{
								Message = "User has been deleted or locked.";
							}
							else
							{
								UserSessionsRepository userSession = new UserSessionsRepository();
								string token = userSession.CreateUserSession(user.UserId, user.UserLogon, projectId);
								user.ProjectId = projectId;
								if (token != "")
								{
									DateTime tokenExpiry = DateTime.Now.AddHours(Convert.ToDouble(settings.SessionExpiryHours));
									user.StatusCode = (int)HttpStatusCode.OK;
									user.Token = token;
									user.IsSucessfull = true;
									user.TokenExpiry = tokenExpiry;
									ApplicationWebPagesDB webPageDb = new ApplicationWebPagesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
									user.ApplicationWebPagesList = webPageDb.GetAllApplicationWebPages();
									//----Get Owner Name
									EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
									EntityDetailsCore entity = DetailsCoreDB.getEntityByEntityid(1);
									user.OwnerName = entity.NameLong;
									//---Get All CloudSettings
									CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
									List<CldSettings> cldSettings = cldSettingsDB.SelectAllCldSettings();
									user.CldSettings = cldSettings;
									//---Get All App Settings
									ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
									List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAll();
									user.AppSettings = applicationSettings;
									//----Get Generic Labels
									RefGenericLabelsDB genericLabelsDB = new RefGenericLabelsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
									List<RefGenericLabels> genericLabels = genericLabelsDB.selectAllRef_Generic_Labels();
									if (genericLabels != null)
									{
										user.GenericLabels = genericLabels;
									}

								}
								else
								{
									//LOGGER.LogError("Error Occured while Creating User Session " + userSession.Message);
									Message = "Error Occured while Creating User Session " + userSession.Message;
									user.ApplicationWebPagesList = null;
									user.IsSucessfull = false;
								}
							}
						}
                    }
                }
                if (!validRequest)
                {
                    Message = "Invalid Project Id";
                }
            }
            catch (Exception ex)
            {
                //LOGGER.LogError("Checking User: " + userLogon);
				Message = "Error occured while trying to login:" + ex.Message;
			}
            return user;
        }

        public UserDetails Login(int userId, string projectId, HttpResponse Response)
        {
            bool validRequest = false;
            UserDetails user = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        validRequest = true;
                        UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        //LOGGER.LogDebug("Checking userid: " + userId);

                        user = userDB.getUserByUserId(userId);
                        if (user == null)
                        {
                            LOGGER.LogError(userDB.ErrorMessage);
                            Response.Headers["message"] = "Invalid userId.";
                        }
                        else if (user.FlgDeleted)
                        {
                            Response.Headers["message"] = "User has been deleted or locked.";
                        }
                        else
                        {
                            UserSessionsRepository userSession = new UserSessionsRepository();
                            string token = userSession.CreateUserSession(user.UserId, user.UserLogon, projectId);
                            user.ProjectId = projectId;
                            if (token != "")
                            {
                                user.StatusCode = (int)HttpStatusCode.OK;
                                user.Token = token;
                                DateTime tokenExpiry = DateTime.Now.AddHours(Convert.ToDouble(settings.SessionExpiryHours));
                                user.TokenExpiry = tokenExpiry;
                                user.IsSucessfull = true;
                                ApplicationWebPagesDB webPageDb = new ApplicationWebPagesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                user.ApplicationWebPagesList = webPageDb.GetAllApplicationWebPages();

                              
                                //----Get Owner Name
                                EntityDetailsCoreDB DetailsCoreDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                EntityDetailsCore entity = DetailsCoreDB.getEntityByEntityid(1);
                                user.OwnerName = entity.NameLong;
                                //---Get All CloudSettings
                                CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                List<CldSettings> cldSettings = cldSettingsDB.SelectAllCldSettings();
                                user.CldSettings = cldSettings;
                                //---Get All App Settings
                                ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAll();
                                user.AppSettings = applicationSettings;
                                //----Get Generic Labels
                                RefGenericLabelsDB genericLabelsDB = new RefGenericLabelsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                List<RefGenericLabels> genericLabels = genericLabelsDB.selectAllRef_Generic_Labels();
                                if (genericLabels != null)
                                {
                                    user.GenericLabels = genericLabels;
                                }

                            }
                            else
                            {
                                //LOGGER.LogError("Error Occured while Creating User Session " + userSession.Message);
                                user.ApplicationWebPagesList = null;
                                user.IsSucessfull = false;
                                user.Message = "User has been deleted or locked.";
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
                //LOGGER.LogError("Checking userId: " + userId);
            }
            return user;
        }

        public List<UserDetails> GetAllUsers(HttpRequest Request, HttpResponse Response)
        {
            List<UserDetails> returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                int userId = Int32.Parse(Request.Headers["UserId"]);
                string token = Request.Headers["token"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = userDB.getAllUsers();
                        if (returnValue == null)
                        {
                            Response.Headers["message"] = "No User Found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Headers["message"] = "Exception occured while getting all Users. " + ex.Message;
            }
            return returnValue;
        }

        public List<UserDetails> GetUsersByUserName(string userName, HttpRequest Request, HttpResponse Response)
        {
            List<UserDetails> returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = userDB.GetUsersByUserName(userName);
                    if (returnValue == null)
                    {
                        Response.Headers["message"] = "No User Found.";
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnValue;
        }

      public List<UserDetails> GetUsersByUserId(long userId, HttpRequest Request, HttpResponse Response)
      {
         List<UserDetails> returnValue = null;
         try
         {
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
            if (settings != null)
            {
               UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
               returnValue = userDB.GetUsersByUserId(userId);
               if (returnValue == null)
               {
                  Response.Headers["message"] = "No User Found.";
               }
            }
         }
         catch (Exception ex)
         {
            Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
         }
         return returnValue;
      }
      public long GetTotalUsers(HttpRequest Request)
        {
            long returnValue = 0;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = userDB.GetTotalUsers();
                }

            }
            catch(Exception ex) { throw ex; }
            return returnValue;
        }
        public UserDetails GetUserByUserId(int userId, HttpRequest request)
        {
            UserDetails returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = userDB.getUserByUserId(userId);
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public bool LogOut(HttpRequest Request, HttpResponse Response)
        {
            bool validRequest = false;
            bool returnValue = false;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                int userId = Int32.Parse(Request.Headers["UserId"]);
                string token = Request.Headers["token"];
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    validRequest = true;
                    UserSessionsDB userSession = new UserSessionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (userSession.updateUserSessionExpiry(token, userId, DateTime.Now))
                    {
                        Response.StatusCode = (int)HttpStatusCode.OK;
                        Response.Headers["message"] = "User has been logged out,";
                        returnValue = true;
                    }
                    else
                    {
                        Response.Headers["message"] = "Error occured while logging user out.";
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
            return returnValue;
        }

        public ResponseModel AddUser(UserDetails userDetail, HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            UserDetails user = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        //----check that username is unique
                        List<UserDetails > existUser = CheckUserAsUniq(userDetail.UserLogon,-1, request);
                        if (existUser.Count == 0)
                        {
                            int userId = Convert.ToInt32(request.Headers["UserId"]);
                            userDetail.UserLevel = 1;
                            userDetail.CreatedBy = userId;
                            userDetail.DateCreated = DateTime.Now;
                            userDetail.LastAmendedBy = -1;
                            //userName.DateLastAmended = new DateTime();
                            UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                            user = userDB.AddUser(userDetail);
                            returnValue.TheObject = user;
                            returnValue.IsSucessfull = true;
                        }
                        else
                        {
                            returnValue.IsSucessfull = false;
                            returnValue.Message = "Logon name already exist. Please type distinct name";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.IsSucessfull = false;
                returnValue.Message = "Error occured:" + ex.Message;
            }
            return returnValue;
        }

        public ResponseModel UpdateUser(UserDetails userDetail, HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            UserDetails User = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    //----check that username is unique
                    List<UserDetails> existUser = CheckUserAsUniq(userDetail.UserLogon,userDetail.UserId ,request);
                    if (existUser.Count == 0)
                    {
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        userDetail.LastAmendedBy = userId;
                        userDetail.DateLastAmended = DateTime.Now;
                        UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        User = userDB.UpdateUser(userDetail);
                        returnValue.TheObject = User;
                        returnValue.IsSucessfull = true;
                    }
                    else
                    {
                        returnValue.IsSucessfull = false;
                        returnValue.Message = "Logon name already exist. Please type distinct name";
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.IsSucessfull = false;
                returnValue.Message = "Error occured:" + ex.Message;
            }
            return returnValue;
        }

        public List<UserDetails> CheckUserAsUniq(string userName,long userId, HttpRequest request)
        {
            List<UserDetails> User = new List<UserDetails>();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        User = userDB.CheckUserAsUniq(userName,userId);
                    }
                }
            }
            catch (Exception ex)
            {
                User = new List<UserDetails>();
            }
            return User;
        }

        public UserDetails DisableOrEnable(UserDetails userName, HttpRequest request)
        {
            UserDetails user = new UserDetails();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    user = userDB.EnableOrDisabled(userName);
                }
            }
            catch (Exception ex)
            {
                user = null;
            }
            return user;
        }
        public UserDetails CheckUserLogon(UserDetails user, HttpRequest request)
        {
            UserDetails User = new UserDetails();
            bool IsValid = false;
            try
            {
                string projectId = user.ProjectId;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        User = userDB.IsUserExist(user.UserLogon);
                        if (User != null)
                        {
                            if (User.UserLogon != null && User.UserLogon != "")
                            {
                                if (User.UserEmail != null && User.UserEmail != "")
                                {
                                    if (Utilities.IsEmailValid(User.UserLogon) || Utilities.IsEmailValid(User.UserEmail))
                                    {
                                        IsValid = true;
                                    }
                                    else
                                    {
                                        User.Message = "User logon or email not valid.";
                                    }
                                }
                                else
                                {
                                    User.Message = "User email not found";
                                }
                            }
                            else
                            {
                                User.Message = "User logon not found";
                            }

                            if (IsValid)
                            {
                                EmailContact fromEmail = new EmailContact();
                                List<EmailContact> toEmail = new List<EmailContact>();
                                fromEmail.EmailAddress = settings.AdminEmailAddress;
                                fromEmail.FullName = "Admin";
                                User.UserEnable = Utilities.GenratePassword();
                                User.ProjectId = projectId;
                                string subject = "Simplicity Password";
                                string body = "Your new password is.";
                                body += "<br /><br />";
                                body += User.UserEnable;
                                toEmail.Add(new EmailContact { EmailAddress = User.UserEmail, FullName = User.UserName });
                                List<string> fileAttachmentsPaths = new List<string>();
                                Utilities.SendMail(fromEmail, toEmail, null, null, subject, body, fileAttachmentsPaths, "", "");
                                updateUserPassword(User);
                                User.Message = "Check Your Email.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                User = null;
            }
            return User;
        }
        void updateUserPassword(UserDetails user)
        {
            UserDetails User = new UserDetails();
            if (!string.IsNullOrWhiteSpace(user.ProjectId))
            {
                ProjectSettings settings = Configs.settings[user.ProjectId];
                if (settings != null)
                {
                    user.UserEnableReminder = "jshjshdjs";
                    UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    User = userDB.UpdateUser(user);
                }
            }
        }
        public UserDetails CheckPassword(UserDetails user)
        {
            UserDetails User = new UserDetails();
            try
            {
                string projectId = user.ProjectId;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        User = userDB.PasswordExist(user);

                    }
                }
            }
            catch (Exception ex)
            {
                User = null;
            }
            return User;
        }
        public UserDetails ResetPassword(UserDetails userName)
        {
            UserDetails User = null;
            try
            {
                string projectId = userName.ProjectId;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        userName.UserEnableReminder = "jshjshdjs";
                        UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        User = userDB.UpdateUser(userName);
                    }
                }
            }
            catch (Exception ex)
            {
                User = null;
            }
            return User;
        }
        public UserDetails GetUserByEmail(UserDetails user)
        {
            UserDetails userDetail = new UserDetails();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(user.ProjectId);
                if (settings != null)
                {
                    UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    userDetail = userDB.IsUserExist(user.UserEmail);
                }

            }
            catch (Exception ex) { throw ex; }
            return userDetail;
        }
        public UserDetails CheckForgotPasswordString(UserDetails user)
        {
            UserDetails userDetail = new UserDetails();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(user.ProjectId);
                if (settings != null)
                {
                    UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    userDetail = userDB.ForgotPasswordStringExist(user);
                }

            }
            catch (Exception ex) { throw ex; }
            return userDetail;
        }
        public UserDetails RecoverForgotPassword(UserDetails user)
        {
            UserDetails userDetail = new UserDetails();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(user.ProjectId);
                if (settings != null)
                {
                    UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    userDetail = userDB.RecoverForgotPassword(user);
                }

            }
            catch (Exception ex) { throw ex; }
            return userDetail;
        }
    }
}
