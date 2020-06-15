using SimplicityOnlineBLL.Entities;
using SimplicityOnlineDAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineDAL
{
    public class StockJobReceivedDB : MainDB
    {

        public StockJobReceivedDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool Insert(out long sequence, long requestSequence, string deliveryRef, string transType, long entityId, 
                           DateTime? stockRecievedDate, string stockCode, double stockQuantity, double stockAmount, 
                           bool flgFromStockroom, long jobSequence, int createdBy, DateTime? dateCreated, int lastAmendedBy,
                           DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "StockJobReceivedDB.Insert()";
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(StockJobReceivedQueries.Insert(this.DatabaseType, requestSequence, deliveryRef, transType, entityId, stockRecievedDate,
                                                                        stockCode, stockQuantity, stockAmount, flgFromStockroom, jobSequence, createdBy, dateCreated,
                                                                        lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Inserting Stock Job Received.", ex);
            }
            return returnValue;
        }

        public List<StockJobReceived> SelectAllStockJobReceivedSequence(long sequence)
        {
            const string METHOD_NAME = "StockJobReceivedDB.SelectAllStockJobReceivedSequence()";
            List<StockJobReceived> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(StockJobReceivedQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<StockJobReceived>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadStockJobReceived(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Selecting Stock Job Received By Sequence.", ex);
            }
            return returnValue;
        }

        public bool UpdateBySequence(long sequence, long requestSequence, string deliveryRef, string transType, long entityId, DateTime stockRecievedDate, string stockCode,
                                    string stockQuantity, double stockAmount, bool flgFromStockroom, long jobSequence, long createdBy, DateTime dateCreated, long lastAmendedBy,
                                    DateTime dateLastAmended)
        {
            const string METHOD_NAME = "StockJobReceivedDB.UpdateBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockJobReceivedQueries.update(this.DatabaseType, sequence, requestSequence, deliveryRef, transType, entityId, stockRecievedDate,
                                                                        stockCode, stockQuantity, stockAmount, flgFromStockroom, jobSequence, createdBy, dateCreated,
                                                                        lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Updating Stock Job Received By Sequence.", ex);
            }
            return returnValue;
        }

        public bool DeleteBySequence(long sequence)
        {
            const string METHOD_NAME = "StockJobReceivedDB.DeleteBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockJobReceivedQueries.delete(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Deleting Stock Job Received By Sequence.", ex);
            }
            return returnValue;
        }

        public bool DeleteByFlgDeleted(long sequence)
        {
            const string METHOD_NAME = "StockJobReceivedDB.DeleteByFlgDeleted()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockJobReceivedQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Deleting Stock Job Received By Flag Deleted And Sequence.", ex);
            }
            return returnValue;
        }

        private StockJobReceived LoadStockJobReceived(OleDbDataReader dr)
        {
            const string METHOD_NAME = "StockJobReceivedDB.Load_StockJobReceived()";
            StockJobReceived stockJobReceived = null;
            try
            {
                if (dr != null)
                {
                    stockJobReceived = new StockJobReceived();
                    stockJobReceived.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    stockJobReceived.RequestSequence = DBUtil.GetLongValue(dr, "request_sequence");
                    stockJobReceived.DeliveryRef = DBUtil.GetStringValue(dr, "delivery_ref");
                    stockJobReceived.TransType = DBUtil.GetStringValue(dr, "trans_type");
                    stockJobReceived.EntityId = DBUtil.GetLongValue(dr, "entity_id");
                    stockJobReceived.StockRecievedDate = DBUtil.GetDateTimeValue(dr, "stock_recieved_date");
                    stockJobReceived.StockCode = DBUtil.GetStringValue(dr, "stock_code");
                    stockJobReceived.StockQuantity = DBUtil.GetIntValue(dr, "stock_quantity");
                    stockJobReceived.FlgFromStockroom = DBUtil.GetBooleanValue(dr, "flg_from_stockroom");
                    stockJobReceived.JobSequence = DBUtil.GetLongValue(dr, "job_sequence");
                    stockJobReceived.CreatedBy = DBUtil.GetIntValue(dr, "created_by");
                    stockJobReceived.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    stockJobReceived.LastAmendedBy = DBUtil.GetIntValue(dr, "last_amended_by");
                    stockJobReceived.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Loading Stock Job Received.", ex);
            }
            return stockJobReceived;
        }
    }
}