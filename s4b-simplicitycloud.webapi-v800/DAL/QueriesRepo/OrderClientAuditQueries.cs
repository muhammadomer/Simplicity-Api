using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrderClientAuditQueries
    {

        public static string getSelectAllByJobSequence(long JobSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT * 
                              FROM    un_order_client_audit
                              WHERE job_sequence = " + JobSequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getSelectActiveByJobSequence(string databaseType,long JobSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT Top 1 *
                              FROM    un_order_client_audit
                              WHERE flg_phase_finish = " + Utilities.GetBooleanForDML(databaseType,false) +" and job_sequence = " + JobSequence
                            + " Order by sequence desc";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string insert(string databaseType, OrderClientAudit orderClientAudit, long createdBy, DateTime? dateCreated)
        {
            string returnValue = "";
            try
            {

                returnValue = @"INSERT INTO un_order_client_audit(job_sequence,  job_client_id
                    , phase_type,flg_phase_ref,  phase_ref 
                    , flg_phase_start,  date_phase_start, phase_start_desc 
                    , flg_phase_finish, date_phase_finish, phase_finish_desc 
                    , created_by,  date_created)
                VALUES (" + orderClientAudit.JobSequence + ", " + orderClientAudit.JobClientId
                    + "," + (orderClientAudit.PhaseType.HasValue ? orderClientAudit.PhaseType.Value : -1)
                    + ", " + Utilities.GetBooleanForDML(databaseType, orderClientAudit.FlgPhaseRef)
                    + ",'" + (String.IsNullOrEmpty(orderClientAudit.PhaseRef) ? " " : Utilities.replaceSpecialChars(orderClientAudit.PhaseRef)) + "'"
                    + " ," + Utilities.GetBooleanForDML(databaseType, orderClientAudit.FlgPhaseStart)
                    + "," + Utilities.GetDateTimeForDML(databaseType, orderClientAudit.DatePhaseStart, true, true)
                    + ",'" + (String.IsNullOrEmpty(orderClientAudit.PhaseStartDesc) ? " " : Utilities.replaceSpecialChars(orderClientAudit.PhaseStartDesc)) + "'"
                    + " ," + Utilities.GetBooleanForDML(databaseType, orderClientAudit.FlgPhaseFinish)
                    + "," + Utilities.GetDateTimeForDML(databaseType, orderClientAudit.DatePhaseFinish, true, true)
                    + ",'" + (String.IsNullOrEmpty(orderClientAudit.PhaseFinishDesc) ? " " : Utilities.replaceSpecialChars(orderClientAudit.PhaseFinishDesc)) + "'"
                    + ", " + createdBy
                    + ", " + Utilities.GetDateTimeForDML(databaseType,dateCreated,true,true) + ")";
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string update(string databaseType, OrderClientAudit orderClientAudit, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_order_client_audit" +
                                 "  SET  job_sequence =  " + orderClientAudit.JobSequence 
                               + ", job_client_id =  " + orderClientAudit.JobClientId
                               + ", phase_type =  " + (orderClientAudit.PhaseType.HasValue ? orderClientAudit.PhaseType.Value : -1) 
                               +" , flg_phase_ref = " + Utilities.GetBooleanForDML(databaseType, orderClientAudit.FlgPhaseRef)
                               +" , phase_ref =  '" + (String.IsNullOrEmpty(orderClientAudit.PhaseRef) ? " " : Utilities.replaceSpecialChars(orderClientAudit.PhaseRef)) + "'"
                               +" , flg_phase_start =" + Utilities.GetBooleanForDML(databaseType, orderClientAudit.FlgPhaseStart)
                               +" , date_phase_start =  " + Utilities.GetDateTimeForDML(databaseType, orderClientAudit.DatePhaseStart, true, true) 
                               + ", phase_start_desc ='"  + (String.IsNullOrEmpty(orderClientAudit.PhaseStartDesc) ? " " : Utilities.replaceSpecialChars(orderClientAudit.PhaseStartDesc)) + "'"
                               + ", flg_phase_finish = " + Utilities.GetBooleanForDML(databaseType, orderClientAudit.FlgPhaseFinish)
                               + ", date_phase_finish = " + Utilities.GetDateTimeForDML(databaseType, orderClientAudit.DatePhaseFinish, true, true)
                               + ", phase_finish_desc='" + (String.IsNullOrEmpty(orderClientAudit.PhaseFinishDesc) ? " " : Utilities.replaceSpecialChars(orderClientAudit.PhaseFinishDesc)) + "'"
                               +" , last_amended_by =  " + lastAmendedBy 
                               +" , date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true)
                               + "  WHERE sequence = " + orderClientAudit.Sequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string updateActiveToFinishByJobSequence(string databaseType, long jobSequence, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_order_client_audit" +
                                 "  SET  flg_phase_finish = " + Utilities.GetBooleanForDML(databaseType, true)
                               + ", date_phase_finish = " + Utilities.GetDateTimeForDML(databaseType, DateTime.Now, true, true)
                               + " , last_amended_by =  " + lastAmendedBy
                               + " , date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true)
                               + "  WHERE job_sequence = " + jobSequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }


        public static string delete(long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "DELETE FROM   un_order_client_audit" +
                              "WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

