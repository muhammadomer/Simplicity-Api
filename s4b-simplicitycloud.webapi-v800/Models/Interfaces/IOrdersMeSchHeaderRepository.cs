using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
   public interface IOrdersMeSchItemsRepository : IRepository
    {

		OrdersMeSchItems Update(OrdersMeSchItems Oi, HttpRequest request);
		OrdersMeSchItems Insert(OrdersMeSchItems Oi, HttpRequest request);
        
    }
}
