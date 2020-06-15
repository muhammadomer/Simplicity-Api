
using Newtonsoft.Json;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System.Collections.Generic;

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class S4BFormsController : Controller
    {
        private readonly IS4BFormsRepository S4BFormsRepository;
        private readonly ILogger<S4BFormsController> Logger;

        public S4BFormsController(ILogger<S4BFormsController> logger, IS4BFormsRepository s4BFormsRepository)
        {
            this.Logger = logger;
            this.S4BFormsRepository = s4BFormsRepository;
        }

        [HttpPost]
        [ActionName("SubmitForm")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel SubmitForm()
        {
            Logger.LogInformation("In SubmitForm()");
            return S4BFormsRepository.ProcessSubmittedForm(Request);
        }

        [HttpPost]
        [ActionName("SubmitFormVideoFiles")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel SubmitFormVideoFile()
        {
            Logger.LogInformation("In SubmitFormVideo()");
            return S4BFormsRepository.ProcessSubmittedFormVideoFile(Request);
        }

        [HttpGet]
        [ActionName("GetAppointmentNotesSetting")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetAppointmentNotesSetting()
        {
            Logger.LogInformation("In GetAppointmentNotesSetting()");
            ResponseModel returnedResponse = S4BFormsRepository.GetAppointmentNotesSetting(Request);
            if (returnedResponse == null)
            {
                returnedResponse = new ResponseModel();
                returnedResponse.IsSucessfull = false;
                returnedResponse.Message = "Unable to GetAppointmentNotesSetting.";
            }
            return returnedResponse;
        }

        [HttpPost]
        [ActionName("GetPrepopulationData")]
        [Route("[action]")]
        [ValidateRequestState]
        public S4BFormPrepopulationDataModel GetPrepopulationData([FromBody]S4BFormPrepopulationDataRequest s4BFormPrepopulationDataRequest)
        {
            S4BFormPrepopulationDataModel returnedResponse = S4BFormsRepository.GetPrepopulationData(s4BFormPrepopulationDataRequest, Request);
            if (returnedResponse == null)
            {
                returnedResponse = new S4BFormPrepopulationDataModel();
                returnedResponse.IsSucessfull = false;
                returnedResponse.Message = "Unable to Process Submitted S4B Form.";
            }
            return returnedResponse;
        }

        /*--- **********************************************************************************************************************
            This method is written for s4bForm submission files which are not uploaded on Google Drive
            It will Get Files from given Path from Director and Upload it on Google Drive and Update FileCabId in Table against submission Record
        **************************************************************************************************************************** */
        [HttpPost]
        [ActionName("GetFilesAndUpdate")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetFilesAndUpdate(string filePath)
        {
            ResponseModel returnedResponse =  S4BFormsRepository.GetFilesAndUpdate(Request, filePath);
            if (returnedResponse == null)
            {
                returnedResponse = new ResponseModel();
                returnedResponse.IsSucessfull = false;
                returnedResponse.Message = "Unable to Get Files";
            }
            return returnedResponse;
        }
    }
}
