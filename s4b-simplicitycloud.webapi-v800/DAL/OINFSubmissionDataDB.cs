using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class OINFSubmissionDataDB : MainDB
    {
        public OINFSubmissionDataDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public OINFSubmissionData insert(OINFSubmissionData Object)
        {
            OINFSubmissionData returnValue = new OINFSubmissionData();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(OINFSubmissionDataQueries.insert(this.DatabaseType, Object), conn))
                    {
                        int result = objCmdInsert.ExecuteNonQuery();
                        if (result > 0)
                        {
                            long sequence = Utilities.GetDBAutoNumber(conn);
                            Object.Sequence = sequence;
                            returnValue = Object;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
                returnValue = null;
            }
            return returnValue;
        }

        public List<OINFSubmissionData> selectAllBySequence(long sequence)
        {
            List<OINFSubmissionData> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OINFSubmissionDataQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<OINFSubmissionData>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_OINFSubmissionData(dr));
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

        private OINFSubmissionData Load_OINFSubmissionData(OleDbDataReader dr)

        {
            OINFSubmissionData OINFSubmissionData = null;
            try
            {
                if (dr != null)
                {
                    OINFSubmissionData = new OINFSubmissionData();
                    OINFSubmissionData.Sequence = Convert.ToInt32(dr["sequence"].ToString());
                    OINFSubmissionData.JobSequence =  Convert.ToInt32(dr["job_sequence"].ToString());
                    OINFSubmissionData.DeSequence =  Convert.ToInt32(dr["de_sequence"].ToString());
                    OINFSubmissionData.DatDe = Convert.ToDateTime(dr["date_de"].ToString());
                    OINFSubmissionData.NfsSubmitNo = Utilities.GetDBString(dr["nfs_submit_no"].ToString());
                    OINFSubmissionData.NfsSubmitTs = Utilities.GetDBString(dr["nfs_submit_ts"]);
                    OINFSubmissionData.FlgRowIsText = bool.Parse(dr["flg_text_row"].ToString());
                    OINFSubmissionData.ItemQuantity = Convert.ToDouble(dr["item_quantity"].ToString());
                    OINFSubmissionData.ItemCode = Utilities.GetDBString(dr["item_code"]);
                    OINFSubmissionData.ItemDesc = Utilities.GetDBString(dr["item_desc"]);
                    OINFSubmissionData.ItemUnits = Utilities.GetDBString(dr["item_units"]);
                    OINFSubmissionData.AmountUnit = Convert.ToDouble(dr["amt_unit"].ToString());
                    OINFSubmissionData.AmountTotal = Convert.ToDouble(dr["amt_total"].ToString());
                    OINFSubmissionData.FlgThirdParty = bool.Parse(dr["flg_3rd_party"].ToString());
                    OINFSubmissionData.IdThirdParty = Int32.Parse(dr["id_3rd_party"].ToString());
                    OINFSubmissionData.CreatedBy =  Convert.ToInt32(dr["created_by"].ToString());
                    OINFSubmissionData.DateCreated = Convert.ToDateTime(dr["date_created"].ToString());
                    OINFSubmissionData.LastAmendedBy =  Convert.ToInt32(dr["last_amended_by"].ToString());
                    OINFSubmissionData.DateLastAmended = Convert.ToDateTime(dr["date_last_amended"].ToString());
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return OINFSubmissionData;
        }
    }
}
