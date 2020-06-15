using System;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineDAL.QueriesRepo
{
    public static class SubConPOItemsQueries
    {

        public static string SelectAllBySequence(string databaseType, long sequence)
        {
            return @"SELECT * FROM  un_sub_con_po_items WHERE sequence = " + sequence;
        }

        public static string Insert(string databaseType, long poSequence, long entityId, int itemType, string itemCode, string itemDesc,
                           string itemUnit, double itemQuantity, double itemAmountMat, double itemAmountLab, double itemAmountNet,
                           bool flgItemDiscount, double itemDiscountPcent, double itemDiscountAmount, double itemSubtotal,
                           bool flgItemVat, double itemVatPcent, double itemVatAmount, double itemTotal, double itemHours, 
                           int createdBy, DateTime? dateCreated, int lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"INSERT INTO un_sub_con_po_items(po_sequence, entity_id, item_type, item_code, item_desc, item_unit,
                            item_quantity, item_amt_mat, item_amt_labour, item_amt_net, flg_item_discount,
                            item_pcent_discount, item_amt_discount, item_amt_subtotal, flg_item_vat, item_pcent_vat,
                            item_amt_vat, item_amt_total, item_hours, created_by, date_created, last_amended_by, date_last_amended)
                     VALUES (" + poSequence + ", " + entityId + ", " + itemType + ", '" + itemCode + "', '" + itemDesc + "', '" + 
                            itemUnit + "', " + itemQuantity + ", " + itemAmountMat + ", " + itemAmountLab + ", " + itemAmountNet + ", " + 
                            Utilities.GetBooleanForDML(databaseType, flgItemDiscount) + ", " + itemDiscountPcent + ", " +  
                            itemDiscountAmount + ", " + itemSubtotal + ", " +  Utilities.GetBooleanForDML(databaseType, flgItemVat) + ", " +  
                            itemVatPcent + ", " +  itemVatAmount + ", " + itemTotal + "," + itemHours + ", " + createdBy + ", " + 
                            Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " +
                            lastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")"; ;
        }

        public static string Update(string databaseType, long sequence, long orderId, long itemImportType, long requestSequence, long jobSequence, string transType, long entityId, long itemType,
                                    string itemHours, string itemCode, string itemDesc, string itemUnit, string itemQuantity, double itemAmount, bool flgItemDiscount,
                                    string itemDiscountPcent, double itemDiscountAmount, double itemSubtotal, bool flgItemVat, string itemVatPcent, double itemVatAmount, double itemTotal)
        {
            return @"UPDATE un_sub_con_po_items
                        SET order_id = " + orderId + ", " + 
		            "       item_import_type = " + itemImportType + ", " + 
		            "       request_sequence = " + requestSequence + ", " + 
		            "       job_sequence = " + jobSequence + ", " + 
		            "       trans_type = '" + transType + "', " + 
		            "       entity_id = " + entityId + ", " + 
		            "       item_type = " + itemType + ", " + 
		            "       item_hours = " + itemHours + ", " + 
		            "       item_code = '" + itemCode + "', " + 
		            "       item_desc = '" + itemDesc + "', " + 
		            "       item_unit = '" + itemUnit + "', " + 
		            "       item_quantity =  " + itemQuantity + ", " + 
		            "       item_amount = " + itemAmount + ", " + 
		            "       flg_item_discount = " + Utilities.GetBooleanForDML(databaseType, flgItemDiscount) + ", " + 
		            "       item_discount_pcent = " + itemDiscountPcent + ", " + 
		            "       item_discount_amount = " + itemDiscountAmount + ", " + 
		            "       item_subtotal = " + itemSubtotal + ", " + 
		            "       flg_item_vat = " + Utilities.GetBooleanForDML(databaseType, flgItemVat) + ", " + 
		            "       item_vat_pcent = " + itemVatPcent + ", " + 
		            "       item_vat_amount = " + itemVatAmount + ", " + 
		            "       item_total = " + itemTotal + 
                    "  WHERE sequence = " + sequence;
        }

        public static string Delete(string databaseType, long sequence)
        {
            return @"DELETE FROM un_sub_con_po_items WHERE sequence = " + sequence;
        }

        public static string DeleteFlagDeleted(string databaseType, long sequence)
        {
            return @"UPDATE un_sub_con_po_items SET flg_deleted = " + Utilities.GetBooleanForDML(databaseType, true) + " " +
                    " WHERE sequence = " + sequence;
        }
    }
}

