using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineDAL.QueriesRepo
{
    public static class StockJobRequestQueries
    {

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
               
                returnValue = " SELECT * " +
                "  FROM    un_stock_job_request" +
                " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string Insert(string databaseType, long jobSequence, long joinSequence, string transType, long entityId, string stockCode,
                                          string stockUnit, string stockDesc, double stockQuantity, double stockAmountEst,
                                          DateTime? stockRequestedDate, DateTime? dateStockRequired, bool flgStockOrdered,
                                          DateTime? stockOrderedDate, bool flgStockReceived, DateTime? stockReceivedDate,
                                          bool flgSorDrillDown, string sorItemCode, int itemType, double itemHours,
                                          int createdBy, DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"INSERT INTO un_stock_job_request(job_sequence, join_sequence, trans_type, entity_id, stock_code, stock_unit, stock_desc,
                            stock_quantity, stock_amount_est, stock_requested_date, date_stock_required, flg_stock_ordered,
                            stock_ordered_date, flg_stock_received, stock_received_date, flg_sor_drill_down, sor_item_code, 
                            item_type, item_hours, created_by, date_created, last_amended_by, date_last_amended) 
                     VALUES (" + jobSequence + ", " + joinSequence + ", '" + transType + "', " + entityId + ", '" + stockCode + "', '" + 
                            stockUnit + "', '" + stockDesc + "', " + stockQuantity + ", " + stockAmountEst + ", " +
                            Utilities.GetDateTimeForDML(databaseType, stockRequestedDate, true, true) + ", " +
                            Utilities.GetDateTimeForDML(databaseType, dateStockRequired, true, true) + ", " +
                            Utilities.GetBooleanForDML(databaseType, flgStockOrdered) + ", " + 
                            Utilities.GetDateTimeForDML(databaseType, stockOrderedDate, true, true) + ", " +
                            Utilities.GetBooleanForDML(databaseType, flgStockReceived) + ", " +
                            Utilities.GetDateTimeForDML(databaseType, stockReceivedDate, true, true) + ", " +
                            Utilities.GetBooleanForDML(databaseType, flgSorDrillDown) + ", '" + sorItemCode + "', " + itemType + ", " +
                            itemHours + ", " + createdBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " + 
                            lastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
        }


        public static string Update(string databaseType, long sequence, long jobSequence, long joinSequence, string transType, long entityId, 
                                    string stockCode, string stockUnit, string stockDesc, double stockQuantity, double stockAmountEst,
                                    DateTime? stockRequestedDate, DateTime? dateStockRequired, bool flgStockOrdered, DateTime? stockOrderedDate, 
                                    bool flgStockReceived, DateTime? stockReceivedDate, bool flgSorDrillDown, string sorItemCode,
                                    int itemType, double itemHours, int createdBy, DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
               
                returnValue = " UPDATE   un_stock_job_request" +
                    "   SET   job_sequence =  " + jobSequence + ",  " +
                    " join_sequence =  " + joinSequence + ",  " +
                    " trans_type =  '" + transType + "',  " +
                    " entity_id =  " + entityId + ",  " +
                    " stock_code =  '" + stockCode + "',  " +
                    " stock_unit =  '" + stockUnit + "',  " +
                    " stock_desc =  '" + stockDesc + "',  " +
                    " stock_quantity =  '" + stockQuantity + "',  " +
                    " stock_amount_est =  " + stockAmountEst + ",  " +
                    " stock_requested_date =  " + Utilities.GetDateTimeForDML(databaseType, stockRequestedDate,true,true) + ",  " +
                    " date_stock_required =  " + Utilities.GetDateTimeForDML(databaseType, dateStockRequired,true,true) + ", " +
                    " flg_stock_ordered = " + Utilities.GetBooleanForDML(databaseType, flgStockOrdered) + ",  " +
                    " stock_ordered_date =  " + Utilities.GetDateTimeForDML(databaseType, stockOrderedDate,true,true) + ",  " +
                    " flg_stock_received = " + Utilities.GetBooleanForDML(databaseType, flgStockReceived) + ",  " +
                    " stock_received_date =  " + Utilities.GetDateTimeForDML(databaseType, stockReceivedDate,true,true) + ",  " +
                    " flg_sor_drill_down = " + Utilities.GetBooleanForDML(databaseType, flgSorDrillDown) + ",  " +
                    " sor_item_code =  '" + sorItemCode + "',  " +
                    " item_type =  " + itemType + ",  " +
                    " item_hours =  '" + itemHours + "',  " +
                    " created_by =  " + createdBy + ",  " +
                    " date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " +
                    " last_amended_by =  " + lastAmendedBy + ",  " +
                    " date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " +
                    "  WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string delete(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = " DELETE FROM   un_stock_job_request" +
                " WHERE sequence = " + sequence;
            }

            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string deleteFlagDeleted(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                bool flg = true;
                returnValue = " UPDATE   un_stock_job_request" +
                                "   SET flg_deleted =  " + flg + ", " +
                                " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

