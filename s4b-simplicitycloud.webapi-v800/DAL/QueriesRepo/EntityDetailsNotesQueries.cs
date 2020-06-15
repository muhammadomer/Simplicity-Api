using System;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class EntityDetailsNotesQueries
    {

        public static string getAllBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT * 
                              FROM    un_edc_notes 
                              WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getAllByEntityId(string databaseType, long entityId)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT en.sequence, en.entity_id, en.entity_notes,
                    en.created_by, en.date_created, en.last_amended_by, en.date_last_amended,
                    ud.user_name, ud.user_logon
                FROM un_edc_notes AS en
                    LEFT JOIN un_user_details AS ud ON en.created_by = ud.user_id
                WHERE en.entity_id = " + entityId
                + " ORDER BY en.sequence DESC";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public static string insert(string databaseType, long entityId, string entityNotes, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"INSERT INTO un_edc_notes(entity_id, entity_notes, created_by, date_created, last_amended_by,  date_last_amended) 
                     VALUES (" + entityId + ", '" + entityNotes + "', " + createdBy + ", " + 
                     Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " + lastAmendedBy + ", " +
                     Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
        }

        public static string update(string databaseType, long sequence, long entityId, string entityNotes, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_edc_notes " +
                                     "   Set    entity_id =  " + entityId + ", " +
                                     "       entity_notes =  '" + entityNotes + "', " +
                                     "       last_amended_by =  " + lastAmendedBy + ", " +
                                     "       date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true)  +
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
                returnValue = @"DELETE FROM un_edc_notes 
                             WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

       
    }
}

