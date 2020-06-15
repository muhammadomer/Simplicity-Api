using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IProductListRepository : IRepository
    {
        ProductList getProductListByCode(HttpRequest Request, string productCode);
    }
}
