
using SimplicityOnlineWebApi.Models.Interfaces;
using System.Collections.Generic;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class DiaryResourcesController : Controller
    {
        private readonly IDiaryResourcesRepository DiaryResourcesRepository;
        public DiaryResourcesController (IDiaryResourcesRepository diaryResourcesRepository)
            { this.DiaryResourcesRepository = diaryResourcesRepository; }

        [HttpGet]
        [ActionName("GetAllDiaryResources")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetAllDiaryResources()
        {
            List<DiaryResourcesMin> diaryResources = DiaryResourcesRepository.GetAllDiaryResources(Request);
            if (diaryResources == null)
            {
                return new ObjectResult(false);
            }
            return new ObjectResult(diaryResources);
        }

        [HttpGet]
        [ActionName("GetDiaryResourceNotesbyEntityId")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult GetDiaryResourceNotesbyEntityId(long entityId)
        {
            ResponseModel diaryResources = DiaryResourcesRepository.GetDiaryResourceNotesbyEntityId(Request,entityId);
            return new ObjectResult(diaryResources);
        }


    }
}
   