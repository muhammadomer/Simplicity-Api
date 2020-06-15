using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class EntityDetailsJoinQueries
    {

        public static string getSelectAllBySequence(long entityId)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                              "  FROM    un_entity_details_join" +
                              " WHERE entity_id = " + entityId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string insert(long entityId,string transType)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO   un_entity_details_join(entity_id, trans_type )" +
                              "VALUES (" + entityId + ", '" + transType + "')";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string update(long entityId, string transType)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_entity_details_join" +
                                 "   SET  trans_type =  '" + transType + "',  " +
                                 "  WHERE entity_id = " + entityId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string delete(long entityId)
        {
            string returnValue = "";
            try
            {
                returnValue = "DELETE FROM   un_entity_details_join" +
                              "WHERE entity_id = " + entityId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string deleteFlagDeleted(string databaseType, long entityId)
        {
            string returnValue = "";
            try
            {
                bool flg = true;
                returnValue = "UPDATE   un_entity_details_join" +
                              "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg) + ", " +
                              "WHERE entity_id = " + entityId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

