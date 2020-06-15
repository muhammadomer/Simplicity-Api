using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineWebApi.DAL
{
    public class InvoiceEntriesNewDB : MainDB
    {

        public InvoiceEntriesNewDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool insertInvoiceEntriesNew(InvoiceEntriesNew obj)
        {
            bool returnValue = false;
            long sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(InvoiceEntriesNewQueries.insert(this.DatabaseType, obj), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public List<InvoiceEntriesNew> selectByInvoiceNoTransTypeAndEntryType(string invoiceNo, string transType, string entryType)
        {
            List<InvoiceEntriesNew> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(InvoiceEntriesNewQueries.selectByInvoiceNoTransTypeAndEntryType(invoiceNo, transType, entryType), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<InvoiceEntriesNew>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_InvoiceEntriesNew(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public List<InvoiceEntriesNew> selectAllInvoiceEntriesNewSequence(long sequence)
        {
            List<InvoiceEntriesNew> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(InvoiceEntriesNewQueries.getSelectAllBySequence(sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<InvoiceEntriesNew>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_InvoiceEntriesNew(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool updateBySequence(long sequence, bool flgCancelled, bool flgSysEntry, string transType, string entryType, string invoicenoOrItemref,
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
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(InvoiceEntriesNewQueries.update(this.DatabaseType, sequence, flgCancelled, flgSysEntry, transType, entryType, invoicenoOrItemref,
                                                                        billSequence, jobSequence, contactId, entitySubId, entryDate, entryAmtOrMat,
                                                                        entryAmtLabour, entryAmtDiscounted, entryAmtSubtotal, flgAddVat, entryAmtVat,
                                                                        flgTaxRequired, entryTaxRate, entryAmtTax, entryAmtTotalMat, entryAmtTotalLabour,
                                                                        entryAmtTotal, entryDetails, entryVoucherRef, entryExtraInfo, flgCardDetails,
                                                                        cardDetailsId, flgCardPayeeIsEntity, sageId, entryAmtAllocated,
                                                                        entryAmtAllocatedLabour, flgSettled, flgSubEntryRow, subEntryJoinDrSequence,
                                                                        subEntryJoinCrSequence, subEntryAmt, subEntryDetails, createdBy, dateCreated,
                                                                        lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +  ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool deleteBySequence(long sequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(InvoiceEntriesNewQueries.delete(sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " + ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }

        public bool deleteByFlgDeleted(long sequence)
        {
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(InvoiceEntriesNewQueries.deleteFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = "Error occured while inserting into audit downloads " + ex.Message + " " +ex.InnerException;
                // Requires Logging
            }
            return returnValue;
        }
        private InvoiceEntriesNew Load_InvoiceEntriesNew(OleDbDataReader dr)
        {
            const string METHOD_NAME = "InvoiceEntriesNewDB.Load_InvoiceEntriesNew()";
            InvoiceEntriesNew invoiceEntriesNew = null;
            try
            {
                if (dr != null)
                {
                    invoiceEntriesNew = new InvoiceEntriesNew();
                    invoiceEntriesNew.Sequence = long.Parse(dr["sequence"].ToString());
                    invoiceEntriesNew.FlgCancelled = bool.Parse(dr["flg_cancelled"].ToString());
                    invoiceEntriesNew.FlgSysEntry = bool.Parse(dr["flg_sys_entry"].ToString());
                    invoiceEntriesNew.TransType = Utilities.GetDBString(dr["trans_type"]);
                    invoiceEntriesNew.EntryType = Utilities.GetDBString(dr["entry_type"]);
                    invoiceEntriesNew.InvoicenoOrItemref = Utilities.GetDBString(dr["invoiceno_or_itemref"]);
                    invoiceEntriesNew.BillSequence = long.Parse(dr["bill_sequence"].ToString());
                    invoiceEntriesNew.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    invoiceEntriesNew.ContactId = long.Parse(dr["contact_id"].ToString());
                    invoiceEntriesNew.EntitySubId = long.Parse(dr["entity_sub_id"].ToString());
                    //invoiceEntriesNew.EntryDate = Utilities.GetDateTimeForDML(this.DatabaseType, Convert.ToDateTime( dr["entry_date"].ToString()),true,true);
                    invoiceEntriesNew.FlgAddVat = bool.Parse(dr["flg_add_vat"].ToString());
                    invoiceEntriesNew.FlgTaxRequired = bool.Parse(dr["flg_tax_required"].ToString());
                    invoiceEntriesNew.EntryTaxRate = Convert.ToDouble(dr["entry_tax_rate"].ToString());
                    invoiceEntriesNew.EntryDetails = Utilities.GetDBString(dr["entry_details"]);
                    invoiceEntriesNew.EntryVoucherRef = Utilities.GetDBString(dr["entry_voucher_ref"]);
                    invoiceEntriesNew.EntryExtraInfo = Utilities.GetDBString(dr["entry_extra_info"]);
                    invoiceEntriesNew.FlgCardDetails = bool.Parse(dr["flg_card_details"].ToString());
                    invoiceEntriesNew.CardDetailsId = long.Parse(dr["card_details_id"].ToString());
                    invoiceEntriesNew.FlgCardPayeeIsEntity = bool.Parse(dr["flg_card_payee_is_entity"].ToString());
                    invoiceEntriesNew.SageId = long.Parse(dr["sage_id"].ToString());
                    invoiceEntriesNew.FlgSettled = bool.Parse(dr["flg_settled"].ToString());
                    invoiceEntriesNew.FlgSubEntryRow = bool.Parse(dr["flg_sub_entry_row"].ToString());
                    invoiceEntriesNew.SubEntryJoinDrSequence = long.Parse(dr["sub_entry_join_dr_sequence"].ToString());
                    invoiceEntriesNew.SubEntryJoinCrSequence = long.Parse(dr["sub_entry_join_cr_sequence"].ToString());
                    invoiceEntriesNew.SubEntryDetails = Utilities.GetDBString(dr["sub_entry_details"]);
                    invoiceEntriesNew.CreatedBy = long.Parse(dr["created_by"].ToString());
                    invoiceEntriesNew.DateCreated = Utilities.getDBDate(dr["date_created"]);
                    invoiceEntriesNew.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    invoiceEntriesNew.DateLastAmended = Utilities.getDBDate(dr["date_last_amended"]);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Loading Invoice Entries New.", ex);
            }
            return invoiceEntriesNew;
        }

        public InvoiceEntriesNew GetInvoiceEntriesNewByInvoiceNo(string invoiceNo)
        {
            const string METHOD_NAME = "InvoiceEntriesNewDB.GetInvoiceEntriesNewByInvoiceNo()";
            InvoiceEntriesNew returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(InvoiceEntriesNewQueries.SelectByInvoiceNo(invoiceNo), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new InvoiceEntriesNew();
                                dr.Read();
                                returnValue = Load_InvoiceEntriesNew(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Getting Invoice Entries New By Invoice No.", ex);
            }
            return returnValue;
        }
    }
}
