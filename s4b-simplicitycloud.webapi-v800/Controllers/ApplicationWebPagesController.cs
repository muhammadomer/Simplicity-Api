
using Microsoft.AspNetCore.Mvc;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class ApplicationWebPagesController : Controller
    {
        private readonly IApplicationWebPagesRepository ApplicationWebPagesRepository;
        public ApplicationWebPagesController(IApplicationWebPagesRepository applicationWebPagesRepository)
        {
            this.ApplicationWebPagesRepository = applicationWebPagesRepository;
        }

        [HttpGet]
        [ActionName("GetAllApplicationWebPages")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult GetAllApplicationWebPages()
        {
            List<ApplicationWebPages> webPages = ApplicationWebPagesRepository.GetAllApplicationWebPages(Request, Response);
            if (webPages == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(webPages);
        }
    }
}
