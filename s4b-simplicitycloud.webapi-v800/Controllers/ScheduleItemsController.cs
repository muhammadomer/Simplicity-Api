using SimplicityOnlineWebApi.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SimplicityOnlineWebApi.Controllers
{
    [Route("api/[controller]")]
    public class ScheduleItemsController : Controller
    {
        private readonly IScheduleItemsRepository ScheduleItemsRepository;
        public ScheduleItemsController(IScheduleItemsRepository scheduleItemsRepository)
        {
            this.ScheduleItemsRepository=scheduleItemsRepository;
        }


        [HttpGet]
        [ActionName("GetItemsGroupsHierarchy")]
        [Route("[action]")]
        public IActionResult GetItemsGroupsHierarchy(int groupId)
        {   
            return new ObjectResult(ScheduleItemsRepository.GetItemsGroupsHierarchy(HttpContext.Request, groupId));
        }

        [HttpGet]
        [ActionName("GetItemsGroupsDesc")]
        [Route("[action]")]
        public IActionResult GetItemsGroupsDesc()
        {
            return new ObjectResult(ScheduleItemsRepository.GetItemsGroupsDesc(HttpContext.Request));
        }

        [HttpGet]
        [ActionName("GetScheduleItemsByGroup")]
        [Route("[action]")]
        public IActionResult GetScheduleItemsByGroup(int groupId,string parentCode)
        {
            return new ObjectResult(ScheduleItemsRepository.GetScheduleItemsByGroup(HttpContext.Request,groupId,parentCode));
        }

    }
}
