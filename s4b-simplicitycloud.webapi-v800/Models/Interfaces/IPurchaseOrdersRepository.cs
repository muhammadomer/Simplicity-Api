using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IPurchaseOrdersRepository : IRepository
    {
        string GenerateNewPORef(HttpRequest Request);
        PurchaseOrders Insert(HttpRequest Request, PurchaseOrders obj);
    }
}
