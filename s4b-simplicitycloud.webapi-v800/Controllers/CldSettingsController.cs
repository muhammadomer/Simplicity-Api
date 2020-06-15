
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class CldSettingsController : Controller
    {
        private readonly ICldSettingsRepository CldSettingsRepository;
        public CldSettingsController (ICldSettingsRepository cldSettingsRepository )
            {this.CldSettingsRepository =cldSettingsRepository; }

        [HttpGet]
        [ActionName("GetSmartAppSettings")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetSmartAppSettings()
        {
            return new ObjectResult(CldSettingsRepository.GetSmartAppSettings(Request));
        }

        [HttpGet]
        [ActionName("GetDemoAppSettings")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetDemoAppSettings()
        {
            return new ObjectResult(CldSettingsRepository.GetDemoAppSettings(Request));
        }

        [HttpGet]
        [ActionName("GetAllAppSettings")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllAppSettings()
        {
            return new ObjectResult(CldSettingsRepository.GetAllCldAppSettings(Request));
        }

        [HttpGet]
        [ActionName("GetFilingCabinetRootFolder")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetFilingCabinetRootFolder()
        {
            return new ObjectResult(CldSettingsRepository.GetFilingCabinetRootFolder(Request));
        }
    }
}
