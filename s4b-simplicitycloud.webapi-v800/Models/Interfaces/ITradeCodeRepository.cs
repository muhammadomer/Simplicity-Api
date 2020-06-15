using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface ITradeCodeRepository : IRepository
    {
        List<TradeCode> GetAllTradeCodes(HttpRequest request);
    }
}
