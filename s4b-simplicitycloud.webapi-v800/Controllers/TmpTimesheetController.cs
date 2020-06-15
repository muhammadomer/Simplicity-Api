
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;

using SimplicityOnlineBLL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class TmpTimesheetController : Controller
    {
        private readonly ITmpTimesheetRepository TmpTimesheetRepository;
        private readonly ILogger<RefS4bFormsController> Logger;
        public TmpTimesheetController(ITmpTimesheetRepository tmpTimesheetRepository, ILogger<RefS4bFormsController> logger)
        {
            this.TmpTimesheetRepository = tmpTimesheetRepository;
            this.Logger = logger;
        }

        [HttpPost]
        [ActionName("CreateTimeSheet")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel CreateTimeSheet([FromBody]TmpTimesheet timesheet)
        {
            ResponseModel response = new ResponseModel();
            if (timesheet != null && timesheet.DateRowFinishTime < timesheet.DateRowStartTime)
            {
                response.IsSucessfull = false;
                response.ProjectId = Request.Headers["ProjectId"];
                response.Message = "Finish time should be less then start time.";
                return response;
            }
            else {
                return TmpTimesheetRepository.CreateTimeSheetData(Request, timesheet);
            }
        }
    }
}
