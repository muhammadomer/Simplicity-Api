using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IRefJobStatusTypeRepository : IRepository
    {
        List<RefJobStatusType> GetAllJobStatusTypes(HttpRequest request, HttpResponse response);
    }
}
