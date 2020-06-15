using SimplicityOnlineBLL.Entities;
using SimplicityOnlineDAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineDAL
{
    public class StockDetailsDB : MainDB
    {
        public StockDetailsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool InsertStockDetails(string stockCode, long entityId, string stockReorderLevel, string stockReorderAmount, string stockMinReorderAmount, string stockQuantityAvail,
                                       string stockQuantityOnOrder, double stockPriceSale, string stockShippingWeight, string stockBinLocation, double stockAmtLabour,
                                       string stockLabourHours, string stockMaterialWaste, long createdBy, DateTime dateCreated, long lastAmendedBy, DateTime dateLastAmended)
        {
            const string METHOD_NAME = "StockDetailsDB.InsertStockDetails()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(StockDetailsQueries.Insert(this.DatabaseType, stockCode, entityId, stockReorderLevel, stockReorderAmount, stockMinReorderAmount,
                                                                    stockQuantityAvail, stockQuantityOnOrder, stockPriceSale, stockShippingWeight, stockBinLocation,
                                                                    stockAmtLabour, stockLabourHours, stockMaterialWaste, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Inserting Stock Details.", ex);
            }
            return returnValue;
        }

        public List<StockDetails> SelectAllStockDetailsByStockCode(long stockCode)
        {
            const string METHOD_NAME = "StockDetailsDB.SelectAllStockDetailsByStockCode()";
            List<StockDetails> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(StockDetailsQueries.getSelectAllBystockCode(this.DatabaseType, stockCode), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<StockDetails>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadStockDetails(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Selecting Stock Details By Stock Code.", ex);
            }
            return returnValue;
        }

        public bool UpdateBystockCode(long stockCode, long entityId, string stockReorderLevel, string stockReorderAmount, string stockMinReorderAmount, string stockQuantityAvail,
                                      string stockQuantityOnOrder, double stockPriceSale, string stockShippingWeight, string stockBinLocation, double stockAmtLabour,
                                      string stockLabourHours, string stockMaterialWaste, long createdBy, DateTime dateCreated, long lastAmendedBy, DateTime dateLastAmended)
        {
            const string METHOD_NAME = "StockDetailsDB.UpdateBystockCode()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockDetailsQueries.Update(this.DatabaseType, stockCode, entityId, stockReorderLevel, stockReorderAmount, stockMinReorderAmount,
                                                                    stockQuantityAvail, stockQuantityOnOrder, stockPriceSale, stockShippingWeight, stockBinLocation,
                                                                    stockAmtLabour, stockLabourHours, stockMaterialWaste, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Updating Stock Details By Stock Code.", ex);
            }
            return returnValue;
        }

        public bool UpdateIncrementAvailableStockQtyByStockCodeAndEntityId(string stockCode, long entityId, double stockQtyAvail, 
                                                                           int lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "StockDetailsDB.UpdateIncrementAvailableStockQtyByStockCodeAndEntityId()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockDetailsQueries.DecrementStockAvailQtyByStockCodeAndEntityId(this.DatabaseType, stockCode, entityId, 
                                         stockQtyAvail, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Incrementing Available Stock Qty for Stock Details By Stock Code and Entity.", ex);
            }
            return returnValue;
        }

        public bool DeleteByStockCode(long stockCode)
        {
            const string METHOD_NAME = "StockDetailsDB.DeleteByStockCode()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockDetailsQueries.Delete(this.DatabaseType, stockCode), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Deleting Stock Details By Stock Code.", ex);
            }
            return returnValue;
        }

        public bool DeleteByFlgDeleted(long stockCode)
        {
            const string METHOD_NAME = "StockDetailsDB.DeleteByFlgDeleted()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(StockDetailsQueries.deleteFlagDeleted(this.DatabaseType, stockCode), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Deleting Stock Details By Flag Deleted.", ex);
            }
            return returnValue;
        }

        private StockDetails LoadStockDetails(OleDbDataReader dr)
        {
            const string METHOD_NAME = "StockDetailsDB.LoadStockDetails()";
            StockDetails stockDetails = null;
            try
            {
                if (dr != null)
                {
                    stockDetails = new StockDetails();
                    stockDetails.StockCode = DBUtil.GetStringValue(dr, "stock_code");
                    stockDetails.EntityId = DBUtil.GetLongValue(dr, "entity_id");
                    stockDetails.StockReorderLevel = DBUtil.GetDoubleValue(dr, "stock_reorder_level");
                    stockDetails.StockReorderAmount = DBUtil.GetDoubleValue(dr, "stock_reorder_amount");
                    stockDetails.StockMinReorderAmount = DBUtil.GetDoubleValue(dr, "stock_min_reorder_amount");
                    stockDetails.StockQuantityAvail = DBUtil.GetDoubleValue(dr, "stock_quantity_avail");
                    stockDetails.StockQuantityOnOrder = DBUtil.GetDoubleValue(dr, "stock_quantity_on_order");
                    stockDetails.StockShippingWeight = DBUtil.GetDoubleValue(dr, "stock_shipping_weight");
                    stockDetails.StockBinLocation = DBUtil.GetStringValue(dr, "stock_bin_location");
                    stockDetails.StockLabourHours = DBUtil.GetDoubleValue(dr, "stock_labour_hours");
                    stockDetails.StockMaterialWaste = DBUtil.GetDoubleValue(dr, "stock_material_waste");
                    stockDetails.CreatedBy = DBUtil.GetIntValue(dr, "created_by");
                    stockDetails.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    stockDetails.LastAmendedBy = DBUtil.GetIntValue(dr, "last_amended_by");
                    stockDetails.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Loading Stock Details.", ex);
            }
            return stockDetails;
        }
    }
}