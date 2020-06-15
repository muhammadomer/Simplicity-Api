using System;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class CldSettingsQueries
    {
        public static string getSelectAll(string databaseType)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT * 
                                      FROM un_cld_settings";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllForSmartSetting(string databaseType)
        {   
            string returnValue = @"SELECT * 
                                      FROM un_cld_settings
                                      Where flg_smart_setting = " + Utilities.GetBooleanForDML(databaseType, true);
           
            return returnValue;
        }

        
        public static string getSelectAllBysettingName(string databaseType, string settingName)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = "SELECT * " +
                             "  FROM un_cld_settings" +
                             " WHERE setting_name = '" + settingName + "'";
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, string settingName, string settingValue)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "INSERT INTO un_cld_settings(setting_name, setting_value) " +
                                      "VALUES ('" + settingName + "', '" + settingValue + "')";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string update(string databaseType, string settingName, string settingValue)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "UPDATE un_cld_settings " +
                              "   SET setting_name = '" + settingName + "'," +
                              "       setting_value = '" + settingValue + "' " +
                              "WHERE setting_name = " + settingName;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string delete(string databaseType, long settingName)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "DELETE FROM un_cld_settings " +
                              "WHERE setting_name = " + settingName;
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    
    }
}

