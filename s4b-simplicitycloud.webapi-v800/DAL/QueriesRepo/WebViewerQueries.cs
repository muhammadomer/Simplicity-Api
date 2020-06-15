using SimplicityOnlineWebApi.Commons;
using System;
using SimplicityOnlineBLL.Entities;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class WebViewerQueries
    {
        public static string SelectAllFieldsByUserNameAndEnable(string databaseType, string userLogon, string userEnable)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                                "  FROM un_web_viewer " +
                                " WHERE viewer_logon = '" + userLogon.Trim() + "'" +
                                "   AND viewer_enable='" + Utilities.strUFFAFU(userEnable) + "' " +
                                "   AND flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
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
                        "  FROM un_web_viewer " +
                        " WHERE flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string GetWebViewerAssignedToByViewerId(string databaseType, long ViewerId)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                                "  FROM un_web_viewer_assign_to " +
                                " WHERE viewer_id = " + ViewerId +
                                " and flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        //public static string GetAllByEntityId(string databaseType, long EntityId)
        //{
        //    string returnValue = "";
        //    try
        //    {
        //        switch (databaseType)
        //        {
        //            case "MSACCESS":
        //                returnValue = "SELECT * " +
        //                      "  FROM un_web_viewer " +
        //                      " WHERE entity_id="+EntityId+" and flg_deleted = false";
        //                break;

        //            case "SQLSERVER":
        //                returnValue = "SELECT * " +
        //                    "  FROM un_web_viewer " +
        //                    " WHERE entity_id=" + EntityId + " and flg_deleted=0 ";
        //                break;

        //            default:
        //                returnValue = "SELECT * " +
        //                    "  FROM un_web_viewer " +
        //                     " WHERE entity_id=" + EntityId + " and flg_deleted=0 ";
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return returnValue;
        //}


        //public static string SelectAllUsersByUserName(string databaseType, string userName)
        //{
        //    string returnValue = "";
        //    try
        //    {
        //        switch (databaseType)
        //        {
        //            case "MSACCESS":
        //                returnValue = "SELECT * " +
        //                      "  FROM un_web_viewer " +
        //                      " WHERE viewer_logon like '%" + userName + "%'" +
        //                      "   AND flg_deleted <> 1";
        //                break;

        //            case "SQLSERVER":
        //            default:
        //                returnValue = "SELECT * " +
        //                     "  FROM un_web_viewer " +
        //                     " WHERE viewer_logon like '%" + userName + "%'" +
        //                     "   AND flg_deleted <> 1";
        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return returnValue;
        //}
        //public static string GetUsersById(string databaseType, int userId)
        //{
        //    string returnValue = "";
        //    try
        //    {
        //        switch (databaseType)
        //        {
        //            case "MSACCESS":
        //                returnValue = "SELECT * " +
        //                      "  FROM un_web_viewer " +
        //                      " WHERE web_id = "+ userId + "" +
        //                      "   AND flg_deleted <> 1";
        //                break;

        //            case "SQLSERVER":
        //            default:
        //                returnValue = "SELECT * " +
        //                     "  FROM un_web_viewer " +
        //                      " WHERE web_id = " + userId + "" +
        //                     "   AND flg_deleted <> 1";
        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return returnValue;
        //}
        //public static string CreateUser(string databaseType, WebThirdParties userName)
        //{

        //    string returnValue = "";
        //    try
        //    {
        //        switch (databaseType)
        //        {
        //            case "MSACCESS":
        //            case "SQLSERVER":
        //            default:
        //                returnValue = "INSERT INTO un_web_viewer(entity_id, web_level, flg_deleted, user_name,  viewer_logon, viewer_enable, " +
        //                              "       web_telephone, web_tel_ext, web_tel_mobile, web_company, web_department, web_location) " +
        //                              "VALUES (" + userName.EntityId + ",'" + userName.WebLevel + "'," + userName.FlgDeleted + ", " + 
        //                              "      '" + userName.UserName + "','" + userName.UserLogon + "','" + Utilities.strUFFAFU(userName.WebEnable) + "', " + 
        //                              "      '" + userName.WebTelephone + "', '" + userName.WebTelExt + "','" + userName.WebTelMobile + "','" + userName.WebCompany + "', " +
        //                              "      '" + userName.WebCompany + "','" + userName.WebLocation + "')";
        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return returnValue;
        //}
        //public static string UpdateUser(string databaseType, WebThirdParties userName)
        //{
        //    string returnValue = "";
        //    try
        //    {
        //        switch (databaseType)
        //        {
        //            case "MSACCESS":
        //            case "SQLSERVER":
        //            default:
        //                returnValue = "UPDATE un_web_viewer set web_level = '" + userName.WebLevel + "', flg_deleted = " + userName.FlgDeleted + ", " +
        //                              "       user_name = '" + userName.UserName + "', viewer_logon = '" + userName.UserLogon + "', " + 
        //                              "       viewer_enable = '" + Utilities.strUFFAFU(userName.WebEnable) + "', " +
        //                              "       web_tel_ext = '" + userName.WebTelExt + "', web_telephone='" + userName.WebTelephone + "', " +
        //                              "       web_tel_mobile = '"+userName.WebTelMobile+"'," + 
        //                              "       web_company = '" + userName.WebCompany + "', web_department= '" + userName.WebDepartment + "' " +
        //                              " WHERE web_id = " + userName.WebId;
        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return returnValue;
        //}

        //public static string UpdateUserPassword(string databaseType, WebThirdParties userName)
        //{
        //    string returnValue = "";
        //    try
        //    {
        //        switch (databaseType)
        //        {
        //            case "MSACCESS":
        //                returnValue = "UPDATE un_web_viewer set viewer_enable = '" + Utilities.strUFFAFU(userName.WebEnableNew) + "', " +
        //                              "       flg_reset = " + false + ", " +
        //                              "       last_amended_by = " + userName.LastAmendedBy + ", " +
        //                              "       date_last_amended =  " + Utilities.getAccessDate(userName.DateLastAmended) + ", " +
        //                              " WHERE web_id = " + userName.WebId;
        //                break;
        //            case "SQLSERVER":
        //            default:
        //                returnValue = "UPDATE un_web_viewer set viewer_enable = '" + Utilities.strUFFAFU(userName.WebEnableNew) + "', " +
        //                              "       flg_reset = " + 0 + ", " +
        //                              "       last_amended_by = " + userName.LastAmendedBy + ", " +
        //                              "       date_last_amended =  " + Utilities.getSQLDate(userName.DateLastAmended) + ", " +
        //                              " WHERE web_id = " + userName.WebId;
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return returnValue;
        //}

        //public static string CheckUser(string databaseType, string userLogon)
        //{
        //    string returnValue = "";
        //    try
        //    {
        //        switch (databaseType)
        //        {
        //            case "MSACCESS":
        //                returnValue = "SELECT * " +
        //                     "  FROM un_web_viewer " +
        //                     " WHERE viewer_logon like '%" + userLogon + "%'";
        //                break;

        //            case "SQLSERVER":
        //                returnValue = "SELECT * " +
        //                     "  FROM un_web_viewer " +
        //                     " WHERE viewer_logon like '%" + userLogon + "%'";
        //                break;
        //            default:
        //                returnValue = "SELECT * " +
        //                     "  FROM un_web_viewer " +
        //                     " WHERE viewer_logon like '%" + userLogon + "%'";
        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return returnValue;
        //}

        //public static string EnableOrDisabled(string databaseType, WebThirdParties userName)
        //{
        //    string returnValue = "";
        //    try
        //    {
        //        switch (databaseType)
        //        {
        //            case "MSACCESS":
        //                returnValue = "update un_web_viewer set flg_deleted = " + userName.FlgDeleted + ""+
        //                     " WHERE web_id = "+userName.WebId+ "";
        //                break;

        //            case "SQLSERVER":
        //                returnValue = "update un_web_viewer set flg_deleted = " + userName.FlgDeleted + "" +
        //                     " WHERE web_id = " + userName.WebId + "";
        //                break;
        //            default:
        //                returnValue = "update un_web_viewer set flg_deleted = " + userName.FlgDeleted + "" +
        //                      " WHERE web_id = " + userName.WebId + "";
        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return returnValue;
        //}
    }
}
