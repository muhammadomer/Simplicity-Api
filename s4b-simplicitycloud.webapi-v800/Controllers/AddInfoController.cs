
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AddInfoController : Controller
    {
        private readonly IAddInfoRepository AddInfoRepository;
        public AddInfoController(IAddInfoRepository addInfoRepository)
        {
            this.AddInfoRepository = addInfoRepository;
        }

        //[FromServices]
        //public IAddInfoRepository AddInfoRepository { get; set; }

        [HttpGet]
        [ActionName("GetAddInfoWithSequence")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAddInfoWithSequence(long sequence)
        {
            Cld_Ord_Labels_Files_Add_Info oiFireProtectionIAddInfo = AddInfoRepository.GetAddInfoWithSequence(sequence, Request, Response);
            if (oiFireProtectionIAddInfo == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(oiFireProtectionIAddInfo);
        }

        [HttpGet]
        [ActionName("GetAddInfoWithDesc")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult GetAddInfoWithDesc(long sequence,string addInfo)
        {
            Cld_Ord_Labels_Files_Add_Info oiFireProtectionIAddInfo = AddInfoRepository.GetAddInfoWithDesc(sequence,addInfo, Request, HttpContext.Response);
            if (oiFireProtectionIAddInfo == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(oiFireProtectionIAddInfo);
        }

        [HttpPost]
        [ActionName("CreaeUpdateAddInfo")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult CreaeUpdateAddInfo([FromBody]Cld_Ord_Labels_Files_Add_Info addInfo)
        {
            Cld_Ord_Labels_Files_Add_Info oiFireProtectionIAddInfo = AddInfoRepository.CreaeUpdateAddInfo(addInfo, Request, HttpContext.Response);
            if (oiFireProtectionIAddInfo == null)
            {
                return new ObjectResult(HttpContext.Response);
            }
            return new ObjectResult(oiFireProtectionIAddInfo);
        }

        

    }
}
