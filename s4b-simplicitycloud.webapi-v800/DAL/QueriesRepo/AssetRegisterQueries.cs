using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class AssetRegisterQueries
    { 

        public static string getSelectAllBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
               
                returnValue = "SELECT * " +
                        "  FROM un_asset_register" +
                        " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string search(string databaseType, FilterOption obj)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "SELECT Edc_c.name_short, edc_p.address_full, " +
                                "       Ard.asset_dept_desc, Arc.asset_category_desc, Ar.*, Arst.supp_table_desc " +
                                "  FROM ((((un_asset_register AS Ar LEFT JOIN un_entity_details_core AS Edc_c " +
                                "    ON Ar.entity_id = Edc_c.entity_id) " + 
                                " LEFT JOIN un_asset_register_depts AS Ard ON Ar.item_join_dept = Ard.asset_dept_id) " +
                                "  LEFT JOIN un_asset_register_cats AS Arc  ON Ar.item_join_category = Arc.asset_category_id) " +
                                "  LEFT JOIN un_ref_asset_register_supplementary_tables AS Arst ON Ar.item_join_supplementary = Arst.supp_table_id) " +
                                "  LEFT JOIN un_entity_details_core AS edc_p ON Ar.item_address_id = edc_p.entity_id " +
                                " WHERE Ar.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType,true);
                if (obj.ClientId > 0)
                {
                    returnValue = returnValue + " AND Ar.entity_id = " + obj.ClientId + " ";
                }
                if (obj.DeptId > 0)
                {
                    returnValue = returnValue + " AND Ard.asset_dept_id = " + obj.DeptId + " ";
                }
                if (obj.CateId > 0)
                {
                    returnValue = returnValue + " AND Arc.asset_category_id = " + obj.CateId + " ";
                }
                if (obj.AddressId > 0)
                {
                    returnValue = returnValue + " AND Ar.item_address_id = " + obj.AddressId + " ";
                }
                returnValue = returnValue + " ORDER BY Ar.date_last_amended DESC";
                       
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string getSelectAllAssetsList(string databaseType, ClientRequest clientRequest)
        {
            string returnValue = "";
            try
            {
                string globalFilterQuery = "";
                if (clientRequest.globalFilter != "" && clientRequest.globalFilter != null)
                {
                    string filterValue = clientRequest.globalFilter;
                    globalFilterQuery = " And ( edc.name_short like '%" + filterValue + "%'"
                       + " or uard.asset_dept_desc like '%" + filterValue + "%'"
                       + " or uarc.asset_category_desc like '%" + filterValue + "%')";
                }
                returnValue = @"SELECT uar.sequence, uar.entity_id, edc.name_short, uar.item_join_dept, uard.asset_dept_desc,
                  uar.item_join_category, uarc.asset_category_desc, uar.item_join_supplementary, uar.item_manufacturer,
                  uar.item_model, uar.item_serial_ref, uar.item_extra_info, uar.item_user_field1, uar.item_user_field2, uar.item_quantity, uar.date_acquired,
                  uar.date_disposed, uar.item_value_book, uar.item_value_depreciation, uar.item_value_disposal,
                  uar.item_location_join_id, uar.item_address_id,
                  uar.item_desc, uar.item_address, uar.item_location, urarst.supp_table_desc, uarsp.asset_id AS supp_key,
                  uar.flg_item_chargeable , uar.item_cost_material_rate, uar.item_cost_labour_rate,
                  uar.flg_service, uar.service_start_day, uar.service_start_month, uar.service_renewal
                  FROM ((((un_asset_register AS uar
                  LEFT JOIN un_entity_details_core AS edc ON uar.entity_id = edc.entity_id)
                  LEFT JOIN un_asset_register_depts AS uard ON uar.item_join_dept = uard.asset_dept_id)
                  LEFT JOIN un_asset_register_cats AS uarc  ON uar.item_join_category = uarc.asset_category_id)
                  LEFT JOIN un_ref_asset_register_supplementary_tables AS urarst  ON uar.item_join_supplementary = urarst.supp_table_id)
                  INNER JOIN un_asset_register_supp_plant AS uarsp ON uar.sequence = uarsp.join_sequence
                  WHERE uar.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) + globalFilterQuery;
            returnValue += @" UNION
                  SELECT uar.sequence, uar.entity_id, edc.name_short, uar.item_join_dept, uard.asset_dept_desc,
                         uar.item_join_category, uarc.asset_category_desc, uar.item_join_supplementary, uar.item_manufacturer,
                         uar.item_model, uar.item_serial_ref, uar.item_extra_info, uar.item_user_field1, uar.item_user_field2, uar.item_quantity, uar.date_acquired,
                         uar.date_disposed, uar.item_value_book, uar.item_value_depreciation, uar.item_value_disposal,
                         uar.item_location_join_id, uar.item_address_id,
                         uar.item_desc, uar.item_address, uar.item_location, urarst.supp_table_desc, uarst.asset_id AS supp_key,
                         uar.flg_item_chargeable , uar.item_cost_material_rate, uar.item_cost_labour_rate,
                         uar.flg_service, uar.service_start_day, uar.service_start_month, uar.service_renewal
                    FROM ((((un_asset_register AS uar
                    LEFT JOIN un_entity_details_core AS edc ON uar.entity_id = edc.entity_id)
                    LEFT JOIN un_asset_register_depts AS uard ON uar.item_join_dept = uard.asset_dept_id)
                    LEFT JOIN un_asset_register_cats AS uarc ON uar.item_join_category = uarc.asset_category_id)
                    LEFT JOIN un_ref_asset_register_supplementary_tables AS urarst ON uar.item_join_supplementary = urarst.supp_table_id)
                   INNER JOIN un_asset_register_supp_tools AS uarst ON uar.sequence = uarst.join_sequence
                   WHERE uar.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) + globalFilterQuery
                +" ORDER BY uar.sequence DESC";
                //returnValue = @"SELECT uar.sequence, uar.entity_id, edc.name_short, uar.item_join_dept, uard.asset_dept_desc,
                //        uar.item_join_category, uarc.asset_category_desc, uar.item_join_supplementary, uar.item_manufacturer,
                //        uar.item_model, uar.item_serial_ref, uar.item_extra_info, uar.item_user_field1, uar.item_user_field2, uar.item_quantity, uar.date_acquired,
                //        uar.date_disposed, uar.item_value_book, uar.item_value_depreciation, uar.item_value_disposal,
                //        uar.item_location_join_id, uar.item_address_id,
                //        uar.item_desc, uar.item_address, uar.item_location, urarst.supp_table_desc, uarsv.vehicle_reg AS supp_key,
                //        uar.flg_item_chargeable , uar.item_cost_material_rate, uar.item_cost_labour_rate,
                //        uar.flg_service, uar.service_start_day, uar.service_start_month, uar.service_renewal
                //    FROM ((((un_asset_register AS uar
                //    LEFT JOIN un_entity_details_core AS edc ON uar.entity_id = edc.entity_id)
                //    LEFT JOIN un_asset_register_depts AS uard ON uar.item_join_dept = uard.asset_dept_id)
                //    LEFT JOIN un_asset_register_cats AS uarc ON uar.item_join_category = uarc.asset_category_id)
                //        LEFT JOIN un_ref_asset_register_supplementary_tables AS urarst ON uar.item_join_supplementary = urarst.supp_table_id)
                //    INNER JOIN un_asset_register_supp_vehicles AS uarsv ON uar.sequence = uarsv.join_sequence
                //    WHERE uar.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) + globalFilterQuery ;
                //returnValue += @" UNION
                //    SELECT uar.sequence, uar.entity_id, edc.name_short, uar.item_join_dept, uard.asset_dept_desc,
                //        uar.item_join_category, uarc.asset_category_desc, uar.item_join_supplementary, uar.item_manufacturer,
                //        uar.item_model, uar.item_serial_ref, uar.item_extra_info, uar.item_user_field1, uar.item_user_field2, uar.item_quantity, uar.date_acquired,
                //        uar.date_disposed, uar.item_value_book, uar.item_value_depreciation, uar.item_value_disposal,
                //        uar.item_location_join_id, uar.item_address_id,
                //        uar.item_desc, uar.item_address, uar.item_location, urarst.supp_table_desc, uarsp.asset_id AS supp_key,
                //        uar.flg_item_chargeable , uar.item_cost_material_rate, uar.item_cost_labour_rate,
                //        uar.flg_service, uar.service_start_day, uar.service_start_month, uar.service_renewal
                //    FROM ((((un_asset_register AS uar
                //        LEFT JOIN un_entity_details_core AS edc ON uar.entity_id = edc.entity_id)
                //        LEFT JOIN un_asset_register_depts AS uard ON uar.item_join_dept = uard.asset_dept_id)
                //        LEFT JOIN un_asset_register_cats AS uarc  ON uar.item_join_category = uarc.asset_category_id)
                //        LEFT JOIN un_ref_asset_register_supplementary_tables AS urarst  ON uar.item_join_supplementary = urarst.supp_table_id)
                //        INNER JOIN un_asset_register_supp_plant AS uarsp ON uar.sequence = uarsp.join_sequence
                //    WHERE uar.flg_deleted <> " +Utilities.GetBooleanForDML(databaseType, true) + globalFilterQuery;
                //returnValue += @" UNION
                //    SELECT uar.sequence, uar.entity_id, edc.name_short, uar.item_join_dept, uard.asset_dept_desc,
                //        uar.item_join_category, uarc.asset_category_desc, uar.item_join_supplementary, uar.item_manufacturer,
                //        uar.item_model, uar.item_serial_ref, uar.item_extra_info, uar.item_user_field1, uar.item_user_field2, uar.item_quantity, uar.date_acquired,
                //        uar.date_disposed, uar.item_value_book, uar.item_value_depreciation, uar.item_value_disposal,
                //        uar.item_location_join_id, uar.item_address_id,
                //        uar.item_desc, uar.item_address, uar.item_location, urarst.supp_table_desc, uarst.asset_id AS supp_key,
                //        uar.flg_item_chargeable , uar.item_cost_material_rate, uar.item_cost_labour_rate,
                //        uar.flg_service, uar.service_start_day, uar.service_start_month, uar.service_renewal
                //    FROM ((((un_asset_register AS uar
                //        LEFT JOIN un_entity_details_core AS edc ON uar.entity_id = edc.entity_id)
                //        LEFT JOIN un_asset_register_depts AS uard ON uar.item_join_dept = uard.asset_dept_id)
                //        LEFT JOIN un_asset_register_cats AS uarc ON uar.item_join_category = uarc.asset_category_id)
                //        LEFT JOIN un_ref_asset_register_supplementary_tables AS urarst ON uar.item_join_supplementary = urarst.supp_table_id)
                //        INNER JOIN un_asset_register_supp_tools AS uarst ON uar.sequence = uarst.join_sequence
                //    WHERE uar.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) + globalFilterQuery;
                //returnValue += @" UNION
                //    SELECT uar.sequence, uar.entity_id, edc.name_short, uar.item_join_dept, uard.asset_dept_desc,
                //        uar.item_join_category, uarc.asset_category_desc, uar.item_join_supplementary, uar.item_manufacturer,
                //        uar.item_model, uar.item_serial_ref, uar.item_extra_info, uar.item_user_field1, uar.item_user_field2, uar.item_quantity, uar.date_acquired,
                //        uar.date_disposed, uar.item_value_book, uar.item_value_depreciation, uar.item_value_disposal,
                //        uar.item_location_join_id, uar.item_address_id,
                //        uar.item_desc, uar.item_address, uar.item_location, urarst.supp_table_desc, 'Not Set' AS supp_key,
                //        uar.flg_item_chargeable , uar.item_cost_material_rate, uar.item_cost_labour_rate,
                //        uar.flg_service, uar.service_start_day, uar.service_start_month, uar.service_renewal
                //    FROM (((un_asset_register AS uar
                //        LEFT JOIN un_entity_details_core AS edc  ON uar.entity_id = edc.entity_id)
                //        LEFT JOIN un_asset_register_depts AS uard ON uar.item_join_dept = uard.asset_dept_id)
                //        LEFT JOIN un_asset_register_cats AS uarc  ON uar.item_join_category = uarc.asset_category_id)
                //        LEFT JOIN un_ref_asset_register_supplementary_tables AS urarst ON uar.item_join_supplementary = urarst.supp_table_id
                //    WHERE uar.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType, true) + globalFilterQuery;
                //returnValue = returnValue + " ORDER BY uar.sequence DESC";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string getSelectAllAssets(string databaseType)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT Edc_c.name_short, edc_p.address_full, 
                     Ard.asset_dept_desc, Arc.asset_category_desc, Ar.*, Arst.supp_table_desc 
                     FROM ((((un_asset_register AS Ar 
                        LEFT JOIN un_entity_details_core AS Edc_c ON Ar.entity_id = Edc_c.entity_id) 
                        LEFT JOIN un_asset_register_depts AS Ard ON Ar.item_join_dept = Ard.asset_dept_id) 
                        LEFT JOIN un_asset_register_cats AS Arc ON Ar.item_join_category = Arc.asset_category_id)
                        LEFT JOIN un_ref_asset_register_supplementary_tables AS Arst ON Ar.item_join_supplementary = Arst.supp_table_id) 
                        LEFT JOIN un_entity_details_core AS edc_p  ON Ar.item_address_id = edc_p.entity_id 
                     WHERE Ar.flg_deleted <> " + Utilities.GetBooleanForDML(databaseType,true) +
                  " ORDER BY Ar.date_last_amended DESC";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public static string deleteBySequence(string databaseType, long Sequence)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "DELETE " +
                        "  FROM un_asset_register" +
                        " WHERE sequence = " + Sequence;
               
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

		public static string insert(string databaseType, bool flgDeleted, string transType, long entityId, long itemJoinDept, long itemJoinCategory, long itemJoinSupplementary,
									string itemManufacturer, string itemModel, string itemSerialRef, string itemExtraInfo, string itemUserField1,
									string itemUserField2, string itemUserField3, double itemQuantity, DateTime? dateInstalled, DateTime? dateAcquired,
									DateTime? dateDisposed, double itemValueBook, double itemValueDepreciation, double itemValueDisposal, string itemDesc,
									string itemAddress, bool flgUseAddressId, long itemAddressId, long itemLocationJoinId, string itemLocation,
									bool flgItemChargeable, double itemCostMaterialRate, double itemCostLabourRate, double itemCostAssetRateWeek, double itemCostLabourRateWeek,
									bool flgService, long serviceStartDay,
									long serviceStartMonth, long serviceRenewal, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
		{
			return "INSERT INTO un_asset_register(flg_deleted, trans_type, entity_id, item_join_dept, item_join_category, item_join_supplementary, item_manufacturer," +
																		   "item_model, item_serial_ref, item_extra_info, item_user_field1, item_user_field2, item_user_field3," +
																		   "item_quantity, date_installed, date_acquired, date_disposed, item_value_book, item_value_depreciation," +
																		   "item_value_disposal, item_desc, item_address, flg_use_address_id, item_address_id, item_location_join_id," +
																		   "item_location, flg_item_chargeable, item_cost_asset_rate_day, item_cost_labour_rate_day, flg_service, service_start_day," +
																		   "item_cost_asset_rate_week , item_cost_labour_rate_week," +
																		   "service_start_month, service_renewal, created_by, date_created, last_amended_by, date_last_amended) " +
									 "VALUES (" + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", '" + transType + "', " + entityId + ", " + itemJoinDept + ", " + itemJoinCategory + ", " + itemJoinSupplementary + ", '" +
									  itemManufacturer + "', '" + itemModel + "', '" + itemSerialRef + "', '" + itemExtraInfo + "', '" + itemUserField1 + "', '" + itemUserField2 + "', '" +
									  itemUserField3 + "', '" + itemQuantity + "', " + Utilities.GetDateTimeForDML(databaseType, dateInstalled, true, true) + ", " + Utilities.GetDateTimeForDML(databaseType, dateAcquired, true, true) + ", " +
									  Utilities.GetDateTimeForDML(databaseType, dateDisposed, true, true) + ", " + itemValueBook + ", " + itemValueDepreciation + ", " + itemValueDisposal + ", '" + itemDesc + "', '" +
									  itemAddress + "', " + Utilities.GetBooleanForDML(databaseType, flgUseAddressId) + ", " + itemAddressId + ", " + itemLocationJoinId + ", '" + itemLocation + "', " + Utilities.GetBooleanForDML(databaseType, flgItemChargeable) + ", " +
									  itemCostMaterialRate + ", " + itemCostLabourRate + ", " + Utilities.GetBooleanForDML(databaseType, flgService) + ", " + serviceStartDay + ", " +
									  itemCostAssetRateWeek + "," + itemCostLabourRateWeek + ", " +
									  serviceStartMonth + ", " + serviceRenewal + ", " +
									  createdBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " + lastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
		}

		public static string update(string databaseType, long sequence, bool flgDeleted, string transType, long entityId, long itemJoinDept, long itemJoinCategory, long itemJoinSupplementary,
									string itemManufacturer, string itemModel, string itemSerialRef, string itemExtraInfo, string itemUserField1, string itemUserField2,
									string itemUserField3, string itemQuantity, DateTime? dateInstalled, DateTime? dateAcquired, DateTime? dateDisposed, double itemValueBook,
									double itemValueDepreciation, double itemValueDisposal, string itemDesc, string itemAddress, bool flgUseAddressId, long itemAddressId,
									long itemLocationJoinId, string itemLocation, bool flgItemChargeable, double itemCostMaterialRate, double itemCostLabourRate,
									double itemCostAssetRateWeek, double itemCostLabourRateWeek,
									bool flgService, long serviceStartDay, long serviceStartMonth, long serviceRenewal, long createdBy, DateTime? dateCreated, long lastAmendedBy,
									DateTime? dateLastAmended)
		{
			return "UPDATE un_asset_register" +
				   "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", " +
				   "       trans_type =  '" + transType + "', " +
				   "       entity_id =  " + entityId + ", " +
				   "       item_join_dept =  " + itemJoinDept + ", " +
				   "       item_join_category =  " + itemJoinCategory + ", " +
				   "       item_join_supplementary =  " + itemJoinSupplementary + ", " +
				   "       item_manufacturer =  '" + itemManufacturer + "', " +
				   "       item_model =  '" + itemModel + "', " +
				   "       item_serial_ref =  '" + itemSerialRef + "', " +
				   "       item_extra_info =  '" + itemExtraInfo + "', " +
				   "       item_user_field1 = '" + itemUserField1 + "', " +
				   "       item_user_field2 = '" + itemUserField2 + "', " +
				   "       item_user_field3 = '" + itemUserField3 + "', " +
				   "       item_quantity =  '" + itemQuantity + "', " +
				   "       date_installed =  " + Utilities.GetDateTimeForDML(databaseType, dateInstalled, true, true) + ", " +
				   "       date_acquired =  " + Utilities.GetDateTimeForDML(databaseType, dateAcquired, true, true) + ", " +
				   "       date_disposed =  " + Utilities.GetDateTimeForDML(databaseType, dateDisposed, true, true) + ", " +
				   "       item_value_book = " + itemValueBook + ", " +
				   "       item_value_depreciation = " + itemValueDepreciation + ", " +
				   "       item_value_disposal = '" + itemValueDisposal + ", " +
				   "       item_desc = '" + itemDesc + "', " +
				   "       item_address = '" + itemAddress + "', " +
				   "       flg_use_address_id = " + Utilities.GetBooleanForDML(databaseType, flgUseAddressId) + ", " +
				   "       item_address_id = " + itemAddressId + ", " +
				   "       item_location_join_id = " + itemLocationJoinId + ", " +
				   "       item_location = '" + itemLocation + "', " +
				   "       flg_item_chargeable = " + Utilities.GetBooleanForDML(databaseType, flgItemChargeable) + ", " +
				   "       item_cost_asset_rate_day = " + itemCostMaterialRate + ", " +
				   "       item_cost_labour_rate_day= " + itemCostLabourRate + ", " +
				   "       item_cost_asset_rate_week = " + itemCostAssetRateWeek + "," +
				   "       item_cost_labour_rate_week = " + itemCostLabourRateWeek + "," +
				   "       flg_service = " + Utilities.GetBooleanForDML(databaseType, flgService) + ", " +
				   "       service_start_day= " + serviceStartDay + ", " +
				   "       service_start_month= " + serviceStartMonth + ", " +
				   "       service_renewal = " + serviceRenewal + ", " +
				   "       created_by = " + createdBy + ", " +
				   "       date_created= " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " +
				   "       last_amended_by = " + lastAmendedBy + ", " +
				   "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ", " +
				   "WHERE sequence = " + sequence;
		}


		public static string getAssetDetails(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT daa.de_sequence, da.date_app_start, dr.resource_name, ord.job_ref, ord.job_address " +
                    " FROM((un_diary_apps_assets AS daa " +
                    " INNER JOIN un_diary_apps AS da ON daa.de_sequence = da.sequence) " +
                    " INNER JOIN un_diary_resources AS dr ON da.resource_sequence = dr.sequence) " +
                    " LEFT JOIN un_orders AS ord ON da.job_sequence = ord.sequence " + 
                    " where daa.asset_sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string getAssetRegisterByLocationMakeModelTypeSearialNo(string databaseType, string location, string make, string model, string type, string serialNo)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                            "  FROM un_asset_register " +
                            " WHERE item_location = '" + (location == null ? "" : location + "'") +
                            "   AND item_manufacturer = '" + (make == null ? "" : make + "'") +
                            "   AND item_model = '" + (model == null ? "" : model + "'") +
                            "   AND item_serial_ref = '" + (serialNo == null ? "" : serialNo + "'") +
                            "   AND item_user_field1 = '" + (type == null ? "" : type + "'");
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

    }
}

