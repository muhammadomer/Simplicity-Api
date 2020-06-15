using SimplicityOnlineBLL.Entities;

using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class OrdersMeSchItemsDB : MainDB
    {

        public OrdersMeSchItemsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }
        public OrdersMeSchItems UpdateOrdersMeSchItems(OrdersMeSchItems Object)
        {
			OrdersMeSchItems returnValue = new OrdersMeSchItems();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrderMeSchItemsQueries.update(this.DatabaseType, Object), conn))
                    {
                       int result = objCmdInsert.ExecuteNonQuery();
                        if (result > 0)
                        {
                            returnValue = Object;
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
                returnValue = null;
            }
            return returnValue;
        }
        public OrdersMeSchItems InsertOrdersMeSchItems(OrdersMeSchItems Object)
        {
			OrdersMeSchItems returnValue = new OrdersMeSchItems();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrderMeSchItemsQueries.insert(this.DatabaseType, Object), conn))
                    {
                        int result = objCmdInsert.ExecuteNonQuery();
                        if (result > 0)
                        {
                            long sequence = Utilities.GetDBAutoNumber(conn);
                            Object.Sequence = sequence;
                            returnValue = Object;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
				//errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
				// Requires Logging
				throw ex;
            }
            return returnValue;
        }


       
    }
}
