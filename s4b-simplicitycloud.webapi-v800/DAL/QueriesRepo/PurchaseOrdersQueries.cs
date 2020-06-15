using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineDAL.QueriesRepo
{
    public static class PurchaseOrdersQueries
    {

        public static string getSelectAllByorderId(string databaseType, long orderId)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "SELECT * " +
                                      "  FROM    un_purchase_orders" +
                                      " WHERE order_id = " + orderId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, bool flgEformsImport, long eformsImportId, bool flgPoPlaced, int poType, 
                                    string orderRef, string customerRef, long supplierId, string supplierAddress, string supplierTelephone, 
                                    DateTime? orderDate, string addressInvoice, string addressDelivery, double orderAmount, double orderDiscountAmount, 
                                    double orderShippingAmount, double orderSubtotalAmount, double orderVatAmount, double orderTotalAmount, 
                                    long contactId, string vehicleReg, string additionInfo, DateTime? requiredByDate, bool flgDispatchDate, 
                                    DateTime? dateDespatchDate, string orderedBy, int orderStatus, string userField01, string userField02, 
                                    string userField03, string userField04, string userField05, string userField06, string userField07, 
                                    string userField08, string userField09, string userField10, long createdBy, DateTime? dateCreated, 
                                    long lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"INSERT INTO un_purchase_orders(flg_eforms_import, eforms_import_id, flg_po_placed, po_type, order_ref,  
                            customer_ref, supplier_id, supplier_address,  supplier_telephone, order_date, address_invoice, address_delivery, 
                            order_amount, order_discount_amount, order_shipping_amount, order_subtotal_amount, order_vat_amount, 
                            order_total_amount, contact_id, vehicle_reg, addition_info, required_by_date, flg_dispatch_date, date_despatch_date, 
                            ordered_by, order_status, user_field_01, user_field_02, user_field_03, user_field_04, user_field_05, user_field_06, 
                            user_field_07,  user_field_08,  user_field_09,  user_field_10, created_by, date_created, 
                            last_amended_by, date_last_amended)
                    VALUES(" + Utilities.GetBooleanForDML(databaseType, flgEformsImport) + ", " + eformsImportId + ", " +
                            Utilities.GetBooleanForDML(databaseType, flgPoPlaced) + ", " + poType + ", '" + orderRef + "', '" + customerRef + "', " +
                            supplierId + ", '" + supplierAddress + "', '" + supplierTelephone + "', " + 
                            Utilities.GetDateTimeForDML(databaseType, orderDate, true, true) + ", '" + addressInvoice + "', '" +
                            addressDelivery + "', " + orderAmount + ", " + orderDiscountAmount + ", " + orderShippingAmount + ", " + 
                            orderSubtotalAmount + ", " + orderVatAmount + ", " + orderTotalAmount + ", " + contactId + ", '" + vehicleReg + "', '" +
                            additionInfo + "', " + Utilities.GetDateTimeForDML(databaseType, requiredByDate, true, true) + ", " +
                            Utilities.GetBooleanForDML(databaseType, flgDispatchDate) + ", " + 
                            Utilities.GetDateTimeForDML(databaseType, dateDespatchDate, true, true) + ", '" + orderedBy + "', " + 
                            orderStatus + ", '" + userField01 + "', '" + userField02 + "', '" + userField03 + "', '" + userField04 + "', '" + 
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
               
                   
                        returnValue = "UPDATE   un_purchase_orders" +
                                      "   SET   flg_eforms_import = " + Utilities.GetBooleanForDML(databaseType, flgEformsImport) + ",  " +
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
                                      " date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true)  +
                                      "  WHERE order_id = " + orderId;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string delete(string databaseType, long orderId)
        {
            string returnValue = "";
            try
            {
               
                        returnValue = "DELETE FROM   un_purchase_orders" +
                               "WHERE order_id = " + orderId;
            }

            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string deleteFlagDeleted(string databaseType, long orderId)
        {
            string returnValue = "";
            try
            {
               
                        bool flg = true;
                        returnValue = "UPDATE   un_purchase_orders" +
                                  "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType, flg)  +
                                  " WHERE order_id = " + orderId;

            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
    }
}

