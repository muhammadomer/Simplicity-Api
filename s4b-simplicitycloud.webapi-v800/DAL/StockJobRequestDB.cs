using SimplicityOnlineBLL.Entities;
using SimplicityOnlineDAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineDAL
{
    public class StockJobRequestDB : MainDB
    {

        public StockJobRequestDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool InsertStockJobRequest(out long sequence, long jobSequence, long joinSequence, string transType, long entityId, string stockCode, 
                                          string stockUnit, string stockDesc, double stockQuantity, double stockAmountEst, 
                                          DateTime? stockRequestedDate, DateTime? dateStockRequired, bool flgStockOrdered,
                                          DateTime? stockOrderedDate, bool flgStockReceived, DateTime? stockReceivedDate, 
                                          bool flgSorDrillDown, string sorItemCode, int itemType, double itemHours, 
                                          int createdBy, DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "StockJobRequestDB.InsertStockJobRequest()";
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(StockJobRequestQueries.Insert(this.DatabaseType, jobSequence, joinSequence, transType, entityId, stockCode, stockUnit,
                                                                       stockDesc, stockQuantity, stockAmountEst, stockRequestedDate, dateStockRequired, flgStockOrdered,
                                                                       stockOrderedDate, flgStockReceived, stockReceivedDate, flgSorDrillDown, sorItemCode, itemType,
                                                                       itemHours, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Inserting Stock Job Request.", ex);
            }
            return returnValue;
        }

        public List<StockJobRequest> GetAllStockJobRequestSequence(long sequence)
        {
            const string METHOD_NAME = "StockJobRequestDB.GetAllStockJobRequestSequence()";
            List<StockJobRequest> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(StockJobRequestQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<StockJobRequest>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadStockJobRequest(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting Stock Job Request.", ex);
            }
            return returnValue;
        }

        public bool Update(long sequence, long jobSequence, long joinSequence, string transType, long entityId, string stockCode, string stockUnit,
                                    string stockDesc, double stockQuantity, double stockAmountEst, DateTime? stockRequestedDate, DateTime? dateStockRequired, bool flgStockOrdered,
                                    DateTime stockOrderedDate, bool flgStockReceived, DateTime? stockReceivedDate, bool flgSorDrillDown, string sorItemCode, int itemType,
                                    double itemHours, int createdBy, DateTime dateCreated, int lastAmendedBy, DateTime dateLastAmended)
        {
            const string METHOD_NAME = "StockJobRequestDB.Update()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockJobRequestQueries.Update(this.DatabaseType, sequence, jobSequence, joinSequence, transType, entityId, stockCode, stockUnit,
                                                                       stockDesc, stockQuantity, stockAmountEst, stockRequestedDate, dateStockRequired, flgStockOrdered,
                                                                       stockOrderedDate, flgStockReceived, stockReceivedDate, flgSorDrillDown, sorItemCode, itemType,
                                                                       itemHours, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Updating Stock Job Request.", ex);
            }
            return returnValue;
        }

        public bool DeleteBySequence(long sequence)
        {
            const string METHOD_NAME = "StockJobRequestDB.DeleteBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockJobRequestQueries.delete(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Deleting Stock Job Request.", ex);
            }
            return returnValue;
        }

        public bool DeleteByFlgDeletedAndSequence(long sequence)
        {
            const string METHOD_NAME = "StockJobRequestDB.DeleteByFlgDeletedAndSequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockJobRequestQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Deleting Stock Job Request.", ex);
            }
            return returnValue;
        }

        private StockJobRequest LoadStockJobRequest(OleDbDataReader dr)
        {
            const string METHOD_NAME = "StockJobRequestDB.LoadStockJobRequest()";
            StockJobRequest stockJobRequest = null;
            try
            {
                if (dr != null)
                {
                    stockJobRequest = new StockJobRequest();
                    stockJobRequest.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    stockJobRequest.JobSequence = DBUtil.GetLongValue(dr, "job_sequence");
                    stockJobRequest.JoinSequence = DBUtil.GetLongValue(dr, "join_sequence");
                    stockJobRequest.TransType = DBUtil.GetStringValue(dr, "trans_type");
                    stockJobRequest.EntityId = DBUtil.GetLongValue(dr, "entity_id");
                    stockJobRequest.StockCode = DBUtil.GetStringValue(dr, "stock_code");
                    stockJobRequest.StockUnit = DBUtil.GetStringValue(dr, "stock_unit");
                    stockJobRequest.StockDesc = DBUtil.GetStringValue(dr, "stock_desc");
                    stockJobRequest.StockQuantity = DBUtil.GetDoubleValue(dr, "stock_quantity");
                    stockJobRequest.StockRequestedDate = DBUtil.GetDateTimeValue(dr, "stock_requested_date");
                    stockJobRequest.DateStockRequired = DBUtil.GetDateTimeValue(dr, "date_stock_required");
                    stockJobRequest.FlgStockOrdered = bool.Parse(dr["flg_stock_ordered"].ToString());
                    stockJobRequest.StockOrderedDate = DBUtil.GetDateTimeValue(dr, "stock_ordered_date");
                    stockJobRequest.FlgStockReceived = bool.Parse(dr["flg_stock_received"].ToString());
                    stockJobRequest.StockReceivedDate = DBUtil.GetDateTimeValue(dr, "stock_received_date");
                    stockJobRequest.FlgSorDrillDown = bool.Parse(dr["flg_sor_drill_down"].ToString());
                    stockJobRequest.SorItemCode = DBUtil.GetStringValue(dr, "sor_item_code");
                    stockJobRequest.ItemType = DBUtil.GetIntValue(dr, "item_type");
                    stockJobRequest.ItemHours = DBUtil.GetDoubleValue(dr, "item_hours");
                    stockJobRequest.CreatedBy = DBUtil.GetIntValue(dr, "created_by");
                    stockJobRequest.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    stockJobRequest.LastAmendedBy = DBUtil.GetIntValue(dr, "last_amended_by");
                    stockJobRequest.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Loading Stock Job Request.", ex);
            }
            return stockJobRequest;
        }
    }
}