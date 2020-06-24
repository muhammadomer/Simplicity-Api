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
    public class OrdersKpiController : Controller
    {

		private readonly IOrdersKpiRepository OrdersKpiRepository;
		private readonly ILogger<OrdersKpiController> Logger;
		public OrdersKpiController(IOrdersKpiRepository ordersKpiRepository, ILogger<OrdersKpiController> logger)
		{
			this.OrdersKpiRepository = ordersKpiRepository;
			this.Logger = logger;
		}
		[HttpPost]
		[ActionName("GetOutstandingKpiOrderList")]
		[Route("[action]")]
		[Produces("application/json")]
		public IActionResult GetOutstandingKpiOrderList([FromBody]ClientRequest clientRequest, string fromDate, string toDate)
		{
			DateTime? FromDate = null; DateTime? ToDate = null;
			if (fromDate != null)
				FromDate = Convert.ToDateTime(fromDate.Substring(0, 24));
			if (toDate != null)
				ToDate = Convert.ToDateTime(toDate.Substring(0, 24));
			return new ObjectResult(OrdersKpiRepository.GetOutstandingKpiOrderList(Request, clientRequest, FromDate, ToDate));
		}
		[HttpPost]
		[ActionName("GetSuccessKpiOrderList")]
		[Route("[action]")]
		[Produces("application/json")]
		public IActionResult GetSuccessKpiOrderList([FromBody]ClientRequest clientRequest, string fromDate, string toDate)
		{
			DateTime? FromDate = null; DateTime? ToDate = null;
			if (fromDate != null)
				FromDate = Convert.ToDateTime(fromDate.Substring(0, 24));
			if (toDate != null)
				ToDate = Convert.ToDateTime(toDate.Substring(0, 24));
			return new ObjectResult(OrdersKpiRepository.GetSuccessKpiOrderList(Request, clientRequest, FromDate, ToDate));
		}









	}
}
