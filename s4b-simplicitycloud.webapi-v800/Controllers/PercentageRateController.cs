
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class PercentageRateController : Controller
    {

        private readonly IPercentageRateRepository PercentageRateRepository;
        private readonly ILogger<PercentageRateController> Logger;


        public PercentageRateController(IPercentageRateRepository percentageRateRepository, ILogger<PercentageRateController> logger)
        {
            this.PercentageRateRepository =percentageRateRepository;
            this.Logger = logger;
        }


        [HttpGet]
        [ActionName("GetAdjustmentCodes")]
        [Route("[action]")]
        public IActionResult GetAdjustmentCodes()
        {
            return new ObjectResult(PercentageRateRepository.GetAdjustmentCodes(HttpContext.Request));
        }

    }
}
