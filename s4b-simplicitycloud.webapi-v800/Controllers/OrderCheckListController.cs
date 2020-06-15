
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;

using SimplicityOnlineBLL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class OrderCheckListController : Controller
    {
        private readonly IOrderCheckListRepository OrderCheckListRepository;
        private readonly ILogger<OrderCheckListController> Logger;
        public OrderCheckListController(ILogger<OrderCheckListController> logger, IOrderCheckListRepository orderCheckListRepository)
        {
            this.Logger = logger;
            this.OrderCheckListRepository =orderCheckListRepository;
        }

        [HttpGet]
        [ActionName("GetOrderCheckList")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetOrderCheckList(long jobSequence)
        {
            return new ObjectResult(OrderCheckListRepository.GetOrderCheckListByJobSequence(Request, jobSequence));
        }

        [HttpPost]
        [ActionName("UpdateOrderCheckList")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateOrderCheckList([FromBody]RequestModel reqModel)
        {
            return new ObjectResult(OrderCheckListRepository.UpdateOrderCheckList(Request, reqModel));
        }
    }
}
