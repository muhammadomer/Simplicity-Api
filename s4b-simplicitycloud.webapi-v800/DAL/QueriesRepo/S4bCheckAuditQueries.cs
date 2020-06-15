using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class S4bCheckAuditQueries
    {

        public static string insert(string databaseType,S4bCheckAudit obj )
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO un_s4b_check_audit(job_sequence,  check_type,  flg_passed,  de_sequence " +
                "   ,flg_self_isolation,date_self_isolation,self_isolation_notes,  created_by,  date_created " +
                "   ,last_amended_by,  date_last_amended) " +
                "VALUES (" + obj.JobSequence +  ", " + obj.CheckType + ", " + Utilities.GetBooleanForDML(databaseType, obj.FlgPassed)
                + ", " + obj.DeSequence +"," + Utilities.GetBooleanForDML(databaseType,obj.FlgSelfIsolation)
				+","+ Utilities.GetDateTimeForDML(databaseType,obj.DateSelfIsolation,true,true)
				+",'"+ obj.SelfIsolationNotes +"'"
                + ", " + obj.CreatedBy 
                + ", " + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated, true, true)
                + ", " + obj.LastAmendedBy 
                + ", " + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended, true, true) + ")";
                       
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

    }
}

