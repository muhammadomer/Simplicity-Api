
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using SimplicityOnlineWebApi.BLL.Entities;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AppSettingController : Controller
    {
        private readonly IAppSettingRepository AppSettingRepository;
        public AppSettingController(IAppSettingRepository appSettingRepository)
        {
            this.AppSettingRepository = appSettingRepository;
        }

        [HttpGet]
        [ActionName("GetAllAppSettings")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllAppSettings()
        {
            AppSetting appSetting = AppSettingRepository.GetAppSetting(Request);
           
            if (appSetting == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(appSetting);
        }

        [HttpGet]
        [ActionName("GetAppSettingById")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAppSettingById(string settingId)
        {
            return new ObjectResult(AppSettingRepository.GetAppSettingById(Request,settingId));
        }

    }
}
