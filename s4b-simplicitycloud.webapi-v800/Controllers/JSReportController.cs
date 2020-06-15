using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class JSReportController : Controller
    {
        private readonly ILogger<JSReportController> LOGGER;
        private readonly IReportRepository ReportRepository;
        public JSReportController(ILogger<JSReportController> _LOGGER, IReportRepository reportRepository)
        {
            this.LOGGER = _LOGGER;
            this.ReportRepository =reportRepository;
        }

        [HttpPost]
        [ActionName("GetInvoiceReport")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetInvoiceReport([FromBody]string incomingData)
        {
            return new ObjectResult( ReportRepository.GenerateReport(Request, incomingData));
        }

        
    }
}
