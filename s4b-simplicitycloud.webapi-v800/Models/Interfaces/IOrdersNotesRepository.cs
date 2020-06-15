using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IOrdersNotesRepository : IRepository
    {
        OrdersNotes Insert(HttpRequest request, OrdersNotes obj);
        List<OrdersNotes> GetOrderNotesBySequence(HttpRequest Request, long sequence);
        OrdersNotes update(HttpRequest request, OrdersNotes obj);
    }
}
