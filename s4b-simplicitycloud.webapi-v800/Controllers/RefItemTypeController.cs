
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
    public class RefItemTypeController : Controller
    {
        private readonly IRefItemTypeRepository RefItemTypeRepository;
        private readonly ILogger<RefTradeCodeTypeController> Logger;

        public RefItemTypeController(IRefItemTypeRepository refItemTypeRepository, ILogger<RefTradeCodeTypeController> logger)
        {
            this.Logger=logger;
            this.RefItemTypeRepository = refItemTypeRepository;
        }

        [HttpGet]
        [ActionName("GetItemType")]
        [Route("[action]")]
        public IActionResult GetItemType(bool isAllItems)
        {
            return new ObjectResult(RefItemTypeRepository.GetItemType(HttpContext.Request, isAllItems));
        }

    }
}
