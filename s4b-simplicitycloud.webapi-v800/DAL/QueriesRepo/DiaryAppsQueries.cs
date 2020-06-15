using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class DiaryAppsQueries
    {
        public static string getDiaryResourceMobileNo(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT edc.tel_mobile AS mobile_no
                 FROM un_diary_apps AS da
                 INNER JOIN (un_diary_resources AS dr
                 INNER JOIN un_entity_details_core AS edc
                    ON dr.join_resource = edc.entity_id)
                    ON da.resource_sequence = dr.sequence
                 WHERE  edc.tel_mobile >'' And da.sequence = " + Sequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getDiaryClientMobileNo(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT ord.occupier_tel_mobile as mobile
                          FROM un_diary_apps AS da2
                         INNER JOIN un_orders AS ord
                            ON da2.job_sequence = ord.sequence
                         WHERE da2.sequence =" + Sequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                              "  FROM    un_diary_apps" +
                              " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getResourceSequenceByUserId(string databaseType, long userId)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT dr.join_resource AS entity_id
                    FROM un_user_details AS ud
                    INNER JOIN un_diary_resources AS dr ON ud.resource_sequence = dr.sequence
                    WHERE ud.user_id = " + userId;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string getAllDiaryAppTypes(string databaseType)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                              "  FROM    un_ref_diary_app_types";

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string getAllDiaryAppRates(string databaseType)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                              "  FROM    un_ref_diary_app_rates";

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }



        public static string getSelectAllThirdpartyByEntityId(string databaseType, long entityId)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "SELECT da.*,w3p.user_name,ord.job_ref, ord.job_client_ref,ord.job_desc, " +
                                "      ord.job_cost_centre, ord.job_trade_code, ord.job_address, ord.job_date, " +
                                "      ord.job_date_due, ord.date_user1,rjst.status_desc,ord.job_priority_code " +
                                " ,un_diary_apps_web_assign.add_info,un_diary_apps_web_assign.delay_reason,un_diary_apps_web_assign.flg_complete,un_diary_apps_web_assign.flg_delay,un_diary_apps_web_assign.date_app_completed,un_diary_apps_web_assign.sequence as web_assign_sequence " +
                                " FROM((((un_diary_apps AS da INNER JOIN un_orders AS ord " +
                                "   ON da.job_sequence = ord.sequence) " +
                                " INNER JOIN un_diary_resources AS dr " +
                                "   ON da.resource_sequence = dr.sequence) " +
                                " LEFT JOIN un_ref_job_status_type AS rjst " +
                                "   ON ord.job_status = rjst.status_id) " +
                                " LEFT JOIN un_diary_apps_web_assign " +
                                "   ON da.sequence = un_diary_apps_web_assign.de_sequence) " +
                                " LEFT JOIN un_web_3rd_parties AS w3p " +
                                "   ON un_diary_apps_web_assign.web_id = w3p.web_id " +
                                "WHERE dr.join_resource = " + entityId + " " +
                                "ORDER BY da.date_app_start DESC ";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string checkClientTimeSlotDuplicate(string databaseType, DateTime? appStartDate, DateTime? appEndDate, long clientId, long resourceId, long sequence = 0)
        {
            string returnValue = "";
            try
            {
                returnValue = @"Select * from un_diary_apps
				Where (resource_sequence = " + resourceId
                + " And " + Utilities.GetDateTimeForDML(databaseType, appStartDate, true, true) + " >=date_app_start"
                + " And " + Utilities.GetDateTimeForDML(databaseType, appStartDate, true, true) + " <= date_app_end"
                + " or "
                + "client_id = " + clientId
                + " And " + Utilities.GetDateTimeForDML(databaseType, appStartDate, true, true) + " >=date_app_start"
                + " And " + Utilities.GetDateTimeForDML(databaseType, appStartDate, true, true) + " <= date_app_end)";
                if (sequence > 0)
                    returnValue += $" and sequence!='{sequence}'";
                returnValue += " Union all "
                + " Select * from un_diary_apps "
                + " Where (resource_sequence =" + resourceId
                + " And " + Utilities.GetDateTimeForDML(databaseType, appEndDate, true, true) + " >=date_app_start"
                + " And " + Utilities.GetDateTimeForDML(databaseType, appEndDate, true, true) + " <= date_app_end"
                + " or "
                + "client_id = " + clientId
                + " And " + Utilities.GetDateTimeForDML(databaseType, appEndDate, true, true) + " >=date_app_start"
                + " And " + Utilities.GetDateTimeForDML(databaseType, appEndDate, true, true) + " <= date_app_end)";
                if (sequence > 0)
                    returnValue += $" and sequence!='{sequence}'";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getSelectAllByAppDate(string databaseType, DateTime? appStartDate, DateTime? appEndDate)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                        returnValue = @"SELECT un_diary_apps.*, un_orders.job_ref, un_orders.job_client_name, un_orders.job_address, un_orders.job_desc 
                                      FROM    un_diary_apps 
                                        LEFT JOIN un_orders ON un_diary_apps.job_sequence = un_orders.sequence
                                       WHERE date_app_start BETWEEN#" + ((DateTime)appStartDate).ToString("MM/dd/yyyy") + " 00:00:00# " +
                                      "   AND #" + ((DateTime)appEndDate).ToString("MM/dd/yyyy") + " 23:59:59#";
                        break;
                    case "SQLSERVER":
                    default:
                        returnValue = @"SELECT un_diary_apps.*, un_orders.job_ref, un_orders.job_client_name, un_orders.job_address, un_orders.job_desc
                                      FROM    un_diary_apps 
                                      LEFT Outer JOIN un_orders ON un_diary_apps.job_sequence = un_orders.sequence
                                      WHERE date_app_start BETWEEN '" + ((DateTime)appStartDate).ToString("yyyy-MM-dd") + " 00:00:00' AND " +
                                      "    '" + ((DateTime)appEndDate).ToString("yyyy-MM-dd") + " 23:59:59'";
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByAppDateAndJobRef(string databaseType, DateTime? appStartDate, DateTime? appEndDate,string jobRef)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                        returnValue = @"SELECT un_diary_apps.*, un_orders.job_ref, un_orders.job_client_name, un_orders.job_address, un_orders.job_desc 
                                      FROM    un_diary_apps 
                                        LEFT JOIN un_orders ON un_diary_apps.job_sequence = un_orders.sequence
                                       WHERE un_orders.sequence=" + jobRef+" AND ( date_app_start BETWEEN#" + ((DateTime)appStartDate).ToString("MM/dd/yyyy") + " 00:00:00# " +
                                      "   AND #" + ((DateTime)appEndDate).ToString("MM/dd/yyyy") + " 23:59:59#)";
                        break;
                    case "SQLSERVER":
                    default:
                        returnValue = @"SELECT un_diary_apps.*, un_orders.job_ref, un_orders.job_client_name, un_orders.job_address, un_orders.job_desc
                                      FROM    un_diary_apps 
                                      LEFT Outer JOIN un_orders ON un_diary_apps.job_sequence = un_orders.sequence
                                      WHERE  un_orders.sequence=" + jobRef + " AND ( date_app_start BETWEEN '" + ((DateTime)appStartDate).ToString("yyyy-MM-dd") + " 00:00:00' AND " +
                                      "    '" + ((DateTime)appEndDate).ToString("yyyy-MM-dd") + " 23:59:59')";
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByJobSequence(string databaseType,long jobSequence)
        {
            string returnValue = "";
            try
            {
                        returnValue = @"SELECT un_diary_apps.sequence,un_diary_apps.date_app_start, un_diary_apps.date_app_end
                            , un_diary_apps.app_subject, un_diary_apps.app_notes, un_diary_apps.app_category, un_diary_resources.resource_name
                            , un_orders.job_ref, un_orders.job_client_name, un_orders.job_address, un_orders.job_desc 
                        FROM un_diary_resources INNER JOIN (un_diary_apps 
                           LEFT JOIN un_orders ON un_diary_apps.job_sequence = un_orders.sequence) ON un_diary_resources.sequence = un_diary_apps.resource_sequence
                        WHERE un_orders.sequence= " + jobSequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, string transType, long joinResource, bool flgUseClientId, long clientId, long jobSequence, long jobAddressId, bool flgBookingRequired,
                                    long resourceSequence, DateTime? dateAppStart, DateTime? dateAppEnd, bool flgAppAllDay, string appPostCode, string appSubject,
                                    string appLocation, bool flgAppReminder, string appReminderSound, long appReminderMins, string appNotes, string appCategory,
                                    string appAttachmentPath, bool flgOnlineMeeting, bool flgUnavailable, long repeatSequence, long multiResourceSequence, long appType,
                                    bool flgAppDeleted, bool flgAppCompleted, bool flgAppBroken, long appBrokenReason, bool flgNoAccess, bool flgAppConfirmed,
                                    DateTime? dateAppConfirmed, string appConfirmedBy, long certSequence, long visitStatus,  bool flgAppFixed,
                                    bool flgPrint, long printUserId, long unscheduledDeSeq, long rateSequence, long createdBy, DateTime? dateCreated
                                    )
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "INSERT INTO  un_diary_apps(trans_type,  join_resource,  flg_use_client_id,  client_id,  job_sequence,  job_address_id,  flg_booking_required," +
                                          "                           resource_sequence,  date_app_start,  date_app_end,  flg_app_all_day,  app_post_code,  app_subject,  app_location," +
                                          "                           flg_app_reminder,  app_reminder_sound,  app_reminder_mins,  app_notes,  app_category,  app_attachment_path," +
                                          "                           flg_online_meeting,  flg_unavailable,  repeat_sequence,  multi_resource_sequence,  app_type,  flg_app_deleted," +
                                          "                           flg_app_completed,  flg_app_broken,  app_broken_reason,  flg_no_access,  flg_app_confirmed,  date_app_confirmed," +
                                          "                           app_confirmed_by,  cert_sequence,  visit_status,   flg_app_fixed,  flg_print,  print_user_id," +
                                          "                           unscheduled_de_seq,  rate_sequence,  created_by,  date_created)" +
                                          "VALUES ('" + transType + "',   " + joinResource + ",   " + Utilities.GetBooleanForDML(databaseType, flgUseClientId) + ",   " + clientId + ",   " + jobSequence + ",   " + jobAddressId + ",   " +
                                                   Utilities.GetBooleanForDML(databaseType, flgBookingRequired) + ",   " + resourceSequence + ",   " + Utilities.GetDateTimeForDML(databaseType, dateAppStart,true,true) + ",   " + Utilities.GetDateTimeForDML(databaseType, dateAppEnd,true,true) + ",   " +
                                                   Utilities.GetBooleanForDML(databaseType, flgAppAllDay) + ",   '" + appPostCode + "',  ' " + appSubject + "',   '" + appLocation + "',   " + Utilities.GetBooleanForDML(databaseType, flgAppReminder) + ",   '" + appReminderSound + "',   " +
                                                    appReminderMins + ",  ' " + appNotes + "',   '" + appCategory + "',   '" + appAttachmentPath + "',   " + Utilities.GetBooleanForDML(databaseType, flgOnlineMeeting) + ",   " +
                                                    Utilities.GetBooleanForDML(databaseType, flgUnavailable) + ",   " + repeatSequence + ",   " + multiResourceSequence + ",   " + appType + ",   " + Utilities.GetBooleanForDML(databaseType, flgAppDeleted) + ",   " +
                                                    Utilities.GetBooleanForDML(databaseType, flgAppCompleted) + ",   " + Utilities.GetBooleanForDML(databaseType, flgAppBroken) + ",   " + appBrokenReason + ",   " + Utilities.GetBooleanForDML(databaseType, flgNoAccess) + ",   " + Utilities.GetBooleanForDML(databaseType, flgAppConfirmed) + ",   " +
                                                    Utilities.GetDateTimeForDML(databaseType, dateAppConfirmed,true,true) + ",   '" + appConfirmedBy + "',   " + certSequence + ",   " + visitStatus + ",  " +
                                                    Utilities.GetBooleanForDML(databaseType, flgAppFixed) + ",   " + Utilities.GetBooleanForDML(databaseType, flgPrint) + ",   " + printUserId + ",   " + unscheduledDeSeq + ",   " + rateSequence + ",   " + createdBy + ",   " +
                                                    Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true)  + ")";
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getAppoinmentsByAppDateAndUserId(string databaseType, DateTime? appStartDate, int userId)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                        returnValue = "SELECT da.*, dr.resource_name, " +
                                      "       ord.job_ref, ord.job_client_id, ord.job_client_name, ord.job_client_ref, ord.job_desc, " +
                                      "       ord.job_date_due, ord.job_cost_centre, ord.job_trade_code, ord.job_address, " +
                                      "       ord.occupier_name, ord.occupier_tel_home, ord.occupier_tel_work, " +
                                      "       ord.occupier_tel_work_ext, ord.occupier_tel_mobile, ord.occupier_email, " +
                                      "       edc_p.address_full AS job_address_vert, edc_p.address_post_code, " +
                                      "       edc_f.user_text_field1 AS gas_safe_no, " +
                                      "       edc_b.entity_id, edc_b.flg_entity_join, " +
                                      "       edc_bp.entity_id AS landlord_id, edc_bp.name_long AS landlord_name, edc_bp.address_full AS landlord_address_vert " +
                                      "       FROM ((((( un_user_details AS ud " +
                                      "       INNER JOIN (un_diary_resources AS dr " +
                                      "       INNER JOIN un_diary_apps AS da " +
                                      "          ON dr.sequence = da.resource_sequence) " +
                                      "          ON ud.resource_sequence = dr.sequence) " +
                                      "       INNER JOIN un_orders AS ord ON da.job_sequence = ord.sequence) " +
                                      "       INNER JOIN un_entity_details_core AS edc_f  ON dr.join_resource = edc_f.entity_id) " +
                                      "       INNER JOIN un_entity_details_core AS edc_p ON ord.job_address_id = edc_p.entity_id) " +
                                      "       INNER JOIN un_entity_details_core AS edc_b ON ord.job_client_id = edc_b.entity_id) " +
                                      "       INNER JOIN un_entity_details_core AS edc_bp  ON edc_b.entity_join_id = edc_bp.entity_id " +
                                      " WHERE ud.user_id = " + userId + " " +
                                      "   AND da.date_app_start BETWEEN #" + Utilities.getAccessDateWithoutTimeAndHashes(appStartDate) + " 00:00:00# AND " +
                                      "      #" + Utilities.getAccessDateWithoutTimeAndHashes(appStartDate) + " 23:59:59# " +
                                      " ORDER BY da.date_app_start ASC";
                        break;
                    case "SQLSERVER":
                    default:
                        returnValue = "SELECT da.*, dr.resource_name, " +
                                      "       ord.job_ref, ord.job_client_id, ord.job_client_name, ord.job_client_ref, ord.job_desc, " +
                                      "       ord.job_date_due, ord.job_cost_centre, ord.job_trade_code, ord.job_address, " +
                                      "       ord.occupier_name, ord.occupier_tel_home, ord.occupier_tel_work, " +
                                      "       ord.occupier_tel_work_ext, ord.occupier_tel_mobile, ord.occupier_email, " +
                                      "       edc_p.address_full AS job_address_vert, edc_p.address_post_code, " +
                                      "       edc_f.user_text_field1 AS gas_safe_no, " +
                                      "       edc_b.entity_id, edc_b.flg_entity_join, " +
                                      "       edc_bp.entity_id AS landlord_id, edc_bp.name_long AS landlord_name, edc_bp.address_full AS landlord_address_vert " +
                                      "       FROM ((((( un_user_details AS ud " +
                                       "       INNER JOIN (un_diary_resources AS dr " +
                                      "       INNER JOIN un_diary_apps AS da " +
                                      "          ON dr.sequence = da.resource_sequence) " +
                                      "          ON ud.resource_sequence = dr.sequence) " +
                                      "       INNER JOIN un_orders AS ord ON da.job_sequence = ord.sequence) " +
                                      "       INNER JOIN un_entity_details_core AS edc_f  ON dr.join_resource = edc_f.entity_id) " +
                                      "       INNER JOIN un_entity_details_core AS edc_p ON ord.job_address_id = edc_p.entity_id) " +
                                      "       INNER JOIN un_entity_details_core AS edc_b ON ord.job_client_id = edc_b.entity_id) " +
                                      "       INNER JOIN un_entity_details_core AS edc_bp  ON edc_b.entity_join_id = edc_bp.entity_id " +
                                      " WHERE ud.user_id = " + userId + " " +
                                      "   AND da.date_app_start BETWEEN '" + Utilities.getSQLDateWithoutTimeAndQuotes(appStartDate) + " 00:00:00' AND " +
                                      "      '" + Utilities.getSQLDateWithoutTimeAndQuotes(appStartDate) + " 23:59:59' " +
                                      " ORDER BY da.date_app_start ASC";
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string update(string databaseType, long sequence, string transType, long joinResource, bool flgUseClientId, long clientId, long jobSequence, long jobAddressId, bool flgBookingRequired,
                                    long resourceSequence, DateTime? dateAppStart, DateTime? dateAppEnd, bool flgAppAllDay, string appPostCode, string appSubject,
                                    string appLocation, bool flgAppReminder, string appReminderSound, long appReminderMins, string appNotes, string appCategory,
                                    string appAttachmentPath, bool flgOnlineMeeting, bool flgUnavailable, long repeatSequence, long multiResourceSequence, long appType,
                                    bool flgAppDeleted, bool flgAppCompleted, bool flgAppBroken, long appBrokenReason, bool flgNoAccess, bool flgAppConfirmed,
                                    DateTime? dateAppConfirmed, string appConfirmedBy, long certSequence, long visitStatus, DateTime? visitVam, bool flgAppFixed,
                                    bool flgPrint, long printUserId, long unscheduledDeSeq, long rateSequence, 
                                    long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_diary_apps " +
                                 "   SET    trans_type =  '" + transType + "',  " +
                                 "          join_resource =  " + joinResource + ",  " +
                                 "          flg_use_client_id = " + Utilities.GetBooleanForDML(databaseType, flgUseClientId) + ",  " +
                                 "          client_id =  " + clientId + ",  " +
                                 "          job_sequence =  " + jobSequence + ",  " +
                                 "          job_address_id =  " + jobAddressId + ",  " +
                                 "          flg_booking_required = " + Utilities.GetBooleanForDML(databaseType, flgBookingRequired) + ",  " +
                                 "          resource_sequence =  " + resourceSequence + ",  " +
                                 "          date_app_start =  " + Utilities.GetDateTimeForDML(databaseType, dateAppStart,true,true) + ", " +
                                 "          date_app_end =  " + Utilities.GetDateTimeForDML(databaseType, dateAppEnd,true,true) + ", " +
                                 "          flg_app_all_day = " + Utilities.GetBooleanForDML(databaseType, flgAppAllDay) + ",  " +
                                 "          app_post_code =  '" + appPostCode + "',  " +
                                 "          app_subject =  '" + appSubject + "',  " +
                                 "          app_location = ' " + appLocation + "',  " +
                                 "          flg_app_reminder = " + Utilities.GetBooleanForDML(databaseType, flgAppReminder) + ",  " +
                                 "          app_reminder_sound =  '" + appReminderSound + "',  " +
                                 "          app_reminder_mins =  " + appReminderMins + ",  " +
                                 "          app_notes =  '" + appNotes + "',  " +
                                 "          app_category =  '" + appCategory + "',  " +
                                 "          app_attachment_path = ' " + appAttachmentPath + "',  " +
                                 "          flg_online_meeting = " + Utilities.GetBooleanForDML(databaseType, flgOnlineMeeting) + ",  " +
                                 "          flg_unavailable = " + Utilities.GetBooleanForDML(databaseType, flgUnavailable) + ",  " +
                                 "          repeat_sequence =  " + repeatSequence + ",  " +
                                 "          multi_resource_sequence =  " + multiResourceSequence + ",  " +
                                 "          app_type =  " + appType + ",  " +
                                 "          flg_app_deleted = " + Utilities.GetBooleanForDML(databaseType, flgAppDeleted) + ",  " +
                                 "          flg_app_completed = " + Utilities.GetBooleanForDML(databaseType, flgAppCompleted) + ",  " +
                                 "          flg_app_broken = " + Utilities.GetBooleanForDML(databaseType, flgAppBroken) + ",  " +
                                 "          app_broken_reason =  " + appBrokenReason + ",  " +
                                 "          flg_no_access = " + Utilities.GetBooleanForDML(databaseType, flgNoAccess) + ",  " +
                                 "          flg_app_confirmed = " + Utilities.GetBooleanForDML(databaseType, flgAppConfirmed) + ",  " +
                                 "          date_app_confirmed =  " + Utilities.GetDateTimeForDML(databaseType, dateAppConfirmed,true,true) + ", " +
                                 "          app_confirmed_by =  '" + appConfirmedBy + "',  " +
                                 "          cert_sequence =  " + certSequence + ",  " +
                                 "          visit_status =  " + visitStatus + ",  " +
                                 //"          visit_vam =  " + Utilities.getSQLDate(visitVam) + ",  " +
                                 "          flg_app_fixed = " + Utilities.GetBooleanForDML(databaseType, flgAppFixed) + ",  " +
                                 "          flg_print = " + Utilities.GetBooleanForDML(databaseType, flgPrint) + ",  " +
                                 "          print_user_id =  " + printUserId + ",  " +
                                 "          unscheduled_de_seq =  " + unscheduledDeSeq + ",  " +
                                 "          rate_sequence =  " + rateSequence + ",  " +
                                 "          last_amended_by =  " + lastAmendedBy + ",  " +
                                 "          date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) +
                                 "  WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string updateVisitStatusBySequence(string databaseType, long sequence, int visitStatus, bool flgNoAccess,
                                                           long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_diary_apps " +
                                     "   SET visit_status = " + visitStatus + ", " +
                                     "       flg_no_access = " + Utilities.GetBooleanForDML(databaseType, flgNoAccess) + ", " +
                                     "       last_amended_by = " + lastAmendedBy + ", " +
                                     "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, DateTime.Now,true,true) + " " +
                                     " WHERE sequence = " + sequence;
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateDiaryAppGPSDetails(string databaseType, long sequence, DiaryAppsGPS obj,
                                                        int lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"UPDATE un_diary_apps 
                        SET date_user_start = " + Utilities.GetDateTimeForDML(databaseType, obj.DateUserStart, true, true) + ", " +
                    "       user_start_gps_long = '" + obj.UserStartGPSLong + "', " +
                    "       user_start_gps_lat = '" + obj.UserStartGPSLat + "', " +
                    "       date_user_end = " + Utilities.GetDateTimeForDML(databaseType, obj.DateUserEnd, true, true) + ", " +
                    "       user_end_gps_long = '" + obj.UserEndGPSLong + "', " +
                    "       user_end_gps_lat = '" + obj.UserEndGPSLat + "', " +
                    "       last_amended_by = " + lastAmendedBy + ", " +
                    "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) +
                    " WHERE sequence = " + sequence;
        }

        internal static string updateVisitStatusAndFlgCompletedBySequence(string databaseType, long sequence, int visitStatus, bool flgCompleted,
                                                                          long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {

                returnValue = "UPDATE un_diary_apps SET ";
                if (visitStatus >= 0)
                {
                    returnValue += " visit_status = " + visitStatus + ", ";
                }
                if (flgCompleted)
                {
                    returnValue += " flg_app_completed = " + Utilities.GetBooleanForDML(databaseType,true) + ",";
                }
                returnValue += " last_amended_by = " + lastAmendedBy + ", " +
                            " date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, DateTime.Now,true,true) + " " +
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
                returnValue = "DELETE FROM   un_diary_apps " +
                              "WHERE sequence = " + sequence;
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
                returnValue = "UPDATE   un_diary_apps " +
                              "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                              " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string SelectDiaryAppAssetsBySequence(string databaseType, long sequence)
        {
            return @"SELECT da_assets.asset_sequence, ar.sequence, ar.item_location, ar.item_manufacturer, ar.item_model, 
                            ar.item_serial_ref, ar.item_extra_info, ar.item_user_field1, da_assets.de_sequence,
                    (SELECT asset_category_desc from un_asset_register_cats 
                      WHERE un_asset_register_cats.asset_category_id = ar.item_join_category) AS asset_category_details
                       FROM un_diary_apps_assets  AS da_assets, un_asset_register AS ar 
                      WHERE da_assets.asset_sequence = ar.sequence 
                        AND da_assets.de_sequence = " + sequence;
        }

        public static string SelectAppointmentsByUserIdAndDate(string databaseType, DateTime? appStartDate, int userId)
        {
            string returnValue = "";
            switch (databaseType)
            {
                case "MSACCESS":
                    returnValue = @"SELECT da.sequence, da.date_app_start, da.app_notes, da.visit_status, da.job_sequence, da.date_user_start,
                                           da.user_start_gps_long, da.user_start_gps_lat, da.date_user_end, da.user_end_gps_long, da.user_end_gps_lat,
                                           ord.job_ref, ord.job_client_name, ord.job_address, ord.occupier_name, ord.occupier_tel_home, 
                                           ord.occupier_tel_mobile, ord.occupier_email, ord.job_client_ref, ord.job_desc, ord.job_date_due
                                        FROM un_diary_apps AS da, un_orders AS ord, un_user_details AS ud
                                    WHERE da.job_sequence = ord.sequence
                                        AND ud.resource_sequence = da.resource_sequence
                                       AND flg_app_broken <> " + Utilities.GetBooleanForDML(databaseType, true) +
                                      " AND flg_app_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) +
                                      "  AND ud.user_id = " + userId +
                                    "   AND da.date_app_start BETWEEN #" + Utilities.GetDateTimeForDML(databaseType, appStartDate, false, false) + " 00:00:00# " +
                                    "   AND #" + Utilities.GetDateTimeForDML(databaseType, appStartDate, false, false) + " 23:59:59# " +
                                    " ORDER BY da.date_app_start ASC";
                    break;
                case "SQLSERVER":
                default:
                    returnValue = @"SELECT da.sequence, da.date_app_start, da.app_notes, da.visit_status, da.job_sequence, ord.job_ref, 
                                            ord.job_client_name, ord.job_address, ord.occupier_name, ord.occupier_tel_home,                                            
                                            ord.occupier_tel_mobile, ord.occupier_email, ord.job_client_ref, ord.job_desc, ord.job_date_due
                                        FROM un_diary_apps AS da, un_orders AS ord, un_user_details AS ud
                                    WHERE da.job_sequence = ord.sequence
                                        AND ud.resource_sequence = da.resource_sequence
                                        AND flg_app_broken <> " + Utilities.GetBooleanForDML(databaseType,true) +
                                      " AND flg_app_deleted <> " + Utilities.GetBooleanForDML(databaseType,true) +
                                      " AND ud.user_id = " + userId +
                                    "   AND da.date_app_start BETWEEN '" + Utilities.GetDateTimeForDML(databaseType, appStartDate, false, false) + " 00:00:00' " +
                                    "   AND '" + Utilities.GetDateTimeForDML(databaseType, appStartDate, false, false) + " 23:59:59' " +
                                    " ORDER BY da.date_app_start ASC";
                    break;
            }
            return returnValue;
        }

        public static string SelectAppointmentsByUserIdAndDateForTimeEntry(string databaseType, DateTime? appStartDate, int userId)
        {
            string returnValue = "";
            switch (databaseType)
            {
                case "MSACCESS":
                    returnValue = @"SELECT da.sequence,  da.date_app_start,  da.app_notes,  da.visit_status,  da.job_sequence,  ord.job_ref, 
                                         da.date_app_end,  da.date_user_start, da.user_start_gps_long,  da.user_start_gps_lat,  da.date_user_end, 
                                         da.user_end_gps_long,  da.user_end_gps_lat, ord.job_client_name,  ord.job_address,  ord.occupier_name,  ord.occupier_tel_home, 
                                         IIF (((select count(sequence) from un_tmp_timesheet utime where utime.job_sequence = da.job_sequence) > 0 ), 1, 0) as is_timesheet_done,
                                         ord.occupier_tel_mobile,  ord.occupier_email,  ord.job_client_ref,  ord.job_desc,  ord.job_date_due
                                    FROM un_diary_apps AS da, un_orders AS ord, un_user_details AS ud
                                    WHERE da.job_sequence = ord.sequence
                                        AND ud.resource_sequence = da.resource_sequence
                                        AND flg_app_broken <> " + Utilities.GetBooleanForDML(databaseType, true) +
                                       " AND flg_app_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) +
                                       " AND ud.user_id = " + userId +
                                       " AND da.date_app_start BETWEEN #" + Utilities.GetDateTimeForDML(databaseType, appStartDate, false, false) + " 00:00:00# " +
                                       " AND #" + Utilities.GetDateTimeForDML(databaseType, appStartDate, false, false) + " 23:59:59# " +
                                       " ORDER BY da.date_app_start ASC";
                    break;
                case "SQLSERVER":
                default:
                    returnValue = @"SELECT da.sequence,  da.date_app_start,  da.app_notes,  da.visit_status,  da.job_sequence,  ord.job_ref, 
                                         da.date_app_end,  da.date_user_start, da.user_start_gps_long,  da.user_start_gps_lat,  da.date_user_end, 
                                         da.user_end_gps_long,  da.user_end_gps_lat, ord.job_client_name,  ord.job_address,  ord.occupier_name,  ord.occupier_tel_home, 
                                         case when (select count(*) from un_tmp_timesheet utime where utime.job_sequence = da.job_sequence ) > 0 then 1 else 0 end as is_timesheet_done,
                                         ord.occupier_tel_mobile,  ord.occupier_email,  ord.job_client_ref,  ord.job_desc,  ord.job_date_due
                                    FROM un_diary_apps AS da, un_orders AS ord, un_user_details AS ud
                                    WHERE da.job_sequence = ord.sequence
                                        AND ud.resource_sequence = da.resource_sequence
                                        AND flg_app_broken <> " + Utilities.GetBooleanForDML(databaseType, true) +
                                      " AND flg_app_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) +
                                      " AND ud.user_id = " + userId +
                                    "   AND da.date_app_start BETWEEN '" + Utilities.GetDateTimeForDML(databaseType, appStartDate, false, false) + " 00:00:00' " +
                                    "   AND '" + Utilities.GetDateTimeForDML(databaseType, appStartDate, false, false) + " 23:59:59' " +
                                    " ORDER BY da.date_app_start ASC";
                    break;
            }
            return returnValue;
        }

        public static string SelectS4BFormIdsByDiaryAppSequence(string databaseType, long sequence)
        {
         //return @"SELECT rs4b.form_sequence 
         //           FROM un_diary_apps_natural_forms as danf, 
         //                un_ref_natural_forms as rnf, un_ref_s4b_forms as rs4b
         //          WHERE rs4b.form_id = rnf.form_id 
         //            AND danf.form_sequence = rnf.form_sequence 
         //            AND danf.diary_apps_sequence = " + sequence;
         return @"SELECT rs4b.form_sequence 
                       FROM un_diary_apps_natural_forms as danf,  un_ref_s4b_forms as rs4b
                      WHERE danf.form_sequence = rs4b.form_sequence 
                        AND danf.diary_apps_sequence = " + sequence;
      }
    }
}

