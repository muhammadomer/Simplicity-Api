using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IEformsPOItemsRepository : IRepository
    {
        EFormsPOItems Insert(HttpRequest Request, EFormsPOItems obj);
    }
}
