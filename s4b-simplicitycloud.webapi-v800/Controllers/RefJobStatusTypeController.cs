
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
    public class RefJobStatusTypeController : Controller
    {
        private readonly IRefJobStatusTypeRepository RefJobStatusTypeRepository;
        private readonly ILogger<RefJobStatusTypeController> Logger;

        public RefJobStatusTypeController(IRefJobStatusTypeRepository refJobStatusTypeRepository, ILogger<RefJobStatusTypeController> logger)
        {
            this.RefJobStatusTypeRepository=refJobStatusTypeRepository;
            this.Logger = logger;
        }

        [HttpGet]
        [ActionName("GetAllJobStatusType")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllJobStatusType()
        {
            List<RefJobStatusType> JobStatus = RefJobStatusTypeRepository.GetAllJobStatusTypes(Request,Response);
            if (JobStatus == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(JobStatus);
        }

    }
}
