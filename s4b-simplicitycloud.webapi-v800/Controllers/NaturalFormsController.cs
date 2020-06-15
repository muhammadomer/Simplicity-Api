
using Microsoft.AspNetCore.Mvc;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class NaturalFormsController : Controller
    {
        private readonly INaturalFormsRepository NaturalFormsRepository;
        public NaturalFormsController(INaturalFormsRepository naturalFormsRepository)
        { this.NaturalFormsRepository =naturalFormsRepository; }

        // POST api/values
        [HttpPost]
        [ActionName("GetTemplateURL")]
        [Route("[action]")]
        [ValidateRequestState]
        public ResponseModel GetTemplateURL([FromBody]NaturalFormRequest naturalFormRequest)
        {
            ResponseModel returnedResponse = NaturalFormsRepository.GetTemplateURL(naturalFormRequest, Request);
            if (returnedResponse == null)
            {
                returnedResponse = new ResponseModel();
                returnedResponse.IsSucessfull = false;
                returnedResponse.Message = "Unable to Generate the Form URL.";
            }
            return returnedResponse;
        }

        


    }
}
