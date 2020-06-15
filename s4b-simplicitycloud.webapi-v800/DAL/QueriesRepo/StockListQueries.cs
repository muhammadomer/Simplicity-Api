using System;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineDAL.QueriesRepo
{
    public static class StockListQueries
    {
        public static string SelectAllByStockCodeAndEntityId(string databaseType, string stockCode, long entityId)
        {
            return @"SELECT * FROM  un_stock_list
                      WHERE stock_code = '" + stockCode + "'" +
                    "   AND entity_id = " + entityId;
        }

        public static string Insert(string databaseType, string stockCode, long entityId, long stockTypeId, string stockUnits, double stockCostPrice, 
                                    DateTime? dateCpLastAmended, string stockBarCode, int sageId, string sageNominalCode, string sageTaxCode, 
                                    byte[] stockPicture, int createdBy, DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"INSERT INTO un_stock_list(stock_code, entity_id, stock_type_id, stock_units, stock_cost_price, date_cp_last_amended, stock_bar_code,
                            sage_id, sage_nominal_code, sage_tax_code, stock_picture, created_by, date_created, last_amended_by, date_last_amended)
                    VALUES ('" + stockCode + "', " + entityId + ", " + stockTypeId + ", '" + stockUnits + "', " + stockCostPrice + ", " +
                            Utilities.GetDateTimeForDML(databaseType, dateCpLastAmended, true, true) + ", '" + stockBarCode + "', " + sageId + ", '" + 
                            sageNominalCode + "', '" + sageTaxCode + "', " + stockPicture + ", " + createdBy + ", " + 
                            Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " + lastAmendedBy + ", " +
                            Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
        }

        public static string update(string databaseType, long stockCode, long entityId, long stockTypeId, string stockUnits, double stockCostPrice, DateTime dateCpLastAmended,
                                    string stockBarCode, long sageId, string sageNominalCode, string sageTaxCode, byte[] stockPicture, long createdBy, DateTime dateCreated,
                                    long lastAmendedBy, DateTime dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = " UPDATE   un_stock_list" +
                    "   SET   entity_id =  " + entityId + ",  " +
                    " stock_type_id =  " + stockTypeId + ",  " +
                    " stock_units =  '" + stockUnits + "',  " +
                    " stock_cost_price =  " + stockCostPrice + ",  " +
                    " date_cp_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateCpLastAmended,true,true) + ", " +
                    " stock_bar_code =  '" + stockBarCode + "',  " +
                    " sage_id =  " + sageId + ",  " +
                    " sage_nominal_code =  '" + sageNominalCode + "',  " +
                    " sage_tax_code =  '" + sageTaxCode + "',  " +
                    " stock_picture =  " + stockPicture + ",  " +
                    " created_by =  " + createdBy + ",  " +
                    " date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " +
                    " last_amended_by =  " + lastAmendedBy + ",  " +
                    " date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " +
                    "  WHERE stock_code = " + stockCode;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string delete(string databaseType, long stockCode)
        {
            string returnValue = "";
            try
            {
               
                returnValue = " DELETE FROM   un_stock_list" +
                            " WHERE stock_code = " + stockCode;
            }

            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string deleteFlagDeleted(string databaseType, long stockCode)
        {
            string returnValue = "";
            try
            {
                
                bool flg = true;
                returnValue = " UPDATE   un_stock_list" +
                    "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg) +
                    " WHERE stock_code = " + stockCode;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

