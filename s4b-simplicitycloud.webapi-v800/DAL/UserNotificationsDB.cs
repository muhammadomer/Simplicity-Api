using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;

namespace SimplicityOnlineWebApi.DAL
{
    public class UserNotificationsDB : MainDB
    {

        public UserNotificationsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public List<NotificationsToken> getAllTokenList()
        {
            List<NotificationsToken> returnValue = new List<NotificationsToken>();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(UserNotificationsQueries.getAllTokenList(this.DatabaseType), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {   
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(LoadToken(row));
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

        public List<NotificationsToken> getTokenList(string token)
        {
            List<NotificationsToken> returnValue = new List<NotificationsToken>();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(UserNotificationsQueries.getTokenRecord(this.DatabaseType,token), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(LoadToken(row));
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
        public List<UserNotifications> getUserNotificationList(int userId, bool isOnlyNonRead)
        {
            List<UserNotifications> returnValue = new List<UserNotifications>();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(UserNotificationsQueries.getUserNotificationList(this.DatabaseType,userId, isOnlyNonRead), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(LoadNotifications(row));
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
        public bool insertFirebaseToken(out long sequence, NotificationsToken obj)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(UserNotificationsQueries.insertToken(this.DatabaseType, obj), conn))
                    {
                        objCmd.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public bool deleteByToken(int userId, string fb_token)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(UserNotificationsQueries.deleteByToken(this.DatabaseType, userId,fb_token), conn))
                    {
                        objCmd.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public bool insertNotification(out long sequence, UserNotifications obj)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(UserNotificationsQueries.insertNotification(this.DatabaseType, obj), conn))
                    {
                        objCmd.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public bool updateNotification(UserNotifications obj)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(UserNotificationsQueries.UpdatetNotification(this.DatabaseType, obj), conn))
                    {
                        objCmd.ExecuteNonQuery();
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public bool insertSMS(out long sequence, SMSNotifications obj)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(UserNotificationsQueries.insertSMSNotification(this.DatabaseType, obj), conn))
                    {
                        objCmd.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public bool updateSMSNotificationAsSent(SMSNotifications obj)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(UserNotificationsQueries.updateSMSNotificationAsSent(this.DatabaseType, obj), conn))
                    {
                        objCmd.ExecuteNonQuery();
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        private NotificationsToken LoadToken(DataRow row)
        {
            NotificationsToken returnValue = null;
            if (row != null)
            {
                returnValue = new NotificationsToken();
                returnValue.Sequence = DBUtil.GetLongValue(row, "sequence");
                returnValue.UserId = DBUtil.GetIntValue(row, "user_id");
                returnValue.FirebaseToken = DBUtil.GetStringValue(row, "firebase_token");

            }
            return returnValue;
        }

        private UserNotifications LoadNotifications(DataRow row)
        {
            UserNotifications returnValue = null;
            if (row != null)
            {
                returnValue = new UserNotifications();
                returnValue.Sequence = DBUtil.GetLongValue(row, "sequence");
                returnValue.title = DBUtil.GetStringValue(row, "title");
                returnValue.body = DBUtil.GetStringValue(row, "body");
                returnValue.click_action = DBUtil.GetStringValue(row, "click_action");
                returnValue.registration_ids = DBUtil.GetStringValue(row, "registration_ids");
                returnValue.data = DBUtil.GetStringValue(row, "data");
                returnValue.flgMarkAsRead = DBUtil.GetBooleanValue(row, "flg_mark_as_read");
                returnValue.UserId = DBUtil.GetIntValue(row, "user_id");
                returnValue.CreatedBy = DBUtil.GetIntValue(row, "created_by");
                returnValue.DateCreated = DBUtil.GetDateTimeValue(row, "date_created");
                returnValue.LastAmendedBy = DBUtil.GetIntValue(row, "last_amended_by");
                returnValue.DateLastAmended = DBUtil.GetDateTimeValue(row, "date_last_amended");


            }
            return returnValue;
        }
    }
}
