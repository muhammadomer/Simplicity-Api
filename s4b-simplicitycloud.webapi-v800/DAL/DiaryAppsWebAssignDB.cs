using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.BLL.Entities;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System.Dynamic;
using Newtonsoft.Json;

namespace SimplicityOnlineWebApi.DAL
{
    public class DiaryAppsWebAssignDB : MainDB
    {
        private bool IsJobDetails = false;
        public DiaryAppsWebAssignDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        internal List<DiaryAppsWebAssign> GetAllWebAssignApp()
        {
            List<DiaryAppsWebAssign> returnObj = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsWebAssignQueries.SelectAllWebAssignApps(this.DatabaseType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnObj = new List<DiaryAppsWebAssign>();
                                while (dr.Read())
                                {
                                    returnObj.Add(LoadWebAppsWebAssign(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while getting details. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnObj;
        }

        internal DiaryAppsWebAssign IsWebAssignAppExists(DiaryAppsWebAssign webAssign)
        {
            DiaryAppsWebAssign returnObj = null; 
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsWebAssignQueries.SelectWebAssignAppsByCriteria(this.DatabaseType,webAssign), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnObj = new DiaryAppsWebAssign();
                                while (dr.Read())
                                {
                                    returnObj = LoadWebAppsWebAssign(dr);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while getting details. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnObj;
        }
        internal DiaryAppsWebAssign IsWebAssignAppExists(DiaryAppsWebAssign webAssign, int webId)
        {
            DiaryAppsWebAssign returnObj = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsWebAssignQueries.SelectWebAssignAppsByCriteria(this.DatabaseType, webAssign, webId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnObj = new DiaryAppsWebAssign();
                                while (dr.Read())
                                {
                                    returnObj = LoadWebAppsWebAssign(dr);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while getting details. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnObj;
        }

        internal string GetThirdPartyTemplateURL(NaturalFormRequest naturalFormRequest, ProjectSettings settings)
        {
            string returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsWebAssignQueries.SelectQueryForTemplateURL(this.DatabaseType, naturalFormRequest), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                dr.Read();
                                switch (naturalFormRequest.Form.form_ref)
                                {
                                    case "1630957754": // Woodvale Maintenance Third Party Job Ticket
                                        returnValue = generateWoodvaleMaintJobTicketFormUrl(settings, naturalFormRequest.Form.form_ref,                                                                                            
                                                                                            dr["job_client_name"].ToString(),
                                                                                            dr["job_client_ref"].ToString(),
                                                                                            dr["job_ref"].ToString(),
                                                                                            dr["job_address_vert"].ToString(),
                                                                                            dr["resource_name"].ToString(),
                                                                                            dr["date_app_start"].ToString(),
                                                                                            dr["occupier_name"].ToString(),
                                                                                            dr["occupier_tel_work"].ToString(),
                                                                                            dr["occupier_tel_mobile"].ToString(),
                                                                                            dr["occupier_tel_home"].ToString(),
                                                                                            dr["occupier_email"].ToString(),
                                                                                            dr["job_sequence"].ToString(),
                                                                                            dr["job_address_id"].ToString(),
                                                                                            dr["da_sequence"].ToString(),
                                                                                            dr["job_client_id"].ToString(),
                                                                                            1,
                                                                                            dr["date_set_to_jt"].ToString(),
                                                                                            dr["property_upn"].ToString(),
                                                                                            dr["job_manager_name"].ToString(),
                                                                                            dr["job_desc"].ToString(),
                                                                                            dr["job_cost_centre"].ToString(),
                                                                                            dr["job_priority_code"].ToString(),
                                                                                            dr["job_date_due"].ToString(),
                                                                                            dr["job_trade_code"].ToString(),
                                                                                            dr["landlord_name"].ToString(),
                                                                                            dr["dawa_sequence"].ToString(),
                                                                                            dr["dawa_entity_id"].ToString(),
                                                                                            dr["web_id"].ToString());
                                        break;

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while getting details. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public static String generateWoodvaleMaintJobTicketFormUrl(ProjectSettings settings, string formId, object clientName,
                                                                   object jobClientRef, object jobRef, object jobAddress, object diaryResource,
                                                                   object appDate, object jobOccupierName, object jobOccupierTelWork,
                                                                   object jobOccupierTelMobile, object jobOccupierTelHome,
                                                                   object jobOccupierEmail, object jobSequence, object jobAddressId,
                                                                   object appSequence, object jobClientId, object userId, object jtDate,
                                                                   object edcUPN, object jobManager, object jobDesc, object jobCostCenter,
                                                                   object jobPriorityCode, object jobDateDue, object jobTradeCode,
                                                                   object jobClientParent, object dawaSequence, object dawaEntityId,
                                                                   object webId)
        {
            String url = "nfapi://x-callback-url/addDocument?documents=[";

            String successURL = settings.NaturalFormErrorURL;
            String errorURL = settings.NaturalFormSuccessURL;
            String formattedDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

            string formattedJobAddress = Utilities.replaceSpecialChars((String)jobAddress);
            string formattedJobAddressHorizontal = Utilities.replaceSpecialChars((String)jobAddress).Replace("\r\n", " ");
            String formName = formattedDate + " - " + (String)jobRef + " - " + formattedJobAddressHorizontal;
            formName = Utilities.replaceSpecialChars(formName);
            if (formName.Length > 45)
            {
                formName = formName.Substring(0, 45).Trim();
            }
            dynamic form = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)form;
            dictionary.Add("id", formId);
            dictionary.Add("name", formName);
            dynamic valuesObj = new ExpandoObject();
            var valuesDic = (IDictionary<string, object>)valuesObj;
            valuesDic.Add("VARIABLE_PG1_JOB_SEQUENCE", jobSequence.ToString());
            valuesDic.Add("VARIABLE_PG1_JOB_ADDRESS_ID", jobAddressId.ToString());
            valuesDic.Add("VARIABLE_PG1_DIARY_ENTRY_ID", appSequence.ToString());
            valuesDic.Add("VARIABLE_PG1_JOB_CLIENT_ID", jobClientId.ToString());
            valuesDic.Add("VARIABLE_PG1_USER_ID", userId.ToString());
            valuesDic.Add("VAR_PG1_WEB_ASS_SEQ", dawaSequence.ToString());
            valuesDic.Add("VAR_PG1_ENTITY_ID", dawaEntityId.ToString());
            valuesDic.Add("VAR_PG1_USER_LOG_ON", webId.ToString());

            valuesDic.Add("VARIABLE_PG1_DIARY_ENTRY_DATE", Utilities.formatDates(appDate, "dd/MM/yyyy"));
            valuesDic.Add("VARIABLE_PG1_JOB_REF", (String)jobRef);
            if (jobClientRef == null)
            {
                valuesDic.Add("VARIABLE_PG1_JOB_CLIENT_REF", "N/A");
            }
            else
            {
                valuesDic.Add("VARIABLE_PG1_JOB_CLIENT_REF", (jobClientRef == DBNull.Value) ? string.Empty : Utilities.replaceSpecialChars(jobClientRef.ToString()));
            }
            if (jobCostCenter == null)
            {
                valuesDic.Add("VAR_PG1_JOB_COST_CENTRE", "N/A");
            }
            else
            {
                valuesDic.Add("VAR_PG1_JOB_COST_CENTRE", (jobCostCenter == DBNull.Value) ? string.Empty : Utilities.replaceSpecialChars(jobCostCenter.ToString()));
            }

            if (jobPriorityCode == null)
            {
                valuesDic.Add("VAR_PG1_JOB_PRIORITY_CODE", "N/A");
            }
            else
            {
                valuesDic.Add("VAR_PG1_JOB_PRIORITY_CODE", (jobPriorityCode == DBNull.Value) ? string.Empty : Utilities.replaceSpecialChars(jobPriorityCode.ToString()));
            }
            valuesDic.Add("VAR_PG1_JOB_DATE_DUE", Utilities.formatDates(jobDateDue, "dd/MM/yyyy"));
            if (jobTradeCode == null)
            {
                valuesDic.Add("VAR_PG1_JOB_TRADE_CODE", "N/A");
            }
            else
            {
                valuesDic.Add("VAR_PG1_JOB_TRADE_CODE", (jobTradeCode == DBNull.Value) ? string.Empty : Utilities.replaceSpecialChars(jobTradeCode.ToString()));
            }

            if (jobClientParent == null)
            {
                valuesDic.Add("VAR_PG1_JOB_CLIENT_PARENT", "N/A");
            }
            else
            {
                valuesDic.Add("VAR_PG1_JOB_CLIENT_PARENT", (jobClientParent == DBNull.Value) ? string.Empty : Utilities.replaceSpecialChars(jobClientParent.ToString()));
            }
            if (clientName == null)
            {
                valuesDic.Add("VARIABLE_PG1_JOB_CLIENT_NAME", "N/A");
            }
            else
            {
                valuesDic.Add("VARIABLE_PG1_JOB_CLIENT_NAME", (clientName == DBNull.Value) ? string.Empty : Utilities.replaceSpecialChars(clientName.ToString()));
            }
            valuesDic.Add("VARIABLE_PG1_JOB_ADDRESS", (String)formattedJobAddressHorizontal);
            if (edcUPN == DBNull.Value)
            {
                valuesDic.Add("VAR_PG1_EDC_UPN", "");
            }
            else
            {
                valuesDic.Add("VAR_PG1_EDC_UPN", (String)edcUPN);
            }
            if (jobOccupierName == null)
            {
                valuesDic.Add("VARIABLE_PG1_JOB_OCCUPIER_NAME", "N/A");
            }
            else
            {
                valuesDic.Add("VARIABLE_PG1_JOB_OCCUPIER_NAME", (jobOccupierName == DBNull.Value) ? string.Empty : Utilities.replaceSpecialChars(jobOccupierName.ToString()));
            }
            if (jobOccupierTelHome == null)
            {
                valuesDic.Add("VARIABLE_PG1_JOB_OCCUPIER_TEL_HOME", "N/A");
            }
            else
            {
                valuesDic.Add("VARIABLE_PG1_JOB_OCCUPIER_TEL_HOME", (jobOccupierTelHome == DBNull.Value) ? string.Empty : Utilities.replaceSpecialChars(jobOccupierTelHome.ToString()));
            }
            if (jobDesc == null)
            {
                valuesDic.Add("VARIABLE_PG2_JOB_DESC", "N/A");
            }
            else
            {
                valuesDic.Add("VARIABLE_PG2_JOB_DESC", (jobDesc == DBNull.Value) ? string.Empty : Utilities.replaceSpecialChars(jobDesc.ToString()));
            }
            dictionary.Add("values", valuesDic);
            url += JsonConvert.SerializeObject(dictionary);
            url += "]&x-source=" + settings.DatabaseType + "&x-success=" + successURL + "&x-error=" + errorURL + "&values={}";
            url = System.Uri.EscapeUriString(url);
            return url;
        }

        public DiaryAppsWebAssign CreateWebAssignApp(DiaryAppsWebAssign Obj)
        {
            DiaryAppsWebAssign returnObj = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsWebAssignQueries.InsertWebAssign(this.DatabaseType, Obj), conn))
                    {

                        objCmdSelect.ExecuteNonQuery();
                        string LatestId = "select @@IDENTITY";
                        using (OleDbCommand NewQuery = new OleDbCommand(LatestId, conn))
                        {
                            OleDbDataReader dr = NewQuery.ExecuteReader();
                            if (dr.HasRows)
                            {
                                dr.Read();
                                returnObj = Obj;
                                returnObj.SequenceId = Int32.Parse(dr[0].ToString());
                            }
                            else
                            {
                                //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record.                                                 '" + METHOD_NAME + "'\n";
                            }
                        }
                        returnObj = Obj;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return returnObj;
        }
        public DiaryAppsWebAssign UpdateWebAssignApp(DiaryAppsWebAssign WebAssignObj)
        {
            DiaryAppsWebAssign returnObj = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsWebAssignQueries.UpdateWebAssign(this.DatabaseType, WebAssignObj), conn))
                    {

                        int result = objCmdSelect.ExecuteNonQuery();
                        if (result > 0)
                        {
                            returnObj = WebAssignObj;
                        }
                        else
                        {
                            returnObj = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnObj = null;
            }
            return returnObj;
        }

        public DiaryAppsWebAssign UpdateWebAssignByCriteria(DiaryAppsWebAssign WebAssignObj)
        {
            DiaryAppsWebAssign returnObj = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect = new OleDbCommand(DiaryAppsWebAssignQueries.UpdateWebAssignByCriteria(this.DatabaseType ,WebAssignObj), conn))
                    {
                        int result = objCmdSelect.ExecuteNonQuery();
                        if (result > 0)
                        {
                            returnObj = WebAssignObj;
                        }
                        else
                        {
                            returnObj = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                returnObj = null;
            }
            return returnObj;
        }

        internal List<DiaryAppsWebAssign> GetThirdPartyApp(DiaryAppsWebAssign WebAssignObj)
        {
            List<DiaryAppsWebAssign> returnObj = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(DiaryAppsWebAssignQueries.GetThirdPartyApp(this.DatabaseType, WebAssignObj), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                               returnObj = new List<DiaryAppsWebAssign>();
                               while (dr.Read())
                                {
                                    IsJobDetails = true;
                                    returnObj.Add(LoadWebAppsWebAssign(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error occured while getting details. " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnObj;
        }

        DiaryAppsWebAssign LoadWebAppsWebAssign(OleDbDataReader dr)
        {
            DiaryAppsWebAssign returnValue = null;
            try
            {
                if (dr != null)
                {
                    DiaryAppsWebAssign obj = new DiaryAppsWebAssign();
                    if (IsJobDetails)
                    {
                        obj.DiaryApp = new DiaryApps();
                        obj.DiaryApp.Order = new Orders();
                        obj.DiaryApp.Order.EntityJobAddress = new EntityDetailsCore();
                        obj.WebThirdParty = new WebThirdParties();
                        obj.Forms = new List<NaturalForm>();
                        obj.SequenceId = Int32.Parse(dr["sequence"].ToString());
                        obj.DeSequence = long.Parse(dr["de_sequence"].ToString());
                        obj.WebId = Int32.Parse(dr["web_id"].ToString());
                        obj.EntityId = long.Parse(dr["entity_id"].ToString());
                        obj.DateAppStart = Convert.ToDateTime(dr["date_app_start"].ToString());
                        obj.DiaryApp.ResourceSequence = long.Parse(dr["resource_sequence"].ToString());
                        obj.DiaryApp.JobSequence = long.Parse(dr["job_sequence"].ToString());
                        obj.DiaryApp.VisitStatus = long.Parse(dr["visit_status"].ToString());
                        obj.WebThirdParty.UserName = dr["user_name"].ToString();
                        obj.DiaryApp.Order.JobRef = dr["job_ref"].ToString();
                        obj.DiaryApp.Order.JobAddress = Utilities.GetDBString(dr["job_address"].ToString());
                        obj.DiaryApp.Order.JobClientName = Utilities.GetDBString(dr["job_client_name"].ToString());
                        obj.DiaryApp.Order.JobDesc = Utilities.GetDBString(dr["job_desc"].ToString());
                        obj.DiaryApp.Order.JobDateDue = Utilities.getDBDate(dr["job_date_due"]);
                        obj.DiaryApp.Order.JobClientRef = Utilities.GetDBString(dr["job_client_ref"]);
                        obj.DiaryApp.Order.EntityJobAddress.AddressPostCode = dr["address_post_code"].ToString();
                        
                        returnValue = obj;
                    }
                    else
                    {
                        obj.SequenceId = Int32.Parse(dr["sequence"].ToString());
                        obj.ResourceSequence = long.Parse(dr["resource_sequence"].ToString());
                        obj.DeSequence = long.Parse(dr["de_sequence"].ToString());
                        obj.EntityId = long.Parse(dr["entity_id"].ToString());
                        obj.WebId = Int32.Parse(dr["web_id"].ToString());
                        obj.AddInfo = dr["add_info"].ToString();
                        obj.DateAppStart = Convert.ToDateTime(dr["date_app_start"].ToString());
                        obj.DateAppCompleted = Convert.ToDateTime(dr["date_app_completed"].ToString());
                        obj.DateCreated = Convert.ToDateTime(dr["date_created"].ToString());
                        obj.DelayReason = dr["delay_reason"].ToString();
                        obj.FlgComplete = Boolean.Parse(dr["flg_complete"].ToString());
                        obj.FlgDelay = Boolean.Parse(dr["flg_delay"].ToString());
                        obj.LastAmendedBy = Int32.Parse(dr["last_amended_by"].ToString());
                        obj.DateLastAmended = Convert.ToDateTime(dr["date_last_amended"].ToString());
                        obj.CreatedBy = Int32.Parse(dr["created_by"].ToString());
                        returnValue = obj;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }

}

