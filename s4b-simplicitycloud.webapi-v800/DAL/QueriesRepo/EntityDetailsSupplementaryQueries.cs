using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class EntityDetailsSupplementaryQueries
    {
        public static string getSelectAllBySequence(long entityId)
        {
            return @"SELECT * 
                   FROM un_entity_details_supplementary
                   WHERE entity_id = " + entityId;
        }

        public static string insert (long entityId, string dataType,  string data)
        {
            return @"INSERT INTO un_entity_details_supplementary(entity_id, data_type,  data) 
                   VALUES(" + entityId + ", '" + dataType + "', '" + Utilities.replaceSpecialChars(data)  +"')";
        }

        public static string update(long entityId, string dataType, string data)
        {
            return @"UPDATE un_entity_details_supplementary 
                   SET data_type = '" + dataType + "', " 
                   + "        data = '" + data + "' " 
                   + " WHERE data_type = '" + dataType + "' and entity_id = " + entityId;
        }

        public static string delete(string databaseType, long entityId)
        {
            return "DELETE FROM  un_entity_details_supplementary WHERE entity_id = " + entityId;
        }

        public static string deleteFlagDeleted(string databaseType, long sequence)
        {
            return "UPDATE un_entity_details_supplementary  SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, true) + " " +
                   " WHERE entity_id = " + sequence;
        }
    }
}

