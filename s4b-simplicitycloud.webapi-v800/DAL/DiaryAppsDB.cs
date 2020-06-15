using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
namespace SimplicityOnlineWebApi.DAL
{
    public class DiaryAppsDB : MainDB
    {

        public DiaryAppsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public List<DiaryAppsMobileNo> getDiaryResourceMobileNo(long diaryAppSequence)
        {
            List<DiaryAppsMobileNo> returnValue = new List<DiaryAppsMobileNo>();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(DiaryAppsQueries.getDiaryResourceMobileNo(this.DatabaseType,diaryAppSequence), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(LoadMobileNo(row));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public List<DiaryAppsMobileNo> getDiaryClientMobileNo(long diaryAppSequence)
        {
            List<DiaryAppsMobileNo> returnValue = new List<DiaryAppsMobileNo>();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(DiaryAppsQueries.getDiaryClientMobileNo(this.DatabaseType, diaryAppSequence), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(LoadMobileNo(row));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public bool checkClientTimeslotDuplicate(DateTime? appStartDate, DateTime? appEndDate, long clientId, long resourceId, long sequence = 0)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsQueries.checkClientTimeSlotDuplicate(this.DatabaseType, appStartDate, appEndDate, clientId, resourceId, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while checking duplicate time slots " + ex.Message + " " + ex.InnerException;
                Utilities.WriteLog(ErrorMessage);
            }
            return returnValue;
        }
        public bool insertDiaryApps(out long sequence, string transType, long joinResource, bool flgUseClientId, long clientId, long jobSequence, long jobAddressId, bool flgBookingRequired,
                                     long resourceSequence, DateTime? dateAppStart, DateTime? dateAppEnd, bool flgAppAllDay, string appPostCode, string appSubject,
                                     string appLocation, bool flgAppReminder, string appReminderSound, long appReminderMins, string appNotes, string appCategory,
                                     string appAttachmentPath, bool flgOnlineMeeting, bool flgUnavailable, long repeatSequence, long multiResourceSequence, long appType,
                                     bool flgAppDeleted, bool flgAppCompleted, bool flgAppBroken, long appBrokenReason, bool flgNoAccess, bool flgAppConfirmed,
                                     DateTime? dateAppConfirmed, string appConfirmedBy, long certSequence, long visitStatus, bool flgAppFixed,
                                     bool flgPrint, long printUserId, long unscheduledDeSeq, long rateSequence, long createdBy, DateTime? dateCreated
                                     )
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    if (resourceSequence >= 0)
                    {
                        DatabaseInfo dbInfo = new DatabaseInfo();
                        dbInfo.DatabaseType = this.DatabaseType;
                        dbInfo.ConnectionString = this.connectionString;
                        List<DiaryResources> diaryResource = new DiaryResourcesDB(dbInfo).selectAllDiaryResourcesSequence(resourceSequence);
                        if (diaryResource.Count > 0)
                            joinResource = diaryResource[0].JoinResource;
                    }
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(DiaryAppsQueries.insert(this.DatabaseType, transType, joinResource, flgUseClientId, clientId, jobSequence, jobAddressId, flgBookingRequired,
                                                               resourceSequence, dateAppStart, dateAppEnd, flgAppAllDay, appPostCode, appSubject, appLocation,
                                                               flgAppReminder, appReminderSound, appReminderMins, appNotes, appCategory, appAttachmentPath,
                                                               flgOnlineMeeting, flgUnavailable, repeatSequence, multiResourceSequence, appType, flgAppDeleted,
                                                               flgAppCompleted, flgAppBroken, appBrokenReason, flgNoAccess, flgAppConfirmed, dateAppConfirmed,
                                                               appConfirmedBy, certSequence, visitStatus, flgAppFixed, flgPrint, printUserId, unscheduledDeSeq,
                                                               rateSequence, createdBy, dateCreated), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        string sql = "select @@IDENTITY";
                        using (OleDbCommand objCommand =
                            new OleDbCommand(sql, conn))
                        {
                            OleDbDataReader dr = objCommand.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                sequence = long.Parse(dr[0].ToString());
                            }
                            else
                            {
                                //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record.'" + METHOD_NAME + "'\n";
                            }
                        }
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }


		public List<DiaryApps> selectAllDiaryAppsByAppDate(DateTime? appStartDate,DateTime? appEndDate)
        {
            List<DiaryApps> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsQueries.getSelectAllByAppDate(this.DatabaseType, appStartDate,appEndDate), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<DiaryApps>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_DiaryApps(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<DiaryApps> selectAllDiaryAppsByAppDate(DateTime? appStartDate, DateTime? appEndDate,string jobRef)
        {
            List<DiaryApps> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsQueries.getSelectAllByAppDateAndJobRef(this.DatabaseType, appStartDate, appEndDate,jobRef), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<DiaryApps>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_DiaryApps(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<DiaryAppsHistory> selectAllDiaryAppsByJobSequence(long jobSequence)
        {
            List<DiaryAppsHistory> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsQueries.getSelectAllByJobSequence(this.DatabaseType, jobSequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<DiaryAppsHistory>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadDiaryAppsHistory(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<DiaryApps> selectAllDiaryAppsThirdPartyByEntityId(long entityId)
        {
            List<DiaryApps> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsQueries.getSelectAllThirdpartyByEntityId(this.DatabaseType, entityId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<DiaryApps>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_DiaryApps(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }


        public List<DiaryApps> getAppoinmentsByAppDateAndUserId(DateTime? appStartDate,int userId)
        {
            List<DiaryApps> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsQueries.getAppoinmentsByAppDateAndUserId(this.DatabaseType, appStartDate, userId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<DiaryApps>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_DiaryApps(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public DiaryApps selectAllDiaryAppsSequence(long sequence)
        {
            DiaryApps returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new DiaryApps();
                                dr.Read();
                                returnValue = Load_DiaryApps(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public long selectDiaryResourceSequenceByUserId(long userId)
        {
            long returnValue = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsQueries.getResourceSequenceByUserId(this.DatabaseType, userId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    returnValue = Convert.ToInt64(dr["entity_id"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured while getting resource sequence " + ex.Message + " " + ex.InnerException);

            }
            return returnValue;
        }
        public bool updateBySequence(long sequence, string transType, long joinResource, bool flgUseClientId, long clientId, long jobSequence, long jobAddressId, bool flgBookingRequired,
                                    long resourceSequence, DateTime? dateAppStart, DateTime? dateAppEnd, bool flgAppAllDay, string appPostCode, string appSubject,
                                    string appLocation, bool flgAppReminder, string appReminderSound, long appReminderMins, string appNotes, string appCategory,
                                    string appAttachmentPath, bool flgOnlineMeeting, bool flgUnavailable, long repeatSequence, long multiResourceSequence, long appType,
                                    bool flgAppDeleted, bool flgAppCompleted, bool flgAppBroken, long appBrokenReason, bool flgNoAccess, bool flgAppConfirmed,
                                    DateTime? dateAppConfirmed, string appConfirmedBy, long certSequence, long visitStatus, DateTime? visitVam, bool flgAppFixed,
                                    bool flgPrint, long printUserId, long unscheduledDeSeq, long rateSequence, 
                                    long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {

                    if (resourceSequence >= 0)
                    {
                        DatabaseInfo dbInfo = new DatabaseInfo();
                        dbInfo.DatabaseType = this.DatabaseType;
                        dbInfo.ConnectionString = this.connectionString;
                        List <DiaryResources> diaryResource = new DiaryResourcesDB(dbInfo).selectAllDiaryResourcesSequence(resourceSequence);
                        if(diaryResource.Count>0)
                            joinResource = diaryResource[0].JoinResource;
                    }
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(DiaryAppsQueries.update(this.DatabaseType, sequence, transType, joinResource, flgUseClientId, clientId, jobSequence, jobAddressId, flgBookingRequired,
                                                                 resourceSequence, dateAppStart, dateAppEnd, flgAppAllDay, appPostCode, appSubject, appLocation,
                                                                 flgAppReminder, appReminderSound, appReminderMins, appNotes, appCategory, appAttachmentPath,
                                                                 flgOnlineMeeting, flgUnavailable, repeatSequence, multiResourceSequence, appType, flgAppDeleted,
                                                                 flgAppCompleted, flgAppBroken, appBrokenReason, flgNoAccess, flgAppConfirmed, dateAppConfirmed,
                                                                 appConfirmedBy, certSequence, visitStatus, visitVam, flgAppFixed, flgPrint, printUserId, unscheduledDeSeq,
                                                                 rateSequence, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool updateVisitStatusBySequence(long sequence, int visitStatus, bool flgNoAccess, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(DiaryAppsQueries.updateVisitStatusBySequence(this.DatabaseType, sequence, visitStatus, flgNoAccess,
                                                                                      lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool UpdateDiaryAppGPSDetails(long sequence, DiaryAppsGPS obj, int lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "DiaryAppsRepository.UpdateDiaryAppGPSDetails()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(DiaryAppsQueries.UpdateDiaryAppGPSDetails(this.DatabaseType, sequence, obj,
                                                                                   lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Updating Diary App GPS details.", ex);
            }
            return returnValue;
        }

        public bool updateVisitStatusAndFlgCompletedBySequence(long sequence, int visitStatus, bool flgCompleted, long lastAmendedBy, 
                                                               DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(DiaryAppsQueries.updateVisitStatusAndFlgCompletedBySequence(this.DatabaseType, sequence, visitStatus, 
                                                                                                     flgCompleted,
                                                                                                     lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
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
                        new OleDbCommand(DiaryAppsQueries.delete(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
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
                        new OleDbCommand(DiaryAppsQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }
        private DiaryApps Load_DiaryApps(OleDbDataReader dr)
        {
            const string METHOD_NAME = "DiaryAppsDB.Load_DiaryApps()";
            DiaryApps diaryApps = null;
            try
            {
                if (dr != null)
                {
                    diaryApps = new DiaryApps();
                    diaryApps.Sequence = long.Parse(dr["sequence"].ToString());
                    diaryApps.TransType = Utilities.GetDBString(dr["trans_type"]);
                    diaryApps.JoinResource = long.Parse(dr["join_resource"].ToString());
                    diaryApps.FlgUseClientId = bool.Parse(dr["flg_use_client_id"].ToString());
                    diaryApps.ClientId = long.Parse(dr["client_id"].ToString());
                    diaryApps.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    diaryApps.JobAddressId = dr["job_address_id"] == DBNull.Value ? 0 : long.Parse(dr["job_address_id"].ToString());
                    diaryApps.FlgBookingRequired = dr["flg_booking_required"]==DBNull.Value?false: bool.Parse(dr["flg_booking_required"].ToString());
                    diaryApps.ResourceSequence = long.Parse(dr["resource_sequence"].ToString());
                    diaryApps.DateAppStart = DateTime.Parse(dr["date_app_start"].ToString());
                    diaryApps.DateAppEnd = DateTime.Parse(dr["date_app_end"].ToString());
                    diaryApps.FlgAppAllDay = bool.Parse(dr["flg_app_all_day"].ToString());
                    diaryApps.AppPostCode = Utilities.GetDBString(dr["app_post_code"]);
                    diaryApps.AppSubject = Utilities.GetDBString(dr["app_subject"]);
                    diaryApps.AppLocation = Utilities.GetDBString(dr["app_location"]);
                    diaryApps.FlgAppReminder = bool.Parse(dr["flg_app_reminder"].ToString());
                    diaryApps.AppReminderSound = Utilities.GetDBString(dr["app_reminder_sound"]);
                    diaryApps.AppReminderMins = dr["app_reminder_mins"] == DBNull.Value ? 0 : long.Parse(dr["app_reminder_mins"].ToString());
                    diaryApps.AppNotes = Utilities.GetDBString(dr["app_notes"]);
                    diaryApps.AppCategory = Utilities.GetDBString(dr["app_category"]);
                    diaryApps.AppAttachmentPath = Utilities.GetDBString(dr["app_attachment_path"]);
                    diaryApps.FlgOnlineMeeting = bool.Parse(dr["flg_online_meeting"].ToString());
                    diaryApps.FlgUnavailable = dr["flg_unavailable"]==DBNull.Value?false: bool.Parse(dr["flg_unavailable"].ToString());
                    diaryApps.RepeatSequence = dr["repeat_sequence"]==DBNull.Value?0: long.Parse(dr["repeat_sequence"].ToString());
                    diaryApps.MultiResourceSequence = dr["multi_resource_sequence"]==DBNull.Value?0: long.Parse(dr["multi_resource_sequence"].ToString());
                    diaryApps.AppType = dr["app_type"]==DBNull.Value?0: long.Parse(dr["app_type"].ToString());
                    diaryApps.FlgAppDeleted = bool.Parse(dr["flg_app_deleted"].ToString());
                    diaryApps.FlgAppCompleted = bool.Parse(dr["flg_app_completed"].ToString());
                    diaryApps.FlgAppBroken = bool.Parse(dr["flg_app_broken"].ToString());
                    diaryApps.AppBrokenReason = dr["app_broken_reason"]==DBNull.Value?0: long.Parse(dr["app_broken_reason"].ToString());
                    diaryApps.FlgNoAccess = bool.Parse(dr["flg_no_access"].ToString());
                    diaryApps.FlgAppConfirmed = bool.Parse(dr["flg_app_confirmed"].ToString());
                    if(dr["date_app_confirmed"] != DBNull.Value) diaryApps.DateAppConfirmed = DateTime.Parse(dr["date_app_confirmed"].ToString());
                    diaryApps.AppConfirmedBy = Utilities.GetDBString(dr["app_confirmed_by"]);
                    diaryApps.CertSequence = dr["cert_sequence"]==DBNull.Value?0: long.Parse(dr["cert_sequence"].ToString());
                    diaryApps.VisitStatus = dr["visit_status"]==DBNull.Value?0: long.Parse(dr["visit_status"].ToString());
                    diaryApps.VisitVam = Utilities.getDBDate(dr["visit_vam"]);
                    diaryApps.FlgAppFixed = bool.Parse(dr["flg_app_fixed"].ToString());
                    diaryApps.FlgPrint = bool.Parse(dr["flg_print"].ToString());
                    diaryApps.PrintUserId = dr["print_user_id"]==DBNull.Value?0: long.Parse(dr["print_user_id"].ToString());
                    diaryApps.UnscheduledDeSeq = dr["unscheduled_de_seq"]==DBNull.Value?0: long.Parse(dr["unscheduled_de_seq"].ToString());
                    diaryApps.RateSequence = dr["rate_sequence"]==DBNull.Value?0: long.Parse(dr["rate_sequence"].ToString());
                    diaryApps.CreatedBy = dr["created_by"]==DBNull.Value?0 :long.Parse(dr["created_by"].ToString());
                    if(dr["date_created"] != DBNull.Value) diaryApps.DateCreated =  DateTime.Parse(dr["date_created"].ToString());
                    diaryApps.LastAmendedBy = dr["last_amended_by"]==DBNull.Value?0: long.Parse(dr["last_amended_by"].ToString());
                    if (dr["date_last_amended"] != DBNull.Value) diaryApps.DateLastAmended = DateTime.Parse(dr["date_last_amended"].ToString());

                    diaryApps.Order = new Orders();
                    if (ColumnExists(dr, "job_ref")) diaryApps.Order.JobRef = dr["job_ref"].ToString();
                    if (ColumnExists(dr, "job_client_id")) diaryApps.Order.JobClientId = Int32.Parse(dr["job_client_id"].ToString());
                    if (ColumnExists(dr, "job_client_ref")) diaryApps.Order.JobClientRef = dr["job_client_ref"].ToString();
                    if (ColumnExists(dr, "job_client_name")) diaryApps.Order.JobClientName = dr["job_client_name"].ToString();
                    if (ColumnExists(dr, "job_cost_centre")) diaryApps.Order.JobCostCentre = dr["job_cost_centre"].ToString();
                    if (ColumnExists(dr, "job_trade_code")) diaryApps.Order.JobTradeCode = dr["job_trade_code"].ToString();
                    if (ColumnExists(dr, "job_desc")) diaryApps.Order.JobDesc = dr["job_desc"].ToString();
                    if (ColumnExists(dr, "job_address")) diaryApps.Order.JobAddress = dr["job_address"].ToString();
                    if (ColumnExists(dr, "status_desc")) diaryApps.Order.StatusDescription = dr["status_desc"].ToString();
                    if (ColumnExists(dr, "job_date") && dr["job_date"]!=DBNull.Value) diaryApps.Order.JobDate = Convert.ToDateTime(dr["job_date"]);
                    if (ColumnExists(dr, "job_date_due") && dr["job_date_due"]!=DBNull.Value) diaryApps.Order.JobDateDue = Convert.ToDateTime(dr["job_date_due"]);
                    if (ColumnExists(dr, "date_user1") && dr["date_user1"] != DBNull.Value) diaryApps.Order.DateUser1 = Convert.ToDateTime(dr["date_user1"]);
                    if (ColumnExists(dr, "user_name")) diaryApps.AssignToUserName = dr["user_name"].ToString();
                    if (ColumnExists(dr, "add_info")) diaryApps.AppNotes = dr["add_info"].ToString();
                    if (ColumnExists(dr, "job_priority_code")) diaryApps.Order.JobPriorityCode = dr["job_priority_code"].ToString();
                    if (ColumnExists(dr, "occupier_name")) diaryApps.Order.OccupierName = dr["occupier_name"].ToString();
                    if (ColumnExists(dr, "occupier_tel_home")) diaryApps.Order.OccupierTelHome = dr["occupier_tel_home"].ToString();
                    if (ColumnExists(dr, "occupier_tel_work")) diaryApps.Order.OccupierTelWork = dr["occupier_tel_work"].ToString();
                    if (ColumnExists(dr, "occupier_tel_work_ext")) diaryApps.Order.OccupierTelWorkExt = dr["occupier_tel_work_ext"].ToString();
                    if (ColumnExists(dr, "occupier_tel_mobile")) diaryApps.Order.OccupierTelMobile = dr["occupier_tel_mobile"].ToString();
                    if (ColumnExists(dr, "occupier_email")) diaryApps.Order.OccupierEmail = dr["occupier_email"].ToString();
                    
                    diaryApps.DiaryAppWebAssign = new DiaryAppsWebAssign();
                    if (ColumnExists(dr, "web_assign_sequence")) diaryApps.DiaryAppWebAssign.SequenceId = Convert.ToInt32(dr["web_assign_sequence"]);
                    if (ColumnExists(dr, "add_info")) diaryApps.DiaryAppWebAssign.AddInfo = dr["add_info"].ToString();
                    if (ColumnExists(dr, "delay_reason")) diaryApps.DiaryAppWebAssign.DelayReason = dr["delay_reason"].ToString();
                    if (ColumnExists(dr, "flg_complete")) diaryApps.DiaryAppWebAssign.FlgComplete = Convert.ToBoolean(dr["flg_complete"]);
                    if (ColumnExists(dr, "flg_delay")) diaryApps.DiaryAppWebAssign.FlgDelay = Convert.ToBoolean(dr["flg_delay"]);
                    if (ColumnExists(dr, "date_app_completed")) diaryApps.DiaryAppWebAssign.DateAppCompleted = Convert.ToDateTime(dr["date_app_completed"]);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Loading Diary Apps.", ex);
            }
            return diaryApps;
        }

        private DiaryAppsAssets LoadDiaryAppsAssets(OleDbDataReader dr)
        {
            const string METHOD_NAME = "DiaryAppsDB.LoadDiaryAppsAssets()";
            DiaryAppsAssets returnValue = null;
            try
            {
                if (dr != null)
                {
                    returnValue = new DiaryAppsAssets();
                    returnValue.AppSequence = DBUtil.GetLongValue(dr, "de_sequence");
                    returnValue.AssetSequence = DBUtil.GetLongValue(dr, "asset_sequence");
                    returnValue.DiaryAssetSequence = DBUtil.GetLongValue(dr, "sequence");
                    returnValue.ItemLocation = DBUtil.GetStringValue(dr, "item_location");
                    returnValue.ItemExtraInfo = DBUtil.GetStringValue(dr, "item_extra_info");
                    returnValue.ItemSerialRef = DBUtil.GetStringValue(dr, "item_serial_ref");
                    returnValue.ItemManufacturer = DBUtil.GetStringValue(dr, "item_manufacturer");
                    returnValue.ItemModel = DBUtil.GetStringValue(dr, "item_model");
                    returnValue.ItemUserField1 = DBUtil.GetStringValue(dr, "item_user_field1");
                    returnValue.AssetCategoryDetails = DBUtil.GetStringValue(dr, "asset_category_details");                    
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Loading Diary Apps Assets.", ex);
            }
            return returnValue;
        }

        private DiaryAppsHistory LoadDiaryAppsHistory(OleDbDataReader dr)
        {
            const string METHOD_NAME = "DiaryAppsDB.LoadDiaryAppsHistory()";
            DiaryAppsHistory returnValue = null;
            try
            {
                if (dr != null)
                {
                    returnValue = new DiaryAppsHistory();
                    returnValue.AppSequence = DBUtil.GetLongValue(dr, "sequence");
                    returnValue.AppCategory = DBUtil.GetStringValue(dr, "app_category");
                    returnValue.AppSubject = DBUtil.GetStringValue(dr, "app_subject");
                    returnValue.AppNotes = DBUtil.GetStringValue(dr, "app_notes");
                    returnValue.DateAppStart = DBUtil.GetDateTimeValue(dr, "date_app_start");
                    returnValue.DateAppEnd = DBUtil.GetDateTimeValue(dr, "date_app_end");
                    returnValue.ResourceName = DBUtil.GetStringValue(dr, "resource_name");
                    returnValue.JobClientName = DBUtil.GetStringValue(dr, "job_client_name");
                    returnValue.JobClientAddress = DBUtil.GetStringValue(dr, "job_address");
                    returnValue.JobDescription = DBUtil.GetStringValue(dr, "job_desc");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Loading Diary Appointments History.", ex);
            }
            return returnValue;
        }
        private DiaryAppsSmart LoadDiaryAppsSmart(OleDbDataReader dr)
        {
            const string METHOD_NAME = "DiaryAppsDB.LoadDiaryAppsSmart()";
            DiaryAppsSmart returnValue = null;
            try
            {
                if (dr != null)
                {
                    returnValue = new DiaryAppsSmart();
                    returnValue.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    returnValue.JobSequence = DBUtil.GetLongValue(dr, "job_sequence");
                    returnValue.AppNotes = DBUtil.GetStringValue(dr, "app_notes");
                    returnValue.DateAppStart = DBUtil.GetDateTimeValue(dr, "date_app_start");
                    returnValue.VisitStatus = DBUtil.GetLongValue(dr, "visit_status");
                    returnValue.DAGPS = new DiaryAppsGPS();
                    returnValue.DAGPS.DateUserStart = DBUtil.GetDateTimeValue(dr, "date_user_start");
                    returnValue.DAGPS.UserStartGPSLong = DBUtil.GetStringValue(dr, "user_start_gps_long");
                    returnValue.DAGPS.UserStartGPSLat = DBUtil.GetStringValue(dr, "user_start_gps_lat");
                    returnValue.DAGPS.DateUserEnd = DBUtil.GetDateTimeValue(dr, "date_user_end");
                    returnValue.DAGPS.UserEndGPSLong = DBUtil.GetStringValue(dr, "user_end_gps_long");
                    returnValue.DAGPS.UserEndGPSLat = DBUtil.GetStringValue(dr, "user_end_gps_lat");
                    
                    if (returnValue.JobSequence>0)
                    {
                        returnValue.Order = new OrdersSmart();
                        returnValue.Order.Sequence = DBUtil.GetLongValue(dr, "job_sequence");
                        returnValue.Order.JobRef = DBUtil.GetStringValue(dr, "job_ref");
                        returnValue.Order.JobClientName = DBUtil.GetStringValue(dr, "job_address");
                        returnValue.Order.JobAddress = DBUtil.GetStringValue(dr, "job_address");
                        returnValue.Order.OccupierName = DBUtil.GetStringValue(dr, "occupier_name");
                        returnValue.Order.OccupierTelHome = DBUtil.GetStringValue(dr, "occupier_tel_home");
                        returnValue.Order.OccupierTelMobile = DBUtil.GetStringValue(dr, "occupier_tel_mobile");
                        returnValue.Order.OccupierEmail = DBUtil.GetStringValue(dr, "occupier_email");
                        returnValue.Order.JobClientRef = DBUtil.GetStringValue(dr, "job_client_ref");
                        returnValue.Order.JobDesc = DBUtil.GetStringValue(dr, "job_desc");
                        returnValue.Order.JobDateDue = DBUtil.GetDateTimeValue(dr, "job_date_due");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Loading Diary Appointments.", ex);
            }
            return returnValue;
        }
        private DiaryAppsSmartForTimeSheet LoadDiaryAppsSmartForTimeEntry(OleDbDataReader dr)
        {
            const string METHOD_NAME = "DiaryAppsDB.LoadDiaryAppsSmartForTimeEntry()";
            DiaryAppsSmartForTimeSheet returnValue = null;
            try
            {
                if (dr != null)
                {
                    returnValue = new DiaryAppsSmartForTimeSheet();
                    returnValue.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    returnValue.JobSequence = DBUtil.GetLongValue(dr, "job_sequence");
                    returnValue.AppNotes = DBUtil.GetStringValue(dr, "app_notes");
                    returnValue.DateAppStart = DBUtil.GetDateTimeValue(dr, "date_app_start");
                    returnValue.VisitStatus = DBUtil.GetLongValue(dr, "visit_status");
                    returnValue.DateAppEnd = DBUtil.GetDateTimeValue(dr, "date_app_end");
                    returnValue.IsTimeSheetDone = DBUtil.GetBooleanValueForSql(dr, "is_timesheet_done");

                    returnValue.DAGPS = new DiaryAppsGPS();
                    returnValue.DAGPS.DateUserStart = DBUtil.GetDateTimeValue(dr, "date_user_start");
                    returnValue.DAGPS.UserStartGPSLong = DBUtil.GetStringValue(dr, "user_start_gps_long");
                    returnValue.DAGPS.UserStartGPSLat = DBUtil.GetStringValue(dr, "user_start_gps_lat");
                    returnValue.DAGPS.DateUserEnd = DBUtil.GetDateTimeValue(dr, "date_user_end");
                    returnValue.DAGPS.UserEndGPSLong = DBUtil.GetStringValue(dr, "user_end_gps_long");
                    returnValue.DAGPS.UserEndGPSLat = DBUtil.GetStringValue(dr, "user_end_gps_lat");
                    
                    if (returnValue.JobSequence > 0)
                    {
                        returnValue.Order = new OrdersSmartForTimeSheet();
                        returnValue.Order.JobRef = DBUtil.GetStringValue(dr, "job_ref");  
                        returnValue.Order.JobClientName = DBUtil.GetStringValue(dr, "job_client_name");
                        returnValue.Order.JobAddress = DBUtil.GetStringValue(dr, "job_address");
                        returnValue.Order.OccupierName = DBUtil.GetStringValue(dr, "occupier_name");
                        returnValue.Order.OccupierTelHome = DBUtil.GetStringValue(dr, "occupier_tel_home");
                        returnValue.Order.OccupierTelMobile = DBUtil.GetStringValue(dr, "occupier_tel_mobile");
                        returnValue.Order.OccupierEmail = DBUtil.GetStringValue(dr, "occupier_email");
                        returnValue.Order.JobClientRef = DBUtil.GetStringValue(dr, "job_client_ref");
                        returnValue.Order.JobDesc = DBUtil.GetStringValue(dr, "job_desc");
                        returnValue.Order.JobDateDue = DBUtil.GetDateTimeValue(dr, "job_date_due");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Loading Diary Appointments For TimeEntries.", ex);
            }
            return returnValue;
        }

        public List<DiaryAppsAssets> SelectDiaryAppsAssets(long sequence)
        {
            const string METHOD_NAME = "DiaryAppsDB.SelectDiaryAppsAssets()";
            List<DiaryAppsAssets> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsQueries.SelectDiaryAppAssetsBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<DiaryAppsAssets>();
                                while(dr.Read())
                                { 
                                    returnValue.Add(LoadDiaryAppsAssets(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting Diary App Assets.", ex);
            }
            return returnValue;
        }

        public List<DiaryAppsSmart> GetUserDiaryAppsByDateSmart(DateTime? appStartDate, int userId, bool isIncludeS4BFormIds)
        {
            const string METHOD_NAME = "DiaryAppsDB.GetUserDiaryAppsByDateSmart()";
            List<DiaryAppsSmart> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsQueries.SelectAppointmentsByUserIdAndDate(this.DatabaseType, appStartDate, userId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<DiaryAppsSmart>();
                                while (dr.Read())
                                {
                                    DiaryAppsSmart diaryAppsSmart = LoadDiaryAppsSmart(dr);
                                    if(isIncludeS4BFormIds && diaryAppsSmart != null)
                                    {
                                        diaryAppsSmart.S4BFormsIds = GetS4BFormSequencesByDiaryAppSequence(diaryAppsSmart.Sequence ?? 0);
                                    }
                                    returnValue.Add(diaryAppsSmart);
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
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting User Diary Apps By Date.", ex);
                Utilities.WriteLog(METHOD_NAME + ex.Message);
            }
            return returnValue;
        }

        public List<DiaryAppsSmartForTimeSheet> GetUserDiaryAppsByDateSmartForTimeEntry(DateTime? appStartDate, int userId)
        {
            const string METHOD_NAME = "DiaryAppsDB.GetUserDiaryAppsByDateSmartForTimeEntry()";
            List<DiaryAppsSmartForTimeSheet> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect = new OleDbCommand(DiaryAppsQueries.SelectAppointmentsByUserIdAndDateForTimeEntry(this.DatabaseType, appStartDate, userId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<DiaryAppsSmartForTimeSheet>();
                                while (dr.Read())
                                {
                                    DiaryAppsSmartForTimeSheet diaryAppsSmart = LoadDiaryAppsSmartForTimeEntry(dr);                                    
                                    returnValue.Add(diaryAppsSmart);
                                }
                            }
                            else
                                ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting User Diary Apps for Time entry By Date.", ex);
                Utilities.WriteLog(METHOD_NAME + ex.Message);
            }
            return returnValue;
        }
        
        public List<long> GetS4BFormSequencesByDiaryAppSequence(long sequence)
        {
            const string METHOD_NAME = "DiaryAppsDB.GetS4BFormSequencesByDiaryAppSequence()";
            List<long> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsQueries.SelectS4BFormIdsByDiaryAppSequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<long>();
                                while (dr.Read())
                                {
                                    returnValue.Add(DBUtil.GetLongValue(dr, "form_sequence"));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting S4B Form Sequences By Diary App Sequence.", ex);
            }
            return returnValue;
        }

        private DiaryAppsMobileNo LoadMobileNo(DataRow row)
        {
            DiaryAppsMobileNo returnValue = null;
            if (row != null)
            {
                returnValue = new DiaryAppsMobileNo();
                returnValue.MobileNo = DBUtil.GetStringValue(row, "mobile_no");

            }
            return returnValue;
        }
    }
}
