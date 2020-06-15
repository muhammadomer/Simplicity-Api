using System;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class TmpTimesheetQueries
    {

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {

                returnValue = " SELECT * " +
                    "  FROM    un_tmp_timesheet" +
                    " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, string impRef, int dataStatus, string uncWebSessionId, string rowEmployeeName,
                                    string rowDesc, string rowDesc2, string rowDesc3, DateTime? dateRowStartTime, DateTime? dateRowFinishTime, double rowTimeTotal,
                                    string rowPymtType, string rowNotes, DateTime? dateRowDate, string rowJobRef, bool flgJobRefValid, long? jobSequence,
                                    bool flgPayrollEntry, bool flgLessBreakTime, string rowAssetName, long entityId, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            return "INSERT INTO un_tmp_timesheet(imp_ref, data_status, unc_web_session_id, row_employee_name, row_desc, row_desc2, " +
                   "       row_desc3, date_row_start_time, date_row_finish_time, row_time_total, row_pymt_type, row_notes, " +
                   "       date_row_date, row_job_ref, flg_job_ref_valid, job_sequence, flg_payroll_entry, flg_less_break_time, row_asset_name, entity_id, " +
                   "       created_by,  date_created, last_amended_by, date_last_amended) " +
                   "VALUES('" + impRef + "', " + dataStatus + ", '" + uncWebSessionId + "', '" + rowEmployeeName + "', '" +
                   rowDesc + "', '" + rowDesc2 + "', '" + rowDesc3 + "', " + Utilities.GetDateTimeForDML(databaseType, dateRowStartTime, true, true) + ", " +
                   Utilities.GetDateTimeForDML(databaseType, dateRowFinishTime, true, true) + ", " + rowTimeTotal + ", '" + rowPymtType + "', '" + rowNotes + "', " +
                   Utilities.GetDateTimeForDML(databaseType, dateRowDate, true, true) + ", '" + rowJobRef + "', " + Utilities.GetBooleanForDML(databaseType, flgJobRefValid) + ", " + jobSequence + ", " +
                   Utilities.GetBooleanForDML(databaseType, flgPayrollEntry) + ", " + Utilities.GetBooleanForDML(databaseType, flgLessBreakTime) + ", '" + rowAssetName + "', " +
                   entityId + ", " + createdBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " +
                   lastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
        }

        public static string update(string databaseType, long sequence, string impRef, string dataStatus, string uncWebSessionId, string rowEmployeeName,
                                    string rowDesc, string rowDesc2, string rowDesc3, DateTime dateRowStartTime, DateTime dateRowFinishTime, string rowTimeTotal,
                                    string rowPymtType, string rowNotes, DateTime dateRowDate, string rowJobRef, bool flgJobRefValid, long jobSequence,
                                    bool flgPayrollEntry, long entityId, long createdBy, DateTime dateCreated, long lastAmendedBy, DateTime dateLastAmended)
        {
            string returnValue = "";
            try
            {

                returnValue = " UPDATE   un_tmp_timesheet " +
                    "   SET  imp_ref =  '" + impRef + "', " +
                    " data_status =  '" + dataStatus + "', " +
                    " unc_web_session_id =  '" + uncWebSessionId + "', " +
                    " row_employee_name =  '" + rowEmployeeName + "', " +
                    " row_desc =  '" + rowDesc + "', " +
                    " row_desc2 =  '" + rowDesc2 + "', " +
                    " row_desc3 =  '" + rowDesc3 + "', " +
                    " date_row_start_time =  " + Utilities.GetDateTimeForDML(databaseType, dateRowStartTime, true, true) + ", " +
                    " date_row_finish_time =  " + Utilities.GetDateTimeForDML(databaseType, dateRowFinishTime, true, true) + ", " +
                    " row_time_total =  '" + rowTimeTotal + "',  " +
                    " row_pymt_type =  '" + rowPymtType + "',  " +
                    " row_notes =  '" + rowNotes + "',  " +
                    " date_row_date =  " + Utilities.GetDateTimeForDML(databaseType, dateRowDate, true, true) + ", " +
                    " row_job_ref =  '" + rowJobRef + "',  " +
                    " flg_job_ref_valid = " + Utilities.GetBooleanForDML(databaseType, flgJobRefValid) + ",  " +
                    " job_sequence =  " + jobSequence + ",  " +
                    " flg_payroll_entry = " + Utilities.GetBooleanForDML(databaseType, flgPayrollEntry) + ",  " +
                    " entity_id =  " + entityId + ",  " +
                    " created_by =  " + createdBy + ",  " +
                    " date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " +
                    " last_amended_by =  " + lastAmendedBy + ",  " +
                    " date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ", " +
                    "   WHERE sequence = " + sequence;
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
                returnValue = " DELETE FROM   un_tmp_timesheet" +
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
                returnValue = " UPDATE un_tmp_timesheet SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg) + " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string CreateTimeSheet(string databaseType,
                                        DateTime? dateRowStartTime, DateTime? dateRowFinishTime,
                                        string rowPymtType, string rowNotes, string rowJobRef,
                                        bool flgJobRefValid, long? jobSequence,
                                        double deSequence, string startTimeLocation, string finishTimeLocation,
                                        long createdBy, DateTime? dateCreated,
                                        long lastAmendedBy, DateTime? dateLastAmended,
                                        double rowTimeTotal, long entityId, DateTime? datRowDate,
                                        long userId

            )
        {
            return " INSERT INTO un_tmp_timesheet ( " +
                " date_row_start_time , date_row_finish_time , " +
                " row_pymt_type , row_notes , row_job_ref , " +
                " flg_job_ref_valid, job_sequence, " +
                " de_sequence , start_time_location , finish_time_location ," +
                " created_by,  date_created, last_amended_by, date_last_amended ," +
                " row_time_total, entity_id, date_row_date,row_employee_name ) " +
                " VALUES ("
                + Utilities.GetDateTimeForDML(databaseType, dateRowStartTime, true, true) + " , " + Utilities.GetDateTimeForDML(databaseType, dateRowFinishTime, true, true) + " , "
                + " '" + rowPymtType + "' " + " , " + " '" + rowNotes + "'" + " , " + " '" + rowJobRef + "' " + " , "
                + Utilities.GetBooleanForDML(databaseType, flgJobRefValid) + " , " + jobSequence + " , "
                + deSequence + " , " + " '" + startTimeLocation + "'" + " , " + " '" + finishTimeLocation + "' " + " , "
                + createdBy + " , " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + " , "
                + lastAmendedBy + " , " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + " , "
                + rowTimeTotal + " , " + entityId + " , " + Utilities.GetDateValueForDML(databaseType, datRowDate) + " , "
                + "(SELECT edc.name_short FROM (un_user_details AS ud INNER JOIN un_diary_resources AS dr ON ud.resource_sequence = dr.sequence) INNER JOIN un_entity_details_core AS edc  ON dr.join_resource = edc.entity_id WHERE ud.user_id = " + userId + ") "
                + ")";
        }

        public static string GetAllTimeEntriesByDate(string databaseType, DateTime? appStartDate)
        {
            string returnValue = "";
            switch (databaseType)
            {
                case "MSACCESS":
                    returnValue = @"SELECT sequence,date_row_start_time,date_row_finish_time,row_pymt_type,row_notes,row_job_ref,
	                                    flg_job_ref_valid,job_sequence,de_sequence, start_time_location,finish_time_location,
	                                    created_by,date_created,last_amended_by,date_last_amended
                                    FROM un_tmp_timesheet 
                                    where date_row_start_time BETWEEN #" + Utilities.GetDateTimeForDML(databaseType, appStartDate, false, false) + " 00:00:00# " +
                                       " AND #" + Utilities.GetDateTimeForDML(databaseType, appStartDate, false, false) + " 23:59:59# " +
                                    " order by date_row_start_time desc";
                    break;
                case "SQLSERVER":
                default:
                    returnValue = @"SELECT sequence,date_row_start_time,date_row_finish_time,row_pymt_type,row_notes,row_job_ref,
	                                    flg_job_ref_valid,job_sequence,de_sequence, start_time_location,finish_time_location,
	                                    created_by,date_created,last_amended_by,date_last_amended
                                    FROM un_tmp_timesheet
                                    where date_row_start_time BETWEEN '" + Utilities.GetDateTimeForDML(databaseType, appStartDate, false, false) + " 00:00:00' " +
                                    "   AND '" + Utilities.GetDateTimeForDML(databaseType, appStartDate, false, false) + " 23:59:59' " +
                                    " order by date_row_start_time desc";
                    break;
            }
            return returnValue;
        }
    }
}

