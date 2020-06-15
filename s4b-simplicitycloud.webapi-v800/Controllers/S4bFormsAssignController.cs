
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class S4bFormsAssignController : Controller
    {
        private readonly IS4bFormsAssignRepository S4bFormsAssignRepository;
        private readonly ILogger<S4bFormsAssignController> Logger;
        public S4bFormsAssignController(IS4bFormsAssignRepository s4bFormsAssignRepository, ILogger<S4bFormsAssignController> logger)
        {
            this.S4bFormsAssignRepository =s4bFormsAssignRepository;
            this.Logger = logger;
        }

        [HttpGet]
        [ActionName("GetAssigUsers")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAssigUsers(long formSequ)
        {
             
            return new ObjectResult(S4bFormsAssignRepository.GetAllAssignUser(formSequ, Request));
        }

        [HttpGet]
        [ActionName("GetUnAssigUsers")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetUnAssigUsers(long formSequ)
        {
            
            return new ObjectResult(S4bFormsAssignRepository.GetUnAssignUsers(formSequ, Request));
        }

        [HttpPost]
        [ActionName("UpdateFormUserAssignment")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateFormUserAssignment(long formSeq, [FromBody] List<long> assignUserIds)
        {
            bool result = false;
            try
            {
                result = S4bFormsAssignRepository.UpdateFormUserAssignment(formSeq, assignUserIds, Request);
            }
            catch (System.Exception ex)
            {
                result = false;
                Logger.LogError(ex.Message, ex);
            }

            return new ObjectResult(result);
        }


    }
}
