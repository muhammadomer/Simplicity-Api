using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefOrderTypeQueries
    {
        public static string getSelectAll(string databaseType)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = " Select * From un_ref_order_type order by order_type_desc_short";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByorderTypeId(string databaseType, long orderTypeId)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = " SELECT * " +
                                      "  FROM    un_ref_order_type" +
                                      " WHERE order_type_id = " + orderTypeId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, long orderTypeId, string orderTypeDescShort, string orderTypeDescLong)
        {
            string returnValue = "";
            try
            {
                        returnValue = "INSERT INTO   un_ref_order_type( order_type_id,  order_type_desc_short,  order_type_desc_long)" +
                                      "VALUES ( " + orderTypeId + ",   '" + orderTypeDescShort + "',   '" + orderTypeDescLong + "')";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string update(string databaseType, long orderTypeId, string orderTypeDescShort, string orderTypeDescLong)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = " UPDATE   un_ref_order_type" +
                                      "  SET  order_type_desc_short =  '" + orderTypeDescShort + "',  " +
                                      " order_type_desc_long =  '" + orderTypeDescLong + "',  " +
                                      "  WHERE order_type_id = " + orderTypeId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string delete(string databaseType, long orderTypeId)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = " DELETE FROM   un_ref_order_type" +
                                      " WHERE order_type_id = " + orderTypeId;
            }

            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

