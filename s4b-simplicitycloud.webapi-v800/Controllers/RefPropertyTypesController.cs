
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
    public class RefPropertyTypesController : Controller
    {
        private readonly IRefPropertyTypesRepository RefPropertyTypesRepository;
        public RefPropertyTypesController(IRefPropertyTypesRepository refPropertyTypesRepository)
        {
            this.RefPropertyTypesRepository =refPropertyTypesRepository;
        }

        [HttpGet]
        [ActionName("GetAllPropertyTypes")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllPropertyTypes()
        {
            List<RefPropertyType> refVisitStatusTypes = RefPropertyTypesRepository.GetAllPropertyTypes(Request, Response);
            if (refVisitStatusTypes == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(refVisitStatusTypes);
        }
    }
}
