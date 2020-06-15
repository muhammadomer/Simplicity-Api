
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class RefOrderTypeController : Controller
    {
        private readonly IRefOrderTypeRepository RefOrderTypeRepository;
        private readonly ILogger<RefOrderTypeController> Logger;

        public RefOrderTypeController(IRefOrderTypeRepository refOrderTypeRepository, ILogger<RefOrderTypeController> logger)
        {
            this.Logger = logger;
            this.RefOrderTypeRepository = refOrderTypeRepository;
        }

        [HttpGet]
        [ActionName("GetAllOrderTypes")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllOrderTypes()
        {
            List<RefOrderType> Orders = RefOrderTypeRepository.GetAllOrderTypes(Request);
            if (Orders == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(Orders);
        }

    }
}
