using System;
using SimplicityOnlineWebApi.Commons;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class ApplicationLogOnsQueries
    { 

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                        "  FROM un_application_log_ons" +
                        " WHERE sequence = " + Sequence;
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string insert(string databaseType, long  userId, DateTime userLogOnTime,bool flgUserLogOff, bool flgReset, 
                                   DateTime  userLogOffTime, long userProcessId, string userIpAddress)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "INSERT INTO un_application_log_ons(user_id, user_log_on_time, flg_user_log_off, flg_reset, user_log_off_time, user_process_id, user_ip_address) " +
                        "VALUES (" + userId + ", " + Utilities.GetDateTimeForDML(databaseType, userLogOnTime,true,true) + ", " + Utilities.GetBooleanForDML(databaseType, flgUserLogOff) + ", " + Utilities.GetBooleanForDML(databaseType, flgReset) + ", " + Utilities.GetDateTimeForDML(databaseType, userLogOffTime,true,true) + ", " + userProcessId + ", '" + userIpAddress + "')";
                        
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

       public static string update(string databaseType, long sequence, long userId, DateTime userLogOnTime, bool flgUserLogOff, bool flgReset,
                                  DateTime userLogOffTime, long userProcessId, string userIpAddress)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_application_log_ons " +
                        "       SET user_id =  " + userId + "," +
                        "       user_log_on_time = " + Utilities.GetDateTimeForDML(databaseType, userLogOnTime,true,true) + "," +
                        "       flg_user_log_off =  " + Utilities.GetBooleanForDML(databaseType, flgUserLogOff) + "," +
                        "       flg_reset = " + Utilities.GetBooleanForDML(databaseType, flgReset) + "," +
                        "       user_log_off_time = " + Utilities.GetDateTimeForDML(databaseType, userLogOffTime,true,true) + "," +
                        "       user_process_id = " + userProcessId + "," +
                        "       user_ip_address = '" + userIpAddress + "'," +
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
                returnValue = " DELETE FROM un_application_log_ons" +
                             " WHERE sequence = " + sequence;
                
           }
           catch (Exception ex)
           {
           }
           return returnValue;
        }
    
    }
}

