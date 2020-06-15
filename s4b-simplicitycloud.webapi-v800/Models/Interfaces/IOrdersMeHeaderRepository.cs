using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
   public interface IOrdersMeHeaderRepository : IRepository
    {
        OrdersMeHeader GetOrdersMeHeaderBySequence(long sequence, HttpRequest request);
		OrdersMeHeader GetOrdersMeHeaderByJobSequence(long jobSequence, HttpRequest request);
	}
}
