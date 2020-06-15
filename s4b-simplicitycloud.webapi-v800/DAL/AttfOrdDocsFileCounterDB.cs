using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class AttfOrdDocsFileCounterDB : MainDB
    {
        public AttfOrdDocsFileCounterDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }
        public bool insertAttfOrdDocsFileCounter(out long sequence, long jobSequence, bool flgMasterFile, long lastFileNo)
        {
            bool returnValue = false;
            sequence = -1;
            this.ErrorMessage = "";
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(AttfOrdDocsFileCounterQueries.insert(this.DatabaseType, jobSequence, flgMasterFile, lastFileNo), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<AttfOrdDocsFileCounter> selectAllAttfOrdDocsFileCounterSequence(long sequence)
        {
            List<AttfOrdDocsFileCounter> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AttfOrdDocsFileCounterQueries.getSelectAllBySequence(sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<AttfOrdDocsFileCounter>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_AttfOrdDocsFileCounter(dr));
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

        public AttfOrdDocsFileCounter selectAllAttfOrdDocsFileCounterByJobSequenceAndFlgMasterFile(long jobSequence, bool flgMasterFile)
        {
            AttfOrdDocsFileCounter returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(AttfOrdDocsFileCounterQueries.getSelectAllByJobSequenceAndFlgMasterFile(this.DatabaseType, jobSequence, flgMasterFile), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new AttfOrdDocsFileCounter();
                                dr.Read();
                                returnValue = Load_AttfOrdDocsFileCounter(dr);
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

        public bool updateBySequence(long sequence, long jobSequence, bool flgMasterFile, long lastFileNo)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(AttfOrdDocsFileCounterQueries.update(this.DatabaseType, sequence, jobSequence, flgMasterFile, lastFileNo), conn))
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
                        new OleDbCommand(AttfOrdDocsFileCounterQueries.delete(sequence), conn))
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
                        new OleDbCommand(AttfOrdDocsFileCounterQueries.delete(sequence), conn))
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
        private AttfOrdDocsFileCounter Load_AttfOrdDocsFileCounter(OleDbDataReader dr)
        {
            AttfOrdDocsFileCounter attfOrdDocsFileCounter = null;
            try
            {
                if (dr != null)
                {
                    attfOrdDocsFileCounter = new AttfOrdDocsFileCounter();
                    attfOrdDocsFileCounter.Sequence = long.Parse(dr["sequence"].ToString());
                    attfOrdDocsFileCounter.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    attfOrdDocsFileCounter.FlgMasterFile = bool.Parse(dr["flg_master_file"].ToString());
                    attfOrdDocsFileCounter.LastFileNo = long.Parse(dr["last_file_no"].ToString());
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return attfOrdDocsFileCounter;
        }

    }
}