
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
    public class RefTradeCodeTypeController : Controller
    {
        private readonly IRefTradeCodeTypeRepository RefTradeCodeTypeRepository;
        private readonly ILogger<RefTradeCodeTypeController> Logger;
        public RefTradeCodeTypeController(IRefTradeCodeTypeRepository refTradeCodeTypeRepository, ILogger<RefTradeCodeTypeController> logger)
        {
            this.RefTradeCodeTypeRepository =refTradeCodeTypeRepository;
            this.Logger = logger;
        }

        [HttpGet]
        [ActionName("GetAllTradeCodes")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllTradeCodes()
        {
            List<RefTradeCodeType> tradeCodes = RefTradeCodeTypeRepository.GetAllTradeCodes(Request);
            if (tradeCodes == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(tradeCodes);
        }

    }
}
