
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class DiaryAppsNaturalFormsController : Controller
    {
        private readonly IDiaryAppsNaturalFormsRepository DiaryAppsNaturalFormsRepository;
        public DiaryAppsNaturalFormsController(IDiaryAppsNaturalFormsRepository diaryAppsNaturalFormsRepository)
        { this.DiaryAppsNaturalFormsRepository =diaryAppsNaturalFormsRepository; }

        [HttpGet]
        [ActionName("GetAllNaturalFormsByFormSequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllNaturalFormsByFormSequence(long deSequence, long formSequence)
        {
            return new ObjectResult( DiaryAppsNaturalFormsRepository.GetAllNaturalFormsByFormSequence(Request, deSequence,formSequence));
        }

        [HttpGet]
        [ActionName("GetAllNaturalFormsByDESequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllNaturalFormsByDESequence(long deSequence)
        {
            return new ObjectResult(DiaryAppsNaturalFormsRepository.GetAllNaturalFormsByDESequence(Request, deSequence));
        }

        [HttpGet]
        [ActionName("GetUnassignedNaturalForms")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetUnassignedNaturalForms(long deSequence)
        {
            return new ObjectResult(DiaryAppsNaturalFormsRepository.GetUnassignedNaturalFormsOfDESequence(Request, deSequence));
        }
        [HttpPost]
        [ActionName("InsertNaturalForm")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult InsertNaturalForm(long deSequence,[FromBody]DiaryAppNaturalForm[] diaryAppsNaturalForm)
        {
            return new ObjectResult(DiaryAppsNaturalFormsRepository.InsertNaturalForm(diaryAppsNaturalForm,deSequence, Request));
        }

        [HttpPost]
        [ActionName("InsertPasteDiaryAppsNaturalForms")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult InsertPasteDiaryAppsNaturalForms([FromBody]DiaryAppNaturalForm diaryAppsNaturalForm)
        {
            return new ObjectResult(DiaryAppsNaturalFormsRepository.InsertPasteDiaryAppsNaturalForms(diaryAppsNaturalForm, Request));
        }

        [HttpPost]
        [ActionName("InsertTFRFromUnscheduled")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult InsertTFRFromUnscheduled(long deSequence, long deSequenceUnscheduled)
        {
            return new ObjectResult(DiaryAppsNaturalFormsRepository.InsertTFRFromUnscheduled(deSequence, deSequenceUnscheduled, Request));
        }

        [HttpPost]
        [ActionName("DeleteNaturalFormsBySequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult DeleteNaturalFormsBySequence(long sequence)
        {
            return new ObjectResult(DiaryAppsNaturalFormsRepository.DeleteNaturalFormsBySequence(sequence, Request));
        }

        [HttpPost]
        [ActionName("DeleteNaturalFormsByDESequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult DeleteNaturalFormsByDESequence(long deSequence)
        {
            return new ObjectResult(DiaryAppsNaturalFormsRepository.DeleteNaturalFormsByDESequence(deSequence,  Request));
        }
    }
}
