
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SimplicityOnlineWebApi.BLL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AttachmentFilesFolderController: Controller
    {
        private ILogger<AttachmentFilesFolderController> LOGGER;
        private readonly IAttachmentFilesFolderRepository AttachmentFolderRepository;
        public AttachmentFilesFolderController(ILogger<AttachmentFilesFolderController> _LOGGER, IAttachmentFilesFolderRepository attachmentFolderRepository)
        {
            this.LOGGER= _LOGGER;
            this.AttachmentFolderRepository = attachmentFolderRepository;
        }


        [HttpGet]
        [ActionName("GetAttachmentFilesFolderByJobRef")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAttachmentFilesFolderByJobRef(string jobRef)
        {
            AttachmentFilesFolder attachments = AttachmentFolderRepository.GetAttachmentFilesFolderByFolderName(jobRef, Request);
            if (attachments == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(attachments);
        }

        [HttpGet]
        [ActionName("GetMailMergeFolderContents")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetMailMergeFolderContents()
        {
            return new ObjectResult(AttachmentFolderRepository.GetMailMergeFolderContents(Request));
        }

        [HttpGet]
        [ActionName("GetRootFolderContents")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetRootFolderContents()
        {
            AttachmentFilesFolder attachments = AttachmentFolderRepository.GetRootFolderContents(Request, HttpContext.Response);
            if (attachments == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(attachments);
        }
        [HttpGet]
        [ActionName("GetFolderContentsById")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetFolderContentsById(string folderId)
        {
            AttachmentFilesFolder attachments = AttachmentFolderRepository.GetFolderContentsById(folderId, Request, HttpContext.Response);
            if (attachments == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(attachments);
        }


        [HttpPost]
        [ActionName("AddFolder")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult AddFolder(string folderName, string parentFolderId)
        {
            DriveRequest request = new DriveRequest { Name = folderName, ParentFolderId = parentFolderId };
            AttachmentFilesFolder fodler = AttachmentFolderRepository.AddFolder(request, Request, HttpContext.Response);
            if (fodler == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(fodler);
        }

        [HttpPost]
        [ActionName("AddJobRefFolderWithSubFolders")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult AddJobRefFolderWithSubFolders(string folderName)
        {
            DriveRequest request = new DriveRequest { Name = folderName};
            AttachmentFilesFolder fodler = AttachmentFolderRepository.AddJobRefFolderWithSubFolders(request, Request, HttpContext.Response);
            if (fodler == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(fodler);
        }

        [HttpGet]
        [ActionName("RenameFolderByRootFolderName")]
        [ValidateRequestState]
        [Route("[action]")]
        public IActionResult RenameFolderByRootFolderName(string newFolderName, string oldFolderName)
        {
            DriveRequest request = new DriveRequest { Name = newFolderName, ParentFolderName = oldFolderName };
            AttachmentFilesFolder folder = AttachmentFolderRepository.RenameFolderByRootFolderName(request, Request, HttpContext.Response);
            ResponseModel response = new ResponseModel();
            if (folder == null)
            {
                response.IsSucessfull = false;
                response.Message = AttachmentFolderRepository.Message;
           }
            else
            {
                response.IsSucessfull = true;
            }
            return new ObjectResult(response);
        }

        [HttpPost]
        [ActionName("RenameFolder")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult RenameFolder(string folderName, string folderId)
        {
            DriveRequest request = new DriveRequest { Name = folderName, FolderId = folderId };
            AttachmentFilesFolder fodler = AttachmentFolderRepository.RenameFolder(request, Request, HttpContext.Response);
            if (fodler == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(fodler);
        }

        [HttpPost]
        [ActionName("AddFile")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult AddFile(string fileName, string parentFolderId, [FromBody]Cld_Ord_Labels_Files oiFireProtectionIImages)
        {
            DriveRequest request = new DriveRequest { Name = fileName, ParentFolderId = parentFolderId, FireProtectionImages=oiFireProtectionIImages };
            AttachmentFiles file = AttachmentFolderRepository.AddFile(request, Request, HttpContext.Response);
            if (file == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(file);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="parentFolderNames">Comma seperated folder names e.g. FolderA,FolderB,FolderC, this will create: SimplicityOnline/FolderA/FolderB/FolderC</param>
        /// <param name="oiFireProtectionIImages">Image stream</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("AddFileInSpecificFolder")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult AddFileInSpecificFolder(string fileName, string parentFolderNames, Cld_Ord_Labels_Files oiFireProtectionIImages)
        {
            AttachmentFiles file = UploadFileInSpecificFolder(fileName, parentFolderNames, oiFireProtectionIImages);
            if (file == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(file);
        }

        /// <summary>
        /// Two parameters fileName and parentFolderNames will go as queryString.
        /// Object OiFireProtectionIImages will go as json
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="parentFolderNames"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("AddFileInSpecificFolderByBase64")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult AddFileInSpecificFolderByBase64(string fileName, string parentFolderNames)
        {
            StreamReader reader = new StreamReader(Request.Body);
            JToken token = JObject.Parse(reader.ReadToEnd());
            Cld_Ord_Labels_Files oiFireProtectionIImages = JsonConvert.DeserializeObject<Cld_Ord_Labels_Files>(token["OiFireProtectionIImages"].ToString());
            AttachmentFiles file = UploadFileInSpecificFolder(fileName, parentFolderNames, oiFireProtectionIImages);
            if (file == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(file);
        }

        [HttpPost]
        [ActionName("CopyFile")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult CopyFile(string sourceFileId, string destinationFolderId)
        {
            DriveRequest request = new DriveRequest { FileId = sourceFileId };
            AttachmentFiles file = AttachmentFolderRepository.CopyFile(request, destinationFolderId, Request, HttpContext.Response);
            if (file == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(file);
        }

        [HttpPost]
        [ActionName("CopyFiles")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult CopyFiles([FromBody]AttachmentFilesFolderTransfer obj)
        {
            if (obj != null)
            {
                for (int index = 0; index < obj.SourceFilesId.Length; index++)
                {
                    DriveRequest request = new DriveRequest { FileId = obj.SourceFilesId[index] };
                    AttachmentFiles file = AttachmentFolderRepository.CopyFile(request, obj.DestinationFolderId, Request, HttpContext.Response);
                }
                return new ObjectResult(true);
            }
            else
            {
                return new ObjectResult(false);
            }
        }

        [HttpPost]
        [ActionName("MoveFile")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult MoveFile(string sourceFileId, string destinationFolderId)
        {
            DriveRequest request = new DriveRequest { FileId = sourceFileId };
            AttachmentFiles file = AttachmentFolderRepository.MoveFile(request, destinationFolderId, Request, HttpContext.Response);
            if (file == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(file);
        }

        [HttpPost]
        [ActionName("MoveFiles")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult MoveFiles([FromBody]AttachmentFilesFolderTransfer obj)
        {
            if (obj != null)
            {
                for (int index = 0; index < obj.SourceFilesId.Length; index++)
                {
                    DriveRequest request = new DriveRequest { FileId = obj.SourceFilesId[index] };
                    AttachmentFiles file = AttachmentFolderRepository.MoveFile(request, obj.DestinationFolderId, Request, HttpContext.Response);
                }
                return (new ObjectResult(true));
            }
            else
            {
                return new ObjectResult(false);
            }
        }

        private AttachmentFiles UploadFileInSpecificFolder(string fileName, string parentFolderNames, Cld_Ord_Labels_Files oiFireProtectionIImages)
        {
            DriveRequest request = new DriveRequest { Name = fileName, ParentFolderNames = parentFolderNames, FireProtectionImages = oiFireProtectionIImages };
            AttachmentFiles file = AttachmentFolderRepository.AddFileInSpecificFolder(request, Request, HttpContext.Response);
            return file;
        }

        [HttpGet]
        [ActionName("AddFileInConfigurableFolder")]
        [Route("[action]")]
        public IActionResult AddFileInConfigurableFolder()
        {
            string fileName = "Book1.xlsx";
            string jobRef = "job2";
            Cld_Ord_Labels_Files oiFireProtectionIImages = new Cld_Ord_Labels_Files();
            oiFireProtectionIImages.ImageName = "Book1.xlsx";
            oiFireProtectionIImages.Base64Img = "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";
            oiFireProtectionIImages.FlgIsBase64Img = true;
            DriveRequest request = new DriveRequest { Name = fileName, ParentFolderName = jobRef, FireProtectionImages = oiFireProtectionIImages };
            AttachmentFiles file = AttachmentFolderRepository.AddFileInSpecificFolder(request, Request, HttpContext.Response);
            if (file == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(file);
        }

        [HttpPost]
        [ActionName("RenameFile")]
        [Route("[action]")]
        public IActionResult RenameFile(string fileName, string fileId)
        {
            DriveRequest request = new DriveRequest { Name = fileName, FileId = fileId };
            AttachmentFiles file = AttachmentFolderRepository.RenameFile(request, Request, HttpContext.Response);
            if (file == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(file);
        }

        [HttpPost]
        [ActionName("DeleteFolder")]
        [Route("[action]")]
        public IActionResult DeleteFolder(string folderId)
        {
            DriveRequest request = new DriveRequest { FileId = folderId };
            bool flag = AttachmentFolderRepository.DeleteFile(request, Request, HttpContext.Response);
            if (!flag)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(flag);
        }

        [HttpPost]
        [ActionName("DeleteFile")]
        [Route("[action]")]
        public IActionResult DeleteFile(string fileId)
        {
            DriveRequest request = new DriveRequest { FileId = fileId };
            bool flag = AttachmentFolderRepository.DeleteFile(request, Request, HttpContext.Response);
            if (!flag)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(flag);
        }

        [HttpGet]
        [ActionName("DownloadFile")]
        [Route("[action]")]
        public IActionResult DownloadFile(string fileId)
        {
            //string fileId = "0Bzhp8zV0Leq1U0VMN0F5TUdDVlk"; //.xls
                                                            //string fileId = "0Bzhp8zV0Leq1bTkwMmxtaEVpNzA"; //service account
                                                            //string fileId = "1aFViq5z9KLdAPd8ZjzwEIBRx7FR4DILewaTxpphIJnA"; //me
                                                            //string fileId = "0Bzhp8zV0Leq1NkRqYVpwOVhIc2M"; //7mb mp3
                                                            //string fileId = "0B_EG--x9zFZpZDQ0bVBNRThRZzQ"; //11 mb javascript pdf
                                                            //DriveRequest request = new DriveRequest { FileId = fileId };

            SimplicityFile downloadFile = AttachmentFolderRepository.DowloadFile(fileId, Request, HttpContext.Response);
            if (downloadFile==null || downloadFile.MemStream == null)
            {
                return new ObjectResult("{\"StatusMessage\":\"File Not Found.\"}");
            }
            downloadFile.Base64String = Convert.ToBase64String(downloadFile.MemStream.ToArray());
            downloadFile.Extension = System.IO.Path.GetExtension(downloadFile.FileName).ToLower();
            downloadFile.MemStream = null;
            return new ObjectResult(downloadFile);
        }

        //[HttpGet]
        //[ActionName("GetFile")]
        //[Route("[action]")]
        //public HttpResponseMessage GetFile()
        //{
        //    var path = @"C:\Temp\KILNBRIDGE\Sheeta.xlsx";

        //    var stream = new System.IO.MemoryStream();
        //    // processing the stream.

        //    var result = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new ByteArrayContent(stream.GetBuffer())
        //    };
        //    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //    {
        //        FileName = path
        //    };
        //    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        //    return result;

        //    //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
        //    //var stream = new System.IO.FileStream(path, System.IO.FileMode.Open);
        //    //result.Content = new StreamContent(stream);
        //    //result.Content.Headers.ContentType= new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        //    //result.Content.Headers.ContentDisposition=new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    //result.Content.Headers.ContentDisposition.FileName = "Sheeta.xlsx";
        //    //stream.Close();
        //    //return result;


        //string str = "0M8R4KGxGuEAAAAAAAAAAAAAAAAAAAAAPgADAP7/CQAGAAAAAAAAAAAAAAABAAAAAQAAAAAAAAAAEAAAHwAAAAEAAAD+////AAAAAAAAAAD////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////9////IQAAAAMAAAAEAAAABQAAAAYAAAAHAAAACAAAAAkAAAAKAAAACwAAAAwAAAANAAAADgAAAA8AAAAQAAAAEQAAABIAAAATAAAAFAAAABUAAAAWAAAAFwAAABgAAAAZAAAAGgAAABsAAAAcAAAAHQAAAB4AAAD+/////v///yIAAAD+/////v///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////1IAbwBvAHQAIABFAG4AdAByAHkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAWAAUA//////////8CAAAAIAgCAAAAAADAAAAAAAAARgAAAAAAAAAAAAAAAICMBDF6ZtEBIAAAAIACAAAAAAAAVwBvAHIAawBiAG8AbwBrAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABIAAgEEAAAA//////////8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAAABTgAAAAAAAAFAFMAdQBtAG0AYQByAHkASQBuAGYAbwByAG0AYQB0AGkAbwBuAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKAACAQEAAAADAAAA/////wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADYAAAAAAAAAAUARABvAGMAdQBtAGUAbgB0AFMAdQBtAG0AYQByAHkASQBuAGYAbwByAG0AYQB0AGkAbwBuAAAAAAAAAAAAAAA4AAIB////////////////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAAAAOAAAAAAAAAACQgQAAAGBQCpH80HwQABAAYEAADhAAIAsATBAAIAAADiAAAAXABwAAgAAFRuREJsb2NrICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBCAAIAsARhAQIAAADAAQAAPQECAAEAnAACABAAGQACAAAAEgACAAAAEwACAAAArwECAAAAvAECAAAAPQASAPAAPABXTg4fOAAAAAAAAQBYAkAAAgAAAI0AAgAAACIAAgAAAA4AAgABALcBAgAAANoA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AAD1/yAAALQAAAAAAAAABKwg4AAUAAUAAAD1/yAAALQAAAAAAAAABLMg4AAUABUAAAD1/yAAALQAAAAAAAAABJ4g4AAUABUAAAD1/yAAALQAAAAAAAAABJ0g4AAUABUAAAD1/yAAALQAAAAAAAAABIsg4AAUABUAAAD1/yAAALQAAAAAAAAABKQg4AAUABUAAAD1/yAAALQAAAAAAAAABLEg4AAUABUAAAD1/yAAALQAAAAAAAAABLQg4AAUABUAAAD1/yAAALQAAAAAAAAABL4g4AAUABUAAAD1/yAAALQAAAAAAAAABIog4AAUABUAAAD1/yAAALQAAAAAAAAABLkg4AAUABUAAAD1/yAAALQAAAAAAAAABKQg4AAUABUAAAD1/yAAALQAAAAAAAAABLEg4AAUABUAAAD1/yAAALQAAAAAAAAABLUg4AAUAAsAAAD1/yAAALQAAAAAAAAABK0g4AAUAA8AAAD1/yAAAJQREZcLlwsABJYg4AAUABEAAAD1/yAAAJRmZr8fvx8ABLcg4AAUAAUAKwD1/yAAAPgAAAAAAAAAAMAg4AAUAAUAKQD1/yAAAPgAAAAAAAAAAMAg4AAUAAUALAD1/yAAAPgAAAAAAAAAAMAg4AAUAAUAKgD1/yAAAPgAAAAAAAAAAMAg4AAUABMAAAD1/yAAAPQAAAAAAAAAAMAg4AAUABcAAAD0/wAAAPQAAAAAAAAAAMAg4AAUAAoAAAD1/yAAALQAAAAAAAAABKog4AAUAAcAAAD1/yAAANQAUAAAAB8AAMAg4AAUAAgAAAD1/yAAANQAUAAAAAsAAMAg4AAUAAkAAAD1/yAAANQAIAAAAA8AAMAg4AAUAAkAAAD1/yAAAPQAAAAAAAAAAMAg4AAUABYAAAD0/wAAAPQAAAAAAAAAAMAg4AAUAA0AAAD1/yAAAJQREZcLlwsABK8g4AAUABAAAAD1/yAAANQAYAAAABoAAMAg4AAUAAwAAAD1/yAAALQAAAAAAAAABKsg4AAUAAUAAAD1/yAAAJwRERYLFgsABJog4AAUAA4AAAD1/yAAAJQREb8fvx8ABJYg4AAUAAUACQD1/yAAAPgAAAAAAAAAAMAg4AAUAAYAAAD1/yAAAPQAAAAAAAAAAMAg4AAUABQAAAD1/yAAANQAYQAAPh8AAMAg4AAUABIAAAD1/yAAAPQAAAAAAAAAAMAg4AAUABQAAAABACAAACgREUAgQCAAAsAgfAgUAHwIAAAAAAAAAAAAAAAAQQDPcLy9fQgtAH0IAAAAAAAAAAAAAAAAAAAAAAIADQAUAAMAAAABAAAAMDBcKTtfKCoOAAUAAn0ILQB9CAAAAAAAAAAAAAAAAAEAAAACAA0AFAADAAAAAQAAADAwXCk7XygqDgAFAAJ9CC0AfQgAAAAAAAAAAAAAAAACAAAAAgANABQAAwAAAAEAAAAwMFwpO18oKg4ABQACfQgtAH0IAAAAAAAAAAAAAAAAAwAAAAIADQAUAAMAAAABAAAAMDBcKTtfKCoOAAUAAn0ILQB9CAAAAAAAAAAAAAAAAAQAAAACAA0AFAADAAAAAQAAADAwXCk7XygqDgAFAAJ9CC0AfQgAAAAAAAAAAAAAAAAFAAAAAgANABQAAwAAAAEAAAAwMFwpO18oKg4ABQACfQgtAH0IAAAAAAAAAAAAAAAABgAAAAIADQAUAAMAAAABAAAAMDBcKTtfKCoOAAUAAn0ILQB9CAAAAAAAAAAAAAAAAAcAAAACAA0AFAADAAAAAQAAADAwXCk7XygqDgAFAAJ9CC0AfQgAAAAAAAAAAAAAAAAIAAAAAgANABQAAwAAAAEAAAAwMFwpO18oKg4ABQACfQgtAH0IAAAAAAAAAAAAAAAACQAAAAIADQAUAAMAAAABAAAAMDBcKTtfKCoOAAUAAn0ILQB9CAAAAAAAAAAAAAAAAAoAAAACAA0AFAADAAAAAQAAADAwXCk7XygqDgAFAAJ9CC0AfQgAAAAAAAAAAAAAAAALAAAAAgANABQAAwAAAAEAAAAwMFwpO18oKg4ABQACfQgtAH0IAAAAAAAAAAAAAAAADAAAAAIADQAUAAMAAAABAAAAMDBcKTtfKCoOAAUAAn0ILQB9CAAAAAAAAAAAAAAAAA0AAAACAA0AFAADAAAAAQAAADAwXCk7XygqDgAFAAJ9CC0AfQgAAAAAAAAAAAAAAAAOAAAAAgANABQAAwAAAAEAAAAwMFwpO18oKg4ABQACfQgtAH0IAAAAAAAAAAAAAAAADwAAAAIADQAUAAMAAAABAAAAMDBcKTtfKCoOAAUAAn0ILQB9CAAAAAAAAAAAAAAAACsAAAACAA0AFAADAAAAAQAAADAwXCk7XygqDgAFAAJ9CC0AfQgAAAAAAAAA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ADAwXCk7XygqDgAFAAIEABQAAwBlZgQAAAA7XyhAXykgIH0IQQB9CAAAAAAAAAAAAAAAABYAAAADAA0AFAADAAAAAQAAADAwXCk7XygqDgAFAAIEABQAAwDMTAQAAAA7XyhAXykgIH0IQQB9CAAAAAAAAAAAAAAAABwAAAADAA0AFAADAAAAAAAAADAwXCk7XygqDgAFAAIEABQAAwAyMwQAAAA7XyhAXykgIH0IQQB9CAAAAAAAAAAAAAAAACMAAAADAA0AFAADAAAAAAAAADAwXCk7XygqDgAFAAIEABQAAwAAAAUAAAA7XyhAXykgIH0IQQB9CAAAAAAAAAAAAAAAABEAAAADAA0AFAADAAAAAQAAADAwXCk7XygqDgAFAAIEABQAAwBlZgUAAAA7XyhAXykgIH0IQQB9CAAAAAAAAAAAAAAAABcAAAADAA0AFAADAAAAAQAAADAwXCk7XygqDgAFAAIEABQAAwDMTAUAAAA7XyhAXykgIH0IQQB9CAAAAAAAAAAAAAAAAB0AAAADAA0AFAADAAAAAAAAADAwXCk7XygqDgAFAAIEABQAAwAyMwUAAAA7XyhAXykgIH0IQQB9CAAAAAAAAAAAAAAAACQAAAADAA0AFAADAAAAAAAAADAwXCk7XygqDgAFAAIEABQAAwAAAAYAAAA7XyhAXykgIH0IQQB9CAAAAAAAAAAAAAAAABIAAAADAA0AFAADAAAAAQAAADAwXCk7XygqDgAFAAIEABQAAwBlZgYAAAA7XyhAXykgIH0IQQB9CAAAAAAAAAAAAAAAABgAAAADAA0AFAADAAAAAQAAADAwXCk7XygqDgAFAAIEABQAAwDMTAYAAAA7XyhAXykgIH0IQQB9CAAAAAAAAAAAAAAAAB4AAAADAA0AFAADAAAAAAAAADAwXCk7XygqDgAFAAIEABQAAwAyMwYAAAA7XyhAXykgIH0IQQB9CAAAAAAAAAAAAAAAACUAAAADAA0AFAADAAAAAAAAADAwXCk7XygqDgAFAAIEABQAAwAAAAcAAAA7XyhAXykgIH0IQQB9CAAAAAAAAAAAAAAAABMAAAADAA0AFAADAAAAAQAAADAwXCk7XygqDgAFAAIEABQAAwBlZgcAAAA7XyhAXykgIH0IQQB9CAAAAAAAAAAAAAAAABkAAAADAA0AFAADAAAAAQAAADAw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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEAAAACAAAAAwAAAP7///8FAAAABgAAAAcAAAD+////CQAAAP7//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////v8AAAYCAgAAAAAAAAAAAAAAAAAAAAAAAQAAAOCFn/L5T2gQq5EIACsns9kwAAAAqAAAAAcAAAABAAAAQAAAAAQAAABIAAAACAAAAFwAAAASAAAAcAAAAAwAAACIAAAADQAAAJQAAAATAAAAoAAAAAIAAADkBAAAHgAAAAwAAABUbkRCbG9jawAAAAAeAAAADAAAAFRuREJsb2NrAAAAAB4AAAAQAAAATWljcm9zb2Z0IEV4Y2VsAEAAAACAGcCR4lXQAUAAAAAA88dT7FXQAQMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP7/AAAGAgIAAAAAAAAAAAAAAAAAAAAAAAEAAAAC1c3VnC4bEJOXCAArLPmuMAAAALAAAAAIAAAAAQAAAEgAAAAXAAAAUAAAAAsAAABYAAAAEAAAAGAAAAATAAAAaAAAABYAAABwAAAADQAAAHgAAAAMAAAAiwAAAAIAAADkBAAAAwAAAAAADAALAAAAAAAAAAsAAAAAAAAACwAAAAAAAAALAAAAAAAAAB4QAAABAAAABwAAAFNoZWV0MQAMEAAAAgAAAB4AAAALAAAAV29ya3NoZWV0cwADAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAEMAbwBtAHAATwBiAGoAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEgACAP///////////////wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAByAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA////////////////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD///////////////8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP///////////////wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEA/v8DCgAA/////yAIAgAAAAAAwAAAAAAAAEYmAAAATWljcm9zb2Z0IE9mZmljZSBFeGNlbCAyMDAzIFdvcmtzaGVldAAGAAAAQmlmZjgADgAAAEV4Y2VsLlNoZWV0LjgA9DmycQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
        //return new ObjectResult(str);

        //HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);            
        //result.Content = new StreamContent(flag);
        //result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        //result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //{
        //    FileName = "Data.xls"
        //};
        //return result;



        //    ////HttpResponseMessage result = null;
        //    //HttpResponseMessage response = new HttpResponseMessage();
        //    //response.Content = new StreamContent(System.IO.File.Open(@"C:\Temp\KILNBRIDGE\Sheeta.xlsx",System.IO.FileMode.Open));
        //    //response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
        //    ////response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment"); //new ContentDispositionHeaderValue("attachment");
        //    //return new ObjectResult(response);

        //    // NOTE: Here I am just setting the result on the Task and not really doing any async stuff. 
        //    // But let's say you do stuff like contacting a File hosting service to get the file, then you would do 'async' stuff here.


        //    //var localFilePath = @"C:\Temp\KILNBRIDGE\Sheeta.xlsx";

        //    //if (!System.IO.File.Exists(localFilePath))
        //    //{
        //    //    result = Request.CreateResponse(HttpStatusCode.Gone);
        //    //}
        //    //else
        //    //{
        //    //    // Serve the file to the client
        //    //    result = Request.CreateResponse(HttpStatusCode.OK);
        //    //    result.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
        //    //    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    //    result.Content.Headers.ContentDisposition.FileName = "SampleImg";
        //    //}

        //    //return result;
        //}

        //[HttpGet]
        //[ActionName("UploadFile")]
        //[Route("[action]")]
        //public IActionResult UploadFile()
        //{
        //    OI_FireProtection_I_Images oiFireProtectionIImages = new OI_FireProtection_I_Images();
        //    oiFireProtectionIImages.ImageName = "bus.jpg";
        //    oiFireProtectionIImages.Base64Img = "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";
        //    oiFireProtectionIImages.FlgIsBase64Img = true;

        //    //DriveRequest request = new DriveRequest { FileId = fileId };
        //    AttachmentFiles file = AttachmentFolderRepository.UploadFile(oiFireProtectionIImages, Request, HttpContext.Response);
        //    if (string.IsNullOrEmpty(file.FilePath))
        //    {
        //        return new ObjectResult(false);
        //    }
        //    return new ObjectResult(true);
        //}


        /// <summary>
        /// This method uploads all the files and contents of provided folder to the google drive root folder.
        /// </summary>
        /// <param name="rootPath">Path of the root folder</param>
        [HttpGet]
        [ActionName("UploadDirectoryStructure")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel UploadDirectoryStructure(string rootPath, string rootFolderName)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                LOGGER.LogDebug("In Method UploadDirectoryStructure(). Root Path " + rootPath + ". RootFolder Name" + rootFolderName);
                Stack<string> dirs = new Stack<string>(20);
                if (!System.IO.Directory.Exists(Path.Combine(rootPath, rootFolderName)))
                {
                    throw new ArgumentException();
                }
                dirs.Push(Path.Combine(rootPath, rootFolderName));
                while (dirs.Count > 0)
                {
                    string currentDir = dirs.Pop();
                    string[] subDirs;
                    try
                    {
                        subDirs = System.IO.Directory.GetDirectories(currentDir);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        LOGGER.LogError(e.Message + " - " + e.InnerException);
                        continue;
                    }
                    catch (System.IO.DirectoryNotFoundException e)
                    {
                        LOGGER.LogError(e.Message + " - " + e.InnerException);
                        continue;
                    }
                    string[] files = null;
                    try
                    {
                        files = System.IO.Directory.GetFiles(currentDir);
                        if (files.Length == 0)
                        {
                            var folderNames = currentDir.Replace(rootPath, "").Replace("\\", ",");
                            AttachmentFiles attachmentFiles = UploadFileInSpecificFolder("", folderNames, null);
                        }
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        LOGGER.LogError(e.Message + " - " + e.InnerException);
                        continue;
                    }
                    catch (System.IO.DirectoryNotFoundException e)
                    {
                        LOGGER.LogError(e.Message + " - " + e.InnerException);
                        continue;
                    }
                    foreach (string file in files)
                    {
                        try
                        {
                            System.IO.FileInfo fi = new System.IO.FileInfo(file);
                            byte[] bytes = System.IO.File.ReadAllBytes(file);
                            string base64String = Convert.ToBase64String(bytes);
                            var folderNames = file.Replace(rootPath, "").Replace(fi.Name, "").Replace("\\", ",");
                            folderNames = folderNames.Substring(0, folderNames.Length - 1);
                            Cld_Ord_Labels_Files oiFireProtectionIImages = new Cld_Ord_Labels_Files();
                            oiFireProtectionIImages.ImageName = fi.Name;
                            oiFireProtectionIImages.Base64Img = base64String;
                            oiFireProtectionIImages.FlgIsBase64Img = true;
                            AttachmentFiles attachmentFiles = UploadFileInSpecificFolder(fi.Name, folderNames, oiFireProtectionIImages);
                        }
                        catch (System.IO.FileNotFoundException e)
                        {
                            LOGGER.LogError(e.Message + " - " + e.InnerException);
                            continue;
                        }
                    }
                    foreach (string str in subDirs)
                        dirs.Push(str);
                }
                response.IsSucessfull = true;
            }
            catch (Exception ex)
            {
                response.IsSucessfull = false;
                response.Message = ex.Message + " " + ex.InnerException;
                LOGGER.LogError(response.Message);
            }
            return response;
        }

        [HttpPost]
        [ActionName("ShareFile")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult ShareFile(string fileId, string email)
        {
            //string fileId = "0Bzhp8zV0Leq1Tzl0b3ZZNHduc3c";
            //string fileId = "0Bzhp8zV0Leq1UU05RV83NkdKdEU";
            //string email = "saeed.ahmed@msn.com";
            DriveRequest request = new DriveRequest { FileId = fileId, Email = email };
            var flag = AttachmentFolderRepository.ShareFile(request, Request, HttpContext.Response);
            return new ObjectResult(flag);
        }

        [HttpGet]
        [ActionName("GetFileSharedPerissions")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetFileSharedPerissions()
        {
            string fileId = "0Bzhp8zV0Leq1Tzl0b3ZZNHduc3c";            
            DriveRequest request = new DriveRequest { FileId = fileId};
            var flag = AttachmentFolderRepository.GetFileSharedPerissions(request, Request, HttpContext.Response);
            return new ObjectResult(flag);
        }

        [HttpGet]
        [ActionName("GetGoogleAPIToken")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetGoogleAPIToken(string scope)
        {
            AuthenticationToken token = GoogleAPI.getToken(scope, Request);
            if (token == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(token);
        }

        //---This method is added Temporarily for script of shifting Google drive to GSuit accounts from service acc. It is used to map file Id from old service acc to new g suite acc
        [HttpPost]
        [ActionName("InsertGDriveFileMapping")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult InsertGDriveFileMapping([FromBody]List<GDriveMapping> obj)
        {
            var ret = AttachmentFolderRepository.InsertGDriveFileMapping(Request, obj);
            if (ret == null)
            {
                return new ObjectResult(HttpContext.Response);
            }

            return new ObjectResult(ret);
        }
    }
}
