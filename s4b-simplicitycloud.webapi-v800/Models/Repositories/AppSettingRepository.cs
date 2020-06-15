using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.DAL;
using Newtonsoft.Json;
namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class AppSettingRepository : IAppSettingRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public AppSettingRepository()
        {
            //
        }
        public AppSettingRepository(bool _IsSecondaryDatabase, string _SecondaryDatabaseId)
        {
            this.IsSecondaryDatabase = _IsSecondaryDatabase;
            this.SecondaryDatabaseId = _SecondaryDatabaseId;
        }

        public AppSetting GetAppSetting(HttpRequest request)
        {
            AppSetting returnValue=new AppSetting();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    //---Get User Details
                    UserDetails retUser = userDB.getUserByUserId(Convert.ToInt32(request.Headers["UserId"]));
                    if (retUser != null)
                    {
                        User user = new User();
                        user.userId = retUser.UserId;
                        user.userName = retUser.UserName;
                        user.userEmail = retUser.UserEmail;
                        returnValue.User = user;
                    }
                    //---Get Filing Cabinet Root Folder Name
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    CldSettings cldSettings = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingFilingCabinetRootFolder);
                    if (cldSettings != null)
                    {
                        ProjectSetting _projectSetting = new ProjectSetting();
                        _projectSetting.FilingCabinetRootFolder = cldSettings.SettingValue;
                        returnValue.ProjectSetting = _projectSetting;
                    }
                    //---Get Google Drive settings
                    CldSettings googleDriveAPIContent = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingGoogleDriveAPI);
                    if (googleDriveAPIContent != null)
                    {
                        GoogleDriveAPIKeys api_list = JsonConvert.DeserializeObject<GoogleDriveAPIKeys>(googleDriveAPIContent.SettingValue);
                        returnValue.GoogleDriveAPIKey = api_list;
                    }
                    else
                    {
                        returnValue.Message = "Error: Google drive api setting does not exist";
                    }
                    //---Get Google Old Drive settings
                    CldSettings googleDriveAPIContentOld = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingOldGoogleDriveAPI);
                    if (googleDriveAPIContentOld != null)
                    {
                        GoogleDriveAPIKeys api_list = JsonConvert.DeserializeObject<GoogleDriveAPIKeys>(googleDriveAPIContentOld.SettingValue);
                        returnValue.GoogleDriveAPIKeyOld = api_list;
                    }
                    else
                    {
                        returnValue.Message = "Error: Google drive api setting does not exist";
                    }
                    //---Get Firebase settings
                    CldSettings firebaseAPI = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingFirebaseAPI);
                    if (firebaseAPI != null)
                    {
                        FirebaseAPIKeys api_list = JsonConvert.DeserializeObject<FirebaseAPIKeys>(firebaseAPI.SettingValue);
                        returnValue.FirebaseAPIKey = api_list;
                        returnValue.FirebaseAPIKey.serverKey = null;
                    }
                    else
                    {
                        returnValue.Message = "Error:Firebase api setting does not exist";
                    }
                    //---Get Generic Labels
                    Microsoft.Extensions.Primitives.StringValues userId;
                    request.Headers.TryGetValue("UserId", out userId);
                    if (userId.ToString() == "undefined")
                    {
                        int webUserId = Int32.Parse(request.Headers["WebId"]);
                    }   
                    if (settings != null)
                    {
                        RefGenericLabelsDB genericLabelsDB = new RefGenericLabelsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List <RefGenericLabels> genericLabels = genericLabelsDB.selectAllRef_Generic_Labels();
                        if (genericLabels != null)
                        {
                            returnValue.GenericLabels = genericLabels;
                        }else { 
                            returnValue.Message += "No Generic Label Found.";
                        }
                    }
                   
                    returnValue.IsSucessfull = true;
                }
                
            }catch(Exception ex)
            {
                returnValue.Message = Message = "Exception Occured While Getting APP Settings. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel GetAppSettingById(HttpRequest request,string SettingId)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    ApplicationSettingsDB applicationSettingDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<ApplicationSettings> applicationSettings = applicationSettingDB.selectAllApplication_SettingsSettingId(SettingId);
                    returnValue.TheObject = applicationSettings;
                    if (applicationSettings != null)
                    {
                        returnValue.IsSucessfull = true;
                        returnValue.StatusCode = 1;
                    }
                    
                }
                return returnValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
