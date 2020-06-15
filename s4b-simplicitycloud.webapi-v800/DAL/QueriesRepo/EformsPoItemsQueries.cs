using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineDAL.QueriesRepo
{
    public static class EformsPoItemsQueries
    {
        public static string SelectAllBySequence(long sequence)
        {
            return @"SELECT * FROM un_eforms_po_items WHERE sequence = " + sequence;
        }

        public static string Insert(string databaseType, bool flgDeleted, int dataType, string impRef, long joinSequence, int itemType, string itemCode, 
                                    string itemDesc, string itemUnit, double itemQuantity, double itemAmtUnitPrice, double itemAmtSubtotalBeforeDiscount,
                                    bool flgItemDiscount, double itemDiscountPcent, double itemAmtDiscount, double itemAmtSubtotal, bool flgItemVat,
                                    double itemVatPcent, double itemAmtVat, double itemAmtTotal, DateTime? dateItemDueDate, bool flgDeliverToSite,
                                    bool flgDeliveryNote, string deliveryNoteRef, double deliveryNoteQty, DateTime? dateDeliveryNote, int createdBy,
                                    DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"INSERT INTO un_eforms_po_items(flg_deleted,  data_type,  imp_ref,  join_sequence,  item_type,  item_code,
                            item_desc,  item_unit,  item_quantity,  item_amt_unit_price,  item_amt_subtotal_before_discount,
                            flg_item_discount,  item_discount_pcent,  item_amt_discount,  item_amt_subtotal,
                            flg_item_vat,  item_vat_pcent,  item_amt_vat,  item_amt_total,  date_item_due_date,
                            flg_deliver_to_site,  flg_delivery_note,  delivery_note_ref,  delivery_note_qty,
                            date_delivery_note, created_by, date_created, last_amended_by, date_last_amended) 
                     VALUES (" + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", " + dataType + ", '" + impRef + "', " + joinSequence + ", " + 
                            itemType + ", '" + itemCode + "', '" + itemDesc + "', '"  + itemUnit + "', " + itemQuantity + ",  " + itemAmtUnitPrice + ", " + 
                            itemAmtSubtotalBeforeDiscount + ", " + Utilities.GetBooleanForDML(databaseType, flgItemDiscount) + ", " + 
                            itemDiscountPcent + ", " + itemAmtDiscount + ", " + itemAmtSubtotal + ", " + Utilities.GetBooleanForDML(databaseType, flgItemVat) + ", " + itemVatPcent + ", " + 
                            itemAmtVat + ", " + itemAmtTotal + ", " + Utilities.GetDateTimeForDML(databaseType, dateItemDueDate, true, true) + ", " + 
                            Utilities.GetBooleanForDML(databaseType, flgDeliverToSite) + ", " + Utilities.GetBooleanForDML(databaseType, flgDeliveryNote) + ", '" + deliveryNoteRef + "', " + 
                            deliveryNoteQty + ", " + Utilities.GetDateTimeForDML(databaseType, dateDeliveryNote, true, true) + ", " + createdBy + ", " + 
                            Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " + lastAmendedBy + ", " + 
                            Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
        }

        public static string update(string databaseType, long sequence, bool flgDeleted, int dataType, string impRef, long joinSequence, int itemType, 
                                    string itemCode, string itemDesc, string itemUnit, double itemQuantity, double itemAmtUnitPrice, 
                                    double itemAmtSubtotalBeforeDiscount, bool flgItemDiscount, double itemDiscountPcent, double itemAmtDiscount, 
                                    double itemAmtSubtotal, bool flgItemVat, double itemVatPcent, double itemAmtVat, double itemAmtTotal, 
                                    DateTime? dateItemDueDate, bool flgDeliverToSite, bool flgDeliveryNote, string deliveryNoteRef, 
                                    double deliveryNoteQty, DateTime? dateDeliveryNote, int createdBy,
                                    DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE un_eforms_po_items " +
                              "   SET  flg_deleted = " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", " +
                              "        data_type = " + dataType + ", " +
                              "        imp_ref = '" + impRef + "', " +
                              "        join_sequence = " + joinSequence + ", " +
                              "        item_type = " + itemType + ", " +
                              "        item_code = '" + itemCode + "', " +
                              "        item_desc = '" + itemDesc + "', " +
                              "        item_unit = '" + itemUnit + "', " +
                              "        item_quantity = " + itemQuantity + ", " +
                              "        item_amt_unit_price = " + itemAmtUnitPrice + ", " +
                              "        item_amt_subtotal_before_discount = " + itemAmtSubtotalBeforeDiscount + ", " +
                              "        flg_item_discount = " + Utilities.GetBooleanForDML(databaseType, flgItemDiscount) + ", " +
                              "        item_discount_pcent = " + itemDiscountPcent + ", " +
                              "        item_amt_discount = " + itemAmtDiscount + ", " +
                              "        item_amt_subtotal = " + itemAmtSubtotal + ", " +
                              "        flg_item_vat = " + Utilities.GetBooleanForDML(databaseType, flgItemVat) + ", " +
                              "        item_vat_pcent =  " + itemVatPcent + ", " +
                              "        item_amt_vat = " + itemAmtVat + ", " +
                              "        item_amt_total = " + itemAmtTotal + ", " +
                              "        date_item_due_date = " + Utilities.GetDateTimeForDML(databaseType, dateItemDueDate, true, true) + ", " +
                              "        flg_deliver_to_site = " + Utilities.GetBooleanForDML(databaseType, flgDeliverToSite) + ", " +
                              "        flg_delivery_note = " + Utilities.GetBooleanForDML(databaseType, flgDeliveryNote) + ", " +
                              "        delivery_note_ref = " + deliveryNoteRef + ", " +
                              "        delivery_note_qty = " + deliveryNoteQty + ", " +
                              "        date_delivery_note = " + Utilities.GetDateTimeForDML(databaseType, dateDeliveryNote, true, true) + ", " +
                              "        created_by = " + createdBy + ", " +
                              "        date_created = " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " +
                              "        last_amended_by = " + lastAmendedBy + ", " +
                              "        date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) +
                              "  WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        
        public static string delete(string databaseType, long sequence)
        {
            return @"DELETE FROM   un_eforms_po_items WHERE sequence = " + sequence;
        }

        public static string deleteFlagDeleted(string databaseType, long sequence)
        {
            return @"UPDATE un_eforms_po_items SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, true) +
                    " WHERE sequence = " + sequence;
        }
    }
}

