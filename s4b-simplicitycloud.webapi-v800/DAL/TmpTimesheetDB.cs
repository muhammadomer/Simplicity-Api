using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class TmpTimesheetDB : MainDB
    {

        public TmpTimesheetDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertTmpTimesheet(out long sequence, string impRef, int dataStatus, string uncWebSessionId, string rowEmployeeName, string rowDesc,
                                       string rowDesc2, string rowDesc3, DateTime? dateRowStartTime, DateTime? dateRowFinishTime, double rowTimeTotal, string rowPymtType,
                                       string rowNotes, DateTime? dateRowDate, string rowJobRef, bool flgJobRefValid, long? jobSequence, bool flgPayrollEntry,
                                       long entityId, bool flgLessBreakTime, string rowAssetName, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(TmpTimesheetQueries.insert(this.DatabaseType, impRef, dataStatus, uncWebSessionId, rowEmployeeName, rowDesc,
                                                                    rowDesc2, rowDesc3, dateRowStartTime, dateRowFinishTime, rowTimeTotal, rowPymtType, rowNotes,
                                                                    dateRowDate, rowJobRef, flgJobRefValid, jobSequence, flgPayrollEntry, flgLessBreakTime,
                                                                    rowAssetName, entityId, createdBy,
                                                                    dateCreated, lastAmendedBy, dateLastAmended), conn))
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

        public bool CreateTmpTimesheet(out long sequence,
                                        DateTime? dateRowStartTime, DateTime? dateRowFinishTime,
                                        string rowPymtType, string rowNotes, string rowJobRef,
                                        bool flgJobRefValid, long? jobSequence,
                                        double deSequence, string startTimeLocation, string finishTimeLocation,
                                        long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended,
                                        double rowTimeTotal, long entityId, DateTime? datRowDate, long userId)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(TmpTimesheetQueries.CreateTimeSheet(this.DatabaseType,
                                                                    dateRowStartTime, dateRowFinishTime,
                                                                    rowPymtType, rowNotes, rowJobRef,
                                                                    flgJobRefValid, jobSequence,
                                                                    deSequence, startTimeLocation, finishTimeLocation,
                                                                    createdBy, dateCreated, lastAmendedBy, dateLastAmended,
                                                                    rowTimeTotal, entityId, datRowDate, userId), conn))
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

        public List<TmpTimesheet> selectAllTmpTimesheetSequence(long sequence)
        {
            List<TmpTimesheet> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(TmpTimesheetQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<TmpTimesheet>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_TmpTimesheet(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public List<AppointmentTimeEntries> GetAllTimeEntriesByDate(DateTime? appStartDate)
        {
            List<AppointmentTimeEntries> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect = new OleDbCommand(TmpTimesheetQueries.GetAllTimeEntriesByDate(this.DatabaseType, appStartDate), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<AppointmentTimeEntries>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadTimeEntry(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }


        public bool updateBySequence(long sequence, string impRef, string dataStatus, string uncWebSessionId, string rowEmployeeName, string rowDesc,
                                      string rowDesc2, string rowDesc3, DateTime dateRowStartTime, DateTime dateRowFinishTime, string rowTimeTotal, string rowPymtType,
                                      string rowNotes, DateTime dateRowDate, string rowJobRef, bool flgJobRefValid, long jobSequence, bool flgPayrollEntry,
                                      long entityId, long createdBy, DateTime dateCreated, long lastAmendedBy, DateTime dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(TmpTimesheetQueries.update(this.DatabaseType, sequence, impRef, dataStatus, uncWebSessionId, rowEmployeeName, rowDesc,
                                                                    rowDesc2, rowDesc3, dateRowStartTime, dateRowFinishTime, rowTimeTotal, rowPymtType, rowNotes,
                                                                    dateRowDate, rowJobRef, flgJobRefValid, jobSequence, flgPayrollEntry, entityId, createdBy,
                                                                    dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
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
                        new OleDbCommand(TmpTimesheetQueries.delete(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public bool deleteByFlgDeleted(long sequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(TmpTimesheetQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        private TmpTimesheet Load_TmpTimesheet(OleDbDataReader dr)

        {
            TmpTimesheet tmpTimesheet = null;
            try
            {
                if (dr != null)
                {
                    tmpTimesheet = new TmpTimesheet();
                    tmpTimesheet.Sequence = long.Parse(dr["sequence"].ToString());
                    tmpTimesheet.ImpRef = Utilities.GetDBString(dr["imp_ref"]);
                    tmpTimesheet.DataStatus = int.Parse(dr["data_status"].ToString());
                    tmpTimesheet.UncWebSessionId = Utilities.GetDBString(dr["unc_web_session_id"]);
                    tmpTimesheet.RowEmployeeName = Utilities.GetDBString(dr["row_employee_name"]);
                    tmpTimesheet.RowDesc = Utilities.GetDBString(dr["row_desc"]);
                    tmpTimesheet.RowDesc2 = Utilities.GetDBString(dr["row_desc2"]);
                    tmpTimesheet.RowDesc3 = Utilities.GetDBString(dr["row_desc3"]);
                    tmpTimesheet.DateRowStartTime = Utilities.getDBDate(dr["date_row_start_time"]);
                    tmpTimesheet.DateRowFinishTime = Utilities.getDBDate(DateTime.Parse(dr["date_row_finish_time"].ToString()));
                    tmpTimesheet.RowTimeTotal = double.Parse(dr["row_time_total"].ToString());
                    tmpTimesheet.RowPymtType = Utilities.GetDBString(dr["row_pymt_type"]);
                    tmpTimesheet.RowNotes = Utilities.GetDBString(dr["row_notes"]);
                    tmpTimesheet.DateRowDate = Utilities.getDBDate(dr["date_row_date"]);
                    tmpTimesheet.RowJobRef = Utilities.GetDBString(dr["row_job_ref"]);
                    tmpTimesheet.FlgJobRefValid = bool.Parse(dr["flg_job_ref_valid"].ToString());
                    tmpTimesheet.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    tmpTimesheet.FlgPayrollEntry = bool.Parse(dr["flg_payroll_entry"].ToString());
                    tmpTimesheet.EntityId = long.Parse(dr["entity_id"].ToString());
                    tmpTimesheet.CreatedBy = long.Parse(dr["created_by"].ToString());
                    tmpTimesheet.DateCreated = Utilities.getDBDate(dr["date_created"]);
                    tmpTimesheet.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    tmpTimesheet.DateLastAmended = Utilities.getDBDate(dr["date_last_amended"]);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return tmpTimesheet;
        }

        private AppointmentTimeEntries LoadTimeEntry(OleDbDataReader dr)
        {
            AppointmentTimeEntries tmpTimesheet = null;
            try
            {
                if (dr != null)
                {
                    tmpTimesheet = new AppointmentTimeEntries();
                    tmpTimesheet.Sequence = long.Parse(dr["sequence"].ToString());
                    tmpTimesheet.DateRowStartTime = Utilities.getDBDate(dr["date_row_start_time"]);
                    tmpTimesheet.DateRowFinishTime = Utilities.getDBDate(DateTime.Parse(dr["date_row_finish_time"].ToString()));                    
                    tmpTimesheet.RowPymtType = Utilities.GetDBString(dr["row_pymt_type"]);
                    tmpTimesheet.RowNotes = Utilities.GetDBString(dr["row_notes"]);                    
                    tmpTimesheet.RowJobRef = Utilities.GetDBString(dr["row_job_ref"]);
                    tmpTimesheet.FlgJobRefValid = bool.Parse(dr["flg_job_ref_valid"].ToString());
                    tmpTimesheet.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    tmpTimesheet.DeSequence = long.Parse(dr["de_sequence"].ToString());
                    tmpTimesheet.StartTimeLocation = Utilities.GetDBString(dr["start_time_location"]);
                    tmpTimesheet.FinishTimeLocation = Utilities.GetDBString(dr["finish_time_location"]);
                    tmpTimesheet.CreatedBy = long.Parse(dr["created_by"].ToString());
                    tmpTimesheet.DateCreated = Utilities.getDBDate(dr["date_created"]);
                    tmpTimesheet.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    tmpTimesheet.DateLastAmended = Utilities.getDBDate(dr["date_last_amended"]);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message + " " + ex.InnerException;
            }
            return tmpTimesheet;
        }
    }
}