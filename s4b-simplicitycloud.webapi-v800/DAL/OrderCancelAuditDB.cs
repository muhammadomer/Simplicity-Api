using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.IO;
namespace SimplicityOnlineWebApi.DAL
{
    public class OrderCancelAuditDB : MainDB
    {
        public OrderCancelAuditDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insert(out long sequence, OrderCancelAudit orderCancel)
        {
            bool returnValue = false;
            sequence = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrderCancelAuditQueries.insert(this.DatabaseType, orderCancel), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        string sql = "select @@IDENTITY";
                        using (OleDbCommand objCommand =new OleDbCommand(sql, conn))
                        {
                           OleDbDataReader dr = objCommand.ExecuteReader();
                           if (dr.HasRows)
                           {
                              dr.Read();
                              sequence = long.Parse(dr[0].ToString());
                           }
                           else
                           {
                              //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record. '" + METHOD_NAME + "'\n";
                           }
                        }
                  }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured in inserting Order Cancel Audit:" + ex.Message);
                throw ex;
            }
            return returnValue;
        }
     
    }
}
