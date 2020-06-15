using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface ISubConPOHeaderRepository : IRepository
    {
        SubConPoHeader Insert(HttpRequest Request, SubConPoHeader obj);
    }
}
