using SimplicityOnlineWebApi.Commons;
using System;

namespace SimplicityOnlineDAL.QueriesRepo
{
    public static class EformsPoHeaderQueries
    {

        public static string getSelectAllBySequence(long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                    "  FROM un_eforms_po_header " +
                    " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, bool flgDeleted, object dataType, string nfsSubmitNo, string nfsSubmitTimeStamp, string impRef, 
                                    int formType, string jobRef, bool flgValidJobRef, long jobSequence, string supplierShortName, 
                                    bool flgValidSupplierShortName, long supplierId, long supplierMultiAddId, string supplierEmail, 
                                    string attentionOf, string nfPoRef, DateTime? datePoDate, DateTime? requiredByDate, bool flgDeliverToSite, 
                                    string orderedByShortName, bool flgValidOrderedByShortName, long orderedById, string requestedByShortName,
                                    bool flgValidRequestedByShortName, long requestedById, string poAddressInvoice, string poNotes, 
                                    long poVoTypeSequence, string voRef, long orderId, double orderAmount, double orderDiscountAmount, 
                                    double orderShippingAmount, double orderSubtotalAmount, double orderVatAmount, double orderTotalAmount,
                                    bool flgOtherIssue, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"INSERT INTO  un_eforms_po_header(flg_deleted,  data_type,  nfs_submit_no,  nfs_submit_time_stamp,  imp_ref,
                           form_type,  job_ref,  flg_valid_job_ref,  job_sequence,  supplier_short_name, 
                           flg_valid_supplier_short_name,  supplier_id,  supplier_multi_add_id,  supplier_email, 
                           attention_of,  nf_po_ref,  date_po_date,  required_by_date,  flg_deliver_to_site,
                           ordered_by_short_name,  flg_valid_ordered_by_short_name,  ordered_by_id,  requested_by_short_name,
                           flg_valid_requested_by_short_name,  requested_by_id,  po_address_invoice,  po_notes,
                           po_vo_type_sequence,  vo_ref,  order_id,  order_amount,  order_discount_amount, 
                           order_shipping_amount,  order_subtotal_amount,  order_vat_amount,  order_total_amount,
                           flg_other_issue,  created_by,  date_created,  last_amended_by,  date_last_amended)
                    VALUES (" + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ", " + dataType + ", '" + 
                           Utilities.replaceSpecialCharsForInsert(nfsSubmitNo) + "', '" + Utilities.replaceSpecialCharsForInsert(nfsSubmitTimeStamp) + 
                          "', '" + Utilities.replaceSpecialCharsForInsert(impRef) + "', " + formType + ", '" +
                           Utilities.replaceSpecialCharsForInsert(jobRef) + "', " + Utilities.GetBooleanForDML(databaseType, flgValidJobRef) + ", " + jobSequence + ", '" + Utilities.replaceSpecialCharsForInsert(supplierShortName) + 
                          "', " + Utilities.GetBooleanForDML(databaseType, flgValidSupplierShortName) + ", " + supplierId + ", " + supplierMultiAddId + ", '" +
                           Utilities.replaceSpecialCharsForInsert(supplierEmail) + "', '" + Utilities.replaceSpecialCharsForInsert(attentionOf) + 
                          "', '" + Utilities.replaceSpecialCharsForInsert(nfPoRef) + "', " + 
                           Utilities.GetDateTimeForDML(databaseType, datePoDate, true, true) + ", " + Utilities.GetDateTimeForDML(databaseType, requiredByDate,true,true) + ", " + Utilities.GetBooleanForDML(databaseType, flgDeliverToSite) + ", '" +
                           Utilities.replaceSpecialCharsForInsert(orderedByShortName) + "', " + Utilities.GetBooleanForDML(databaseType, flgValidOrderedByShortName) + ", " + orderedById + 
                          ", '" + Utilities.replaceSpecialCharsForInsert(requestedByShortName) + "', " + Utilities.GetBooleanForDML(databaseType, flgValidRequestedByShortName) + ", " +
                           requestedById + ", '" + Utilities.replaceSpecialCharsForInsert(poAddressInvoice) + "', '" + Utilities.replaceSpecialCharsForInsert(poNotes) + 
                          "', " + poVoTypeSequence + ", '" + Utilities.replaceSpecialCharsForInsert(voRef) + "', " + orderId + ", " +
                           orderAmount + ",   " + orderDiscountAmount + ",   " + orderShippingAmount + ",   " + orderSubtotalAmount + ", " + 
                           orderVatAmount + ", " + orderTotalAmount + ", " + Utilities.GetBooleanForDML(databaseType, flgOtherIssue) + ", " + createdBy + ", " + 
                           Utilities.GetDateTimeForDML(databaseType, dateCreated, true, true) + ",   " + lastAmendedBy + ", " +
                           Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) + ")";
        }

        public static string update(string databaseType, long sequence, bool flgDeleted, int dataType, string nfsSubmitNo, string nfsSubmitTimeStamp, string impRef,
                                    int formType, string jobRef, bool flgValidJobRef, long jobSequence, string supplierShortName,
                                    bool flgValidSupplierShortName, long supplierId, long supplierMultiAddId, string supplierEmail,
                                    string attentionOf, string nfPoRef, DateTime? datePoDate, DateTime? requiredByDate, bool flgDeliverToSite,
                                    string orderedByShortName, bool flgValidOrderedByShortName, long orderedById, string requestedByShortName,
                                    bool flgValidRequestedByShortName, long requestedById, string poAddressInvoice, string poNotes,
                                    long poVoTypeSequence, string voRef, long orderId, double orderAmount, double orderDiscountAmount,
                                    double orderShippingAmount, double orderSubtotalAmount, double orderVatAmount, double orderTotalAmount,
                                    bool flgOtherIssue, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_eforms_po_header" +
                                 "   SET  flg_deleted = " + Utilities.GetBooleanForDML(databaseType, flgDeleted) + ",  " +
                                 "        data_type =  " + dataType + ",  " +
                                 "        nfs_submit_no =  '" + nfsSubmitNo + "',  " +
                                 "        nfs_submit_time_stamp =  '" + nfsSubmitTimeStamp + "',  " +
                                 "        imp_ref =  '" + impRef + "',  " +
                                 "        form_type =  " + formType + ",  " +
                                 "        job_ref =  '" + jobRef + "',  " +
                                 "        flg_valid_job_ref = " + Utilities.GetBooleanForDML(databaseType, flgValidJobRef) + ",  " +
                                 "        job_sequence =  " + jobSequence + ",  " +
                                 "        supplier_short_name =  '" + supplierShortName + "',  " +
                                 "        flg_valid_supplier_short_name = " + Utilities.GetBooleanForDML(databaseType, flgValidSupplierShortName) + ",  " +
                                 "        supplier_id =  " + supplierId + ",  " +
                                 "        supplier_multi_add_id =  " + supplierMultiAddId + ",  " +
                                 "        supplier_email =  '" + supplierEmail + "',  " +
                                 "        attention_of =  '" + attentionOf + "',  " +
                                 "        nf_po_ref =  '" + nfPoRef + "',  " +
                                 "        date_po_date =  " + Utilities.GetDateTimeForDML(databaseType, datePoDate,true,true) + ", " +
                                 "        required_by_date =  " + Utilities.GetDateTimeForDML(databaseType, requiredByDate,true,true) + ",  " +
                                 "        flg_deliver_to_site = " + Utilities.GetBooleanForDML(databaseType, flgDeliverToSite) + ",  " +
                                 "        ordered_by_short_name =  '" + orderedByShortName + "',  " +
                                 "        flg_valid_ordered_by_short_name = " + Utilities.GetBooleanForDML(databaseType, flgValidOrderedByShortName) + ",  " +
                                 "        ordered_by_id =  " + orderedById + ",  " +
                                 "        requested_by_short_name =  '" + requestedByShortName + "',  " +
                                 "        flg_valid_requested_by_short_name = " + Utilities.GetBooleanForDML(databaseType, flgValidRequestedByShortName) + ",  " +
                                 "        requested_by_id =  " + requestedById + ",  " +
                                 "        po_address_invoice = ' " + poAddressInvoice + "',  " +
                                 "        po_notes =  '" + poNotes + "',  " +
                                 "        po_vo_type_sequence =  " + poVoTypeSequence + ",  " +
                                 "        vo_ref =  '" + voRef + "',  " +
                                 "        order_id =  " + orderId + ",  " +
                                 "        order_amount =  " + orderAmount + ",  " +
                                 "        order_discount_amount =  " + orderDiscountAmount + ",  " +
                                 "        order_shipping_amount =  " + orderShippingAmount + ",  " +
                                 "        order_subtotal_amount =  " + orderSubtotalAmount + ",  " +
                                 "        order_vat_amount =  " + orderVatAmount + ",  " +
                                 "        order_total_amount =  " + orderTotalAmount + ",  " +
                                 "        flg_other_issue = " + Utilities.GetBooleanForDML(databaseType, flgOtherIssue) + ",  " +
                                 "        created_by =  " + createdBy + ",  " +
                                 "        date_created =  " + Utilities.getSQLDate(dateCreated) + ", " +
                                 "        last_amended_by =  " + lastAmendedBy + ",  " +
                                 "        date_last_amended =  " + Utilities.getSQLDate(dateLastAmended) + ", " +
                                 "  WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string UpdateOrderIdAndAmounts(string databaseType, long sequence, long orderId, double orderAmount, 
                                                     double orderSubtotalAmount, double orderTotalAmount,
                                                     bool flgOtherIssue, long lastAmendedBy, DateTime? dateLastAmended)
        {
            return @"UPDATE un_eforms_po_header 
                        SET order_id = " + orderId + ", " +
                    "       order_amount = " + orderAmount + ", " +
                    "       order_subtotal_amount = " + orderSubtotalAmount + ", " +
                    "       order_total_amount = " + orderTotalAmount + ", " +
                    "       flg_other_issue = " + Utilities.GetBooleanForDML(databaseType, flgOtherIssue) + ", " +
                    "       last_amended_by = " + lastAmendedBy + ", " +
                    "       date_last_amended = " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended, true, true) +
                    " WHERE sequence = " + sequence;
        }

        public static string Delete(long sequence)
        {
            return @"DELETE FROM un_eforms_po_header
                      WHERE sequence = " + sequence;
        }

        public static string DeleteByFlagDeleted(string databaseType, long sequence)
        {
            return @"UPDATE un_eforms_po_header
                        SET flg_deleted = " + Utilities.GetBooleanForDML(databaseType, true)  +
                     " WHERE sequence = " + sequence;
        }
    }
}

