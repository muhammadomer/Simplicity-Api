using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineBLL.Entities;
namespace SimplicityOnlineDAL.QueriesRepo
{
    public static class PurchaseOrderItemsQueries
    {

        public static string SelectAllBySequence(string databaseType, long sequence)
        {
            return @"SELECT * FROM  un_purchase_order_items WHERE sequence = " + sequence;
        }

        public static string selectAllPOItems(string databaseType, ClientRequest clientRequest)
        {
            string returnValue = "";
            try
            {
                returnValue = @"SELECT poi.sequence, poi.order_id, poi.item_import_type, poi.request_sequence, poi.job_sequence, ord.job_ref,
                    poi.item_type, poi.item_hours, poi.item_code, poi.item_desc, poi.item_unit, poi.item_quantity, poi.item_amount,
                    poi.flg_item_discount, poi.item_discount_pcent, poi.item_discount_amount,poi.item_subtotal, 
                    poi.flg_item_vat, poi.item_vat_pcent, poi.item_vat_amount,poi.item_total,
                    po.supplier_id,UNC.name_short AS supplier,po.order_ref,po.order_date,ord.job_address
                FROM ((un_purchase_order_items AS poi
                    LEFT JOIN un_orders AS ord ON poi.job_sequence = ord.sequence)
                    INNER JOIN un_purchase_orders AS po  ON poi.order_id = po.order_id)
                    LEFT JOIN un_entity_details_core UNC on UNC.entity_id = PO.supplier_id
                 WHERE poi.sequence> 0";
                if (clientRequest.globalFilter != null && clientRequest.globalFilter != "")
                {
                    string filterValue = clientRequest.globalFilter;
                    string globalFilterQuery = " And( ord.job_ref like '%" + filterValue + "%'"
                       + " or UNC.name_short like '%" + filterValue + "%')";
                    returnValue += globalFilterQuery;
                }
                
                returnValue += " Order by po.order_date Desc,po.order_id desc";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }


        public static string Insert(string databaseType, long orderId, int itemImportType, long requestSequence, long jobSequence,  
                                    string transType, long entityId, int itemType,  double itemHours, string itemCode, string itemDesc, 
                                    string itemUnit, double itemQuantity, double itemAmount, bool flgItemDiscount, double itemDiscountPcent,  
                                    double itemDiscountAmount, double itemSubtotal, bool flgItemVat, double itemVatPcent, 
                                    double itemVatAmount, double itemTotal)
        {
            return @"INSERT INTO un_purchase_order_items(order_id, item_import_type, request_sequence, job_sequence, trans_type, entity_id,
                            item_type, item_hours, item_code, item_desc, item_unit, item_quantity, item_amount, flg_item_discount, 
                            item_discount_pcent, item_discount_amount, item_subtotal, flg_item_vat, item_vat_pcent, item_vat_amount, 
                            item_total)
                     VALUES (" +  orderId + ", " +  itemImportType + ", " + requestSequence + ", " + jobSequence + ", '" +  transType + "', " + 
                            entityId + ", " + itemType + ", " + itemHours + ", '" + itemCode + "', '" + itemDesc + "', '" + itemUnit + "', " + 
                            itemQuantity + ", " + itemAmount + ", " + Utilities.GetBooleanForDML(databaseType, flgItemDiscount) + ", " + 
                            itemDiscountPcent + ", " +  itemDiscountAmount + ", " + itemSubtotal + ", " +  
                            Utilities.GetBooleanForDML(databaseType, flgItemVat) + ", " +  itemVatPcent + ", " +  itemVatAmount + ", " + 
                            itemTotal + ")";
        }

        public static string Update(string databaseType, long sequence, long orderId, long itemImportType, long requestSequence, long jobSequence, string transType, long entityId, long itemType,
                                    string itemHours, string itemCode, string itemDesc, string itemUnit, string itemQuantity, double itemAmount, bool flgItemDiscount,
                                    string itemDiscountPcent, double itemDiscountAmount, double itemSubtotal, bool flgItemVat, string itemVatPcent, double itemVatAmount, double itemTotal)
        {
            return @"UPDATE un_purchase_order_items
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
            return @"DELETE FROM   un_purchase_order_items WHERE sequence = " + sequence;
        }

        public static string DeleteFlagDeleted(string databaseType, long sequence)
        {
            return @"UPDATE un_purchase_order_items SET flg_deleted = " + Utilities.GetBooleanForDML(databaseType, true) + " " +
                    " WHERE sequence = " + sequence;
        }
    }
}

