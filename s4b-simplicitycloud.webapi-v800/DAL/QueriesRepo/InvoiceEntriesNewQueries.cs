using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.DAL.QueriesRepo
{
    public static class InvoiceEntriesNewQueries
    {

        public static string getSelectAllBySequence(long Sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                              "  FROM    un_invoice_entries_new" +
                              " WHERE sequence = " + Sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string selectByInvoiceNoTransTypeAndEntryType(string invoiceNo, string transType, string entryType)
        {
            string returnValue = "";
            try
            {
                returnValue = "SELECT * " +
                              "  FROM    un_invoice_entries_new " +
                              " WHERE invoiceno_or_itemref = '" + invoiceNo + "' " +
                              "   AND trans_type = '" + transType + "' " +
                              "   AND entry_type = '" + entryType + "' ";
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string insert(string databaseType, InvoiceEntriesNew invoiceEntry)
        {
            string returnValue = "";
            try
            {
                returnValue = @"INSERT INTO   un_invoice_entries_new(flg_cancelled,  flg_sys_entry,  trans_type,  entry_type,  invoiceno_or_itemref,
                                bill_sequence,  job_sequence,  contact_id,  entity_sub_id,  entry_date,  entry_amt_or_mat,
                                entry_amt_labour,  entry_amt_discounted,  entry_amt_subtotal,  flg_add_vat,  entry_amt_vat,
                                flg_tax_required,  entry_tax_rate,  entry_amt_tax,  entry_amt_total_mat,  entry_amt_total_labour,
                                entry_amt_total,  entry_details,  entry_voucher_ref,  entry_extra_info,  flg_card_details,
                                card_details_id,  flg_card_payee_is_entity,  sage_id,  entry_amt_allocated,
                                entry_amt_allocated_labour,  flg_settled,  flg_sub_entry_row,  sub_entry_join_dr_sequence,
                                sub_entry_join_cr_sequence,  sub_entry_amt,  sub_entry_details,  created_by,  date_created,
                                last_amended_by,  date_last_amended )
                              VALUES (" + Utilities.GetBooleanForDML(databaseType, invoiceEntry.FlgCancelled) + ",   " + Utilities.GetBooleanForDML(databaseType, invoiceEntry.FlgSysEntry) 
                              + ",   '" + invoiceEntry.TransType + "',   '" + invoiceEntry.EntryType + "'"
                                        +",   '" + invoiceEntry.InvoicenoOrItemref + "', "+ invoiceEntry.BillSequence + ",   " + invoiceEntry.JobSequence
                                        + " ," + invoiceEntry.ContactId + ",   '" + invoiceEntry.EntitySubId
                                        + "', " + Utilities.GetDateTimeForDML(databaseType, invoiceEntry.EntryDate, true, false) 
                                        + ",   " + invoiceEntry.EntryAmtOrMat + ",   " + invoiceEntry.EntryAmtLabour + ",   " + invoiceEntry.EntryAmtDiscounted 
                                        + ",   " + invoiceEntry.EntryAmtSubtotal + ",   " + Utilities.GetBooleanForDML(databaseType, invoiceEntry.FlgAddVat) + ",   " + invoiceEntry.EntryAmtVat 
                                        + ",   " + Utilities.GetBooleanForDML(databaseType, invoiceEntry.FlgTaxRequired) + ",   " + invoiceEntry.EntryTaxRate + ",   " + invoiceEntry.EntryAmtTax 
                                        + ",   " + invoiceEntry.EntryAmtTotalMat + ",   " + invoiceEntry.EntryAmtTotalLabour + ",   " + invoiceEntry.EntryAmtTotal 
                                        + ",  ' " + invoiceEntry.EntryDetails + "',   '" + invoiceEntry.EntryVoucherRef + "',   '" + invoiceEntry.EntryExtraInfo + "'" 
                                        + ",   " + Utilities.GetBooleanForDML(databaseType, invoiceEntry.FlgCardDetails) + ",   '" + invoiceEntry.CardDetailsId + "',   " + Utilities.GetBooleanForDML(databaseType, invoiceEntry.FlgCardPayeeIsEntity) 
                                        + ",   '" + invoiceEntry.SageId + "',   " + invoiceEntry.EntryAmtAllocated + ",   " + invoiceEntry.EntryAmtAllocatedLabour 
                                        + ",   " + Utilities.GetBooleanForDML(databaseType, invoiceEntry.FlgSettled) + ",   " + Utilities.GetBooleanForDML(databaseType, invoiceEntry.FlgSubEntryRow) + ",   '" + invoiceEntry.SubEntryJoinDrSequence 
                                        + "',   '" + invoiceEntry.SubEntryJoinCrSequence + "',   " + invoiceEntry.SubEntryAmt + ",   '" + invoiceEntry.SubEntryDetails + "'" 
                                        + ",   " + invoiceEntry.CreatedBy + ",   " + Utilities.GetDateTimeForDML(databaseType, invoiceEntry.DateCreated,true, true) 
                                        + ",   '" + invoiceEntry.LastAmendedBy + "',   " + Utilities.GetDateTimeForDML(databaseType, invoiceEntry.DateLastAmended,true, true) + ")";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public static string update(string databaseType,long sequence, bool flgCancelled, bool flgSysEntry, string transType, string entryType, string invoicenoOrItemref,
                                    long billSequence, long jobSequence, long contactId, long entitySubId, string entryDate,
                                    double entryAmtOrMat, double entryAmtLabour, double entryAmtDiscounted, double entryAmtSubtotal,
                                    bool flgAddVat, double entryAmtVat, bool flgTaxRequired, object entryTaxRate, double entryAmtTax,
                                    double entryAmtTotalMat, double entryAmtTotalLabour, double entryAmtTotal, string entryDetails,
                                    string entryVoucherRef, string entryExtraInfo, bool flgCardDetails, long cardDetailsId,
                                    bool flgCardPayeeIsEntity, long sageId, double entryAmtAllocated, double entryAmtAllocatedLabour,
                                    bool flgSettled, bool flgSubEntryRow, long subEntryJoinDrSequence, long subEntryJoinCrSequence,
                                    double subEntryAmt, string subEntryDetails, long createdBy, DateTime? dateCreated,
                                    long lastAmendedBy, DateTime? dateLastAmended)
        {
            string returnValue = "";
            try
            {
                returnValue = "UPDATE   un_invoice_entries_new" +
                                 "   SET  flg_cancelled = " + Utilities.GetBooleanForDML(databaseType, flgCancelled) + ",  " +
                                 "        flg_sys_entry = " + Utilities.GetBooleanForDML(databaseType, flgSysEntry) + ",  " +
                                 "        trans_type =  '" + transType + "',  " +
                                 "        entry_type =  '" + entryType + "',  " +
                                 "        invoiceno_or_itemref =  '" + invoicenoOrItemref + "',  " +
                                 "        bill_sequence =  " + billSequence + ",  " +
                                 "        job_sequence =  " + jobSequence + ",  " +
                                 "        contact_id =  " + contactId + ",  " +
                                 "        entity_sub_id =  " + entitySubId + ",  " +
                                 "        entry_date =  '" + entryDate + "',  " +
                                 "        entry_amt_or_mat =  " + entryAmtOrMat + ",  " +
                                 "        entry_amt_labour =  " + entryAmtLabour + ",  " +
                                 "        entry_amt_discounted =  " + entryAmtDiscounted + ",  " +
                                 "        entry_amt_subtotal =  " + entryAmtSubtotal + ",  " +
                                 "        flg_add_vat = " + Utilities.GetBooleanForDML(databaseType, flgAddVat) + ",  " +
                                 "        entry_amt_vat =  " + entryAmtVat + ",  " +
                                 "        flg_tax_required = " + Utilities.GetBooleanForDML(databaseType, flgTaxRequired) + ",  " +
                                 "        entry_tax_rate =  " + entryTaxRate + ",  " +
                                 "        entry_amt_tax =  " + entryAmtTax + ",  " +
                                 "        entry_amt_total_mat =  " + entryAmtTotalMat + ",  " +
                                 "        entry_amt_total_labour =  " + entryAmtTotalLabour + ",  " +
                                 "        entry_amt_total =  " + entryAmtTotal + ",  " +
                                 "        entry_details =  '" + entryDetails + "',  " +
                                 "        entry_voucher_ref =  '" + entryVoucherRef + "',  " +
                                 "        entry_extra_info =  '" + entryExtraInfo + "',  " +
                                 "        flg_card_details = " + Utilities.GetBooleanForDML(databaseType, flgCardDetails) + ",  " +
                                 "        card_details_id =  " + cardDetailsId + ",  " +
                                 "        flg_card_payee_is_entity = " + Utilities.GetBooleanForDML(databaseType, flgCardPayeeIsEntity) + ",  " +
                                 "        sage_id =  " + sageId + ",  " +
                                 "        entry_amt_allocated =  " + entryAmtAllocated + ",  " +
                                 "        entry_amt_allocated_labour =  " + entryAmtAllocatedLabour + ",  " +
                                 "        flg_settled = " + Utilities.GetBooleanForDML(databaseType, flgSettled) + ",  " +
                                 "        flg_sub_entry_row = " + Utilities.GetBooleanForDML(databaseType, flgSubEntryRow) + ",  " +
                                 "        sub_entry_join_dr_sequence =  " + subEntryJoinDrSequence + ",  " +
                                 "        sub_entry_join_cr_sequence =  " + subEntryJoinCrSequence + ",  " +
                                 "        sub_entry_amt =  " + subEntryAmt + ",  " +
                                 "        sub_entry_details =  '" + subEntryDetails + "',  " +
                                 "        created_by =  " + createdBy + ",  " +
                                 "        date_created =  " + Utilities.GetDateTimeForDML(databaseType, dateCreated,true,true) + ", " +
                                 "        last_amended_by =  " + lastAmendedBy + ",  " +
                                 "        date_last_amended =  " + Utilities.GetDateTimeForDML(databaseType, dateLastAmended,true,true) + ", " +
                                 "  WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }


        public static string delete(long sequence)
        {
            string returnValue = "";
            try
            {
                returnValue = "DELETE FROM   un_invoice_entries_new" +
                              "WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string deleteFlagDeleted(string databaseType, long sequence)
        {
            string returnValue = "";
            try
            {
                bool flg = true;
                returnValue = "UPDATE   un_invoice_entries_new" +
                              "   SET flg_deleted =  " + Utilities.GetBooleanForDML(databaseType,flg) + ", " +
                              " WHERE sequence = " + sequence;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public static string SelectByInvoiceNo(string invoiceNo)
        {
            return @"SELECT * FROM un_invoice_entries_new 
                      WHERE invoiceno_or_itemref = '" + invoiceNo + "'";
        }
    }
}

