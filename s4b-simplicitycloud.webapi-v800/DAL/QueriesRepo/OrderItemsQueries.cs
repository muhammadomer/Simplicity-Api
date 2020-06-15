using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrderItemsQueries
    {
      public static string getSelectAllBySequence(string databaseType, int Sequence)
      {
         string returnValue = "";
         try
         {
            switch (databaseType)
            {
               case "MSACCESS":
               case "SQLSERVER":
               default:
                  returnValue = "SELECT edc.name_long, oi.*, ord.job_ref " +
                             "  FROM(un_order_items AS oi " +
                             " INNER JOIN un_orders AS ord " +
                             "    ON oi.job_sequence = ord.sequence) " +
                             "  LEFT JOIN un_entity_details_core AS edc " +
                             "    ON oi.supplier_id = edc.entity_id " +
                             " WHERE ord.sequence = " + Sequence;
                  break;

            }

         }
         catch (Exception ex)
         {
         }
         return returnValue;
      }

		public static string getSelectItemDescByCode(string databaseType, string itemCode)
		{
			string returnValue = "";
			try
			{
				switch (databaseType)
				{
					case "MSACCESS":
					case "SQLSERVER":
					default:
						returnValue = @"Select Top 1 item_desc from un_order_items 
						where item_code='" + itemCode + "'"
						+ " order by sequence desc";
						break;
				}

			}
			catch (Exception ex)
			{
			}
			return returnValue;
		}
		public static string selectOrderItemsByJobSequence(string databaseType, ClientRequest clientRequest,int jobSequence)
        {
            string returnValue = "";
            try
            {  
               
                        returnValue = @"SELECT  oi.*,'' as assigned_to_name
                                   FROM un_order_items AS oi 
                                   WHERE oi.job_sequence = " + jobSequence;
                if (clientRequest.globalFilter != null && clientRequest.globalFilter != "")
                {
                    string filterValue = clientRequest.globalFilter;
                    string globalFilterQuery = "";
                    //string globalFilterQuery = " and ord.job_ref like '%" + filterValue + "%' or ord.job_client_name like '%"
                    //    + filterValue + "%' or rjst.status_desc like '%" + filterValue + "%' or rtct.trade_desc like '%"
                    //    + filterValue + "%' or ord.job_client_ref like '%" + filterValue + "%' or edc_p.address_post_code like '%" + filterValue + "%' ";
                    returnValue += globalFilterQuery;
                }
                string sortColumn = "oi.row_index"; // default sort
                string orderType = "DESC";
                if (clientRequest.sortOrder == -1)
                {
                    orderType = "ASC";
                }
                else
                {
                    orderType = "Desc";
                }
                returnValue += " ORDER BY " + sortColumn + " " + orderType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string selectOrderItemsBySequence(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = @"SELECT  oi.*,'' as assigned_to_name
                                   FROM un_order_items AS oi 
                                   WHERE oi.Sequence = " + sequence;
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

      public static string getSelectNTBySequence(string databaseType, int Sequence)
      {
         string returnValue = "";
         try
         {
            switch (databaseType)
            {
               case "MSACCESS":
               case "SQLSERVER":
               default:
                  returnValue = "SELECT edc.name_long, oi.*, ord.job_ref " +
                             "  FROM(un_order_items AS oi " +
                             " INNER JOIN un_orders AS ord " +
                             "    ON oi.job_sequence = ord.sequence) " +
                             "  LEFT JOIN un_entity_details_core AS edc " +
                             "    ON oi.supplier_id = edc.entity_id " +
                             " WHERE ord.sequence = " + Sequence + " and flg_row_is_text = " + Utilities.GetBooleanForDML(databaseType, false); ;
                  break;

            }

         }
         catch (Exception ex)
         {
         }
         return returnValue;
      }

      public static string insert(string databaseType,OrderItems oi)
        {
            return @"INSERT INTO un_order_items(job_sequence, row_index, flg_row_locked,flg_row_selected, flg_row_is_text, asset_sequence, 
                            flg_row_is_subtotal,  group_id, trans_type, item_code, item_desc, item_units, item_quantity,  chg_pcent_labour,  
                            amount_labour,  chg_pcent_materials, amount_materials, chg_pcent_plant, amount_plant, adj_code, chg_pcent_adj,  
                            priority_code,  chg_pcent_priority,  chg_pcent_value, amount_value, amt_qty_subtotal, amt_tot_labour, amt_tot_materials,  
                            amt_tot_plant, amt_tot_subcon, amt_tot_sums, amt_tot_lab_only, amt_tot_prelims, amount_total, amount_balance, 
                            assigned_to, vam_cost_sequence, vam_cost_rate, product_outs_dia, product_d_or_w, product_weight, item_due_date, 
                            flg_completed,flg_docs_recd,rci_dd_join, grp_ord_ti, me_oi_sequence,supplier_id, rci_id,rci_oi_sequence, 
                            rci_inv_no,relationship_type, oi_section_sequence, created_by, date_created, last_amended_by, date_last_amended)
                     VALUES("+ oi.JobSequence
                     + "," +  oi.RowIndex 
                     + ","  + Utilities.GetBooleanForDML(databaseType, oi.FlgRowLocked) 
                     +"," + Utilities.GetBooleanForDML(databaseType, oi.FlgRowSelected) 
                     + "," + Utilities.GetBooleanForDML(databaseType, oi.FlgRowIsText) 
                     + "," +  (oi.AssetSequence == null ? 0 : oi.AssetSequence)
                     + "," + Utilities.GetBooleanForDML(databaseType, oi.FlgRowIsSubtotal) 
                     + "," + oi.GroupId 
                     + ",'" +  oi.TransType + "'"
                     +"," + "'"+ oi.ItemCode + "'"
                     +",'" + oi.ItemDesc + "'"
                     +",'" + oi.ItemUnits + "'"
                     +"," + oi.ItemQuantity 
                     + "," + oi.ChgPcentLabour 
                     + "," + oi.AmountLabour 
                     + "," + oi.ChgPcentMaterials 
                     + "," + oi.AmountMaterials 
                     + "," +  oi.ChgPcentPlant 
                     + "," + oi.AmountPlant 
                     + "," + "'"+ oi.AdjCode + "'"
                     +"," + oi.ChgPcentAdj 
                     + ",'" + oi.PriorityCode + "'"
                    + "," +  oi.ChgPcentPriority 
                    + ","+ oi.ChgPcentValue 
                    + "," + oi.AmountValue 
                    + "," + oi.AmtQtySubtotal 
                    + "," + oi.AmtTotLabour 
                    + "," + oi.AmtTotMaterials 
                    + ","+ oi.AmtTotPlant 
                    + "," + oi.AmtTotSubcon 
                    + "," + oi.AmtTotSums 
                    + "," + oi.AmtTotLabOnly 
                    + "," + oi.AmtTotPrelims 
                    + "," + oi.AmountTotal 
                    + "," + oi.AmountBalance 
                    + "," + oi.AssignedTo 
                    + "," + oi.VamCostSequence 
                    + "," + oi.VamCostRate 
                    + "," + oi.ProductOutsDia 
                    + "," + oi.ProductdOrw 
                    + "," + oi.ProductWeight 
                    + "," + Utilities.GetDateTimeForDML(databaseType, oi.ItemDueDate, true, false) 
                    + "," + Utilities.GetBooleanForDML(databaseType, oi.FlgCompleted) 
                    + "," + Utilities.GetBooleanForDML(databaseType, oi.FlgDocsRecd) 
                    + ",'" + oi.RciDdJoin + "'" 
                    + "," + oi.GrpOrdTi 
                    + "," + oi.MeOiSequence 
                    + ","  + oi.SupplierId 
                    + "," + oi.RciId 
                    + "," + oi.RciOiSequence 
                    + ",'" + oi.RciInvNo + "'" 
                    + "," + oi.RelationshipType 
                    + "," + oi.OiSectionSequence 
                    + ","  + oi.CreatedBy 
                    + "," + Utilities.GetDateTimeForDML(databaseType, oi.DateCreated, true, true) 
                    + "," + oi.LastAmendedBy 
                    + "," + Utilities.GetDateTimeForDML(databaseType, oi.DateLastAmended, true, true) 
                    + ")";
        }

        public static string update(string databaseType, OrderItems OrderItem)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = "UPDATE   un_order_items" +
                            "   SET  job_sequence =  " + OrderItem.JobSequence + ",  " +
                            "        row_index =  " + OrderItem.RowIndex + ",  " +
                            "        flg_row_locked =  " + Utilities.GetBooleanForDML(databaseType, OrderItem.FlgRowLocked )+ ",  " +
                            "        flg_row_selected =  " + Utilities.GetBooleanForDML(databaseType, OrderItem.FlgRowSelected) + ",  " +
                            "        flg_row_is_text =  " + Utilities.GetBooleanForDML(databaseType, OrderItem.FlgRowIsText) + ",  " +
                            "        item_type =  " + OrderItem.ItemType + ",  " +
                            "        asset_sequence =  " + OrderItem.AssetSequence + ",  " +
                            "        flg_row_is_subtotal =  " + Utilities.GetBooleanForDML(databaseType, OrderItem.FlgRowIsSubtotal) + ",  " +
                            "        group_id =  " + OrderItem.GroupId + ",  " +
                            "        trans_type =  '" + OrderItem.TransType + "',  " +
                            "        item_code  =  '" + OrderItem.ItemCode + "',  " +
                            "        item_desc =  '" + OrderItem.ItemDesc + "',  " +
                            "        item_units =  '" + OrderItem.ItemUnits + "',  " +
                            "        item_quantity =  " + OrderItem.ItemQuantity + ",  " +
                            "        chg_pcent_labour =  " + OrderItem.ChgPcentLabour + ",  " +
                            "        amount_labour =  " + OrderItem.AmountLabour + ",  " +
                            "        chg_pcent_materials =  " + OrderItem.ChgPcentMaterials + ",  " +
                            "        amount_materials =  " + OrderItem.AmountMaterials + ",  " +
                            "        chg_pcent_plant =  " + OrderItem.ChgPcentPlant + ",  " +
                            "        amount_plant =  " + OrderItem.AmountPlant + ",  " +
                            "        adj_code =  '" + OrderItem.AdjCode + "',  " +
                            "        chg_pcent_adj =  " + OrderItem.ChgPcentAdj + ",  " +
                            "        priority_code =  '" + OrderItem.PriorityCode + "',  " +
                            "        chg_pcent_priority =  " + OrderItem.ChgPcentPriority + ",  " +
                            "        chg_pcent_value =  " + OrderItem.ChgPcentValue + ",  " +
                            "        amount_value =  " + OrderItem.AmountValue + ",  " +
                            "        amt_qty_subtotal =  " + OrderItem.AmtQtySubtotal + ",  " +
                            "        amt_tot_labour =  " + OrderItem.AmtTotLabour + ",  " +
                            "        amt_tot_materials =  " + OrderItem.AmtTotMaterials + ",  " +
                            "        amt_tot_plant =  " + OrderItem.AmtTotPlant + ",  " +
                            "        amt_tot_subcon =  " + OrderItem.AmtTotSubcon + ",  " +
                            "        amt_tot_sums =  " + OrderItem.AmtTotSums + ",  " +
                            "        amt_tot_lab_only =  " + OrderItem.AmtTotLabOnly + ",  " +
                            "        amt_tot_prelims =  " + OrderItem.AmtTotPrelims + ",  " +
                            "        amount_total =  " + OrderItem.AmountTotal + ",  " +
                            "        amount_balance =  " + OrderItem.AmountBalance + ",  " +
                            //"        item_vam =  " + OrderItem.ItemVam + ",  " +
                            "        assigned_to =  " + OrderItem.AssignedTo + ",  " +
                            "        vam_cost_sequence =  " + OrderItem.VamCostSequence + ",  " +
                            "        vam_cost_rate =  " + OrderItem.VamCostRate + ",  " +
                            "        product_outs_dia =  " + OrderItem.ProductOutsDia + ",  " +
                            "        product_d_or_w =  " + OrderItem.ProductdOrw + ",  " +
                            "        product_weight =  " + OrderItem.ProductWeight + ",  " +
                            //"        item_due_date =  '" + OrderItem.ItemDueDate + "',  " +
                            "        flg_completed =  " + Utilities.GetBooleanForDML(databaseType, OrderItem.FlgCompleted) + ",  " +
                            "        flg_docs_recd =  " + Utilities.GetBooleanForDML(databaseType, OrderItem.FlgDocsRecd) + ",  " +
                            "        grp_ord_ti =  " + OrderItem.GrpOrdTi + ",  " +
                            "        me_oi_sequence =  " + OrderItem.MeOiSequence + ",  " +
                            "        supplier_id =  " + OrderItem.SupplierId + ",  " +
                            "        rci_id =  " + OrderItem.RciId + ",  " +
                            "        rci_oi_sequence =  " + OrderItem.RciOiSequence + ",  " +
                            "        rci_inv_no =  '" + OrderItem.RciInvNo + "',  " +
                            "        rci_dd_join =  '" + OrderItem.RciDdJoin + "',  " +
                            "        relationship_type =  " + OrderItem.RelationshipType + ",  " +
                            "        oi_section_sequence =  " + OrderItem.OiSectionSequence + ",  " +
                            "        last_amended_by =  " + OrderItem.LastAmendedBy +
                            "  WHERE sequence = " + OrderItem.Sequence;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }


        public static string delete(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                        returnValue = @"DELETE FROM   un_order_items
                              WHERE sequence = " + sequence;
                        break;
                    case "SQLSERVER":
                        returnValue = @"DELETE FROM   un_order_items
                              WHERE sequence = " + sequence;
                        break;
                    default:
                        returnValue = @"DELETE FROM   un_order_items
                              WHERE sequence = " + sequence;
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }


        //    public static string deleteFlagDeleted(string databaseType, long sequence)
        //    {
        //        string returnValue = "";
        //        try
        //        {
        //            bool flg = true;
        //            switch (databaseType)
        //            {
        //                case "MSACCESS":
        //                    returnValue = "UPDATE   un_order_items" +
        //                         "   SET flg_deleted =  " + flg + ", " +
        //                         "WHERE sequence = " + sequence;
        //                    break;
        //                case "SQLSERVER":
        //                    returnValue = "UPDATE   un_order_items" +
        //                         "   SET flg_deleted =  " + flg + ", " +
        //                         "WHERE sequence = " + sequence;
        //                    break;
        //                default:
        //                    returnValue = "UPDATE   un_order_items" +
        //                         "   SET flg_deleted =  " + flg + ", " +
        //                         "WHERE sequence = " + sequence;
        //                    break;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //        return returnValue;
        //    }
    }
}

