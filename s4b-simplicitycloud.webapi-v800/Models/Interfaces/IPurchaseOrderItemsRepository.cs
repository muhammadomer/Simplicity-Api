using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IPurchaseOrderItemsRepository : IRepository
    {
        PurchaseOrderItems Insert(HttpRequest Request, PurchaseOrderItems obj);
        ResponseModel GetAllPurchaseOrderItems(HttpRequest Request, ClientRequest clientRequest);
    }
}
