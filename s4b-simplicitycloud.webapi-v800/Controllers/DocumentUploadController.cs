
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System;
using SimplicityOnlineWebApi.Models;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class DocumentUploadController : Controller
    {
        [HttpPost]
        [ActionName("FilesUpload")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult FilesUpload([FromBody] FileModel model)
        {
            List<DocumentUploadModel> modelList = new List<DocumentUploadModel>();
            //var form = Request.Form;                                                        
            //foreach (var file in form.Files)
            //{
            //    var filename = ContentDispositionHeaderValue
            //                                 .Parse(file.ContentDisposition)
            //                                 .FileName
            //                                 .Trim('"');
            //    var model = new DocumentUploadModel() { FileName = filename, Date = DateTime.Now.ToShortDateString(), Type = documentType, Status = "Pending" };
            //    modelList.Add(model);
            //}
            return new ObjectResult(modelList);
        }
        public class FileModel
        {
            public string documentType { get; set; }
            public List<DocumentUploadModel> Files { get; set; }
        }  
    }

}
