using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using SimplicityOnlineWebApi.BLL.Entities;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
//using SimplicityOnlineWebApi.BLL.CustomBinders;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private IOrdersRepository OrdersRepository;
        private ITradeCodeRepository TradeCodeRepository;
        private IRefEntityPaymentTypeRepository RefEntityPaymentTypeRepository;
        private ILogger<OrdersController> Logger;
        public OrdersController(
            IOrdersRepository ordersRepository, 
            ITradeCodeRepository tradeCodeRepository,
            IRefEntityPaymentTypeRepository refEntityPaymentTypeRepository,
            ILogger<OrdersController> logger
            )
        {
            this.OrdersRepository = ordersRepository;
            this.TradeCodeRepository = tradeCodeRepository;
            this.RefEntityPaymentTypeRepository = refEntityPaymentTypeRepository;
            this.Logger = logger;
        }
        //[FromServices]
        //public IOrdersRepository OrdersRepository { get; set; }

        //[FromServices]
        //public ITradeCodeRepository TradeCodeRepository { get; set; }

        //[FromServices]
        //public IRefEntityPaymentTypeRepository RefEntityPaymentTypeRepository { get; set; }

        //[FromServices]
        //public ILogger<OrdersController> Logger { get; set; }

        //[HttpGet]
        //[ActionName("GetAllOrders")]
        //[Route("[action]")]
        //[ValidateRequestState]
        //public IActionResult GetAllOrders()
        //{
        //    List<Orders> orders = OrdersRepository.GetAllOrders(Request);
        //    if (orders == null)
        //    {
        //        return new ObjectResult(false);
        //    }
        //    return new ObjectResult(orders);
        //}

        //[HttpGet]
        //[ActionName("GetAllOrdersByJobRef")]
        //[Route("[action]")]
        //[ValidateRequestState]
        //public IActionResult GetAllOrdersByJobRef(string jobRef)
        //{
        //    List<Orders> orders = OrdersRepository.GetAllOrdersByJobRef(jobRef, Request);
        //    if (orders == null)
        //    {
        //        return new ObjectResult(false);
        //    }
        //    return new ObjectResult(orders);
        //}

        [HttpGet]
        [ActionName("GetOrdersMinByJobRef")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetOrdersMinByJobRef(string jobRef)
        {
            List<OrdersMin> ordersMin = OrdersRepository.GetAllOrdersMinByJobRef(jobRef, Request);
            if (ordersMin == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(ordersMin);
        }

        //[HttpGet]
        //[ActionName("Search")]
        //[Route("[action]")]
        //[ValidateRequestState]
        //public IActionResult SearchOrders(string key, string field, string match)
        //{
        //    List<Orders> orders = OrdersRepository.SearchOrders(key, field, match, Request);
        //    if (orders == null)
        //    {
        //        return new ObjectResult(false);
        //    }
        //    return new ObjectResult(orders);
        //}

        //[HttpGet]
        //[ActionName("SearchOrders")]
        //[Route("[action]")]
        //[ValidateRequestState]
        //public IActionResult SearchOrders()
        //{
        //    List<Orders> orders = OrdersRepository.SearchOrders(Request);
        //    if (orders == null)
        //    {
        //        return new ObjectResult(false);
        //    }
        //    return new ObjectResult(orders);
        //}

        [HttpGet]
        [ActionName("GetOrder")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetOrder(long sequence)
        {
            Orders order = OrdersRepository.GetOrderDetailsBySequence(sequence, Request);
            if (order == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(order);
        }

        [HttpGet]
        [ActionName("GetOrderByJobRef")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetOrderByJobRef(string jobRef)
        {
            Orders order = OrdersRepository.GetOrderByJobRef(jobRef, Request);
            if (order == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(order);
        }

         [HttpPost]
         [ActionName("CancelOrderBySequence")]
         [Route("[action]")]
         [ValidateRequestState]
         public IActionResult CancelOrderBySequence([FromBody]Orders order)
         {
            bool returnValue = OrdersRepository.CancelOrderBySequence(order, Request);
            return new ObjectResult(returnValue);
         }

      [HttpPost]
      [ActionName("ReactivateOrderBySequence")]
      [Route("[action]")]
      [ValidateRequestState]
      public IActionResult ReactivateOrderBySequence([FromBody]Orders order)
      {
         bool returnValue = OrdersRepository.ReactivateOrderBySequence(order, Request);
         return new ObjectResult(returnValue);
      }

      //[HttpGet]
      //[ActionName("GetOrdersByJobRefOrAddressOrClientName")]
      //[Route("[action]")]
      //[ValidateRequestState]
      //public IActionResult GetOrdersByJobRefOrAddressOrClientName(string jobRef, string jobAddress, string jobClientName)
      //{
      //    List<Orders> orders = OrdersRepository.GetOrdersByJobRefOrAddressOrClientName(jobRef, jobAddress, jobClientName, Request);
      //    if (orders == null)
      //    {
      //        return new ObjectResult(false);
      //    }
      //    return new ObjectResult(orders);
      //}
      [HttpPost]
        [ActionName("GetOrdersMinByJobRefOrAddressOrClientName")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetOrdersMinByJobRefOrAddressOrClientName([FromBody]ClientRequest clientRequest)
        {   
            return new ObjectResult(OrdersRepository.GetOrdersMinByJobRefOrAddressOrClientName(clientRequest, Request));
        }

        [HttpGet]
        [ActionName("GetOrdersMinByJobAddress")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetOrdersMinByJobAddress(long jobAddressId)
        {
            List<OrdersMinWithJobAddress> orders = OrdersRepository.GetOrdersMinByJobAddress(jobAddressId, Request);
            return new ObjectResult(orders);
        }
        //[HttpGet]
        //[ActionName("GetOrdersMinByJobRefOrAddressOrClientName")]
        //[Route("[action]")]
        //[ValidateRequestState]
        //public IActionResult GetOrdersMinByJobRefOrAddressOrClientName(string jobRef, string jobAddress, string jobClientName, string jobClientRef, string ebsJobRef)
        //{
        //    List<OrdersMinWithJobAddressClientName> orders = OrdersRepository.GetOrdersMinByJobRefOrAddressOrClientName(jobRef, jobAddress, jobClientName, jobClientRef, ebsJobRef, Request);
        //    if (orders == null)
        //    {
        //        return new ObjectResult(false);
        //    }
        //    return new ObjectResult(orders);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ebsJobSequence">EBS Job Sequence</param>
        /// <param name="fileName">File name</param>
        /// <param name="parentFolderNames">Comma seperated folder names e.g. FolderA,FolderB,FolderC, this will create: SimplicityOnline/FolderA/FolderB/FolderC</param>
        /// <param name="oiFireProtectionIImages">Image stream</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("AddFileByEBSJobSequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult AddFileByEBSJobSequence(long ebsJobSequence, string fileName, string parentFolderNames, Cld_Ord_Labels_Files oiFireProtectionIImages)
        {
            bool returnValue = OrdersRepository.AddFileByEBSJobSequence(ebsJobSequence, fileName, parentFolderNames, oiFireProtectionIImages, Request, HttpContext.Response);
            return new ObjectResult(returnValue);
        }

        //[HttpPost]
        //[ActionName("UpdateJobAddress")]
        //[Route("[action]")]
        //[ValidateRequestState]
        //public IActionResult UpdateJobAddress(int sequence, string jobAddress)
        //{
        //    bool success = OrdersRepository.UpdateJobAddress(sequence, jobAddress, Request);
        //    return new ObjectResult(success);
        //}

        [HttpPost]
        [ActionName("UpdateJobAddressDetails")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateJobAddressDetails([FromBody]OrdersJobAddress orderAddress)
        {
            bool success = OrdersRepository.UpdateJobAddress(orderAddress, Request);
            return new ObjectResult(success);
        }

        [HttpPost]
        [ActionName("UpdateJobClientDetails")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateJobClient(int sequence,long clientId,string clientName)
        {   
            return new ObjectResult(OrdersRepository.UpdateJobClient(sequence, clientId, clientName, Request));
        }

        [HttpGet]
        [ActionName("GetOrderMaintenanceScreenTitle")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetOrderMaintenanceScreenTitle(int sequence)
        {
            string label = "Enquiry";

            Orders order = OrdersRepository.GetOrderDetailsBySequence(sequence, Request);
            if (order != null)
            {
                if ((bool)order.FlgJT)
                {
                    label = "Job Ticket";
                }
                else if (order.FlgClient)
                {
                    label = "Estimate";
                }
            }

            return new ObjectResult(label);
        }

        [HttpGet]
        [ActionName("GetNewJobRefNoAndManualCreateJobRefSetting")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetNewJobRefNoAndManualCreateJobRefSetting()
        {
            string jobRefNo = OrdersRepository.GetNewJobRefNo(Request, Response);
            bool? canManualCreateJobRef = OrdersRepository.CanManualCreateJobRefForCreateOrder(Request, Response);

            return new ObjectResult(new { jobRefNo, canManualCreateJobRef });
        }

        [HttpPost ("FromBody")]
        [Route("[action]")]
        [ValidateRequestState]
        //public IActionResult CreateOrderByJobRef([ModelBinder(BinderType = typeof(OrdersBinder))] [FromBody] Orders order, bool autoCreateJobRef)
        public IActionResult CreateOrderByJobRef([FromBody] Orders order, bool autoCreateJobRef)
        {
            var retOrder = OrdersRepository.CreateOrderByJobRef(order, autoCreateJobRef, Request, HttpContext.Response);
            if (retOrder == null)
            {
                return new ObjectResult(HttpContext.Response);
            }

            return new ObjectResult(retOrder);
        }

        [HttpPost]
        [ActionName("UpdateOrder")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateOrder([FromBody]Orders order)
        {
            return new ObjectResult(OrdersRepository.UpdateOrder(order, Request));
            //return new ObjectResult(success);
        }

        [HttpGet]
        [ActionName("GetAllTradeCodes")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllTradeCodes()
        {
            List<TradeCode> tradeCodes = TradeCodeRepository.GetAllTradeCodes(Request);
            if (tradeCodes == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(tradeCodes);
        }

        [HttpGet]
        [ActionName("GetAllPaymentTypes")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllPaymentTypes()
        {
            List<RefEntityPaymentType> paymentTypes = RefEntityPaymentTypeRepository.GetAllPaymentTypes(Request);
            if (paymentTypes == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(paymentTypes);
        }

        [HttpPost]
        [ActionName("OrdersList")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult OrdersList([FromBody]ClientRequest clientRequest)
        {           
            return new ObjectResult(OrdersRepository.OrdersList(clientRequest, Request));
        }

        [HttpGet]
        [ActionName("GetCaptionForActiveProject")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetCaptionForActiveProject()
        {
            string label = OrdersRepository.GetCaptionForActiveProject(Request);
            return new ObjectResult(new { label });
        }

        [HttpGet]
        [ActionName("GetAPSConfig")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAPSConfig()
        {
            bool value = OrdersRepository.GetAPSConfig(Request);
            return new ObjectResult(new { value });
        }

        [HttpGet]
        [ActionName("OrdersList2")]
        [Route("[action]")]
        public IActionResult OrdersList2(int size,string projectId)
        {
            DataTable dt =OrdersRepository.OrdersList2(size, projectId, Request);
            return Json(JsonConvert.SerializeObject(dt));
        }
    }
}
