
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
    public class RefOrderHireDamageTypeController : Controller
    {
        private readonly  IRefOrderHireDamageTypeRepository RefOrderHireDamageTypeRepository;
        private readonly ILogger<RefOrderTypeController> Logger;

        public RefOrderHireDamageTypeController(IRefOrderHireDamageTypeRepository refOrderHireDamageTypeRepository, ILogger<RefOrderTypeController> logger)
        {
            this.RefOrderHireDamageTypeRepository=refOrderHireDamageTypeRepository;
            this.Logger =logger;
        }

        [HttpGet]
        [ActionName("GetAllOrdHireDamageTypes")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllOrdHireDamageTypes()
        {
            return new ObjectResult( RefOrderHireDamageTypeRepository.GetAllDamageTypes(Request));
        }

    }
}
