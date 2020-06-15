
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using SimplicityOnlineWebApi.BLL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class RefDiaryAppRatesController : Controller
    {
        private readonly IRefDiaryAppRatesRepository RefDiaryAppRatesRepository;
        public RefDiaryAppRatesController(IRefDiaryAppRatesRepository refDiaryAppRatesRepository )
        {
            this.RefDiaryAppRatesRepository=refDiaryAppRatesRepository;
        }

      
        [HttpPost]
        [ActionName("AddDiaryAppRates")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult AddStatusTypes([FromBody]RefDiaryAppRates model)
        {
            RefDiaryAppRates genericLabels = RefDiaryAppRatesRepository.AddDiaryAppRates(model, HttpContext.Request);
            if (genericLabels == null)
            {
                return new ObjectResult(genericLabels);
            }
            return new ObjectResult(genericLabels);
        }

        [HttpPost]
        [ActionName("UpdateDiaryAppRates")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateStatusTypes([FromBody]RefDiaryAppRates model)
        {
            RefDiaryAppRates genericLabels = RefDiaryAppRatesRepository.UpdateDiaryAppRates(model, HttpContext.Request);
            if (genericLabels == null)
            {
                return new ObjectResult(genericLabels);
            }
            return new ObjectResult(genericLabels);
        }

        [HttpGet]
        [ActionName("GetDiaryAppRatesById")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetStatusTypesById(long Id)
        {
            List<RefDiaryAppRates> genericLabels = RefDiaryAppRatesRepository.GetDiaryAppRatesById(Id, HttpContext.Request, HttpContext.Response);
            if (genericLabels == null)
            {
                return new ObjectResult(genericLabels);
            }
            return new ObjectResult(genericLabels);
        }


        [HttpGet]
        [ActionName("GetDiaryAppRates")]
        [Route("[action]")]
       // [ValidateRequestState]
        public IActionResult GetDiaryAppRates()
        {
            List<RefDiaryAppRates> genericLabels = RefDiaryAppRatesRepository.GetDiaryAppRates(HttpContext.Request, HttpContext.Response);
            if (genericLabels == null)
            {
                return new ObjectResult(genericLabels);
            }
            return new ObjectResult(genericLabels);
        }

    }
}
