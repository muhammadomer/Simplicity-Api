using System;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineDAL.QueriesRepo
{
    public static class PayrollJoinQueries
    {
        public static string Select(string databaseType, long sequence)
        {
            string returnValue = @"SELECT * FROM un_payroll_join WHERE sequence > 0 ";
            if (sequence > 0)
            {
                returnValue = returnValue + " AND sequence = " + sequence;
            }
            return returnValue;
        }

        public static string Insert(string databaseType, long prReference,  string prFullName,  long entityId,  int userId,  long webViewerId,  double lunchBreak,
                                    double hoursPerWeek,  string hoursDesc,  double pcentJobCostUplift,  int createdBy,  DateTime? dateCreated, int lastAmendedBy,  
                                    DateTime? dateLastAmended)
        {
            return @"INSERT INTO un_payroll_join(pr_reference, pr_full_name, entity_id, user_id, web_viewer_id, lunch_break, hours_per_week,
                            hours_desc, pcent_job_cost_uplift, created_by, date_created, last_amended_by, date_last_amended)
                    VALUES (" + prReference + ", '" +  prFullName + "', " +  entityId + ", " +  userId + ", " +  webViewerId + ",  '" +  
                           lunchBreak + "',  " +  hoursPerWeek + ", '" +  hoursDesc + "', " +  pcentJobCostUplift + ",  " +  createdBy + ",   " + 
                           Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " +  lastAmendedBy + ",  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
        }

        public static string Update(string databaseType, long sequence, long prReference, string prFullName, long entityId, int userId, long webViewerId, double lunchBreak,
                                    double hoursPerWeek, string hoursDesc, double pcentJobCostUplift, int createdBy, DateTime? dateCreated, int lastAmendedBy,
                                    DateTime? dateLastAmended)
        {
            string returnValue = "";
            returnValue = "UPDATE un_payroll_join " +
                          "   SET pr_reference = " +  prReference + ", " + 
		                  "       pr_full_name = '" +  prFullName + "',  " + 
   		                  "       entity_id =  " +  entityId + ",  " + 
		                  "       user_id = " + userId + ",  " + 
		                  "       web_viewer_id =  " +  webViewerId + ", " + 
		                  "       lunch_break =  " +  lunchBreak + ", " + 
		                  "       hours_per_week =  " +  hoursPerWeek + ", " + 
		                  "       hours_desc =  '" +  hoursDesc + "',  " + 
		                  "       pcent_job_cost_uplift =  '" +  pcentJobCostUplift + "',  " + 
		                  "       created_by =  " +  createdBy + ",  " + 
		                  "       date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " + 
		                  "       last_amended_by =  " +  lastAmendedBy + ",  " + 
		                  "       date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ", " + 
                          " WHERE sequence = " + sequence;
            return returnValue;
        }

        public static string Delete(string databaseType, long sequence)
        {
            return "DELETE FROM un_payroll_join WHERE sequence = " + sequence;               
        }
    }
}

