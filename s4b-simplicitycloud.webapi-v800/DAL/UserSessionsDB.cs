using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class UserSessionsDB : MainDB
    {
        public UserSessionsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertUserSession(string token, int userId, string userName, string projectId,
                                      DateTime? tokenExpiry)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert = 
                        new OleDbCommand(UserSessionsQueries.insert(token, userId, userName,  projectId, tokenExpiry,this.DatabaseType), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while inserting into User Sessions " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public UserSessions selectUserSession(int userId, string token)
        {
            UserSessions returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserSessionsQueries.getSelectAllByUserIdAndToken(this.DatabaseType,userId, token), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                UserSessions userSession = new UserSessions();
                                userSession.TokenExpiry = DateTime.Parse(dr["token_expiry"].ToString());
                                userSession.Token = (string)dr["token"];
                                userSession.ProjectId = (string)dr["projectid"];
                                userSession.UserId = Convert.ToInt32(dr["user_id"].ToString());
                                userSession.UserName = (string)dr["user_name"];
                                userSession.FlgAdmin = Boolean.Parse(dr["flg_admin"].ToString());
                                returnValue = userSession;
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

        internal bool updateUserSessionExpiry(string token, int userId, DateTime? newTokenExpiry)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(UserSessionsQueries.updateTokenExpiry(token, userId, newTokenExpiry, this.DatabaseType), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
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
    }
}
