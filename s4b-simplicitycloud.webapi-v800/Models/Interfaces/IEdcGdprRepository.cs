using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
   public interface IEdcGdprRepository : IRepository
    {
        ResponseModel InsertGDPR(HttpRequest request, EdcGdpr obj);
        ResponseModel Update(HttpRequest request, EdcGdpr obj);
        ResponseModel GetByEntityId(HttpRequest Request, long entityId);
    }
}
