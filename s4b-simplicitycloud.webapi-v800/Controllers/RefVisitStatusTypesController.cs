
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using SimplicityOnlineWebApi.BLL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class RefVisitStatusTypesController : Controller
    {
        private readonly IRefVisitStatusTypesRepository RefVisitStatusTypesRepository;
        public RefVisitStatusTypesController(IRefVisitStatusTypesRepository refVisitStatusTypesRepository)
        {
            this.RefVisitStatusTypesRepository =refVisitStatusTypesRepository;
        }

        [HttpGet]
        [ActionName("GetAllVisitStatusTypes")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllVisitStatusTypes()
        {
            List<RefVisitStatusTypes> refVisitStatusTypes = RefVisitStatusTypesRepository.GetAllVisitStatusTypes(Request, Response);
            if (refVisitStatusTypes == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(refVisitStatusTypes);
        }
        [HttpPost]
        [ActionName("AddStatusTypes")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult AddStatusTypes([FromBody]RefVisitStatusTypes model)
        {
            RefVisitStatusTypes retObject = RefVisitStatusTypesRepository.AddVisitStatusTypes(model, HttpContext.Request);
            if (retObject == null)
            {
                return new ObjectResult(retObject);
            }
            return new ObjectResult(retObject);
        }

        [HttpPost]
        [ActionName("UpdateStatusTypes")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateStatusTypes([FromBody]RefVisitStatusTypes model)
        {
            RefVisitStatusTypes retObject = RefVisitStatusTypesRepository.UpdateVisitStatus(model, HttpContext.Request);
            if (retObject == null)
            {
                return new ObjectResult(retObject);
            }
            return new ObjectResult(retObject);
        }

        [HttpGet]
        [ActionName("GetStatusTypesById")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetStatusTypesById(int Id)
        {
            RefVisitStatusTypes genericLabels = RefVisitStatusTypesRepository.GetVisitStatusById(Id,HttpContext.Request, HttpContext.Response);
            if (genericLabels == null)
            {
                return new ObjectResult(genericLabels);
            }
            return new ObjectResult(genericLabels);
        }
    }
}
