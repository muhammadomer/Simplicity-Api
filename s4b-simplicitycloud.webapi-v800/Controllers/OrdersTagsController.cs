
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using Newtonsoft.Json;
using SimplicityOnlineWebApi.BLL.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class OrdersTagsController : Controller
    {
        private readonly IOrdersTagsRepository OrdersTagsRepository;
        public OrdersTagsController(IOrdersTagsRepository ordersTagsRepository)
        {
            this.OrdersTagsRepository = ordersTagsRepository;
        }

        [HttpPost]
        [ActionName("FindCreateOrderByJobRef")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult FindCreateOrderByJobRef([FromBody]Orders order)
        {
            order = OrdersTagsRepository.FindCreateOrderByJobRefWithTag(order.JobRef, false, Request, HttpContext.Response);
            if (order == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            order.IsSucessfull = true;
            return new ObjectResult(order);
        }

        [HttpPost]
        [ActionName("FindCreateOrderByJobRefWithTag")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult FindCreateOrderByJobRefWithTag([FromBody]Orders order)
        {
            order = OrdersTagsRepository.FindCreateOrderByJobRefWithTag(order.JobRef, true, Request, HttpContext.Response);
            if (order == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            order.IsSucessfull = true;
            return new ObjectResult(order);
        }

        [HttpPost]
        [ActionName("FindCreateTagByTagAndOrder")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult FindCreateTagByTagAndOrder([FromBody]Cld_Ord_Labels orderTag)
        {
            orderTag = OrdersTagsRepository.FindCreateTagByTagNoAndJobSequence(orderTag.JobSequence ?? 0, orderTag.TagNo, Request, HttpContext.Response);
            if (orderTag == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(orderTag);
        }

        [HttpPost]
        [ActionName("UpdateTagNoBySequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateTagNoBySequence([FromBody]Cld_Ord_Labels orderTag)
        {
            orderTag = OrdersTagsRepository.UpdateTagNoBySequence(orderTag.Sequence ?? 0, orderTag.TagNo, Request, HttpContext.Response);
            if (orderTag == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(orderTag);
        }

        [HttpPost]
        [ActionName("FindTagByOrderSequenceAndTagNo")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult FindTagByOrderSequenceAndTagNo([FromBody]Cld_Ord_Labels orderTag)
        {
            orderTag = OrdersTagsRepository.FindTagByOrderSequenceAndTagNo(orderTag.JobSequence ?? 0, orderTag.TagNo, Request, HttpContext.Response);
            if (orderTag == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(orderTag);
        }
        [HttpPost]
        [ActionName("FindOtherTagByOrderSequenceAndTagNo")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult FindOtherTagByOrderSequenceAndTagNo([FromBody]Cld_Ord_Labels orderTag)
        {
            List<Cld_Ord_Labels> orderTagResult = OrdersTagsRepository.FindOtherTagByOrderSequenceAndTagNo(orderTag.JobSequence ?? 0, orderTag.Sequence ?? 0, orderTag.TagNo, Request, HttpContext.Response);
            if (orderTagResult == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            bool result = false;
            if (orderTagResult.Count == 1)
            {
                result = true;
            }
            return new ObjectResult(result);
        }

        [HttpPost]
        [ActionName("SearchTagByOrderSequenceAndTagNo")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult SearchTagByOrderSequenceAndTagNo([FromBody]Cld_Ord_Labels orderTag)
        {
            List<Cld_Ord_Labels> orderTagResult = OrdersTagsRepository.SearchTagByOrderSequenceAndTagNo(orderTag.JobSequence ?? 0, orderTag.TagNo, Request, HttpContext.Response);
            if (orderTag == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(orderTagResult);
        }

        [HttpPost]
        [ActionName("CreateUpdateOrderWithTagsAndImages")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult CreateUpdateOrderWithTagsAndImages()
        {
            Orders order = JsonConvert.DeserializeObject<Orders>(Request.Form["Order"]);
            order = OrdersTagsRepository.CreateUpdateOrderWithTagsAndImages(order, Request, HttpContext.Response);
            if (order == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(order);
        }

        [HttpPost]
        [ActionName("GetOrderWithTagsAndImages")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult GetOrderWithTagsAndImages([FromBody]Orders order)
        {
            order = OrdersTagsRepository.GetOrderWithTagsAndImages(order, Request, HttpContext.Response);
            if (order == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(order);
        }

        [HttpPost]
        [ActionName("SearchOrdersWithTagsAndImages")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult SearchOrdersWithTagsAndImages([FromBody]SearchOrderTags searchOptions)
        {
            List<Orders> orders = OrdersTagsRepository.SearchOrderWithTagsAndImages2(searchOptions, Request, HttpContext.Response);
            if (orders == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(orders);
        }

        [HttpPost]
        [ActionName("GetJobRefListForTimeSheet")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetJobRefListForTimeSheet([FromBody]SearchOrderTags searchOptions)
        {
            List<JobRefALL> orders = OrdersTagsRepository.GetJobRefListForTimeSheet(searchOptions, Request, HttpContext.Response);
            if (orders == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(orders);
        }

        [HttpPost]
        [ActionName("EmailOrdersWithTagsAndImages")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult EmailOrdersWithTagsAndImages([FromBody]EmailOrderTags emailOptions)
        {
            ResponseModel response = OrdersTagsRepository.EmailOrdersWithTagsAndImages(emailOptions, Request, HttpContext.Response);
            if (response == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(response);
        }

        [HttpPost]
        [ActionName("GetUrlOfArchiveSystem")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetUrlOfArchiveSystem(long orderSequence)
        {
            string url = OrdersTagsRepository.GetUrlOfArchiveSystem(orderSequence, Request, Response);

            return new ObjectResult(url);
        }        
    }
}
