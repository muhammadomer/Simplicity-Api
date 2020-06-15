using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class SiteInspectionController : Controller
    {
        private readonly ISiteInspectionRepository SiteInspectionRepository;
        private readonly ILogger<RefS4bFormsController> Logger;
        public SiteInspectionController(ISiteInspectionRepository siteInspectionRepository, ILogger<RefS4bFormsController> logger)
        {
            this.SiteInspectionRepository = siteInspectionRepository;
            this.Logger = logger;
        }

        [HttpGet]
        [ActionName("FindMatchingContractNos")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult FindMatchingContractNos(string contractNo)
        {
            List<string> contractNos = null;
            try
            {
                contractNos = SiteInspectionRepository.FindMatchingContractNos(contractNo, Request);               
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }

            if (contractNos == null || contractNos.Count() == 0)
            {

                return new ObjectResult(false);
            }

            return new ObjectResult(contractNos);
        }

        // GET api/values
        [HttpGet]
        [ActionName("GetBySequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetBySequence(int sequence)
        {
            SubmissionsDataFh siteInspection = null;
            try
            {
                siteInspection = SiteInspectionRepository.GetBySequence(sequence, Request);                
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }

            if (siteInspection == null)
            {

                return new ObjectResult(false);
            }

            return new ObjectResult(siteInspection);
        }

        // PUT api/values        
        [HttpPost]
        [ActionName("Update")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult Update([FromBody]SubmissionsDataFh siteInspection)
        {
            bool success = false;
            try
            {
                success = SiteInspectionRepository.Update(siteInspection, Request);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
            return new ObjectResult(success);
        }

        [HttpPost]
        [ActionName("UploadImage")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UploadImage([FromBody]SubmissionsImagesFh fileDetail)
        {
            SubmissionsImagesFh response = SiteInspectionRepository.UploadImageWithSequence(fileDetail, Request, HttpContext.Response);
            if (response == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(response);
        }


        [HttpPost]
        [ActionName("Add")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult Add([FromBody]SubmissionsDataFh siteInspection)
        {
            bool success = SiteInspectionRepository.Insert(siteInspection, Request,Response);
            return new ObjectResult(success);
        }

        [HttpGet]
        [ActionName("GetSubmissionsDataFhList")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult GetSubmissionsDataFhList()
        {
            List<SubmissionsDataFh> result = SiteInspectionRepository.GetSubmissionsDataFhList(Request);
            if (result != null && result.Count > 0)
            {
                return new ObjectResult(result);
            }

            return new ObjectResult(false);
        }

        [HttpPost]
        [ActionName("GeneratePDF")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GeneratePDF([FromBody]long sequence)
        {
            bool success = SiteInspectionRepository.GeneratePDF(sequence, Request);
            return new ObjectResult(success);
        }
    }
}
