
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class MailMergeController : Controller
    {
        private readonly IMailMergeRepository MailMergeRepository;
        public MailMergeController(IMailMergeRepository mailMergeRepository)
        { this.MailMergeRepository =mailMergeRepository; }

        [HttpGet]
        [ActionName("GetAllMailMergeCodes")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllMailMergeCodes()
        {
            return new ObjectResult(MailMergeRepository.GetAllMailMergeCodes(Request));
        }

        [HttpGet]
        [ActionName("GetAllMailMergeCodesMin")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllMailMergeCodesMin()
        {
            return new ObjectResult(MailMergeRepository.GetAllMailMergeCodesMin(Request));
        }

        [HttpPost]
        [ActionName("PerformMailMergeByJobRef")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult PerformMailMergeByJobRef([FromBody]RequestModel reqModel)
        {
            return new ObjectResult(MailMergeRepository.PerformMailMergeByJobRef(Request, reqModel));
        }
    }
}
