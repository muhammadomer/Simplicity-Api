using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public class DiaryAppsWebAssignQueries
    {

        public static string SelectAllWebAssignApps(string databaseType)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "SELECT * FROM un_diary_apps_web_assign ";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string SelectWebAssignAppsByCriteria(string databaseType, DiaryAppsWebAssign webAssign)
        {
            string returnValue = "";
            try
            {
               
                 returnValue = string.Format("select * from  un_diary_apps_web_assign where de_sequence={0} and entity_id={1} and web_id={2} ", webAssign.DeSequence, webAssign.EntityId, webAssign.WebId);
                        
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string SelectWebAssignAppsByCriteria(string databaseType, DiaryAppsWebAssign webAssign, int webId)
        {
            string returnValue = "";
            try
            {
                
                returnValue = string.Format("select * from  un_diary_apps_web_assign where de_sequence={0} and entity_id={1} and web_id={2} ", webAssign.DeSequence, webAssign.EntityId, webId);
                        
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string InsertWebAssign(string databaseType, DiaryAppsWebAssign WebAssign)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                        returnValue = "insert into un_diary_apps_web_assign(de_sequence, resource_sequence, entity_id,  web_id, date_app_start, add_info, flg_complete,date_app_completed,flg_delay,delay_reason,created_by,date_created,last_amended_by,date_last_amended)";
                        returnValue += "values(" + WebAssign.DeSequence + ", " + WebAssign.ResourceSequence + ", " + WebAssign.EntityId + ", " + WebAssign.WebId + ", #" + ((DateTime)WebAssign.DateAppStart).ToString("dd/MM/yyyy HH:mm") + "#, '" + WebAssign.AddInfo + "', " + WebAssign.FlgComplete + ", ";
                        returnValue += "#" + ((DateTime)WebAssign.DateAppCompleted).ToString("dd/MM/yyyy") + "#, " + WebAssign.FlgDelay + ", '" + WebAssign.DelayReason + "', " + WebAssign.CreatedBy + ", #" + ((DateTime)WebAssign.DateCreated).ToString("dd/MM/yyyy HH:mm") + "#, " + WebAssign.LastAmendedBy + ", #" + ((DateTime)WebAssign.DateLastAmended).ToString("dd/MM/yyyy HH:mm") + "#)";
                        break;

                    case "SQLSERVER":
                    default:
                        var FlgComplete = WebAssign.FlgComplete == true ? 1 : 0;
                        var FlgDelay = WebAssign.FlgDelay == true ? 1 : 0;
                        returnValue = "insert into un_diary_apps_web_assign(de_sequence, resource_sequence, entity_id,  web_id, date_app_start, add_info, flg_complete,date_app_completed,flg_delay,delay_reason,created_by,date_created,last_amended_by,date_last_amended)";
                        returnValue += "values('" + WebAssign.DeSequence + "', " + WebAssign.ResourceSequence + ", '" + WebAssign.EntityId + "', '" + WebAssign.WebId + "', '" + WebAssign.DateAppStart + "', '" + WebAssign.AddInfo + "', '" + FlgComplete + "',";
                        returnValue += "'" + WebAssign.DateAppCompleted + "', '" + FlgDelay + "', '" + WebAssign.DelayReason + "', '" + WebAssign.CreatedBy + "', '" + WebAssign.DateCreated + "', '" + WebAssign.LastAmendedBy + "', '" + WebAssign.DateLastAmended + "')";
                        break;

                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string SelectQueryForTemplateURL(string databaseType, NaturalFormRequest naturalFormRequest)
        {
            string returnValue = "";
            try
            {
                switch (naturalFormRequest.Form.form_ref)
                {
                    case "1630957754":  // Woodvale Maintenance Third Party Job Ticket
                       
                                returnValue = "SELECT dr.resource_name, da.sequence AS da_sequence, " +
                                              "       da.join_resource, da.job_sequence, da.date_app_start, ord.job_ref, " +
                                              "       ord.job_client_id, ord.job_client_name, ord.job_client_ref, da.app_notes, " +
                                              "       ord.job_cost_centre, ord.job_trade_code, ord.job_address, ord.job_address_id, " +
                                              "       ord.occupier_name, ord.occupier_tel_home, ord.occupier_tel_work, " +
                                              "       ord.occupier_tel_work_ext, ord.occupier_tel_mobile, ord.occupier_email, " +
                                              "       ord.job_desc, ord.job_date_start, ord.date_set_to_jt, ord.job_priority_code, " +
                                              "       ord.job_date_due, ( " +
                                              "SELECT edc_m.name_long FROM un_entity_details_core AS edc_m " +
                                              " WHERE ord.job_manager = edc_m.entity_id) AS job_manager_name, " +
                                              "       edc_p.address_full AS job_address_vert, edc_p.property_upn, edc_b.entity_id, " +
                                              "       edc_b.flg_entity_join, edc_bp.entity_id AS landlord_id, " +
                                              "       edc_bp.name_long AS landlord_name, edc_bp.address_full AS landlord_address_vert, " +
                                              "       dawa.sequence AS dawa_sequence, dawa.entity_id AS dawa_entity_id, dawa.web_id, " +
                                              "       da.resource_sequence, dawa.resource_sequence " +
                                              " FROM (((((un_diary_resources AS dr " +
                                              "INNER JOIN un_diary_apps AS da ON dr.sequence = da.resource_sequence) " +
                                              "INNER JOIN un_orders AS ord ON da.job_sequence = ord.sequence) " +
                                              "INNER JOIN un_entity_details_core AS edc_p ON ord.job_address_id = edc_p.entity_id) " +
                                              "INNER JOIN un_entity_details_core AS edc_b  ON ord.job_client_id = edc_b.entity_id) " +
                                              "INNER JOIN un_entity_details_core AS edc_bp  ON edc_b.entity_join_id = edc_bp.entity_id) " +
                                              "INNER JOIN un_diary_apps_web_assign AS dawa ON da.sequence = dawa.de_sequence " +
                                              " WHERE dawa.sequence = " + naturalFormRequest.AppWebAssignSequence;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string UpdateWebAssign(string databaseType, DiaryAppsWebAssign WebAssign)
        {
            string returnValue = "";
            try
            {
                
                        returnValue = "UPDATE un_diary_apps_web_assign set de_sequence= " + WebAssign.DeSequence + ", " +
                                      "       resource_sequence = " + WebAssign.ResourceSequence + ", " +
                                      "       entity_id = " + WebAssign.EntityId + ", " +
                                      "       web_id = " + WebAssign.WebId + ", " +
                                      "       date_app_start = " + Utilities.GetDateTimeForDML(databaseType, WebAssign.DateAppStart,true,true) + ", " + 
                                      //"       add_info = '" + WebAssign.AddInfo + "', " +
                                      //"       flg_complete = " + WebAssign.FlgComplete + ", " +
                                      //"       date_app_completed = #" + WebAssign.DateAppCompleted.ToString("dd/MM/yyyy HH:mm") + "#, " +
                                      //"       flg_delay = " + WebAssign.FlgDelay + ", " +
                                      //"       delay_reason = '" + WebAssign.DelayReason + "', " + 
                                      "       last_amended_by = " + WebAssign.LastAmendedBy + ", " +
                                      "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, WebAssign.DateLastAmended,true,true)  +
                                      " where sequence = " + WebAssign.SequenceId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string GetThirdPartyApp(string databaseType, DiaryAppsWebAssign webAssign)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                        returnValue = "SELECT dawa.sequence, dawa.de_sequence, dawa.web_id, dawa.entity_id, " +
                                      "       dawa.date_app_start, da.resource_sequence, da.job_sequence, w3p.user_name, ord.job_ref, " +
                                      "       ord.job_client_ref, ord.job_date_due, " +
                                      "       ord.job_address, ord.job_client_name, edc_p.address_post_code, ord.job_desc, da.visit_status " +
                                      "  FROM (((un_diary_apps_web_assign AS dawa " +
                                      " INNER JOIN un_diary_apps AS da ON dawa.de_sequence = da.sequence) " +
                                      "INNER JOIN un_orders AS ord ON da.job_sequence = ord.sequence) " +
                                      "INNER JOIN un_web_3rd_parties AS w3p  ON dawa.web_id = w3p.web_id) " +
                                      "INNER JOIN un_entity_details_core AS edc_p ON ord.job_address_id = edc_p.entity_id " +
                                      " WHERE dawa.web_id = " + webAssign.WebId + " ";
                        if (webAssign.DateAppStart != null && webAssign.DateAppStart != DateTime.MinValue)
                        {
                            returnValue += "  AND dawa.date_app_start BETWEEN #" + ((DateTime)webAssign.DateAppStart).ToString("MM/dd/yyyy") + " 00:00:00# " +
                                           "  AND #" + ((DateTime)webAssign.DateAppStart).ToString("MM/dd/yyyy") + " 23:59:59#";
                        }
                        break;

                    case "SQLSERVER":
                    default:
                        returnValue = "SELECT dawa.sequence, dawa.de_sequence, dawa.web_id, dawa.entity_id, " +
                                      "       dawa.date_app_start, da.resource_sequence, da.job_sequence, w3p.user_name, ord.job_ref, " +
                                      "       ord.job_client_ref, ord.job_date_due, " +
                                      "       ord.job_address, ord.job_client_name, edc_p.address_post_code, ord.job_desc, da.visit_status " +
                                      "  FROM (((un_diary_apps_web_assign AS dawa " +
                                       " INNER JOIN un_diary_apps AS da ON dawa.de_sequence = da.sequence) " +
                                      "INNER JOIN un_orders AS ord ON da.job_sequence = ord.sequence) " +
                                      "INNER JOIN un_web_3rd_parties AS w3p  ON dawa.web_id = w3p.web_id) " +
                                      "INNER JOIN un_entity_details_core AS edc_p ON ord.job_address_id = edc_p.entity_id " +
                                      "WHERE dawa.web_id = " + webAssign.WebId + " ";
                        if(webAssign.DateAppStart != null && webAssign.DateAppStart!=DateTime.MinValue)
                        {
                            returnValue += "  AND dawa.date_app_start BETWEEN '" + ((DateTime)webAssign.DateAppStart).ToString("yyyy-MM-dd") + " 00:00:00' " +
                                           "  AND '" + ((DateTime)webAssign.DateAppStart).ToString("yyyy-MM-dd") + " 23:59:59'";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string UpdateWebAssignByCriteria(string databaseType, DiaryAppsWebAssign WebAssign)
        {
            string returnValue = "";
            try
            {   returnValue = "update un_diary_apps_web_assign set de_sequence=de_sequence ";
                        if (WebAssign.ActionType == ActionType.Delay)
                        {
                            returnValue += ",flg_delay =" + Utilities.GetBooleanForDML(databaseType, WebAssign.FlgDelay) + " ";
                            returnValue += ",delay_reason ='" + WebAssign.DelayReason + "' ";
                        }
                        else if (WebAssign.ActionType == ActionType.Notes)
                        {
                            returnValue += ", add_info ='" + WebAssign.AddInfo + "' ";
                        }
                        else if (WebAssign.ActionType == ActionType.AptCompleted)
                        {
                            returnValue += ",flg_complete =" + Utilities.GetBooleanForDML(databaseType, WebAssign.FlgComplete) + " ";
                        }
                        returnValue += ", last_amended_by = " + WebAssign.LastAmendedBy + ", ";
                        returnValue += " date_last_amended = " +  Utilities.GetDateTimeForDML(databaseType,WebAssign.DateLastAmended,true,true) ;
                        returnValue += " where sequence = " + WebAssign.SequenceId;
                        
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}
