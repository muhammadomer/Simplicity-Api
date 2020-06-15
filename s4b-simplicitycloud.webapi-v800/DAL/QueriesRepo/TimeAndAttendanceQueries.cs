using System;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class TimeAndAttendanceQueries
    {
        public static string SelectAllSnap365Budget(string databaseType, int entryYear, int teamId, int locationId)
        {
            string returnValue = @"SELECT * FROM un_pr_snap_365_budget WHERE flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
            if(entryYear>0)
            {
                returnValue = returnValue + " AND entry_year = " + entryYear;
            }
            if (teamId > 0)
            {
                returnValue = returnValue + " AND team_id = " + teamId;
            }
            if (locationId > 0)
            {
                returnValue = returnValue + " AND location_id = " + locationId;
            }
            return returnValue;
        }

        public static string SelectAllSnap365Revenue(string databaseType, DateTime? entryDate1, DateTime? entryDate2, int teamId, int locationId)
        {
            string returnValue = @"SELECT * FROM un_pr_snap_365_revenue WHERE flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
            if (teamId > 0)
            {
                returnValue = returnValue + " AND team_id = " + teamId;
            }
            if (locationId > 0)
            {
                returnValue = returnValue + " AND location_id = " + locationId;
            }
            if(entryDate1 !=null && entryDate2 !=null)
            {
                returnValue = returnValue + " AND date_entry_date >= " + Utilities.GetDateTimeForDML(databaseType, entryDate1, true, false) +
                                            " AND date_entry_date < " + Utilities.GetDateTimeForDML(databaseType, entryDate2.Value.AddDays(1), true, false);
            }
            else if (entryDate1 != null)
            {
                returnValue = returnValue + " AND date_entry_date >= " + Utilities.GetDateTimeForDML(databaseType, entryDate1, true, false) +
                                            " AND date_entry_date < " + Utilities.GetDateTimeForDML(databaseType, entryDate1.Value.AddDays(1), true, false);
            }
            return returnValue;
        }

        public static string SelectPrRosterInOutSummary(string databaseType, DateTime? entryDate1, DateTime? entryDate2, long prReference, long sequence)
        {
            string returnValue = @"SELECT * FROM un_pr_roster_in_out_summary WHERE pr_reference > 0 ";
            if (sequence > 0)
            {
                returnValue = returnValue + " AND sequence = " + sequence;
            }
            if (prReference > 0)
            {
                returnValue = returnValue + " AND pr_reference = " + prReference;
            }
            if (entryDate1 != null && entryDate2 != null)
            {
                returnValue = returnValue + " AND date_worked >= " + Utilities.GetDateTimeForDML(databaseType, entryDate1, true, false) +
                                            " AND date_worked < " + Utilities.GetDateTimeForDML(databaseType, entryDate2.Value.AddDays(1), true, false);
            }
            else if (entryDate1 != null)
            {
                returnValue = returnValue + " AND date_worked >= " + Utilities.GetDateTimeForDML(databaseType, entryDate1, true, false) +
                                            " AND date_worked < " + Utilities.GetDateTimeForDML(databaseType, entryDate1.Value.AddDays(1), true, false);
            }
            return returnValue;
        }

        public static string SelectPrRosterInOutSummaryDD(string databaseType, DateTime? entryDate1, DateTime? entryDate2, long prReference, long joinSequence)
        {
            string returnValue = @"SELECT * FROM un_pr_roster_in_out_summary_dd WHERE pr_reference > 0 ";
            if (prReference > 0)
            {
                returnValue = returnValue + " AND pr_reference = " + prReference;
            }
            if (joinSequence > 0)
            {
                returnValue = returnValue + " AND join_sequence = " + joinSequence;
            }
            if (entryDate1 != null)
            {
                returnValue = returnValue + " AND join_sequence in ( select summary.sequence from un_pr_roster_in_out_summary AS summary WHERE summary.sequence > 0 ";
                if (entryDate1 != null && entryDate2 != null)
                {
                    returnValue = returnValue + " AND summary.date_worked >= " + Utilities.GetDateTimeForDML(databaseType, entryDate1, true, false) +
                                                " AND summary.date_worked < " + Utilities.GetDateTimeForDML(databaseType, entryDate2.Value.AddDays(1), true, false);
                }
                else
                {
                    returnValue = returnValue + " AND summary.date_worked >= " + Utilities.GetDateTimeForDML(databaseType, entryDate1, true, false) +
                                                " AND summary.date_worked < " + Utilities.GetDateTimeForDML(databaseType, entryDate1.Value.AddDays(1), true, false);
                }
                returnValue = returnValue + ")";
            }
            return returnValue;
        }

        public static string updateRefOrderCheckList(string databaseType, long sequence, bool flgDeleted, long listSequence, string checkDesc, bool flgCompulsory, bool flgOrdEnqDataCapture, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
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
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string InsertBudget(string databaseType, PRSnap365Budget obj)
        {
            return @"INSERT INTO un_pr_snap_365_budget(flg_deleted, team_id, location_id, entry_year, entry_value, created_by, date_created, 
                            last_amended_by,  date_last_amended)
                   VALUES( " + Utilities.GetBooleanForDML(databaseType, obj.FlgDeleted) + ", " + obj.TeamId + ", " + 
                   "      " + obj.LocationId + ", " + obj.Year + ", " + obj.Value + ", " +
                   "      " + obj.CreatedBy + ", " + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated, true, true) + ", " +
                   "      " + obj.LastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended, true, true) + ")";
        }

        internal static string InsertRevenue(string databaseType, PRSnap365Revenue obj)
        {
            return @"INSERT INTO un_pr_snap_365_revenue(flg_deleted, team_id, location_id, date_entry_date, entry_value, created_by, date_created, 
                            last_amended_by,  date_last_amended)
                   VALUES( " + Utilities.GetBooleanForDML(databaseType, obj.FlgDeleted) + ", " + obj.TeamId + ", " +
                   "      " + obj.LocationId + ", " + Utilities.GetDateTimeForDML(databaseType, obj.EntryDate, true, false) + ", " + obj.EntryValue + ", " +
                   "      " + obj.CreatedBy + ", " + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated, true, true) + ", " +
                   "      " + obj.LastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended, true, true) + ")";
        }

        public static string deleteRefOrderCheckList(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                
                returnValue = " DELETE FROM   un_ref_order_check_list" +
                    " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateBudget(string databaseType, PRSnap365Budget obj)
        {
            return "UPDATE un_pr_snap_365_budget " +
                   "   SET flg_deleted = " + Utilities.GetBooleanForDML(databaseType, obj.FlgDeleted) + ", " +
                   "       team_id = " + obj.TeamId + ", " +
                   "       location_id = " + obj.LocationId + ", " +
                   "       entry_year = " + obj.Year + ", " +
                   "       entry_value = " + obj.Value + ", " +
                   "       last_amended_by = " + obj.LastAmendedBy + ", " +
                   "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended, true, true) + " " +
                   " WHERE sequence = " + obj.Sequence;
        }

        internal static string UpdateRevenue(string databaseType, PRSnap365Revenue obj)
        {
            return "UPDATE un_pr_snap_365_revenue " +
                   "   SET flg_deleted = " + Utilities.GetBooleanForDML(databaseType, obj.FlgDeleted) + ", " +
                   "       team_id = " + obj.TeamId + ", " +
                   "       location_id = " + obj.LocationId + ", " +
                   "       date_entry_date = " + Utilities.GetDateTimeForDML(databaseType, obj.EntryDate, true, true) + ", " +
                   "       entry_value = " + obj.EntryValue + ", " +
                   "       last_amended_by = " + obj.LastAmendedBy + ", " +
                   "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended, true, true) + " " +
                   " WHERE sequence = " + obj.Sequence;
        }

        public static string updateRefOrderCheckListFlagDeleted(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
               
                bool flg = true;
                returnValue = " UPDATE   un_ref_order_check_list" +
                                "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                                " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

