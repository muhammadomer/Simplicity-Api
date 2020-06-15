using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrderCheckListQueries
    {
        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:

                        returnValue = " SELECT * " +
                                      "  FROM    un_ref_order_check_list" +
                                      " WHERE sequence = " + Sequence;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByJobSequence(string databaseType, long jobSequence)
        {
            string returnValue = @"SELECT rocl.sequence AS refsequence, rocl.list_sequence, rocl.check_desc, 
                                   rocl.flg_compulsory, rocl.flg_ord_enq_data_capture,
                                   ocli.join_sequence, 
                                   ocli.sequence AS item_sequence, ocli.job_sequence, ocli.flg_checked, ocli.flg_checked_yes, 
                                   ocli.flg_checked_no, ocli.flg_checked_date, ocli.date_check, ocli.check_details
                                   FROM un_ref_order_check_list AS rocl
                                   LEFT JOIN un_order_check_list_items AS ocli
                                   ON rocl.sequence = ocli.check_sequence
                                   WHERE ocli.job_sequence = " + jobSequence +
                                  " AND rocl.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) +
                                 @" UNION
                                   SELECT rocl.sequence AS refsequence, rocl.list_sequence, rocl.check_desc,
                                   rocl.flg_compulsory, rocl.flg_ord_enq_data_capture, - 1 AS join_sequence,
                                   -1 AS item_sequence, " + jobSequence + @" AS job_sequence, 0 as flg_checked, 
                                   0 AS flg_checked_yes, 0 AS flg_checked_no, 0 AS flg_checked_date, 
                                   NULL AS date_check, '' AS check_details
                                   FROM un_ref_order_check_list AS rocl
                                   WHERE rocl.flg_deleted<> " + Utilities.GetBooleanForDML(databaseType, true) +
                                 @" AND rocl.sequence NOT IN(SELECT ocli.check_sequence
                                 FROM un_order_check_list_items AS ocli
                                 WHERE ocli.job_sequence = " +  jobSequence + ")" +
                                 @" ORDER BY rocl.list_sequence";
            return returnValue;
        }

        public static string insertRefOrderCheckList(string databaseType, bool flgDeleted, long listSequence, string checkDesc, bool flgCompulsory, bool flgOrdEnqDataCapture, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = "INSERT INTO   un_ref_order_check_list(flg_deleted,  list_sequence,  check_desc,  flg_compulsory,  flg_ord_enq_data_capture,  created_by,  date_created,  last_amended_by,  date_last_amended)" +
                                      "VALUES ( " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",  " + listSequence + ",   '" + checkDesc + "',   " 
                                      + Utilities.GetBooleanForDML(databaseType, flgCompulsory) + ",   " + Utilities.GetBooleanForDML(databaseType, flgOrdEnqDataCapture) 
                                      + ",  " + createdBy + ",   " + Utilities.GetDateTimeForDML(databaseType,dateCreated,true,true) + ",  " +
                                      lastAmendedBy + ",   " + Utilities.GetDateTimeForDML(databaseType,dateLastAmended,true,true) + ")";
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string updateRefOrderCheckList(string databaseType, long sequence, bool flgDeleted, long listSequence, string checkDesc, bool flgCompulsory, bool flgOrdEnqDataCapture, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = " UPDATE   un_ref_order_check_list" +
                                      "   SET  flg_deleted = " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",  " +
                                      " list_sequence =  " + listSequence + ",  " +
                                      " check_desc =  '" + checkDesc + "',  " +
                                      " flg_compulsory = " + Utilities.GetBooleanForDML(databaseType, flgCompulsory) + ",  " +
                                      " flg_ord_enq_data_capture = " + Utilities.GetBooleanForDML(databaseType, flgOrdEnqDataCapture) + ",  " +
                                      " created_by =  " + createdBy + ",  " +
                                      " date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " +
                                      " last_amended_by =  " + lastAmendedBy + ",  " +
                                      " date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " +
                                      " WHERE sequence = " + sequence;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string insertOrderCheckListMain(string databaseType, OrderCheckListMain orderCheckListMain)
        {
            return "INSERT INTO  un_order_check_list(job_sequence, flg_cancel_check_list, created_by, date_created, last_amended_by,  " +
                   "       date_last_amended) " +
                   "VALUES( " + orderCheckListMain.JobSequence + ", " + Utilities.GetBooleanForDML(databaseType, orderCheckListMain.FlgCancelCheckList) + ", " +
                   "      " + orderCheckListMain.CreatedBy + ", " + Utilities.GetDateTimeForDML(databaseType, orderCheckListMain.DateCreated, true, true) + ", " +
                   "      " + orderCheckListMain.LastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, orderCheckListMain.DateLastAmended, true, true) + ")";
        }

        internal static string insertOrderCheckListItem(string databaseType, OrderCheckListItems orderCheckListItem)
        {
            return "INSERT INTO  un_order_check_list_items(join_sequence, job_sequence, check_sequence, flg_checked, flg_checked_yes, " +
                   "       flg_checked_no, flg_checked_date, date_check, check_details, created_by, date_created, last_amended_by,  " +
                   "       date_last_amended) " +
                   "VALUES( " + orderCheckListItem.JoinSequence + ", " + orderCheckListItem.JobSequence + ", " + 
                   "      " + orderCheckListItem.CheckSequence + ", " +  Utilities.GetBooleanForDML(databaseType, orderCheckListItem.FlgChecked) + ", " +
                   "      " + Utilities.GetBooleanForDML(databaseType, orderCheckListItem.FlgCheckedYes) + ", " + Utilities.GetBooleanForDML(databaseType, orderCheckListItem.FlgCheckedNo) + ", " +
                   "      " + Utilities.GetBooleanForDML(databaseType, orderCheckListItem.FlgCheckedDate) + ", " + Utilities.GetDateTimeForDML(databaseType, orderCheckListItem.CheckedDate, true, true) + ", " +
                   "     '" + orderCheckListItem.CheckedDetails + "', " + 
                   "      " + orderCheckListItem.CreatedBy + ", " + Utilities.GetDateTimeForDML(databaseType, orderCheckListItem.DateCreated, true, true) + ", " +
                   "      " + orderCheckListItem.LastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, orderCheckListItem.DateLastAmended, true, true) + ")";
        }

        public static string deleteRefOrderCheckList(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = " DELETE FROM   un_ref_order_check_list" +
                                      " WHERE sequence = " + sequence;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string updateOrderCheckListItem(string databaseType, OrderCheckListItems orderCheckListItem)
        {
            return "UPDATE un_order_check_list_items "+
                   "   SET join_sequence = " + orderCheckListItem.JoinSequence + ", " +
                   "       job_sequence = " + orderCheckListItem.JobSequence + ", " +
                   "       check_sequence = " + orderCheckListItem.CheckSequence + ", " +
                   "       flg_checked = " + Utilities.GetBooleanForDML(databaseType, orderCheckListItem.FlgChecked) + ", " +
                   "       flg_checked_yes = " + Utilities.GetBooleanForDML(databaseType, orderCheckListItem.FlgCheckedYes) + ", " +
                   "       flg_checked_no = " + Utilities.GetBooleanForDML(databaseType, orderCheckListItem.FlgCheckedNo) + ", " +
                   "       flg_checked_date = " + Utilities.GetBooleanForDML(databaseType, orderCheckListItem.FlgCheckedDate) + ", " +
                   "       date_check = " + Utilities.GetDateTimeForDML(databaseType, orderCheckListItem.CheckedDate, true, true) + ", " +
                   "       check_details = '" + orderCheckListItem.CheckedDetails + "', " +
                   "       last_amended_by = " + orderCheckListItem.JoinSequence + ", " +
                   "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, orderCheckListItem.DateLastAmended, true, true)  +
                   " WHERE sequence = " + orderCheckListItem.Sequence;
        }

        public static string updateRefOrderCheckListFlagDeleted(string databaseType, long sequence)
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
                        returnValue = " UPDATE   un_ref_order_check_list" +
                                      "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg) + ", " +
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

