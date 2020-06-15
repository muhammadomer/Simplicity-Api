using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class S4bFormsAssignQueries
    {

        public static string getAllAssignUser(string databaseType, long FormSeq)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT un_s4b_forms_assign.sequence, un_s4b_forms_assign.user_id, un_user_details.user_name
                        FROM un_s4b_forms_assign INNER JOIN un_user_details ON un_s4b_forms_assign.user_id = un_user_details.user_id
                        Where  un_s4b_forms_assign.form_sequence = " + FormSeq 
                        +" ORDER BY un_user_details.user_name" ;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getUnAssignUsers(string databaseType, long FormSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT un_user_details.user_id, un_user_details.user_name
                FROM un_user_details
                    INNER JOIN un_diary_resources AS dr ON un_user_details.resource_sequence = dr.sequence
                Where  un_user_details.user_id not in (Select user_id From un_s4b_forms_assign where form_sequence = " + FormSequence +@") 
                And un_user_details.user_id not in (1,3,5) And un_user_details.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true)
                + " ORDER BY un_user_details.user_name"; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getUnAssignUser(string databaseType, long UserId)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = " SELECT * " +
                                     "  FROM    un_user_details" +
                                     " WHERE user_id <> " + UserId;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        
        public static string insert(string databaseType, long formSeq, long userId, long createdBy, DateTime? dateCreated)
        {
            string returnValue = "";

           
                    returnValue = "INSERT INTO un_s4b_forms_assign (form_sequence, user_id, created_by, date_created)" +
                          "VALUES (" + formSeq + ", " + userId +  "," + createdBy + "," + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ")";
                   

            return returnValue;
        }

        public static string delete(string databaseType, long sequence)
        {
            string returnValue = "";
           
                    returnValue = "DELETE FROM un_s4b_forms_assign " +
                         " WHERE sequence = " + sequence;

            return returnValue;
        }
    }
}

