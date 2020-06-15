using SimplicityOnlineBLL.Entities;

using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class OrdersMeSchHeaderDB : MainDB
    {

        public OrdersMeSchHeaderDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }
        public OrdersMeSchHeader UpdateOrdersMeSchHeader(OrdersMeSchHeader Object)
        {
			OrdersMeSchHeader returnValue = new OrdersMeSchHeader();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrderMeSchHeaderQueries.update(this.DatabaseType, Object), conn))
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
        public OrdersMeSchHeader InsertOrdersMeSchHeader(OrdersMeSchHeader Object)
        {
			OrdersMeSchHeader returnValue = new OrdersMeSchHeader();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrderMeSchHeaderQueries.insert(this.DatabaseType, Object), conn))
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
