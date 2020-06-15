using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class AttfOrdDocsFilesDB : MainDB
    {

        public AttfOrdDocsFilesDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertAttfOrdDocsFiles(out long sequence, bool flgDeleted, long jobSequence, long fileMasterId, string fileSubmissonId,
                                           string fileDescription, string fileNotes, string filePathAndName, long createdBy,
                                           DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(AttfOrdDocsFilesQueries.insert(this.DatabaseType, flgDeleted, jobSequence, fileMasterId, fileSubmissonId,
                                                                        fileDescription, fileNotes, filePathAndName, createdBy,
                                                                        dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
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

        public List<AttfOrdDocsFiles> selectAllAttfOrdDocsFilesSequence(long sequence)
        {
            List<AttfOrdDocsFiles> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AttfOrdDocsFilesQueries.getSelectAllBySequence(sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<AttfOrdDocsFiles>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_AttfOrdDocsFiles(dr));
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


        public bool updateBySequence(long sequence, bool flgDeleted, long jobSequence, long fileMasterId, string fileSubmissonId,
                                    string fileDescription, string fileNotes, string filePathAndName, long createdBy,
                                    DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(AttfOrdDocsFilesQueries.update(this.DatabaseType, sequence, flgDeleted, jobSequence, fileMasterId, fileSubmissonId,
                                                                      fileDescription, fileNotes, filePathAndName, createdBy,
                                                                      dateCreated, lastAmendedBy, dateLastAmended), conn))
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
                        new OleDbCommand(AttfOrdDocsFilesQueries.delete(sequence), conn))
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
                        new OleDbCommand(AttfOrdDocsFilesQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
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
        private AttfOrdDocsFiles Load_AttfOrdDocsFiles(OleDbDataReader dr)

        {
            AttfOrdDocsFiles attfOrdDocsFiles = null;
            try
            {
                if (dr != null)
                {
                    attfOrdDocsFiles = new AttfOrdDocsFiles();
                    attfOrdDocsFiles.Sequence = long.Parse(dr["sequence"].ToString());
                    attfOrdDocsFiles.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                    attfOrdDocsFiles.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    attfOrdDocsFiles.FileMasterId = long.Parse(dr["file_master_id"].ToString());
                    attfOrdDocsFiles.FileSubmissonId = Utilities.GetDBString(dr["file_submisson_id"]);
                    attfOrdDocsFiles.FileDescription = Utilities.GetDBString(dr["file_description"]);
                    attfOrdDocsFiles.FileNotes = Utilities.GetDBString(dr["file_notes"]);
                    attfOrdDocsFiles.FilePathAndName = Utilities.GetDBString(dr["file_path_and_name"]);
                    attfOrdDocsFiles.CreatedBy = long.Parse(dr["created_by"].ToString());
                    attfOrdDocsFiles.DateCreated = Utilities.getSQLDate(DateTime.Parse(dr["date_created"].ToString()));
                    attfOrdDocsFiles.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    attfOrdDocsFiles.DateLastAmended = Utilities.getSQLDate(DateTime.Parse(dr["date_last_amended"].ToString()));
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return attfOrdDocsFiles;
        }

    }
}
