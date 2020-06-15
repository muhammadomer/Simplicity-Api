using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineDAL.QueriesRepo
{
    public static class StockDetailsQueries
    {

        public static string getSelectAllBystockCode(string databaseType, long stockCode)
        {
            string returnValue = "";
            try
            {
                returnValue = " SELECT * " +
                                      "  FROM    un_stock_details" +
                                      " WHERE stock_code = " + stockCode;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string Insert(string databaseType, string stockCode, long entityId, string stockReorderLevel, string stockReorderAmount, string stockMinReorderAmount,
                                    string stockQuantityAvail, string stockQuantityOnOrder, double stockPriceSale, string stockShippingWeight, string stockBinLocation,
                                    double stockAmtLabour, string stockLabourHours, string stockMaterialWaste, long createdBy, DateTime dateCreated, long lastAmendedBy,
                                    DateTime dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO   un_stock_details(stock_code,  entity_id,  stock_reorder_level,  stock_reorder_amount,  stock_min_reorder_amount," +
                                      "                               stock_quantity_avail,  stock_quantity_on_order,  stock_price_sale,  stock_shipping_weight," +
                                      "                               stock_bin_location,  stock_amt_labour,  stock_labour_hours,  stock_material_waste,  created_by, " +
                                      "                               date_created,  last_amended_by,  date_last_amended)" +
                                      "VALUES ('" + stockCode + "',  " + entityId + ",   '" + stockReorderLevel + "',   '" + stockReorderAmount + "',   '" +
                                      stockMinReorderAmount + "',   '" + stockQuantityAvail + "',   '" + stockQuantityOnOrder + "',  " + stockPriceSale + ",   '" +
                                      stockShippingWeight + "',   '" + stockBinLocation + "',  " + stockAmtLabour + ",   '" + stockLabourHours + "',   '" +
                                      stockMaterialWaste + "',  " + createdBy + ",   " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true ,true) + ",  " + lastAmendedBy + ",   " +
                                      Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string Update(string databaseType, long stockCode, long entityId, string stockReorderLevel, string stockReorderAmount, string stockMinReorderAmount,
                                    string stockQuantityAvail, string stockQuantityOnOrder, double stockPriceSale, string stockShippingWeight, string stockBinLocation,
                                    double stockAmtLabour, string stockLabourHours, string stockMaterialWaste, long createdBy, DateTime dateCreated, long lastAmendedBy,
                                    DateTime dateLastAmended)
        {
            string returnValue = "";
            try
            {
               
                returnValue = " UPDATE   un_stock_details" +
                "   SET   entity_id =  " + entityId + ",  " +
                " stock_reorder_level =  '" + stockReorderLevel + "',  " +
                " stock_reorder_amount =  '" + stockReorderAmount + "',  " +
                " stock_min_reorder_amount =  '" + stockMinReorderAmount + "',  " +
                " stock_quantity_avail =  '" + stockQuantityAvail + "',  " +
                " stock_quantity_on_order =  '" + stockQuantityOnOrder + "',  " +
                " stock_price_sale =  " + stockPriceSale + ",  " +
                " stock_shipping_weight =  '" + stockShippingWeight + "',  " +
                " stock_bin_location =  '" + stockBinLocation + "',  " +
                " stock_amt_labour =  " + stockAmtLabour + ",  " +
                " stock_labour_hours =  '" + stockLabourHours + "',  " +
                " stock_material_waste =  '" + stockMaterialWaste + "',  " +
                " created_by =  " + createdBy + ",  " +
                " date_created =  " + Utilities.getSQLDate(dateCreated) + ", " +
                " last_amended_by =  " + lastAmendedBy + ",  " +
                " date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " +
                " WHERE stock_code = " + stockCode;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string DecrementStockAvailQtyByStockCodeAndEntityId(string databaseType, string stockCode, long entityId, double stockQuantityAvail, 
                                                                          int lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"UPDATE un_stock_details
                        SET stock_quantity_avail = stock_quantity_avail - " + stockQuantityAvail + ", " +
                    "       last_amended_by = " + lastAmendedBy + ", " +
                    "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) +
                    " WHERE stock_code = '" + stockCode + "'" +
                    "   AND entity_id = " + entityId;
        }

        public static string Delete(string databaseType, long stockCode)
        {
            string returnValue = "";
            try
            {
                returnValue = " DELETE FROM   un_stock_details" +
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
                returnValue = " UPDATE   un_stock_details" +
                                "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                                " WHERE stock_code = " + stockCode;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

