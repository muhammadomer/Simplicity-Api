using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrderHireQueries
    {    
        public static string getSelectListOfOrdersHire(string databaseType, ClientRequest clientRequest,DateTime? fromDate, DateTime? toDate,int hireType)
        {
            string returnValue = "", whereClause="";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                        whereClause = " and (OH.date_hire_start Between #" + ((DateTime)fromDate).ToString("MM/dd/yyyy") + " 00:00:00# " +
                        "   AND  #" + ((DateTime)toDate).ToString("MM/dd/yyyy") + " 23:59:59#)";
                        break;
                    case "SQLSERVER":
                    default:
                        whereClause = " and (OH.date_hire_start Between '" + ((DateTime)fromDate).ToString("yyyy-MM-dd") + " 00:00:00'" +
                       "   AND '" + ((DateTime)toDate).ToString("yyyy-MM-dd") + " 23:59:59')";
                        break;
                }
                if (hireType == 2) //Internal
                {
                    returnValue = @"SELECT OH.sequence, OH.job_sequence, Ord.job_ref, Ord.job_address, OH.hire_type,
	                  AR.sequence as asset_Id, AR.item_model, AR.item_join_category, ARCategory.asset_category_desc , AR.item_desc
                     ,'' as supplier, '' as supplier_po_ref
	                  , OH.item_code, OH.item_quantity, OH.date_hire_start, OH.date_hire_end, OH.hire_day_rate
	                  , OH.date_retruned, OH.end_referenece, OH.damage_type,OH.date_damaged,RDT.damage_type_desc
                     ,AR.item_location as location
	                 FROM (((un_ord_hire AS OH INNER JOIN un_orders AS Ord ON OH.job_sequence = Ord.sequence) 
	                     LEFT JOIN un_asset_register AS AR ON Ord.sequence = AR.sequence)
    	                  LEFT JOIN un_asset_register_cats AS ARCategory ON ARCategory.asset_category_id = AR.item_join_category)
                        LEFT JOIN un_ref_ord_hire_damage_types RDT On OH.damage_type = RDT.damage_type_sequence
                    Where OH.hire_type=2 " + whereClause;
                    if (clientRequest.globalFilter != "" && clientRequest.globalFilter != null)
                    {
                        string filterValue = clientRequest.globalFilter;
                        string globalFilterQuery = " And( ord.job_ref like '%" + filterValue + "%'"
                           + " or ARCategory.asset_category_desc like '%" + filterValue + "%')";
                        returnValue += globalFilterQuery;
                    }
                }
                //returnValue += " Union  ";
                if (hireType == 1) //External
                {
                    returnValue += @"SELECT OH.sequence, OH.job_sequence, Ord.job_ref, Ord.job_address, OH.hire_type
                    , null AS asset_Id, null as item_model , null as item_join_category , null as asset_category_desc , null as item_desc
                    , UNC.name_short AS supplier, '' AS supplier_po_ref
                    , OH.item_code, OH.item_quantity, OH.date_hire_start, OH.date_hire_end, OH.hire_day_rate
                    , OH.date_retruned, OH.end_referenece, OH.damage_type,OH.date_damaged,RDT.damage_type_desc
                    , PO.address_delivery as location
                FROM((((un_ord_hire AS OH INNER JOIN un_orders AS Ord ON OH.job_sequence = Ord.sequence)
                    INNER JOIN un_purchase_order_items AS POI ON OH.poi_sequence = POI.sequence) 
                    LEFT JOIN un_purchase_orders AS PO ON POI.order_id = PO.order_id)
                    LEFT JOIN un_entity_details_core UNC on UNC.entity_id = PO.supplier_id)
                    LEFT JOIN un_ref_ord_hire_damage_types RDT On OH.damage_type = RDT.damage_type_sequence
                WHERE OH.hire_type = 1 " + whereClause;

                    if (clientRequest.globalFilter != "" && clientRequest.globalFilter != null)
                    {
                        string filterValue = clientRequest.globalFilter;
                        string globalFilterQuery = " And( ord.job_ref like '%" + filterValue + "%'"
                           + "or UNC.name_short like '%" + filterValue + "%')";
                        returnValue += globalFilterQuery;
                    }
                }
                returnValue += " Order by OH.date_hire_start Desc";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string getSelectOrdersHireBySequence(string databaseType, int sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT OH.sequence, OH.job_sequence, Ord.job_ref, Ord.job_address, OH.hire_type,OH.asset_sequence,OH.poi_sequence
	                 , AR.sequence as asset_Id, AR.item_model, ARCategory.asset_category_desc 
                     ,'' as supplier, '' as supplier_address,'' as supplier_po_ref
	                 , OH.item_code, OH.item_desc, OH.item_quantity,OH.contract_ref,OH.flg_chargeable,OH.rate_type 
                     , OH.date_hire_start, OH.date_hire_end, OH.flg_half_day, OH.number_of_days,OH.number_of_weeks,OH.total_days 
                     , OH.flg_retruned,OH.date_retruned,OH.end_referenece,OH.flg_extend_hire,OH.extend_hire_ref
	                 , OH.flg_damaged,OH.damage_type,OH.date_damaged,RDT.damage_type_desc
                     , OH.hire_day_rate,OH.hire_total,OH.hire_notes,hire_tfr_costs_total
	                FROM (((un_ord_hire AS OH INNER JOIN un_orders AS Ord ON OH.job_sequence = Ord.sequence) 
	                    LEFT JOIN un_asset_register AS AR ON Ord.sequence = AR.sequence)
    	                LEFT JOIN un_asset_register_cats AS ARCategory ON ARCategory.asset_category_id = AR.item_join_category)
                        LEFT JOIN un_ref_ord_hire_damage_types RDT On OH.damage_type = RDT.damage_type_sequence
                Where OH.hire_type=2 And OH.sequence=" + sequence;
                   
                returnValue += " Union  ";
                
                returnValue += @"SELECT OH.sequence, OH.job_sequence, Ord.job_ref, Ord.job_address, OH.hire_type,OH.asset_sequence,OH.poi_sequence
                    , null AS asset_Id, null as item_model , null as asset_category_desc 
                    , UNC.name_short AS supplier, PO.supplier_address , '' AS supplier_po_ref
                    , OH.item_code, OH.item_desc, OH.item_quantity,OH.contract_ref,OH.flg_chargeable,OH.rate_type 
                    , OH.date_hire_start, OH.date_hire_end, OH.flg_half_day, OH.number_of_days,OH.number_of_weeks,OH.total_days 
                    , OH.flg_retruned,OH.date_retruned,OH.end_referenece,OH.flg_extend_hire,OH.extend_hire_ref
	                , OH.flg_damaged,OH.damage_type,OH.date_damaged,RDT.damage_type_desc
                    , OH.hire_day_rate,OH.hire_total,OH.hire_notes,hire_tfr_costs_total
                FROM((((un_ord_hire AS OH INNER JOIN un_orders AS Ord ON OH.job_sequence = Ord.sequence)
                    INNER JOIN un_purchase_order_items AS POI ON OH.poi_sequence = POI.sequence) 
                    LEFT JOIN un_purchase_orders AS PO ON POI.order_id = PO.order_id)
                    LEFT JOIN un_entity_details_core UNC on UNC.entity_id = PO.supplier_id)
                    LEFT JOIN un_ref_ord_hire_damage_types RDT On OH.damage_type = RDT.damage_type_sequence
                WHERE OH.hire_type = 1 And OH.sequence=" + sequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public static string getSelectOrderHireForReportByDate(string databaseType, DateTime? fromDate,DateTime? toDate)
        {
            string returnValue = "",whereClause= " And (date_hire_start >= " + Utilities.GetDateTimeForDML(databaseType,fromDate,true,true) + " And date_hire_end <=" + Utilities.GetDateTimeForDML(databaseType, toDate,true,true) +")"
               + " OR (date_retruned >=" + Utilities.GetDateTimeForDML(databaseType, fromDate, true, true) + " And date_retruned <=" + Utilities.GetDateTimeForDML(databaseType, toDate, true, true) +")";
            try
            {
                returnValue = @"Select Sequence, job_sequence, job_ref, job_address,job_cost_centre
                     , item_code, item_desc, date_hire_start, item_quantity, contract_ref
                     , hire_day_rate, hire_total, hire_notes From (
                     SELECT  OH.sequence, OH.job_sequence, Ord.job_ref, Ord.job_address,ord.job_cost_centre
                       , OH.item_code, OH.item_desc, OH.date_hire_start, OH.item_quantity, OH.contract_ref
                       , OH.hire_day_rate, OH.hire_total, OH.hire_notes
                      FROM((un_ord_hire AS OH INNER JOIN un_orders AS Ord ON OH.job_sequence = Ord.sequence)
                          LEFT JOIN un_asset_register AS AR ON Ord.sequence = AR.sequence)
                            LEFT JOIN un_asset_register_cats AS ARCategory ON ARCategory.asset_category_id = AR.item_join_category
                      Where OH.hire_type = 2 " + whereClause;
                returnValue += @" Union All
                    SELECT OH.Sequence, OH.job_sequence, Ord.job_ref, Ord.job_address,ord.job_cost_centre
                    , OH.item_code, OH.item_desc,OH.date_hire_start, OH.item_quantity,OH.contract_ref
                    , OH.hire_day_rate,OH.hire_total,OH.hire_notes
                    FROM(((un_ord_hire AS OH INNER JOIN un_orders AS Ord ON OH.job_sequence = Ord.sequence)
                        INNER JOIN un_purchase_order_items AS POI ON OH.poi_sequence = POI.sequence) 
                        LEFT JOIN un_purchase_orders AS PO ON POI.order_id = PO.order_id)
                        LEFT JOIN un_entity_details_core UNC on UNC.entity_id = PO.supplier_id
                    WHERE OH.hire_type = 1  " + whereClause 
				    + @") as OH Group By Sequence, job_sequence, job_ref, job_address
	                , OH.item_code, OH.item_desc,date_hire_start,OH.item_quantity,OH.contract_ref
					, OH.hire_day_rate,OH.hire_total,OH.hire_notes,job_cost_centre";

            }
            catch(Exception ex)
            {

            }
            return returnValue;
        }

        public static string getAssetSelectedForDateRange(string databaseType,long assetSequence, DateTime? fromDate, DateTime? toDate)
        {
            string returnValue = "", whereClause = " And (" + Utilities.GetDateTimeForDML(databaseType, fromDate, true, true) + " >= OH.date_hire_start And " + Utilities.GetDateTimeForDML(databaseType, toDate, true, true) + " <= OH.date_hire_end "  ;
            try
            {
                returnValue = @"SELECT  OH.sequence, OH.date_hire_start, OH.date_hire_end
                      FROM (un_ord_hire AS OH INNER JOIN un_asset_register AS AR ON OH.asset_sequence = AR.sequence)
                      Where OH.hire_type = 2 And OH.asset_sequence = " + assetSequence + whereClause;
            }
            catch (Exception ex)
            {

            }
            return returnValue;
        }

        public static string insert(string databaseType, OrderHire orderHire)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO un_ord_hire(job_sequence,flg_deleted, hire_type,asset_sequence,poi_sequence
                    ,item_code,item_desc, item_quantity,contract_ref
                    ,flg_chargeable,rate_type
                    ,date_hire_start,date_hire_end
                    ,flg_half_day,number_of_days,number_of_weeks,total_days
                    ,flg_retruned,date_retruned,end_referenece
                    ,flg_extend_hire,extend_hire_ref
                    ,flg_damaged,date_damaged,damage_type
                    ,hire_day_rate,hire_total,hire_tfr_costs_total,hire_notes
                    ,created_by,  date_created
                    ,last_amended_by,  date_last_amended) 
                VALUES (" + orderHire.JobSequence + "," + Utilities.GetBooleanForDML(databaseType, orderHire.FlgDeleted) + "," + orderHire.HireType + ", " + (orderHire.AssetSequence??0)+"," + orderHire.POISequence
                       + ", '" + orderHire.ItemCode +"', '" + orderHire.ItemDesc +"',"+ orderHire.ItemQuantity +",'"+ orderHire.ContractRef +"'"
                       + "," + Utilities.GetBooleanForDML(databaseType, orderHire.FlgChargeable) +"," + orderHire.RateType
                       + ", " + Utilities.GetDateTimeForDML(databaseType, orderHire.DateHireStart, true, true) + ", " + Utilities.GetDateTimeForDML(databaseType, orderHire.DateHireEnd, true, true)
                       + "," + Utilities.GetBooleanForDML(databaseType, orderHire.FlgHalfDay) +"," + orderHire.NumberOfDays +"," + orderHire.NumberOfWeeks +"," + orderHire.TotalDays
                       + "," + Utilities.GetBooleanForDML(databaseType, orderHire.FlgRretruned) + "," + Utilities.GetDateTimeForDML(databaseType, orderHire.DateReturned, true, true) +",'" + orderHire.EndRreferenece +"'"
                       + "," + Utilities.GetBooleanForDML(databaseType, orderHire.FlgEextendHire) + ",'" + orderHire.ExtendHireRef +"'"
                       + "," + Utilities.GetBooleanForDML(databaseType, orderHire.FlgDamaged) + "," + Utilities.GetDateTimeForDML(databaseType, orderHire.DateDamaged, true, true) + "," + orderHire.DamageType
                       + ", " + orderHire.HireDayRate + ", " + orderHire.HireTotal + "," + orderHire.HireTfrCostsTotal + ", '" + orderHire.HireNotes +"'"
                       + ", " + orderHire.CreatedBy + ", " + Utilities.GetDateTimeForDML(databaseType, orderHire.DateCreated, true, true)
                       + ", " + orderHire.LastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, orderHire.DateLastAmended, true, true) 
                  + ")";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public static string update(string databaseType, OrderHire orderHire)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE  un_ord_hire SET" +
                                 "   job_sequence =  " + orderHire.JobSequence
                                 + ",  flg_deleted = " + Utilities.GetBooleanForDML(databaseType, orderHire.FlgDeleted) 
                                 + "  ,  hire_type =  " + orderHire.HireType 
                                 + "  ,  asset_sequence =  " + orderHire.AssetSequence 
                                 + "  ,  poi_sequence = " + orderHire.POISequence
                                 + "  ,  item_code =  '" + orderHire.ItemCode + "'" 
                                 + "  ,  item_desc =  '" + orderHire.ItemDesc +"'"
                                 + "  ,  item_quantity =  " + orderHire.ItemQuantity 
                                 + "  ,  contract_ref =  '" + orderHire.ContractRef +"'" 
                                 + "  ,  flg_chargeable =  " + Utilities.GetBooleanForDML(databaseType, orderHire.FlgChargeable)
                                 + "  ,  rate_type =  " + orderHire.RateType 
                                 + "  ,  date_hire_start =  " + Utilities.GetDateValueForDML(databaseType, orderHire.DateHireStart)
                                 + "  ,  date_hire_end =  " + Utilities.GetDateValueForDML(databaseType, orderHire.DateHireEnd)
                                 + "  ,  flg_half_day =  " + Utilities.GetBooleanForDML(databaseType, orderHire.FlgHalfDay)
                                 + "  ,  number_of_days =  " + orderHire.NumberOfDays 
                                 + "  ,  number_of_weeks = " +orderHire.NumberOfWeeks
                                 + "  ,  total_days = " + orderHire.TotalDays
                                 + "  ,  flg_retruned =  " + Utilities.GetBooleanForDML(databaseType, orderHire.FlgRretruned)
                                 + "  ,  date_retruned = " + Utilities.GetDateTimeForDML(databaseType, orderHire.DateReturned,true,true)
                                 + "  ,  end_referenece = '" +  orderHire.EndRreferenece +"'"
                                 + "  ,  flg_extend_hire = " + Utilities.GetBooleanForDML(databaseType, orderHire.FlgEextendHire)
                                 + "  ,  extend_hire_ref =  '" + orderHire.ExtendHireRef +"'"
                                 + "  ,  flg_damaged = " + Utilities.GetBooleanForDML(databaseType, orderHire.FlgDamaged) 
                                 + "  ,  date_damaged =  " + Utilities.GetDateTimeForDML(databaseType, orderHire.DateDamaged,true,false) 
                                 + "  ,   damage_type = " + orderHire.DamageType
                                 + "  ,   hire_day_rate =  " + orderHire.HireDayRate
                                 + "  ,   hire_total =  " + orderHire.HireTotal 
                                 + "  ,   hire_tfr_costs_total = " + orderHire.HireTfrCostsTotal
                                 + "  ,   hire_notes = '" + orderHire.HireNotes +"'"
                                 + "  ,   last_amended_by =  " + orderHire.LastAmendedBy 
                                 + "  ,   date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, orderHire.DateLastAmended, true, true) +
                                 "  WHERE sequence = " + orderHire.Sequence;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }




    }
}

