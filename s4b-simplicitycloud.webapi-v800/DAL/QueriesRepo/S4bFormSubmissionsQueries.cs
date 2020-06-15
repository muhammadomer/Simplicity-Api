using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class S4bFormSubmissionsQueries
    {
        public static string GetFormSubmissionsList(string dataBase)
        {
            string returnVal = "";
            try
            {
                switch(dataBase)
                {
                    case "MSACCESS":
                        //returnVal = "SELECT sub.*,rnf.form_id " +
                        //     "  FROM un_s4b_submissions sub" +
                        //     "  LEFT JOIN un_ref_s4b_forms rnf " +
                        //     "  ON sub.form_sequence = rnf.form_sequence ";
                        returnVal = @"SELECT sub.*, orders.job_ref, orders.job_address as job_Address, Left([orders].[job_address],10)  AS job_address_short
                        , IIf([sub]![flg_3rd_party]=1,[3rdParty]![user_name],[un_user_details]![user_name]) AS submitter
                        FROM (
                            (un_s4b_submissions AS sub LEFT JOIN un_orders AS orders ON sub.job_sequence = orders.sequence) 
                            LEFT JOIN un_user_details ON sub.created_by = un_user_details.user_id) 
                            LEFT JOIN un_web_3rd_parties AS 3rdParty ON sub.id_3rd_party = [3rdParty].web_id ";
                        break;
                    case "SQLSERVER":
                        returnVal = @"SELECT sub.*, orders.job_ref, orders.job_address, Left([orders].[job_address],10) AS job_address_short
                        , case when sub.flg_3rd_party=1 then ThirdParty.user_name else un_user_details.user_name end AS submitter
                        FROM 
                            un_s4b_submissions AS sub LEFT JOIN un_orders AS orders ON sub.job_sequence = orders.sequence
                            LEFT JOIN un_user_details ON sub.created_by = un_user_details.user_id
                            LEFT JOIN un_web_3rd_parties AS ThirdParty ON sub.id_3rd_party = ThirdParty.web_id";
                        break;
                    default:
                        returnVal = @"SELECT sub.*, orders.job_ref, orders.job_address as job_Address, Left([orders].[job_address],10)  AS job_address_short
                        , IIf([sub]![flg_3rd_party]=1,[3rdParty]![user_name],[un_user_details]![user_name]) AS submitter
                        FROM (
                            (un_s4b_submissions AS sub LEFT JOIN un_orders AS orders ON sub.job_sequence = orders.sequence) 
                            LEFT JOIN un_user_details ON sub.created_by = un_user_details.user_id) 
                            LEFT JOIN un_web_3rd_parties AS 3rdParty ON sub.id_3rd_party = [3rdParty].web_id ";
                        break;
                }
            }catch(Exception ex)
            {

            }
            return returnVal;
        }
        public static string GetFormSubmissionsList(string dataBase, SimplicityOnlineBLL.Entities.ClientRequest clientRequest)
        {
            string returnVal = @"SELECT sub.*, orders.job_ref, orders.job_address AS job_Address, Left([orders].[job_address],10) AS job_address_short
                    , IIf(sub.flg_3rd_party=1,rdParty.user_name,un_user_details.user_name) AS submitter, un_ref_s4b_forms.form_id
                    , orders.order_type, orders.job_client_name, orders.job_client_ref, orders.job_cost_centre, rjst.status_desc, orders.sequence as jobSequence
                    FROM ((((un_s4b_submissions AS sub LEFT JOIN un_orders AS orders ON sub.job_sequence = orders.sequence) 
                        LEFT JOIN un_user_details ON sub.created_by = un_user_details.user_id) 
                        LEFT JOIN un_web_3rd_parties AS rdParty ON sub.id_3rd_party = rdParty.web_id) 
                        LEFT JOIN un_ref_s4b_forms ON sub.form_sequence = un_ref_s4b_forms.form_sequence) 
                        LEFT JOIN un_ref_job_status_type AS rjst ON orders.job_status = rjst.status_id";

            if (clientRequest.globalFilter != null && clientRequest.globalFilter != "")
            {
                string filterValue = clientRequest.globalFilter;
                string globalFilterQuery = " where sub.s4b_submit_no like '%" + filterValue + "%'"
                    + "  or un_user_details.user_name like '%" + filterValue + "%'"
                    + " or orders.job_ref like '%" + filterValue + "%'"
                    + " or orders.job_address like '%" + filterValue + "%'"
                    + " or sub.template_name like '%" + filterValue + "%' "
                    + " or orders.order_type like '%" + filterValue + "%' "
                    + " or orders.job_client_name like '%" + filterValue + "%' "
                    + " or orders.job_client_ref like '%" + filterValue + "%' "
                    + " or orders.job_cost_centre like '%" + filterValue + "%' "
                    + " or rjst.status_desc like '%" + filterValue + "%' ";
                returnVal += globalFilterQuery;
            }
            string sortColumn = "sub.sequence"; // default sort
            if (clientRequest.sortField != null){
                switch (clientRequest.sortField)
                {
                    case "S4bSubmitNo":
                        sortColumn = "sub.s4b_submit_no";
                        break;
                    case "DateSubmit":
                        sortColumn = "sub.date_submit";
                        break;
                    case "Submitter":
                        sortColumn = "un_user_details.user_name";
                        break;
                    case "Orders.JobRef":
                        sortColumn = "orders.job_ref";
                        break;
                    case "JobAddressShort":
                        sortColumn = "orders.job_address";
                        break;
                    case "TemplateName":
                        sortColumn = "sub.template_name";
                        break;
                    case "Orders.OrderTypeDesc.OrderTypeDescShort":
                        sortColumn = "orders.order_type";
                        break;
                    case "Orders.JobClientName":
                        sortColumn = "orders.job_client_name";
                        break;
                }
                string orderType = "DESC";
                if (clientRequest.sortOrder == -1)
                {
                    orderType = "DESC";
                }else{
                    orderType = "ASC";
                }
                returnVal += " ORDER BY " + sortColumn + " " + orderType;
            }else{
                returnVal += " ORDER BY " + sortColumn + " DESC";
            }
                        
            return returnVal;
        }

        public static string GetFormSubmissionsListByJobSequence(string dataBase, long jobSequence)
        {
            string returnVal = @"SELECT sub.*, orders.job_ref, orders.job_address AS job_Address, Left([orders].[job_address],10) AS job_address_short
                    , IIf([sub].[flg_3rd_party]=1,ThirdParty.[user_name],[un_user_details].[user_name]) AS submitter, un_ref_s4b_forms.form_id
                    , orders.order_type, orders.job_client_name, orders.job_client_ref, orders.job_cost_centre, rjst.status_desc
                    FROM ((((un_s4b_submissions AS sub LEFT JOIN un_orders AS orders ON sub.job_sequence = orders.sequence) 
                        LEFT JOIN un_user_details ON sub.created_by = un_user_details.user_id) 
                        LEFT JOIN un_web_3rd_parties AS ThirdParty ON sub.id_3rd_party = ThirdParty.web_id) 
                        LEFT JOIN un_ref_s4b_forms ON sub.form_sequence = un_ref_s4b_forms.form_sequence) 
                        LEFT JOIN un_ref_job_status_type AS rjst ON orders.job_status = rjst.status_id
                        Where orders.sequence = " + jobSequence
                     + " ORDER BY sub.date_submit DESC ";

            return returnVal;
        }

        public static string GetFormSubmissionsByS4BSubmitNo(string dataBase, string s4bSubmitNo)
        {
            string returnVal = "";
            try
            {
                returnVal = @"SELECT un_s4b_submissions.*,un_ref_s4b_forms.form_id
                        FROM un_s4b_submissions INNER JOIN un_ref_s4b_forms ON un_s4b_submissions.form_sequence = un_ref_s4b_forms.form_sequence
                        WHERE un_s4b_submissions.s4b_submit_no = '" + s4bSubmitNo + "'";
            }
            catch (Exception ex)
            {
            }
            return returnVal;
        }

        public static string GetFormSubmissionsBySequence(string dataBase, string sequence)
        {
            string returnVal = "";
            try
            {
                returnVal = @"SELECT un_s4b_submissions.*, un_ref_s4b_forms.form_id, un_orders.job_ref
                        FROM (un_s4b_submissions INNER JOIN un_ref_s4b_forms ON un_s4b_submissions.form_sequence = un_ref_s4b_forms.form_sequence) 
                            LEFT JOIN un_orders ON un_s4b_submissions.job_sequence = un_orders.sequence
                        WHERE un_s4b_submissions.sequence=" + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnVal;
        }

        public static string GetFormSubmissionsInfoBySequence(string dataBase, string sequence)
        {
            string returnVal = "";
            try
            {
                    returnVal = @"SELECT un_s4b_submissions.*
                        ,un_orders.job_ref,un_entity_details_core.email, un_ref_s4b_forms.form_id, un_ref_s4b_forms.form_desc, un_ref_s4b_forms.client_id
                        FROM ((un_s4b_submissions LEFT JOIN un_orders ON un_s4b_submissions.job_sequence = un_orders.sequence) 
                            LEFT JOIN un_entity_details_core ON un_orders.job_client_id = un_entity_details_core.entity_id) 
                            INNER JOIN un_ref_s4b_forms ON un_s4b_submissions.form_sequence = un_ref_s4b_forms.form_sequence
                        WHERE un_s4b_submissions.sequence=" + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnVal;
        }
        public static string InsertFormSubmissions(string databaseType, S4bFormSubmissions obj)
        {
            string returnVal = "";
            try
            {
                returnVal = "INSERT INTO un_s4b_submissions ( s4b_submit_no, s4b_submit_ts, date_submit, " +
                                    "       form_sequence, file_cab_id, template_name, submit_details, job_sequence, " +
                                    "       flg_3rd_party,id_3rd_party, created_by,date_created,last_amended_by,date_last_amended) " +
                                    "VALUES('" + obj.S4bSubmitNo + "', '" + obj.S4bSubmitTs + "', " +
                                    "     " + Utilities.GetDateTimeForDML(databaseType, obj.DateSubmit,true,true) + ", " + obj.FormSequence + ", " +
                                    "    '" + obj.FileCabId + "', '" + obj.TemplateName + "', '" + obj.SubmitDetails + "', " +
                                    "     " + obj.JobSequence + ", " +
                                    "     " + Utilities.GetBooleanForDML(databaseType, obj.Flg3rdParty) + ", " + obj.Id3rdParty + ", " +
                                    "     " + obj.CreatedBy + "," + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated,true,true) + ", " +
                                    "     " + obj.LastAmendedBy + "," + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended,true,true) + ") ";

            }
            catch (Exception ex)
            {
            }
            return returnVal;
        }


        public static string UpdateFormSubmissions(string databaseType, S4bFormSubmissions obj)
        {
            string returnVal = "";
            try
            {
                returnVal = "UPDATE un_s4b_submissions " +
                    "   set s4b_submit_no = '" + obj.S4bSubmitNo + "', " +
                    "       s4b_submit_ts = '" + obj.S4bSubmitTs + "', " +
                    "       date_submit = " + Utilities.GetDateTimeForDML(databaseType, obj.DateSubmit,true,true) + ", " +
                    "       form_sequence = " + obj.FormSequence + ", " +
                    "       file_cab_id = '" + obj.FileCabId + "', " +
                    "       job_sequence = " + obj.JobSequence + ", " +
                    "       template_name = '" + obj.TemplateName + "', " +
                    "       submit_details = '" + obj.SubmitDetails + "', " +
                    "       flg_3rd_party =  " + Utilities.GetBooleanForDML(databaseType, obj.Flg3rdParty) + ", " +
                    "       id_3rd_party =  " + obj.Id3rdParty + ", " +
                    "       created_by =  " + obj.CreatedBy + ", " +
                    "       date_created =  " + Utilities.GetDateTimeForDML(databaseType, obj.DateCreated,true,true) + ", " +
                    "       last_amended_by = " + obj.LastAmendedBy + ", " +
                    "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, obj.DateLastAmended,true,true)  +
                    "       WHERE sequence = " + obj.Sequence;
            }
            catch (Exception ex)
            {
                  Utilities.WriteLog("Error in query="+ ex.Message);
            }
            Utilities.WriteLog(returnVal);
            return returnVal;
        }

        internal static string UpdateFileCabIdAndPdfCountForSubmissionsData(string databaseType, S4bFormSubmissions submissionData)
        {
            string returnValue = "";

           
            returnValue = "UPDATE un_s4b_submissions" +
                            "   SET file_cab_id  = '" + submissionData.FileCabId + "', " +
                            "       created_pdf_count = " + submissionData.CreatedPDFCount + ", " +
                            "       last_amended_by = " + submissionData.LastAmendedBy + ", " +
                            "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, (submissionData.DateLastAmended.HasValue ? submissionData.DateLastAmended.Value : DateTime.Now),true,true) +
                            " WHERE sequence = " + submissionData.Sequence;
                    
            return returnValue;
        }

        internal static string UpdateFlgCompletedForSubmissionsData(string databaseType, S4bFormSubmissions submissionData)
        {
            string returnValue = "";


            returnValue = "UPDATE un_s4b_submissions" +
                            "   SET flg_completed  = " + Utilities.GetBooleanForDML(databaseType, true) + ", " +
                            "       last_amended_by = " + submissionData.LastAmendedBy + ", " +
                            "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, (submissionData.DateLastAmended.HasValue ? submissionData.DateLastAmended.Value : DateTime.Now), true, true) +
                            " WHERE sequence = " + submissionData.Sequence;

            return returnValue;
        }
        internal static string SelectSubmissionCountByTemplateName(string databaseType, DateTime fromDate, DateTime toDate)
        {
            return @"SELECT template_name as TemplateName, Count(un_s4b_submissions.sequence) AS SubmissionCount
            FROM un_s4b_submissions
            Where  date_submit between " + Utilities.GetDateValueForDML(databaseType, fromDate) + " and " + Utilities.GetDateValueForDML(databaseType, toDate)
            + @" GROUP BY template_name
            ORDER BY template_name";
        }
    }
}
