using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class UserDetailsQueries
    {
        public static string SelectAllFieldsByUserNameAndEnable(string databaseType, string userLogon, string userEnable)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "SELECT * " +
                                "  FROM un_user_details " +
                                " WHERE user_logon = '" + userLogon.Trim() + "'" +
                                "   AND user_enable='" + Utilities.strUFFAFU(userEnable) + "'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

      public static string SelectNbrOfLoggedInUsers(string databaseType)
      {
         string returnValue = "";
         try
         {
            switch (databaseType) { 
            case "MSACCESS":
                  returnValue = @"Select count(user_id) as NbrOfLoggedInUsers
                  From un_user_sessions
                  Where DateValue(token_expiry) = Date() and token_expiry-Date() >0";
               break;
            case "SQLSERVER":
               returnValue = @"Select count(user_id) as NbrOfLoggedInUsers
               From un_user_sessions
               Where Convert(varchar(10),token_expiry,111)=Convert(varchar(10),getDate(),111)
                  and DATEDIFF(MI,getdate(),token_expiry)>0";
               break;
            }
         }
         catch (Exception ex)
         {
            Utilities.WriteLog("Error Occured in Query");
         }
         return returnValue;
      }

      public static string SelectAllFieldsByUserId(string databaseType, int userId)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "SELECT * " +
                    "  FROM un_user_details " +
                    " WHERE user_id = " + userId.ToString();
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
                
                returnValue = @"SELECT * 
                        FROM un_user_details 
                        WHERE flg_deleted <> "+ Utilities.GetBooleanForDML(databaseType, true) +
                        " AND user_id not in (1,3,5)" +
                        " ORDER BY user_name";
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
                              "  FROM un_user_details " +
                              " WHERE user_logon like '%" + userName + "%'" +
                              "   AND flg_deleted <> "+ Utilities.GetBooleanForDML(databaseType,true);
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

      public static string SelectAllUsersByUserId(string databaseType, long userId)
      {
         string returnValue = "";
         try
         {
            returnValue = "SELECT * " +
                          "  FROM un_user_details " +
                          " WHERE user_id=" + userId  +
                          "   AND flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
         }
         catch (Exception ex)
         {
         }
         return returnValue;
      }
      public static string SelectTotalUsers(string databaseType)
        {
            string returnValue = "";
            try
            {
                returnValue = @"Select count(*) as total_users From un_user_details 
                WHERE flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) +
                " AND user_id not in (1,3,5)";
            }catch(Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public static string CreateUser(string databaseType, UserDetails userName)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "insert into un_user_details(user_level, flg_deleted, user_name,  user_logon, user_enable, user_enable_reminder, user_telephone,flg_sage_opt_out,user_email,user_email_logon,user_email_enable,user_email_smtp,user_email_ssl,user_email_auth_method,resource_sequence,created_by,date_created,last_amended_by,date_last_amended,user_tel_ext,user_tel_mobile,user_location,user_department,user_email_html_file) values ('"
                    + userName.UserLevel + "'," + Utilities.GetBooleanForDML(databaseType, userName.FlgDeleted) + ",'" + userName.UserName + "','" + userName.UserLogon + "','" 
                    + Utilities.strUFFAFU(userName.UserEnable) + "','" + userName.UserEnableReminder + "','" 
                    + userName.UserTelephone + "','" + 1 + "','" + userName.UserEmail + "','" + userName.UserEmailLogon + "','"
                    + userName.UserEmailEnable + "','" + userName.UserEmailSMTP + "'," + userName.UserEmailSSL + "," + userName.UserEmailAuthMethod + ","
                    + userName.ResourceSequence + "," + userName.CreatedBy + "," + Utilities.GetDateTimeForDML(databaseType, userName.DateCreated,true,true) + "," + userName.LastAmendedBy + "," + Utilities.GetDateTimeForDML(databaseType, userName.DateLastAmended,true,true) + ",'"
                    + userName.UserTelExt + "','" + userName.UserTelMobile + "','" + userName.UserLocation + "','" + userName.UserDepartment + "','" + userName.UserEmailHTMLFile + "')";
                        
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string UpdateUser(string databaseType, UserDetails userName, bool IsPasswordMatch)
        {
            string returnValue = "";
            try
            {
                
                        if (IsPasswordMatch)
                        {
                            returnValue = "update un_user_details set "
                                +"user_level=" + userName.UserLevel 
                                + ", flg_deleted=" + Utilities.GetBooleanForDML(databaseType, userName.FlgDeleted) 
                                + ", user_name='" + userName.UserName 
                                + "', user_logon='" + userName.UserLogon 
                                + "',user_enable='" + userName.UserEnable 
                                + "',user_enable_reminder='" + userName.UserEnableReminder 
                                + "',user_telephone='" + userName.UserTelephone
                                + "',user_tel_ext='" + userName.UserTelExt
                                + "',user_tel_mobile='" + userName.UserTelMobile
                                + "',user_department='" + userName.UserDepartment
                                + "',user_location='" + userName.UserLocation
                                + "',flg_sage_opt_out='" + 1 
                                + "', user_email_logon='" + userName.UserEmailLogon
                                + "', user_email_enable='" + userName.UserEmailEnable
                                + "', user_email_smtp='" + userName.UserEmailSMTP
                                + "', user_email_ssl=" + userName.UserEmailSSL
                                + ", user_email_auth_method=" + userName.UserEmailAuthMethod
                                + ", user_email='" + userName.UserEmail
                                + "', last_amended_by=" + userName.LastAmendedBy
                                + ", date_last_amended=" + Utilities.GetDateTimeForDML(databaseType, userName.DateLastAmended,true,true)
                                + " where user_id=" + userName.UserId + "";
                        }
                        else
                        {
                            returnValue = "update un_user_details set "
                                +"user_level=" + userName.UserLevel 
                                + ", flg_deleted=" + Utilities.GetBooleanForDML(databaseType, userName.FlgDeleted)
                                + ", user_name='" + userName.UserName 
                                + "', user_logon='" + userName.UserLogon 
                                + "',user_enable='" + Utilities.strUFFAFU(userName.UserEnable) 
                                + "',user_enable_reminder='" + userName.UserEnableReminder 
                                + "',user_telephone='" + userName.UserTelephone
                                + "',user_tel_ext='" + userName.UserTelExt
                                + "',user_tel_mobile='" + userName.UserTelMobile
                                + "',user_department='" + userName.UserDepartment
                                + "',user_location='" + userName.UserLocation
                                + "',flg_sage_opt_out='" + 1
                                 + "', user_email_logon='" + userName.UserEmailLogon
                                + "', user_email_enable='" + userName.UserEmailEnable
                                + "', user_email_smtp='" + userName.UserEmailSMTP
                                + "', user_email_ssl=" + userName.UserEmailSSL
                                + ", user_email_auth_method=" + userName.UserEmailAuthMethod
                                + ", user_email='" + userName.UserEmail
                                + "', last_amended_by=" + userName.LastAmendedBy
                                + ", date_last_amended=" + Utilities.GetDateTimeForDML(databaseType, userName.DateLastAmended, true, true)
                                + " where user_id=" + userName.UserId + "";
                        }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string CheckUser(string databaseType, string userName,long userId)
        {
            string returnValue = "";
            try
            {
                
                returnValue = @"SELECT * FROM un_user_details 
                         WHERE user_logon like '%" + userName + "%' And user_id <> " + userId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string EnableOrDisabled(string databaseType, UserDetails userName)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "SELECT * " +
                        "  FROM un_user_details " +
                        " WHERE user_id = '"+userName.UserId+ "' and flg_deleted = "+ Utilities.GetBooleanForDML(databaseType, userName.FlgDeleted) ;
                        
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string IsUserExist(string databaseType, string userName)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "SELECT * " +
                        "  FROM un_user_details " +
                        " WHERE user_logon like '%" + userName + "%' or user_email like '%" + userName + "%'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
		public static string IsUserLogonExist(string databaseType, string userLogon)
		{
			string returnValue = "";
			try
			{

				returnValue = "SELECT * " +
						"  FROM un_user_details " +
						" WHERE user_logon='" + userLogon + "'";
			}
			catch (Exception ex)
			{
			}
			return returnValue;
		}
		public static string PasswordExist(string databaseType, UserDetails userName)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "SELECT * " +
                                "  FROM un_user_details " +
                                " WHERE user_logon = '" + userName.UserLogon.Trim() + "'" +
                                "   AND user_enable='" + Utilities.strUFFAFU(userName.UserEnable) + "'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string IsPasswordMatch(string databaseType, string userEnable)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "SELECT * " +
                        "  FROM un_user_details " +
                        " WHERE user_enable = '"+ userEnable + "' ";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string ForgotPasswordStringExist(string databaseType, UserDetails userName)
        {
            string returnValue = "";
            try
            {
                returnValue = $"SELECT * FROM un_user_details " +
                                    $"WHERE user_enable_reset_enabler = '{userName.ForgotPasswordString.Trim()}'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string RecoverForgotPassword(UserDetails userName)
        {
            string returnValue = "";
            try
            {
                returnValue = "update un_user_details set "
                                + "user_enable='" + Utilities.strUFFAFU(userName.UserEnable)
                                + "', user_enable_reset_enabler=''"
                                + " where user_id=" + userName.UserId + "";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}
