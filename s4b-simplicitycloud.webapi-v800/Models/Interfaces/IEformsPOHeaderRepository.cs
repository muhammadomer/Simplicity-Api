using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IEformsPOHeaderRepository : IRepository
    {
        EformsPoHeader Insert(HttpRequest Request, EformsPoHeader obj);
    }
}
