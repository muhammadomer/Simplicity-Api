using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using Microsoft.AspNetCore.Http;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IEntityDetailsJoinRepository : IRepository
    {
        List<EntityDetailsJoin> GetSelectAllByEntityId(long entityId, HttpRequest Request);
        bool UpdateEntityDetailJoin(long entityId, string transType,  HttpRequest request);
        bool InsertEntityDetailJoin(long entityId, string transType,  HttpRequest request);
    }
}
