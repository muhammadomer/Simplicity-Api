using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IApplicationSettingsRepository : IRepository
    {
        List<ApplicationSettings> GetApplicationSettingsAll(HttpRequest request);
        ApplicationSettings GetApplicationSettingsBySettingId(HttpRequest request, string settingId);
        ApplicationSettings GetOrdersBillsLastInvoiceNo(HttpRequest request);
    }
}
