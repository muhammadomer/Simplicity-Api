using System;
using SimplicityOnlineWebApi.Commons;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class UserSessionsQueries
    {
        public static string getSelectAllByUserIdAndToken(string databaseType, long userId, string token)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "SELECT * " +
                            "  FROM un_user_sessions" +
                            " WHERE user_id = " + userId +
                            "   AND token = '" + token + "'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string token, long userId, string userName, string projectId, DateTime? tokenExpiry, string databaseType)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "INSERT INTO un_user_sessions (token, user_id, user_name, projectid, token_expiry) " +
                                  "VALUES ('" + token + "', " + userId + ",'" + userName + "', '" + projectId + "', " +
                                          Utilities.GetDateTimeForDML(databaseType, tokenExpiry,true,true) + ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string updateTokenExpiry(string token, long userId, DateTime? tokenExpiry, string databaseType)
        {
            string returnValue = "";
            try
            {   
                returnValue = "UPDATE un_user_sessions " +
                        "   SET token_expiry =  " + Utilities.GetDateTimeForDML(databaseType, tokenExpiry, true, true) +
                        " WHERE token = '" + token + "'" +
                        "   AND user_id = " + userId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

