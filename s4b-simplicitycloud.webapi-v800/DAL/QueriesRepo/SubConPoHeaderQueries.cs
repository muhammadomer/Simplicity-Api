using System;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineDAL.QueriesRepo
{
    public static class SubConPoHeaderQueries
    {

        public static string getSelectAllByorderId(string databaseType, long orderId)
        {
            return @"SELECT * 
                       FROM  un_sub_con_po_header
                      WHERE order_id = " + orderId;
        }

        public static string insert(string databaseType, bool flgEformsImport, long eformsImportId, bool flgPoPlaced, int poType, string poRef, string poCustomerRef,
                                    long joBSequence, long entityId, string entityAddress, string entityTelephone, DateTime? poDate, string addressInvoice,
                                    string addressDelivery, double poAmtMat, double poAmtLabour, double poAmtDiscount, double poAmtShipping, double poAmtSubtotal,
                                    double poAmtVAT, double poAmtTotal, long requestedId, string poVehicleReg, string poNotes, DateTime? requiredByDate,
                                    bool flgDispatchDate, DateTime? dateDespatchDate, string poOrderedBy, int poStatus, string userField01, string userField02, string userField03,
                                    string userField04, string userField05, string userField06, string userField07, string userField08, string userField09,
                                    string userField10, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"INSERT INTO un_sub_con_po_header(flg_eforms_import, eforms_import_id, flg_po_placed, po_type, po_ref,  
                            po_customer_ref, job_sequence, entity_id, entity_address, entity_telephone, date_po_date, address_invoice, 
                            address_delivery, po_amt_mat, po_amt_labour, po_amt_discount, po_amt_shipping, po_amt_subtotal, 
                            po_amt_vat, po_amt_total, requested_id, po_vehicle_reg, po_notes, required_by_date, flg_dispatch_date,
                            date_despatch_date, po_ordered_by, po_status, po_user_field_01, po_user_field_02, po_user_field_03, 
                            po_user_field_04, po_user_field_05, po_user_field_06, po_user_field_07, po_user_field_08, po_user_field_09, 
                            po_user_field_10, created_by, date_created, last_amended_by, date_last_amended)
                    VALUES(" + Utilities.GetBooleanForDML(databaseType, flgEformsImport) + ", " + eformsImportId + ", " +
                            Utilities.GetBooleanForDML(databaseType, flgPoPlaced) + ", " + poType + ", '" + poRef + "', '" + poCustomerRef + "', " + 
                            joBSequence + "," + entityId + ", '" + entityAddress + "', '" + entityTelephone + "', " + 
                            Utilities.GetDateTimeForDML(databaseType, poDate, true, true) + ", '" + addressInvoice + "', '" +
                            addressDelivery + "', " + poAmtMat + ", " + poAmtLabour + ", " + poAmtDiscount + ", " + 
                            poAmtShipping + ", " + poAmtSubtotal + ", " + poAmtVAT + ", " + poAmtTotal + ", " + requestedId + ", '" + 
                            poVehicleReg + "', '" + poNotes + "', " + Utilities.GetDateTimeForDML(databaseType, requiredByDate, true, true) + ", " +
                            Utilities.GetBooleanForDML(databaseType, flgDispatchDate) + ", " + 
                            Utilities.GetDateTimeForDML(databaseType, dateDespatchDate, true, true) + ", '" + poOrderedBy + "', " + 
                            poStatus + ", '" + userField01 + "', '" + userField02 + "', '" + userField03 + "', '" + userField04 + "', '" + 
                            userField05 + "', '" + userField06 + "', '" + userField07 + "', '" + userField08 + "', '" + userField09 + "', '" + 
                            userField10 + "', " + createdBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ", " + 
                            lastAmendedBy + ", " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
        }

        public static string update(string databaseType, long orderId, bool flgEformsImport, long eformsImportId, bool flgPoPlaced, string poType, string orderRef, string customerRef, long supplierId, string supplierAddress,
                                    string supplierTelephone, DateTime orderDate, string addressInvoice, string addressDelivery, double orderAmount, double orderDiscountAmount, double orderShippingAmount, double orderSubtotalAmount,
                                    double orderVatAmount, double orderTotalAmount, long contactId, string vehicleReg, string additionInfo, DateTime requiredByDate, bool flgDispatchDate, DateTime dateDespatchDate, string orderedBy,
                                    string orderStatus, string userField01, string userField02, string userField03, string userField04, string userField05, string userField06, string userField07, string userField08,
                                    string userField09, string userField10, long createdBy, DateTime dateCreated, long lastAmendedBy, DateTime dateLastAmended)
        {
            string returnValue = "";
            try
            {
                
                returnValue = "UPDATE un_sub_con_po_header" +
                        "   SET flg_eforms_import = " + Utilities.GetBooleanForDML(databaseType, flgEformsImport) + ", " +
                        " eforms_import_id =  " + eformsImportId + ",  " +
                        " flg_po_placed = " + Utilities.GetBooleanForDML(databaseType, flgPoPlaced) + ",  " +
                        " po_type =  '" + poType + "',  " +
                        " order_ref =  '" + orderRef + "',  " +
                        " customer_ref =  '" + customerRef + "',  " +
                        " supplier_id =  " + supplierId + ",  " +
                        " supplier_address =  '" + supplierAddress + "',  " +
                        " supplier_telephone =  '" + supplierTelephone + "',  " +
                        " order_date =  " + Utilities.GetDateTimeForDML(databaseType, orderDate,true,true) + ",  " +
                        " address_invoice =  '" + addressInvoice + "',  " +
                        " address_delivery =  '" + addressDelivery + "',  " +
                        " order_amount =  " + orderAmount + ",  " +
                        " order_discount_amount =  " + orderDiscountAmount + ",  " +
                        " order_shipping_amount =  " + orderShippingAmount + ",  " +
                        " order_subtotal_amount =  " + orderSubtotalAmount + ",  " +
                        " order_vat_amount =  " + orderVatAmount + ",  " +
                        " order_total_amount =  " + orderTotalAmount + ",  " +
                        " contact_id =  " + contactId + ",  " +
                        " vehicle_reg =  '" + vehicleReg + "',  " +
                        " addition_info =  '" + additionInfo + "',  " +
                        " required_by_date =  " + Utilities.GetDateTimeForDML(databaseType, requiredByDate,true,true) + ",  " +
                        " flg_dispatch_date = " + Utilities.GetBooleanForDML(databaseType, flgDispatchDate) + ",  " +
                        " date_despatch_date =  " + Utilities.GetDateTimeForDML(databaseType, dateDespatchDate,true,true) + ", " +
                        " ordered_by =  '" + orderedBy + "',  " +
                        " order_status =  '" + orderStatus + "',  " +
                        " user_field_01 =  '" + userField01 + "',  " +
                        " user_field_02 =  '" + userField02 + "',  " +
                        " user_field_03 =  '" + userField03 + "',  " +
                        " user_field_04 =  '" + userField04 + "',  " +
                        " user_field_05 =  '" + userField05 + "',  " +
                        " user_field_06 =  '" + userField06 + "',  " +
                        " user_field_07 =  '" + userField07 + "',  " +
                        " user_field_08 =  '" + userField08 + "',  " +
                        " user_field_09 =  '" + userField09 + "',  " +
                        " user_field_10 =  '" + userField10 + "',  " +
                        " created_by =  " + createdBy + ",  " +
                        " date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " +
                        " last_amended_by =  " + lastAmendedBy + ",  " +
                        " date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " +
                        "  WHERE order_id = " + orderId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string delete(string databaseType, long orderId)
        {
            return @"DELETE FROM   un_sub_con_po_header
                      WHERE order_id = " + orderId;
        }

        public static string deleteFlagDeleted(string databaseType, long orderId)
        {
            return @"UPDATE un_sub_con_po_header
                        SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, true) +
                      " WHERE order_id = " + orderId;
        }

        public static string UpdatePORefAndAmounts(string databaseType, long sequence, string poRef, double poAmtMat,
                                                     double poAmtSubTotal, double poAmtTotal,
                                                     long lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"UPDATE un_sub_con_po_header 
                        SET po_ref = '" + poRef + "', " +
                    "       po_amt_mat = " + poAmtMat + ", " +
                    "       po_amt_subtotal = " + poAmtSubTotal + ", " +
                    "       po_amt_total = " + poAmtTotal + ", " +
                    "       last_amended_by = " + lastAmendedBy + ", " +
                    "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) +
                    " WHERE sequence = " + sequence;
        }
    }
}

