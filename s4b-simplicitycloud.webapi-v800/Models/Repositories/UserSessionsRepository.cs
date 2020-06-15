using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class UserSessionsRepository : IRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        //private ILogger<UserSessionsRepository> LOGGER;

        public UserSessionsRepository()
        {
            //LOGGER = _LOGGER;
        }

        public bool IsValidSession(int userId, string token, string projectId, out string message)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(projectId);
                UserSessionsDB userSessionDB = new UserSessionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                UserSessions session = userSessionDB.selectUserSession(userId, token);
                if(session!=null)
                {
                    if (session.TokenExpiry > DateTime.Now)
                    {
                        returnValue = true;
                        if (!session.FlgAdmin)
                        {
                            renewUserSession(session);
                        }
                    }
                    else
                    {
                        Message = "Session has been expired. Please login again.";
                    }
                }
                else
                {
                    Message = "Invalid Session. Please login again.";
                }
            }
            catch (Exception ex)
            {
                Message = "Error Occured while verifying the session. " + ex.Message + "\n\nPlease login again.";
                //LOGGER.LogError(Message);
            }
            message = Message;
            return returnValue;
        }

        private void renewUserSession(UserSessions session)
        {
            try
            {
                ProjectSettings settings = Configs.settings[session.ProjectId];
                UserSessionsDB userSessionDB = new UserSessionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                DateTime? newTokenExpiry = DateTime.Now.AddHours(Convert.ToDouble(settings.SessionExpiryHours));
                userSessionDB.updateUserSessionExpiry(session.Token, session.UserId, newTokenExpiry);
            }
            catch (Exception ex)
            {
                Message = "Error Occured while renewing user session. " + ex.Message + "\n\nPlease login again.";
                //LOGGER.LogError(Message);
            }
        }

        public string CreateUserSession(int userId, string userLogon, string projectId)
        {
            string returnValue = "";
            try
            {
                ProjectSettings settings = Configs.settings[projectId];
                UserSessionsDB userSession = new UserSessionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                //DateTime? tokenExpiry = DateTime.Now.AddHours(Convert.ToDouble(settings.SessionExpiryHours));
                DateTime? tokenExpiry = DateTime.Now.AddMinutes(Convert.ToDouble(1));
                string token = Guid.NewGuid().ToString("N");
                if(userSession.insertUserSession(token, userId, userLogon, projectId, tokenExpiry))
                { 
                    returnValue = token;
                }
                else
                {
                    Message = userSession.ErrorMessage;
                    //LOGGER.LogError(Message);
                }
            }
            catch (Exception ex)
            {
                Message = "Error Occured while Creating user session. " + ex.Message + "\n\nPlease login again.";
                //LOGGER.LogError(Message);
            }
            return returnValue;
        }
    }
}
