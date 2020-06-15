using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo.Templates
{
    public class OrdersQueriesTemplate
    {
        string databaseType;
        public static string ORDERS_TABLE = "un_orders";
        public static string ORDERS_SEARCH_PAGE = "orderSearchPage";
        public OrdersQueriesTemplate(string databaseType)
        {
            this.databaseType = databaseType;

        }
        public string buildQuery(string pageId, int offset, int limit)
        {
            if (pageId.Equals("orderSearchPage"))
            {
                return buildOrderSearchQuery(offset, limit);
            }
            else
            {
                return "";
            }
        }
        public string buildOrderSearchQuery(int offset, int limit)
        {
            return "";       
        }
        
    }
}
