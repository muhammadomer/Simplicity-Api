using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrdersKpiQueries
    {
        public static string getOutstandingKpiOrderList(string databaseType, ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate)
        {
            string returnValue = "", whereClause = "";
            try
            {
               
                switch (databaseType)
                {
                    case "MSACCESS":
                        whereClause = " and (ord.job_date_due Between #" + ((DateTime)fromDate).ToString("MM/dd/yyyy") + " 00:00:00# " +
                        "   AND  #" + ((DateTime)toDate).ToString("MM/dd/yyyy") + " 23:59:59#)";
                        break;
                    case "SQLSERVER":
                    default:
                        whereClause = " and (ord.job_date_due Between '" + ((DateTime)fromDate).ToString("yyyy-MM-dd") + " 00:00:00'" +
                       "   AND '" + ((DateTime)toDate).ToString("yyyy-MM-dd") + " 23:59:59')";
                        break;
                }
                returnValue = @"SELECT ord.sequence AS job_sequence, ord.job_ref, ord.job_client_id, ord.job_client_name, ord.job_client_ref,
                   ord.job_cost_centre, ord.job_trade_code, ord.job_address, ord.job_priority_code, ord.job_date_due,
                   ord.job_date, ord.flg_job_date_start, ord.job_date_start, ord.flg_job_date_finish, ord.job_date_finish,
                   ord.flg_bill_proforma, ord.date_user2, edc_c.name_long,
                   edc_p.entity_id AS parent_entity_id, edc_p.name_short AS parent_name_short, edc_p.name_long AS parent_name_long,
                   edc_m.name_short AS job_manager, rot.order_type_desc_short
              FROM (((un_orders AS ord 
                LEFT JOIN un_entity_details_core AS edc_c ON ord.job_client_id = edc_c.entity_id)
                LEFT JOIN un_entity_details_core AS edc_p ON edc_c.entity_join_id = edc_p.entity_id)
                LEFT JOIN un_ref_order_type AS rot ON ord.order_type = rot.order_type_id)
                LEFT JOIN un_entity_details_core AS edc_m ON ord.job_manager = edc_m.entity_id
             WHERE ord.flg_cancelled <> " + Utilities.GetBooleanForDML(databaseType,true)
               + " AND ord.flg_job_completed <> " + Utilities.GetBooleanForDML(databaseType, true)
               + " AND ord.job_no_access_ref IS NULL AND ord.flg_set_to_jt = " + Utilities.GetBooleanForDML(databaseType, true)
               + " AND ord.flg_job_sla_timer_stop <> " + Utilities.GetBooleanForDML(databaseType, true)
               + " AND ord.job_priority_code > '' AND ord.job_priority_code <> 'N/A'"
               + whereClause;
                if (clientRequest.globalFilter != "" && clientRequest.globalFilter != null)
                {
                    string filterValue = clientRequest.globalFilter;
                    string globalFilterQuery = " And( ord.job_ref like '%" + filterValue + "%'"
                        + " or ord.job_client_name like '%" + filterValue + "%')";
                    returnValue += globalFilterQuery;
                }
                returnValue += " Order by ord.job_date Desc";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getSuccessKpiOrderList(string databaseType, ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate)
        {
            string returnValue = "", whereClause = "";
            try
            {

                switch (databaseType)
                {
                    case "MSACCESS":
                        whereClause = " and (ord.job_date_due Between #" + ((DateTime)fromDate).ToString("MM/dd/yyyy") + " 00:00:00# " +
                        "   AND  #" + ((DateTime)toDate).ToString("MM/dd/yyyy") + " 23:59:59#)";
                        break;
                    case "SQLSERVER":
                    default:
                        whereClause = " and (ord.job_date_due Between '" + ((DateTime)fromDate).ToString("yyyy-MM-dd") + " 00:00:00'" +
                       "   AND '" + ((DateTime)toDate).ToString("yyyy-MM-dd") + " 23:59:59')";
                        break;
                }
                returnValue = @"SELECT ord.sequence AS job_sequence, ord.job_ref, ord.job_client_id, ord.job_client_name, ord.job_client_ref,
                   ord.job_cost_centre, ord.job_trade_code, ord.job_address, ord.job_priority_code, ord.job_date_due, ord.date_job_sla_timer_stop,
                   ord.job_date, ord.flg_job_date_start, ord.job_date_start, ord.flg_job_date_finish, ord.job_date_finish,
                   ord.flg_bill_proforma, ord.date_user2,
                   edc_c.name_long, edc_p.entity_id AS parent_entity_id, edc_p.name_short AS parent_name_short, edc_p.name_long AS parent_name_long,
                   edc_m.name_short AS job_manager,rot.order_type_desc_short
                FROM (((un_orders AS ord
                    LEFT JOIN un_entity_details_core AS edc_c ON ord.job_client_id = edc_c.entity_id)
                    LEFT JOIN un_entity_details_core AS edc_p ON edc_c.entity_join_id = edc_p.entity_id)
                    LEFT JOIN un_ref_order_type AS rot ON ord.order_type = rot.order_type_id)
                    LEFT JOIN un_entity_details_core AS edc_m ON ord.job_manager = edc_m.entity_id
                WHERE ord.flg_cancelled <> " + Utilities.GetBooleanForDML(databaseType, true) 
                + " AND ord.job_priority_code > '' AND ord.job_priority_code <> 'N/A'"
                +" AND ord.flg_job_sla_timer_stop = " + Utilities.GetBooleanForDML(databaseType, true) 
                + whereClause;
                if (clientRequest.globalFilter != "" && clientRequest.globalFilter != null)
                {
                    string filterValue = clientRequest.globalFilter;
                    string globalFilterQuery = " And( ord.job_ref like '%" + filterValue + "%'"
                        + " or ord.job_client_name like '%" + filterValue + "%')";
                    returnValue += globalFilterQuery;
                }
                returnValue += " Order by ord.job_date Desc";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

    }
}

