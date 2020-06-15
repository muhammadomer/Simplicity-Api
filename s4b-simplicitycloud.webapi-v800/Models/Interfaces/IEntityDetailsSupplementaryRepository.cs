using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;
using Microsoft.AspNetCore.Http;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IEntityDetailsSupplementaryRepository : IRepository
    {
        List<EntityDetailsSupplementary> GetSelectAllByEntityId(long entityId, HttpRequest Request);
        bool UpdateEntityDetailSupplementary(long entityId, string dataType, string data, HttpRequest request);
        bool InsertEntityDetailSupplementary(HttpRequest request, long entityId, string dataType, string data);
    }
}
