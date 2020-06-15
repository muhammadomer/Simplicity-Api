using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefItemTypeQueries
    {
        public static string selectItemType(string databaseType, bool isAllItems)
        {
            string returnValue = "";
            try
            {
               
                returnValue = @"SELECT un_ref_order_item_type.type_sequence, un_ref_order_item_type.type_desc
                FROM un_ref_order_item_type ";
                if (isAllItems == false)
                {
                    returnValue += " Where un_ref_order_item_type.type_sequence<=3";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        
    }
}

