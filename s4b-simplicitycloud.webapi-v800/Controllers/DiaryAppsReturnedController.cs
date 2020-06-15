
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class DiaryAppsReturnedController : Controller
    {
        private readonly IDiaryAppsReturnedRepository DiaryAppsReturnedRepository;
        public DiaryAppsReturnedController(IDiaryAppsReturnedRepository diaryAppsReturnedRepository)
        { this.DiaryAppsReturnedRepository =diaryAppsReturnedRepository; }

        [HttpPost]
        [ActionName("AddDiaryAppReturnedWithOrderStatus")]
        [Route("[action]")]
        [ValidateRequestState]
        public IActionResult AddDiaryAppReturnedWithOrderStatus([FromBody]DiaryAppsReturned diaryAppReturned)
        {
            ResponseModel response = DiaryAppsReturnedRepository.AddDiaryAppReturnedWithOrderStatus(Request, diaryAppReturned);
            if (response == null)
            {
                response = new ResponseModel();
                response.IsSucessfull = false;
            }
            return new ObjectResult(response);
        }
    }
}
