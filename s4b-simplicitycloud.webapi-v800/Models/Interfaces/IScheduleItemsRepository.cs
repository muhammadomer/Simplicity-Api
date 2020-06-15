using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
   public interface IScheduleItemsRepository : IRepository
    {
        ResponseModel GetItemsGroupsHierarchy(HttpRequest request,int groupId);
        ResponseModel GetItemsGroupsDesc(HttpRequest request);
        ResponseModel GetScheduleItemsByGroup(HttpRequest request, int groupId, string parentCode);

    }
}
