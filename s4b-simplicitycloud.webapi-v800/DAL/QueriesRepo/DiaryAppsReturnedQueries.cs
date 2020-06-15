using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class DiaryAppsReturnedQueries
    {

        public static string getSelectAllBySequence(long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                              "  FROM     un_diary_apps_returned" +
                              " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, long daSequence, long resourceSequence, long jobSequence, DateTime? dateAppStart, DateTime? dateAppEnd, string appSubject, 
                                    string appLocation, string appNotes, long appType, long visitStatus, string returnReason, long createdBy, DateTime? dateCreated, 
                                    long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "INSERT INTO    un_diary_apps_returned(da_sequence,  resource_sequence,  job_sequence,  date_app_start,  date_app_end,"+
                        "                                                   app_subject,  app_location,  app_notes,  app_type,  visit_status,  return_reason,"+
                        "                                                   created_by,  date_created,  last_amended_by,  date_last_amended)" +
                        "VALUES ( " + daSequence + ",   " + resourceSequence + ",   " + jobSequence + ",   " + Utilities.GetDateTimeForDML(databaseType, dateAppStart,true,true) + ",   " + 
                                Utilities.GetDateTimeForDML(databaseType, dateAppEnd,true,true) + ",   '" + appSubject + "',   '" + appLocation + "',   '" + appNotes + "',   " + appType + ",   " +
                                visitStatus + ",   " + returnReason + ",   " + createdBy + ",   " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ",   " + 
                                lastAmendedBy + ",   " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true)+ ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string update(string databaseType, long sequence, long daSequence, long resourceSequence, long jobSequence, DateTime? dateAppStart, DateTime? dateAppEnd, string appSubject,
                                    string appLocation, string appNotes, long appType, long visitStatus, string returnReason, long createdBy, DateTime? dateCreated,
                                    long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_diary_apps_returned " +
                                 "   SET  da_sequence =  " + daSequence + ",  " +
                                 "        resource_sequence =  " + resourceSequence + ",  " +
                                 "        job_sequence =  " + jobSequence + ",  " +
                                 "        date_app_start =  " + Utilities.GetDateTimeForDML(databaseType, dateAppStart,true,true) + ", " +
                                 "        date_app_end =  " + Utilities.GetDateTimeForDML(databaseType, dateAppEnd, true, true) + ", " +
                                 "        app_subject =  '" + appSubject + "',  " +
                                 "        app_location =  '" + appLocation + "',  " +
                                 "        app_notes =  '" + appNotes + "'s,  " +
                                 "        app_type =  " + appType + ",  " +
                                 "        visit_status =  " + visitStatus + ",  " +
                                 "        return_reason =  " + returnReason + ",  " +
                                 "        created_by =  " + createdBy + ",  " +
                                 "        date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " +
                                 "        last_amended_by =  " + lastAmendedBy + ",  " +
                                 "        date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ", " +
                                 "  WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string insertByAppointmentSequence(string databaseType, long daSequence, long visitStatus, string returnReason, 
                                                           long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "INSERT INTO un_diary_apps_returned (da_sequence, resource_sequence, job_sequence, " +
                                "       date_app_start, date_app_end, app_subject, app_location, app_notes, app_type, visit_status, " +
                                "       return_reason, created_by, date_created, last_amended_by, date_last_amended) " +
                                "SELECT sequence, resource_sequence, job_sequence, date_app_start, date_app_end, app_subject, " +
                                "       app_location, app_notes, app_type, " + visitStatus + ", '" + Utilities.replaceSpecialChars(returnReason) + "', " +
                                "       " + createdBy + ", " + Utilities.GetDateTimeForDML(databaseType, DateTime.Now,true,true) + ", " + lastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, DateTime.Now, true, true) +
                                "  FROM un_diary_apps " +
                                " WHERE sequence = " + daSequence;

                   
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string delete(long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "DELETE FROM   un_diary_apps_returned " +
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
                returnValue = "UPDATE   un_diary_apps_returned " +
                              "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg) + ", " +
                              "WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

