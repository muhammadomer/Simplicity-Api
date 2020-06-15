
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class RefNaturalFormsController : Controller
    {
        private readonly IRefNaturalFormsRepository RefNaturalFormsRepository;

        public RefNaturalFormsController(IRefNaturalFormsRepository refNaturalFormsRepository)
        {
            this.RefNaturalFormsRepository=refNaturalFormsRepository;
        }
        [HttpGet]
        [ActionName("GetAllRefNaturalForms")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllRefNaturalForms()
        {
            return new ObjectResult(RefNaturalFormsRepository.GetAllRefNaturalForms(Request));
        }

        [HttpGet]
        [ActionName("GetRefNaturalFormsByClientId")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetRefNaturalFormsByClientId(long ClientId)
        {
            return new ObjectResult(RefNaturalFormsRepository.GetRefNaturalFormsByClientId(Request, ClientId));
        }
    }
}
