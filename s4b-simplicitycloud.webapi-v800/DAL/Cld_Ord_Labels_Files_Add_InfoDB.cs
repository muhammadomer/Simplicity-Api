using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class Cld_Ord_Labels_Files_Add_InfoDB : MainDB
    {

        public Cld_Ord_Labels_Files_Add_InfoDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertCld_Ord_Labels_Files_Add_Info(out long sequence, long jobSequence, long oiSequence, long headerSequence, long joinSequence, bool flgDeleted, string addInfo, long createdBy, DateTime? dateCreated,
                                                   long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(Cld_Ord_Labels_Files_Add_InfoQueries.insert(this.DatabaseType, jobSequence, oiSequence, headerSequence, joinSequence, flgDeleted, addInfo, createdBy, dateCreated,
                                                                                lastAmendedBy, dateLastAmended), conn))
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
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public Cld_Ord_Labels_Files_Add_Info selectAllCld_Ord_Labels_Files_Add_InfoSequence(long sequence)
        {
            Cld_Ord_Labels_Files_Add_Info returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Cld_Ord_Labels_Files_Add_InfoQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new Cld_Ord_Labels_Files_Add_Info();
                                while (dr.Read())
                                {
                                    //returnValue.Add(Load_OiFireProtectionIAddInfo(dr));
                                    returnValue = Load_Cld_Ord_Labels_Files_Add_Info(dr);
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

        public Cld_Ord_Labels_Files_Add_Info selectCld_Ord_Labels_Files_Add_InfoDesc(long jobSequence, string addInfo)
        {
            Cld_Ord_Labels_Files_Add_Info returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(Cld_Ord_Labels_Files_Add_InfoQueries.getSelectAllByDesc(this.DatabaseType, jobSequence, addInfo), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new Cld_Ord_Labels_Files_Add_Info();
                                while (dr.Read())
                                {
                                    returnValue = Load_Cld_Ord_Labels_Files_Add_Info(dr);
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

        public bool updateBySequence(long sequence, long jobSequence, long oiSequence, long headerSequence, long joinSequence, bool flgDeleted, string addInfo, long createdBy, DateTime? dateCreated,
                                     long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(Cld_Ord_Labels_Files_Add_InfoQueries.update(this.DatabaseType, sequence, jobSequence, oiSequence, headerSequence, joinSequence, flgDeleted, addInfo, createdBy, dateCreated,
                                                                                lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
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
                        new OleDbCommand(Cld_Ord_Labels_Files_Add_InfoQueries.delete(this.DatabaseType, sequence), conn))
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

        public bool deleteByFlgDeleted(long sequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(Cld_Ord_Labels_Files_Add_InfoQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        private Cld_Ord_Labels_Files_Add_Info Load_Cld_Ord_Labels_Files_Add_Info(OleDbDataReader dr)
        {
            Cld_Ord_Labels_Files_Add_Info oiFireProtectionIAddInfo = null;
            try
            {
                if (dr != null)
                {
                    oiFireProtectionIAddInfo = new Cld_Ord_Labels_Files_Add_Info();
                    oiFireProtectionIAddInfo.Sequence = long.Parse(dr["sequence"].ToString());
                    oiFireProtectionIAddInfo.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    oiFireProtectionIAddInfo.OiSequence = long.Parse(dr["oi_sequence"].ToString());
                    oiFireProtectionIAddInfo.HeaderSequence = long.Parse(dr["header_sequence"].ToString());
                    oiFireProtectionIAddInfo.JoinSequence = long.Parse(dr["join_sequence"].ToString());
                    oiFireProtectionIAddInfo.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                    oiFireProtectionIAddInfo.AddInfo = dr["add_info"] == DBNull.Value ? "" : Utilities.GetDBString(dr["add_info"]);
                    oiFireProtectionIAddInfo.CreatedBy = long.Parse(dr["created_by"].ToString());
                    oiFireProtectionIAddInfo.DateCreated = Utilities.getSQLDate(DateTime.Parse(dr["date_created"].ToString()));
                    oiFireProtectionIAddInfo.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    oiFireProtectionIAddInfo.DateLastAmended = Utilities.getSQLDate(DateTime.Parse(dr["date_last_amended"].ToString()));
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return oiFireProtectionIAddInfo;
        }

        //public DriveService AuthenticateOauth(string clientId, string clientSecret, string userName)
        //{
        //    return OiFireProtectionIAddInfoQueries.AuthenticateOauth(clientId, clientSecret, userName);
        //}
    }
}