using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class EformsOrdCeeWsrQueries
    {
        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                              "  FROM    un_eforms_ord_cee_wsr" +
                              " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, string formId, string formSubmissionId, string formTimeStamp, long jobSequence, long rowNo, string rowDesc, 
                                    string rowRefNo, DateTime? dateRowSampleDate, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO " +
                    "       un_eforms_ord_cee_wsr(form_id,  form_submission_id,  form_time_stamp, " +
                    "       job_sequence,  row_no,  row_desc,  row_ref_no, " +
                    "       date_row_sample_date,  created_by,  date_created,  last_amended_by,  " +
                    "       date_last_amended) " +
                    "VALUES ('" + formId + "',   '" + formSubmissionId + "',   '" + formTimeStamp + "',   " + jobSequence + ",   " + rowNo + ",   '" + rowDesc + "', " +
                    "     '" + rowRefNo + "',   " + Utilities.GetDateTimeForDML(databaseType, dateRowSampleDate,true,true) + ",   " + createdBy + ",   " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " +
                    "      " + lastAmendedBy + ",   " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ")";
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        
        public static string update(string databaseType, long sequence, string formId, string formSubmissionId, string formTimeStamp, long jobSequence, long rowNo, string rowDesc,
                                    string rowRefNo, DateTime? dateRowSampleDate, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_eforms_ord_cee_wsr" +
                    "   SET  form_id =  '" + formId + "',  " +
                    "        form_submission_id =  '" + formSubmissionId + "',  " +
                    "        form_time_stamp =  '" + formTimeStamp + "',  " +
                    "        job_sequence =  " + jobSequence + ",  " +
                    "        row_no =  " + rowNo + ",  " +
                    "        row_desc =  '" + rowDesc + "',  " +
                    "        row_ref_no =  '" + rowRefNo + "',  " +
                    "        date_row_sample_date =  " + Utilities.GetDateTimeForDML(databaseType, dateRowSampleDate,true,true) + ", " +
                    "        created_by =  " + createdBy + ",  " +
                    "        date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " +
                    "        last_amended_by =  " + lastAmendedBy + ",  " +
                    "        date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " +
                    "  WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string delete(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "DELETE FROM   un_eforms_ord_cee_wsr " +
                              " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string deleteFlagDeleted(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                bool flg = true;
                returnValue = "UPDATE   un_eforms_ord_cee_wsr " +
                              "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                              " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

