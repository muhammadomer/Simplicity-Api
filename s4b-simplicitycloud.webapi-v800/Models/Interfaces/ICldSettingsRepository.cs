using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface ICldSettingsRepository : IRepository
    {
        CldSettings GetCldSettingsBySettingName(HttpRequest request, string settingName);
        CldSettings GetCldSettingsBySettingName(string ProjectId, string settingName);
        List<CldSettings> GetCldSettingsAll(HttpRequest request);
        ResponseModel GetSmartAppSettings(HttpRequest request);
        ResponseModel GetDemoAppSettings(HttpRequest request);        
        ResponseModel GetGoogleAppSettings(HttpRequest request);
        ResponseModel GetAllCldAppSettings(HttpRequest request);
        ResponseModel GetFilingCabinetRootFolder(HttpRequest request);
        bool UpdateSettingsBySettingName(string projectId, string settingName, string settingValue);
    }
}
