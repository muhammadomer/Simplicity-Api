
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Microsoft.AspNetCore.Cors.EnableCors("CorsPolicy")]

    [Route("api/[controller]")]
    public class DiaryAppsController : Controller
    {
        private readonly IDiaryAppsRepository DiaryAppsRepository;
        public DiaryAppsController(IDiaryAppsRepository diaryAppsRepository)
        { this.DiaryAppsRepository =diaryAppsRepository; }

        [HttpGet]
        [ActionName("GetDiaryAppsWithSequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetDiaryAppsWithSequence(long sequence)
        {
            DiaryApps diaryApps = DiaryAppsRepository.GetDiaryAppsBySequence(sequence, Request, Response);
            if (diaryApps == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(diaryApps);
        }

        [HttpGet]
        [ActionName("GetDiaryAppsThirdPartyByEntityId")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult GetDiaryAppsThirdPartyByEntityId(long entityId)
        {
            List<DiaryApps> diaryApps = DiaryAppsRepository.GetDiaryAppsThirdPartyByEntityId(entityId, Request, Response);
            if (diaryApps == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(diaryApps);
        }

        [HttpGet]
        [ActionName("GetDiaryAppsByDate")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetDiaryAppsByDate(string start,string end)
        {
            DateTime? appointmentStartDate = Convert.ToDateTime(start.Substring(0, 24));
            DateTime? appointmentEndDate = Convert.ToDateTime(end.Substring(0, 24));
            List<DiaryApps> diaryApps = DiaryAppsRepository.GetDiaryAppsByDate(appointmentStartDate, appointmentEndDate, Request, HttpContext.Response);
            if (diaryApps == null)
            {
                return new ObjectResult(new List<DiaryApps>());
            }
            return new ObjectResult(diaryApps);
        }

        [HttpGet]
        [ActionName("GetDiaryAppsByDateAndJobRef")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetDiaryAppsByDateAndJobRef(string start, string end,string jobRef)
        {
            DateTime? appointmentStartDate = Convert.ToDateTime(start.Substring(0, 24));
            DateTime? appointmentEndDate = Convert.ToDateTime(end.Substring(0, 24));
            List<DiaryApps> diaryApps = DiaryAppsRepository.GetDiaryAppsByDateAndJobRef(appointmentStartDate, appointmentEndDate, jobRef, Request, HttpContext.Response);
            if (diaryApps == null)
            {
                return new ObjectResult(new List<DiaryApps>());
            }
            return new ObjectResult(diaryApps);
        }

        [HttpGet]
        [ActionName("GetDiaryAppsByJobSequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetDiaryAppsByJobSequence(long jobSequence)
        {   
            List<DiaryAppsHistory> diaryApps = DiaryAppsRepository.GetDiaryAppsByJobSequence(Request, HttpContext.Response, jobSequence);
            if (diaryApps == null)
            {
                return new ObjectResult(new List<DiaryAppsHistory>());
            }
            return new ObjectResult(diaryApps);
        }
        [HttpPost]
        [ActionName("CreatDiaryApps")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult CreatDiaryApps([FromBody]DiaryApps diaryApps)
        {
			return new ObjectResult(DiaryAppsRepository.CreateDiaryApps(diaryApps, Request));
			//DiaryApps diaryApp = DiaryAppsRepository.CreateDiaryApps(diaryApps, Request);
   //         if (diaryApp == null)
   //         {
   //             return new ObjectResult(HttpContext.Response);
				

			//}
   //         else {
   //             DiaryResponse response = new DiaryResponse();
   //             response.Sequence = diaryApp.Sequence;
   //             return new ObjectResult(response);
   //         }
            
        }
        [HttpPost]
        [ActionName("UpdateDiaryApp")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateDiaryApp([FromBody]DiaryApps diaryApps)
        {
            DiaryApps diaryApp = DiaryAppsRepository.UpdateDiaryApp(diaryApps, Request);
            if (diaryApp == null)
            {
                ResponseModel returnValue = new ResponseModel();
                returnValue.IsSucessfull = false;
                returnValue.Message = "Client already has appointment in this Time slot ";
                return new ObjectResult(returnValue);
            }
            else
            {
                DiaryResponse response = new DiaryResponse();
                response.Sequence = diaryApp.Sequence;
                return new ObjectResult(response);
            }
        }

        [HttpPost]
        [ActionName("DeleteDiaryApp")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult DeleteDiaryApp([FromBody]DiaryApps diaryApps)
        {
            Boolean success = DiaryAppsRepository.DeleteDiaryApp(diaryApps, Request);
            if (success == false)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(success);
        }

        [HttpGet]
        [ActionName("GetAppointmentsByAppDateAndUserId")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAppointmentsByAppDateAndUserId(DateTime? date)
        {
            List<DiaryApps> diaryApp = DiaryAppsRepository.GetAppoinmentsByAppDateAndUserId(date, Request);
            if (diaryApp == null)
            {
                return new ObjectResult(Utilities.generateResponseModelWithMessage("No appointment Found."));
            }
            return new ObjectResult(diaryApp);
        }

        [HttpGet]
        [ActionName("GetUserAppointmentsByAppDate")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetUserAppointmentsByAppDate(DateTime? date)
        {
            return new ObjectResult(DiaryAppsRepository.GetUserAppointmentsByAppDate(Request, date));
        }

        [HttpGet]
        [ActionName("GetUserAppointmentsByAppDateForTimeEntry")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetUserAppointmentsByAppDateForTimeEntry(DateTime? date)
        {
            return new ObjectResult(DiaryAppsRepository.GetUserAppointmentsByAppDateForTimeEntry(Request, date));
        }
    }
}
