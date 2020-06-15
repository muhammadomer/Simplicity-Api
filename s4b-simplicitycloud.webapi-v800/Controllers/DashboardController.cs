
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using System;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {

        public IDashboardRepository DashboardRepository;
        public DashboardController(IDashboardRepository dashboardRepository)
        { this.DashboardRepository =dashboardRepository; }

        [HttpGet]
        [ActionName("GetDashboardView")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetDashboardView(DateTime date)
        {
            return new ObjectResult(DashboardRepository.GetDashboardView(Request, date));
        }

        [HttpGet]
        [ActionName("GetDashboardViewForOrdersByOrderType")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetDashboardViewForOrdersCountByType(DateTime fromDate, DateTime toDate)
        {
            return new ObjectResult(DashboardRepository.GetDashboardViewForOrdersByOrderType(Request, fromDate, toDate));
        }

        [HttpGet]
        [ActionName("GetDashboardViewForOrdersByOrderStatus")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetDashboardViewForOrdersByOrderStatus(DateTime fromDate, DateTime toDate)
        {
            return new ObjectResult(DashboardRepository.GetDashboardViewForOrdersByOrderStatus(Request, fromDate, toDate));
        }

        [HttpGet]
        [ActionName("GetDashboardViewForOrdersByJobStatus")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetDashboardViewForOrdersCountByJobStatus(DateTime fromDate, DateTime toDate)
        {
            return new ObjectResult(DashboardRepository.GetDashboardViewForOrdersByJobStatus(Request, fromDate, toDate));
        }

        [HttpGet]
        [ActionName("GetDashboardViewForSubmissionByTemplate")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetDashboardViewForSubmissionByTemplate(DateTime fromDate, DateTime toDate)
        {
            return new ObjectResult(DashboardRepository.GetDashboardViewForSubmissionByTemplate(Request, fromDate, toDate));
        }
    }
}
