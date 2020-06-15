using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.BLL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class GoogleDriveController : Controller
    {
        private readonly ILogger<GoogleDriveController> LOGGER;
        private readonly ICldSettingsRepository CldSettingsRepository;

        public GoogleDriveController(ILogger<GoogleDriveController> _LOGGER, ICldSettingsRepository cldSettingsRepository)
        {
            this.LOGGER = _LOGGER;
            this.CldSettingsRepository =cldSettingsRepository;
        }

        [HttpGet]
        [ActionName("GetGoogleAPIToken")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetGoogleAPIToken(string scope)
        {
            AuthenticationToken token = GoogleAPI.getToken(scope, Request);
            if (token == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(token);
        }

        [HttpGet]
        [ActionName("GetGoogleAPISettings")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetGoogleAPISettings()
        {
            return new ObjectResult(CldSettingsRepository.GetGoogleAppSettings(Request));
        }


        
    }
}
