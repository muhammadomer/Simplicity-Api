using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrderStatusAuditQueries
    {

        public static string getSelectAllBySequence(long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                              "  FROM    un_order_status_audit" +
                              " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string insert(string databaseType, long jobSequence, long statusType, bool flgJobClientId, long jobClientId, bool flgStatusRef, string statusRef, DateTime? dateStatusRef, 
                                   string statusRef2, string statusDesc, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO un_order_status_audit(job_sequence,  status_type,  flg_job_client_id,  job_client_id, " +
                                      "       flg_status_ref,  status_ref,  date_status_ref, status_ref2,  status_desc, " +
                                      "       created_by,  date_created,  last_amended_by,  date_last_amended) " +
                                      "VALUES (" + jobSequence + ", " + statusType + ", " + Utilities.GetBooleanForDML(databaseType, flgJobClientId) + ", " + jobClientId + ", " +
                                      "       " + Utilities.GetBooleanForDML(databaseType, flgStatusRef) + ", '" + statusRef + "', " + Utilities.GetDateTimeForDML(databaseType, dateStatusRef, true, true) + ", " +
                                      "      '" + statusRef2 + "', '" + statusDesc + "', " + createdBy + ", " +
                                      "      " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " + lastAmendedBy + ", " +
                                      "       " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string update(string databaseType, long sequence, long jobSequence, long statusType, bool flgJobClientId, long jobClientId, bool flgStatusRef, string statusRef, DateTime? dateStatusRef,
                                   string statusRef2, string statusDesc, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_order_status_audit" +
                                 "   SET  job_sequence =  " + jobSequence + ",  " +
                                 "        status_type =  " + statusType + ",  " +
                                 "        flg_job_client_id = " + Utilities.GetBooleanForDML(databaseType, flgJobClientId) + ",  " +
                                 "        job_client_id =  " + jobClientId + ",  " +
                                 "        flg_status_ref = " + Utilities.GetBooleanForDML(databaseType, flgStatusRef) + ",  " +
                                 "        status_ref =  '" + statusRef + "',  " +
                                 "        date_status_ref =  " + Utilities.GetDateTimeForDML(databaseType, dateStatusRef,true,true) + ", " +
                                 "        status_ref2 =  '" + statusRef2 + "',  " +
                                 "        status_desc =  '" + statusDesc + "',  " +
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


        public static string delete(long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "DELETE FROM   un_order_status_audit" +
                              "WHERE sequence = " + sequence;
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
                returnValue = "UPDATE   un_order_status_audit" +
                              "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg) 
                              +" WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

