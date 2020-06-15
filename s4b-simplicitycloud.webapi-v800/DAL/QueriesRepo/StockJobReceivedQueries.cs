using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineDAL.QueriesRepo
{
    public static class StockJobReceivedQueries
    {

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
                
                returnValue = " SELECT * " +
                "  FROM    un_stock_job_received" +
                " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string Insert(string databaseType, long requestSequence, string deliveryRef, string transType, long entityId, DateTime? stockRecievedDate,
                                    string stockCode, double stockQuantity, double stockAmount, bool flgFromStockroom, long jobSequence, int createdBy,
                                    DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"INSERT INTO un_stock_job_received(request_sequence, delivery_ref, trans_type, entity_id, stock_recieved_date,
                            stock_code, stock_quantity, stock_amount, flg_from_stockroom, job_sequence, created_by,
                            date_created, last_amended_by, date_last_amended)
                     VALUES (" + requestSequence + ", '" + deliveryRef + "', '" + transType + "', " + entityId + ", " + 
                     Utilities.GetDateTimeForDML(databaseType, stockRecievedDate, true, true) + ", '" + stockCode + "', " + 
                     stockQuantity + ", " + stockAmount + ",  " + Utilities.GetBooleanForDML(databaseType, flgFromStockroom) + ", " + 
                     jobSequence + ", " + createdBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " + 
                     lastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
        }

        public static string update(string databaseType, long sequence, long requestSequence, string deliveryRef, string transType, long entityId, DateTime stockRecievedDate,
                                    string stockCode, string stockQuantity, double stockAmount, bool flgFromStockroom, long jobSequence, long createdBy, DateTime dateCreated,
                                    long lastAmendedBy, DateTime dateLastAmended)
        {
            string returnValue = "";
            try
            {
               
                returnValue = " UPDATE   un_stock_job_received" +
                    "   SET  request_sequence =  " + requestSequence + ",  " +
                    " delivery_ref =  '" + deliveryRef + "',  " +
                    " trans_type =  '" + transType + "',  " +
                    " entity_id =  " + entityId + ",  " +
                    " stock_recieved_date =  " + Utilities.GetDateTimeForDML(databaseType, stockRecievedDate,true,true) + ",  " +
                    " stock_code =  '" + stockCode + "',  " +
                    " stock_quantity =  '" + stockQuantity + "',  " +
                    " stock_amount =  " + stockAmount + ",  " +
                    " flg_from_stockroom = " + Utilities.GetBooleanForDML(databaseType, flgFromStockroom) + ",  " +
                    " job_sequence =  " + jobSequence + ",  " +
                    " created_by =  " + createdBy + ",  " +
                    " date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " +
                    " last_amended_by =  " + lastAmendedBy + ",  " +
                    " date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " +
                    " WHERE sequence = " + sequence;
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
                switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:

                        returnValue = " DELETE FROM   un_stock_job_received" +
                                      " WHERE sequence = " + sequence;

                        break;
                }
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
                switch (databaseType)
                {

                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        bool flg = true;
                        returnValue = " UPDATE   un_stock_job_received" +
                                      "   SET flg_deleted =  " + flg + ", " +
                                      " WHERE sequence = " + sequence;

                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

