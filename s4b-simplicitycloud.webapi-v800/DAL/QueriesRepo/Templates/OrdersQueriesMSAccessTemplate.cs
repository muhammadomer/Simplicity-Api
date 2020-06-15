using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo.Templates
{
    public class OrdersQueriesMSAccessTemplate : OrdersQueriesTemplate
    {
        public static string SELECT_ORDERS_SEARCH_COLUMNS = "sequence,job_ref,job_client_name,job_client,job_address_id,job_address,job_short_desc,";
        public static string SELECT_ORDERS_COLUMNS_ALL = "*";
        public OrdersQueriesMSAccessTemplate(string databaseType) :  base(databaseType)
        {
           
        }
        

    }
}
