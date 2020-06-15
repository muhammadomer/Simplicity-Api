using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrdersDistributionListQueries
    {

        public static string getSelectAllByJobSequence(string databaseType, long jobSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT * 
                              FROM    un_orders_distribution_list
                              WHERE job_sequence = " + jobSequence  +
                          "   AND flg_deleted <>" + Utilities.GetBooleanForDML(databaseType, true);
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string insert(string databaseType, bool flgDeleted, long jobSequence, string emailName, string emailAddress, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO  un_orders_distribution_list(flg_deleted,  job_sequence,  email_name, " +
                                      "       email_address,  created_by,  date_created,  last_amended_by,  date_last_amended) " +
                                      "VALUES (" + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",   " + jobSequence + ",   '" + emailName + "',  " +
                                      "       '" + emailAddress + "', " + createdBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " +
                                      "       " + lastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string update(string databaseType, long sequence, bool flgDeleted, long jobSequence, string emailName, string emailAddress, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_orders_distribution_list" +
                                 "   SET  flg_deleted = " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",  " +
                                 "        job_sequence =  " + jobSequence + ",  " +
                                 "        email_name =  '" + emailName + "',  " +
                                 "        email_address =  '" + emailAddress + "',  " +
                                 "        created_by =  " + createdBy + ",  " +
                                 "        date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " +
                                 "        last_amended_by =  " +  lastAmendedBy + ",  " +
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
                returnValue = "DELETE FROM   un_orders_distribution_list" +
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
                returnValue = "UPDATE   un_orders_distribution_list" +
                              "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                              "WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

