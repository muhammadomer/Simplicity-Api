using SimplicityOnlineWebApi.Commons;
using System;
using Microsoft.VisualBasic;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrdersQueries
    {
        public static string SelectAllFieldsByJobRef(string datebaseType, string jobRef)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT * 
                        FROM un_orders 
                        WHERE job_ref = '" + jobRef.Trim() + "'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string GetAllOrdersMinByJobRef(string datebaseType, string jobRef)
        {
            string returnValue = "";
            try
            {

                returnValue = @"SELECT sequence, job_ref 
                              FROM un_orders";
                if (jobRef.ToLower() != "all") returnValue += " WHERE job_ref like '%" + jobRef.Trim() + "%'";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string GetAllOrdersMinByJobClientRef(string datebaseType, string jobClientRef)
        {
            return @"SELECT sequence, job_ref 
                              FROM un_orders 
                      WHERE job_client_ref = '" + jobClientRef.Trim() + "'";
        }

        public static string GetAllOrdersByJobRef(string datebaseType, string jobRef)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "SELECT * " +
                              "  FROM un_orders " +
                              " WHERE job_ref like '%" + jobRef.Trim() + "%'";
                        break;
                    case "SQLSERVER":
                        returnValue = "SELECT * " +
                              "  FROM un_orders " +
                              " WHERE job_ref like '%" + jobRef.Trim() + "%'";
                        break;
                    default:
                        returnValue = "SELECT * " +
                              "  FROM un_orders " +
                              " WHERE job_ref like '%" + jobRef.Trim() + "%'";
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string GetAllOrdersByClientRef(string datebaseType, string jobClientRef)
        {
            string returnValue = "SELECT * " +
                              "  FROM un_orders " +
                              " WHERE job_client_ref = '" + jobClientRef.Trim() + "'";
            return returnValue;
        }

        public static string SearchOrders(string datebaseType, string key, string field, string match)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = DBUtil.OrderSearchQueryBuilder(key, field, match);
                        break;
                    case "SQLSERVER":
                        returnValue = DBUtil.OrderSearchQueryBuilder(key, field, match);
                        break;
                    default:
                        returnValue = DBUtil.OrderSearchQueryBuilder(key, field, match);
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string SearchOrders(string datebaseType)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = DBUtil.OrderSearchQueryBuilder(null, null, null);
                        break;
                    case "SQLSERVER":
                        returnValue = DBUtil.OrderSearchQueryBuilder(null, null, null);
                        break;
                    default:
                        returnValue = DBUtil.OrderSearchQueryBuilder(null, null, null);
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string GetOrdersByJobRefOrAddressOrClientName(string datebaseType, string jobRef, string jobAddress, string jobClientName)
        {
            string returnValue = "";
            string searchCriteria = string.Empty;
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "SELECT * FROM un_orders WHERE 1=1 ";
                        if (!string.IsNullOrEmpty(jobRef))
                            searchCriteria = searchCriteria + " AND job_ref like '%" + jobRef.Trim() + "%' ";
                        if (!string.IsNullOrEmpty(jobAddress))
                            searchCriteria = searchCriteria + " AND job_address like '%" + jobAddress.Trim() + "%' ";
                        if (!string.IsNullOrEmpty(jobClientName))
                            searchCriteria = searchCriteria + " AND job_client_name like '%" + jobClientName.Trim() + "%' ";
                        returnValue = returnValue + searchCriteria;
                        break;
                    case "SQLSERVER":
                    default:
                        returnValue = "SELECT * FROM un_orders WHERE 1=1 ";
                        if (!string.IsNullOrEmpty(jobRef))
                            searchCriteria = searchCriteria + " AND job_ref like '%" + jobRef.Trim() + "%' ";
                        if (!string.IsNullOrEmpty(jobAddress))
                            searchCriteria = searchCriteria + " AND job_address like '%" + jobAddress.Trim() + "%' ";
                        if (!string.IsNullOrEmpty(jobClientName))
                            searchCriteria = searchCriteria + " AND job_client_name like '%" + jobClientName.Trim() + "%' ";
                        returnValue = returnValue + searchCriteria;
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        //public static string GetOrdersMinByJobRefOrAddressOrClientName(string databaseType, string jobRef, string jobAddress, string jobClientName,string jobClientRef, string ebsJobRef)
        //{
        //    string returnValue = "";
        //    string searchCriteria = string.Empty;
        //    string searchWildCard = "%";// DBUtil.GetSearchWildChar(databaseType);
        //    returnValue = "SELECT sequence, job_ref, job_address, job_client_name FROM un_orders WHERE flg_cancelled <> " + Utilities.GetBooleanForDML(databaseType, true);
        //    if (!string.IsNullOrEmpty(jobRef))
        //    {
        //        searchCriteria = searchCriteria + " AND job_ref like '" + searchWildCard + jobRef.Trim() + searchWildCard + "'";
        //    }
        //    if (!string.IsNullOrEmpty(jobAddress))
        //    {
        //        searchCriteria = searchCriteria + " AND job_address like '" + searchWildCard + jobAddress.Trim() + searchWildCard + "'";
        //    }
        //    if (!string.IsNullOrEmpty(jobClientName))
        //    {
        //        searchCriteria = searchCriteria + " AND job_client_name like '" + searchWildCard + jobClientName.Trim() + searchWildCard + "'";
        //    }
        //    if (!string.IsNullOrEmpty(jobClientRef))
        //    {
        //        searchCriteria = searchCriteria + " AND job_client_ref like '" + searchWildCard + jobClientRef.Trim() + searchWildCard + "'";
        //    }
        //    if (!string.IsNullOrEmpty(ebsJobRef))
        //    {
        //        searchCriteria = searchCriteria + " AND ebs_job_ref like '" + searchWildCard + ebsJobRef.Trim() + searchWildCard + "'";
        //    }
        //    returnValue = returnValue + searchCriteria;
        //    return returnValue;
        //}

        public static string GetOrdersMinByJobRefOrAddressOrClientName(string databaseType, ClientRequest clientRequest)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT ord.sequence, job_ref, job_address, job_client_name,OB.invoice_no
					FROM un_orders ord left outer join un_orders_bills AS OB on OB.job_sequence=ord.sequence
					WHERE flg_cancelled <> " + Utilities.GetBooleanForDML(databaseType, true);

                if (clientRequest.globalFilter != null && clientRequest.globalFilter != "")
                {
                    string filterValue = clientRequest.globalFilter;
                    string globalFilterQuery = " And ord.job_ref like '%" + filterValue + "%' or ord.job_client_name like '%"
                        + filterValue + "%' or ord.job_address like '%" + filterValue + "%' or ord.job_client_ref like '%"
                        + filterValue + "%' or ord.ebs_job_ref like '%" + filterValue + "%' "
						+ " or OB.invoice_no like '%" + filterValue + "%' ";
                    returnValue += globalFilterQuery;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string GetClientAddressByJobSequence(string datebaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT edc_p.name_long, edc_p.address_full
                FROM (un_orders AS ord
                    INNER JOIN un_entity_details_core AS edc_c ON ord.job_client_id = edc_c.entity_id)
                    INNER JOIN un_entity_details_core AS edc_p ON edc_c.entity_join_id = edc_p.entity_id
                WHERE ord.sequence =" + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string GetOrdersMinByJobAddress(string databaseType, long jobAddressId)
        {
            string returnValue = "";
            try
            {
                returnValue = returnValue = @"SELECT ord.sequence, ord.job_ref, ord.job_client_name, ord.job_date, un_entity_details_core.name_long
                FROM un_orders AS ord 
                    INNER JOIN un_entity_details_core ON ord.job_address_id = un_entity_details_core.entity_id
                WHERE flg_cancelled <> " + Utilities.GetBooleanForDML(databaseType, true) + " and flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true)
                + " And ord.job_address_id=" + jobAddressId
                + " order by ord.job_date";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, string jobRef, int createdBy, DateTime? dateCreated)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_orders (job_ref, flg_job_ref_is_numeric, job_status, job_client_id, job_address_id, occupier_name, 
                                    job_desc, flg_no_access_ref_set, job_priority_code, flg_job_received_by_kier, flg_job_sla_report, 
                                    flg_job_sla_report_show, flg_job_sla_timer_stop, flg_job_date_start, flg_job_date_finish, flg_user1, 
                                    flg_user2, flg_set_to_jt, flg_to_client, flg_monthly_check, flg_to_est, flg_from_est, flg_cancelled, 
                                    flg_order_has_vos, flg_bill_proforma, flg_inv_has_allocation, flg_filter, flg_order_items_locked, 
                                    flg_kier_app_booked, flg_kier_app_attended, job_kier_contact_id, flg_kpi_contacted, kpi_contacted_by_id, 
                                    flg_maintenance_order, flg_job_completed, flg_active_complaint, flg_archive, created_by, date_created) 
                             VALUES ('" + jobRef + "'," + (Information.IsNumeric(jobRef) ? 1 : 0) + ", 0, -1, -1, ' N/A', ' N/A', 0,  '', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 0, 0, 0,"
                             + createdBy + "," + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ")";
                //switch (databaseType)
                //{
                //    case "MSACCESS":
                //        returnValue = "INSERT INTO un_orders (job_ref, flg_job_ref_is_numeric, job_status, job_client_id, job_address_id, occupier_name, " +
                //             "       job_desc, flg_no_access_ref_set, job_priority_code, flg_job_received_by_kier, flg_job_sla_report, " +
                //             "       flg_job_sla_report_show, flg_job_sla_timer_stop, flg_job_date_start, flg_job_date_finish, flg_user1, " +
                //             "       flg_user2, flg_set_to_jt, flg_to_client, flg_monthly_check, flg_to_est, flg_from_est, flg_cancelled, " +
                //             "       flg_order_has_vos, flg_bill_proforma, flg_inv_has_allocation, flg_filter, flg_order_items_locked, " +
                //             "       flg_kier_app_booked, flg_kier_app_attended, job_kier_contact_id, flg_kpi_contacted, kpi_contacted_by_id, " +
                //             "       flg_maintenance_order, flg_job_completed, flg_active_complaint, flg_archive, created_by, date_created) " +
                //             "VALUES ('" + jobRef + "'," + (Information.IsNumeric(jobRef) ? 1 : 0) + ", 0, -1, -1, ' ', ' ', 0,  '', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 0, 0, 0," + createdBy + ",'" +
                //                      Utilities.GetDateTimeForDML(databaseType,dateCreated,true,true) + ")";
                //        break;
                //    case "SQLSERVER":
                //    default:
                //        returnValue = "INSERT INTO un_orders (job_ref, flg_job_ref_is_numeric, job_status, job_client_id, job_address_id, occupier_name, " +
                //             "       job_desc, flg_no_access_ref_set, job_priority_code, flg_job_received_by_kier, flg_job_sla_report, " +
                //             "       flg_job_sla_report_show, flg_job_sla_timer_stop, flg_job_date_start, flg_job_date_finish, flg_user1, " +
                //             "       flg_user2, flg_set_to_jt, flg_to_client, flg_monthly_check, flg_to_est, flg_from_est, flg_cancelled, " +
                //             "       flg_order_has_vos, flg_bill_proforma, flg_inv_has_allocation, flg_filter, flg_order_items_locked, " +
                //             "       flg_kier_app_booked, flg_kier_app_attended, job_kier_contact_id, flg_kpi_contacted, kpi_contacted_by_id, " +
                //             "       flg_maintenance_order, flg_job_completed, flg_active_complaint, flg_archive, created_by, date_created) " +
                //             "VALUES ('" + jobRef + "'," + (Information.IsNumeric(jobRef) ? 1 : 0) + ", -1, -1, -1, ' ', '', 0,  '', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 0, 0, 0," + createdBy + ",'" +
                //                     ((DateTime)dateCreated).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                //        break;                    
                //}

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, Orders order, int createdBy, DateTime? dateCreated)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                    //returnValue = "INSERT INTO un_orders (job_ref, flg_job_ref_is_numeric, job_status, job_client_id, job_address_id, occupier_name, " +
                    //     "       job_desc, flg_no_access_ref_set, job_priority_code, flg_job_received_by_kier, flg_job_sla_report, " +
                    //     "       flg_job_sla_report_show, flg_job_sla_timer_stop, flg_job_date_start, flg_job_date_finish, flg_user1, " +
                    //     "       flg_user2, flg_set_to_jt, flg_to_client, flg_monthly_check, flg_to_est, flg_from_est, flg_cancelled, " +
                    //     "       flg_order_has_vos, flg_bill_proforma, flg_inv_has_allocation, flg_filter, flg_order_items_locked, " +
                    //     "       flg_kier_app_booked, flg_kier_app_attended, job_kier_contact_id, flg_kpi_contacted, kpi_contacted_by_id, " +
                    //     "       flg_maintenance_order, flg_job_completed, flg_active_complaint, flg_archive, created_by, date_created) " +
                    //     "VALUES ('" + jobRef + "'," + Information.IsNumeric(jobRef) + ", 0, -1, -1, ' ', ' ', 0,  '', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 0, 0, 0," + createdBy + ",'" +
                    //             ((DateTime)dateCreated).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    //break;
                    case "SQLSERVER":
                    default:
                        returnValue = "INSERT INTO un_orders (job_ref, flg_job_ref_is_numeric, job_status, job_client_id,job_client_name, job_address_id, job_address,occupier_name, " +
                             "       job_desc, flg_no_access_ref_set, job_priority_code, job_trade_code,job_cost_centre,job_manager,job_client_ref,order_type," +
                             "       flg_set_to_jt, date_set_to_jt,flg_user2,date_user2 ,job_date," +
                             "       flg_job_received_by_kier, flg_job_sla_report, " +
                             "       flg_job_sla_report_show, flg_job_sla_timer_stop, flg_job_date_start, flg_job_date_finish, flg_user1, " +
                             "       flg_to_client, flg_monthly_check, flg_to_est, flg_from_est, flg_cancelled, " +
                             "       flg_order_has_vos, flg_bill_proforma, flg_inv_has_allocation, flg_filter, flg_order_items_locked, " +
                             "       flg_kier_app_booked, flg_kier_app_attended, job_kier_contact_id, flg_kpi_contacted, kpi_contacted_by_id, " +
                             "       flg_maintenance_order, flg_job_completed, flg_active_complaint, flg_archive, created_by, date_created) " +
                             "VALUES ('" + order.JobRef + "'," + (Information.IsNumeric(order.JobRef) ? 1 : 0)
                             + "," + (order.JobStatusId != null ? order.JobStatusId : 0)
                             + "," + (order.JobClientId.HasValue ? order.JobClientId.Value : -1)
                             + ",'" + (String.IsNullOrEmpty(order.JobClientName) ? " " : order.JobClientName) + "'"
                             + "," + (order.JobAddressId.HasValue ? order.JobAddressId.Value : -1)
                             + ",'" + (String.IsNullOrEmpty(order.JobAddress) ? " " : Utilities.replaceSpecialChars(order.JobAddress)) + "'"
                             + ",'" + (String.IsNullOrEmpty(order.OccupierName) ? "N/A" : Utilities.replaceSpecialChars(order.OccupierName)) + "'"
                             + ",'" + (String.IsNullOrWhiteSpace(order.JobDesc) ? "N/A" : Utilities.replaceSpecialChars(order.JobDesc)) + "'"
                             + ",0,'" + (String.IsNullOrWhiteSpace(order.JobPriorityCode) ? "N/A" : order.JobPriorityCode) + "'"
                             + ", '" + (String.IsNullOrEmpty(order.JobTradeCode) ? "N/A " : order.JobTradeCode) + "'"
                             + ", '" + (String.IsNullOrEmpty(order.JobCostCentre) ? "N/A " : order.JobCostCentre) + "'"
                             + "," + (order.JobManagerId != null ? order.JobManagerId : -1)
                             + ", '" + (String.IsNullOrEmpty(order.JobClientRef) ? "N/A " : order.JobClientRef) + "'"
                             + "," + (order.OrderType.HasValue ? order.OrderType : -1)
                             + "," + Utilities.GetBooleanForDML(databaseType, order.FlgJT)
                             + "," + Utilities.GetDateTimeForDML(databaseType, order.DateJT, true, false)
                             + "," + Utilities.GetBooleanForDML(databaseType, order.FlgUser2)
                             + "," + Utilities.GetDateTimeForDML(databaseType, order.DateUser2, true, true)
                             + "," + Utilities.GetDateTimeForDML(databaseType, order.JobDate, true, true)
                            + ",0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, -1, 0, 0, 0, 0," + createdBy
                            + "," + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ")";
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateOrder(string databaseType, Orders order)
        {
            string returnValue = "";
            switch (databaseType)
            {
                case "MSACCESS":
                case "SQLSERVER":
                default:
                    returnValue = @"UPDATE un_orders SET job_client_id = " + (order.JobClientId.HasValue ? order.JobClientId.Value : -1)
                        + ", job_client_name = '" + (String.IsNullOrEmpty(order.JobClientName) ? " " : order.JobClientName) + "'"
                        + ",job_client_ref = '" + (String.IsNullOrEmpty(order.JobClientRef) ? "N/A" : order.JobClientRef) + "'"
                        + ", job_cost_centre = '" + (String.IsNullOrEmpty(order.JobCostCentre) ? "N/A" : order.JobCostCentre) + "'"
                        + ", job_trade_code = '" + (String.IsNullOrEmpty(order.JobTradeCode) ? "N/A" : order.JobTradeCode) + "'"
                        + ", job_address_id = " + (order.JobAddressId.HasValue ? order.JobAddressId.Value : -1)
                        + ", job_address = '" + (String.IsNullOrEmpty(order.JobAddress) ? " " : Utilities.replaceSpecialChars(order.JobAddress)) + "'"
                        + ", job_address_details = '" + (String.IsNullOrEmpty(order.JobAddressDetail) ? "" : Utilities.replaceSpecialChars(order.JobAddressDetail)) + "'"
                        + ", occupier_name ='" + (String.IsNullOrEmpty(order.OccupierName) ? "N/A" : Utilities.replaceSpecialChars(order.OccupierName)) + "'"
                        + ", occupier_tel_home = '" + (String.IsNullOrEmpty(order.OccupierTelHome) ? "N/A" : Utilities.replaceSpecialChars(order.OccupierTelHome)) + "'"
                        + ", occupier_tel_work = '" + (String.IsNullOrEmpty(order.OccupierTelWork) ? "N/A" : Utilities.replaceSpecialChars(order.OccupierTelWork)) + "'"
                        + ", occupier_tel_work_ext ='" + (String.IsNullOrEmpty(order.OccupierTelWorkExt) ? "N/A" : Utilities.replaceSpecialChars(order.OccupierTelWorkExt)) + "'"
                        + ", occupier_tel_mobile = '" + (String.IsNullOrEmpty(order.OccupierTelMobile) ? "N/A" : Utilities.replaceSpecialChars(order.OccupierTelMobile)) + "'"
                        + ", occupier_email = '" + (String.IsNullOrEmpty(order.OccupierEmail) ? "N/A" : order.OccupierEmail) + "'"
                        + ", job_originator = '" + (String.IsNullOrEmpty(order.JobOriginator) ? "Not Set" : Utilities.replaceSpecialChars(order.JobOriginator)) + "'"
                        + ", job_short_desc = '" + (String.IsNullOrEmpty(order.JobShortDesc) ? " " : Utilities.replaceSpecialChars(order.JobShortDesc)) + "'"
                        + ", job_resolution = '" + (String.IsNullOrEmpty(order.JobResolution) ? "N/A" : Utilities.replaceSpecialChars(order.JobResolution)) + "'"
                        + ", job_desc = '" + (String.IsNullOrWhiteSpace(order.JobDesc) ? "N/A" : Utilities.replaceSpecialChars(order.JobDesc)) + "'"
                        + ", job_date = " + Utilities.GetDateTimeForDML(databaseType, order.JobDate, true, true)
                        + ", job_priority_code= '" + (String.IsNullOrWhiteSpace(order.JobPriorityCode) ? "N/A" : order.JobPriorityCode) + "'"
                        + ", job_date_due = " + Utilities.GetDateTimeForDML(databaseType, order.JobDueDate, true, true)
                        + ", flg_job_sla_timer_stop = " + Utilities.GetBooleanForDML(databaseType, order.FlgJobSlaTimerStop)
                        + ", date_job_sla_timer_stop= " + Utilities.GetDateTimeForDML(databaseType, order.DateJobSlaTimerStop, true, true)
                        + ", flg_job_date_start = " + Utilities.GetBooleanForDML(databaseType, order.FlgJobDateStart)
                        + ", job_date_start = " + Utilities.GetDateTimeForDML(databaseType, order.JobDateStart, true, true)
                        + ", flg_job_date_finish=" + Utilities.GetBooleanForDML(databaseType, order.FlgJobDateFinish)
                        + ", job_date_finish =" + Utilities.GetDateTimeForDML(databaseType, order.JobDateFinish, true, true)
                        + ", flg_user1= " + Utilities.GetBooleanForDML(databaseType, order.FlgUser1)
                        + ", date_user1= " + Utilities.GetDateTimeForDML(databaseType, order.DateUser1, true, true)
                        + ", flg_user2=" + Utilities.GetBooleanForDML(databaseType, order.FlgUser2)
                        + ", date_user2= " + Utilities.GetDateTimeForDML(databaseType, order.DateUser2, true, true)
                        + ", flg_set_to_jt=" + Utilities.GetBooleanForDML(databaseType, order.FlgJT)
                        + ", date_set_to_jt=" + Utilities.GetDateTimeForDML(databaseType, order.DateJT, true, false)
                        + ", flg_to_client=" + Utilities.GetBooleanForDML(databaseType, order.FlgClient)
                        + ", date_to_client = " + Utilities.GetDateTimeForDML(databaseType, order.DateClient, true, true)
                        + ", flg_bill_proforma =" + Utilities.GetBooleanForDML(databaseType, order.FlgBillProforma)
                        + ", retention_pcent=" + order.RetentionPcent
                        + ", sales_discount_pcent = " + order.SalesDiscountPcent
                        + ", flg_job_completed = " + Utilities.GetBooleanForDML(databaseType, order.FlgJobCompleted)
                        + ", job_manager = " + (order.JobManagerId != null ? order.JobManagerId : -1)
                        + ", job_status = " + (order.JobStatusId != null ? order.JobStatusId : 0)
                        + ", order_type=" + (order.OrderType.HasValue ? order.OrderType : -1)
                        + ", job_est_details = '" + (String.IsNullOrWhiteSpace(order.JobEstDetails) ? " " : Utilities.replaceSpecialChars(order.JobEstDetails)) + "'"
                        + ", job_v_o_details = '" + (String.IsNullOrWhiteSpace(order.JobVODetails) ? " " : Utilities.replaceSpecialChars(order.JobVODetails)) + "'"
                        + ", date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, order.LastAmendedDate, true, true)
                        + ", last_amended_by = " + order.LastAmendedBy
                        + " WHERE sequence = " + order.Sequence;
                    break;
            }
            return returnValue;
        }

        internal static string SelectAllFieldsByJobSequence(string datebaseType, long jobSequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT Orders.*, OClientAudit.sequence AS client_audit_sequence, RefTradeCodeType.trade_desc
                    ,ONotes.order_notes, ONoteKPI.kpi_notes,ParentClient.entity_id as parent_client_id, ParentClient.name_short as parent_name_short
                    ,Client.entity_details
                FROM (((((
                     un_orders AS Orders 
                    LEFT JOIN un_order_client_audit AS OClientAudit ON Orders.sequence = OClientAudit.job_sequence) 
                    LEFT JOIN un_ref_trade_code_type AS RefTradeCodeType ON Orders.job_trade_code = RefTradeCodeType.trade_id) 
                    LEFT JOIN un_orders_notes_kpi AS ONoteKPI ON Orders.sequence = ONoteKPI.job_sequence) 
                    LEFT JOIN un_orders_notes AS ONotes ON Orders.sequence = ONotes.job_sequence) 
                    LEFT JOIN un_entity_details_core AS Client ON Orders.job_client_id = Client.entity_id) 
                    LEFT JOIN un_entity_details_core AS ParentClient ON Client.entity_join_id = ParentClient.entity_id
                WHERE Orders.sequence = " + jobSequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string SelectAllFieldsByEBSJobSequence(string datebaseType, long ebsJobSequence)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "SELECT * " +
                                      "  FROM un_orders " +
                                      " WHERE ebs_job_sequence = " + ebsJobSequence;
                        break;

                    case "SQLSERVER":
                    default:
                        returnValue = "SELECT * " +
                                      "  FROM un_orders " +
                                      " WHERE ebs_job_sequence = " + ebsJobSequence;
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string SelectAllFieldsByActiveOrders(string datebaseType)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "SELECT * " +
                             "  FROM un_orders " +
                             " WHERE flg_cancelled <> True " +
                             "   AND flg_archive <> True " +
                             " ORDER BY sequence DESC ";
                        break;
                    case "SQLSERVER":
                        returnValue = "SELECT * " +
                             "  FROM un_orders " +
                             " WHERE flg_cancelled <> 1" +
                             "   AND flg_archive <> 1" +
                             " ORDER BY sequence DESC";
                        break;
                    default:
                        returnValue = "SELECT * " +
                             "  FROM un_orders " +
                             " WHERE flg_cancelled <> 1" +
                             "   AND flg_archive <> 1" +
                             " ORDER BY sequence DESC";
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string SelectAllFieldsByJobRefSearch(string datebaseType, string jobRef)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "SELECT * " +
                                 "  FROM un_orders " +
                                 " WHERE job_ref like '*" + jobRef + "*' " +
                                 "   AND flg_cancelled <> True " +
                                 "   AND flg_archive <> True" +
                                 " ORDER BY sequence DESC";
                        break;
                    case "SQLSERVER":
                        returnValue = "SELECT * " +
                             "  FROM un_orders " +
                             " WHERE job_ref like '%" + jobRef + "%' " +
                             "   AND flg_cancelled <> 1 " +
                             "   AND flg_archive <> 1" +
                             " ORDER BY sequence DESC";
                        break;
                    default:
                        returnValue = "SELECT * " +
                             "  FROM un_orders " +
                             " WHERE job_ref like '%" + jobRef + "%' " +
                             "   AND flg_cancelled <> 1 " +
                             "   AND flg_archive <> 1" +
                             " ORDER BY sequence DESC";
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateCancelFlagByJobRef(string datebaseType, string jobRef)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "UPDATE un_orders " +
                              "   SET flg_cancelled = 1, " +
                              "       date_cancelled = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                              " WHERE job_ref = '" + jobRef + "' ";
                        break;
                    case "SQLSERVER":
                        returnValue = "UPDATE un_orders " +
                              "   SET flg_cancelled = 1, " +
                              "       date_cancelled = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                              " WHERE job_ref = '" + jobRef + "' ";
                        break;
                    default:
                        returnValue = "UPDATE un_orders " +
                              "   SET flg_cancelled = 1, " +
                              "       date_cancelled = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                              " WHERE job_ref = '" + jobRef + "' ";
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateCancelFlagBySequence(string datebaseType, Orders order)
        {
            string returnValue = "";
            try
            {
                returnValue = @"UPDATE un_orders 
            SET flg_cancelled = " + Utilities.GetBooleanForDML(datebaseType, order.FlgJobCancelled)
                   + " ,  date_cancelled = " + Utilities.GetDateTimeForDML(datebaseType, order.DateCancelled, true, true) +
                   "  WHERE sequence = " + order.Sequence;

            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occured in updating cancel flag :" + ex.Message);
            }
            return returnValue;
        }


        internal static string UpdateOrderStatusBySequence(string datebaseType, long sequence, int jobStatus, int userId, DateTime? lastModifiedDate)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_orders " +
                                      "   SET job_status = " + jobStatus + "," +
                                      "       last_amended_by = " + userId + "," +
                                      "       date_last_amended = " + Utilities.GetDateTimeForDML(datebaseType, lastModifiedDate, true, true) +
                                      " WHERE sequence = " + sequence;
                //switch (datebaseType)
                //{
                //    case "MSACCESS":
                //        returnValue = "UPDATE un_orders " +
                //                      "   SET job_status = " + jobStatus + "," +
                //                      "       last_amended_by = " + userId + "," +
                //                      "       date_last_amended = #" + ((DateTime)lastModifiedDate).ToString("MM/dd/yyyy HH:mm:ss") + "# " +
                //                      " WHERE sequence = " + sequence;
                //        break;

                //    case "SQLSERVER":
                //    default:
                //        returnValue = "UPDATE un_orders " +
                //                      "   SET job_status = " + jobStatus + "," +
                //                      "       last_amended_by = " + userId + "," +
                //                      "       date_last_amended = '" + ((DateTime)lastModifiedDate).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                //                      " WHERE sequence = " + sequence;
                //        break;
                //}
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string updateFlgJobCompletedByJobSequence(string datebaseType, long sequence, bool flgJobCompleted,
                                                                 int userId, DateTime? lastModifiedDate)
        {
            return "UPDATE un_orders SET " +
                    "    flg_job_completed = " + Utilities.GetBooleanForDML(datebaseType, flgJobCompleted) + ", " +
                    "       last_amended_by = " + userId + ", " +
                    "       date_last_amended = " + Utilities.GetDateTimeForDML(datebaseType, lastModifiedDate, true, true) +
                    " WHERE sequence = " + sequence;
        }
        internal static string UpdateFlgBillProformaBySequence(string datebaseType, long sequence, bool flgBillProforma, int userId, DateTime? lastModifiedDate)
        {

            string returnValue = "";

            try
            {
                returnValue = "UPDATE un_orders " +
                                      "   SET flg_bill_proforma = " + flgBillProforma + "," +
                                      "       last_amended_by = " + userId + "," +
                                      "       date_last_amended = " + Utilities.GetDateTimeForDML(datebaseType, lastModifiedDate, true, true) +
                                      " WHERE sequence = " + sequence;
                //switch (datebaseType)
                //{
                //    case "MSACCESS":
                //        returnValue = "UPDATE un_orders " +
                //                      "   SET flg_bill_proforma = " + flgBillProforma + "," +
                //                      "       last_amended_by = " + userId + "," +
                //                      "       date_last_amended = " + Utilities.getAccessDate(lastModifiedDate) +
                //                      " WHERE sequence = " + sequence;
                //        break;

                //    case "SQLSERVER":
                //    default:
                //        returnValue = "UPDATE un_orders " +
                //                      "   SET flg_bill_proforma = " + Utilities.getSQLBoolean(flgBillProforma) + "," +
                //                      "       last_amended_by = " + userId + "," +
                //                      "       date_last_amended = " + Utilities.getSQLDate(lastModifiedDate) +
                //                      " WHERE sequence = " + sequence;
                //        break;
                //}
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateJobAddress(string datebaseType, OrdersJobAddress jobAddress)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = "UPDATE un_orders " +
                            "   SET job_address = '" + jobAddress.JobAddress + "' ," +
                            "   job_address_id = " + jobAddress.EntityId + " ," +
                            "   occupier_name = '" + (String.IsNullOrEmpty(jobAddress.NameTitle) ? "N/A" : jobAddress.NameTitle) + "' , " +
                            "   occupier_tel_home = '" + (String.IsNullOrEmpty(jobAddress.Telephone) ? "N/A" : jobAddress.Telephone) + "' , " +
                            "   occupier_tel_work = '" + (String.IsNullOrEmpty(jobAddress.TelWork) ? "N/A" : jobAddress.TelWork) + "' , " +
                            "   occupier_tel_work_ext = '" + (String.IsNullOrEmpty(jobAddress.TelExt) ? "N/A" : jobAddress.TelExt) + "' , " +
                            "   occupier_tel_mobile = '" + (String.IsNullOrEmpty(jobAddress.TelMobile) ? "N/A" : jobAddress.TelMobile) + "' , " +
                            "   occupier_email = '" + (String.IsNullOrEmpty(jobAddress.Email) ? "N/A" : jobAddress.Email) + "' " +
                            " WHERE sequence = " + jobAddress.Sequence;
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateJobAddress(string datebaseType, long sequence, string jobAddress, int jobAddressId, string name, string telHome, string telWork, string telMobile, string email)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = "UPDATE un_orders " +
                              "   SET job_address = '" + jobAddress + "' , " +
                              "   job_address_id = " + jobAddressId + " , " +
                              "   occupier_name = '" + name + "' , " +
                              "   occupier_tel_home = '" + telHome + "' , " +
                              "   occupier_tel_work = '" + telWork + "' , " +
                              "   occupier_tel_mobile = '" + telMobile + "' , " +
                              "   occupier_email = '" + email + "' " +
                              " WHERE sequence = " + sequence;
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateJobAddressByAddressId(string datebaseType, long addressId, string jobAddress)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = "UPDATE un_orders " +
                              "   SET job_address = '" + jobAddress + "'" +
                               " WHERE job_address_id = " + addressId;
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateJobClient(string datebaseType, long sequence, long jobClientID, string jobClientName)
        {
            string returnValue = "";
            try
            {
                returnValue = @"UPDATE un_orders 
                            SET job_client_id = " + jobClientID + " , " +
                        "   job_client_name = '" + jobClientName + "'  " +
                        " WHERE sequence = " + sequence;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateJobClientName(string datebaseType, int sequence, string jobClientName)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = "UPDATE un_orders " +
                             "   SET job_client_name = '" + jobClientName + "' " +
                               " WHERE sequence = " + sequence;
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateJobClientRef(string datebaseType, int sequence, string jobClientRef)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = "UPDATE un_orders " +
                             "   SET job_client_ref = '" + jobClientRef + "' " +
                               " WHERE sequence = " + sequence;
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateFlgUser1AndDateUser1ByJobSequence(string datebaseType, long sequence, bool flgUser1,
                                                                       DateTime? datUser1, int userId, DateTime? lastModifiedDate)
        {
            string returnValue = "";
            try
            {

                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "UPDATE un_orders " +
                                      "   SET flg_user1 = " + flgUser1 + ", " +
                                      "       date_user1 = " + Utilities.getAccessDate(datUser1) + ", " +
                                      "       last_amended_by = " + userId + ", " +
                                      "       date_last_amended = " + Utilities.getAccessDate(lastModifiedDate) + " " +
                                      " WHERE sequence = " + sequence;
                        break;
                    case "SQLSERVER":
                        returnValue = "UPDATE un_orders " +
                                      "   SET flg_user1 = " + Utilities.getSQLBoolean(flgUser1) + ", " +
                                      "       date_user1 = " + Utilities.getSQLDate(datUser1) + ", " +
                                      "       last_amended_by = " + userId + ", " +
                                      "       date_last_amended = " + Utilities.getSQLDate(lastModifiedDate) + " " +
                                      " WHERE sequence = " + sequence;
                        break;
                    default:
                        returnValue = "UPDATE un_orders " +
                                      "   SET flg_user1 = " + flgUser1 + ", " +
                                      "       date_user1 = " + Utilities.getAccessDate(datUser1) + ", " +
                                      "       last_amended_by = " + userId + ", " +
                                      "       date_last_amended = " + Utilities.getAccessDate(lastModifiedDate) + " " +
                                      " WHERE sequence = " + sequence;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateFlgSetToJTAndDateSetToJTByJobSequence(string datebaseType, long sequence, bool flgSetToJT,
                                                                           DateTime? datSetToJT, int userId, DateTime? lastModifiedDate)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "UPDATE un_orders " +
                                      "   SET flg_set_to_jt = " + flgSetToJT + ", " +
                                      "       date_set_to_jt = " + Utilities.getAccessDate(datSetToJT) + ", " +
                                      "       last_amended_by = " + userId + ", " +
                                      "       date_last_amended = " + Utilities.getAccessDate(lastModifiedDate) + " " +
                                      " WHERE sequence = " + sequence;
                        break;
                    case "SQLSERVER":
                        returnValue = "UPDATE un_orders " +
                                     "   SET flg_set_to_jt = " + Utilities.getSQLBoolean(flgSetToJT) + ", " +
                                     "       date_set_to_jt = " + Utilities.getSQLDate(datSetToJT) + ", " +
                                     "       last_amended_by = " + userId + ", " +
                                     "       date_last_amended = " + Utilities.getSQLDate(lastModifiedDate) + " " +
                                     " WHERE sequence = " + sequence;
                        break;
                    default:
                        returnValue = "UPDATE un_orders " +
                                      "   SET flg_set_to_jt = " + Utilities.getSQLBoolean(flgSetToJT) + ", " +
                                      "       date_set_to_jt = " + Utilities.getSQLDate(datSetToJT) + ", " +
                                      "       last_amended_by = " + userId + ", " +
                                      "       date_last_amended = " + Utilities.getSQLDate(lastModifiedDate) + " " +
                                      " WHERE sequence = " + sequence;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateUserFlag2AndDateUser2ByJobSequence(string datebaseType, long sequence, bool flgUser2,
                                                                        DateTime? datUser2, int userId, DateTime? lastModifiedDate)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "UPDATE un_orders " +
                                      "   SET flg_user2 = " + flgUser2 + ", ";
                        if (datUser2 != null && datUser2 != DateTime.MinValue)
                        {
                            returnValue += "       date_user2 = " + Utilities.getAccessDate(datUser2) + ", ";
                        }
                        returnValue += "       last_amended_by = " + userId + ", " +
                                       "       date_last_amended = " + Utilities.getAccessDate(lastModifiedDate) + " " +
                                       " WHERE sequence = " + sequence;
                        break;
                    case "SQLSERVER":
                        returnValue = "UPDATE un_orders " +
                                     "   SET flg_user2 = " + Utilities.getSQLBoolean(flgUser2) + ", ";
                        if (datUser2 != null && datUser2 != DateTime.MinValue)
                        {
                            returnValue += "       date_user2 = " + Utilities.getSQLDate(datUser2) + ", ";
                        }
                        returnValue += "       last_amended_by = " + userId + ", " +
                                       "       date_last_amended = " + Utilities.getSQLDate(lastModifiedDate) + " " +
                                       " WHERE sequence = " + sequence;
                        break;
                    default:
                        returnValue = "UPDATE un_orders " +
                                      "   SET flg_user2 = " + Utilities.getSQLBoolean(flgUser2) + ", ";
                        if (datUser2 != null && datUser2 != DateTime.MinValue)
                        {
                            returnValue += "       date_user2 = " + Utilities.getSQLDate(datUser2) + ", ";
                        }
                        returnValue += "       last_amended_by = " + userId + ", " +
                                       "       date_last_amended = " + Utilities.getSQLDate(lastModifiedDate) + " " +
                                       " WHERE sequence = " + sequence;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string GetOrdersList(string databaseType, ClientRequest clientRequest)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT " + getOrdersListColumns(databaseType, clientRequest);
                returnValue = returnValue + " FROM((un_orders AS ord LEFT JOIN un_ref_job_status_type AS rjst " +
                                            "   ON ord.job_status = rjst.status_id) " +
                                            " LEFT JOIN un_entity_details_core AS edc_p " +
                                            "   ON ord.job_address_id = edc_p.entity_id) " +
                                            " LEFT JOIN un_ref_trade_code_type AS rtct " +
                                            "   ON ord.job_trade_code = rtct.trade_id ";
                if (clientRequest.globalFilter != null && clientRequest.globalFilter != "")
                {
                    string filterValue = clientRequest.globalFilter;
                    string globalFilterQuery = " where ord.job_ref like '%" + filterValue + "%' or ord.job_client_name like '%"
                        + filterValue + "%' or rjst.status_desc like '%" + filterValue + "%' or rtct.trade_desc like '%"
                        + filterValue + "%'  or ord.job_client_ref like '%" + filterValue + "%' or edc_p.address_post_code like '%"
                        + filterValue + "%' or ord.job_address like '%" + filterValue + "%'";
                    returnValue += globalFilterQuery;
                }
                string sortColumn = "ord.job_date"; // default sort
                if (clientRequest.sortField != null)
                {
                    switch (clientRequest.sortField)
                    {
                        case "JobRef":
                            sortColumn = "ord.job_ref";
                            break;
                        case "RecordType":
                            sortColumn = "ord.flg_set_to_jt";
                            break;
                        case "JobDate":
                            sortColumn = "ord.job_date";
                            break;
                        case "JobClientName":
                            sortColumn = "ord.job_client_name";
                            break;
                        case "StatusDescription":
                            sortColumn = "rjst.status_desc";
                            break;
                    }
                    string orderType = "DESC";
                    if (clientRequest.sortOrder == -1)
                    {
                        orderType = "DESC";
                    }
                    else
                    {
                        orderType = "ASC";
                    }
                    returnValue += " ORDER BY " + sortColumn + " " + orderType;
                }
                else
                {
                    returnValue += " ORDER BY " + sortColumn + " DESC";
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string GetCountOfOrdersList(string databaseType, ClientRequest clientRequest)
        {
            string returnValue = "";
            try
            {
                returnValue = @"Select Count(ord.sequence ) 
                    FROM((un_orders AS ord 
                        LEFT JOIN un_ref_job_status_type AS rjst ON ord.job_status = rjst.status_id) 
                        LEFT JOIN un_entity_details_core AS edc_p  ON ord.job_address_id = edc_p.entity_id) 
                        LEFT JOIN un_ref_trade_code_type AS rtct  ON ord.job_trade_code = rtct.trade_id ";
                if (clientRequest.globalFilter != null && clientRequest.globalFilter != "")
                {
                    string filterValue = clientRequest.globalFilter;
                    string globalFilterQuery = " where ord.job_ref like '%" + filterValue + "%' or ord.job_client_name like '%"
                        + filterValue + "%' or rjst.status_desc like '%" + filterValue + "%' or rtct.trade_desc like '%"
                        + filterValue + "%'  or ord.job_client_ref like '%" + filterValue + "%' or edc_p.address_post_code like '%"
                        + filterValue + "%' or ord.job_address like '%" + filterValue + "%'";
                    returnValue += globalFilterQuery;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string GetOrdersList2(string databaseType, int size)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT Top " + size + @" ord.sequence, ord.job_ref, ord.job_client_name, ord.job_date,ord.date_to_client,ord.date_set_to_jt, ord.order_type, ord.flg_set_to_jt, ord.flg_to_client, 
                    edc_p.address_post_code, ord.job_client_ref, ord.job_trade_code, rtct.trade_desc,rjst.status_desc 
                    ,ord.flg_no_access_ref_set, ord.flg_cancelled, ord.flg_job_completed
                  FROM((un_orders AS ord LEFT JOIN un_ref_job_status_type AS rjst 
                   ON ord.job_status = rjst.status_id) 
                 LEFT JOIN un_entity_details_core AS edc_p 
                   ON ord.job_address_id = edc_p.entity_id) 
                 LEFT JOIN un_ref_trade_code_type AS rtct 
                   ON ord.job_trade_code = rtct.trade_id 
                    ORDER BY ord.job_date DESC";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        private static string getOrdersListColumns(string databaseType, ClientRequest requestModel)
        {
            //TODO: Need to enhance it to get values from ApplicationWebPagesFields.
            return " ord.sequence, ord.job_ref, ord.job_client_name, ord.job_date,ord.date_to_client,ord.date_set_to_jt, ord.order_type, ord.flg_set_to_jt, ord.flg_to_client, " +
                   " edc_p.address_post_code, ord.job_client_ref, ord.job_trade_code, rtct.trade_desc,rjst.status_desc " +
                   " ,ord.job_address,ord.flg_no_access_ref_set, ord.flg_cancelled, ord.flg_job_completed";
        }

        internal static string UpdateFlgJobStartAndJobDateStartByJobSequence(string datebaseType, long sequence, bool flgJobDateStart,
                                                                             DateTime? jobDateStart, int userId, DateTime? lastModifiedDate)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "UPDATE un_orders " +
                                      "   SET flg_job_date_start = " + flgJobDateStart + ", ";
                        if (jobDateStart != null && jobDateStart != DateTime.MinValue)
                        {
                            returnValue += "       job_date_start = " + Utilities.getAccessDate(jobDateStart) + ", ";
                        }
                        returnValue += "       last_amended_by = " + userId + ", " +
                                       "       date_last_amended = " + Utilities.getAccessDate(lastModifiedDate) + " " +
                                       " WHERE sequence = " + sequence;
                        break;
                    case "SQLSERVER":
                        returnValue = "UPDATE un_orders " +
                                      "   SET flg_job_date_start = " + Utilities.getSQLBoolean(flgJobDateStart) + ", ";
                        if (jobDateStart != null && jobDateStart != DateTime.MinValue)
                        {
                            returnValue += "       job_date_start = " + Utilities.getSQLDate(jobDateStart) + ", ";
                        }
                        returnValue += "       last_amended_by = " + userId + ", " +
                                       "       date_last_amended = " + Utilities.getSQLDate(lastModifiedDate) + " " +
                                       " WHERE sequence = " + sequence;
                        break;
                    default:
                        returnValue = "UPDATE un_orders " +
                                      "   SET flg_job_date_start = " + Utilities.getSQLBoolean(flgJobDateStart) + ", ";
                        if (jobDateStart != null && jobDateStart != DateTime.MinValue)
                        {
                            returnValue += "       job_date_start = " + Utilities.getSQLDate(jobDateStart) + ", ";
                        }
                        returnValue += "       last_amended_by = " + userId + ", " +
                                       "       date_last_amended = " + Utilities.getSQLDate(lastModifiedDate) + " " +
                                       " WHERE sequence = " + sequence;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateFlgJobFinishAndJobDateFinishByJobSequence(string datebaseType, long sequence, bool flgJobDateFinish,
                                                                               DateTime? jobDateFinish, int userId, DateTime? lastModifiedDate)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "UPDATE un_orders " +
                                      "   SET flg_job_date_finish = " + flgJobDateFinish + ", ";
                        if (jobDateFinish != null && jobDateFinish != DateTime.MinValue)
                        {
                            returnValue += "       job_date_finish = " + Utilities.getAccessDate(jobDateFinish) + ", ";
                        }
                        returnValue += "       last_amended_by = " + userId + ", " +
                                       "       date_last_amended = " + Utilities.getAccessDate(lastModifiedDate) + " " +
                                       " WHERE sequence = " + sequence;
                        break;
                    case "SQLSERVER":
                    default:
                        returnValue = "UPDATE un_orders " +
                                      "   SET flg_job_date_finish = " + Utilities.getSQLBoolean(flgJobDateFinish) + ", ";
                        if (jobDateFinish != null && jobDateFinish != DateTime.MinValue)
                        {
                            returnValue += "       job_date_finish = " + Utilities.getSQLDate(jobDateFinish) + ", ";
                        }
                        returnValue += "       last_amended_by = " + userId + ", " +
                                       "       date_last_amended = " + Utilities.getSQLDate(lastModifiedDate) + " " +
                                       " WHERE sequence = " + sequence;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateFlgJobSLATimerStopAndDateJobSLATimerStopByJobSequence(string datebaseType, long sequence,
                                                                                           bool flgSLATimerStop, DateTime? slaTimerStopDate,
                                                                                           int userId, DateTime? lastModifiedDate)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "UPDATE un_orders " +
                                      "   SET flg_job_sla_timer_stop = " + flgSLATimerStop + ", ";
                        if (slaTimerStopDate != null && slaTimerStopDate != DateTime.MinValue)
                        {
                            returnValue += "       date_job_sla_timer_stop = " + Utilities.getAccessDate(slaTimerStopDate) + ", ";
                        }
                        returnValue += "       last_amended_by = " + userId + ", " +
                                       "       date_last_amended = " + Utilities.getAccessDate(lastModifiedDate) + " " +
                                       " WHERE sequence = " + sequence;
                        break;

                    case "SQLSERVER":
                    default:
                        returnValue = "UPDATE un_orders " +
                                      "   SET flg_job_sla_timer_stop = " + Utilities.getSQLBoolean(flgSLATimerStop) + ", ";
                        if (slaTimerStopDate != null && slaTimerStopDate != DateTime.MinValue)
                        {
                            returnValue += "       date_job_sla_timer_stop = " + Utilities.getSQLDate(slaTimerStopDate) + ", ";
                        }
                        returnValue += "       last_amended_by = " + userId + ", " +
                                       "       date_last_amended = " + Utilities.getSQLDate(lastModifiedDate) + " " +
                                       " WHERE sequence = " + sequence;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string UpdateOrderInfo(string datebaseType, SimplicityOnlineBLL.Entities.Orders order, string infoType)
        {
            string returnValue = "";
            string fieldName = "", field2Name = "";
            object fieldValue = null, field2Value = null;

            switch (infoType)
            {

                case "job_client_ref": fieldName = "job_client_ref"; fieldValue = "'" + order.JobClientRef + "'"; break;
                case "job_cost_centre": fieldName = "job_cost_centre"; fieldValue = "'" + order.JobCostCentre + "'"; break;
                case "job_trade_code": fieldName = "job_trade_code"; fieldValue = "'" + order.JobTradeCode + "'"; break;
                case "occupier_name": fieldName = "occupier_name"; fieldValue = "'" + order.OccupierName + "'"; break;
                case "occupier_tel_home": fieldName = "occupier_tel_home"; fieldValue = "'" + order.OccupierTelHome + "'"; break;
                case "occupier_tel_work": fieldName = "occupier_tel_work"; fieldValue = "'" + order.OccupierTelWork + "'"; break;
                case "occupier_tel_work_ext": fieldName = "occupier_tel_work_ext"; fieldValue = "'" + order.OccupierTelWorkExt + "'"; break;
                case "occupier_tel_mobile": fieldName = "occupier_tel_mobile"; fieldValue = "'" + order.OccupierTelMobile + "'"; break;
                case SimplicityConstants.DB_FIELD_ORDERS_OCCUPIER_EMAIL: fieldName = "occupier_email"; fieldValue = "'" + order.OccupierEmail + "'"; break;
                case "job_originator": fieldName = "job_originator"; fieldValue = "'" + order.JobOriginator + "'"; break;
                case "job_resolution": fieldName = "job_resolution"; fieldValue = "'" + order.JobResolution + "'"; break;
                case "job_short_desc": fieldName = "job_short_desc"; fieldValue = "'" + order.JobShortDesc + "'"; break;
                case "job_desc": fieldName = "job_desc"; fieldValue = "'" + order.JobDesc + "'"; break;
                case "flg_set_to_jt": fieldName = "flg_set_to_jt"; fieldValue = order.FlgJT ? 1 : 0; break;
                case "flg_job_sla_timer_stop": fieldName = "flg_job_sla_timer_stop"; fieldValue = order.FlgJobSlaTimerStop ? 1 : 0; break;
                case "flg_user2": fieldName = "flg_user2"; fieldValue = order.FlgUser2 ? 1 : 0; break;
                case "flg_job_completed": fieldName = "flg_job_completed"; fieldValue = order.FlgJobCompleted ? 1 : 0; break;
                case "retention_pcent": fieldName = "retention_pcent"; fieldValue = order.RetentionPcent; break;
                case "sales_discount_pcent": fieldName = "sales_discount_pcent"; fieldValue = order.SalesDiscountPcent; break;
                case "flg_job_date_start": fieldName = "flg_job_date_start"; fieldValue = order.FlgJobDateStart ? 1 : 0; break;
                case "flg_job_date_finish": fieldName = "flg_job_date_finish"; fieldValue = order.FlgJobDateFinish ? 1 : 0; break;
                case "date_set_to_jt":
                case "job_date":
                case "date_job_sla_timer_stop":
                case "date_user2":
                case "job_date_due":
                case "job_date_start":
                case "job_date_finish":
                    break;


                default:
                    throw new ArgumentException("Invlid field");
                    break;
            }

            if (String.IsNullOrWhiteSpace(fieldName) == false)
            {
                returnValue = String.Format("UPDATE un_orders SET {0} = {1} WHERE sequence = {2}", fieldName, fieldValue, order.Sequence);
            }

            if (datebaseType == "SQLSERVER")
            {
                switch (infoType)
                {
                    case "date_set_to_jt": fieldName = "date_set_to_jt"; fieldValue = "'" + order.DateJT.Value.ToString("MM/dd/yyyy") + "'"; field2Name = "flg_set_to_jt"; field2Value = order.FlgJT ? 1 : 0; break;
                    case "job_date": fieldName = "job_date"; fieldValue = "'" + order.JobDate.Value.ToString("MM/dd/yyyy") + "'"; break;
                    case "job_date_due": fieldName = "job_date_due"; fieldValue = "'" + order.JobDueDate + "'"; break;
                    case "date_job_sla_timer_stop": fieldName = "date_job_sla_timer_stop"; fieldValue = "'" + order.DateJobSlaTimerStop.Value.ToString("MM/dd/yyyy") + "'"; field2Name = "flg_job_sla_timer_stop"; field2Value = order.FlgJobSlaTimerStop ? 1 : 0; break;
                    case "date_user2": fieldName = "date_user2"; fieldValue = "'" + order.DateUser2.Value.ToString("MM/dd/yyyy") + "'"; field2Name = "flg_user2"; field2Value = order.FlgUser2 ? 1 : 0; break; break;
                    case "job_date_start": fieldName = "job_date_start"; fieldValue = "'" + order.JobDateStart.Value.ToString("MM/dd/yyyy") + "'"; field2Name = "flg_job_date_start"; field2Value = order.FlgJobDateStart ? 1 : 0; break;
                    case "job_date_finish": fieldName = "job_date_finish"; fieldValue = "'" + order.JobDateFinish.Value.ToString("MM/dd/yyyy") + "'"; field2Name = "flg_job_date_finish"; field2Value = order.FlgJobDateFinish ? 1 : 0; break;
                }
            }
            else
            {
                switch (infoType)
                {
                    case "date_set_to_jt": fieldName = "date_set_to_jt"; fieldValue = "#" + order.DateJT.Value.ToString("MM/dd/yyyy") + "#"; field2Name = "flg_set_to_jt"; field2Value = order.FlgJT ? 1 : 0; break;
                    case "job_date": fieldName = "job_date"; fieldValue = "#" + order.JobDate.Value.ToString("MM/dd/yyyy") + "#"; break;
                    case "job_date_due": fieldName = "job_date_due"; fieldValue = "#" + order.JobDueDate.Value.ToString("MM/dd/yyyy") + "#"; break;
                    case "date_job_sla_timer_stop": fieldName = "date_job_sla_timer_stop"; fieldValue = "#" + order.DateJobSlaTimerStop.Value.ToString("MM/dd/yyyy") + "#"; field2Name = "flg_job_sla_timer_stop"; field2Value = order.FlgJobSlaTimerStop ? 1 : 0; break;
                    case "date_user2": fieldName = "date_user2"; fieldValue = "#" + order.DateUser2.Value.ToString("MM/dd/yyyy") + "#"; field2Name = "flg_user2"; field2Value = order.FlgUser2 ? 1 : 0; break; break;
                    case "job_date_start": fieldName = "job_date_start"; fieldValue = "#" + order.JobDateStart.Value.ToString("MM/dd/yyyy") + "#"; field2Name = "flg_job_date_start"; field2Value = order.FlgJobDateStart ? 1 : 0; break;
                    case "job_date_finish": fieldName = "job_date_finish"; fieldValue = "#" + order.JobDateFinish.Value.ToString("MM/dd/yyyy") + "#"; field2Name = "flg_job_date_finish"; field2Value = order.FlgJobDateFinish ? 1 : 0; break;
                }

            }



            if (String.IsNullOrWhiteSpace(fieldName) == false && String.IsNullOrWhiteSpace(field2Name) == false)
            {
                returnValue = String.Format("UPDATE un_orders SET {0} = {1}, {3} = {4} WHERE sequence = {2}", fieldName, fieldValue, order.Sequence, field2Name, field2Value);
            }
            else if (String.IsNullOrWhiteSpace(fieldName) == false && String.IsNullOrWhiteSpace(fieldName) == false)
            {
                returnValue = String.Format("UPDATE un_orders SET {0} = {1} WHERE sequence = {2}", fieldName, fieldValue, order.Sequence);
            }


            return returnValue;
        }

        internal static string SelectAllFieldsBySearchCriteria(string datebaseType, bool isJobRef, string jobRef, bool isTag, string tagNo, bool isTagCreatedDate, DateTime? tagCreatedDate, bool isTagUser, int tagUser)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "SELECT un_orders.*, " +
                                        " un_cld_ord_labels.sequence as un_cld_ord_labels_sequence, " +
                                        " un_cld_ord_labels.job_sequence as un_cld_ord_labels_job_sequence,  " +
                                        " un_cld_ord_labels.tag_no as un_cld_ord_labels_tag_no, " +

                                        " un_cld_ord_labels_files.sequence 	as	un_cld_ord_labels_files_sequence, " +
                                        " un_cld_ord_labels_files.job_sequence as un_cld_ord_labels_files_job_sequence,  " +
                                        " un_cld_ord_labels_files.oi_sequence as un_cld_ord_labels_files_oi_sequence,  " +
                                        " un_cld_ord_labels_files.header_sequence as un_cld_ord_labels_files_header_sequence,  " +
                                        " un_cld_ord_labels_files.join_sequence as un_cld_ord_labels_files_join_sequence,  " +
                                        " un_cld_ord_labels_files.flg_deleted as un_cld_ord_labels_files_flg_deleted,  " +
                                        " un_cld_ord_labels_files.date_file_date as un_cld_ord_labels_files_date_file_date,  " +
                                        " un_cld_ord_labels_files.file_desc as un_cld_ord_labels_files_file_desc,  " +
                                        " un_cld_ord_labels_files.image_url as un_cld_ord_labels_files_image_url,  " +
                                        " un_cld_ord_labels_files.logo_url as un_cld_ord_labels_files_logo_url,  " +
                                        " un_cld_ord_labels_files.file_name_and_path as un_cld_ord_labels_files_file_name_and_path,  " +
                                        " un_cld_ord_labels_files.created_by as un_cld_ord_labels_files_created_by,  " +
                                        " un_cld_ord_labels_files.date_created as un_cld_ord_labels_files_date_created,  " +
                                        " un_cld_ord_labels_files.last_amended_by as un_cld_ord_labels_files_last_amended_by,  " +
                                        " un_cld_ord_labels_files.date_last_amended as un_cld_ord_labels_files_date_last_amended,  " +
                                        //" un_cld_ord_labels_files.image_user_name as un_cld_ord_labels_files_image_user_name,  " +
                                        //" un_cld_ord_labels_files.add_info_sequence as un_cld_ord_labels_files_add_info_sequence, " +
                                        //" un_cld_ord_labels_files.add_info as un_cld_ord_labels_files_add_info, " +
                                        " un_cld_ord_labels_files.drive_file_id as un_cld_ord_labels_files_drive_file_id, " +
                                        " un_user_details.user_name " +

                                        " FROM((un_orders LEFT JOIN un_cld_ord_labels ON un_orders.sequence = un_cld_ord_labels.job_sequence) " +
                                        " LEFT JOIN un_cld_ord_labels_files ON un_cld_ord_labels.sequence = un_cld_ord_labels_files.join_sequence) " +
                                        " LEFT JOIN un_user_details ON un_cld_ord_labels_files.created_by = un_user_details.user_id " +
                                        " WHERE un_orders.flg_cancelled <> 1 " +
                                        " AND un_orders.flg_archive <> 1 " +
                                        " AND (un_cld_ord_labels_files.sequence is null OR (un_cld_ord_labels_files.sequence is not null AND un_cld_ord_labels_files.flg_deleted <> 1)) ";

                        if (isJobRef)
                        {
                            returnValue = returnValue + " AND un_orders.job_ref like '%" + jobRef + "%' ";
                        }
                        if (isTag)
                        {
                            returnValue = returnValue + " AND un_cld_ord_labels.tag_no LIKE '%" + tagNo + "%'";
                        }
                        if (isTagUser)
                        {
                            returnValue = returnValue + " AND un_cld_ord_labels_files.created_by = " + tagUser;
                        }
                        if (isTagCreatedDate)
                        {
                            returnValue = returnValue + " AND un_cld_ord_labels_files.date_file_date >= #" + ((DateTime)tagCreatedDate).ToString("MM/dd/yyyy") + "# AND un_cld_ord_labels_files.date_file_date < #" + ((DateTime)tagCreatedDate).AddDays(1).ToString("MM/dd/yyyy") + "#";
                        }
                        returnValue = returnValue + " ORDER by un_orders.sequence desc ";
                        break;
                    case "SQLSERVER":
                    default:
                        returnValue = "SELECT un_orders.*, " +
                                        " un_cld_ord_labels.sequence as un_cld_ord_labels_sequence, " +
                                        " un_cld_ord_labels.job_sequence as un_cld_ord_labels_job_sequence,  " +
                                        " un_cld_ord_labels.tag_no as un_cld_ord_labels_tag_no, " +

                                        " un_cld_ord_labels_files.sequence 	as	un_cld_ord_labels_files_sequence, " +
                                        " un_cld_ord_labels_files.job_sequence as un_cld_ord_labels_files_job_sequence,  " +
                                        " un_cld_ord_labels_files.oi_sequence as un_cld_ord_labels_files_oi_sequence,  " +
                                        " un_cld_ord_labels_files.header_sequence as un_cld_ord_labels_files_header_sequence,  " +
                                        " un_cld_ord_labels_files.join_sequence as un_cld_ord_labels_files_join_sequence,  " +
                                        " un_cld_ord_labels_files.flg_deleted as un_cld_ord_labels_files_flg_deleted,  " +
                                        " un_cld_ord_labels_files.date_file_date as un_cld_ord_labels_files_date_file_date,  " +
                                        " un_cld_ord_labels_files.file_desc as un_cld_ord_labels_files_file_desc,  " +
                                        " un_cld_ord_labels_files.image_url as un_cld_ord_labels_files_image_url,  " +
                                        " un_cld_ord_labels_files.logo_url as un_cld_ord_labels_files_logo_url,  " +
                                        " un_cld_ord_labels_files.file_name_and_path as un_cld_ord_labels_files_file_name_and_path,  " +
                                        " un_cld_ord_labels_files.created_by as un_cld_ord_labels_files_created_by,  " +
                                        " un_cld_ord_labels_files.date_created as un_cld_ord_labels_files_date_created,  " +
                                        " un_cld_ord_labels_files.last_amended_by as un_cld_ord_labels_files_last_amended_by,  " +
                                        " un_cld_ord_labels_files.date_last_amended as un_cld_ord_labels_files_date_last_amended,  " +
                                        //" un_cld_ord_labels_files.image_user_name as un_cld_ord_labels_files_image_user_name,  " +
                                        //" un_cld_ord_labels_files.add_info_sequence as un_cld_ord_labels_files_add_info_sequence, " +
                                        //" un_cld_ord_labels_files.add_info as un_cld_ord_labels_files_add_info, " +
                                        " un_cld_ord_labels_files.drive_file_id as un_cld_ord_labels_files_drive_file_id, " +
                                        " un_user_details.user_name " +
                                "  FROM un_orders " +
                                " LEFT OUTER JOIN un_cld_ord_labels ON un_orders.sequence = un_cld_ord_labels.job_sequence " +
                                " LEFT OUTER JOIN un_cld_ord_labels_files ON un_cld_ord_labels.sequence = un_cld_ord_labels_files.join_sequence" +
                                " LEFT OUTER JOIN un_user_details ON un_cld_ord_labels_files.created_by = un_user_details.user_id " +
                                " WHERE un_orders.flg_cancelled <> 1 " +
                                " AND un_orders.flg_archive <> 1 " +
                                " AND (un_cld_ord_labels_files.sequence is null OR (un_cld_ord_labels_files.sequence is not null AND un_cld_ord_labels_files.flg_deleted <> 1)) ";

                        if (isJobRef)
                        {
                            returnValue = returnValue + " AND un_orders.job_ref like '%" + jobRef + "%' ";
                        }
                        if (isTag)
                        {
                            returnValue = returnValue + " AND un_cld_ord_labels.tag_no LIKE '%" + tagNo + "%'";
                        }
                        if (isTagUser)
                        {
                            returnValue = returnValue + " AND un_cld_ord_labels_files.created_by = " + tagUser;
                        }
                        if (isTagCreatedDate)
                        {
                            returnValue = returnValue + " AND un_cld_ord_labels_files.date_file_date >= '" + ((DateTime)tagCreatedDate).ToString("MM/dd/yyyy") + "' AND un_cld_ord_labels_files.date_file_date < '" + ((DateTime)tagCreatedDate).AddDays(1).ToString("MM/dd/yyyy") + "'";
                        }
                        returnValue = returnValue + " ORDER by un_orders.sequence desc ";
                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string GetJobRefListForTimeSheet(string datebaseType, bool isJobRef, string jobRef, bool isTag, string tagNo, bool isTagCreatedDate, DateTime? tagCreatedDate, bool isTagUser, int tagUser)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "SELECT un_orders.job_ref,job_address " +
                                     " FROM un_orders " +
                                     "	LEFT OUTER JOIN un_cld_ord_labels ON un_orders.sequence = un_cld_ord_labels.job_sequence " +
                                     "	LEFT OUTER JOIN un_cld_ord_labels_files ON un_cld_ord_labels.sequence = un_cld_ord_labels_files.join_sequence ";

                        if (isJobRef)
                            returnValue = returnValue + " AND un_orders.job_ref like '%" + jobRef + "%' ";

                        if (isTag)
                            returnValue = returnValue + " AND un_cld_ord_labels.tag_no LIKE '%" + tagNo + "%'";

                        if (isTagUser)
                            returnValue = returnValue + " AND un_cld_ord_labels_files.created_by = " + tagUser;

                        if (isTagCreatedDate)
                            returnValue = returnValue + " AND un_cld_ord_labels_files.date_file_date >= #" + ((DateTime)tagCreatedDate).ToString("MM/dd/yyyy") + "# AND un_cld_ord_labels_files.date_file_date < #" + ((DateTime)tagCreatedDate).AddDays(1).ToString("MM/dd/yyyy") + "#";

                        returnValue = returnValue + " ORDER by un_orders.sequence desc ";

                        break;
                    case "SQLSERVER":
                    default:
                        returnValue = "SELECT un_orders.job_ref,job_address " +
                                        " FROM un_orders  " +
                                        "	LEFT OUTER JOIN un_cld_ord_labels ON un_orders.sequence = un_cld_ord_labels.job_sequence  " +
                                        "	LEFT OUTER JOIN un_cld_ord_labels_files ON un_cld_ord_labels.sequence = un_cld_ord_labels_files.join_sequence   " + 
                                " WHERE un_orders.flg_cancelled <> 1 " +
                                " AND un_orders.flg_archive <> 1 " +
                                " AND (un_cld_ord_labels_files.sequence is null OR (un_cld_ord_labels_files.sequence is not null AND un_cld_ord_labels_files.flg_deleted <> 1)) "+
                                " And flg_set_to_jt  = 1 AND flg_job_completed != 1 AND flg_cancelled != 1 ";

                        if (isJobRef)
                            returnValue = returnValue + " AND un_orders.job_ref like '%" + jobRef + "%' ";

                        if (isTag)
                            returnValue = returnValue + " AND un_cld_ord_labels.tag_no LIKE '%" + tagNo + "%'";

                        if (isTagUser)
                            returnValue = returnValue + " AND un_cld_ord_labels_files.created_by = " + tagUser;

                        if (isTagCreatedDate)
                            returnValue = returnValue + " AND un_cld_ord_labels_files.date_file_date >= '" + ((DateTime)tagCreatedDate).ToString("MM/dd/yyyy") + "' AND un_cld_ord_labels_files.date_file_date < '" + ((DateTime)tagCreatedDate).AddDays(1).ToString("MM/dd/yyyy") + "'";

                        returnValue = returnValue + " ORDER by un_orders.sequence desc ";

                        break;
                }

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        internal static string SelectMaxJobRef(string datebaseType)
        {
            string returnValue = "";
            try
            {
                switch (datebaseType)
                {
                    case "MSACCESS":
                        returnValue = "Select MAX(INT(job_ref)) from un_orders where flg_job_ref_is_numeric = true and IsNumeric(job_Ref)=true";
                        break;
                    case "SQLSERVER":
                        returnValue = "Select MAX(CONVERT(numeric, job_ref)) from un_orders where flg_job_ref_is_numeric = 1";
                        break;
                    default:
                        returnValue = "Select MAX(CONVERT(numeric, job_ref)) from un_orders where flg_job_ref_is_numeric = 1";
                        break;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Error occur in getting max job ref query:" + ex.Message + datebaseType);
            }
            return returnValue;
        }

        internal static string SelectNoOrdersClientByYear(string datebaseType, int year)
        {
            return @"SELECT Count(ord.sequence) AS NoOfOrders, edc.name_short as Client 
                                      FROM un_orders AS ord LEFT JOIN un_entity_details_core AS edc 
                                        ON ord.job_client_id = edc.entity_id
                                     GROUP BY edc.name_short, Year(job_date)
                                    HAVING Year(job_date) = " + year + " order by Count(ord.sequence) desc ";
        }

        internal static string SelectInvoiceTotalByClient(string datebaseType)
        {
            return @"SELECT edc.name_short AS Client, Sum(un_orders_bills.amount_sub_total) AS InvoiceTotal
                       FROM un_orders_bills LEFT JOIN un_entity_details_core AS edc ON un_orders_bills.client_id = edc.entity_id
                    GROUP BY edc.name_short order by Sum(un_orders_bills.amount_sub_total) desc ";
        }

        internal static string SelectOrdersByOrderStatus(string databaseType, DateTime fromDate, DateTime toDate)
        {
            //return @"SELECT switch(un_orders.flg_cancelled, 'Cancelled', un_orders.flg_set_to_jt, 'Job Ticket', un_orders.flg_to_client, 'Estimate',sequence>0, 'Enquiry') as Status,Count(un_orders.sequence) AS OrderCount
            //FROM un_orders 
            //Where  un_orders.date_created between #" + ((DateTime)fromDate).ToString("MM/dd/yyyy") + "# and #" + ((DateTime)toDate).ToString("MM/dd/yyyy") +"#"
            //+ @" GROUP BY switch(un_orders.flg_cancelled, 'Cancelled', un_orders.flg_set_to_jt, 'Job Ticket', un_orders.flg_to_client, 'Estimate',sequence>0, 'Enquiry')";
            string returnStmt = "";
            switch (databaseType)
            {
                case "MSACCESS":
                    returnStmt = @"SELECT switch(un_orders.flg_cancelled, 'Cancelled', un_orders.flg_set_to_jt, 'Job Ticket', un_orders.flg_to_client, 'Estimate',sequence>0, 'Enquiry') as Status,Count(un_orders.sequence) AS OrderCount
                    FROM un_orders 
                    Where  un_orders.date_created between " + Utilities.GetDateValueForDML(databaseType, fromDate) + " and " + Utilities.GetDateValueForDML(databaseType, toDate)
                    + @" GROUP BY switch(un_orders.flg_cancelled, 'Cancelled', un_orders.flg_set_to_jt, 'Job Ticket', un_orders.flg_to_client, 'Estimate',sequence>0, 'Enquiry')";
                    break;
                case "SQLSERVER":
                    returnStmt = @"SELECT Case when un_orders.flg_cancelled=1 THEN 'Cancelled'
                                WHEN un_orders.flg_set_to_jt=1 THEN 'Job Ticket'
                                WHEN un_orders.flg_to_client=1 THEN 'Estimate'
                                WHEN sequence>0 THEN 'Enquiry'
                                ELSE 'Other' End as status
                                ,Count(un_orders.sequence) AS OrderCount
                    FROM un_orders 
                    Where  un_orders.date_created between " + Utilities.GetDateValueForDML(databaseType, fromDate) + " and " + Utilities.GetDateValueForDML(databaseType, toDate)
                    + @" GROUP BY Case when un_orders.flg_cancelled=1 THEN 'Cancelled'
                            WHEN un_orders.flg_set_to_jt=1 THEN 'Job Ticket'
                            WHEN un_orders.flg_to_client=1 THEN 'Estimate'
                            WHEN sequence>0 THEN 'Enquiry'
                            ELSE 'Other' End";
                    break;
                default:
                    returnStmt = @"SELECT switch(un_orders.flg_cancelled, 'Cancelled', un_orders.flg_set_to_jt, 'Job Ticket', un_orders.flg_to_client, 'Estimate',sequence>0, 'Enquiry') as Status,Count(un_orders.sequence) AS OrderCount
                    FROM un_orders 
                    Where  un_orders.date_created between " + Utilities.GetDateValueForDML(databaseType, fromDate) + " and " + Utilities.GetDateValueForDML(databaseType, toDate)
                    + @" GROUP BY switch(un_orders.flg_cancelled, 'Cancelled', un_orders.flg_set_to_jt, 'Job Ticket', un_orders.flg_to_client, 'Estimate',sequence>0, 'Enquiry')";
                    break;

            }
            return returnStmt;
        }

        internal static string SelectOrdersCountByOrderType(string databaseType, DateTime fromDate, DateTime toDate)
        {
            return @"SELECT un_ref_order_type.order_type_desc_short as OrderType, Count(un_orders.sequence) AS OrderCount
            FROM un_orders INNER JOIN un_ref_order_type ON un_orders.order_type = un_ref_order_type.order_type_id
            Where  un_orders.date_created between " + Utilities.GetDateValueForDML(databaseType, fromDate) + " and " + Utilities.GetDateValueForDML(databaseType, toDate)
            + @" GROUP BY un_ref_order_type.order_type_desc_short";
        }

        internal static string SelectOrdersCountByJobStatus(string databaseType, DateTime fromDate, DateTime toDate)
        {
            return @"SELECT rjst.status_desc as JobStatus, Count(un_orders.sequence) AS OrderCount
            FROM un_orders LEFT JOIN un_ref_job_status_type as rjst  ON rjst.status_id = un_orders.job_status
            Where  un_orders.date_created between " + Utilities.GetDateValueForDML(databaseType, fromDate) + " and " + Utilities.GetDateValueForDML(databaseType, toDate)
            + @" GROUP BY  rjst.status_desc
            ORDER BY rjst.status_desc";
        }
    }
}
