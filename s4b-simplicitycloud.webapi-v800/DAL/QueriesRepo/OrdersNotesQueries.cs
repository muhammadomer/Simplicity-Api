using System;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrdersNotesQueries
    {

        public static string getSelectAllBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT * 
                              FROM    un_orders_notes 
                              WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getSelectAllByJobSequence(string databaseType, long jobSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT uon.sequence, uon.order_notes,uon.date_created,uon.date_last_amended,ud.user_name, ud.user_logon
                FROM    un_orders_notes AS uon
                    LEFT JOIN un_user_details AS ud ON uon.created_by = ud.user_id
                WHERE uon.job_sequence = " + jobSequence
                +" ORDER BY uon.sequence DESC";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public static string insert(string databaseType, long jobSequence, string orderNotes, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"INSERT INTO un_orders_notes(job_sequence, order_notes, created_by, date_created, last_amended_by,  date_last_amended) 
                     VALUES (" + jobSequence + ", '" + orderNotes + "', " + createdBy + ", " + 
                     Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " + lastAmendedBy + ", " +
                     Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
        }

        public static string update(string databaseType, long sequence, long jobSequence, string orderNotes, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_orders_notes " +
                                     "   SET sequence =  " + sequence + ", " +
                                     "       job_sequence =  " + jobSequence + ", " +
                                     "       order_notes =  '" + orderNotes + "', " +
                                     "       last_amended_by =  " + lastAmendedBy + ", " +
                                     "       date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ", " +
                                     " WHERE sequence = " + sequence;
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
                returnValue = @"DELETE FROM un_orders_notes 
                             WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string deleteFlagDeleted(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_orders_notes " +
                                     "   SET flg_deleted =   " + Utilities.GetBooleanForDML(databaseType, true) +
                                     " WHERE sequence = " + sequence;
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

