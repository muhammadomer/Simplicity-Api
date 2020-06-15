using SimplicityOnlineBLL.Entities;
using SimplicityOnlineDAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;

namespace SimplicityOnlineDAL
{
    public class PurchaseOrderItemsDB : MainDB
    {

        public PurchaseOrderItemsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public List<PurchaseOrderItems> selectAllPOItems(ClientRequest clientRequest, out int count, bool isCountRequired)
        {
            List<PurchaseOrderItems> returnValue = null;
            count = 0;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(PurchaseOrderItemsQueries.selectAllPOItems(this.DatabaseType, clientRequest), conn))
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
                            returnValue = new List<PurchaseOrderItems>();
                            foreach (DataRow row in dt.Rows)
                            {
                                returnValue.Add(LoadPurchaseOrderItems(row));
                            }

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
        public bool Insert(out long sequence, long orderId, int itemImportType, long requestSequence, long jobSequence, string transType, long entityId,
                           int itemType, double itemHours, string itemCode, string itemDesc, string itemUnit, double itemQuantity, double itemAmount,
                           bool flgItemDiscount, double itemDiscountPcent, double itemDiscountAmount, double itemSubtotal, bool flgItemVat, double itemVatPcent,
                           double itemVatAmount, double itemTotal)
        {
            const string METHOD_NAME = "PurchaseOrderItemsDB.Insert()";
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(PurchaseOrderItemsQueries.Insert(this.DatabaseType, orderId, itemImportType, requestSequence, jobSequence, transType, entityId,
                                                                          itemType, itemHours, itemCode, itemDesc, itemUnit, itemQuantity, itemAmount, flgItemDiscount, itemDiscountPcent,
                                                                          itemDiscountAmount, itemSubtotal, flgItemVat, itemVatPcent, itemVatAmount, itemTotal), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Inserting Purchase Order Item.", ex);
            }
            return returnValue;
        }

        public List<PurchaseOrderItems> SelectBySequence(long sequence)
        {
            const string METHOD_NAME = "PurchaseOrderItemsDB.SelectBySequence()";
            List<PurchaseOrderItems> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(PurchaseOrderItemsQueries.SelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<PurchaseOrderItems>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadPurchaseOrderItems(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while selecting Purchase Order Item.", ex);
            }
            return returnValue;
        }

        public bool UpdateBySequence(long sequence, long orderId, long itemImportType, long requestSequence, long jobSequence, string transType, long entityId,
                                     long itemType, string itemHours, string itemCode, string itemDesc, string itemUnit, string itemQuantity, double itemAmount,
                                     bool flgItemDiscount, string itemDiscountPcent, double itemDiscountAmount, double itemSubtotal, bool flgItemVat, string itemVatPcent,
                                     double itemVatAmount, double itemTotal)
        {
            const string METHOD_NAME = "PurchaseOrderItemsDB.UpdateBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(PurchaseOrderItemsQueries.Update(this.DatabaseType, sequence, orderId, itemImportType, requestSequence, jobSequence, transType, entityId,
                                                                          itemType, itemHours, itemCode, itemDesc, itemUnit, itemQuantity, itemAmount, flgItemDiscount, itemDiscountPcent,
                                                                          itemDiscountAmount, itemSubtotal, flgItemVat, itemVatPcent, itemVatAmount, itemTotal), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while updating Purchase Order Item.", ex);
            }
            return returnValue;
        }

        public bool DeleteBySequence(long sequence)
        {
            const string METHOD_NAME = "PurchaseOrderItemsDB.DeleteBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(PurchaseOrderItemsQueries.Delete(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while deleting Purchase Order Item By sequence.", ex);
            }
            return returnValue;
        }

        public bool DeleteByFlgDeleted(long sequence)
        {
            const string METHOD_NAME = "PurchaseOrderItemsDB.DeleteByFlgDeleted()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(PurchaseOrderItemsQueries.DeleteFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while deleting Purchase Order Item By Flg Deleted.", ex);
            }
            return returnValue;
        }

        private PurchaseOrderItems LoadPurchaseOrderItems(OleDbDataReader dr)
        {
            const string METHOD_NAME = "PurchaseOrderItemsDB.LoadPurchaseOrderItems()";
            PurchaseOrderItems purchaseOrderItems = null;
            try
            {
                if (dr != null)
                {
                    purchaseOrderItems = new PurchaseOrderItems();
                    purchaseOrderItems.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    purchaseOrderItems.OrderId = DBUtil.GetLongValue(dr, "order_id");
                    purchaseOrderItems.ItemImportType = DBUtil.GetIntValue(dr, "item_import_type");
                    purchaseOrderItems.RequestSequence = DBUtil.GetLongValue(dr, "request_sequence");
                    purchaseOrderItems.JobSequence = DBUtil.GetLongValue(dr, "job_sequence");
                    purchaseOrderItems.TransType = DBUtil.GetStringValue(dr, "trans_type");
                    purchaseOrderItems.EntityId = DBUtil.GetLongValue(dr, "entity_id");
                    purchaseOrderItems.ItemType = DBUtil.GetIntValue(dr, "item_type");
                    purchaseOrderItems.ItemHours = DBUtil.GetDoubleValue(dr, "item_hours");
                    purchaseOrderItems.ItemCode = DBUtil.GetStringValue(dr, "item_code");
                    purchaseOrderItems.ItemDesc = DBUtil.GetStringValue(dr, "item_desc");
                    purchaseOrderItems.ItemUnit = DBUtil.GetStringValue(dr, "item_unit");
                    purchaseOrderItems.ItemQuantity = DBUtil.GetDoubleValue(dr, "item_quantity");
                    purchaseOrderItems.FlgItemDiscount = DBUtil.GetBooleanValue(dr, "flg_item_discount");
                    purchaseOrderItems.ItemDiscountPcent = DBUtil.GetDoubleValue(dr, "item_discount_pcent");
                    purchaseOrderItems.FlgItemVat = DBUtil.GetBooleanValue(dr, "flg_item_vat");
                    purchaseOrderItems.ItemVatPcent = DBUtil.GetDoubleValue(dr, "item_vat_pcent");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Loading Purchase Order Item.", ex);
            }
            return purchaseOrderItems;
        }

        private PurchaseOrderItems LoadPurchaseOrderItems(DataRow row)
        { 
            PurchaseOrderItems purchaseOrderItems = null;
            try
            {
                if (row != null)
                {
                    purchaseOrderItems = new PurchaseOrderItems();
                    purchaseOrderItems.Sequence = DBUtil.GetLongValue(row, "sequence");
                    purchaseOrderItems.OrderId = DBUtil.GetLongValue(row, "order_id");
                    purchaseOrderItems.ItemImportType = DBUtil.GetIntValue(row, "item_import_type");
                    purchaseOrderItems.RequestSequence = DBUtil.GetLongValue(row, "request_sequence");
                    purchaseOrderItems.JobSequence = DBUtil.GetLongValue(row, "job_sequence");
                    purchaseOrderItems.JobRef = DBUtil.GetStringValue(row, "job_ref");
                    purchaseOrderItems.JobAddress = DBUtil.GetStringValue(row, "job_address");
                    purchaseOrderItems.Supplier = DBUtil.GetStringValue(row, "supplier");
                    purchaseOrderItems.OrderRef = DBUtil.GetStringValue(row, "order_ref");
                    purchaseOrderItems.OrderDate = DBUtil.GetDateValue(row, "order_date");
                    purchaseOrderItems.ItemType = DBUtil.GetIntValue(row, "item_type");
                    purchaseOrderItems.ItemHours = DBUtil.GetDoubleValue(row, "item_hours");
                    purchaseOrderItems.ItemCode = DBUtil.GetStringValue(row, "item_code");
                    purchaseOrderItems.ItemDesc = DBUtil.GetStringValue(row, "item_desc");
                    purchaseOrderItems.ItemUnit = DBUtil.GetStringValue(row, "item_unit");
                    purchaseOrderItems.ItemQuantity = DBUtil.GetDoubleValue(row, "item_quantity");
                    purchaseOrderItems.ItemAmount = DBUtil.GetDoubleValue(row, "item_amount");
                    purchaseOrderItems.FlgItemDiscount = DBUtil.GetBooleanValue(row, "flg_item_discount");
                    purchaseOrderItems.ItemDiscountPcent = DBUtil.GetDoubleValue(row, "item_discount_pcent");
                    purchaseOrderItems.ItemDiscountAmount = DBUtil.GetDoubleValue(row, "item_discount_amount");
                    purchaseOrderItems.ItemSubtotal = DBUtil.GetDoubleValue(row, "item_subtotal");
                    purchaseOrderItems.FlgItemVat = DBUtil.GetBooleanValue(row, "flg_item_vat");
                    purchaseOrderItems.ItemVatAmount = DBUtil.GetDoubleValue(row, "item_vat_amount");
                    purchaseOrderItems.ItemVatPcent = DBUtil.GetDoubleValue(row, "item_vat_pcent");
                    purchaseOrderItems.ItemTotal = DBUtil.GetDoubleValue(row, "item_total");
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog( "Error occured while Loading Purchase Order Item."+ ex.Message);
            }
            return purchaseOrderItems;
        }
    }
}
