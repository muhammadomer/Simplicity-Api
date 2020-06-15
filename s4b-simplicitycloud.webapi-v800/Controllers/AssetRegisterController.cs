
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AssetRegisterController : Controller
    {
        private readonly IAssetRegisterRepository AssetRegisterRepository;
        public AssetRegisterController(IAssetRegisterRepository assetRegisterRepository )
            { this.AssetRegisterRepository = assetRegisterRepository; }

        [HttpPost]
        [ActionName("GetAllAssetList")]
        [Route("[action]")]
        public IActionResult GetAllAssetList([FromBody]ClientRequest clientRequest)
        {
            return new ObjectResult(AssetRegisterRepository.GetSelectAllAssetsList(Request, clientRequest));

        }

        [HttpGet]
        [ActionName("GetAllAssets")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllAssets()
        {
            List<AssetRegister> result = AssetRegisterRepository.getAssetsList(Request);
            if (result != null && result.Count > 0)
            {
                return new ObjectResult(result);
            }
            else
            {
                return new ObjectResult(result);
            }
        }



        [HttpPost]
        [ActionName("Search")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult Search([FromBody]FilterOption filterOption)
        {
            List<AssetRegister> result = AssetRegisterRepository.search(filterOption,Request);
            if (result != null && result.Count > 0)
            {
                return new ObjectResult(result);
            }
            else
            {
                return new ObjectResult(result);
            }
        }



        [HttpGet]
        [ActionName("GetAssetDetail")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAssetDetail(long assertRegisterSequence)
        {
            List<AssetDetail> result = AssetRegisterRepository.getAssetsDetail(assertRegisterSequence, Request);
            if (result != null && result.Count > 0)
            {
                return new ObjectResult(result);
            }
            else
            {
                return new ObjectResult(result);
            }
        }
    }
}
