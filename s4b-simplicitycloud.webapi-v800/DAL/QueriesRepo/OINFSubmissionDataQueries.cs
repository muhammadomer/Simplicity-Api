using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OINFSubmissionDataQueries
    {

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
               
                returnValue = @"SELECT * 
                                FROM un_oi_nf_submission_data 
                                WHERE sequence = " + Sequence;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, OINFSubmissionData oi)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO un_oi_nf_submission_data(job_sequence, de_sequence, date_de, nfs_submit_no, " +
                                "       nfs_submit_ts, flg_text_row,  item_quantity, item_code,  item_desc,  item_units, " +
                                "       amt_unit, amt_total, flg_3rd_party, id_3rd_party, " +
                                "       created_by, date_created, last_amended_by, date_last_amended) " +
                                "VALUES (" + oi.JobSequence + ", " + oi.DeSequence + ", " + Utilities.GetDateTimeForDML(databaseType, oi.DatDe,true,true) + ", " +
                                "     '" + oi.NfsSubmitNo + "', '" + oi.NfsSubmitTs + "', " + Utilities.GetBooleanForDML(databaseType, oi.FlgRowIsText) + ", " +
                                "      " + oi.ItemQuantity + ", '" + oi.ItemCode + "', '" + oi.ItemDesc + "', '" + oi.ItemUnits + "', " +
                                "      " + oi.AmountUnit + ", " + oi.AmountTotal + ", " + Utilities.GetBooleanForDML(databaseType, oi.FlgThirdParty) + ", " +
                                "      " + oi.IdThirdParty + ", " + oi.CreatedBy + ", " + Utilities.GetDateTimeForDML(databaseType, oi.DateCreated,true,true) + ", " +
                                "      " + oi.LastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, oi.DateLastAmended,true,true) + ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

