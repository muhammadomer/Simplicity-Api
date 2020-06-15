using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;
using Newtonsoft.Json;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class Entity_Details_CoreQueries
    {
        public static string getSelectAllByEntityId(string databaseType, long? entityId)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT parent.name_short as parent_name,pymt_type.entity_pymt_desc,EDC.*
                 ,GDPR.entity_id as gdpr_entityId,GDPR.user_accepts, GDPR.date_user_accepts, GDPR.accepts_type, GDPR.contact_by_post, GDPR.contact_by_phone, GDPR.contact_by_sms,GDPR.contact_by_email
               FROM ((un_entity_details_core AS EDC 
                    LEFT JOIN un_entity_details_core AS parent ON EDC.entity_join_id = parent.entity_id) 
                    LEFT JOIN un_ref_entity_pymt_type AS pymt_type ON EDC.entity_pymt_type = pymt_type.entity_pymt_id) 
                    LEFT JOIN un_edc_gdpr AS GDPR ON EDC.entity_id = GDPR.entity_id
                WHERE EDC.entity_id = " + entityId;
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string getSelectAllByShortName(string databaseType, string shortName)
        {
            return @"SELECT * FROM un_entity_details_core 
                   WHERE name_short = '" + shortName + "'";
        }

        //---this will return last numeric value in shortname string use to increment
        public static string getLastShortNameValue(string databaseType, string shortName)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = @"SELECT  Max( val( Right(edc.name_short, len(edc.name_short)- len('" + shortName + @"'))))  as short_name
                                    FROM un_entity_details_core AS edc
                                    Where edc.name_short like '" + shortName + "%'";

                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getSelectAllByTransType(string databaseType, ClientRequest clientRequest, string transType)
        {
            string query = "", filterQuery = "";
            // Make Filter first
            if (clientRequest.globalFilter != "" && clientRequest.globalFilter != null)
            {
                string filterValue = clientRequest.globalFilter;
                string[] separators = new string[] { " " };
                string wordFilterQuery = "";
                //----find if word exist
                foreach (string word in filterValue.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                {
                    Console.WriteLine(word);
                    wordFilterQuery += (wordFilterQuery.Length > 0 ? "or" : "") + " edc.name_short like '%" + word + "%' or  edc.name_long like '%" + word + "%'";
                }
                wordFilterQuery += wordFilterQuery.Length > 0 ? " or " : "";
                filterQuery = " and ( " + wordFilterQuery + " edc.name_short like '%" + filterValue + "%' or edc.name_long like '%" + filterValue + "%' or edc.address_full like '%" + filterValue + "%' or edc.name_forename +' '+ edc.name_surname like '%" + filterValue + "%' or edc_parent.name_short like '%" + filterValue + "%' or edc.telephone like '%" + filterValue + "%' or edc.tel_mobile like '%" + filterValue + "%'  or edc.email like '%" + filterValue + "%') ";
                //query += filterQuery;
            }
            dynamic filter = JsonConvert.DeserializeObject<dynamic>(clientRequest.filters.ToString());
            if (filter.approvalStatus == "approved")
            {
                filterQuery += " and (edc.flg_entity_approved=" + Utilities.GetBooleanForDML(databaseType, true) + ")";
            }
            else if (filter.approvalStatus == "unapproved")
            {
                filterQuery += " and (edc.flg_entity_approved=" + Utilities.GetBooleanForDML(databaseType, false) + ")";
            }
            else if (filter.approvalStatus == "onHold")
            {
                filterQuery += " and ( edc.flg_entity_on_hold = " + Utilities.GetBooleanForDML(databaseType, true) + ")";
            }
           
            if (transType == "B")
            {
                query = @"SELECT edc.entity_id, edc.entity_join_id, edc.address_full, edj.trans_type, 
						edc.name_short, edc.name_long, edc.name_forename, edc.name_surname,
						edc_parent.entity_id as parent_entity_id
                        ,  iif(edc.entity_join_id= edc.entity_id or edc.entity_join_id<=0 , 'N/A' , edc_parent.name_short ) as parent_name_short,  
                        edc_parent.name_long as parent_name_long ,
                        edc.telephone, edc.tel_ext, edc.tel_mobile,edc.tel_work,edc.tel_fax, edc.email,
                        edc.flg_invoicing_client, edc.flg_entity_approved,edc.flg_entity_on_hold
                     FROM (un_entity_details_core AS edc 
                       INNER JOIN un_entity_details_join AS edj ON edc.entity_id = edj.entity_id) 
                       LEFT JOIN un_entity_details_core AS edc_parent ON edc.entity_join_id = edc_parent.entity_id 
                    WHERE edj.trans_type = '" + transType + "' and edc.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true)
                    + filterQuery;
            }
            else if (transType == "D" || transType == "C")
            {

                query = @"SELECT 'Supplier' AS trans_type_text,
					   edc.entity_id, edc.entity_join_id, edc.address_full, edj.trans_type,
					   edc.name_short, edc.name_long, edc.name_forename, edc.name_surname,
					   edc_parent.entity_id as parent_entity_id,
					   iif(edc.entity_join_id = edc.entity_id or edc.entity_join_id <= 0, 'N/A', edc_parent.name_short) as parent_name_short,  
					   edc_parent.name_long as parent_name_long ,
					   edc.telephone, edc.tel_ext, edc.tel_mobile,edc.tel_work,edc.tel_fax, edc.email,
					   edc.flg_invoicing_client, edc.flg_entity_approved,edc.flg_entity_on_hold
				FROM(un_entity_details_core AS edc
					INNER JOIN un_entity_details_join AS edj ON edc.entity_id = edj.entity_id)
					LEFT JOIN un_entity_details_core AS edc_parent ON edc.entity_join_id = edc_parent.entity_id
				WHERE edj.trans_type = 'D' AND edc.flg_deleted<>  " + Utilities.GetBooleanForDML(databaseType, true) + filterQuery;
                
    //            + @" UNION
				//SELECT 'Contractor' AS trans_type_text,
				//	edc.entity_id, edc.entity_join_id, edc.address_full, edj.trans_type,
				//	edc.name_short, edc.name_long, edc.name_forename, edc.name_surname,
				//	edc_parent.entity_id AS parent_entity_id,
				//	IIf(edc.entity_join_id = edc.entity_id Or edc.entity_join_id <= 0, 'N/A', edc_parent.name_short) AS parent_name_short,
				//	edc_parent.name_long AS parent_name_long,
				//	edc.telephone, edc.tel_ext, edc.tel_mobile, edc.tel_work, edc.tel_fax, edc.email,
				//	edc.flg_invoicing_client, edc.flg_entity_approved, edc.flg_entity_on_hold
				//FROM((un_entity_details_core AS edc
				//	INNER JOIN un_entity_details_join AS edj ON edc.entity_id = edj.entity_id)
				//	LEFT JOIN un_entity_details_core AS edc_parent ON edc.entity_join_id = edc_parent.entity_id)
				//	INNER JOIN un_entity_details_supplementary AS eds ON edc.entity_id = eds.entity_id
				//WHERE edj.trans_type = 'C'  AND eds.data_type = '021' AND eds.data <>  'True'
				//	 AND edc.flg_deleted<>  " + Utilities.GetBooleanForDML(databaseType, true) + filterQuery
    //            + @" UNION
				//SELECT 'Sub-Contractor' AS trans_type_text,
				//	edc.entity_id, edc.entity_join_id, edc.address_full, edj.trans_type,
				//	edc.name_short, edc.name_long, edc.name_forename, edc.name_surname,
				//	edc_parent.entity_id AS parent_entity_id,
				//	IIf(edc.entity_join_id = edc.entity_id Or edc.entity_join_id <= 0, 'N/A', edc_parent.name_short) AS parent_name_short,
				//	edc_parent.name_long AS parent_name_long,
				//	edc.telephone, edc.tel_ext, edc.tel_mobile, edc.tel_work, edc.tel_fax, edc.email,
				//	edc.flg_invoicing_client, edc.flg_entity_approved, edc.flg_entity_on_hold
				//FROM((un_entity_details_core AS edc
				//	INNER JOIN un_entity_details_join AS edj ON edc.entity_id = edj.entity_id)
				//	LEFT JOIN un_entity_details_core AS edc_parent ON edc.entity_join_id = edc_parent.entity_id)
				//	INNER JOIN un_entity_details_supplementary AS eds ON edc.entity_id = eds.entity_id
				//WHERE edj.trans_type = 'C' AND eds.data_type = '021'
				//	AND eds.data = 'True' AND edc.flg_deleted<>  " + Utilities.GetBooleanForDML(databaseType, true) + filterQuery;
            }
            //----Sorting
            string sortColumn = "edc.name_short"; // default sort
            if (clientRequest.sortField != null)
            {
                switch (clientRequest.sortField)
                {
                    case "NameShort":
                        sortColumn = "edc.name_short";
                        break;
                    case "NameLong":
                        sortColumn = "edc.name_long";
                        break;
                    case "ParentName":
                        sortColumn = " iif(edc.entity_join_id= edc.entity_id or edc.entity_join_id<=0 , 'N/A' , edc_parent.name_short )";
                        break;
                    case "AddressFull":
                        sortColumn = "edc.address_full";
                        break;
                    case "Type":
                        sortColumn = "edc.flg_invoicing_client";
                        break;
                    case "Email":
                        sortColumn = "edc.email";
                        break;
                    case "TransTypeDesc":
                        sortColumn = "trans_type_text";
                        break;
                }
                string orderType = "ASC";
                if (clientRequest.sortOrder == -1)
                {
                    orderType = "DESC";
                }
                else
                {
                    orderType = "ASC";
                }
                query += " ORDER BY " + sortColumn + " " + orderType;
            }
            else
            {
                query += " ORDER BY " + sortColumn + " ASC";
            }
            return query;
        }

        public static string getSelectAllSuppliers(string databaseType, ClientRequest clientRequest)
        {
            string query = "", filterQuery = "", filterSupplier="";
            //--- Make Filter first
            if (clientRequest.globalFilter != "" && clientRequest.globalFilter != null)
            {
                string filterValue = clientRequest.globalFilter;
                string[] separators = new string[] { " " };
                string wordFilterQuery = "";
                //----find if word exist
                foreach (string word in filterValue.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                {
                    wordFilterQuery += (wordFilterQuery.Length > 0 ? "or" : "") + " edc.name_short like '%" + word + "%' or  edc.name_long like '%" + word + "%'";
                }
                wordFilterQuery += wordFilterQuery.Length > 0 ? " or " : "";
                filterQuery = " and ( " + wordFilterQuery + " edc.name_short like '%" + filterValue + "%' or edc.name_long like '%" + filterValue + "%' or edc.address_full like '%" + filterValue + "%' or edc.name_forename +' '+ edc.name_surname like '%" + filterValue + "%' or edc_parent.name_short like '%" + filterValue + "%' or edc.telephone like '%" + filterValue + "%' or edc.tel_mobile like '%" + filterValue + "%'  or edc.email like '%" + filterValue + "%') ";
               
            }
            dynamic filter = JsonConvert.DeserializeObject<dynamic>(clientRequest.filters.ToString());
            if (filter.approvalStatus == "approved")
            {
                filterQuery += " and (edc.flg_entity_approved=" + Utilities.GetBooleanForDML(databaseType, true) + ")";
            }
            else if (filter.approvalStatus == "unapproved")
            {
                filterQuery += " and (edc.flg_entity_approved=" + Utilities.GetBooleanForDML(databaseType, false) + ")";
            }
            else if (filter.approvalStatus == "onHold")
            {
                filterQuery += " and ( edc.flg_entity_on_hold = " + Utilities.GetBooleanForDML(databaseType, true) + ")";
            }
            if (filter.supplierType == "contractor") filterSupplier = " AND eds.data <>  'True'";
            if (filter.supplierType == "subContractor") filterSupplier = " AND eds.data =  'True'";

            if (filter.supplierType == "supplier" || filter.supplierType == "all")
            {
                query = @"SELECT 'Supplier' AS trans_type_text,
					edc.entity_id, edc.entity_join_id, edc.address_full, edj.trans_type,
					edc.name_short, edc.name_long, edc.name_forename, edc.name_surname,
					edc_parent.entity_id as parent_entity_id,
					iif(edc.entity_join_id = edc.entity_id or edc.entity_join_id <= 0, 'N/A', edc_parent.name_short) as parent_name_short,  
					edc_parent.name_long as parent_name_long ,
					edc.telephone, edc.tel_ext, edc.tel_mobile,edc.tel_work,edc.tel_fax, edc.email,
					edc.flg_invoicing_client, edc.flg_entity_approved,edc.flg_entity_on_hold
				FROM(un_entity_details_core AS edc
					INNER JOIN un_entity_details_join AS edj ON edc.entity_id = edj.entity_id)
					LEFT JOIN un_entity_details_core AS edc_parent ON edc.entity_join_id = edc_parent.entity_id
				WHERE edj.trans_type = 'D' AND edc.flg_deleted<>  " + Utilities.GetBooleanForDML(databaseType, true) + filterQuery;
            } else if (filter.supplierType != "supplier") {
                query = (query.Length > 0) ? " UNION " : "";
                query += @" SELECT IIF( eds.data = 'True' , 'Sub-Contractor' , 'Contractor') AS trans_type_text,
					edc.entity_id, edc.entity_join_id, edc.address_full, edj.trans_type,
					edc.name_short, edc.name_long, edc.name_forename, edc.name_surname,
					edc_parent.entity_id AS parent_entity_id,
					IIf(edc.entity_join_id = edc.entity_id Or edc.entity_join_id <= 0, 'N/A', edc_parent.name_short) AS parent_name_short,
					edc_parent.name_long AS parent_name_long,
					edc.telephone, edc.tel_ext, edc.tel_mobile, edc.tel_work, edc.tel_fax, edc.email,
					edc.flg_invoicing_client, edc.flg_entity_approved, edc.flg_entity_on_hold
				FROM((un_entity_details_core AS edc
					INNER JOIN un_entity_details_join AS edj ON edc.entity_id = edj.entity_id)
					LEFT JOIN un_entity_details_core AS edc_parent ON edc.entity_join_id = edc_parent.entity_id)
					INNER JOIN un_entity_details_supplementary AS eds ON edc.entity_id = eds.entity_id
				WHERE edj.trans_type = 'C'  AND eds.data_type = '021' 
					 AND edc.flg_deleted<>  " + Utilities.GetBooleanForDML(databaseType, true) + filterSupplier + filterQuery;
            }
            //----Sorting
            string sortColumn = "edc.name_short"; // default sort
            if (clientRequest.sortField != null)
            {
                switch (clientRequest.sortField)
                {
                    case "NameShort":
                        sortColumn = "edc.name_short";
                        break;
                    case "NameLong":
                        sortColumn = "edc.name_long";
                        break;
                    case "ParentName":
                        sortColumn = " iif(edc.entity_join_id= edc.entity_id or edc.entity_join_id<=0 , 'N/A' , edc_parent.name_short )";
                        break;
                    case "AddressFull":
                        sortColumn = "edc.address_full";
                        break;
                    case "Type":
                        sortColumn = "edc.flg_invoicing_client";
                        break;
                    case "Email":
                        sortColumn = "edc.email";
                        break;
                    case "TransTypeDesc":
                        sortColumn = "trans_type_text";
                        break;
                }
                string orderType = "ASC";
                if (clientRequest.sortOrder == -1)
                {
                    orderType = "DESC";
                }
                else
                {
                    orderType = "ASC";
                }
                query += " ORDER BY " + sortColumn + " " + orderType;
            }
            else
            {
                query += " ORDER BY " + sortColumn + " ASC";
            }
            return query;
        }

        public static string getSelectAllByTransType(string databaseType, string transType, string qSearch)
        {
            string query = @"SELECT edc_client.entity_id, edc_client.entity_join_id, edc_client.address_full
                            , edc_client.name_short, edc_client.name_long, edj.trans_type
                            , edc_parent.entity_id as parent_entity_id, edc_parent.name_short as parent_name_short, edc_parent.name_long as parent_name_long
                           , edc_client.telephone,edc_client.tel_ext,edc_client.tel_mobile,edc_client.tel_work,edc_client.tel_fax,edc_client.email, edc_client.flg_entity_on_hold, edc_client.flg_entity_approved
                     FROM (un_entity_details_core AS edc_client 
                    INNER JOIN un_entity_details_join AS edj 
                       ON edc_client.entity_id = edj.entity_id) 
                     LEFT JOIN un_entity_details_core AS edc_parent 
                       ON edc_client.entity_join_id = edc_parent.entity_id 
                    WHERE edj.trans_type = '" + transType + "' and edc_client.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
            if (!string.IsNullOrEmpty(qSearch))
            {
                string filterValue = qSearch;
                string[] separators = new string[] { " " };
                string wordFilterQuery = "";
                //----find if word exist
                foreach (string word in filterValue.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                {
                    Console.WriteLine(word);
                    wordFilterQuery += (wordFilterQuery.Length > 0 ? "or" : "") + " edc_client.name_short like '%" + word + "%' or  edc_client.name_long like '%" + word + "%'";
                }
                wordFilterQuery += wordFilterQuery.Length > 0 ? " or " : "";
                string globalFilterQuery = " and ( " + wordFilterQuery + " edc_client.name_short like '%" + filterValue + "%' or edc_client.name_long like '%" + filterValue + "%' or edc_client.address_full like '%" + filterValue + "%' or edc_client.name_forename +' '+ edc_client.name_surname like '%" + filterValue + "%' or edc_parent.name_short like '%" + filterValue + "%') ";
                query += globalFilterQuery;
            }
            return query;

        }

        public static string getAllEmailAddresses(string databaseType, long? jobSequence)
        {
            string returnValue = "", whereOrdStr="";
            try
            {
                if (jobSequence > 0)
                    whereOrdStr = " And ord.sequence=" + jobSequence;
                returnValue = @"SELECT 'B' AS trans_type, edc_b.entity_id, edc_b.name_short, edc_b.name_long, edc_b.email, edc_b.address_full
                     FROM un_orders AS ord 
                     INNER JOIN un_entity_details_core AS edc_b  ON ord.job_client_id = edc_b.entity_id
                     WHERE edc_b.email > '' and edc_b.email <> 'Not Set' AND edc_b.email <> 'null' and edc_b.email <> 'N/A'" + whereOrdStr;


                returnValue += @" UNION 
                     SELECT 'F' AS trans_type, edc_f.entity_id, edc_f.name_short, edc_f.name_long, edc_f.email, edc_f.address_full
                     FROM un_orders AS ord 
                     INNER JOIN un_entity_details_core AS edc_f  ON ord.job_manager = edc_f.entity_id
                     WHERE edc_f.email > '' and edc_f.email <> 'Not Set' AND edc_f.email <> 'null' and edc_f.email <> 'N/A'"+ whereOrdStr;

                returnValue += @" UNION 
                     SELECT 'F' AS trans_type, edc_b.entity_id, edc_ma.address_name AS name_short, redc_mat.address_type_desc AS name_long, edc_ma.email, edc_ma.address_full
                     FROM ((un_orders AS ord
                     INNER JOIN un_entity_details_core AS edc_b ON ord.job_client_id = edc_b.entity_id)
                     INNER JOIN un_edc_multi_addresses AS edc_ma ON edc_b.entity_id = edc_ma.entity_id)
                     INNER JOIN un_ref_edc_multi_add_type AS redc_mat ON edc_ma.address_type = redc_mat.address_type_sequence
                     WHERE edc_ma.email > '' and edc_ma.email <> 'Not Set' AND edc_ma.email <> 'null' and edc_ma.email <> 'N/A' AND redc_mat.trans_type = 'B'" + whereOrdStr;

                returnValue += @" UNION  
                    SELECT ii.trans_type, edc.entity_id, edc.name_short, edc.name_long, edc.email, edc.address_full
                    FROM (un_invoice_itemised AS ii
                     INNER JOIN un_invoice_itemised_items AS ord ON ii.sequence = ord.invoice_sequence)
                     INNER JOIN un_entity_details_core AS edc ON ii.contact_id = edc.entity_id
                     WHERE edc.email > '' and edc.email <> 'Not Set' AND edc.email <> 'null' and email <> 'N/A' AND (ii.trans_type = 'D' OR ii.trans_type = 'C')" + whereOrdStr 
                     +@" GROUP BY ii.trans_type, edc.entity_id, edc.name_short, edc.name_long, edc.email, edc.address_full, ord.job_sequence";
                    
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string insert(string databaseType,long entityId, bool flgDeleted, bool flgEntityOnHold, bool flgContactManager, long clientType, bool flgInvoicingClient, bool flgEntityJoin,
                                    long entityJoinId, long entityApprovedStatus, long entityPymtType, bool flgEformsPreferred, string nameShort, string nameLong, long sageId,
                                    bool flgSageTurnOn, string nameSage, string nameTitle, string nameInitilas, string nameForename, string nameSurname, string addressNo,
                                    string addressLine1, string addressLine2, string addressLine3, string addressLine4, string addressLine5, string addressPostCode, string addressFull,
                                    string telephone, string telExt, string telFax, string telMobile, string telWork, string email, string PropertyEpn, string PropertyUpn, string entityDetails,
                                    bool flgSupAddressHeld, bool flgClientCheck, long userListId, long userListId2, long userListId3, object userNumericField1, bool flgUserField1,
                                    bool flgUserField2, bool flgUserField3, bool flgUserField4, string userTextField1, string userTextField2, string userTextField3, string userTextField4,
                                    bool flgUserDateField1, DateTime? dateUserDateField1, bool flgUserDateField2, DateTime? dateUserDateField2, bool flgUserDateField3, DateTime? dateUserDateField3,
                                    bool flgUserDateField4, DateTime? dateUserDateField4,bool flgEntityApproved, long createdBy, DateTime? dateCreated, long LastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "INSERT INTO un_entity_details_core(entity_ID,flg_deleted, flg_entity_on_hold, flg_contact_manager, client_type," +
                            "flg_invoicing_client, flg_entity_join, entity_join_id, entity_approved_status," +
                            "entity_pymt_type, flg_eforms_preferred, name_short, name_long, sage_id," +
                            "flg_sage_turn_on, name_sage, name_title, name_initilas, name_forename," +
                            "name_surname, address_no, address_line1, address_line2, address_line3," +
                            "address_line4, address_line5, address_post_code, address_full, telephone," +
                            "tel_ext,tel_fax, tel_mobile, tel_work, email, property_epn, property_upn," +
                            "entity_details, flg_sup_address_held, flg_client_check, user_list_id, user_list_id2," +
                            "user_list_id3, user_numeric_field1, flg_user_field1, flg_user_field2, flg_user_field3," +
                            "flg_user_field4, user_text_field1, user_text_field2, user_text_field3, user_text_field4," +
                            "flg_user_date_field1, date_user_date_field1, flg_user_date_field2, date_user_date_field2," +
                            "flg_user_date_field3, date_user_date_field3, flg_user_date_field4, date_user_date_field4,flg_entity_approved," +
                            "created_by, date_created, last_amended_by,date_last_amended)" +
                        "VALUES (" + entityId + ", " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", " + Utilities.GetBooleanForDML(databaseType, flgEntityOnHold) 
                            + ", " + Utilities.GetBooleanForDML(databaseType, flgContactManager) + ", " + clientType + ", " +
                            Utilities.GetBooleanForDML(databaseType, flgInvoicingClient) + ", " + (entityJoinId > 0 ? 1 : 0) + ", " + entityJoinId + ", " + entityApprovedStatus + ", " + entityPymtType + ", " +
                            Utilities.GetBooleanForDML(databaseType, flgEformsPreferred) + ", '" + nameShort + "', '" + nameLong + "', " + sageId + ", " + Utilities.GetBooleanForDML(databaseType, flgSageTurnOn) + ", '" + nameSage + "', '" +
                            Utilities.replaceSpecialChars(nameTitle) + "', '" + Utilities.replaceSpecialChars(nameInitilas) + "',' " + Utilities.replaceSpecialChars(nameForename) + "'"
                            + ", '" + Utilities.replaceSpecialChars(nameSurname) + "', '" + Utilities.replaceSpecialChars(addressNo)  + "', '" + Utilities.replaceSpecialChars(addressLine1) + "', '" +
                            Utilities.replaceSpecialChars(addressLine2) + "',' " + Utilities.replaceSpecialChars(addressLine3) + "',' " + Utilities.replaceSpecialChars(addressLine4) + "',' " + Utilities.replaceSpecialChars(addressLine5) + "'"
                            + ", '" + Utilities.replaceSpecialChars(addressPostCode) + "',' " + Utilities.replaceSpecialChars(addressFull) + "',' " +
                            Utilities.replaceSpecialChars(telephone) + "', '" + Utilities.replaceSpecialChars(telExt) + "',' " + Utilities.replaceSpecialChars(telFax)  + "',' " + Utilities.replaceSpecialChars(telMobile)  + "', '" + Utilities.replaceSpecialChars(telWork)  + "', '" + email + "', '" + Utilities.replaceSpecialChars(PropertyEpn)  + "',' " +
                            Utilities.replaceSpecialChars(PropertyUpn) + "',' " + Utilities.replaceSpecialChars(entityDetails) + "', " + Utilities.GetBooleanForDML(databaseType, flgSupAddressHeld) + ", " + Utilities.GetBooleanForDML(databaseType, flgClientCheck) + ", " + userListId + ", " + userListId2 + ", " +
                            userListId3 + ", " + userNumericField1 + ", " + Utilities.GetBooleanForDML(databaseType, flgUserField1) + ", " + Utilities.GetBooleanForDML(databaseType, flgUserField2) + ", " + Utilities.GetBooleanForDML(databaseType, flgUserField3) + ", " + Utilities.GetBooleanForDML(databaseType, flgUserField4) + ", '" +
                            userTextField1 + "', '" + userTextField2 + "', '" + userTextField3 + "', '" + userTextField4 + "', " + Utilities.GetBooleanForDML(databaseType, flgUserDateField1) + ", " +
                            Utilities.GetDateTimeForDML(databaseType, dateUserDateField1,true,true) + ", " + Utilities.GetBooleanForDML(databaseType, flgUserDateField2) + ", " + Utilities.GetDateTimeForDML(databaseType, dateUserDateField2,true,true) + ", " +
                            Utilities.GetBooleanForDML(databaseType, flgUserDateField3) + ", " + Utilities.GetDateTimeForDML(databaseType, dateUserDateField3,true,true) + ", " + Utilities.GetBooleanForDML(databaseType, flgUserDateField4) + ", " +
                            Utilities.GetDateTimeForDML(databaseType ,dateUserDateField4,true,true) + "," + Utilities.GetBooleanForDML(databaseType, flgEntityApproved) + "," +
                            createdBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " + LastAmendedBy + ", " +
                            Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ")";
                                            
                
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateEntityDetailsCoreInfo(string databaseType, EntityDetailsCore edc, string infoType)
        {
            string returnValue = "";
            string fieldName = "";
            object fieldValue = null;
            switch (infoType)
            {
                case SimplicityConstants.DB_FIELD_EDC_EMAIL: fieldName = "email"; fieldValue = "'" + edc.Email + "'"; break;
                default:
                    throw new ArgumentException("Invalid field");
                    break;
            }

            if (String.IsNullOrWhiteSpace(fieldName) == false)
            {
                returnValue = String.Format("UPDATE un_entity_details_core SET {0} = {1}", fieldName, fieldValue);
            }
            if(!string.IsNullOrEmpty(returnValue))
            {
                if(edc.LastAmendedBy>0)
                {
                    returnValue = returnValue + ", last_amended_by = " + edc.LastAmendedBy;
                }
                if (edc.DateLastAmended != null && edc.DateLastAmended != DateTime.MinValue)
                {
                    returnValue = returnValue + ", date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, edc.DateLastAmended, true, true);
                }
                returnValue = returnValue + " WHERE entity_id = " + edc.EntityId;
            }
            return returnValue;
        }

        public static string update(string databaseType, long entityId, bool flgDeleted, bool flgEntityOnHold, bool flgContactManager, long clientType, bool flgInvoicingClient, bool flgEntityJoin,
                                   long entityJoinId, long entityApprovedStatus, long entityPymtType, bool flgEformsPreferred, string nameLong, long sageId,
                                   bool flgSageTurnOn, string nameSage, string nameTitle, string nameInitilas, string nameForename, string nameSurname, string addressNo,
                                   string addressLine1, string addressLine2, string addressLine3, string addressLine4, string addressLine5, string addressPostCode, string addressFull,
                                   string telephone, string telExt, string telFax, string telMobile, string telWork, string email, string PropertyEpn, string PropertyUpn, string entityDetails,
                                   bool flgSupAddressHeld, bool flgClientCheck, long userListId, long userListId2, long userListId3, object userNumericField1, bool flgUserField1,
                                   bool flgUserField2, bool flgUserField3, bool flgUserField4, string userTextField1, string userTextField2, string userTextField3, string userTextField4,
                                   bool flgUserDateField1, DateTime? dateUserDateField1, bool flgUserDateField2, DateTime? dateUserDateField2, bool flgUserDateField3, DateTime? dateUserDateField3,
                                   bool flgUserDateField4, DateTime? dateUserDateField4, bool flgEntityApproved,  long LastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            returnValue = "UPDATE un_entity_details_core " +
               "   SET   flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", " +
               "         flg_entity_on_hold =  " + Utilities.GetBooleanForDML(databaseType, flgEntityOnHold) + ", " +
               "         flg_contact_manager = " + Utilities.GetBooleanForDML(databaseType, flgContactManager) + ", " +
               "         client_type =  " + clientType + ", " +
               "         flg_invoicing_client = " + Utilities.GetBooleanForDML(databaseType, flgInvoicingClient) + ", " +
               "         flg_entity_join =  " + Utilities.GetBooleanForDML(databaseType, flgEntityJoin) + ", " +
               "         entity_join_id =  " + entityJoinId + ", " +
               "         entity_approved_status = " + entityApprovedStatus + ", " +
               "         entity_pymt_type = " + entityPymtType + ", " +
               "         flg_eforms_preferred =  " + Utilities.GetBooleanForDML(databaseType, flgEformsPreferred) + ", " +
               "         name_long =  '" + nameLong + "', " +
               "         sage_id = " + sageId + ", " +
               "         flg_sage_turn_on =  " + Utilities.GetBooleanForDML(databaseType, flgSageTurnOn) + ", " +
               "         name_sage = '" + nameSage + "', " +
               "         name_title = '" + nameTitle + "'," +
               "         name_initilas = '" + nameInitilas + "'," +
               "         name_forename = ' " + nameForename + "'," +
               "         name_surname = '" + nameSurname + "'," +
               "         address_no = '" + addressNo + "'," +
               "         address_line1 = '" + addressLine1 + "'," +
               "         address_line2 = '" + addressLine2 + "'," +
               "         address_line3 = ' " + addressLine3 + "'," +
               "         address_line4 = ' " + addressLine4 + "'," +
               "         address_line5 = ' " + addressLine5 + "', " +
               "         address_post_code = '" + addressPostCode + "'," +
               "         address_full = ' " + addressFull + "'," +
               "         telephone =  '" + telephone + "', " +
               "         tel_ext = '" + telExt + "'," +
               "         tel_fax = ' " + telFax + "'," +
               "         tel_mobile = ' " + telMobile + "'," +
               "         tel_work = '" + telWork + "', " +
               "         email = '" + email + "'," +
               "         property_epn = '" + PropertyEpn + "'," +
               "         property_upn = '" + PropertyUpn + "'," +
               "         entity_details = ' " + entityDetails + "', " +
               "         flg_sup_address_held = " + Utilities.GetBooleanForDML(databaseType, flgSupAddressHeld) + ", " +
               "         flg_client_check = " + Utilities.GetBooleanForDML(databaseType, flgClientCheck) + ", " +
               "         user_list_id = " + userListId + ", " +
               "         user_list_id2 = " + userListId2 + ", " +
               "         user_list_id3 = " + userListId3 + ", " +
               "         user_numeric_field1 = " + userNumericField1 + ", " +
               "         flg_user_field1 = " + Utilities.GetBooleanForDML(databaseType, flgUserField1) + ", " +
               "         flg_user_field2 = " + Utilities.GetBooleanForDML(databaseType, flgUserField2) + ", " +
               "         flg_user_field3 = " + Utilities.GetBooleanForDML(databaseType, flgUserField3) + ", " +
               "         flg_user_field4 = " + Utilities.GetBooleanForDML(databaseType, flgUserField4) + ", " +
               "         user_text_field1 = '" + userTextField1 + "'," +
               "         user_text_field2 = '" + userTextField2 + "'," +
               "         user_text_field3 = '" + userTextField3 + "'," +
               "         user_text_field4 = '" + userTextField4 + "'," +
               "         flg_user_date_field1 = " + Utilities.GetBooleanForDML(databaseType, flgUserDateField1) + ", " +
               "         date_user_date_field1 =   " + Utilities.GetDateTimeForDML(databaseType, dateUserDateField1, true, true) + ", " +
               "         flg_user_date_field2 = " + Utilities.GetBooleanForDML(databaseType, flgUserDateField2) + ", " +
               "         date_user_date_field2 = " + Utilities.GetDateTimeForDML(databaseType, dateUserDateField2, true, true) + ", " +
               "         flg_user_date_field3 = " + Utilities.GetBooleanForDML(databaseType, flgUserDateField3) + ", " +
               "         date_user_date_field3 = " + Utilities.GetDateTimeForDML(databaseType, dateUserDateField3, true, true) + ", " +
               "         flg_user_date_field4 = " + Utilities.GetBooleanForDML(databaseType, flgUserDateField4) + ", " +
               "         date_user_date_field4 = " + Utilities.GetDateTimeForDML(databaseType, dateUserDateField4, true, true) + ", " +
               "         flg_entity_approved = " + Utilities.GetBooleanForDML(databaseType, flgEntityApproved) + ", " +
               "         last_amended_by = " + LastAmendedBy + ", " +
               "         date_last_amended=  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) +
               " WHERE entity_id = " + entityId;
            return returnValue;
        }

        public static string updateEntityJoinId(string databaseType, long entityId, long entityJoinId)
        {
            string returnValue = "";
            returnValue = @"UPDATE un_entity_details_core 
               SET   entity_join_id =  " + entityJoinId 
               + " WHERE entity_id = " + entityId;
            return returnValue;
        }

        public static string updateAddress(string databaseType, long entityId, bool flgDeleted,  bool flgEntityJoin,
                                   long entityJoinId,  string addressNo,
                                   string addressLine1, string addressLine2, string addressLine3, string addressLine4, string addressLine5, string addressPostCode, string addressFull,
                                   string telephone, string telExt, string telFax, string telMobile, string telWork, string email, string PropertyEpn, string PropertyUpn, string entityDetails,
                                   long LastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            returnValue = "UPDATE un_entity_details_core " +
               "   SET   flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", " +
               "         flg_entity_join =  " + Utilities.GetBooleanForDML(databaseType, flgEntityJoin) + ", " +
               "         entity_join_id =  " + entityJoinId + ", " +
               "         address_no = '" + Utilities.replaceSpecialChars(addressNo) + "'," +
               "         address_line1 = '" + Utilities.replaceSpecialChars(addressLine1) + "'," +
               "         address_line2 = '" + Utilities.replaceSpecialChars(addressLine2) + "'," +
               "         address_line3 = ' " + Utilities.replaceSpecialChars(addressLine3) + "'," +
               "         address_line4 = ' " + Utilities.replaceSpecialChars(addressLine4) + "'," +
               "         address_line5 = ' " + Utilities.replaceSpecialChars(addressLine5) + "', " +
               "         address_post_code = '" +  Utilities.replaceSpecialChars(addressPostCode) + "'," +
               "         address_full = ' " + Utilities.replaceSpecialChars(addressFull) + "'," +
               "         telephone =  '" +  Utilities.replaceSpecialChars(telephone) + "', " +
               "         tel_ext = '" +  Utilities.replaceSpecialChars(telExt)  + "'," +
               "         tel_fax = ' " +  Utilities.replaceSpecialChars(telFax) + "'," +
               "         tel_mobile = ' " +  Utilities.replaceSpecialChars(telMobile) + "'," +
               "         tel_work = '" +  Utilities.replaceSpecialChars(telWork) + "', " +
               "         email = '" +  Utilities.replaceSpecialChars(email) + "'," +
               "         property_epn = '" + Utilities.replaceSpecialChars(PropertyEpn)  + "'," +
               "         property_upn = '" + Utilities.replaceSpecialChars(PropertyUpn)  + "'," +
               "         entity_details = ' " + Utilities.replaceSpecialChars(entityDetails) + "', " +
               "         last_amended_by = " + LastAmendedBy + ", " +
               "         date_last_amended=  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) +
               " WHERE entity_id = " + entityId;
            return returnValue;
        }

        public static string delete(string databaseType, long entityId)
        {
           string returnValue = "";
           try
           {
                returnValue = "DELETE FROM un_entity_details_core" +
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
                returnValue = "UPDATE un_entity_details_core" +
                        "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                        " WHERE entity_id = " + entityId;
                       
           }
           catch (Exception ex)
           {
           }
           return returnValue;
       }       

       public static string getfullAddress(string databaseType,string address)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "SELECT *" +
                        "  FROM un_entity_details_core" +
                        "  where address_full like '%" + address + "%' ";

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string searchEntitiesNamesByTransType(string databaseType, string name, string transType)
        {
            string returnValue = "";
            try
            {
               
                returnValue = @"SELECT edj.trans_type, edc.* 
                FROM un_entity_details_core AS edc 
                INNER JOIN un_entity_details_join AS edj ON edc.entity_id = edj.entity_id 
                WHERE edj.trans_type = '" + transType + "' " +
                "   AND edc.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) +
                "   AND (edc.name_short like  '%" + name + "%' " +
                "    OR edc.name_long like  '%" + name + "%')";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string GetEntitiesByTransTypeAndAddress(string databaseType, string transType, string address)
        {
            return @"SELECT edj.trans_type, edc.* 
                FROM un_entity_details_core AS edc 
                INNER JOIN un_entity_details_join AS edj ON edc.entity_id = edj.entity_id 
                WHERE edj.trans_type = '" + transType + "'" 
              + "  AND edc.address_full LIKE '%" + address + "%'";
        }

        internal static string GetAddressByTransType(string databaseType, string transType, ClientRequest requestModel)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                        returnValue = @"SELECT edj.trans_type, edc.entity_id, edc.flg_deleted, edc.flg_entity_join, edc.entity_join_id,client.name_long as client, edc_detail.ContactName
                        , edc.address_no, edc.address_line1,edc.address_line2, edc.address_line3, edc.address_line4, edc.address_line5,edc.address_post_code, edc.address_full
                        , edc.telephone, edc.tel_ext, edc.tel_fax, edc.tel_mobile, edc.tel_work, edc.email, edc.property_epn, edc.property_upn, edc.entity_details
                        ,edc_detail.PropertyType, edc_detail.PropertyStatus
                        FROM ((un_entity_details_core AS edc 
                            INNER JOIN un_entity_details_join AS edj ON edc.entity_id = edj.entity_id) 
                            LEFT JOIN un_entity_details_core AS client on edc.entity_join_id=client.entity_id)
                            LEFT JOIN (SELECT entity_Id,
                                Max(Switch(data_Type = '022', data)) as PropertyType,
                                max(Switch(data_Type = '036', data)) as PropertyStatus,
                                max(Switch(data_Type = '011', data)) as ContactName
                                From un_entity_details_supplementary
                                    Where  data_Type in ('022', '036', '038','011')
                                    group by entity_id
                            )  AS edc_detail ON edc.entity_Id = edc_detail.entity_Id
                        WHERE edj.trans_type = '" + transType + "' and edc.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
                        if (requestModel.globalFilter != "" && requestModel.globalFilter != null)
                        {
                            string filterValue = requestModel.globalFilter;
                            string globalFilterQuery = " And ( IIF( Len(edc.address_full)>0,Replace(edc.address_full,Chr(13) & Chr(10),' '),'')  LIKE '%" + filterValue + "%' or edc.address_full  LIKE '%" + filterValue + "%')";
                            returnValue += globalFilterQuery;

                        }
                        break;

                    case "SQLSERVER":
                        returnValue = @"SELECT edj.trans_type, edc.entity_id, edc.flg_deleted, edc.flg_entity_join, edc.entity_join_id,client.name_long as client, edc_detail.ContactName
                        , edc.address_no, edc.address_line1,edc.address_line2, edc.address_line3, edc.address_line4, edc.address_line5,edc.address_post_code, edc.address_full
                        , edc.telephone, edc.tel_ext, edc.tel_fax, edc.tel_mobile, edc.tel_work, edc.email, edc.property_epn, edc.property_upn, edc.entity_details
                        ,edc_detail.PropertyType, edc_detail.PropertyStatus
                        FROM ((un_entity_details_core AS edc 
                            INNER JOIN un_entity_details_join AS edj ON edc.entity_id = edj.entity_id) 
                            LEFT JOIN un_entity_details_core AS client on edc.entity_join_id=client.entity_id)
                            LEFT JOIN (SELECT entity_Id,
                                Max(case data_Type when '022' then data end) as propertyType,
                                Max(case data_Type when '036' then data end) as PropertyStatus,
                                Max(case data_Type when '011'then data end) as ContactName
                                From un_entity_details_supplementary
                                    Where  data_Type in ('022', '036', '038','011')
                                    group by entity_id
                            )  AS edc_detail ON edc.entity_Id = edc_detail.entity_Id
                        WHERE edj.trans_type = '" + transType + "' and edc.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
                        if (requestModel.globalFilter != "" && requestModel.globalFilter != null)
                        {
                            string filterValue = requestModel.globalFilter;
                            string globalFilterQuery = " And (Replace(edc.address_full,Char(13) + Char(10),' ')  LIKE '%" + filterValue + "%' or edc.address_full  LIKE '%" + filterValue + "%')";
                            returnValue += globalFilterQuery;

                        }
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string GetCountOfAddressByTransType(string databaseType, string transType, ClientRequest requestModel)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                        returnValue = @"SELECT Count(edc.entity_id) as recordCount
                        FROM ((un_entity_details_core AS edc 
                            INNER JOIN un_entity_details_join AS edj ON edc.entity_id = edj.entity_id) 
                            LEFT JOIN un_entity_details_core AS client on edc.entity_join_id=client.entity_id)
                            LEFT JOIN (SELECT entity_Id,
                                Max(Switch(data_Type = '022', data)) as PropertyType,
                                max(Switch(data_Type = '036', data)) as PropertyStatus,
                                max(Switch(data_Type = '011', data)) as ContactName
                                From un_entity_details_supplementary
                                    Where  data_Type in ('022', '036', '038','011')
                                    group by entity_id
                            )  AS edc_detail ON edc.entity_Id = edc_detail.entity_Id
                        WHERE edj.trans_type = '" + transType + "' and edc.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
                        if (requestModel.globalFilter != "" && requestModel.globalFilter != null)
                        {
                            string filterValue = requestModel.globalFilter;
                            string globalFilterQuery = " And ( IIF( Len(edc.address_full)>0,Replace(edc.address_full,Chr(13) & Chr(10),' '),'')  LIKE '%" + filterValue + "%' or edc.address_full  LIKE '%" + filterValue + "%')";
                            returnValue += globalFilterQuery;

                        }
                        break;

                    case "SQLSERVER":
                        returnValue = @"SELECT Count(edc.entity_id) as recordCount
                        FROM ((un_entity_details_core AS edc 
                            INNER JOIN un_entity_details_join AS edj ON edc.entity_id = edj.entity_id) 
                            LEFT JOIN un_entity_details_core AS client on edc.entity_join_id=client.entity_id)
                            LEFT JOIN (SELECT entity_Id,
                                Max(case data_Type when '022' then data end) as propertyType,
                                Max(case data_Type when '036' then data end) as PropertyStatus,
                                Max(case data_Type when '011'then data end) as ContactName
                                From un_entity_details_supplementary
                                    Where  data_Type in ('022', '036', '038','011')
                                    group by entity_id
                            )  AS edc_detail ON edc.entity_Id = edc_detail.entity_Id
                        WHERE edj.trans_type = '" + transType + "' and edc.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
                        if (requestModel.globalFilter != "" && requestModel.globalFilter != null)
                        {
                            string filterValue = requestModel.globalFilter;
                            string globalFilterQuery = " And (Replace(edc.address_full,Char(13) + Char(10),' ')  LIKE '%" + filterValue + "%' or edc.address_full  LIKE '%" + filterValue + "%')";
                            returnValue += globalFilterQuery;

                        }
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string GetEntitiesByTransTypeAndAddressAndEntityJoinId(string databaseType, string transType, string address, long entityJoinId)
        {
            return @"SELECT edj.trans_type, edc.* 
            FROM un_entity_details_core AS edc 
            INNER JOIN un_entity_details_join AS edj  ON edc.entity_id = edj.entity_id 
            WHERE edj.trans_type = '" + transType + "'" +
            "   AND edc.address_full LIKE '%" + address + "%'" +
            "   AND edc.entity_join_id = " + entityJoinId +
            " And edc.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true);
        }

       
        internal static string UpdateClientInfo(string datebaseType, EntityDetailsCore edc, string infoType)
        {
            string returnValue = "";
            string fieldName = "";
            object fieldValue = null;
            bool addrressUpdate = true;
            string fullAddress = "";
            switch (infoType)
            {
                case "name_short": fieldName = "name_short"; fieldValue = "'" + edc.NameShort + "'"; break;
                case "name_long": fieldName = "name_long"; fieldValue = "'" + edc.NameLong + "'"; break;
                case "address_no": fieldName = "address_no"; fieldValue = "'" + edc.AddressNo + "'"; break;
                case "address_line1": fieldName = "address_line1"; fieldValue = "'" + edc.AddressLine1 + "'"; break;
                case "address_line2": fieldName = "address_line2"; fieldValue = "'" + edc.AddressLine2 + "'"; break;
                case "address_line3": fieldName = "address_line3"; fieldValue = "'" + edc.AddressLine3 + "'"; break;
                case "address_line4": fieldName = "address_line4"; fieldValue = "'" + edc.AddressLine4 + "'"; break;
                case "address_line5": fieldName = "address_line5"; fieldValue = "'" + edc.AddressLine5 + "'"; break;
                case "address_post_code": fieldName = "address_post_code"; fieldValue = "'" + edc.AddressPostCode + "'"; break;
                case "telephone": fieldName = "telephone"; fieldValue = "'" + edc.Telephone + "'"; break;
                case "tel_ext": fieldName = "tel_ext"; fieldValue = "'" + edc.TelExt + "'"; break;
                case "tel_mobile": fieldName = "tel_mobile"; fieldValue = "'" + edc.TelMobile + "'"; break;
                case "tel_work": fieldName = "tel_work"; fieldValue = "'" + edc.TelWork + "'"; break;
                case "email": fieldName = "email"; fieldValue = "'" + edc.Email + "'"; break;
                case "property_epn": fieldName = "property_epn"; fieldValue = "'" + edc.PropertyEpn + "'"; break;
                case "property_upn": fieldName = "property_upn"; fieldValue = "'" + edc.PropertyUpn + "'"; break;
                case "entity_details": fieldName = "entity_details"; fieldValue = "'" + edc.EntityDetails + "'"; break;
                case "entity_pymt_type": fieldName = "entity_pymt_type"; fieldValue = "'" + edc.EntityPymtType + "'"; break;
                case "name_title": fieldName = "name_title"; fieldValue = "'" + edc.NameTitle + "'"; break;
                case "name_initilas": fieldName = "name_initilas"; fieldValue = "'" + edc.NameInitilas + "'"; break;
                case "name_forename": fieldName = "name_forename"; fieldValue = "'" + edc.NameForename + "'"; break;
                case "name_surname": fieldName = "name_surname"; fieldValue = "'" + edc.NameSurname + "'"; break;
                case "entity_join_id":
                    {
                        if (edc.EntityJoinId > 0)
                        {
                            fieldName = "entity_join_id"; fieldValue = edc.EntityJoinId ;
                        }
                        else
                            throw new ArgumentException("Invalid Parent Id."); break;
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid field");break;
            }
            

            if (String.IsNullOrWhiteSpace(fieldName) == false)
            {
                returnValue = String.Format("UPDATE un_entity_details_core SET {0} = {1} WHERE entity_id = {2}", fieldName, fieldValue, edc.EntityId);
            }

            return returnValue;
        }

    }
}

