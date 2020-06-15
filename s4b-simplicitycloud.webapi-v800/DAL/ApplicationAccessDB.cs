using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{

	public class ApplicationAccessDB:MainDB
		{

        public ApplicationAccessDB(DatabaseInfo dbInfo) : base(dbInfo)
            {
            }

        public bool insertApplicationAccess(out long processId, bool usrLevel01, bool usrLevel02, bool usrLevel03, bool usrLevel04, bool usrLevel05, bool usrLevel06,
                                             bool usrLevel07, bool usrLevel08, bool usrLevel09, bool usrLevel10)
            {
                bool returnValue = false;
                processId = -1;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdInsert =
                            new OleDbCommand(ApplicationAccessQueries.insert(this.DatabaseType, usrLevel01, usrLevel02, usrLevel03, usrLevel04, usrLevel05, usrLevel06,
                                                                            usrLevel07, usrLevel08,  usrLevel09,  usrLevel10 ), conn))
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
                                    processId = long.Parse(dr[0].ToString());
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
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                    // Requires Logging
                }
                return returnValue;
            }

      public List<ApplicationAccess> selectAllApplicationAccessProcessId(long processId)
            {
                List<ApplicationAccess> returnValue = null;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdSelect =
                            new OleDbCommand(ApplicationAccessQueries.getSelectAllByProcessId(this.DatabaseType, processId), conn))
                        {
                            using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                            {
                                if (dr.HasRows)
                                {
                                    returnValue = new List<ApplicationAccess>();
                                    while (dr.Read())
                                    {
                                        returnValue.Add(Load_ApplicationAccess(dr));
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

        public bool updateByProcessId(long processId, bool usrLevel01, bool usrLevel02, bool usrLevel03, bool usrLevel04, bool usrLevel05, bool usrLevel06,
                                      bool usrLevel07, bool usrLevel08, bool usrLevel09, bool usrLevel10)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(ApplicationAccessQueries.update(this.DatabaseType, processId, usrLevel01, usrLevel02, usrLevel03, usrLevel04, usrLevel05, usrLevel06, usrLevel07, usrLevel08, usrLevel09, usrLevel10), conn))
                        {
                            objCmdUpdate.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
                    // Requires Logging
                }
                return returnValue;
            }
        
       public bool deleteByprocessId(long processId)
            {
                bool returnValue = false;
                try
                {
                    using (OleDbConnection conn = this.getDbConnection())
                    {
                        using (OleDbCommand objCmdUpdate =
                            new OleDbCommand(ApplicationAccessQueries.delete(this.DatabaseType, processId), conn))
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
       
      private ApplicationAccess Load_ApplicationAccess(OleDbDataReader dr)
            {
                ApplicationAccess applicationAccess = null;
                try
                { 
                    if(dr!=null)
                    {
                        applicationAccess = new ApplicationAccess();
                        applicationAccess.ProcessId = long.Parse(dr["process_id"].ToString());
                        applicationAccess.UsrLevel01 = bool.Parse(dr["usr_level01"].ToString());
                        applicationAccess.UsrLevel02 = bool.Parse(dr["usr_level02"].ToString());
                        applicationAccess.UsrLevel03 = bool.Parse(dr["usr_level03"].ToString());
                        applicationAccess.UsrLevel04 = bool.Parse(dr["usr_level04"].ToString());
                        applicationAccess.UsrLevel05 = bool.Parse(dr["usr_level05"].ToString());
                        applicationAccess.UsrLevel06 = bool.Parse(dr["usr_level06"].ToString());
                        applicationAccess.UsrLevel07 = bool.Parse(dr["usr_level07"].ToString());
                        applicationAccess.UsrLevel08 = bool.Parse(dr["usr_level08"].ToString());
                        applicationAccess.UsrLevel09 = bool.Parse(dr["usr_level09"].ToString());
                        applicationAccess.UsrLevel10 = bool.Parse(dr["usr_level10"].ToString());
                    
                    }
                }
                catch(Exception ex)
                {
                    //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                    // Requires Logging

                }
                return applicationAccess;
            }			
		}
}