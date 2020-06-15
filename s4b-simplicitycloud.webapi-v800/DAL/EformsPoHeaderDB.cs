using SimplicityOnlineBLL.Entities;
using SimplicityOnlineDAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace SimplicityOnlineDAL
{
    public class EformsPoHeaderDB : MainDB
    {

        public EformsPoHeaderDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool InsertEformsPoHeader(out long sequence, bool flgDeleted, int dataType, string nfsSubmitNo, string nfsSubmitTimeStamp, string impRef,
                                         int formType, string jobRef, bool flgValidJobRef, long jobSequence, string supplierShortName,
                                         bool flgValidSupplierShortName, long supplierId, long supplierMultiAddId, string supplierEmail,
                                         string attentionOf, string nfPoRef, DateTime? datePoDate, DateTime? requiredByDate, bool flgDeliverToSite,
                                         string orderedByShortName, bool flgValidOrderedByShortName, long orderedById, string requestedByShortName,
                                         bool flgValidRequestedByShortName, long requestedById, string poAddressInvoice, string poNotes,
                                         long poVoTypeSequence, string voRef, long orderId, double orderAmount, double orderDiscountAmount,
                                         double orderShippingAmount, double orderSubtotalAmount, double orderVatAmount, double orderTotalAmount,
                                         bool flgOtherIssue, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "EformsPoHeaderDB.InsertEformsPoHeader()";
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(EformsPoHeaderQueries.insert(this.DatabaseType, flgDeleted, dataType, nfsSubmitNo, nfsSubmitTimeStamp, impRef,
                                                                      formType, jobRef, flgValidJobRef, jobSequence, supplierShortName, flgValidSupplierShortName,
                                                                      supplierId, supplierMultiAddId, supplierEmail, attentionOf, nfPoRef, datePoDate,
                                                                      requiredByDate, flgDeliverToSite, orderedByShortName, flgValidOrderedByShortName,
                                                                      orderedById, requestedByShortName, flgValidRequestedByShortName, requestedById,
                                                                      poAddressInvoice, poNotes, poVoTypeSequence, voRef, orderId, orderAmount, orderDiscountAmount,
                                                                      orderShippingAmount, orderSubtotalAmount, orderVatAmount, orderTotalAmount, flgOtherIssue,
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
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Inserting EForms PO Header.", ex);
            }
            return returnValue;
        }


        public List<EformsPoHeader> selectAllEformsPoHeaderSequence(long sequence)
        {
            const string METHOD_NAME = "EformsPoHeaderDB.selectAllEformsPoHeaderSequence()";
            List<EformsPoHeader> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(EformsPoHeaderQueries.getSelectAllBySequence(sequence), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<EformsPoHeader>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_EformsPoHeader(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Selecting EForms PO Header.", ex);
            }
            return returnValue;
        }

        public bool updateBySequence(long sequence, bool flgDeleted, int dataType, string nfsSubmitNo, string nfsSubmitTimeStamp, string impRef,
                                     int formType, string jobRef, bool flgValidJobRef, long jobSequence, string supplierShortName,
                                     bool flgValidSupplierShortName, long supplierId, long supplierMultiAddId, string supplierEmail,
                                     string attentionOf, string nfPoRef, DateTime? datePoDate, DateTime? requiredByDate, bool flgDeliverToSite,
                                     string orderedByShortName, bool flgValidOrderedByShortName, long orderedById, string requestedByShortName,
                                     bool flgValidRequestedByShortName, long requestedById, string poAddressInvoice, string poNotes,
                                     long poVoTypeSequence, string voRef, long orderId, double orderAmount, double orderDiscountAmount,
                                     double orderShippingAmount, double orderSubtotalAmount, double orderVatAmount, double orderTotalAmount,
                                     bool flgOtherIssue, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "EformsPoHeaderDB.updateBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EformsPoHeaderQueries.update(this.DatabaseType, sequence, flgDeleted, dataType, nfsSubmitNo, nfsSubmitTimeStamp, impRef,
                                                                      formType, jobRef, flgValidJobRef, jobSequence, supplierShortName, flgValidSupplierShortName,
                                                                      supplierId, supplierMultiAddId, supplierEmail, attentionOf, nfPoRef, datePoDate,
                                                                      requiredByDate, flgDeliverToSite, orderedByShortName, flgValidOrderedByShortName,
                                                                      orderedById, requestedByShortName, flgValidRequestedByShortName, requestedById,
                                                                      poAddressInvoice, poNotes, poVoTypeSequence, voRef, orderId, orderAmount, orderDiscountAmount,
                                                                      orderShippingAmount, orderSubtotalAmount, orderVatAmount, orderTotalAmount, flgOtherIssue,
                                                                      createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Updating EForms PO Header.", ex);
            }
            return returnValue;
        }

        public bool UpdateOrderIdAndAmounts(long sequence, long orderId, double orderAmount, double orderSubtotalAmount, double orderTotalAmount,
                                            bool flgOtherIssue, long lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "EformsPoHeaderDB.updateBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EformsPoHeaderQueries.UpdateOrderIdAndAmounts(this.DatabaseType, sequence, orderId, orderAmount, 
                                                                                       orderSubtotalAmount, orderTotalAmount, flgOtherIssue,
                                                                                       lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Updating EForms PO Header Order Id and Amounts.", ex);
            }
            return returnValue;
        }

        public bool deleteBySequence(long sequence)
        {
            const string METHOD_NAME = "EformsPoHeaderDB.deleteBySequence()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EformsPoHeaderQueries.Delete(sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while deleting EForms PO Header By Sequence.", ex);
            }
            return returnValue;
        }

        public bool deleteByFlgDeleted(long sequence)
        {
            const string METHOD_NAME = "EformsPoHeaderDB.deleteByFlgDeleted()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(EformsPoHeaderQueries.DeleteByFlagDeleted(this.DatabaseType, sequence), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Loading EForms PO Header.", ex);
            }
            return returnValue;
        }

        private EformsPoHeader Load_EformsPoHeader(OleDbDataReader dr)
        {
            const string METHOD_NAME = "EformsPoHeaderDB.Load_EformsPoHeader()";
            EformsPoHeader eformsPoHeader = null;
            try
            {
                if (dr != null)
                {
                    eformsPoHeader = new EformsPoHeader();
                    eformsPoHeader.Sequence = long.Parse(dr["sequence"].ToString());
                    eformsPoHeader.FlgDeleted = bool.Parse(dr["flg_deleted"].ToString());
                    eformsPoHeader.DataType = DBUtil.GetIntValue(dr, "data_type");
                    eformsPoHeader.NfsSubmitNo = DBUtil.GetStringValue(dr, "nfs_submit_no");
                    eformsPoHeader.NfsSubmitTimeStamp = DBUtil.GetStringValue(dr, "nfs_submit_time_stamp");
                    eformsPoHeader.ImpRef = DBUtil.GetStringValue(dr, "imp_ref");
                    eformsPoHeader.FormType = DBUtil.GetIntValue(dr, "form_type");
                    eformsPoHeader.JobRef = DBUtil.GetStringValue(dr, "job_ref");
                    eformsPoHeader.FlgValidJobRef = DBUtil.GetBooleanValue(dr, "flg_valid_job_ref");
                    eformsPoHeader.JobSequence = long.Parse(dr["job_sequence"].ToString());
                    eformsPoHeader.SupplierShortName = DBUtil.GetStringValue(dr, "supplier_short_name");
                    eformsPoHeader.FlgValidSupplierShortName = bool.Parse(dr["flg_valid_supplier_short_name"].ToString());
                    eformsPoHeader.SupplierId = long.Parse(dr["supplier_id"].ToString());
                    eformsPoHeader.SupplierMultiAddId = long.Parse(dr["supplier_multi_add_id"].ToString());
                    eformsPoHeader.SupplierEmail = DBUtil.GetStringValue(dr, "supplier_email");
                    eformsPoHeader.AttentionOf = DBUtil.GetStringValue(dr, "attention_of");
                    eformsPoHeader.NfPoRef = DBUtil.GetStringValue(dr, "nf_po_ref");
                    eformsPoHeader.DatePoDate = DBUtil.GetDateTimeValue(dr, "date_po_date");
                    eformsPoHeader.RequiredByDate = DBUtil.GetDateTimeValue(dr, "required_by_date");
                    eformsPoHeader.FlgDeliverToSite = bool.Parse(dr["flg_deliver_to_site"].ToString());
                    eformsPoHeader.OrderedByShortName = DBUtil.GetStringValue(dr, "ordered_by_short_name");
                    eformsPoHeader.FlgValidOrderedByShortName = bool.Parse(dr["flg_valid_ordered_by_short_name"].ToString());
                    eformsPoHeader.OrderedById = long.Parse(dr["ordered_by_id"].ToString());
                    eformsPoHeader.RequestedByShortName = DBUtil.GetStringValue(dr, "requested_by_short_name");
                    eformsPoHeader.FlgValidRequestedByShortName = bool.Parse(dr["flg_valid_requested_by_short_name"].ToString());
                    eformsPoHeader.RequestedById = long.Parse(dr["requested_by_id"].ToString());
                    eformsPoHeader.PoAddressInvoice = DBUtil.GetStringValue(dr, "po_address_invoice");
                    eformsPoHeader.PoNotes = DBUtil.GetStringValue(dr, "po_notes");
                    eformsPoHeader.PoVoTypeSequence = long.Parse(dr["po_vo_type_sequence"].ToString());
                    eformsPoHeader.VoRef = DBUtil.GetStringValue(dr, "vo_ref");
                    eformsPoHeader.OrderId = long.Parse(dr["order_id"].ToString());
                    eformsPoHeader.FlgOtherIssue = bool.Parse(dr["flg_other_issue"].ToString());
                    eformsPoHeader.CreatedBy = long.Parse(dr["created_by"].ToString());
                    eformsPoHeader.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    eformsPoHeader.LastAmendedBy = long.Parse(dr["last_amended_by"].ToString());
                    eformsPoHeader.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Loading EForms PO Header.", ex);
            }
            return eformsPoHeader;
        }
    }
}