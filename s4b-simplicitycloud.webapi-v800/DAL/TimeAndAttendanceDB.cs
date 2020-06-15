using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class TimeAndAttendanceDB : MainDB
    {

        public TimeAndAttendanceDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertRefOrderCheckList(out long sequence, bool flgDeleted, long listSequence, string checkDesc, bool flgCompulsory, bool flgOrdEnqDataCapture, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OrderCheckListQueries.insertRefOrderCheckList(this.DatabaseType, flgDeleted, listSequence, checkDesc, flgCompulsory, flgOrdEnqDataCapture, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public List<PRSnap365Revenue> GetPRSnap365Revenue(int teamId, int locationId, DateTime? entryDate1, DateTime? entryDate2)
        {
            const string METHOD_NAME = "TimeAndAttendanceDB.GetPRSnap365Revenue()";
            List<PRSnap365Revenue> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(TimeAndAttendanceQueries.SelectAllSnap365Revenue(this.DatabaseType, entryDate1, entryDate2, teamId, locationId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<PRSnap365Revenue>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadPRSnap365Revenue(dr));
                                }
                            }
                            else
                            {
                                ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While loading PR Snap 365 Revenue.", ex);
            }
            return returnValue;
        }

        public List<PRSnap365Budget> GetPRSnap365Budget(int teamId, int locationId, int entryYear)
        {
            const string METHOD_NAME = "TimeAndAttendanceDB.GetPRSnap365Budget()";
            List<PRSnap365Budget> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(TimeAndAttendanceQueries.SelectAllSnap365Budget(this.DatabaseType, entryYear, teamId, locationId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<PRSnap365Budget>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadPRSnap365Budget(dr));
                                }
                            }
                            else
                            {
                                ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting PR Snap 365 Budget.", ex);
            }
            return returnValue;
        }

        public List<PRRosterInOutSummary> GetPRRosterInOutSummary(DateTime? entryDate1, DateTime? entryDate2, long prReference, long sequence)
        {
            const string METHOD_NAME = "TimeAndAttendanceDB.GetPRRosterInOutSummary()";
            List<PRRosterInOutSummary> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(TimeAndAttendanceQueries.SelectPrRosterInOutSummary(this.DatabaseType, entryDate1, entryDate2, prReference, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<PRRosterInOutSummary>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadPRRosterInOutSummary(dr));
                                }
                            }
                            else
                            {
                                ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting PR Roster In Out Summary.", ex);
            }
            return returnValue;
        }

        public List<PRRosterInOutSummaryDD> GetPRRosterInOutSummaryDD(DateTime? entryDate1, DateTime? entryDate2, long prReference, long joinSequence)
        {
            const string METHOD_NAME = "TimeAndAttendanceDB.GetPRRosterInOutSummaryDD()";
            List<PRRosterInOutSummaryDD> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(TimeAndAttendanceQueries.SelectPrRosterInOutSummaryDD(this.DatabaseType, entryDate1, entryDate2, prReference, joinSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<PRRosterInOutSummaryDD>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadPRRosterInOutSummaryDD(dr));
                                }
                            }
                            else
                            {
                                ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting PR Roster In Out Summary DD.", ex);
            }
            return returnValue;
        }

        public PRSnap365Budget InsertBudget(PRSnap365Budget obj)
        {
            const string METHOD_NAME = "TimeAndAttendanceDB.InsertBudget()";
            PRSnap365Budget returnValue = obj;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(TimeAndAttendanceQueries.InsertBudget(this.DatabaseType, obj), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        returnValue.Sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Inserting PR Snap 365 Budget.", ex);
            }
            return returnValue;
        }

        public PRSnap365Budget UpdateBudget(PRSnap365Budget obj)
        {
            const string METHOD_NAME = "TimeAndAttendanceDB.UpdateBudget()";
            PRSnap365Budget returnValue = obj;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(TimeAndAttendanceQueries.UpdateBudget(this.DatabaseType, obj), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Updating PR Snap 365 Budget.", ex);
            }
            return returnValue;
        }

        public PRSnap365Revenue UpdateRevenue(PRSnap365Revenue obj)
        {
            const string METHOD_NAME = "TimeAndAttendanceDB.UpdateRevenue()";
            PRSnap365Revenue returnValue = obj;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(TimeAndAttendanceQueries.UpdateRevenue(this.DatabaseType, obj), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Updating PR Snap 365 Revenue.", ex);
            }
            return returnValue;
        }

        public PRSnap365Revenue InsertRevenue(PRSnap365Revenue obj)
        {
            const string METHOD_NAME = "TimeAndAttendanceDB.InsertRevenue()";
            PRSnap365Revenue returnValue = obj;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(TimeAndAttendanceQueries.InsertRevenue(this.DatabaseType, obj), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        returnValue.Sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Inserting PR Snap 365 Revenue.", ex);
            }
            return returnValue;
        }

        public OrderCheckListItems updateOrderCheckListItem(OrderCheckListItems orderCheckListItem)
        {
            OrderCheckListItems returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrderCheckListQueries.updateOrderCheckListItem(this.DatabaseType, orderCheckListItem), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = orderCheckListItem;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool updateBySequence(long sequence, bool flgDeleted, long listSequence, string checkDesc, bool flgCompulsory, bool flgOrdEnqDataCapture, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrderCheckListQueries.updateRefOrderCheckList(this.DatabaseType, sequence, flgDeleted, listSequence, checkDesc, flgCompulsory, flgOrdEnqDataCapture, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool deleteBySequence(long sequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrderCheckListQueries.deleteRefOrderCheckList(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool DeleteByFlgDeleted(long sequence)
        {
            const string METHOD_NAME = "TimeAndAttendanceDB.DeleteByFlgDeleted()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(OrderCheckListQueries.updateRefOrderCheckListFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While loading PR Snap 365 Budget.", ex);
            }
            return returnValue;
        }

        private PRSnap365Budget LoadPRSnap365Budget(OleDbDataReader dr)
        {
            const string METHOD_NAME = "TimeAndAttendanceDB.LoadPRSnap365Budget()";
            PRSnap365Budget pRSnap365Budget = null;
            try
            {
                if (dr != null)
                {
                    pRSnap365Budget = new PRSnap365Budget();
                    pRSnap365Budget.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    pRSnap365Budget.FlgDeleted = DBUtil.GetBooleanValue(dr, "flg_deleted");
                    pRSnap365Budget.LocationId = DBUtil.GetIntValue(dr, "location_id");
                    pRSnap365Budget.TeamId = DBUtil.GetIntValue(dr, "team_id");
                    pRSnap365Budget.Year = DBUtil.GetIntValue(dr, "entry_year");
                    pRSnap365Budget.Value = DBUtil.GetDoubleValue(dr, "entry_value");
                    pRSnap365Budget.CreatedBy = Int32.Parse(dr["created_by"].ToString());
                    pRSnap365Budget.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    pRSnap365Budget.LastAmendedBy = Int32.Parse(dr["last_amended_by"].ToString());
                    pRSnap365Budget.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While loading PR Snap 365 Budget.", ex);
            }
            return pRSnap365Budget;
        }

        private PRSnap365Revenue LoadPRSnap365Revenue(OleDbDataReader dr)
        {
            const string METHOD_NAME = "TimeAndAttendanceDB.LoadPRSnap365Revenue()";
            PRSnap365Revenue pRSnap365Revenue = null;
            try
            {
                if (dr != null)
                {
                    pRSnap365Revenue = new PRSnap365Revenue();
                    pRSnap365Revenue.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    pRSnap365Revenue.FlgDeleted = DBUtil.GetBooleanValue(dr, "flg_deleted");
                    pRSnap365Revenue.LocationId = DBUtil.GetIntValue(dr, "location_id");
                    pRSnap365Revenue.TeamId = DBUtil.GetIntValue(dr, "team_id");
                    pRSnap365Revenue.EntryDate = DBUtil.GetDateTimeValue(dr, "date_entry_date");
                    pRSnap365Revenue.EntryValue = DBUtil.GetDoubleValue(dr, "entry_value");
                    pRSnap365Revenue.CreatedBy = Int32.Parse(dr["created_by"].ToString());
                    pRSnap365Revenue.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    pRSnap365Revenue.LastAmendedBy = Int32.Parse(dr["last_amended_by"].ToString());
                    pRSnap365Revenue.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While loading PR Snap 365 Revenue.", ex);
            }
            return pRSnap365Revenue;
        }

        private PRRosterInOutSummary LoadPRRosterInOutSummary(OleDbDataReader dr)
        {
            const string METHOD_NAME = "TimeAndAttendanceDB.LoadPRRosterInOutSummary()";
            PRRosterInOutSummary pRRosterInOutSummary = null;
            try
            {
                if (dr != null)
                {
                    pRRosterInOutSummary = new PRRosterInOutSummary();
                    pRRosterInOutSummary.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    pRRosterInOutSummary.deviceId = DBUtil.GetIntValue(dr, "device_id");
                    pRRosterInOutSummary.PrReference = DBUtil.GetLongValue(dr, "pr_reference");
                    pRRosterInOutSummary.DateWorked = DBUtil.GetDateTimeValue(dr, "date_worked");
                    pRRosterInOutSummary.FlgHoursIssueIn = DBUtil.GetBooleanValue(dr, "flg_hours_issue_in");
                    pRRosterInOutSummary.FlgHoursIssueOut = DBUtil.GetBooleanValue(dr, "flg_hours_issue_out");
                    pRRosterInOutSummary.HoursWorked = DBUtil.GetDoubleValue(dr, "hours_worked");
                    pRRosterInOutSummary.FlgBreakIssueBreak = DBUtil.GetBooleanValue(dr, "flg_break_issue_break");
                    pRRosterInOutSummary.FlgBreakIssueReturn = DBUtil.GetBooleanValue(dr, "flg_break_issue_return");
                    pRRosterInOutSummary.HoursBreak = DBUtil.GetDoubleValue(dr, "hours_break");
                    pRRosterInOutSummary.CreatedBy = Int32.Parse(dr["created_by"].ToString());
                    pRRosterInOutSummary.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    pRRosterInOutSummary.LastAmendedBy = Int32.Parse(dr["last_amended_by"].ToString());
                    pRRosterInOutSummary.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While loading PR Roster In out Summary.", ex);
            }
            return pRRosterInOutSummary;
        }

        private PRRosterInOutSummaryDD LoadPRRosterInOutSummaryDD(OleDbDataReader dr)
        {
            const string METHOD_NAME = "TimeAndAttendanceDB.LoadPRRosterInOutSummaryDD()";
            PRRosterInOutSummaryDD pRRosterInOutSummaryDD = null;
            try
            {
                if (dr != null)
                {
                    pRRosterInOutSummaryDD = new PRRosterInOutSummaryDD();
                    pRRosterInOutSummaryDD.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    pRRosterInOutSummaryDD.JoinSequence = DBUtil.GetLongValue(dr, "join_sequence");
                    pRRosterInOutSummaryDD.PRPayElement = DBUtil.GetLongValue(dr, "pr_pay_element");
                    pRRosterInOutSummaryDD.PrReference = DBUtil.GetLongValue(dr, "pr_reference");
                    pRRosterInOutSummaryDD.ElementHours = DBUtil.GetDoubleValue(dr, "element_hours");
                    pRRosterInOutSummaryDD.ElementRate = DBUtil.GetDoubleValue(dr, "element_rate");
                    pRRosterInOutSummaryDD.ElementAmount = DBUtil.GetDoubleValue(dr, "element_amount");
                    pRRosterInOutSummaryDD.CreatedBy = Int32.Parse(dr["created_by"].ToString());
                    pRRosterInOutSummaryDD.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    pRRosterInOutSummaryDD.LastAmendedBy = Int32.Parse(dr["last_amended_by"].ToString());
                    pRRosterInOutSummaryDD.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While loading PR Roster In out Summary DD.", ex);
            }
            return pRRosterInOutSummaryDD;
        }
    }
}