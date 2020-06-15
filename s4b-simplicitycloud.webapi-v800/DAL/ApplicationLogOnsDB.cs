using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace SimplicityOnlineWebApi.DAL
{

	public class ApplicationLogOnsDB:MainDB
		{
			 
        public ApplicationLogOnsDB(DatabaseInfo dbInfo) : base(dbInfo)
            {
            }

        public bool insertApplicationLogOns(out long sequence, long userId, DateTime userLogOnTime, bool flgUserLogOff, bool flgReset,
                                              DateTime userLogOffTime, int userProcessId, string userIpAddress)
            {
                bool returnValue = false;
                sequence = -1;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdInsert =
                            new OleDbCommand(ApplicationLogOnsQueries.insert(this.DatabaseType, userId, userLogOnTime, flgUserLogOff, flgReset, userLogOffTime, userProcessId, userIpAddress ), conn))
                        {
                            objCmdInsert.ExecuteNonQuery();
                            string sql = "select @@IDENTITY";
                            using (OleDbCommand objCommand =
                                new OleDbCommand(sql, conn))
                            {
                                OleDbDataReader dr = objCommand.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Read();
                                    sequence = long.Parse(dr[0].ToString());
                                }
                                else
                                {
                                    //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record.'" + METHOD_NAME + "'\n";
                                }
                            }
                        }
                    }
                    returnValue = true;
                }
                catch (Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +                   ex.InnerException;
                    // Requires Logging
                }
                return returnValue;
            }

       public List<ApplicationLogOns> selectAllApplicationLogOnsSequence(long sequence)
            {
                List<ApplicationLogOns> returnValue = null;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdSelect =
                            new OleDbCommand(ApplicationLogOnsQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                        {
                            using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    returnValue = new List<ApplicationLogOns>();
                                    while (dr.Read())
                                    {
                                        returnValue.Add(Load_ApplicationLogOns(dr));
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                    // Requires Logging
                }
                return returnValue;
            }

        public bool updateBySequence(long sequence, long userId, DateTime userLogOnTime, bool flgUserLogOff, bool flgReset,
                                     DateTime userLogOffTime, long userProcessId, string userIpAddress)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(ApplicationLogOnsQueries.update(this.DatabaseType, sequence, userId, userLogOnTime, flgUserLogOff, flgReset, userLogOffTime, userProcessId, userIpAddress), conn))
                        {
                            objCmdUpdate.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                    // Requires Logging
                }
                return returnValue;
            }
        
       public bool deleteBySequence(long sequence)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(ApplicationLogOnsQueries.delete(this.DatabaseType, sequence), conn))
                        {
                            objCmdUpdate.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                    // Requires Logging
                }
                return returnValue;
            }
       
      
       private ApplicationLogOns Load_ApplicationLogOns(OleDbDataReader dr)
            {
                ApplicationLogOns applicationLogOns = null;
                try
                { 
                    if(dr!=null)
                    {
                        applicationLogOns = new ApplicationLogOns();
                        applicationLogOns.Sequence = long.Parse(dr["sequence"].ToString());
                        applicationLogOns.UserId = long.Parse(dr["user_id"].ToString());
                        applicationLogOns.UserLogOnTime = Utilities.GetDBString(dr["user_log_on_time"]);
                        applicationLogOns.FlgUserLogOff = bool.Parse(dr["flg_user_log_off"].ToString());
                        applicationLogOns.FlgReset = bool.Parse(dr["flg_reset"].ToString());
                        applicationLogOns.UserLogOffTime = Utilities.GetDBString(dr["user_log_off_time"]);
                        applicationLogOns.UserProcessId = long.Parse(dr["user_process_id"].ToString());
                        applicationLogOns.UserIpAddress = Utilities.GetDBString(dr["user_ip_address"]);

                }
                }
                catch(Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                    // Requires Logging
                }
                return applicationLogOns;
            }
      }
}