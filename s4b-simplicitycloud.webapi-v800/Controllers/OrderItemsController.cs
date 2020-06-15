using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineBLL.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class OrderItemsController : Controller
    {
        private readonly IOrderItemsRepository OrderItemsRepository;
        public OrderItemsController(IOrderItemsRepository orderItemsRepository)
        {
            this.OrderItemsRepository = orderItemsRepository;
        }

        [HttpPost]
        [ActionName("GetOrderItemsByJobSequence")]
        [Route("[action]")]
        public IActionResult GetOrderItemsByJobSequence([FromBody]ClientRequest clientRequest,int jobSequence)
        {
            return new ObjectResult(OrderItemsRepository.GetOrderItemsByJobSequence(clientRequest, jobSequence, HttpContext.Request, HttpContext.Response));
           
        }

		[HttpGet]
		[ActionName("GetOrderItemDescByItemCode")]
		[Route("[action]")]
		public IActionResult GetOrderItemDescByItemCode(string itemCode)
		{
			return new ObjectResult(OrderItemsRepository.GetOrderItemDescByItemCode(HttpContext.Request,itemCode ));

		}
		[HttpPost]
        [ActionName("UpdateOrderItems")]
        [Route("[action]")]
        public IActionResult UpdateOrderItems([FromBody]RequestModel Object)
        {  
            return new ObjectResult(OrderItemsRepository.UpdateOrderItems(Object, HttpContext.Request));
        }
        [HttpPost]
        [ActionName("CreateOrderItem")]
        [Route("[action]")]
        public IActionResult CreateOrderItem([FromBody]OrderItems Object)
        {
            OrderItems OrderItems = OrderItemsRepository.CreateOrderItems(Object, HttpContext.Request);
            if (OrderItems != null)
            {
                return new ObjectResult(OrderItems);
            }
            return new ObjectResult(OrderItems);
        }
        [HttpGet]
        [ActionName("GetAllSupliers")]
        [Route("[action]")]
        public IActionResult GetAllSupliers(string transType)
        {
            List<EntityDetailsCoreMin> Supliers = OrderItemsRepository.GetAllSupliers(HttpContext.Request, transType);
            if (Supliers != null && Supliers.Count > 0)
            {
                return new ObjectResult(Supliers);
            }
            return new ObjectResult(Supliers);
        }

       
    }
}
