using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class DiaryResourcesQueries
    {

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "SELECT * FROM    un_diary_resources  WHERE sequence = " + Sequence;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByUserId(string databaseType, long userId)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "SELECT dr.*, edc.name_short " +
                    "  FROM (un_user_details AS ud " +
                    " INNER JOIN un_diary_resources AS dr  ON ud.resource_sequence = dr.sequence) " +
                    " INNER JOIN un_entity_details_core AS edc ON dr.join_resource = edc.entity_id " +
                    " WHERE ud.user_id = " + userId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getAllDiaryResources(string databaseType)
        {
            return @"SELECT * FROM un_diary_resources 
                     WHERE flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) +
                    " ORDER BY resource_display_order, sequence ";
        }

        public static string getDiaryResourceContactByResourceId(string databaseType,long resourceSequence)
        {
            return @"SELECT dr.sequence, dr.resource_name, edc.name_long , edc.telephone, edc.tel_ext, edc.tel_fax, edc.tel_mobile, edc.tel_work, edc.email
            FROM (un_diary_resources AS dr INNER JOIN un_diary_apps AS da ON dr.sequence = da.resource_sequence) 
                INNER JOIN un_entity_details_core AS edc ON dr.join_resource = edc.entity_id
            WHERE da.sequence = " + resourceSequence;
        }

        public static string getResourceNotesByEntityId(string databaseType, long entityId)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT da.sequence, da.date_app_start, da.app_location, da.app_subject, da.app_notes,dr.resource_name
                FROM un_diary_resources AS dr, un_diary_apps AS da
                WHERE dr.sequence = da.resource_sequence And da.flg_use_client_id = " + Utilities.GetBooleanForDML(databaseType, true) 
                +" AND da.client_id = " + entityId
                +" ORDER BY da.date_app_start DESC";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string insert(string databaseType, bool flgDeleted, string resourceName, long resourceStatus, long resourceDisplayOrder, long resourceGroup,
                                    long resourceType, long joinResource, string resourceNotes, long resourceLogOnId, long createdBy,
                                    DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO   un_diary_resources(flg_deleted,  resource_name,  resource_status,  resource_display_order,  resource_group," +
                              "                                 resource_type,  join_resource,  resource_notes,  resource_log_on_id,  created_by,  date_created," +
                              "                                 last_amended_by,  date_last_amended " +
                              "VALUES (" + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",   '" + resourceName + "',   " + resourceStatus + ",   " + resourceDisplayOrder + ",   " +
                                        resourceGroup + ",   " + resourceType + ",   " + joinResource + ",   '" + resourceNotes + "',   " +
                                        resourceLogOnId + ",   " + createdBy + ",   " +  Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ",   " +
                                        lastAmendedBy + ",   " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ")";

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string update(string databaseType,long sequence, bool flgDeleted, string resourceName, long resourceStatus, long resourceDisplayOrder, long resourceGroup,
                                    long resourceType, long joinResource, string resourceNotes, long resourceLogOnId, long createdBy,
                                    DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_diary_resources" +
                            "   SET  flg_deleted = " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",  " +
                            "        resource_name = ' " + resourceName + "',  " +
                            "        resource_status =  " + resourceStatus + ",  " +
                            "        resource_display_order =  " + resourceDisplayOrder + ",  " +
                            "        resource_group =  " + resourceGroup + ",  " +
                            "        resource_type =  " + resourceType + ",  " +
                            "        join_resource =  " + joinResource + ",  " +
                            "        resource_notes = ' " + resourceNotes + "',  " +
                            "        resource_log_on_id =  " + resourceLogOnId + ",  " +
                            "        created_by =  " + createdBy + ",  " +
                            "        date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " +
                            "        last_amended_by =  " + lastAmendedBy + ",  " +
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
               
                        returnValue = "DELETE FROM   un_diary_resources" +
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
                        returnValue = "UPDATE   un_diary_resources" +
                                      "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg) +
                                      " WHERE sequence = " + sequence;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string SelectResourceVAMCostRateBySequence(string databaseType, long sequence)
        {
            return @"SELECT dr.sequence, dr.resource_name,  edc.name_short, edj.trans_type, 
                            edvcr.vam_cost_rate_sequence, edvcr.vam_cost_rate 
                      FROM ((un_diary_resources AS dr 
                     INNER JOIN un_entity_details_core AS edc ON dr.join_resource = edc.entity_id) 
                     INNER JOIN un_entity_details_join AS edj ON edc.entity_id = edj.entity_id) 
                     INNER JOIN un_entity_details_vam_cost_rates AS edvcr ON edc.entity_id = edvcr.entity_id 
                     WHERE dr.sequence = " + sequence + 
                   "   AND edj.trans_type = '" + SimplicityConstants.ContactTransType + "'" +
                   "   AND edvcr.vam_cost_rate_sequence = 1";
        }
    }
}

