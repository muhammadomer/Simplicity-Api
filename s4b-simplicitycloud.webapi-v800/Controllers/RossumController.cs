using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models;
using SimplicityOnlineWebApi.Models.Interfaces;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class RossumController : Controller
    {
        private readonly IRossumRepository RossumRepository;
        protected readonly ILogger<RossumController> Logger;
        protected readonly ICldSettingsRepository CldSettingsRepository;

        public RossumController(IRossumRepository rossumRepository, ILogger<RossumController> logger, ICldSettingsRepository cldSettingsRepository)
        {
            this.RossumRepository = rossumRepository;
            this.Logger = logger;
        }

        [HttpPost]
        [ActionName("InsertFiles")]
        [Route("[action]")]
        public IActionResult InsertFiles([FromBody] List<RossumFile> receivedFilesList)
        {
            RequestHeaderModel header = new RequestHeaderModel();
            ResponseModel returnValue = new ResponseModel();
            header = Utilities.prepareRequestModel(Request);
            returnValue = RossumRepository.InsertRossumFiles(header, receivedFilesList);
            if (returnValue.IsSucessfull)
            {
                RossumRepository.ScheduleUploadToRossumAsync(header);
                returnValue = RossumRepository.GetUnConfirmedFilesList(header, null, null);
            }
            else {
                returnValue.Message = "file could not be saved properly. please contact support!";
            }
            return new ObjectResult(returnValue);
        }

        [HttpPost]
        [ActionName("DeleteFile")]
        [Route("[action]")]
        public IActionResult DeleteFile(long sequence)
        {
            ResponseModel returnValue = new ResponseModel();
            if (sequence > 1)
            {
                RequestHeaderModel header = new RequestHeaderModel();
                header = Utilities.prepareRequestModel(Request);
                returnValue = RossumRepository.DeleteRossumFile(sequence, header);
                if (!returnValue.IsSucessfull)
                    returnValue.Message = "file could not be deleted properly. please contact support!";
            }
            else
            {
                returnValue.IsSucessfull = false;
                returnValue.Message = "Invalid file sequence. please contact customer support.";
            }
            return new ObjectResult(returnValue);
        }

        [HttpPost]
        [ActionName("RossumFilesUpload")]
        [Route("[action]")]
        public IActionResult RossumFilesUpload(string documentType)
        {
            List<DocumentUploadModel> modelList = new List<DocumentUploadModel>();
            var form = Request.Form;
            ResponseModel rossumResult = new ResponseModel();
            ResponseModel listFiles = new ResponseModel();
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);

            if (form.Files.Count == 0) listFiles.Message = "No file attached to upload";
            foreach (var file in form.Files)
            {
                SimplicityFile simFile = new SimplicityFile();
                simFile.FileName = file.FileName;
                MemoryStream ms = new MemoryStream();
                simFile.MemStream = ms;
                file.CopyTo(simFile.MemStream);
                header.TheObject = documentType;
                //rossumResult = await RossumRepository.UploadDocToRossumAsync(header, simFile);
                listFiles = RossumRepository.GetUnConfirmedFilesList(header, null, null);

                //var filename = ContentDispositionHeaderValue
                //                             .Parse(file.ContentDisposition)
                //                             .FileName
                //                             .Trim('"');
                //var model = new DocumentUploadModel() { FileName = filename, Date = DateTime.Now.ToShortDateString(), Type = documentType, Status = "Pending" };
                //modelList.Add(model);
            }
            return new ObjectResult(listFiles.TheObject);
        }

        [HttpPost]
        [ActionName("GetUnConfirmedFilesList")]
        [Route("[action]")]
        public IActionResult GetUnConfirmedFilesList(string fromDate, string toDate)
        {
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            DateTime? FromDate = null; DateTime? ToDate = null;
            if (fromDate != null)
                FromDate = Convert.ToDateTime(fromDate.Substring(0, 24));
            if (toDate != null)
                ToDate = Convert.ToDateTime(toDate.Substring(0, 24));
            ResponseModel filesList = RossumRepository.GetUnConfirmedFilesList(header, FromDate, ToDate);
            //RossumRepository.GetandMoveFilesTest(Request, HttpContext.Response); //Invoices folder=> Read files from "Received" folder and move to "InReview" folder
            Utilities.WriteLog("METHOD CALLED: Get rossum unfinalised files");
            return new ObjectResult(filesList);
        }


        [HttpPost]
        [ActionName("GetAnnotationURL")]
        [Route("[action]")]
        public IActionResult GetAnnotationURL(int annotationId)
        {
            ResponseModel returnValue = new ResponseModel();
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);

            var response = RossumRepository.GetAnnotationURL(header, annotationId, String.Format("{0}://{1}", Request.Scheme, Request.Host.Value));
            returnValue.TheObject = response.Result.TheObject;
            returnValue.IsSucessfull = true;
            return new ObjectResult(returnValue);
        }
        [HttpPost]
        [ActionName("GetDocumentTypes")]
        [Route("[action]")]
        public IActionResult GetDocumentTypes()
        {
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            ResponseModel returnValue = new ResponseModel();

            returnValue.TheObject = RossumRepository.GetDocumentTypes(header);
            return new ObjectResult(returnValue);
        }

        #region Webhooks Region
        [HttpPost]
        [ActionName("RossumWebHook")]
        [Route("[action]")]
        public string RossumWebHook()
        {
            RequestHeaderModel header = new RequestHeaderModel();
            string streamText;
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                streamText = reader.ReadToEndAsync().Result;
                //...
            }
            RossWebHook resp = JsonConvert.DeserializeObject<RossWebHook>(streamText);

            int queueId = Convert.ToInt32(resp.annotation.queue.Split('/').Last());
            RossumSetting projectSetting = new RossumSetting();
            projectSetting = Configs.RossumSettings.Find(x => x.InvoicesQueueID == queueId);

            if (string.IsNullOrEmpty(projectSetting.ProjectId) || queueId==0)
            {
                string message = "Rossum Webhook Call Failed. Rossum settings could not be found.";
                Utilities.WriteLog(message, "RossumWebHook");
                return message;
            }
            header.ProjectId = projectSetting.ProjectId;
            header.UserId = projectSetting.UserId;

            var output = RossumRepository.RossumWebhook(resp, header);
            return output.Result;
        }

        [HttpPost]
        [ActionName("DebugRossumContent")]
        [Route("[action]")]
        public IActionResult DebugRossumContent(int annotationId)
        {
            ResponseModel returnValue = new ResponseModel();
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            string streamText;

            if (String.IsNullOrEmpty(header.ProjectId))
            {
                header.ProjectId = "DEMOMSSQL";
                header.UserId = 1;
            }
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                streamText = reader.ReadToEndAsync().Result;
            }
            //string debugMode = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_DEBUG_MODE).SettingValue;

            if (streamText.Length > 0)
            {
                RossumRepository.DebugContent = streamText;
                RossSchemaInvoice annotationData = JsonConvert.DeserializeObject<RossSchemaInvoice>(streamText);
            }
            var response = RossumRepository.SupplierInvoiceImportAsync(annotationId, header);
            return new ObjectResult(returnValue.TheObject);
        }

        [HttpPost]
        [ActionName("GetDebugContent")]
        [Route("[action]")]
        public IActionResult GetDebugContent(long rossumFileSequence)
        {
            RossumFile returnValue = new RossumFile();
            RequestHeaderModel header = new RequestHeaderModel();
            header = Utilities.prepareRequestModel(Request);
            returnValue = RossumRepository.GetDebugData(header, rossumFileSequence);
            return new ObjectResult(returnValue);
        }

        [HttpPost]
        [ActionName("SchedulerRossumMainCall")]
        [Route("[action]")]
        public IActionResult SchedulerRossumMainCall()
        {
            RossumRepository.SchedulerRossumMainCall();
            var a = new object();
            return new ObjectResult(a);
        }

        #endregion

        #region Connector Methods Rqgion

        [HttpPost]
        [ActionName("save")]
        [Route("[action]")]
        public string save(string a)
        {
            string content;
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                content = reader.ReadToEndAsync().Result;
                //...
            }
            var resp = JsonConvert.DeserializeObject(content);

            string lines = "--Webhook called--";
            Console.Write(content);
            //if (para != null) lines += para.ToString();

            //System.IO.File.WriteAllText(@"WriteLines.txt", lines);
            //this.Logger.LogError("WebHook Called by Postman - " + para.ToString()) ;
            return "Successful";
        }

        [HttpPost]
        [ActionName("validate")]
        [Route("[action]")]
        public RossumConValidResponse validate(RossSchemaContentSection validateDate)
        {
            string content;
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                content = reader.ReadToEndAsync().Result;
                //...
            }
            var resp = JsonConvert.DeserializeObject(content);

            string lines = "--Webhook called--";
            Console.Write(content);
            RossumConValidResponse returnValue = new RossumConValidResponse();
            //RossumConValidResMessage newMsg = new RossumConValidResMessage();
            //newMsg.content = "Ok";
            //returnValue.messages = new List<RossumConValidResMessage>() { newMsg };
            //Response.StatusCode = (int)HttpStatusCode.OK;
            return returnValue;
        }

        #endregion
        

        #region Archive Rqgion
        /* x
        //[HttpPost]
        //[ActionName("FileUpload")]
        //[Route("[action]")]
        //        public async Task<IActionResult> Index(List<IFormFile> files)
        //        {
        //            long size = files.Sum(f => f.Length);
        //            var filePaths = new List<string>();
        //            string rossumResult = "";
        //            try
        //            {
        //                foreach (var formFile in files)
        //                {
        //                    if (formFile.Length > 0)
        //                    {
        //                        SimplicityFile simFile = new SimplicityFile();
        //                        simFile.FileName = formFile.FileName;
        //                        MemoryStream ms = new MemoryStream();
        //                        simFile.MemStream = ms;
        //                        formFile.CopyTo(simFile.MemStream);
        //                         rossumResult = RossumRepository.UploadDocToRossumAsync("DEMOMSSQL", simFile).ToString();

        //                        //var filePath = Path.GetTempFileName(); //we are using Temp file name just for the example. Add your own file path.
        //                        //RossumRepository.UploadDocToRossum("DEMOMSSQL", filePath, formFile.FileName);

        //                        //FileInfo fileInfo = new FileInfo(formFile);
        //                        // full path to file in temp location
        //                        //var filePath = Path.GetTempFileName(); //we are using Temp file name just for the example. Add your own file path.
        //                        //filePaths.Add(filePath);

        //                        //using (var stream = new FileStream(filePath, FileMode.Create))
        //                        //{
        //                        //    await formFile.CopyToAsync(stream);                        
        //                        //}
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return Ok(new { message = ex });
        //            }
        //            return Ok(new { result = rossumResult});
        ////            return Ok(new { result= files.Count, size, filePaths });
        //        }
        //[HttpPost]
        //[ActionName("RossumLogin")]
        //[Route("[action]")]
        //public IActionResult RossumLogin()
        //{
        //    string token = RossumRepository.getToken(Request.Headers["ProjectId"]);
        //    return new ObjectResult(token);
        //}

        //[ActionName("FileInput")]
        //[Route("[action]")]
        //public IActionResult FileInput()
        //{
        //    return View();
        //}
        */
        #endregion Archive
    }
    

}
