using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IRefItemTypeRepository : IRepository
    {
        ResponseModel GetItemType(HttpRequest request, bool isAllItem);
    }
}
