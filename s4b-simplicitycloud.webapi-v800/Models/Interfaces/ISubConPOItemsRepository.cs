using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface ISubConPOItemsRepository : IRepository
    {
        SubConPOItems Insert(HttpRequest Request, SubConPOItems obj); 
    }
}
