using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class EformsOrdCeeWsrDB : MainDB
    {
        public EformsOrdCeeWsrDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertEformsOrdCeeWsr(out long sequence, string formId, string formSubmissionId, string formTimeStamp, long jobSequence, long rowNo, string rowDesc,
                                          string rowRefNo, DateTime? dateRowSampleDate, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(EformsOrdCeeWsrQueries.insert(this.DatabaseType, formId, formSubmissionId, formTimeStamp, jobSequence, rowNo, rowDesc, rowRefNo, dateRowSampleDate,
                                                                       createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
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

        public List<EformsOrdCeeWsr> selectAllEformsOrdCeeWsrSequence(long sequence)
        {
            List<EformsOrdCeeWsr> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(EformsOrdCeeWsrQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<EformsOrdCeeWsr>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_EformsOrdCeeWsr(dr));
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


        public bool update(long sequence, string formId, string formSubmissionId, string formTimeStamp, long jobSequence, long rowNo, string rowDesc,
                                    string rowRefNo, DateTime? dateRowSampleDate, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EformsOrdCeeWsrQueries.update(this.DatabaseType, sequence, formId, 
                                                                       formSubmissionId, formTimeStamp, jobSequence, rowNo, rowDesc, rowRefNo,
                                                                       dateRowSampleDate, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
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
                        new OleDbCommand(EformsOrdCeeWsrQueries.delete(this.DatabaseType, sequence), conn))
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
                        new OleDbCommand(EformsOrdCeeWsrQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
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
        private EformsOrdCeeWsr Load_EformsOrdCeeWsr(OleDbDataReader dr)

        {
            EformsOrdCeeWsr eformsOrdCeeWsr = null;
            try
            {
                if (dr != null)
                {

                    eformsOrdCeeWsr = new EformsOrdCeeWsr();
                    eformsOrdCeeWsr.Sequence = long.Parse(dr["sequence"].ToString());
                    eformsOrdCeeWsr.FormId = Utilities.GetDBString(dr["form_id"]);
                    eformsOrdCeeWsr.FormSubmissionId = Utilities.GetDBString(dr["form_submission_id"]);
                    eformsOrdCeeWsr.FormTimeStamp = Utilities.GetDBString(dr["form_time_stamp"]);
                    eformsOrdCeeWsr.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    eformsOrdCeeWsr.RowNo = long.Parse(dr["row_no"].ToString());
                    eformsOrdCeeWsr.RowDesc = Utilities.GetDBString(dr["row_desc"]);
                    eformsOrdCeeWsr.RowRefNo = Utilities.GetDBString(dr["row_ref_no"]);
                    eformsOrdCeeWsr.DateRowSampleDate = DateTime.Parse(dr["date_row_sample_date"].ToString());
                    eformsOrdCeeWsr.CreatedBy = long.Parse(dr["created_by"].ToString());
                    eformsOrdCeeWsr.DateCreated = DateTime.Parse(dr["date_created"].ToString());
                    eformsOrdCeeWsr.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    eformsOrdCeeWsr.DateLastAmended = DateTime.Parse(dr["date_last_amended"].ToString());

                }
            }

            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return eformsOrdCeeWsr;
        }
    }
}
