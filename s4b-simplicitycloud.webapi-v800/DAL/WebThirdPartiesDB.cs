using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class WebThirdPartiesDB : MainDB
    {
        public WebThirdPartiesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public WebThirdParties getUserByUserLogonAndEnable(string userName, string userEnable)
        {
            WebThirdParties returnValue = null;
            ErrorMessage = "";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(WebThirdPartiesQueries.SelectAllFieldsByUserNameAndEnable(this.DatabaseType, userName, userEnable), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = LoadWebThirdParties(dr);
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

        public List<WebThirdParties> getAllUsers()
        {
            List<WebThirdParties> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(WebThirdPartiesQueries.SelectAllUsers(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<WebThirdParties>();
                                while(dr.Read())
                                { 
                                    returnValue.Add(LoadWebThirdParties(dr));
                                }
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
        public List<WebThirdParties> getAllByEntity(long EntityId)
        {
            List<WebThirdParties> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(WebThirdPartiesQueries.GetAllByEntityId(this.DatabaseType, EntityId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<WebThirdParties>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadWebThirdParties(dr));
                                }
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


        internal List<WebThirdParties> GetUsersByUserName(string userName)
        {
            List<WebThirdParties> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(WebThirdPartiesQueries.SelectAllUsersByUserName(this.DatabaseType , userName), conn))
                    {   
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<WebThirdParties>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadWebThirdParties(dr));
                                }
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
        internal WebThirdParties GetUsersByUserId(int userId)
        {
            WebThirdParties returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(WebThirdPartiesQueries.GetUsersById(this.DatabaseType, userId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new WebThirdParties();
                                while (dr.Read())
                                {
                                    returnValue = LoadWebThirdParties(dr);
                                    returnValue.WebEnable = Utilities.strUFFAFU(returnValue.WebEnable);
                                }
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
        public WebThirdParties AddUser(WebThirdParties user)
        {
            WebThirdParties returnUser = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(WebThirdPartiesQueries.CreateUser(this.DatabaseType, user), conn))
                    {
                        objCmdSelect.ExecuteNonQuery();
                        string sql = "select @@IDENTITY";
                        using (OleDbCommand objCommand =
                            new OleDbCommand(sql, conn))
                        {
                            OleDbDataReader dr = objCommand.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                user.WebId = Int32.Parse(dr[0].ToString());
                            }
                            else
                            {
                                //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record.                                                 '" + METHOD_NAME + "'\n";
                            }
                        }
                        returnUser = user;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: Needs logging
            }
            return returnUser;
        }

        public WebThirdParties UpdateUser(WebThirdParties user)
        {
            WebThirdParties returnUser = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(WebThirdPartiesQueries.UpdateUser(this.DatabaseType, user), conn))
                    {

                        objCmdSelect.ExecuteNonQuery();
                        returnUser = user;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: Needs logging
            }
            return returnUser;
        }

        public WebThirdParties UpdateUserPassword(WebThirdParties user)
        {
            WebThirdParties returnUser = null;
            ErrorMessage = "";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(WebThirdPartiesQueries.UpdateUserPassword(this.DatabaseType, user), conn))
                    {
                        objCmdSelect.ExecuteNonQuery();
                        user.WebEnable = user.WebEnableNew;
                        returnUser = user;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception Occured While Updating User Password. " + ex.Message;
            }
            return returnUser;
        }

        public List<WebThirdParties> CheckUserAsUniq(string user)
        {
            List<WebThirdParties> returnUser = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(WebThirdPartiesQueries.CheckUser(this.DatabaseType, user), conn))
                    {

                        OleDbDataReader dr = objCmdSelect.ExecuteReader();
                        if (dr.HasRows)
                        {
                            returnUser = new List<WebThirdParties>();
                            while (dr.Read())
                            {
                                returnUser.Add(LoadWebThirdParties(dr));
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Exception Occured While CheckUserAsUniq. " + ex.Message;
            }
            return returnUser;
        }
        public bool DeleteUser(WebThirdParties user)
        {
            bool result = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(WebThirdPartiesQueries.EnableOrDisabled(this.DatabaseType, user), conn))
                    {

                        int res = objCmdSelect.ExecuteNonQuery();
                        if (res > 0)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        WebThirdParties LoadWebThirdParties(OleDbDataReader dr)
        {
            WebThirdParties returnValue = null;
            try
            {
                if (dr != null)
                {
                    WebThirdParties webThirdParty = new WebThirdParties();
                    webThirdParty.WebId = Int32.Parse(dr["web_id"].ToString());
                    webThirdParty.EntityId = long.Parse(dr["entity_id"].ToString());
                    webThirdParty.WebLevel = Int32.Parse(dr["web_level"].ToString());
                    webThirdParty.FlgDeleted = (bool)dr["flg_deleted"];
                    webThirdParty.UserName = Utilities.GetDBString(dr["user_name"]);
                    webThirdParty.UserLogon = Utilities.GetDBString(dr["user_logon"]);
                    webThirdParty.FlgReset = (bool)dr["flg_reset"];
                    webThirdParty.WebEnable = Utilities.GetDBString(dr["web_enable"]);
                    webThirdParty.WebTelephone = Utilities.GetDBString(dr["web_telephone"]);
                    webThirdParty.WebTelExt = Utilities.GetDBString(dr["web_tel_ext"]);
                    webThirdParty.WebTelMobile = Utilities.GetDBString(dr["web_tel_mobile"]);
                    webThirdParty.WebCompany = Utilities.GetDBString(dr["web_company"]);
                    webThirdParty.WebDepartment = Utilities.GetDBString(dr["web_department"]);
                    webThirdParty.WebLocation = Utilities.GetDBString(dr["web_location"]);
                    returnValue = webThirdParty;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}
