using SimplicityOnlineBLL.Entities;
using SimplicityOnlineDAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineDAL
{
    public class SubConPOItemsDB : MainDB
    {

        public SubConPOItemsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool Insert(out long sequence, long poSequence, long entityId, int itemType, string itemCode, string itemDesc, 
                           string itemUnit, double itemQuantity, double itemAmountMat, double itemAmountLab, double itemAmountNet,
                           bool flgItemDiscount, double itemDiscountPcent, double itemDiscountAmount, double itemSubtotal, 
                           bool flgItemVat, double itemVatPcent, double itemVatAmount, double itemTotal, double itemHours,
                           int createdBy, DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "SubConPOItemsDB.Insert()";
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(SubConPOItemsQueries.Insert(this.DatabaseType, poSequence, entityId, itemType, itemCode, 
                                                                     itemDesc, itemUnit, itemQuantity, itemAmountMat, itemAmountLab,
                                                                     itemAmountNet, flgItemDiscount, itemDiscountPcent, itemDiscountAmount, 
                                                                     itemSubtotal, flgItemVat, itemVatPcent, itemVatAmount, itemTotal, itemHours,
                                                                     createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Inserting Sub Con PO Item.", ex);
            }
            return returnValue;
        }

        public List<SubConPOItems> SelectBySequence(long sequence)
        {
            const string METHOD_NAME = "SubConPOItemsDB.SelectBySequence()";
            List<SubConPOItems> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(SubConPOItemsQueries.SelectAllBySequence(this.DatabaseType, sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<SubConPOItems>();
                                while (dr.Read())
                                {
                                    returnValue.Add(LoadSubConPOItems(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while selecting Sub Con PO Item.", ex);
            }
            return returnValue;
        }

        public bool UpdateBySequence(long sequence, long orderId, long itemImportType, long requestSequence, long jobSequence, string transType, long entityId,
                                     long itemType, string itemHours, string itemCode, string itemDesc, string itemUnit, string itemQuantity, double itemAmount,
                                     bool flgItemDiscount, string itemDiscountPcent, double itemDiscountAmount, double itemSubtotal, bool flgItemVat, string itemVatPcent,
                                     double itemVatAmount, double itemTotal)
        {
            const string METHOD_NAME = "SubConPOItemsDB.UpdateBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(SubConPOItemsQueries.Update(this.DatabaseType, sequence, orderId, itemImportType, requestSequence, jobSequence, transType, entityId,
                                                                          itemType, itemHours, itemCode, itemDesc, itemUnit, itemQuantity, itemAmount, flgItemDiscount, itemDiscountPcent,
                                                                          itemDiscountAmount, itemSubtotal, flgItemVat, itemVatPcent, itemVatAmount, itemTotal), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while updating Sub Con PO Item.", ex);
            }
            return returnValue;
        }

        public bool DeleteBySequence(long sequence)
        {
            const string METHOD_NAME = "SubConPOItemsDB.DeleteBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(SubConPOItemsQueries.Delete(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while deleting Sub Con PO Item By sequence.", ex);
            }
            return returnValue;
        }

        public bool DeleteByFlgDeleted(long sequence)
        {
            const string METHOD_NAME = "SubConPOItemsDB.DeleteByFlgDeleted()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(SubConPOItemsQueries.DeleteFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while deleting Sub Con PO Item By Flg Deleted.", ex);
            }
            return returnValue;
        }

        private SubConPOItems LoadSubConPOItems(OleDbDataReader dr)
        {
            const string METHOD_NAME = "SubConPOItemsDB.LoadSubConPOItems()";
            SubConPOItems SubConPOItems = null;
            try
            {
                if (dr != null)
                {
                    SubConPOItems = new SubConPOItems();
                    SubConPOItems.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    SubConPOItems.POSequence = DBUtil.GetLongValue(dr, "po_sequence");
                    SubConPOItems.EntityId = DBUtil.GetLongValue(dr, "entity_id");
                    SubConPOItems.ItemType = DBUtil.GetIntValue(dr, "item_type");
                    SubConPOItems.ItemCode = DBUtil.GetStringValue(dr, "item_code");
                    SubConPOItems.ItemDesc = DBUtil.GetStringValue(dr, "item_desc");
                    SubConPOItems.ItemUnit = DBUtil.GetStringValue(dr, "item_unit");
                    SubConPOItems.ItemQuantity = DBUtil.GetDoubleValue(dr, "item_quantity");
                    SubConPOItems.ItemAmountMat = DBUtil.GetDoubleValue(dr, "item_amt_mat");
                    SubConPOItems.ItemAmountLabour = DBUtil.GetDoubleValue(dr, "item_amt_labour");
                    SubConPOItems.ItemAmountNet = DBUtil.GetDoubleValue(dr, "item_amt_net");
                    SubConPOItems.FlgItemDiscount = DBUtil.GetBooleanValue(dr, "flg_item_discount");
                    SubConPOItems.ItemDiscountPcent = DBUtil.GetDoubleValue(dr, "item_discount_pcent");
                    SubConPOItems.ItemDiscountAmount = DBUtil.GetDoubleValue(dr, "item_amt_discount");
                    SubConPOItems.ItemSubtotal = DBUtil.GetDoubleValue(dr, "item_amt_subtotal");
                    SubConPOItems.FlgItemVat = DBUtil.GetBooleanValue(dr, "flg_item_vat");
                    SubConPOItems.ItemVatPcent = DBUtil.GetDoubleValue(dr, "item_pcent_vat");
                    SubConPOItems.ItemVatAmount = DBUtil.GetDoubleValue(dr, "item_amt_vat");
                    SubConPOItems.ItemTotal = DBUtil.GetDoubleValue(dr, "item_amt_total");
                    SubConPOItems.ItemHours = DBUtil.GetDoubleValue(dr, "item_hours");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Loading Sub Con PO Item.", ex);
            }
            return SubConPOItems;
        }
    }
}
