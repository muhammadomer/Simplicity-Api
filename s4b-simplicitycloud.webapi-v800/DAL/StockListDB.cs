using SimplicityOnlineBLL.Entities;
using SimplicityOnlineDAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineDAL
{
    public class StockListDB : MainDB
    {

        public StockListDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool Insert(string stockCode, long entityId, long stockTypeId, string stockUnits, double stockCostPrice, DateTime? dateCpLastAmended, 
                           string stockBarCode, int sageId, string sageNominalCode, string sageTaxCode, byte[] stockPicture, 
                           int createdBy, DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "StockListDB.Insert()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(StockListQueries.Insert(this.DatabaseType, stockCode, entityId, stockTypeId, stockUnits, stockCostPrice, dateCpLastAmended, stockBarCode,
                                                                 sageId, sageNominalCode, sageTaxCode, stockPicture, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        returnValue = true;
                    }
                }                
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Inserting Stock List.", ex);
            }
            return returnValue;
        }

        public StockList SelectByStockCodeAndEntityId(string stockCode, long entityId)
        {
            const string METHOD_NAME = "StockListDB.SelectByStockCodeAndEntityId()";
            StockList returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(StockListQueries.SelectAllByStockCodeAndEntityId(this.DatabaseType, stockCode, entityId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadStockList(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Selecting Stock By Stock Code And Entity Id.", ex);
            }
            return returnValue;
        }


        public bool updateBystockCode(long stockCode, long entityId, long stockTypeId, string stockUnits, double stockCostPrice, DateTime dateCpLastAmended, string stockBarCode,
                                    long sageId, string sageNominalCode, string sageTaxCode, byte[] stockPicture, long createdBy, DateTime dateCreated, long lastAmendedBy,
                                    DateTime dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockListQueries.update(this.DatabaseType, stockCode, entityId, stockTypeId, stockUnits, stockCostPrice, dateCpLastAmended, stockBarCode,
                                                                sageId, sageNominalCode, sageTaxCode, stockPicture, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool deleteBystockCode(long stockCode)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockListQueries.delete(this.DatabaseType, stockCode), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool deleteByFlgDeleted(long stockCode)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockListQueries.deleteFlagDeleted(this.DatabaseType, stockCode), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        private StockList LoadStockList(OleDbDataReader dr)
        {
            const string METHOD_NAME = "StockListDB.LoadStockList()";
            StockList stockList = null;
            try
            {
                if (dr != null)
                {
                    stockList = new StockList();
                    stockList.StockCode = DBUtil.GetStringValue(dr, "stock_code");
                    stockList.EntityId = DBUtil.GetLongValue(dr, "entity_id");
                    stockList.StockTypeId = DBUtil.GetLongValue(dr, "stock_type_id");
                    stockList.StockUnits = DBUtil.GetStringValue(dr, "stock_units");
                    stockList.DateCpLastAmended = DBUtil.GetDateTimeValue(dr, "date_cp_last_amended");
                    stockList.StockBarCode = DBUtil.GetStringValue(dr, "stock_bar_code");
                    stockList.SageId = DBUtil.GetLongValue(dr, "sage_id");
                    stockList.SageNominalCode = DBUtil.GetStringValue(dr, "sage_nominal_code");
                    stockList.SageTaxCode = DBUtil.GetStringValue(dr, "sage_tax_code");
                    stockList.CreatedBy = DBUtil.GetIntValue(dr, "created_by");
                    stockList.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    stockList.LastAmendedBy = DBUtil.GetIntValue(dr, "last_amended_by");
                    stockList.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Loading Stock List.", ex);
            }
            return stockList;
        }
    }
}