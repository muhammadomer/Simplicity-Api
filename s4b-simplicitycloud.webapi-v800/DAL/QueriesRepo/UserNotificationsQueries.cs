using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class UserNotificationsQueries
    {
        public static string insertToken(string databaseType, NotificationsToken obj)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "INSERT INTO un_user_firebase_sessions( user_id,  firebase_token, created_by,  date_created) " +
                                "VALUES ("  + obj.UserId + ", '" + obj.FirebaseToken + "', " +  obj.CreatedBy
                                + ", " + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated, true, true) + ")";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }


        public static string deleteByToken(string databaseType, int userId, string fb_token)
        {
            string returnValue = "";
            try
            {
                returnValue = "DELETE FROM   un_user_firebase_sessions " +
                              " WHERE user_id = " + userId + " and firebase_token='" + fb_token +"'";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string insertNotification(string databaseType, UserNotifications obj)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_user_notifications(user_id , title , body , click_action 
                    , registration_ids , data , flg_mark_as_read
                    , created_by,  date_created , last_amended_by, date_last_amended) " +
                "VALUES (" + obj.UserId +", '" + obj.title + "', '"  + obj.body +"' , '" + obj.click_action + "'" 
                    + ", '" + obj.registration_ids +"' ,'" + obj.data +"' , " + Utilities.GetBooleanForDML(databaseType, obj.flgMarkAsRead )
                    + " ,"+ obj.CreatedBy + ", " + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated, true, true) 
                    +" ," + obj.LastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended, true, true) +")";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string UpdatetNotification(string databaseType, UserNotifications obj)
        {
            string returnValue = "";
            try
            {
               
                returnValue = @"Update un_user_notifications Set
                    flg_mark_as_read = " + Utilities.GetBooleanForDML(databaseType, obj.flgMarkAsRead)
                    + ", last_amended_by = " + obj.LastAmendedBy
                    + ", date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended, true, true)
                    + " where sequence = " + obj.Sequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string insertSMSNotification(string databaseType, SMSNotifications obj)
        {
            string returnValue = "";
            try
            {
                
                returnValue = @"INSERT INTO un_notifications_sms(title , message , receiver_Id 
                    , send_to , sent_at , flg_mark_as_send,flg_reminder_enabled
                    , created_by,  date_created , last_amended_by, date_last_amended) " +
                "VALUES ('" +  obj.Title + "', '" + obj.Message + "' , " + obj.ReceiverId 
                    + ", '" + obj.SendTo + "' ," + Utilities.GetDateTimeForDML(databaseType, obj.SentAt,true,true) 
                    + " , " + Utilities.GetBooleanForDML(databaseType, obj.FlgMarkAsSend)
                    + " , " + Utilities.GetBooleanForDML(databaseType, obj.FlgReminderEnabled)
                    + " ," + obj.CreatedBy + ", " + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated, true, true)
                    + " ," + obj.LastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended, true, true) + ")";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string updateSMSNotificationAsSent(string databaseType, SMSNotifications obj)
        {
            string returnValue = "";
            try
            {
                
                returnValue = @"Update un_notifications_sms Set flg_mark_as_send = " +  Utilities.GetBooleanForDML(databaseType, true)
                    + " , last_amended_by = " + obj.LastAmendedBy 
                    + ", date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended, true, true) 
                    + " where sequence = " + obj.Sequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public static string getTokenRecord(string databaseType,string token)
        {
            string returnValue = "";
            try
            { 
                returnValue = "Select * From  un_user_firebase_sessions where firebase_token='" + token +"'";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getAllTokenList(string databaseType)
        {
            string returnValue = "";
            try
            {
                returnValue = "Select firebase_token From  un_user_firebase_sessions" ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getUserNotificationList(string databaseType,int userId,bool isOnlyNonRead)
        {
            string returnValue = "";
            string strWhere = "";
            try
            {
                if(isOnlyNonRead == true)
                {
                    strWhere = " And  flg_mark_as_read = " + Utilities.GetBooleanForDML(databaseType, false);
                }
                
                returnValue = @"Select * From  un_user_notifications 
                Where user_Id = " + userId + strWhere
                + " Order By date_created desc";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }


    }
}
