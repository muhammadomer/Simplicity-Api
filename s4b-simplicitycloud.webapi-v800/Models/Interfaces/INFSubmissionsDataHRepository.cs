using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IS4BSubmissionsDataHRepository : IRepository
    {
        S4BSubmissionsDataH Insert(S4BSubmissionsDataH Obj, HttpRequest request);
        List<S4BSubmissionsDataH> GetAll(HttpRequest request, HttpResponse response);
    }
}
