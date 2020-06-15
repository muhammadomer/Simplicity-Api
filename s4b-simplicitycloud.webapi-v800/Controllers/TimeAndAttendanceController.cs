
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;

using SimplicityOnlineBLL.Entities;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class TimeAndAttendanceController : Controller
    {
        private readonly ITimeAndAttendanceRepository TimeAndAttendanceRepository;
        private readonly ILogger<RefS4bFormsController> Logger;
        public TimeAndAttendanceController(ITimeAndAttendanceRepository timeAndAttendanceRepository, ILogger<RefS4bFormsController> logger)
        {
            this.TimeAndAttendanceRepository = timeAndAttendanceRepository;
            this.Logger = logger;
        }

        [HttpGet]
        [ActionName("GetBudget")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetBudget(int yearValue, int teamId, int locationId)
        {
            return new ObjectResult(TimeAndAttendanceRepository.GetBudget(Request, yearValue, teamId, locationId));
        }

        [HttpGet]
        [ActionName("GetRevenue")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetRevenue(int teamId, int locationId, DateTime? entryDate1, DateTime? entryDate2)
        {
            return new ObjectResult(TimeAndAttendanceRepository.GetRevenue(Request, teamId, locationId, entryDate1, entryDate2));
        }

        [HttpGet]
        [ActionName("Roster")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult Roster(DateTime? entryDate1, DateTime? entryDate2, long prReference, long summarySequence)
        {
            return new ObjectResult(TimeAndAttendanceRepository.GetRosterDetails(Request, entryDate1, entryDate2, prReference, summarySequence));
        }

        [HttpPost]
        [ActionName("UpdateBudget")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateBudget([FromBody]RequestModel reqModel)
        {
            return new ObjectResult(TimeAndAttendanceRepository.UpdateBudget(Request, reqModel));
        }

        [HttpPost]
        [ActionName("UpdateRevenue")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateRevenue([FromBody]RequestModel reqModel)
        {
            return new ObjectResult(TimeAndAttendanceRepository.UpdateRevenue(Request, reqModel));
        }

    }
}
