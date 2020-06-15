using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class ProductListRepository : IProductListRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public ProductList getProductListByCode(HttpRequest Request, string productCode)
        {
            ProductList returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    ProductListDB prodctListDB = new ProductListDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = prodctListDB.selectProductByProductCode(productCode);
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }
    }
}
