using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class S4bCheckAuditFailsQueries
    {

        public static string insert(string databaseType,S4bCheckAuditFails obj )
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO un_s4b_check_audit_fails(join_sequence,job_sequence,  check_type,  check_id " +
                "   ,  created_by,  date_created " +
                "   ,last_amended_by,  date_last_amended) " +
                "VALUES (" + obj.JoinSequence +"," + obj.JobSequence +  ", " + obj.CheckType + ", " + obj.CheckId
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

