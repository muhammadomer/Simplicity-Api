using SimplicityOnlineBLL.Entities;
using SimplicityOnlineDAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineDAL
{
    public class SubConPoHeaderDB : MainDB
    { 

        public SubConPoHeaderDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool Insert(out long sequence, bool flgEformsImport, long eformsImportId, bool flgPoPlaced, int poType, string poRef, string poCustomerRef,
                           long jobSequence, long entityId, string entityAddress, string entityTelephone, DateTime? poDate, string addressInvoice, 
                           string addressDelivery, double poAmtMat, double poAmtLabour, double poAmtDiscount, double poAmtShipping, double poAmtSubtotal, 
                           double poAmtVAT, double poAmtTotal, long requestedId, string poVehicleReg, string poNotes, DateTime? requiredByDate, 
                           bool flgDispatchDate, DateTime? dateDespatchDate, string poOrderedBy, int poStatus, string userField01, string userField02, string userField03,
                           string userField04, string userField05, string userField06, string userField07, string userField08, string userField09,
                           string userField10, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "SubConPoHeaderDB.Insert()";
            bool returnValue = false;
            sequence = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(SubConPoHeaderQueries.insert(this.DatabaseType, flgEformsImport, eformsImportId, flgPoPlaced, poType, poRef,
                                                                      poCustomerRef, jobSequence, entityId, entityAddress, entityTelephone, poDate, 
                                                                      addressInvoice, addressDelivery, poAmtMat, poAmtLabour, poAmtDiscount, 
                                                                      poAmtShipping, poAmtSubtotal, poAmtVAT, poAmtTotal, requestedId, poVehicleReg,
                                                                      poNotes, requiredByDate, flgDispatchDate, dateDespatchDate, poOrderedBy, 
                                                                      poStatus, userField01, userField02, userField03, userField04, userField05, 
                                                                      userField06, userField07, userField08, userField09, userField10, createdBy, 
                                                                      dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        sequence = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Inserting Sub Con Header.", ex);
            }
            return returnValue;
        }

        public List<SubConPoHeader> GetSubConPoHeaderByOrderId(long orderId)
        {
            const string METHOD_NAME = "SubConPoHeaderDB.GetSubConPoHeaderByOrderId()";
            List<SubConPoHeader> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(SubConPoHeaderQueries.getSelectAllByorderId(this.DatabaseType, orderId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<SubConPoHeader>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_SubConPoHeader(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Selecting Sub Con Header By Order Id.", ex);
            }
            return returnValue;
        }
        
        public bool Update(long orderId, bool flgEformsImport, long eformsImportId, bool flgPoPlaced, string poType, string orderRef, string customerRef,
                           long entityId, string entityAddress, string entityTelephone, DateTime orderDate, string addressInvoice, string addressDelivery,
                           double orderAmount, double orderDiscountAmount, double orderShippingAmount, double orderSubtotalAmount, double orderVatAmount,
                           double orderTotalAmount, long contactId, string vehicleReg, string additionInfo, DateTime requiredByDate, bool flgDispatchDate,
                           DateTime dateDespatchDate, string orderedBy, string orderStatus, string userField01, string userField02, string userField03,
                           string userField04, string userField05, string userField06, string userField07, string userField08, string userField09,
                           string userField10, long createdBy, DateTime dateCreated, long lastAmendedBy, DateTime dateLastAmended)
        {
            const string METHOD_NAME = "SubConPoHeaderDB.Update()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(SubConPoHeaderQueries.update(this.DatabaseType, orderId, flgEformsImport, eformsImportId, flgPoPlaced, poType, orderRef,
                                                                      customerRef, entityId, entityAddress, entityTelephone, orderDate, addressInvoice,
                                                                      addressDelivery, orderAmount, orderDiscountAmount, orderShippingAmount, orderSubtotalAmount,
                                                                      orderVatAmount, orderTotalAmount, contactId, vehicleReg, additionInfo, requiredByDate,
                                                                      flgDispatchDate, dateDespatchDate, orderedBy, orderStatus, userField01, userField02,
                                                                      userField03, userField04, userField05, userField06, userField07, userField08,
                                                                      userField09, userField10, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Updating Sub Con Header By Order Id.", ex);
            }
            return returnValue;
        }

        public bool DeleteByOrderId(long orderId)
        {
            const string METHOD_NAME = "SubConPoHeaderDB.DeleteByOrderId()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(SubConPoHeaderQueries.delete(this.DatabaseType, orderId), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while deleting Sub Con Header By Order Id.", ex);
            }
            return returnValue;
        }

        public bool DeleteByFlgDeleted(long orderId)
        {
            const string METHOD_NAME = "SubConPoHeaderDB.DeleteByFlgDeleted()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(SubConPoHeaderQueries.deleteFlagDeleted(this.DatabaseType, orderId), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Deleting Sub Con Header by Flg Deleted.", ex);
            }
            return returnValue;
        }

        public bool UpdatePORefAndAmounts(long sequence, string poRef, double poAmtMat,
                                          double poAmtSubTotal, double poAmtTotal,
                                          long lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "SubConPoHeaderDB.UpdatePORefAndAmounts()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(SubConPoHeaderQueries.UpdatePORefAndAmounts(this.DatabaseType, sequence, poRef, poAmtMat,
                                                                                     poAmtSubTotal, poAmtTotal, 
                                                                                     lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Updating Sub Con PO Header PO Ref and Amounts.", ex);
            }
            return returnValue;
        }

        private SubConPoHeader Load_SubConPoHeader(OleDbDataReader dr)
        {
            const string METHOD_NAME = "SubConPoHeaderDB.Load_SubConPoHeader()";
            SubConPoHeader SubConPoHeader = null;
            try
            {
                if (dr != null)
                {
                    SubConPoHeader = new SubConPoHeader();
                    SubConPoHeader.Sequence = DBUtil.GetLongValue(dr, "sequence");
                    SubConPoHeader.FlgEformsImport = DBUtil.GetBooleanValue(dr, "flg_eforms_import");
                    SubConPoHeader.EformsImportId = DBUtil.GetLongValue(dr, "eforms_import_id");
                    SubConPoHeader.FlgPoPlaced = DBUtil.GetBooleanValue(dr, "flg_po_placed");
                    SubConPoHeader.PoType = DBUtil.GetIntValue(dr, "po_type");
                    SubConPoHeader.PORef = DBUtil.GetStringValue(dr, "po_ref");
                    SubConPoHeader.CustomerRef = DBUtil.GetStringValue(dr, "po_customer_ref");
                    SubConPoHeader.JobSequence = DBUtil.GetLongValue(dr, "job_sequence");
                    SubConPoHeader.EntityId = DBUtil.GetLongValue(dr, "entity_id");
                    SubConPoHeader.EntityAddress = DBUtil.GetStringValue(dr, "entity_address");
                    SubConPoHeader.EntityTelephone = DBUtil.GetStringValue(dr, "entity_telephone");
                    SubConPoHeader.PODate = DBUtil.GetDateTimeValue(dr, "date_po_date");
                    SubConPoHeader.AddressInvoice = DBUtil.GetStringValue(dr, "address_invoice");
                    SubConPoHeader.AddressDelivery = DBUtil.GetStringValue(dr, "address_delivery");
                    SubConPoHeader.PoAmtMat = DBUtil.GetDoubleValue(dr, "po_amt_mat");
                    SubConPoHeader.PoAmtLab = DBUtil.GetDoubleValue(dr, "po_amt_labour");
                    SubConPoHeader.PoAmtDiscount = DBUtil.GetDoubleValue(dr, "po_amt_discount");
                    SubConPoHeader.PoAmtShipping = DBUtil.GetDoubleValue(dr, "po_amt_shipping");
                    SubConPoHeader.PoAmtSubtotal = DBUtil.GetDoubleValue(dr, "po_amt_subtotal");
                    SubConPoHeader.PoAmtVat = DBUtil.GetDoubleValue(dr, "po_amt_vat");
                    SubConPoHeader.PoAmtTotal = DBUtil.GetDoubleValue(dr, "po_amt_total");
                    SubConPoHeader.RequestedId = DBUtil.GetLongValue(dr, "requested_id");
                    SubConPoHeader.VehicleReg = DBUtil.GetStringValue(dr, "po_vehicle_reg");
                    SubConPoHeader.poNotes = DBUtil.GetStringValue(dr, "po_notes");
                    SubConPoHeader.RequiredByDate = DBUtil.GetDateTimeValue(dr, "required_by_date");
                    SubConPoHeader.FlgDispatchDate = DBUtil.GetBooleanValue(dr, "flg_dispatch_date");
                    SubConPoHeader.DateDespatchDate = DBUtil.GetDateTimeValue(dr, "date_despatch_date");
                    SubConPoHeader.OrderedBy = DBUtil.GetStringValue(dr, "po_ordered_by");
                    SubConPoHeader.POStatus = DBUtil.GetIntValue(dr, "po_status");
                    SubConPoHeader.UserField01 = DBUtil.GetStringValue(dr, "po_user_field_01");
                    SubConPoHeader.UserField02 = DBUtil.GetStringValue(dr, "po_user_field_02");
                    SubConPoHeader.UserField03 = DBUtil.GetStringValue(dr, "po_user_field_03");
                    SubConPoHeader.UserField04 = DBUtil.GetStringValue(dr, "po_user_field_04");
                    SubConPoHeader.UserField05 = DBUtil.GetStringValue(dr, "po_user_field_05");
                    SubConPoHeader.UserField06 = DBUtil.GetStringValue(dr, "po_user_field_06");
                    SubConPoHeader.UserField07 = DBUtil.GetStringValue(dr, "po_user_field_07");
                    SubConPoHeader.UserField08 = DBUtil.GetStringValue(dr, "po_user_field_08");
                    SubConPoHeader.UserField09 = DBUtil.GetStringValue(dr, "po_user_field_09");
                    SubConPoHeader.UserField10 = DBUtil.GetStringValue(dr, "po_user_field_10");
                    SubConPoHeader.CreatedBy = DBUtil.GetLongValue(dr, "created_by");
                    SubConPoHeader.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    SubConPoHeader.LastAmendedBy = DBUtil.GetLongValue(dr, "last_amended_by");
                    SubConPoHeader.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Loading Sub Con Header.", ex);
            }
            return SubConPoHeader;
        }
    }
}
