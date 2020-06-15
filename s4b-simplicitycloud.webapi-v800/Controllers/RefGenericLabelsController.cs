using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class RefGenericLabelsController : Controller
    {
        private readonly IRefGenericLabelsRepository RefGenericLabelsRepository;
        private readonly ILogger<RefGenericLabelsController> LOGGER;
        public RefGenericLabelsController(ILogger<RefGenericLabelsController> _LOGGER, IRefGenericLabelsRepository refGenericLabelsRepository)
        {
            this.LOGGER = _LOGGER;
            this.RefGenericLabelsRepository = refGenericLabelsRepository;
        }

        [HttpGet]
        [ActionName("GetAllGenericLabels")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult GetAllGenericLabels()
        {
            List<RefGenericLabels> genericLabels = RefGenericLabelsRepository.GetAllGenericLabels(Request, Response);
            if (genericLabels == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(genericLabels);
        }
        [HttpPost]
        [ActionName("AddGenericLabels")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult AddGenericLabels([FromBody]RefGenericLabels Obj)
        {
            return new ObjectResult(RefGenericLabelsRepository.AddGenericLable(Obj,Request));
        }
        [HttpPost]
        [ActionName("UpdateGenericLabels")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateGenericLabels([FromBody]RefGenericLabels Obj)
        {
           return new ObjectResult(RefGenericLabelsRepository.UpdateGenericLable(Obj,Request));
        }
        [HttpGet]
        [ActionName("GetGenericLabelsById")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetGenericLabelsById(long Id)
        {
            RefGenericLabels genericLabels = RefGenericLabelsRepository.GetGenericLableById(Id,Request, Response);
            if (genericLabels == null)
            {
                return new ObjectResult(genericLabels);
            }
            return new ObjectResult(genericLabels);
        }
    }
}
