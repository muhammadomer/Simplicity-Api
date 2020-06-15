using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class HealthAndCheckController : Controller
    {
        
        private IHealthAndCheckRepository HealthAndCheckRepository { get; set; }

       
        private  ILogger<HealthAndCheckController> Logger { get; set; }

        [HttpGet]
        [ActionName("GetQuestionList")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetQuestionList()
        {
            ResponseModel response = new ResponseModel();
            return HealthAndCheckRepository.GetQuestionList(Request);
        }

		[HttpGet]
		[ActionName("GetS4bCheckTypeByType")]
		[Route("[action]")]
		[ValidateRequestState]
		public ResponseModel GetS4bCheckTypeByType(int checkType)
		{
			ResponseModel response = new ResponseModel();
			return HealthAndCheckRepository.GetS4bCheckTypesByType(Request,checkType);
		}

		[HttpGet]
		[ActionName("GetS4bCheckPaymentTypes")]
		[Route("[action]")]
		[ValidateRequestState]
		public ResponseModel GetS4bCheckPaymentTypes()
		{
			ResponseModel response = new ResponseModel();
			return HealthAndCheckRepository.GetS4bCheckPymtTypes(Request);
		}

		[HttpPost]
		[ActionName("SaveS4bCheckAudit")]
		[Route("[action]")]
		[ValidateRequestState]
		public IActionResult SaveS4bCheckAudit([FromBody]S4bCheckAudit s4bCheckAudit)
		{
			return new ObjectResult(HealthAndCheckRepository.SaveS4bCheckAudit(Request, s4bCheckAudit));
		}

		[HttpPost]
		[ActionName("SaveS4bCheckTimeSheet")]
		[Route("[action]")]
		[ValidateRequestState]
		public IActionResult SaveS4bCheckTimeSheet([FromBody]S4bCheckTimeSheet s4bCheckTimeSheet)
		{
			return new ObjectResult(HealthAndCheckRepository.SaveS4bCheckTimesheet(Request, s4bCheckTimeSheet));
		}

		[HttpPost]
		[ActionName("DeleteS4bCheckTimeSheet")]
		[Route("[action]")]
		[ValidateRequestState]
		public IActionResult DeleteS4bCheckTimeSheet([FromBody]S4bCheckTimeSheet s4bCheckTimeSheet)
		{
			return new ObjectResult(HealthAndCheckRepository.DeleteS4bCheckTimesheetBySequence(Request, s4bCheckTimeSheet));
		}
	}
}
