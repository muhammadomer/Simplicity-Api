using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class AttfOrdDocsMastersDB : MainDB
    {

        public AttfOrdDocsMastersDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertAttfOrdDocsMasters(out long sequence, bool flgDeleted, bool flgHide, long jobSequence, string fileName,
                                    string fileVersionNo, DateTime? dateFileVersionNo, string fileVersionOption, bool flgFileVo,
                                    string fileNotes, string filePathAndName, long createdBy, DateTime? dateCreated, long lastAmendedBy,
                                    DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(AttfOrdDocsMastersQueries.insert(this.DatabaseType, flgDeleted, flgHide, jobSequence, fileName, fileVersionNo, dateFileVersionNo,
                                       fileVersionOption, flgFileVo, fileNotes, filePathAndName, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        string OleDb = "select @@IDENTITY";
                        using (OleDbCommand objCommand =
                            new OleDbCommand(OleDb, conn))
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

        public List<AttfOrdDocsMasters> selectAllAttfOrdDocsMastersSequence(long sequence)
        {
            List<AttfOrdDocsMasters> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AttfOrdDocsMastersQueries.getSelectAllBySequence(sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<AttfOrdDocsMasters>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_AttfOrdDocsMasters(dr));
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

        public AttfOrdDocsMasters selectAllAttfOrdDocsMastersByJobSequenceAndSequence(long jobSequence, long sequence)
        {
            AttfOrdDocsMasters returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AttfOrdDocsMastersQueries.getSelectByJobSequenceAndSequence(this.DatabaseType, jobSequence, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnValue = Load_AttfOrdDocsMasters(dr);
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

        public List<AttfOrdDocsMasters> selectAllAttfOrdDocsMastersByJobSequence(long jobSequence)
        {
            List<AttfOrdDocsMasters> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AttfOrdDocsMastersQueries.getSelectByJobSequenceAndSequence(this.DatabaseType, jobSequence, -1), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<AttfOrdDocsMasters>();
                                while (dr.Read())

                                    returnValue.Add(Load_AttfOrdDocsMasters(dr));
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

        public bool updateBySequence(long sequence, bool flgDeleted, bool flgHide, long jobSequence, string fileName,
                                     string fileVersionNo, DateTime? dateFileVersionNo, string fileVersionOption, bool flgFileVo,
                                     string fileNotes, string filePathAndName, long createdBy, DateTime? dateCreated, long lastAmendedBy,
                                     DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(AttfOrdDocsMastersQueries.update(this.DatabaseType, sequence, flgDeleted, flgHide, jobSequence, fileName, fileVersionNo, dateFileVersionNo,
                                       fileVersionOption, flgFileVo, fileNotes, filePathAndName, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
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
                        new OleDbCommand(AttfOrdDocsMastersQueries.delete(sequence), conn))
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
                        new OleDbCommand(AttfOrdDocsMastersQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
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

        private AttfOrdDocsMasters Load_AttfOrdDocsMasters(OleDbDataReader dr)
        {
            AttfOrdDocsMasters attfOrdDocsMasters = null;
            try
            {
                if (dr != null)
                {
                    attfOrdDocsMasters = new AttfOrdDocsMasters();
                    attfOrdDocsMasters.Sequence = long.Parse(dr["sequence"].ToString());
                    attfOrdDocsMasters.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                    attfOrdDocsMasters.FlgHide = bool.Parse(dr["flg_hide"].ToString());
                    attfOrdDocsMasters.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    attfOrdDocsMasters.FileName = Utilities.GetDBString(dr["file_name"]);
                    attfOrdDocsMasters.FileVersionNo = Utilities.GetDBString(dr["file_version_no"]);
                    attfOrdDocsMasters.DateFileVersionNo = Utilities.getSQLDate(DateTime.Parse(dr["date_file_version_no"].ToString()));
                    attfOrdDocsMasters.FileVersionOption = Utilities.GetDBString(dr["file_version_option"]);
                    attfOrdDocsMasters.FlgFileVo = bool.Parse(dr["flg_file_vo"].ToString());
                    attfOrdDocsMasters.FileNotes = Utilities.GetDBString(dr["file_notes"]);
                    attfOrdDocsMasters.FilePathAndName = Utilities.GetDBString(dr["file_path_and_name"]);
                    attfOrdDocsMasters.CreatedBy = long.Parse(dr["created_by"].ToString());
                    attfOrdDocsMasters.DateCreated = DateTime.Parse(dr["date_created"].ToString());
                    attfOrdDocsMasters.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    attfOrdDocsMasters.DateLastAmended = DateTime.Parse(dr["date_last_amended"].ToString());
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return attfOrdDocsMasters;
        }
    }
}