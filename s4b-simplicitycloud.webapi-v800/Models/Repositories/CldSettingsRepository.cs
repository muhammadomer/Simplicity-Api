using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using Newtonsoft.Json;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class CldSettingsRepository : ICldSettingsRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public CldSettingsRepository()
        {
        }

        public List<CldSettings> GetCldSettingsAll(HttpRequest request)
        {
            List<CldSettings> returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = cldSettingsDB.SelectAllCldSettings();
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured While Getting All Cld Settings. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public CldSettings GetCldSettingsBySettingName(HttpRequest request, string settingName)
        {
            CldSettings returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = cldSettingsDB.SelectAllCldSettingsBySettingName(settingName);
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured While Getting Cld Settings by Setting Name. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }
        public CldSettings GetCldSettingsBySettingName(string projectId, string settingName)
        {
            CldSettings returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(projectId);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = cldSettingsDB.SelectAllCldSettingsBySettingName(settingName);
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured While Getting Cld Settings by Setting Name. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool UpdateSettingsBySettingName(string projectId, string settingName, string settingValue)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(projectId);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = cldSettingsDB.updateBySettingId(settingName, settingValue);
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured While Updated Cld Settings by Setting Name. " + ex.Message + " " + ex.InnerException;
                Utilities.WriteLog(Message, "UpdateSettingsBySettingName");
            }
            return returnValue;
        }

        public string GetThirdPartyHomePageUrl(HttpRequest request)
        {
            string returnValue = "";
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    CldSettings cldSetting = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingThirdPartyWebHomePageUrl);
                    if (cldSetting != null)
                    {
                        returnValue = cldSetting.SettingValue;
                    }
                    else
                    {
                        returnValue = SimplicityConstants.ThirdPartyWebDefaultHomePageUrl;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured While Getting Third Party Home Page Url. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public string GetDefaultEmailForDistribution(HttpRequest request)
        {
            string returnValue = "";
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    CldSettings cldSetting = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingS4BFormDefaultDistributionEmailAddress);
                    if(cldSetting!=null)
                    {
                        returnValue = cldSetting.SettingValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured While Getting Default Email For Distribution. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public string GetFolderNameForMailMergeTemplates(HttpRequest request)
        {
            string returnValue = SimplicityConstants.FilingCabinetCustomFolderName;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    CldSettings cldSetting = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingFilingCabinetMailMergeTemplateName);
                    if (cldSetting != null)
                    {
                        returnValue = cldSetting.SettingValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured While Getting Folder Name for Mail Merge Templates. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public string GetS4BFormShowDiaryJobNotesBoth(HttpRequest request)
        {
            string returnValue = "0";
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    CldSettings cldSetting = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingS4BFormShowDiaryJobNotesBoth);
                    if (cldSetting != null)
                    {
                        returnValue = cldSetting.SettingValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured While Getting S4B Form Show Diary Job Notes Both. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool GetCldSettingIsOrderStatusUpdateEnabledOnS4BFormVisitStatusUpdate(HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    CldSettings cldSetting = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingIsOrderStatusUpdateEnabledOnS4BFormVisitStatusUpdate);
                    if (cldSetting != null)
                    {
                        if(!string.IsNullOrEmpty(cldSetting.SettingValue))
                        { 
                            returnValue =  bool.Parse(cldSetting.SettingValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured While Getting CldSetting IsOrderStatusUpdateEnabledOnS4BFormVisitStatusUpdate. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool GetIsS4BFormJobRefPaddedByZeros(HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    CldSettings cldSetting = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingIsS4BFormJobRefPaddedByZeros);
                    if (cldSetting != null)
                    {
                        if (!string.IsNullOrEmpty(cldSetting.SettingValue))
                        {
                            returnValue = bool.Parse(cldSetting.SettingValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured While Getting CldSetting IsS4BFormJobRefPaddedByZeros. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool GetCldSettingIsKPICompleteUpdateEnabledOnS4BFormVisitStatusUpdate(HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    CldSettings cldSetting = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingIsKPICompleteUpdateEnabledOnS4BFormVisitStatusUpdate);
                    if (cldSetting != null)
                    {
                        if (!string.IsNullOrEmpty(cldSetting.SettingValue))
                        {
                            returnValue = bool.Parse(cldSetting.SettingValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured While Getting CldSetting IsKPICompleteUpdateEnabledOnS4BFormVisitStatusUpdate. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool GetCldSettingIsUserControl1UpdateEnabledOnS4BFormVisitStatusUpdate(HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    CldSettings cldSetting = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingIsUserControl1UpdateEnabledOnS4BFormVisitStatusUpdate);
                    if (cldSetting != null)
                    {
                        if (!string.IsNullOrEmpty(cldSetting.SettingValue))
                        {
                            returnValue = bool.Parse(cldSetting.SettingValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured While Getting CldSetting IsUserControl1UpdateEnabledOnS4BFormVisitStatusUpdate. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel GetSmartAppSettings(HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<CldSettings> cldSettings = cldSettingsDB.SelectAllSmartCldSettings();
                    if (cldSettings != null)
                    {
                        returnValue.TheObject = cldSettings;
                        returnValue.IsSucessfull = true;
                    }
                    else
                    {
                        returnValue.Message = cldSettingsDB.ErrorMessage;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = "Exception Occured While Getting Smart App Cld Settings. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel GetAllCldAppSettings(HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<CldSettings> cldSettings = cldSettingsDB.SelectAllCldSettings();
                    if (cldSettings != null)
                    {
                        returnValue.TheObject = cldSettings;
                        returnValue.IsSucessfull = true;
                    }
                    else
                    {
                        returnValue.Message = cldSettingsDB.ErrorMessage;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = "Exception Occured While Getting All Cld Settings. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel GetDemoAppSettings(HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    CldSettings cldSettings = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingIsDemoModeEnabled);
                    if (cldSettings != null)
                    {
                        returnValue.TheObject = cldSettings;
                        returnValue.IsSucessfull = true;
                    }
                    else
                    {
                        returnValue.Message = cldSettingsDB.ErrorMessage;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = "Exception Occured While Getting Demo App Cld Settings. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel GetFilingCabinetRootFolder(HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    CldSettingsDB cldSettingsDB = new CldSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    CldSettings cldSettings = cldSettingsDB.SelectAllCldSettingsBySettingName(SimplicityConstants.CldSettingFilingCabinetRootFolder);
                    if (cldSettings != null)
                    {
                        returnValue.TheObject = cldSettings;
                        returnValue.IsSucessfull = true;
                    }
                    else
                    {
                        returnValue.Message = cldSettingsDB.ErrorMessage;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = "Exception Occured While Getting Demo App Cld Settings. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel GetGoogleAppSettings(HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                CldSettings googleDriveAPIContent = GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingGoogleDriveAPI);
                if (googleDriveAPIContent != null)
                {
                    GoogleAPIKeys api_list = JsonConvert.DeserializeObject<GoogleAPIKeys>(googleDriveAPIContent.SettingValue);
                    returnValue.TheObject = api_list;
                    returnValue.IsSucessfull = true;
                }
                else
                {
                    returnValue.Message = "Error: Google drive api setting does not exist";
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = "Exception Occured While Getting Google Drive API Settings. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }
    }
}