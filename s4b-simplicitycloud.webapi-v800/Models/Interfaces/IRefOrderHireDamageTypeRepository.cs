using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IRefOrderHireDamageTypeRepository : IRepository
    {
        List<RefOrderHireDamageType> GetAllDamageTypes(HttpRequest request);
    }
}
