using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class S4BFormTemplateController : Controller
    {
        private readonly IS4BFormTemplateRepository S4BFormTemplateRepository;
        private readonly ILogger<S4BFormTemplateController> Logger;

        public S4BFormTemplateController(IS4BFormTemplateRepository s4BFormTemplateRepository, ILogger<S4BFormTemplateController> logger)
        {
            this.S4BFormTemplateRepository =s4BFormTemplateRepository;
            this.Logger = logger;
        }

        // GET api/values
        [HttpGet]
        [ActionName("GetTemplateBySequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetTemplateBySequence(int sequence)
        {
            return new ObjectResult(S4BFormTemplateRepository.GetTemplateBySequence(sequence, Request));
        }

        // PUT api/values        
        [HttpPost]
        [ActionName("Update")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult Update([FromBody]ExpandoObject templateData,long joinSequence)
        {   
            return new ObjectResult(S4BFormTemplateRepository.Update(templateData, joinSequence, Request));
        }

        [HttpPost]
        [ActionName("GeneratePDF")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GeneratePDF([FromBody]long sequence)
        {   
            return new ObjectResult(S4BFormTemplateRepository.GeneratePDF(sequence, Request));
        }

        [HttpGet]
        [ActionName("SendEmailSubmissionPdfFile")]
        [Route("[action]")]
        public IActionResult EmailFile(long sequence)
        {  
             
           return new ObjectResult(S4BFormTemplateRepository.EmailSubmissionPdfFile(sequence, Request));

        }
    }
}
