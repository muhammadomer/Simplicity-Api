using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IOrdersDistributionListRepository : IRepository
    {
        List<OrdersDistributionList> GetByJobSequence(HttpRequest Request, long jobSequence);
    }
}
