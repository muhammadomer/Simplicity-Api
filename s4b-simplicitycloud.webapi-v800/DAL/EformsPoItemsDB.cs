using SimplicityOnlineBLL.Entities;
using SimplicityOnlineDAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineDAL
{
    public class EformsPoItemsDB : MainDB
    {

        public EformsPoItemsDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool InsertEformsPoItems(out long sequence, bool flgDeleted, int dataType, string impRef, long joinSequence, int itemType, 
                                        string itemCode, string itemDesc, string itemUnit, double itemQuantity, double itemAmtUnitPrice, 
                                        double itemAmtSubtotalBeforeDiscount, bool flgItemDiscount, double itemDiscountPcent, 
                                        double itemAmtDiscount, double itemAmtSubtotal, bool flgItemVat, double itemVatPcent, double itemAmtVat, 
                                        double itemAmtTotal, DateTime? dateItemDueDate, bool flgDeliverToSite, bool flgDeliveryNote, 
                                        string deliveryNoteRef, double deliveryNoteQty, DateTime? dateDeliveryNote, int createdBy,
                                        DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "EformsPoItemsDB.InsertEformsPoItems()";
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(EformsPoItemsQueries.Insert(this.DatabaseType, flgDeleted, dataType, impRef, joinSequence, itemType, itemCode, itemDesc, itemUnit,
                                                                    itemQuantity, itemAmtUnitPrice, itemAmtSubtotalBeforeDiscount, flgItemDiscount,
                                                                    itemDiscountPcent, itemAmtDiscount, itemAmtSubtotal, flgItemVat, itemVatPcent,
                                                                    itemAmtVat, itemAmtTotal, dateItemDueDate, flgDeliverToSite, flgDeliveryNote,
                                                                    deliveryNoteRef, deliveryNoteQty, dateDeliveryNote, createdBy, dateCreated,
                                                                    lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Inserting EForms PO Items.", ex);
            }
            return returnValue;
        }

        public List<EFormsPOItems> GetAllEformsPoItemsSequence(long sequence)
        {
            const string METHOD_NAME = "EformsPoItemsDB.GetAllEformsPoItemsSequence()";
            List<EFormsPOItems> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(EformsPoItemsQueries.SelectAllBySequence(sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<EFormsPOItems>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_EformsPoItems(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Inserting EForms PO Items.", ex);
            }
            return returnValue;
        }


        public bool UpdateBySequence(long sequence, bool flgDeleted, int dataType, string impRef, long joinSequence, int itemType, string itemCode,
                                     string itemDesc, string itemUnit, double itemQuantity, double itemAmtUnitPrice, double itemAmtSubtotalBeforeDiscount,
                                     bool flgItemDiscount, double itemDiscountPcent, double itemAmtDiscount, double itemAmtSubtotal, bool flgItemVat,
                                     double itemVatPcent, double itemAmtVat, double itemAmtTotal, DateTime? dateItemDueDate, bool flgDeliverToSite,
                                     bool flgDeliveryNote, string deliveryNoteRef, double deliveryNoteQty, DateTime? dateDeliveryNote, int createdBy,
                                     DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "EformsPoItemsDB.UpdateBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EformsPoItemsQueries.update(this.DatabaseType, sequence, flgDeleted, dataType, impRef, joinSequence, itemType, itemCode, itemDesc, itemUnit,
                                                                    itemQuantity, itemAmtUnitPrice, itemAmtSubtotalBeforeDiscount, flgItemDiscount,
                                                                    itemDiscountPcent, itemAmtDiscount, itemAmtSubtotal, flgItemVat, itemVatPcent,
                                                                    itemAmtVat, itemAmtTotal, dateItemDueDate, flgDeliverToSite, flgDeliveryNote,
                                                                    deliveryNoteRef, deliveryNoteQty, dateDeliveryNote, createdBy, dateCreated,
                                                                    lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Updating EForms PO Items.", ex);
            }
            return returnValue;
        }

        public bool DeleteBySequence(long sequence)
        {
            const string METHOD_NAME = "EformsPoItemsDB.DeleteBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EformsPoItemsQueries.delete(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Deleting EForms PO Items.", ex);
            }
            return returnValue;
        }

        public bool DeleteByFlgDeleted(long sequence)
        {
            const string METHOD_NAME = "EformsPoItemsDB.DeleteByFlgDeleted()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EformsPoItemsQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Deleting EForms PO Items by Deleted Flag.", ex);
            }
            return returnValue;
        }

        private EFormsPOItems Load_EformsPoItems(OleDbDataReader dr)
        {
            const string METHOD_NAME = "EformsPoItemsDB.Load_EformsPoItems()";
            EFormsPOItems eformsPoItems = null;
            try
            {
                if (dr != null)
                {
                    eformsPoItems = new EFormsPOItems();
                    eformsPoItems.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    eformsPoItems.FlgDeleted = DBUtil.GetBooleanValue(dr, "flg_deleted");
                    eformsPoItems.DataType = DBUtil.GetIntValue(dr, "data_type");
                    eformsPoItems.ImpRef = DBUtil.GetStringValue(dr, "imp_ref");
                    eformsPoItems.JoinSequence = DBUtil.GetLongValue(dr, "join_sequence");
                    eformsPoItems.ItemType = DBUtil.GetIntValue(dr, "item_type");
                    eformsPoItems.ItemCode = DBUtil.GetStringValue(dr, "item_code");
                    eformsPoItems.ItemDesc = DBUtil.GetStringValue(dr, "item_desc");
                    eformsPoItems.ItemUnit = DBUtil.GetStringValue(dr, "item_unit");
                    eformsPoItems.ItemQuantity = DBUtil.GetDoubleValue(dr, "item_quantity");
                    eformsPoItems.FlgItemDiscount = DBUtil.GetBooleanValue(dr, "flg_item_discount");
                    eformsPoItems.ItemDiscountPcent = DBUtil.GetDoubleValue(dr, "item_discount_pcent");
                    eformsPoItems.FlgItemVat = DBUtil.GetBooleanValue(dr, "flg_item_vat");
                    eformsPoItems.ItemVatPcent = DBUtil.GetDoubleValue(dr, "item_vat_pcent");
                    eformsPoItems.DateItemDueDate = DBUtil.GetDateTimeValue(dr, "date_item_due_date");
                    eformsPoItems.FlgDeliverToSite = DBUtil.GetBooleanValue(dr, "flg_deliver_to_site");
                    eformsPoItems.FlgDeliveryNote = DBUtil.GetBooleanValue(dr, "flg_delivery_note");
                    eformsPoItems.DeliveryNoteRef = DBUtil.GetStringValue(dr, "delivery_note_ref");
                    eformsPoItems.DeliveryNoteQty = DBUtil.GetDoubleValue(dr, "delivery_note_qty");
                    eformsPoItems.DateDeliveryNote = DBUtil.GetDateTimeValue(dr, "date_delivery_note");
                    eformsPoItems.CreatedBy = DBUtil.GetIntValue(dr, "created_by");
                    eformsPoItems.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    eformsPoItems.LastAmendedBy = DBUtil.GetIntValue(dr, "last_amended_by");
                    eformsPoItems.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Loading EForms PO Items by Deleted Flag.", ex);
            }
            return eformsPoItems;
        }

    }
}
