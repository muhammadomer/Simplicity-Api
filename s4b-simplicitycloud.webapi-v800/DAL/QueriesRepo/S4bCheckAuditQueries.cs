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
        public static string getSelectCheckAuditList(string databaseType, ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate, long jobSequence)
        {
            string returnValue = "", whereClause = "";
            try
            {
                if (jobSequence > 0)
                    whereClause = " and chkAudit.job_sequence=" + jobSequence;
                switch (databaseType)
                {
                    case "MSACCESS":
                        whereClause = " and (chkAudit.date_created Between #" + ((DateTime)fromDate).ToString("MM/dd/yyyy") + " 00:00:00# " +
                        "   AND  #" + ((DateTime)toDate).ToString("MM/dd/yyyy") + " 23:59:59#)";
                        break;
                    case "SQLSERVER":
                    default:
                        whereClause = " and (chkAudit.date_created Between '" + ((DateTime)fromDate).ToString("yyyy-MM-dd") + " 00:00:00'" +
                       "   AND '" + ((DateTime)toDate).ToString("yyyy-MM-dd") + " 23:59:59')";
                        break;
                }
                returnValue = @"SELECT users.user_name, chkAudit.sequence,  chkAudit.check_type,chkAudit.flg_passed 
                    ,chkAudit.date_self_isolation, chkAudit.self_isolation_notes
                    ,chkAudit.date_created, un_orders.job_ref, un_orders.job_address, address.address_post_code
                FROM   ((un_s4b_check_audit AS chkAudit 
                INNER JOIN  un_orders ON chkAudit.job_sequence = un_orders.sequence) 
						LEFT OUTER JOIN un_user_details users on chkAudit.Created_by=users.user_id)
                        LEFT OUTER JOIN un_entity_details_core AS address ON un_orders.job_address_id = address.entity_id
                Where 1=1 " + whereClause;
                if (clientRequest.globalFilter != "" && clientRequest.globalFilter != null)
                {
                    string filterValue = clientRequest.globalFilter;
                    string globalFilterQuery = " And( un_orders.job_ref like '%" + filterValue + "%'"
                        + " or users.user_name like '%" + filterValue + "%'"
                        + " or chkAudit.check_type like '%" + filterValue +"%')";
                    returnValue += globalFilterQuery;
                }
                returnValue += " Order by chkAudit.date_created Desc";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
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

