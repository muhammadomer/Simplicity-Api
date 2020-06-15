using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class WebViewerDB : MainDB
    {
        public WebViewerDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public WebViewer getUserByUserLogonAndEnable(string userName, string userEnable)
        {
            WebViewer returnValue = null;
            ErrorMessage = "";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(WebViewerQueries.SelectAllFieldsByUserNameAndEnable(this.DatabaseType, userName, userEnable), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadWebViewer(dr, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while getting User details. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public WebViewerAssignedTo getWebViewerAssignedToByViewerId(long ViewerId)
        {
            WebViewerAssignedTo returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(WebViewerQueries.GetWebViewerAssignedToByViewerId(this.DatabaseType, ViewerId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadWebViewerAssignedTo(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while getting User details. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        //public List<WebViewer> getAllUsers()
        //{
        //    List<WebViewer> returnValue = null;
        //    try
        //    {
        //        using (OleDbConnection conn = this.getDbConnection())
        //        {
        //            using (OleDbCommand objCmdSelect =
        //                new OleDbCommand(WebViewerQueries.SelectAllUsers(this.DatabaseType), conn))
        //            {
        //                using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
        //                {
        //                    if (dr.HasRows)
        //                    {
        //                        returnValue = new List<WebViewer>();
        //                        while(dr.Read())
        //                        { 
        //                            returnValue.Add(LoadWebViewer(dr));
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = "Error occured while getting User details. " + ex.Message + " " + ex.InnerException;
        //        // Requires Logging
        //    }
        //    return returnValue;
        //}
        //public List<WebViewer> getAllByEntity(long EntityId)
        //{
        //    List<WebViewer> returnValue = null;
        //    try
        //    {
        //        using (OleDbConnection conn = this.getDbConnection())
        //        {
        //            using (OleDbCommand objCmdSelect =
        //                new OleDbCommand(WebViewerQueries.GetAllByEntityId(this.DatabaseType, EntityId), conn))
        //            {
        //                using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
        //                {
        //                    if (dr.HasRows)
        //                    {
        //                        returnValue = new List<WebViewer>();
        //                        while (dr.Read())
        //                        {
        //                            returnValue.Add(LoadWebViewer(dr));
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = "Error occured while getting User details. " + ex.Message + " " + ex.InnerException;
        //        // Requires Logging
        //    }
        //    return returnValue;
        //}


        //internal List<WebViewer> GetUsersByUserName(string userName)
        //{
        //    List<WebViewer> returnValue = null;
        //    try
        //    {
        //        using (OleDbConnection conn = this.getDbConnection())
        //        {
        //            using (OleDbCommand objCmdSelect =
        //                new OleDbCommand(WebViewerQueries.SelectAllUsersByUserName(this.DatabaseType , userName), conn))
        //            {   
        //                using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
        //                {
        //                    if (dr.HasRows)
        //                    {
        //                        returnValue = new List<WebViewer>();
        //                        while (dr.Read())
        //                        {
        //                            returnValue.Add(LoadWebViewer(dr));
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = "Error occured while getting User details. " + ex.Message + " " + ex.InnerException;
        //        // Requires Logging
        //    }
        //    return returnValue;
        //}
        //internal WebViewer GetUsersByUserId(int userId)
        //{
        //    WebViewer returnValue = null;
        //    try
        //    {
        //        using (OleDbConnection conn = this.getDbConnection())
        //        {
        //            using (OleDbCommand objCmdSelect =
        //                new OleDbCommand(WebViewerQueries.GetUsersById(this.DatabaseType, userId), conn))
        //            {
        //                using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
        //                {
        //                    if (dr.HasRows)
        //                    {
        //                        returnValue = new WebViewer();
        //                        while (dr.Read())
        //                        {
        //                            returnValue = LoadWebViewer(dr);
        //                            returnValue.WebEnable = Utilities.strUFFAFU(returnValue.WebEnable);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = "Error occured while getting User details. " + ex.Message + " " + ex.InnerException;
        //        // Requires Logging
        //    }
        //    return returnValue;
        //}
        //public WebViewer AddUser(WebViewer user)
        //{
        //    WebViewer returnUser = null;
        //    try
        //    {
        //        using (OleDbConnection conn = this.getDbConnection())
        //        {
        //            using (OleDbCommand objCmdSelect =
        //                new OleDbCommand(WebViewerQueries.CreateUser(this.DatabaseType, user), conn))
        //            {
        //                objCmdSelect.ExecuteNonQuery();
        //                string sql = "select @@IDENTITY";
        //                using (OleDbCommand objCommand =
        //                    new OleDbCommand(sql, conn))
        //                {
        //                    OleDbDataReader dr = objCommand.ExecuteReader();
        //                    if (dr.HasRows)
        //                    {
        //                        dr.Read();
        //                        user.WebId = Int32.Parse(dr[0].ToString());
        //                    }
        //                    else
        //                    {
        //                        //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record.                                                 '" + METHOD_NAME + "'\n";
        //                    }
        //                }
        //                returnUser = user;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //TODO: Needs logging
        //    }
        //    return returnUser;
        //}

        //public WebViewer UpdateUser(WebViewer user)
        //{
        //    WebViewer returnUser = null;
        //    try
        //    {
        //        using (OleDbConnection conn = this.getDbConnection())
        //        {
        //            using (OleDbCommand objCmdSelect =
        //                new OleDbCommand(WebViewerQueries.UpdateUser(this.DatabaseType, user), conn))
        //            {

        //                objCmdSelect.ExecuteNonQuery();
        //                returnUser = user;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //TODO: Needs logging
        //    }
        //    return returnUser;
        //}

        //public WebViewer UpdateUserPassword(WebViewer user)
        //{
        //    WebViewer returnUser = null;
        //    ErrorMessage = "";
        //    try
        //    {
        //        using (OleDbConnection conn = this.getDbConnection())
        //        {
        //            using (OleDbCommand objCmdSelect =
        //                new OleDbCommand(WebViewerQueries.UpdateUserPassword(this.DatabaseType, user), conn))
        //            {
        //                objCmdSelect.ExecuteNonQuery();
        //                user.WebEnable = user.WebEnableNew;
        //                returnUser = user;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = "Exception Occured While Updating User Password. " + ex.Message;
        //    }
        //    return returnUser;
        //}

        //public List<WebViewer> CheckUserAsUniq(string user)
        //{
        //    List<WebViewer> returnUser = null;
        //    try
        //    {
        //        using (OleDbConnection conn = this.getDbConnection())
        //        {
        //            using (OleDbCommand objCmdSelect =
        //                new OleDbCommand(WebViewerQueries.CheckUser(this.DatabaseType, user), conn))
        //            {

        //                OleDbDataReader dr = objCmdSelect.ExecuteReader();
        //                if (dr.HasRows)
        //                {
        //                    returnUser = new List<WebViewer>();
        //                    while (dr.Read())
        //                    {
        //                        returnUser.Add(LoadWebViewer(dr));
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = "Exception Occured While CheckUserAsUniq. " + ex.Message;
        //    }
        //    return returnUser;
        //}
        //public bool DeleteUser(WebViewer user)
        //{
        //    bool result = false;
        //    try
        //    {
        //        using (OleDbConnection conn = this.getDbConnection())
        //        {
        //            using (OleDbCommand objCmdSelect =
        //                new OleDbCommand(WebViewerQueries.EnableOrDisabled(this.DatabaseType, user), conn))
        //            {

        //                int res = objCmdSelect.ExecuteNonQuery();
        //                if (res > 0)
        //                {
        //                    result = true;
        //                }
        //                else
        //                {
        //                    result = false;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = false;
        //    }
        //    return result;
        //}

        WebViewer LoadWebViewer(OleDbDataReader dr, bool loadAssignedTo)
        {
            WebViewer returnValue = null;
            try
            {
                if (dr != null)
                {
                    WebViewer webViewer = new WebViewer();
                    webViewer.ViewerId = Int32.Parse(dr["viewer_id"].ToString());
                    webViewer.ViewerLevel = Int32.Parse(dr["viewer_level"].ToString());
                    webViewer.FlgDeleted = (bool)dr["flg_deleted"];
                    webViewer.ViewerName = Utilities.GetDBString(dr["viewer_name"]);
                    webViewer.ViewerLogon = Utilities.GetDBString(dr["viewer_logon"]);
                    webViewer.FlgReset = (bool)dr["flg_reset"];
                    webViewer.ViewerEnable = Utilities.GetDBString(dr["viewer_enable"]);
                    webViewer.ViewerTelephone = Utilities.GetDBString(dr["viewer_telephone"]);
                    webViewer.ViewerTelExt = Utilities.GetDBString(dr["viewer_tel_ext"]);
                    webViewer.ViewerTelMobile = Utilities.GetDBString(dr["viewer_tel_mobile"]);
                    webViewer.ViewerCompany = Utilities.GetDBString(dr["viewer_company"]);
                    webViewer.ViewerDepartment = Utilities.GetDBString(dr["viewer_department"]);
                    webViewer.ViewerLocation = Utilities.GetDBString(dr["viewer_location"]);
                    webViewer.FlgTimesheets = (bool)dr["flg_timesheets"];
                    if(loadAssignedTo)
                    { 
                        webViewer.AssignedTo = getWebViewerAssignedToByViewerId(webViewer.ViewerId);
                    }
                    returnValue = webViewer;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        WebViewerAssignedTo LoadWebViewerAssignedTo(OleDbDataReader dr)
        {
            WebViewerAssignedTo returnValue = null;
            try
            {
                if (dr != null)
                {
                    WebViewerAssignedTo webViewerAssignedTo = new WebViewerAssignedTo();
                    webViewerAssignedTo.Sequence = long.Parse(dr["sequence"].ToString());
                    webViewerAssignedTo.FlgDeleted = (bool)dr["flg_deleted"];
                    webViewerAssignedTo.ViewerId = Int32.Parse(dr["viewer_id"].ToString());
                    webViewerAssignedTo.EntityId = long.Parse(dr["entity_id"].ToString());
                    webViewerAssignedTo.DefaultJobAddress = long.Parse(dr["entity_id"].ToString());
                    webViewerAssignedTo.DefaultJobStatus = Int32.Parse(dr["default_job_status"].ToString());
                    webViewerAssignedTo.DefaultJobManager = long.Parse(dr["default_job_manager"].ToString());
                    returnValue = webViewerAssignedTo;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

    }
}
