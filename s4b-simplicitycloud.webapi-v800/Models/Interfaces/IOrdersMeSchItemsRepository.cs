using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
   public interface IOrdersMeSchHeaderRepository : IRepository
    {

		OrdersMeSchHeader Update(OrdersMeSchHeader Oi, HttpRequest request);
		OrdersMeSchHeader Insert(OrdersMeSchHeader Oi, HttpRequest request);
        
    }
}
