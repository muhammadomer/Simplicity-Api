using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IDiaryResourcesRepository : IRepository
    {
        List<DiaryResourcesMin> GetAllDiaryResources(HttpRequest Request);
        ResponseModel GetDiaryResourceNotesbyEntityId(HttpRequest Request, long entityId);
    }
}
