using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class RefS4bFormsController : Controller
    {
        private readonly IRefS4bFormsRepository RefS4bFormsRepository;
        private readonly ILogger<RefS4bFormsController> Logger;
        public RefS4bFormsController(IRefS4bFormsRepository refS4bFormsRepository, ILogger<RefS4bFormsController> logger)
        {
            this.RefS4bFormsRepository =refS4bFormsRepository;
            this.Logger = logger;
        }

        [HttpGet]
        [ActionName("GetAllNaturalForm")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllNaturalForm()
        {
            List<RefS4bForms> result = RefS4bFormsRepository.GetFormList(Request);
            if (result != null && result.Count > 0)
            {
                return new ObjectResult(result);
            }
            else
            {
                return new ObjectResult(result);
            }
        }

        [HttpGet]
        [ActionName("GetRecordsById")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetRecordsById(long formsequence)
        {
            RefS4bForms result = RefS4bFormsRepository.GetRecodsById(formsequence, Request);
            if (result != null && result.FormSequence > 0)
            {
                return new ObjectResult(result);
            }
            else
            {
                return new ObjectResult(result);
            }
        }

        [HttpPost]
        [ActionName("UpdateRecords")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateRecords([FromBody]RefS4bForms forms)
        {   
            return new ObjectResult(RefS4bFormsRepository.updateRecord(forms, Request));
        }

        [HttpPost]
        [ActionName("AddRecord")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult AddRecord([FromBody]RefS4bForms forms)
        {
            forms.RowIndex = 999;   // Default Value
            return new ObjectResult(RefS4bFormsRepository.AddRecord(forms, Request));
        }

        [HttpPost]
        [ActionName("JsonFileResource")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult JsonFileResource([FromBody]RefS4bForms refNaturalForm)
        {
            bool result = RefS4bFormsRepository.SaveJsonFile(refNaturalForm, Request);
            return new ObjectResult(result);
        }

        [HttpPost]
        [ActionName("PdfFileResource")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult PdfFileResource([FromBody]RefS4bForms refNaturalForm)
        {
            bool result = RefS4bFormsRepository.SavePdfFile(refNaturalForm, Request);
            return new ObjectResult(result);
        }

        [HttpPost]
        [ActionName("ReferencFileResource")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult ReferencFileResource([FromBody]List<RefS4bForms> refNaturalForms)
        {
            bool result = RefS4bFormsRepository.SaveReferenceFiles(refNaturalForms, Request);
            return new ObjectResult(result);
        }

        [HttpGet]
        [ActionName("DeleteTemplate")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult DeleteTemplate(long FormSeq)
        {
            bool result = RefS4bFormsRepository.DeleteTemplate(FormSeq, Request);
            return new ObjectResult(result);
        }

        [HttpGet]
        [ActionName("LoadDeleteTemplate")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult LoadDeleteTemplate()
        {
            List<RefS4bForms> result = RefS4bFormsRepository.LoadTemplateByFlgDelete(Request);
            if (result != null)
            {
                return new ObjectResult(result);
            }
            else
            {
                return new ObjectResult(result);
            }

        }

        [HttpGet]
        [ActionName("GetUserChangedTemplates")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetUserChangedTemplates(DateTime? lastSynDate)
        {
            try
            {
                List<RefS4bForms> result = RefS4bFormsRepository.GetUserChangedForms(lastSynDate, Request);

                return new ObjectResult(new { S4bFormTemplates = result, ServerProcessingTime = DateTime.Now });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
            return new ObjectResult(false);
        }

        [HttpGet]
        [ActionName("GetUserTemplatesBySyncDate")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetUserTemplatesBySyncDate(DateTime? lastSyncDate)
        {
            return new ObjectResult(RefS4bFormsRepository.GetUserTemplatesBySyncDate(Request, lastSyncDate));
        }

        [HttpGet]
        [ActionName("GetTemplateZipFile")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetTemplateZipFile(string formId)
        {
            try
            {
                var downloadFile = RefS4bFormsRepository.MakeZipFileOfTemplate(formId, Request);
                if (downloadFile != null && downloadFile.MemStream != null)
                {
                    /* Set custom headers to force browser to download the file instad of trying to open it */

                    return new FileStreamResult(downloadFile.MemStream, "application/x-zip-compressed")
                    {
                        FileDownloadName = "Archive.zip"
                    };
                    downloadFile.Base64String = Convert.ToBase64String(downloadFile.MemStream.ToArray());
                    downloadFile.MemStream = null;
                    return new ObjectResult(downloadFile);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
            return new ObjectResult(false);
        }

        [HttpPost]
        [ActionName("MakeTempUndelete")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult MakeTempUndelete(long formSeq)
        {
            bool result = RefS4bFormsRepository.UnDeleteTemp(formSeq, Request);

            return new ObjectResult(result);
        }

        [HttpPost]
        [ActionName("SetTemplateDefault")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult SetTemplateDefault(long formSeq)
        {
            bool result = RefS4bFormsRepository.SetTempDefault(formSeq, Request);

            return new ObjectResult(result);
        }

        [HttpGet]
        [ActionName("GetTemplateMapping")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetTemplateMapping(long formSeq)
        {
            List<RefS4bMapping> result = RefS4bFormsRepository.GetTemplateMapping(formSeq, Request);
            if (result != null && result.Any())
            {
                return new ObjectResult(result);
            }
            return new ObjectResult(null);
        }

        [HttpPost]
        [ActionName("AddTemplateMapping")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel AddTemplateMapping([FromBody]RefS4bMapping mapping)
        {
            ResponseModel returnedResponse = new ResponseModel();
            returnedResponse.IsSucessfull = false;
            try
            {
                var mappingId = RefS4bFormsRepository.AddTemplateMapping(mapping, Request);
                if (mappingId > 0)
                {
                    returnedResponse.IsSucessfull = true;
                    returnedResponse.TheObject = mappingId;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                returnedResponse.Message = ex.Message;
            }

            return returnedResponse;
        }

        [HttpPost]
        [ActionName("UpdateTemplateMapping")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel UpdateTemplateMapping([FromBody]RefS4bMapping mapping)
        {
            ResponseModel returnedResponse = new ResponseModel();
            try
            {
                returnedResponse.IsSucessfull = RefS4bFormsRepository.UpdateTemplateMapping(mapping, Request);                
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                returnedResponse.Message = ex.Message;
            }

            return returnedResponse;
        }

        [HttpPost]
        [ActionName("DeleteTemplateMapping")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel DeleteTemplateMapping(long sequence)
        {
            ResponseModel returnedResponse = new ResponseModel();
            try
            {
                returnedResponse.IsSucessfull = RefS4bFormsRepository.DeleteTemplateMapping(sequence, Request);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                returnedResponse.Message = ex.Message;
            }

            return returnedResponse;
        }

    }
}
