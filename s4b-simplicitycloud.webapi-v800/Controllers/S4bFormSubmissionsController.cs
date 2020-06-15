using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class S4bFormSubmissionsController : Controller
    {
        private readonly IS4bFormSubmissionRepository S4bSubmissionRepository;
        public S4bFormSubmissionsController(IS4bFormSubmissionRepository s4bSubmissionRepository)
        {
            this.S4bSubmissionRepository = s4bSubmissionRepository;
        }

        [HttpGet]
        [Route("[action]")]
        [ActionName("GetAllFormSubmissions")]
        //[ValidateRequestState]
        public IActionResult GetAllFormSubmissions()
        {
            List<S4bFormSubmissions> list = S4bSubmissionRepository.getFormSubmissionLsit(Request);
            if(list != null && list.Count > 0)
            {
                return new ObjectResult(list);
            }
            else
            {
                return new ObjectResult(list);
            }
        }

        [HttpPost]
        [ActionName("InsertSubmissions")]
        [ValidateRequestState]
        public IActionResult InsertSubmissions([FromBody] S4bFormSubmissions sub)
        {
            S4bFormSubmissions result = S4bSubmissionRepository.InsertFormSubmission(sub,Request);
            if (result != null)
            {
                return new ObjectResult(result);
            }
            else
            {
                return new ObjectResult(result);
            }
        }

        [HttpPost]
        [ActionName("UpdateSubmissions")]
        [ValidateRequestState]
        public IActionResult UpdateSubmissions([FromBody] S4bFormSubmissions sub)
        {
            bool result = S4bSubmissionRepository.UpdateFormSubmission(sub, Request);
            if (result)
            {
                return new ObjectResult(result);
            }
            else
            {
                return new ObjectResult(result);
            }
        }
        [HttpPost]
        [ActionName("S4BeFormsList")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult eFormSubmissionsList([FromBody]ClientRequest clientRequest)
        {
            return new ObjectResult(S4bSubmissionRepository.S4BeFormsList(clientRequest, Request));
        }

        [HttpGet]
        [ActionName("GetS4BeFormsListByJobSequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GeteFormSubmissionsByJobSequence(long jobSequence)
        {
            return new ObjectResult(S4bSubmissionRepository.getFormSubmissionListByJobSequence( Request, jobSequence));
        }

    }
}
