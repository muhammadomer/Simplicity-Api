using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class OrderMeSchItemsQueries
    {

       
      public static string insert(string databaseType, OrdersMeSchItems oi)
        {
            return @"INSERT INTO un_orders_me_sch_items(join_sequence,job_sequence,cloned_from_seq ,row_index
           ,flg_row_locked,flg_row_selected,flg_row_is_text,item_type,asset_sequence,flg_row_is_subtotal
           ,group_id,trans_type,item_code,item_desc,item_units ,item_quantity
           ,chg_pcent_labour,amount_labour ,chg_pcent_materials,amount_materials ,chg_pcent_plant,amount_plant
           ,adj_code,chg_pcent_adj,priority_code,chg_pcent_priority,chg_pcent_value
		   ,amount_value,amount_total,amount_balance
           ,item_vam,assigned_to ,vam_cost_sequence,vam_cost_rate
           ,product_outs_dia ,product_d_or_w,product_weight ,item_due_date,flg_completed,flg_docs_recd
           ,supplier_id,meoi_section_sequence
           ,created_by,date_created ,last_amended_by,date_last_amended)
           VALUES(" +  oi.joinSequence + "," + oi.JobSequence + "," +  oi.ClonedFromSeq +","+  oi.RowIndex 
			+ "," + Utilities.GetBooleanForDML(databaseType, oi.FlgRowLocked)
			+ "," + Utilities.GetBooleanForDML(databaseType, oi.FlgRowSelected) + "," + Utilities.GetBooleanForDML(databaseType, oi.FlgRowIsText)
			+ "," + oi.ItemType + "," + oi.AssetSequence + "," + Utilities.GetBooleanForDML(databaseType, oi.FlgRowIsSubtotal) 
			+ "," + oi.GroupId + ",'" +  oi.TransType + "'," + "'"+ oi.ItemCode + "','" + oi.ItemDesc + "','" + oi.ItemUnits + "'," + oi.ItemQuantity 
			+ "," + oi.ChgPcentLabour + "," + oi.AmountLabour + "," + oi.ChgPcentMaterials + "," + oi.AmountMaterials + "," + oi.ChgPcentPlant + "," + oi.AmountPlant
			+ "," + "'"+ oi.AdjCode + "'," + oi.ChgPcentAdj + ",'" + oi.PriorityCode + "'," + oi.ChgPcentPriority + "," + oi.ChgPcentValue 
			+ "," + oi.AmountValue + "," + "," + oi.AmountTotal + "," + oi.AmountBalance 
			+ ","+ Utilities.GetDateTimeForDML(databaseType, oi.ItemVam,true,false) +"," + oi.AssignedTo + "," + oi.VamCostSequence + "," + oi.VamCostRate 
			+ "," + oi.ProductOutsDia + "," + oi.ProductdOrw + "," + oi.ProductWeight 
			+ "," + Utilities.GetDateTimeForDML(databaseType, oi.ItemDueDate, true, false)
			+ "," + Utilities.GetBooleanForDML(databaseType, oi.FlgCompleted) + "," + Utilities.GetBooleanForDML(databaseType, oi.FlgDocsRecd) 
            + ","  + oi.SupplierId + "," + oi.MeoiSectionSequence 
			+ ","  +  oi.CreatedBy + "," + Utilities.GetDateTimeForDML(databaseType, oi.DateCreated, true, true)
			+ "," + oi.LastAmendedBy + "," + Utilities.GetDateTimeForDML(databaseType, oi.DateLastAmended, true, true) + ")";
        }

        public static string update(string databaseType, OrdersMeSchItems OrderItem)
        {
            string returnValue = "";
            try
            {
                switch (databaseType)
                {
                    case "MSACCESS":
                    case "SQLSERVER":
                    default:
                        returnValue = "UPDATE   un_orders_me_sch_items" +
                            "   SET  job_sequence =  " + OrderItem.JobSequence + ",  " +
							"        join_sequence = " + OrderItem.joinSequence +", " +
							"        cloned_from_seq =" + OrderItem.ClonedFromSeq +", " +
							"        row_index =  " + OrderItem.RowIndex + ",  " +
                            "        flg_row_locked =  " + Utilities.GetBooleanForDML(databaseType ,OrderItem.FlgRowLocked) + ",  " +
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
                            "        amount_total =  " + OrderItem.AmountTotal + ",  " +
                            "        amount_balance =  " + OrderItem.AmountBalance + ",  " +
                            "        assigned_to =  " + OrderItem.AssignedTo + ",  " +
                            "        vam_cost_sequence =  " + OrderItem.VamCostSequence + ",  " +
                            "        vam_cost_rate =  " + OrderItem.VamCostRate + ",  " +
                            "        product_outs_dia =  " + OrderItem.ProductOutsDia + ",  " +
                            "        product_d_or_w =  " + OrderItem.ProductdOrw + ",  " +
                            "        product_weight =  " + OrderItem.ProductWeight + ",  " +
                            "        item_due_date =  " + Utilities.GetDateTimeForDML(databaseType, OrderItem.ItemDueDate,true,false) + ",  " +
                            "        flg_completed =  " + Utilities.GetBooleanForDML(databaseType, OrderItem.FlgCompleted) + ",  " +
                            "        flg_docs_recd =  " + Utilities.GetBooleanForDML(databaseType, OrderItem.FlgDocsRecd) + ",  " +
                            "        supplier_id =  " + OrderItem.SupplierId + ",  " +
							"        meoi_section_sequence =  " + OrderItem.MeoiSectionSequence + ",  " +
                            "        last_amended_by =  " + OrderItem.LastAmendedBy + ", " +
                            "        date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, OrderItem.DateLastAmended,true,true) + 
                            "  WHERE sequence = " + OrderItem.Sequence;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


       
    }
}

