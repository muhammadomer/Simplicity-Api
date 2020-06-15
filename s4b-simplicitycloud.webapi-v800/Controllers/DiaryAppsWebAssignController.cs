using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.BLL.Entities;
using Microsoft.AspNetCore.Mvc;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class DiaryAppsWebAssignController : Controller
    {
        private readonly IDiaryAppsWebAssignRepository AppsWebAssignRepository;
        public DiaryAppsWebAssignController(IDiaryAppsWebAssignRepository appsWebAssignRepository)
            {this.AppsWebAssignRepository = appsWebAssignRepository; }

        [HttpGet]
        [ActionName("GetAllWebAssignApp")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllWebAssignApp()
        {
            List<DiaryAppsWebAssign> ObjList = AppsWebAssignRepository.GetAllWebAssignApp(Request, HttpContext.Response);
            if (ObjList == null)
            {
                return new ObjectResult(ObjList);
            }


            return new ObjectResult(ObjList);
        }
        [HttpPost]
        [ActionName("AddWebAssignApp")]
        [Route("[action]")]
        public IActionResult AddWebAssignApp([FromBody]DiaryAppsWebAssign WebAssignObj)
        {
            DiaryAppsWebAssign Obj = AppsWebAssignRepository.AddWebAssignObject(WebAssignObj, HttpContext.Request);
            if (Obj == null)
            {
                return new ObjectResult(Obj);
            }
            else
            {
                return new ObjectResult(Obj);
            }
        }
   
        [HttpPost]
        [ActionName("UpdateWebAssignApp")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult UpdateWebAssignApp([FromBody]DiaryAppsWebAssign WebAssignObj)
        {
            DiaryAppsWebAssign Obj = AppsWebAssignRepository.UpdateWebAssignObject(WebAssignObj, HttpContext.Request);
            if (Obj == null)
            {
                return new ObjectResult(Obj);
            }
            else
            {
                return new ObjectResult(Obj);
            }

        }

        [HttpPost]
        [ActionName("UpdateWebAssignByCriteria")]
        [Route("[action]")]
        //[ValidateRequestState]
        public IActionResult UpdateWebAssignByCriteria([FromBody]DiaryAppsWebAssign WebAssignObj)
        {
            DiaryAppsWebAssign Obj = AppsWebAssignRepository.UpdateWebAssignByCriteria(WebAssignObj, HttpContext.Request);
            if (Obj == null)
            {
                return new ObjectResult(Obj);
            }
            else
            {
                return new ObjectResult(Obj);
            }

        }

        [HttpPost]
        [ActionName("GetThirdPartyApp")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetThirdPartyApp([FromBody]DiaryAppsWebAssign WebAssignObj)
        {
           List<DiaryAppsWebAssign> Obj = AppsWebAssignRepository.GetThirdPartyApp(WebAssignObj, HttpContext.Request);
            if (Obj == null)
            {
                return new ObjectResult(Obj);
            }
            else
            {
                return new ObjectResult(Obj);
            }
        }
       
    }
}
