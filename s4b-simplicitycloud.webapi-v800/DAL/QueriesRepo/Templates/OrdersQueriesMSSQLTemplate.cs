using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo.Templates
{
    public class OrdersQueriesMSSQLTemplate : OrdersQueriesTemplate
    {
        public static string SELECT_ORDERS_SEARCH_COLUMNS = "";
        public static string SELECT_ORDERS_COLUMNS_ALL = "";

        public OrdersQueriesMSSQLTemplate(string databaseType) :  base(databaseType)
        {

        }
    }
}
