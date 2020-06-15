using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class PassthroughQueries
    {
        public static string SelectAllFieldsByPassthroughString(string databaseType, string passthroughString)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "SELECT * " +
                                      "  FROM un_attach_folder_passthrough " +
                                      " WHERE passthroughString = '" + passthroughString + "'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string SelectAllFieldsBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "SELECT * " +
                                      "  FROM un_attach_folder_passthrough " +
                                      " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string DeleteBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "DELETE " +
                                "  FROM un_attach_folder_passthrough " +
                                " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, string passthroughString, long jobSequence, long jobClientId, long jobAddressId, int entityId, bool flagAdminMode, string componentName, int createdBy, DateTime? createdDate)
        {
            string returnValue = "";

                    returnValue = "INSERT INTO un_attach_folder_passthrough(" +
                        "passthorugh_string," +
                        "job_sequence, " +
                        "job_client_id, " +
                        "job_address_id, " +
                        "entity_id, " +
                        "flg_admin_mode, " +
                        "created_by, " +
                        "component_name, " +
                        "date_created) " +

                         "VALUES ('" + passthroughString + "', " +
                         jobSequence + ", " +
                         jobClientId + ", " +
                         jobAddressId + ", " +
                         entityId + ", " +
                         Utilities.GetBooleanForDML(databaseType, flagAdminMode ) + ", " +
                         createdBy + ", '" +
                         componentName + "', " +
                         Utilities.GetDateTimeForDML(databaseType, createdDate,true,true) + ") ";



            return returnValue;
        }

    }
}
