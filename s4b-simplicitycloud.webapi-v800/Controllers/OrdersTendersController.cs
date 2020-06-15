
using Microsoft.AspNetCore.Mvc;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class OrdersTendersController : Controller
    {
        private readonly IOrdersTendersRepository OrdersTendersRepository;

        public OrdersTendersController (IOrdersTendersRepository ordersTendersRepository )
            {this.OrdersTendersRepository =ordersTendersRepository; }

        [HttpGet]
        [ActionName("GetTendersSpecificationsByEntityId")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetTendersSpecificationsByEntityId(long EntityId, bool flgFilterByStatus, int statusSequence,bool fliterOutFutureTender, bool flgFilterByFinalised, bool flgTenderFinalised)
        {
            List<OrdTendersTP> OrdersTendersTP = OrdersTendersRepository.GetTendersSpecificationsByEntityId(EntityId, flgFilterByStatus, statusSequence, fliterOutFutureTender, flgFilterByFinalised, flgTenderFinalised, Request);
            ResponseModel response = new ResponseModel();
            response.TheObject = OrdersTendersTP;
            if (OrdersTendersTP == null)
            {
                response.IsSucessfull = false;
                response.Message = "No Tender Specification Found for Entity Id " + EntityId;
            }
            else
            {
                response.IsSucessfull = true;
            }
            return response;
        }

        [HttpGet]
        [ActionName("GetTenderTPBySequenceWithAllDetails")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetTenderTPBySequenceWithAllDetails(long sequence)
        {
            List<OrdTendersTP> OrdersTendersTP = OrdersTendersRepository.GetTenderTPDetailsBySequence(sequence,  true, Request);
            ResponseModel response = new ResponseModel();
            response.TheObject = OrdersTendersTP;
            if (OrdersTendersTP == null)
            {
                response.IsSucessfull = false;
                response.Message = "No Tender Found for Sequence " + sequence;
            }
            else
            {
                response.IsSucessfull = true;
            }
            return response;
        }

        [HttpGet]
        [ActionName("GetTenderDetailsBySequenceWithAllDetails")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetTenderDetailsBySequenceWithAllDetails(long sequence)
        {
            OrdTendersSpecs OrdersTendersSpec = OrdersTendersRepository.GetTenderDetailsBySequence(sequence, true, Request);
            ResponseModel response = new ResponseModel();
            response.TheObject = OrdersTendersSpec;
            if (OrdersTendersSpec == null)
            {
                response.IsSucessfull = false;
                response.Message = "No Tender Found for Sequence " + sequence;
            }
            else
            {
                response.IsSucessfull = true;
            }
            return response;
        }

        [HttpGet]
        [ActionName("GetTenderDetailsBySequenceWithAllDetails4Client")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetTenderDetailsBySequenceWithAllDetails4Client(long sequence)
        {
            OrdTendersSpecsClient OrdersTendersSpec = OrdersTendersRepository.GetTenderDetailsBySequence4Client(sequence, true, Request);
            ResponseModel response = new ResponseModel();
            response.TheObject = OrdersTendersSpec;
            if (OrdersTendersSpec == null)
            {
                response.IsSucessfull = false;
                response.Message = "No Tender Found for Sequence " + sequence;
            }
            else
            {
                response.IsSucessfull = true;
            }
            return response;
        }

        [HttpGet]
        [ActionName("GetTendersTPFilesByTenderTPSequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetTendersTPFilesByTenderTPSequence(long tenderTPSequence)
        {
            List<OrdTendersTPFiles> OrdersTendersTPFiles = OrdersTendersRepository.GetTendersTPFilesByTenderTPSequence(tenderTPSequence, Request);
            ResponseModel response = new ResponseModel();
            response.TheObject = OrdersTendersTPFiles;
            if (OrdersTendersTPFiles == null)
            {
                response.IsSucessfull = false;
                response.Message = "No Tender Files Found for Tender TP Sequence ." + tenderTPSequence;
            }
            else
            {
                response.IsSucessfull = true;
            }
            return response;
        }

        [HttpGet]
        [ActionName("GetTendersTPFilesByGuId")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetTendersTPFilesByGuId(long guId)
        {
            List<OrdTendersTPFiles> OrdersTendersTPFiles = OrdersTendersRepository.GetTendersTPFilesByGuId(guId, Request);
            ResponseModel response = new ResponseModel();
            response.TheObject = OrdersTendersTPFiles;
            if (OrdersTendersTPFiles == null)
            {
                response.IsSucessfull = false;
                response.Message = "No Tender Files Found for Tender GuId ." + guId;
            }
            else
            {
                response.IsSucessfull = true;
            }
            return response;
        }

        [HttpGet]
        [ActionName("GetTendersSpecFilesByGuId")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetTendersSpecFilesByGuId(long guId)
        {
            List<OrdTendersSpecsFiles> OrdersTendersSpecFiles = OrdersTendersRepository.GetTenderSpecsFilesByGuId(guId, Request);
            ResponseModel response = new ResponseModel();
            response.TheObject = OrdersTendersSpecFiles;
            if (OrdersTendersSpecFiles == null)
            {
                response.IsSucessfull = false;
                response.Message = "No Tender Specification Files Found for Tender GuId ." + guId;
            }
            else
            {
                response.IsSucessfull = true;
            }
            return response;
        }

        [HttpGet]
        [ActionName("GetRefOrderTenderStatus")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetRefOrderTenderStatus()
        {
            List<RefOrdTenderStatus> RefOrdTenderStatus = OrdersTendersRepository.GetRefOrderTenderStatus(Request);
            ResponseModel response = new ResponseModel();
            response.TheObject = RefOrdTenderStatus;
            if (RefOrdTenderStatus == null)
            {
                response.IsSucessfull = false;
                response.Message = "No RefOrdTenderStatus Found.";
            }
            else
            {
                response.IsSucessfull = true;
            }
            return response;
        }

        [HttpPost]
        [ActionName("SendTenderNotificationToJobManager")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult SendTenderNotificationToJobManager(long sequence,long jobSequence,long tpQsSequence,long tpFileSequence, string notificationType)
        {
            bool result = OrdersTendersRepository.SendTenderNotificationToJobManager(sequence,jobSequence,tpQsSequence,tpFileSequence, notificationType, Request,HttpContext.Response);

            ResponseModel response = new ResponseModel();
            response.IsSucessfull = result;
            if (result==false)
            {
                response.Message = "Unable to send notification email";
            }
            return new ObjectResult(response);
        }

        [HttpPost]
        [ActionName("InsertOrdTendersTPFiles")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult InsertOrdTendersTPFiles([FromBody] OrdTendersTPFiles ordTendersTPFiles)
        {
            OrdTendersTPFiles result = OrdersTendersRepository.InsertOrdTendersTPFiles(ordTendersTPFiles, Request);
            if (result == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(result);
        }

        [HttpPost]
        [ActionName("InsertOrdTendersQA")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult InsertOrdTendersQA([FromBody] OrdTendersTPQS ordTendersTPQA)
        {
            OrdTendersTPQS result = OrdersTendersRepository.InsertOrdTendersTPQA(ordTendersTPQA, Request);
            if (result != null)
            {
                return new ObjectResult(result);
            }
            else
            {
                return new ObjectResult(result);
            }
        }

        [HttpPost]
        [ActionName("UpdateOrdTendersTP")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateOrdTendersTP([FromBody] OrdTendersTP ordTendersTP)
        {
            bool result = OrdersTendersRepository.UpdateOrdTendersTP(ordTendersTP, Request);
            ResponseModel response = new ResponseModel();
            response.IsSucessfull = result;
            if(!result)
            {
                response.Message = "Unable to Update Orders Tenders TP.";
            }
            return new ObjectResult(response);
        }

        [HttpPost]
        [ActionName("UpdateOrdTendersTPFileDeletedFlag")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateOrdTendersTPFileDeletedFlag(long sequence, bool flgDeleted)
        {
            bool result = OrdersTendersRepository.UpdateOrdTendersTPFileDeletedFlag(sequence, flgDeleted, Request);
            ResponseModel response = new ResponseModel();
            response.IsSucessfull = result;
            if (!result)
            {
                response.Message = "Unable to Update Orders Tenders TP Files Deleted Flag.";
            }
            return new ObjectResult(response);
        }

        [HttpGet]
        [ActionName("GetTendersQAsByTenderTPSequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetTendersQAsByTenderTPSequence(long tenderTPSequence)
        {
            List<OrdTendersTPQS> OrdersTendersTPQA = OrdersTendersRepository.GetTendersQAsByTenderTPSequence(tenderTPSequence, Request);
            ResponseModel response = new ResponseModel();
            response.TheObject = OrdersTendersTPQA;
            if (OrdersTendersTPQA == null)
            {
                response.IsSucessfull = false;
                response.Message = "No Tender Question Found for Sequence " + tenderTPSequence;
            }
            else
            {
                response.IsSucessfull = true;
            }
            return response;
        }

        [HttpGet]
        [ActionName("GetTenderQuestionDetail")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetTenderQuestionDetail(long sequence)
        {
            ResponseModel response = new ResponseModel();
            response.TheObject = OrdersTendersRepository.GetTenderQuestionDetail(sequence, Request);
            if (response.TheObject == null)
            {
                response.IsSucessfull = false;
                response.Message = "No Tender Question Found for Sequence " + sequence;
            }
            else
            {
                response.IsSucessfull = true;
            }
            return response;
        }

        [HttpGet]
        [ActionName("GetTendersSpecificationsByClientId")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetTendersSpecificationsByClientId(long ClientId, bool flgFilterByStatus, int statusSequence, bool flgFilterByAwarded, bool flgTenderAwarded)
        {
            List<OrdTendersSpecs> OrdersTendersTP = OrdersTendersRepository.GetTendersSpecificationsByClientId(ClientId, flgFilterByStatus, statusSequence, flgFilterByAwarded, flgTenderAwarded, Request);
            ResponseModel response = new ResponseModel();
            response.TheObject = OrdersTendersTP;
            if (OrdersTendersTP == null)
            {
                response.IsSucessfull = false;
                response.Message = "No Tender Specification Found for Client Id " + ClientId;
            }
            else
            {
                response.IsSucessfull = true;
            }
            return response;
        }

        [HttpGet]
        [ActionName("GetTendersSpecificationsByViewerId")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetTendersSpecificationsByViewerId(long ViewerId, bool flgFilterByStatus, int statusSequence, bool flgFilterByAwarded, bool flgTenderAwarded)
        {
            List<OrdTendersSpecs> OrdersTendersTP = OrdersTendersRepository.GetTendersSpecificationsByViewerId(ViewerId, flgFilterByStatus, statusSequence, flgFilterByAwarded, flgTenderAwarded, Request);
            ResponseModel response = new ResponseModel();
            response.TheObject = OrdersTendersTP;
            if (OrdersTendersTP == null)
            {
                response.IsSucessfull = false;
                response.Message = "No Tender Specification Found for Viewer Id " + ViewerId;
            }
            else
            {
                response.IsSucessfull = true;
            }
            return response;
        }

        [HttpGet]
        [ActionName("GetOrderVariationsByViewerId")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetOrderVariationsByViewerId(long ViewerId, bool flgFilterByStatus, int statusSequence, bool flgFilterByAwarded, bool flgTenderAwarded)
        {
            List<OrdTendersSpecs> OrdersTendersTP = OrdersTendersRepository.GetTendersSpecificationsByViewerId(ViewerId, flgFilterByStatus, statusSequence, flgFilterByAwarded, flgTenderAwarded, Request);
            ResponseModel response = new ResponseModel();
            response.TheObject = OrdersTendersTP;
            if (OrdersTendersTP == null)
            {
                response.IsSucessfull = false;
                response.Message = "No Tender Specification Found for Viewer Id " + ViewerId;
            }
            else
            {
                response.IsSucessfull = true;
            }
            return response;
        }

    }
}
