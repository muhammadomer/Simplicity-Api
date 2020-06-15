
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using SimplicityOnlineWebApi.BLL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class RefDiaryAppTypesController : Controller
    {
        private readonly IRefDiaryTypesRepository RefDiaryTypesRepository;
        public RefDiaryAppTypesController(IRefDiaryTypesRepository refDiaryTypesRepository)
        {
            this.RefDiaryTypesRepository = refDiaryTypesRepository;
        }

        [HttpGet]
        [ActionName("GetAllDiaryTypes")]
        [Route("[action]")]
       // [ValidateRequestState]
        public IActionResult GetAllDiaryTypes()
        {
            List<RefDiaryAppTypes> refVisitStatusTypes = RefDiaryTypesRepository.GetAllDiaryAppTypes(Request, Response);
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
        public IActionResult AddStatusTypes([FromBody]RefDiaryAppTypes model)
        {
            RefDiaryAppTypes genericLabels = RefDiaryTypesRepository.AddDiaryAppTypes(model, HttpContext.Request);
            if (genericLabels == null)
            {
                return new ObjectResult(genericLabels);
            }
            return new ObjectResult(genericLabels);
        }

        [HttpPost]
        [ActionName("UpdateStatusTypes")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateStatusTypes([FromBody]RefDiaryAppTypes model)
        {
            RefDiaryAppTypes genericLabels = RefDiaryTypesRepository.UpdateDiaryAppTypes(model, HttpContext.Request);
            if (genericLabels == null)
            {
                return new ObjectResult(genericLabels);
            }
            return new ObjectResult(genericLabels);
        }

        [HttpGet]
        [ActionName("GetStatusTypesById")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetStatusTypesById(long Id)
        {
            List<RefDiaryAppTypes> genericLabels = RefDiaryTypesRepository.GetDiaryAppTypesById(Id, HttpContext.Request, HttpContext.Response);
            if (genericLabels == null)
            {
                return new ObjectResult(genericLabels);
            }
            return new ObjectResult(genericLabels);
        }
    }
}
