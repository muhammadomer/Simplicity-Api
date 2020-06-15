using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IAppSettingRepository : IRepository
    {
        AppSetting GetAppSetting(HttpRequest request);
        ResponseModel GetAppSettingById(HttpRequest request, string SettingId);
    }
}
