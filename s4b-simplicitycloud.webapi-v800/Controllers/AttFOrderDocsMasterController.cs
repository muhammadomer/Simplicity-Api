
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AttFOrderDocsMasterController : Controller
    {
        private readonly IAttFOrderDocsMasterRepository AttFOrderDocsMasterRepository;
        public AttFOrderDocsMasterController(IAttFOrderDocsMasterRepository attFOrderDocsMasterRepository)
        { this.AttFOrderDocsMasterRepository = attFOrderDocsMasterRepository; }

        [HttpGet]
        [ActionName("GetAttFOrdDocsMasterByJobSequenceAndSequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAttFOrdDocsMasterByJobSequenceAndSequence(long jobSequence, long sequence)
        {
            SimplicityFile file = AttFOrderDocsMasterRepository.GetAttfOrderDocsMasterByJobSequenceAndSequence(Request, jobSequence, sequence);
            if (file != null && file.MemStream != null)
            {
                return new FileStreamResult(file.MemStream, MimeKit.MimeTypes.GetMimeType(file.FileName))
                {
                    FileDownloadName = file.FileName
                };
            }
            else
            {
                return new ObjectResult(new { file });

            }
        }

        [HttpGet]
        [ActionName("GetAttFOrdDocsMasterByJobSequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetAttFOrdDocsMasterByJobSequence(long jobSequence)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                List<AttfOrdDocsMasters> attfOrdDocsMaster = AttFOrderDocsMasterRepository.GetAttfOrderDocsMasterByJobSequence(Request, jobSequence);
                if (attfOrdDocsMaster != null)
                {
                    returnValue.IsSucessfull = true;
                    returnValue.TheObject = attfOrdDocsMaster;
                }
                else
                {
                    returnValue.IsSucessfull = false;
                    returnValue.Message = "Unable to get Order Templates List.";
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }

        [HttpPost]
        [ActionName("PutAttFOrdDocsMaster")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel PutAttFOrdDocsMaster([FromBody]AttfOrdDocsMastersFile attfOrdDocsMastersFile)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                returnValue.IsSucessfull = AttFOrderDocsMasterRepository.PutAttFOrdDocsMaster(Request, attfOrdDocsMastersFile);
                if (!returnValue.IsSucessfull)
                {
                    returnValue.Message = "Unable to Save Order Document. " + AttFOrderDocsMasterRepository.Message;
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex.Message, ex);
            }
            return returnValue;
        }
    }
}
