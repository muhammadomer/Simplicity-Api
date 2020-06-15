using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;

namespace SimplicityOnlineWebApi.DAL
{
    public class OrderItemsDB : MainDB
    {

        public OrderItemsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public OrderItems InsertOrderItem(OrderItems Object)
        {
            OrderItems returnValue = new OrderItems();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(OrderItemsQueries.insert(this.DatabaseType, Object), conn))
                    {
                        int result = objCmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            long sequence = Utilities.GetDBAutoNumber(conn);
                            Object.Sequence = sequence;
                            returnValue = Object;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public OrderItems UpdateOrderItem(OrderItems Object)
        {
            OrderItems returnValue = new OrderItems();
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(OrderItemsQueries.update(this.DatabaseType, Object), conn))
                    {
                       int result = objCmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            returnValue = Object;
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        
        public bool DeleteOrderItem(long sequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmd =
                        new OleDbCommand(OrderItemsQueries.delete(this.DatabaseType, sequence), conn))
                    {
                        int result = objCmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            returnValue = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

       public string selectItemDescByItemCode(string itemCode)
		{
			string returnValue = "";
			try
			{
				using (OleDbConnection conn = this.getDbConnection())
				{
					using (OleDbCommand objCmdSelect =
						new OleDbCommand(OrderItemsQueries.getSelectItemDescByCode(this.DatabaseType, itemCode), conn))
					{
						OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);

						DataTable dt = new DataTable();
						da.Fill(dt);
						if (dt.Rows != null && dt.Rows.Count > 0)
						{
							returnValue = dt.Rows[0][0].ToString();
						}
						else
						{
							ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
						}
					}
				}

			}
			catch(Exception ex)
			{

			}
			return returnValue;
		}

        public List<OrderItems> selectOrderItemsByJobSequence(ClientRequest clientRequest,int jobSequence,out int count, bool isCountRequired)
        {
            List<OrderItems> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderItemsQueries.selectOrderItemsByJobSequence(this.DatabaseType, clientRequest,jobSequence), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                        if (isCountRequired)
                        {
                            count = da.Fill(new DataSet("temp"));
                        }
                        DataTable dt = new DataTable();
                        da.Fill(clientRequest.first, clientRequest.rows, dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new List<OrderItems>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(Load_OrderItems(row));
                            }
                        }
                        else
                        {
                            ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public OrderItems selectOrderItemsBySequence(long sequence)
        {
            OrderItems returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(OrderItemsQueries.selectOrderItemsBySequence(this.DatabaseType, sequence), conn))
                    {
                        OleDbDataAdapter da = new OleDbDataAdapter(objCmdSelect);
                       
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows != null && dt.Rows.Count > 0)
                        {
                            returnValue = new OrderItems();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue = Load_OrderItems(row);
                            }
                        }
                        else
                        {
                            ErrorMessage = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        private OrderItems Load_OrderItems(DataRow row)
        {
            OrderItems OrderItems = null;
            try
            {
                if (row != null)
                {
                    OrderItems = new OrderItems();
                    OrderItems.Sequence = Convert.ToInt32(row["sequence"].ToString());
                    OrderItems.JobSequence = Convert.ToInt32(row["job_sequence"].ToString());
                    OrderItems.RowIndex = Convert.ToInt32(row["row_index"].ToString());
                    OrderItems.ItemType = Convert.ToInt32(row["item_type"].ToString());
                    OrderItems.ItemCode = Utilities.GetDBString(row["item_code"]);
                    OrderItems.ItemDesc = Utilities.GetDBString(row["item_desc"]);
                    OrderItems.ItemUnits = Utilities.GetDBString(row["item_units"]);
                    OrderItems.ItemQuantity = Convert.ToDouble(row["item_quantity"].ToString());
                    OrderItems.FlgRowLocked = bool.Parse(row["flg_row_locked"].ToString());
                    OrderItems.FlgRowSelected = bool.Parse(row["flg_row_selected"].ToString());
                    OrderItems.FlgRowIsText = bool.Parse(row["flg_row_is_text"].ToString());
                    OrderItems.FlgRowIsSubtotal = bool.Parse(row["flg_row_is_subtotal"].ToString());
                    OrderItems.GroupId = Convert.ToInt32(row["group_id"].ToString());
                    OrderItems.TransType = Utilities.GetDBString(row["trans_type"]);
                    OrderItems.ChgPcentLabour = double.Parse(row["chg_pcent_labour"].ToString());
                    OrderItems.AmountLabour = double.Parse(row["amount_labour"].ToString());
                    OrderItems.ChgPcentMaterials = double.Parse(row["chg_pcent_materials"].ToString());
                    OrderItems.AmountMaterials = double.Parse(row["amount_materials"].ToString());
                    OrderItems.ChgPcentPlant = double.Parse(row["chg_pcent_plant"].ToString());
                    OrderItems.AmountPlant = double.Parse(row["amount_plant"].ToString());
                    OrderItems.AdjCode = Utilities.GetDBString(row["adj_code"]);
                    OrderItems.ChgPcentAdj = double.Parse(row["chg_pcent_adj"].ToString());
                    if (OrderItems.AdjCode != "" && OrderItems.AdjCode!="0")
                        OrderItems.ChgPcentAdjCode = OrderItems.AdjCode + "-" + string.Format("{0:0.00}", (OrderItems.ChgPcentAdj * 100)) + "%";
                    else
                        OrderItems.ChgPcentAdjCode = "N/A - 0%";
                    OrderItems.ChgPcentValue = double.Parse(row["chg_pcent_value"].ToString());
                    OrderItems.AmountValue = double.Parse(row["amount_value"].ToString());
                    OrderItems.AmtTotLabour = double.Parse(row["amt_tot_labour"].ToString());
                    OrderItems.AmtTotMaterials = double.Parse(row["amt_tot_materials"].ToString());
                    OrderItems.AmtTotPlant = double.Parse(row["amt_tot_plant"].ToString());
                    OrderItems.AmtTotSums = double.Parse(row["amt_tot_sums"].ToString());
                    OrderItems.AmtTotLabOnly = double.Parse(row["amt_tot_lab_only"].ToString());
                    OrderItems.AmtTotPrelims = double.Parse(row["amt_tot_prelims"].ToString());
                    OrderItems.AmountTotal = double.Parse(row["amount_total"].ToString());
                    OrderItems.AmountBalance = double.Parse(row["amount_balance"].ToString());
                    OrderItems.ItemVam = Convert.ToDateTime(row["item_vam"].ToString());
                    OrderItems.AssignedTo = Convert.ToInt32(row["assigned_to"].ToString());
                    OrderItems.AssignedToName = DBUtil.GetStringValue(row,"assigned_to_name");
                    OrderItems.VamCostSequence = Convert.ToInt32(row["vam_cost_sequence"].ToString());
                    OrderItems.VamCostRate = double.Parse(row["vam_cost_rate"].ToString());
                    OrderItems.ProductOutsDia = Convert.ToInt32(row["product_outs_dia"].ToString());
                    OrderItems.ProductdOrw = Convert.ToInt32(row["product_d_or_w"].ToString());
                    OrderItems.ProductWeight = double.Parse(row["product_weight"].ToString());
                    OrderItems.FlgCompleted = bool.Parse(row["flg_completed"].ToString());
                    OrderItems.FlgDocsRecd = bool.Parse(row["flg_docs_recd"].ToString());
                    OrderItems.GrpOrdTi = Convert.ToInt32(row["grp_ord_ti"].ToString());
                    OrderItems.MeOiSequence = Convert.ToInt32(row["me_oi_sequence"].ToString());
                    OrderItems.AmtTotSubcon = double.Parse(row["supplier_id"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OrderItems;
        }
        public List<OrderItems> selectAllOrderItemsSequence(int sequence)
      {
         List<OrderItems> returnValue = null;
         try
         {
            using (OleDbConnection conn = this.getDbConnection())
            {
               using (OleDbCommand objCmdSelect =
                   new OleDbCommand(OrderItemsQueries.getSelectAllBySequence(this.DatabaseType, sequence), conn))
               {
                  using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                  {
                     if (dr.HasRows)
                     {
                        returnValue = new List<OrderItems>();
                        while (dr.Read())
                        {
                           returnValue.Add(Load_OrderItems(dr));
                        }
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
            // Requires Logging
         }
         return returnValue;
      }

        public List<OrderItems> selectNTOrderItemsSequence(int sequence)
      {
         List<OrderItems> returnValue = null;
         try
         {
            using (OleDbConnection conn = this.getDbConnection())
            {
               using (OleDbCommand objCmdSelect =
                   new OleDbCommand(OrderItemsQueries.getSelectNTBySequence(this.DatabaseType, sequence), conn))
               {
                  Utilities.WriteLog("Query:" + objCmdSelect.CommandText);
                  using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                  {
                     if (dr.HasRows)
                     {
                        returnValue = new List<OrderItems>();
                        while (dr.Read())
                        {
                           returnValue.Add(Load_OrderItems(dr));
                        }
                     }
                  }
               }
            }
         }
         catch (Exception ex)
         {
            //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
            // Requires Logging
            Utilities.WriteLog("Error occured in exceuting query:" + ex.InnerException);
         }
         return returnValue;
      }

        private OrderItems Load_OrderItems(OleDbDataReader dr)

      {
         OrderItems OrderItems = null;
         try
         {
            if (dr != null)
            {

               OrderItems = new OrderItems();
               OrderItems.Order = new Orders();
               OrderItems.Supplier = new EntityDetailsCore();
               OrderItems.Supplier.NameLong = dr["name_long"].ToString();
               OrderItems.Order.JobRef = dr["job_ref"].ToString();
               OrderItems.Sequence = Convert.ToInt32(dr["sequence"].ToString());
               OrderItems.JobSequence = Convert.ToInt32(dr["job_sequence"].ToString());
               OrderItems.RowIndex = Convert.ToInt32(dr["row_index"].ToString());
               OrderItems.ItemType = Convert.ToInt32(dr["item_type"].ToString());
               OrderItems.ItemCode = Utilities.GetDBString(dr["item_code"]);
               OrderItems.ItemDesc = Utilities.GetDBString(dr["item_desc"]);
               OrderItems.ItemUnits = Utilities.GetDBString(dr["item_units"]);
               OrderItems.ItemQuantity = Convert.ToInt32(dr["item_quantity"].ToString());
               OrderItems.FlgRowLocked = bool.Parse(dr["flg_row_locked"].ToString());
               OrderItems.FlgRowSelected = bool.Parse(dr["flg_row_selected"].ToString());
               OrderItems.FlgRowIsText = bool.Parse(dr["flg_row_is_text"].ToString());
               OrderItems.AssetSequence = Convert.ToInt32(dr["asset_sequence"].ToString());
               OrderItems.FlgRowIsSubtotal = bool.Parse(dr["flg_row_is_subtotal"].ToString());
               OrderItems.GroupId = Convert.ToInt32(dr["group_id"].ToString());
               OrderItems.TransType = Utilities.GetDBString(dr["trans_type"]);
               OrderItems.ChgPcentLabour = Convert.ToInt32(dr["chg_pcent_labour"].ToString());
               OrderItems.AmountLabour = double.Parse(dr["amount_labour"].ToString());
               OrderItems.ChgPcentMaterials = Convert.ToInt32(dr["chg_pcent_materials"].ToString());
               OrderItems.AmountMaterials = double.Parse(dr["amount_materials"].ToString());
               OrderItems.ChgPcentPlant = Convert.ToInt32(dr["chg_pcent_plant"].ToString());
               OrderItems.AmountPlant = double.Parse(dr["amount_plant"].ToString());
               OrderItems.AdjCode = Utilities.GetDBString(dr["adj_code"]);
               //OrderItems.ChgPcentAdj =  Convert.ToInt32(dr["chg_pcent_adj"].ToString());
               OrderItems.PriorityCode = Utilities.GetDBString(dr["priority_code"].ToString());
               OrderItems.ChgPcentPriority = Convert.ToInt32(dr["chg_pcent_priority"].ToString());
               OrderItems.ChgPcentValue = Convert.ToInt32(dr["chg_pcent_value"].ToString());
               OrderItems.AmountValue = double.Parse(dr["amount_value"].ToString());
               //OrderItems.AmtQtySubtotal = dr["amt_qty_subtotal"].ToString();
               OrderItems.AmtTotLabour = double.Parse(dr["amt_tot_labour"].ToString());
               OrderItems.AmtTotMaterials = double.Parse(dr["amt_tot_materials"].ToString());
               OrderItems.AmtTotPlant = double.Parse(dr["amt_tot_plant"].ToString());
               OrderItems.AmtTotSums = double.Parse(dr["amt_tot_sums"].ToString());
               OrderItems.AmtTotLabOnly = double.Parse(dr["amt_tot_lab_only"].ToString());
               OrderItems.AmtTotPrelims = double.Parse(dr["amt_tot_prelims"].ToString());
               OrderItems.AmountTotal = double.Parse(dr["amount_total"].ToString());
               OrderItems.AmountBalance = double.Parse(dr["amount_balance"].ToString());
               OrderItems.ItemVam = Convert.ToDateTime(dr["item_vam"].ToString());
               OrderItems.AssignedTo = Convert.ToInt32(dr["assigned_to"].ToString());
               OrderItems.VamCostSequence = Convert.ToInt32(dr["vam_cost_sequence"].ToString());
               OrderItems.VamCostRate = double.Parse(dr["vam_cost_rate"].ToString());
               OrderItems.ProductOutsDia = Convert.ToInt32(dr["product_outs_dia"].ToString());
               OrderItems.ProductdOrw = Convert.ToInt32(dr["product_d_or_w"].ToString());
               OrderItems.ProductWeight = Convert.ToInt32(dr["product_weight"].ToString());
               OrderItems.ItemDueDate = Convert.ToDateTime(dr["item_due_date"].ToString());
               OrderItems.FlgCompleted = bool.Parse(dr["flg_completed"].ToString());
               OrderItems.FlgDocsRecd = bool.Parse(dr["flg_docs_recd"].ToString());
               OrderItems.GrpOrdTi = Convert.ToInt32(dr["grp_ord_ti"].ToString());
               OrderItems.MeOiSequence = Convert.ToInt32(dr["me_oi_sequence"].ToString());
               OrderItems.AmtTotSubcon = Convert.ToInt32(dr["supplier_id"].ToString());
               OrderItems.ProductdOrw = Convert.ToInt32(dr["rci_id"].ToString());
               OrderItems.ProductWeight = Convert.ToInt32(dr["rci_oi_sequence"].ToString());
               OrderItems.RciInvNo = dr["rci_inv_no"].ToString();
               OrderItems.RciDdJoin = dr["rci_dd_join"].ToString();
               //OrderItems.OiSectionSequence =  Convert.ToInt32(dr["oi_section_sequence"].ToString());
               OrderItems.RelationshipType = Convert.ToInt32(dr["relationship_type"].ToString());
               OrderItems.CreatedBy = Convert.ToInt32(dr["created_by"].ToString());
               OrderItems.DateCreated = Convert.ToDateTime(dr["date_created"].ToString());
               OrderItems.LastAmendedBy = Convert.ToInt32(dr["last_amended_by"].ToString());
               OrderItems.DateLastAmended = Convert.ToDateTime(dr["date_last_amended"].ToString());

            }
         }

         catch (Exception ex)
         {
            //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
            // Requires Logging
            Utilities.WriteLog("Error occured in load items:" + ex.Message + ex.InnerException);
         }
         return OrderItems;
      }
      //public bool updateBySequence(long sequence, long jobSequence, long joinSequence, long rowIndex, long itemType, string itemCode,
      //                            string itemDesc, string itemUnits, object itemQuantity, object labourHours,
      //                            object labourRate, double amtLabourBase, object pcentLabourA, double amtLabourSale,
      //                            object pcentLabourB, object pcentLabourC, object pcentLabourD, double amtLabourSubTotal,
      //                            double amtMaterialBase, object pcentMaterialA, double amtMaterialSale, object pcentMaterialB,
      //                            object pcentMaterialC, object pcentMaterialD, double amtMaterialSubTotal, double amtPlantBase,
      //                            object pcentPlantA, double amtPlantSale, object pcentPlantB, object pcentPlantC, object pcentPlantD,
      //                            double amtPlantSubTotal, double amtSubTotal, double amtTotal, string itemDueDate, bool flgCompleted,
      //                            string itemCompletedDate, bool flgDocsRecd, bool flgNoCharge, long supplierId, long createdBy,
      //                            DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
      //{
      //    bool returnValue = false;
      //    try
      //    {
      //        using (OleDbConnection conn = this.getDbConnection())
      //        {
      //            using (OleDbCommand objCmdUpdate =
      //                new OleDbCommand(OrderItemsQueries.update(this.DatabaseType, sequence, jobSequence, joinSequence, rowIndex, itemType, itemCode, itemDesc,
      //                                                          itemUnits, itemQuantity, labourHours, labourRate, amtLabourBase,
      //                                                          pcentLabourA, amtLabourSale, pcentLabourB, pcentLabourC, pcentLabourD,
      //                                                          amtLabourSubTotal, amtMaterialBase, pcentMaterialA, amtMaterialSale,
      //                                                          pcentMaterialB, pcentMaterialC, pcentMaterialD, amtMaterialSubTotal,
      //                                                          amtPlantBase, pcentPlantA, amtPlantSale, pcentPlantB, pcentPlantC,
      //                                                          pcentPlantD, amtPlantSubTotal, amtSubTotal, amtTotal, itemDueDate,
      //                                                          flgCompleted, itemCompletedDate, flgDocsRecd, flgNoCharge, supplierId,
      //                                                          createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
      //            {
      //                objCmdUpdate.ExecuteNonQuery();
      //            }
      //        }
      //    }
      //    catch (Exception ex)
      //    {
      //        //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
      //        // Requires Logging
      //    }
      //    return returnValue;
      //}

      //public bool deleteBySequence(long sequence)
      //{
      //    bool returnValue = false;
      //    try
      //    {
      //        using (OleDbConnection conn = this.getDbConnection())
      //        {
      //            using (OleDbCommand objCmdUpdate =
      //                new OleDbCommand(OrderItemsQueries.delete(this.DatabaseType, sequence), conn))
      //            {
      //                objCmdUpdate.ExecuteNonQuery();
      //            }
      //        }
      //    }
      //    catch (Exception ex)
      //    {
      //        //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
      //        // Requires Logging
      //    }
      //    return returnValue;
      //}

      //public bool deleteByFlgDeleted(long sequence)
      //{
      //    bool returnValue = false;
      //    try
      //    {
      //        using (OleDbConnection conn = this.getDbConnection())
      //        {
      //            using (OleDbCommand objCmdUpdate =
      //                new OleDbCommand(OrderItemsQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
      //            {
      //                objCmdUpdate.ExecuteNonQuery();
      //            }
      //        }
      //    }
      //    catch (Exception ex)
      //    {
      //        //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
      //        // Requires Logging
      //    }
      //    return returnValue;
      //}

   }
}
