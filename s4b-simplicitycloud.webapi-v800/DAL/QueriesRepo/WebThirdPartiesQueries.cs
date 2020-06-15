using SimplicityOnlineWebApi.Commons;
using System;
using SimplicityOnlineBLL.Entities;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class WebThirdPartiesQueries
    {
        public static string SelectAllFieldsByUserNameAndEnable(string databaseType, string userLogon, string userEnable)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "SELECT * " +
                                "  FROM un_web_3rd_parties " +
                                " WHERE user_logon = '" + userLogon.Trim() + "'" +
                                "   AND web_enable='" + Utilities.strUFFAFU(userEnable) + "'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        
        public static string SelectAllUsers(string databaseType)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "SELECT * " +
                        "  FROM un_web_3rd_parties " +
                        " WHERE flg_deleted <> " + Utilities.GetBooleanForDML(databaseType,true);
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string GetAllByEntityId(string databaseType,long EntityId)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                        "  FROM un_web_3rd_parties " +
                        " WHERE entity_id= "+ EntityId + " and flg_deleted = " + Utilities.GetBooleanForDML(databaseType, false);
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string SelectAllUsersByUserName(string databaseType, string userName)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                        "  FROM un_web_3rd_parties " +
                        " WHERE user_logon like '%" + userName + "%'" +
                        "   AND flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string GetUsersById(string databaseType, int userId)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "SELECT * " +
                        "  FROM un_web_3rd_parties " +
                        " WHERE web_id = "+ userId + "" +
                        "   AND flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
                        
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string CreateUser(string databaseType, WebThirdParties userName)
        {
         
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO un_web_3rd_parties(entity_id, web_level, flg_deleted, user_name,  user_logon, web_enable, " +
                                "       web_telephone, web_tel_ext, web_tel_mobile, web_company, web_department, web_location) " +
                                "VALUES (" + userName.EntityId + ",'" + userName.WebLevel + "'," + Utilities.GetBooleanForDML(databaseType, userName.FlgDeleted) + ", " +
                                "      '" + userName.UserName + "','" + userName.UserLogon + "','" + Utilities.strUFFAFU(userName.WebEnable) + "', " +
                                "      '" + userName.WebTelephone + "', '" + userName.WebTelExt + "','" + userName.WebTelMobile + "','" + userName.WebCompany + "', " +
                                "      '" + userName.WebCompany + "','" + userName.WebLocation + "')";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string UpdateUser(string databaseType, WebThirdParties userName)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_web_3rd_parties set web_level = '" + userName.WebLevel + "', flg_deleted = " + Utilities.GetBooleanForDML(databaseType, userName.FlgDeleted) + ", " +
                                "       user_name = '" + userName.UserName + "', user_logon = '" + userName.UserLogon + "', " + 
                                "       web_enable = '" + Utilities.strUFFAFU(userName.WebEnable) + "', " +
                                "       web_tel_ext = '" + userName.WebTelExt + "', web_telephone='" + userName.WebTelephone + "', " +
                                "       web_tel_mobile = '"+userName.WebTelMobile+"'," + 
                                "       web_company = '" + userName.WebCompany + "', web_department= '" + userName.WebDepartment + "' " +
                                " WHERE web_id = " + userName.WebId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string UpdateUserPassword(string databaseType, WebThirdParties userName)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_web_3rd_parties set web_enable = '" + Utilities.strUFFAFU(userName.WebEnableNew) + "', " +
                                "       flg_reset = " + Utilities.GetBooleanForDML(databaseType, false) + ", " +
                                "       last_amended_by = " + userName.LastAmendedBy + ", " +
                                "       date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, userName.DateLastAmended,true,true) + ", " +
                                " WHERE web_id = " + userName.WebId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string CheckUser(string databaseType, string userLogon)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                        "  FROM un_web_3rd_parties " +
                        " WHERE user_logon like '%" + userLogon + "%'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string EnableOrDisabled(string databaseType, WebThirdParties userName)
        {
            string returnValue = "";
            try
            {
                returnValue = "update un_web_3rd_parties set flg_deleted = " + Utilities.GetBooleanForDML(databaseType, userName.FlgDeleted) +
                        " WHERE web_id = "+userName.WebId+ "";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}
