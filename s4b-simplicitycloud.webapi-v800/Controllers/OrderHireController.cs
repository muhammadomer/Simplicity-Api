using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class OrderHireController : Controller
    {
        private readonly IOrderHireRepository OrderHireRepository;
        private readonly ILogger<OrderHireController> Logger;

        public OrderHireController(IOrderHireRepository orderHireRepository , ILogger<OrderHireController> logger)
        {
            this.OrderHireRepository = orderHireRepository;
            this.Logger = logger;
        }

        [HttpPost]
        [ActionName("Insert")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult Insert([FromBody]OrderHire orderHire)
        {
            var retOrder = OrderHireRepository.Insert(Request, orderHire);
            if (retOrder == null)
            {
                return new ObjectResult(HttpContext.Response);
            }

            return new ObjectResult(retOrder);
        }

        [HttpPost]
        [ActionName("UpdateOrderHireBySequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateOrderHireBySequence([FromBody]OrderHire orderHire)
        {
            return new ObjectResult(OrderHireRepository.UpdateBySequence(Request, orderHire));
        }

        [HttpGet]
        [ActionName("GetOrderHireBySequence")]
        [Route("[action]")]
        public IActionResult GetOrderHireBySequence(int sequence)
        {  
            return new ObjectResult(OrderHireRepository.GetOrderHireBySequence(Request, sequence));

        }

        [HttpPost]
        [ActionName("GetListOfOrderHire")]
        [Route("[action]")]
        public IActionResult GetListOfOrderHire([FromBody]ClientRequest clientRequest,string fromDate, string toDate,int hireType)
        {
            DateTime? FromDate = null; DateTime? ToDate = null;
            if (fromDate != null)
                FromDate = Convert.ToDateTime(fromDate.Substring(0, 24));
            if (toDate != null)
                ToDate = Convert.ToDateTime(toDate.Substring(0, 24));
            return new ObjectResult(OrderHireRepository.GetListOfOrderHire(Request, clientRequest, FromDate, ToDate,hireType));

        }

        [HttpGet]
        [ActionName("GetAssetSelectedForDateRange")]
        [Route("[action]")]
        public IActionResult GetAssetSelectedForDateRange(long assetSequence, string fromDate, string toDate)
        {
            DateTime? FromDate = null; DateTime? ToDate = null;
            if (fromDate != null)
                FromDate = Convert.ToDateTime(fromDate.Substring(0, 24));
            if (toDate != null)
                ToDate = Convert.ToDateTime(toDate.Substring(0, 24));
            return new ObjectResult(OrderHireRepository.GetAssetSelectedForDateRange(Request, assetSequence, FromDate, ToDate));

        }
        [HttpGet]
        [ActionName("GetOrdHireForReportByDate")]
        [Route("[action]")]
        public IActionResult GetOrdHireForReportByDate(string fromDate,string toDate)
        {
            DateTime? FromDate = null; DateTime? ToDate = null;
            if (fromDate != null)
               FromDate = Convert.ToDateTime(fromDate.Substring(0, 24));
            if (toDate != null)
               ToDate = Convert.ToDateTime(toDate.Substring(0, 24));
            return new ObjectResult(OrderHireRepository.GetOrdHireForReportByDate(Request, FromDate,ToDate));

        }



    }
}
