using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class RefJobStatusTypeQueries
    {

        public static string getSelectAllBystatusId(string databaseType, long statusId)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "SELECT * " +
                                      "  FROM    un_ref_job_status_type" +
                                      " WHERE status_id = " + statusId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAll(string databaseType)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = @" SELECT * 
                                      FROM  un_ref_job_Status_type
                                      Order by status_index";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, long statusId, bool flgDeleted, long statusIndex, string statusDesc, bool flgAutoUpdate, long orderFieldId, bool flgLinkToDiaryApps,
                                    bool flgCompletedStatus)
        {
            string returnValue = "";
            try
            {

               
                        returnValue = "INSERT INTO   un_ref_job_status_type(status_id,  flg_deleted,  status_index,  status_desc,  flg_auto_update,  order_field_id,  flg_link_to_diary_apps)" +
                                      "                                     flg_completed_status" +
                                      "VALUES (  '" + statusId + "',   " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",  " + statusIndex + ",   '" + statusDesc + "',   "
                                      + Utilities.GetBooleanForDML(databaseType, flgAutoUpdate) + ",  " + orderFieldId + ",   "
                                      + Utilities.GetBooleanForDML(databaseType, flgLinkToDiaryApps) + ",   " + Utilities.GetBooleanForDML(databaseType, flgCompletedStatus) + ")";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string update(string databaseType, long statusId, bool flgDeleted, long statusIndex, string statusDesc, bool flgAutoUpdate, long orderFieldId, bool flgLinkToDiaryApps,
                                    bool flgCompletedStatus)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "UPDATE   un_ref_job_status_type" +
                                      "   SET  flg_deleted = " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",  " +
                                      " status_index =  " + statusIndex + ",  " +
                                      " status_desc =  '" + statusDesc + "',  " +
                                      " flg_auto_update = " + Utilities.GetBooleanForDML(databaseType, flgAutoUpdate) + ",  " +
                                      " order_field_id =  " + orderFieldId + ",  " +
                                      " flg_link_to_diary_apps = " + Utilities.GetBooleanForDML(databaseType, flgLinkToDiaryApps) + ",  " +
                                      " flg_completed_status = " + Utilities.GetBooleanForDML(databaseType, flgCompletedStatus) + ",  " +
                                      "  WHERE status_id = " + statusId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string delete(string databaseType, long statusId)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "DELETE FROM   un_ref_job_status_type" +
                                      "WHERE status_id = " + statusId;
            }

            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string deleteFlagDeleted(string databaseType, long statusId)
        {
            string returnValue = "";
            try
            {
                        bool flg = true;
                        returnValue = "   UPDATE   un_ref_job_status_type" +
                                      "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                                      "   WHERE status_id = " + statusId;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

