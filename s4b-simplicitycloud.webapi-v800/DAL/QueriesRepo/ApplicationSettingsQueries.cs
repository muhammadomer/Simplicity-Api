using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class ApplicationSettingsQueries
    {

        public static string selectAll(string databaseType)
        {
            string returnValue = "";
            try
            {
                
                returnValue = @"SELECT * FROM un_application_settings ";

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllBySettingId(string databaseType, string  settingId)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "SELECT * " +
                                "  FROM un_application_settings " +
                                " WHERE setting_id = '" + settingId + "'";
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, string settingId, string setting1, string setting2, string setting3,
                                    string setting4)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "INSERT INTO un_application_settings(setting_id, setting_1, setting_2, setting_3, setting_4) " +
                        "VALUES ('" + settingId + "', '" + setting1 + "', '" + setting2 + "', '" + setting3 + "', '" + setting4 + "')";
                             
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string update(string databaseType, string settingId, string setting1, string setting2, string setting3,
                                    string setting4)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_application_settings " +
                                "   SET setting_1 = '" + setting1 + "', " +
                                "       setting_2 = '" + setting2 + "', " +
                                "       setting_3 = '" + setting3 + "', " +
                                "       setting_4 = '" + setting4 + "', " +
                                " WHERE setting_id = '" + settingId + "'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string delete(string databaseType, long settingId)
        {
            string returnValue = "";
            try
            {
                returnValue = "DELETE FROM un_application_settings " +
                                      " WHERE setting_id = '" + settingId + "'";   
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string updateSetting1BySettingId(string databaseType, string settingId, string setting1)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_application_settings " +
                    "   SET setting_1 = '" + setting1 + "' " +
                    " WHERE setting_id = '" + settingId + "'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

