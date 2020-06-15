using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
   public interface IOrderItemsRepository : IRepository
    {
        ResponseModel GetOrderItemsByJobSequence(ClientRequest clientRequest,int jobSequence, HttpRequest request, HttpResponse response);
        ResponseModel UpdateOrderItems(RequestModel Object, HttpRequest request);
        OrderItems CreateOrderItems(OrderItems Oi, HttpRequest request);
        List<EntityDetailsCoreMin> GetAllSupliers(HttpRequest request, string transType);
		ResponseModel GetOrderItemDescByItemCode(HttpRequest request, string itemCode);

	}
}
