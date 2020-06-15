using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
namespace SimplicityOnlineWebApi.DAL
{
    public class UserDetailsDB : MainDB
    {
        public UserDetailsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public UserDetails getUserByUserLogonAndEnable(string userName, string userEnable)
        {
            UserDetails returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserDetailsQueries.SelectAllFieldsByUserNameAndEnable(this.DatabaseType, userName, userEnable), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                UserDetails userDetail = new UserDetails();
                                userDetail.UserId = Int32.Parse(dr["user_id"].ToString());
                                userDetail.UserName = Utilities.GetDBString(dr["user_name"]);
                                userDetail.UserLogon = Utilities.GetDBString(dr["user_logon"]);
                                userDetail.UserTelephone = Utilities.GetDBString(dr["user_telephone"]);
                                userDetail.UserEmail = Utilities.GetDBString(dr["user_email"]);
                                userDetail.FlgDeleted = Boolean.Parse(dr["flg_deleted"].ToString());
                                returnValue = userDetail;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while getting User details. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public UserDetails getUserByUserId(int userId)
        {
            UserDetails returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserDetailsQueries.SelectAllFieldsByUserId(this.DatabaseType, userId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                UserDetails userDetail = new UserDetails();
                                userDetail.UserId = Int32.Parse(dr["user_id"].ToString());
                                userDetail.UserName = Utilities.GetDBString(dr["user_name"]);
                                userDetail.UserLogon = Utilities.GetDBString(dr["user_logon"]);
                                userDetail.UserTelephone = Utilities.GetDBString(dr["user_telephone"]);
                                userDetail.UserEmail = Utilities.GetDBString(dr["user_email"]);
                                userDetail.FlgDeleted = Boolean.Parse(dr["flg_deleted"].ToString());
                                returnValue = userDetail;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while getting User details. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        internal List<UserDetails> getAllUsers()
        {
            List<UserDetails> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserDetailsQueries.SelectAllUsers(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<UserDetails>();
                                while(dr.Read())
                                { 
                                    UserDetails userDetail = new UserDetails();
                                    userDetail.UserId = Int32.Parse(dr["user_id"].ToString());
                                    userDetail.UserName = Utilities.GetDBString(dr["user_name"]);
                                    userDetail.UserLogon = Utilities.GetDBString(dr["user_logon"]);
                                    userDetail.UserTelephone = Utilities.GetDBString(dr["user_telephone"]);
                                    userDetail.UserEmail = Utilities.GetDBString(dr["user_email"]);
                                    userDetail.FlgDeleted = Boolean.Parse(dr["flg_deleted"].ToString());
                                    returnValue.Add(userDetail);
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

        internal List<UserDetails> GetUsersByUserName(string userName)
        {
            List<UserDetails> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserDetailsQueries.SelectAllUsersByUserName(this.DatabaseType , userName), conn))
                    {
                        
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<UserDetails>();
                                while (dr.Read())
                                {
                                    UserDetails userDetail = new UserDetails();
                                    userDetail.UserId = Int32.Parse(dr["user_id"].ToString());
                                    userDetail.UserName = Utilities.GetDBString(dr["user_name"]);
                                    userDetail.UserLogon = Utilities.GetDBString(dr["user_logon"]);
                                    userDetail.UserTelephone = Utilities.GetDBString(dr["user_telephone"]);
                                    userDetail.UserEmail = Utilities.GetDBString(dr["user_email"]);

                                    userDetail.UserLevel = Int32.Parse(dr["user_level"].ToString());
                                    userDetail.UserTelExt = Utilities.GetDBString(dr["user_tel_ext"]);
                                    userDetail.UserEnableReminder = Utilities.GetDBString(dr["user_enable_reminder"]);
                                    userDetail.UserTelMobile = Utilities.GetDBString(dr["user_tel_mobile"]);
                                    userDetail.UserEmailLogon = Utilities.GetDBString(dr["user_email_logon"]);
                                    userDetail.UserEmailEnable = Utilities.GetDBString(dr["user_email_enable"]);
                                    userDetail.UserEmailSMTP = Utilities.GetDBString(dr["user_email_smtp"]);
                                    userDetail.UserEmailHTMLFile = Utilities.GetDBString(dr["user_email_html_file"]);
                                    userDetail.UserLocation = Utilities.GetDBString(dr["user_location"]);
                                    userDetail.UserDepartment = Utilities.GetDBString(dr["user_department"]);
                                    userDetail.ResourceSequence = Int32.Parse(dr["resource_sequence"].ToString());
                                    userDetail.UserEmailSSL = Int32.Parse(dr["user_email_ssl"].ToString());
                                    userDetail.UserEmailAuthMethod = Int32.Parse(dr["user_email_auth_method"].ToString());


                                    userDetail.FlgDeleted = Boolean.Parse(dr["flg_deleted"].ToString());
                                    userDetail.UserEnable = dr["user_enable"].ToString();
                                    returnValue.Add(userDetail);
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

      internal List<UserDetails> GetUsersByUserId(long userId)
      {
         List<UserDetails> returnValue = null;
         try
         {
            using (OleDbConnection conn = this.getDbConnection())
            {
               using (OleDbCommand objCmdSelect =
                   new OleDbCommand(UserDetailsQueries.SelectAllUsersByUserId(this.DatabaseType, userId), conn))
               {

                  using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                  {
                     if (dr.HasRows)
                     {
                        returnValue = new List<UserDetails>();
                        while (dr.Read())
                        {
                           UserDetails userDetail = new UserDetails();
                           userDetail.UserId = Int32.Parse(dr["user_id"].ToString());
                           userDetail.UserName = Utilities.GetDBString(dr["user_name"]);
                           userDetail.UserLogon = Utilities.GetDBString(dr["user_logon"]);
                           userDetail.UserTelephone = Utilities.GetDBString(dr["user_telephone"]);
                           userDetail.UserEmail = Utilities.GetDBString(dr["user_email"]);

                           userDetail.UserLevel = Int32.Parse(dr["user_level"].ToString());
                           userDetail.UserTelExt = Utilities.GetDBString(dr["user_tel_ext"]);
                           userDetail.UserEnableReminder = Utilities.GetDBString(dr["user_enable_reminder"]);
                           userDetail.UserTelMobile = Utilities.GetDBString(dr["user_tel_mobile"]);
                           userDetail.UserEmailLogon = Utilities.GetDBString(dr["user_email_logon"]);
                           userDetail.UserEmailEnable = Utilities.GetDBString(dr["user_email_enable"]);
                           userDetail.UserEmailSMTP = Utilities.GetDBString(dr["user_email_smtp"]);
                           userDetail.UserEmailHTMLFile = Utilities.GetDBString(dr["user_email_html_file"]);
                           userDetail.UserLocation = Utilities.GetDBString(dr["user_location"]);
                           userDetail.UserDepartment = Utilities.GetDBString(dr["user_department"]);
                           userDetail.ResourceSequence = Int32.Parse(dr["resource_sequence"].ToString());
                           userDetail.UserEmailSSL = Int32.Parse(dr["user_email_ssl"].ToString());
                           userDetail.UserEmailAuthMethod = Int32.Parse(dr["user_email_auth_method"].ToString());


                           userDetail.FlgDeleted = Boolean.Parse(dr["flg_deleted"].ToString());
                           userDetail.UserEnable = dr["user_enable"].ToString();
                           returnValue.Add(userDetail);
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

      internal long GetTotalUsers()
        {
            long returnValue = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserDetailsQueries.SelectTotalUsers(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    returnValue = Int32.Parse(dr["total_users"].ToString());
                                }
                            }
                        }
                    }
                }
            }catch(Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public UserDetails AddUser(UserDetails user)
        {
            UserDetails returnUser = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserDetailsQueries.CreateUser(this.DatabaseType, user), conn))
                    {

                       int result = objCmdSelect.ExecuteNonQuery();
                        if (result > 0)
                        {
                            returnUser = user;
                        }
                        else
                        {
                            returnUser = null;
                        }
                    }
                }
            }catch(Exception ex)
            {
                returnUser = null;
            }
            return returnUser;
        }
        public UserDetails UpdateUser(UserDetails user)
        {
            UserDetails returnUser = null;
            bool IsPassMatch = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                   if(IsPasswordMatch(user.UserEnable))
                    {
                        IsPassMatch = true;
                    }
                    else
                    {
                        IsPassMatch = false;
                    }
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserDetailsQueries.UpdateUser(this.DatabaseType, user, IsPassMatch), conn))
                    {
                        int result = objCmdSelect.ExecuteNonQuery();
                        if (result > 0)
                        {
                            returnUser = user;
                        }
                        else
                        {
                            returnUser = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnUser = null;
            }
            return returnUser;
        }
        public List<UserDetails> CheckUserAsUniq(string user,long userId)
        {
            List<UserDetails> returnUser = new List<UserDetails>();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserDetailsQueries.CheckUser(this.DatabaseType, user,userId), conn))
                    {

                        OleDbDataReader dr = objCmdSelect.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                UserDetails userDetail = new UserDetails();
                                userDetail.UserId = Int32.Parse(dr["user_id"].ToString());
                                userDetail.UserName = Utilities.GetDBString(dr["user_name"]);
                                userDetail.UserLogon = Utilities.GetDBString(dr["user_logon"]);
                                userDetail.UserTelephone = Utilities.GetDBString(dr["user_telephone"]);
                                userDetail.UserEmail = Utilities.GetDBString(dr["user_email"]);
                                userDetail.FlgDeleted = Boolean.Parse(dr["flg_deleted"].ToString());
                                userDetail.UserEnable = dr["user_enable"].ToString();
                                returnUser.Add(userDetail);

                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                returnUser = new List<UserDetails>();
            }
            return returnUser;
        }
        public UserDetails EnableOrDisabled(UserDetails user)
        {
           UserDetails returnUser = new UserDetails();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserDetailsQueries.EnableOrDisabled(this.DatabaseType, user), conn))
                    {

                        OleDbDataReader dr = objCmdSelect.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                returnUser.UserId = Int32.Parse(dr["user_id"].ToString());
                                returnUser.UserName = Utilities.GetDBString(dr["user_name"]);
                                returnUser.UserLogon = Utilities.GetDBString(dr["user_logon"]);
                                returnUser.UserTelephone = Utilities.GetDBString(dr["user_telephone"]);
                                returnUser.UserEmail = Utilities.GetDBString(dr["user_email"]);
                                returnUser.FlgDeleted = Boolean.Parse(dr["flg_deleted"].ToString());
                                returnUser.UserEnable = dr["user_enable"].ToString();

                            }
                        }
                        else
                        {
                            returnUser = null;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                returnUser = null;
            }
            return returnUser;
        }
        public UserDetails IsUserExist(string user)
        {
            UserDetails returnUser = new UserDetails();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserDetailsQueries.IsUserExist(this.DatabaseType, user), conn))
                    {

                        OleDbDataReader dr = objCmdSelect.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                UserDetails userDetail = new UserDetails();
                                userDetail.UserId = Int32.Parse(dr["user_id"].ToString());
                                userDetail.UserName = Utilities.GetDBString(dr["user_name"]);
                                userDetail.UserLogon = Utilities.GetDBString(dr["user_logon"]);
                                userDetail.UserTelephone = Utilities.GetDBString(dr["user_telephone"]);
                                userDetail.UserEmail = Utilities.GetDBString(dr["user_email"]);
                                userDetail.FlgDeleted = Boolean.Parse(dr["flg_deleted"].ToString());
                                userDetail.UserEnable = dr["user_enable"].ToString();
                                returnUser = userDetail;
                            }

                        }
                        else
                        {
                            returnUser = null;
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                returnUser = null;
            }
            return returnUser;
        }

		public Boolean IsUserLogonExist(string userLogon)
		{
			Boolean returnValue = false;
			try
			{
				using (OleDbConnection conn = this.getDbConnection())
				{
					using (OleDbCommand objCmdSelect =
						new OleDbCommand(UserDetailsQueries.IsUserLogonExist(this.DatabaseType, userLogon), conn))
					{

						OleDbDataReader dr = objCmdSelect.ExecuteReader();
						if (dr.HasRows)
						{
							returnValue = true;
						}

					}
				}
			}
			catch (Exception ex)
			{
				Utilities.WriteLog("Exception occur in accessing database:" + ex.Message);
				throw ex;
			}
			return returnValue;
		}
		public UserDetails PasswordExist(UserDetails user)
        {
            UserDetails returnUser = new UserDetails();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserDetailsQueries.PasswordExist(this.DatabaseType, user), conn))
                    {

                        OleDbDataReader dr = objCmdSelect.ExecuteReader();
                        if (dr.HasRows)
                        {
                            
                            while (dr.Read())
                            {
                                UserDetails userDetail = new UserDetails();
                                userDetail.UserId = Int32.Parse(dr["user_id"].ToString());
                                userDetail.UserName = Utilities.GetDBString(dr["user_name"]);
                                userDetail.UserLogon = Utilities.GetDBString(dr["user_logon"]);
                                userDetail.UserTelephone = Utilities.GetDBString(dr["user_telephone"]);
                                userDetail.UserEmail = Utilities.GetDBString(dr["user_email"]);
                                userDetail.FlgDeleted = Boolean.Parse(dr["flg_deleted"].ToString());
                                userDetail.UserEnable = dr["user_enable"].ToString();
                                returnUser = userDetail;
                            }
                           
                        }
                        else
                        {
                            returnUser = null;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                returnUser = null;
            }
            return returnUser;
        }
        public bool IsPasswordMatch(string userEnable)
        {
            bool returnUser = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserDetailsQueries.IsPasswordMatch(this.DatabaseType, userEnable), conn))
                    {

                       OleDbDataReader dr = objCmdSelect.ExecuteReader();
                       if(dr.HasRows)
                        {
                            returnUser = true;
                        }
                        else
                        {
                            returnUser = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnUser = false;
            }
            return returnUser;
        }

      public Int32 getNbrOfLoggedInUsers()
      {
         Int32 returnValue = 0;
         try
         {
            using (OleDbConnection conn = this.getDbConnection())
            {
               using (OleDbCommand objCmdSelect =
                   new OleDbCommand(UserDetailsQueries.SelectNbrOfLoggedInUsers(this.DatabaseType), conn))
               {
                  using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                  {
                     if (dr.HasRows)
                     {
                        dr.Read();
                        returnValue = Int32.Parse(dr["NbrOfLoggedInUsers"].ToString());
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            Utilities.WriteLog( "Error occured while getting Current Logged in Users. " + ex.Message + " " + ex.InnerException);
         }
         return returnValue;
      }
        public UserDetails ForgotPasswordStringExist(UserDetails user)
        {
            UserDetails returnUser = new UserDetails();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserDetailsQueries.ForgotPasswordStringExist(this.DatabaseType, user), conn))
                    {

                        OleDbDataReader dr = objCmdSelect.ExecuteReader();
                        if (dr.HasRows)
                        {

                            while (dr.Read())
                            {
                                UserDetails userDetail = new UserDetails();
                                userDetail.UserId = Int32.Parse(dr["user_id"].ToString());
                                userDetail.UserName = Utilities.GetDBString(dr["user_name"]);
                                userDetail.UserLogon = Utilities.GetDBString(dr["user_logon"]);
                                userDetail.UserTelephone = Utilities.GetDBString(dr["user_telephone"]);
                                userDetail.UserEmail = Utilities.GetDBString(dr["user_email"]);
                                userDetail.FlgDeleted = Boolean.Parse(dr["flg_deleted"].ToString());
                                userDetail.UserEnable = dr["user_enable"].ToString();
                                returnUser = userDetail;
                            }

                        }
                        else
                        {
                            returnUser = null;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                returnUser = null;
            }
            return returnUser;
        }
        public UserDetails RecoverForgotPassword(UserDetails user)
        {
            UserDetails returnUser = new UserDetails();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(UserDetailsQueries.RecoverForgotPassword(user), conn))
                    {

                        int result = objCmdSelect.ExecuteNonQuery();
                        if (result > 0)
                        {
                            returnUser = user;
                        }
                        else
                        {
                            returnUser = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnUser = null;
            }
            return returnUser;
        }
    }
}
