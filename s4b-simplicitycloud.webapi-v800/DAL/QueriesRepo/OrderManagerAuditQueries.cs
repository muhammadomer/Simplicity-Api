using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrderManagerAuditQueries
    {

        public static string getSelectAllByJobSequence(long JobSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT * 
                              FROM    un_order_manager_audit
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
                              FROM    un_order_manager_audit
                              WHERE flg_phase_finish = " + Utilities.GetBooleanForDML(databaseType,false) +" and job_sequence = " + JobSequence
                            + " Order by sequence desc";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string insert(string databaseType, OrderManagerAudit orderManagerAudit, long createdBy, DateTime? dateCreated)
        {
            string returnValue = "";
            try
            {

                returnValue = @"INSERT INTO un_order_manager_audit(job_sequence,  job_manager_id
                    , phase_type,flg_phase_ref,  phase_ref 
                    , flg_phase_start,  date_phase_start, phase_start_desc 
                    , flg_phase_finish, date_phase_finish, phase_finish_desc 
                    , created_by,  date_created)
                VALUES (" + orderManagerAudit.JobSequence + ", " + orderManagerAudit.JobManagerId
                    + "," + (orderManagerAudit.PhaseType.HasValue ? orderManagerAudit.PhaseType.Value : -1)
                    + ", " + Utilities.GetBooleanForDML(databaseType, orderManagerAudit.FlgPhaseRef)
                    + ",'" + (String.IsNullOrEmpty(orderManagerAudit.PhaseRef) ? " " : Utilities.replaceSpecialChars(orderManagerAudit.PhaseRef)) + "'"
                    + " ," + Utilities.GetBooleanForDML(databaseType, orderManagerAudit.FlgPhaseStart)
                    + "," + Utilities.GetDateTimeForDML(databaseType, orderManagerAudit.DatePhaseStart, true, true)
                    + ",'" + (String.IsNullOrEmpty(orderManagerAudit.PhaseStartDesc) ? " " : Utilities.replaceSpecialChars(orderManagerAudit.PhaseStartDesc)) + "'"
                    + " ," + Utilities.GetBooleanForDML(databaseType, orderManagerAudit.FlgPhaseFinish)
                    + "," + Utilities.GetDateTimeForDML(databaseType, orderManagerAudit.DatePhaseFinish, true, true)
                    + ",'" + (String.IsNullOrEmpty(orderManagerAudit.PhaseFinishDesc) ? " " : Utilities.replaceSpecialChars(orderManagerAudit.PhaseFinishDesc)) + "'"
                    + ", " + createdBy
                    + ", " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ")";
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string update(string databaseType, OrderManagerAudit orderManagerAudit, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_order_manager_audit" +
                                 "  SET  job_sequence =  " + orderManagerAudit.JobSequence 
                               + ", job_manager_id =  " + orderManagerAudit.JobManagerId
                               + ", phase_type =  " + (orderManagerAudit.PhaseType.HasValue ? orderManagerAudit.PhaseType.Value : -1) 
                               +" , flg_phase_ref = " + Utilities.GetBooleanForDML(databaseType, orderManagerAudit.FlgPhaseRef)
                               +" , phase_ref =  '" + (String.IsNullOrEmpty(orderManagerAudit.PhaseRef) ? " " : Utilities.replaceSpecialChars(orderManagerAudit.PhaseRef)) + "'"
                               +" , flg_phase_start =" + Utilities.GetBooleanForDML(databaseType, orderManagerAudit.FlgPhaseStart)
                               +" , date_phase_start =  " + Utilities.GetDateTimeForDML(databaseType, orderManagerAudit.DatePhaseStart, true, true) 
                               + ", phase_start_desc ='"  + (String.IsNullOrEmpty(orderManagerAudit.PhaseStartDesc) ? " " : Utilities.replaceSpecialChars(orderManagerAudit.PhaseStartDesc)) + "'"
                               + ", flg_phase_finish = " + Utilities.GetBooleanForDML(databaseType, orderManagerAudit.FlgPhaseFinish)
                               + ", date_phase_finish = " + Utilities.GetDateTimeForDML(databaseType, orderManagerAudit.DatePhaseFinish, true, true)
                               + ", phase_finish_desc='" + (String.IsNullOrEmpty(orderManagerAudit.PhaseFinishDesc) ? " " : Utilities.replaceSpecialChars(orderManagerAudit.PhaseFinishDesc)) + "'"
                               +" , last_amended_by =  " + lastAmendedBy 
                               +" , date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true )
                               + "  WHERE sequence = " + orderManagerAudit.Sequence;
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
                returnValue = "UPDATE  un_order_manager_audit" +
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
                returnValue = "DELETE FROM   un_order_manager_audit" +
                              "WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

