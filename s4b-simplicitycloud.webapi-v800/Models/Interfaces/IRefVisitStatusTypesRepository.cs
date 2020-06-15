using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IRefVisitStatusTypesRepository : IRepository
    {
        List<RefVisitStatusTypes> GetAllVisitStatusTypes(HttpRequest request, HttpResponse response);
        RefVisitStatusTypes AddVisitStatusTypes(RefVisitStatusTypes Obj, HttpRequest request);
        RefVisitStatusTypes GetVisitStatusById(int UserId, HttpRequest Request, HttpResponse Response);
        RefVisitStatusTypes UpdateVisitStatus(RefVisitStatusTypes userName, HttpRequest request);

    }
}
