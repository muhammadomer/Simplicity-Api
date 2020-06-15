using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IRefOrderTypeRepository : IRepository
    {
        List<RefOrderType> GetAllOrderTypes(HttpRequest request);
    }
}
