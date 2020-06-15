using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class RefProductUnitController : Controller
    {
        private readonly IRefProductUnitRepository RefProductUnitRepository;
        private readonly ILogger<RefTradeCodeTypeController> Logger;
        public RefProductUnitController(IRefProductUnitRepository refProductUnitRepository, ILogger<RefTradeCodeTypeController> logger)
        {
            this.RefProductUnitRepository = refProductUnitRepository;
            this.Logger = logger;
        }

        [HttpGet]
        [ActionName("GetProductUnits")]
        [Route("[action]")]
        public IActionResult GetProductUnits()
        {
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            return new ObjectResult(RefProductUnitRepository.GetProductUnits(header));
        }

    }
}
