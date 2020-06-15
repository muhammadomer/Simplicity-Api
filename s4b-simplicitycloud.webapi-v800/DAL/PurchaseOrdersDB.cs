using SimplicityOnlineBLL.Entities;
using SimplicityOnlineDAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineDAL
{
    public class PurchaseOrdersDB : MainDB
    { 

        public PurchaseOrdersDB(DatabaseInfo dbInfo) : base(dbInfo)
        {
        }

        public bool Insert(out long orderId, bool flgEformsImport, long eformsImportId, bool flgPoPlaced, int poType, string orderRef, string customerRef,
                                         long supplierId, string supplierAddress, string supplierTelephone, DateTime? orderDate, string addressInvoice, string addressDelivery,
                                         double orderAmount, double orderDiscountAmount, double orderShippingAmount, double orderSubtotalAmount, double orderVatAmount,
                                         double orderTotalAmount, long contactId, string vehicleReg, string additionInfo, DateTime? requiredByDate, bool flgDispatchDate,
                                         DateTime? dateDespatchDate, string orderedBy, int orderStatus, string userField01, string userField02, string userField03,
                                         string userField04, string userField05, string userField06, string userField07, string userField08, string userField09,
                                         string userField10, long createdBy, DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            const string METHOD_NAME = "PurchaseOrdersDB.Insert()";
            bool returnValue = false;
            orderId = -1;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdInsert =
                        new OleDbCommand(PurchaseOrdersQueries.insert(this.DatabaseType, flgEformsImport, eformsImportId, flgPoPlaced, poType, orderRef,
                                                                      customerRef, supplierId, supplierAddress, supplierTelephone, orderDate, addressInvoice,
                                                                      addressDelivery, orderAmount, orderDiscountAmount, orderShippingAmount, orderSubtotalAmount,
                                                                      orderVatAmount, orderTotalAmount, contactId, vehicleReg, additionInfo, requiredByDate,
                                                                      flgDispatchDate, dateDespatchDate, orderedBy, orderStatus, userField01, userField02,
                                                                      userField03, userField04, userField05, userField06, userField07, userField08,
                                                                      userField09, userField10, createdBy, dateCreated, lastAmendedBy, dateLastAmended), conn))
                    {
                        objCmdInsert.ExecuteNonQuery();
                        orderId = Utilities.GetDBAutoNumber(conn);
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Inserting Purchase Order.", ex);
            }
            return returnValue;
        }

        public List<PurchaseOrders> GetPurchaseOrdersByOrderId(long orderId)
        {
            const string METHOD_NAME = "PurchaseOrdersDB.GetPurchaseOrdersByOrderId()";
            List<PurchaseOrders> returnValue = null;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdSelect =
                        new OleDbCommand(PurchaseOrdersQueries.getSelectAllByorderId(this.DatabaseType, orderId), conn))
                    {
                        using (OleDbDataReader dr = objCmdSelect.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                returnValue = new List<PurchaseOrders>();
                                while (dr.Read())
                                {
                                    returnValue.Add(Load_PurchaseOrders(dr));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Selecting Purchase Order By Order Id.", ex);
            }
            return returnValue;
        }
        
        public bool Update(long orderId, bool flgEformsImport, long eformsImportId, bool flgPoPlaced, string poType, string orderRef, string customerRef,
                           long supplierId, string supplierAddress, string supplierTelephone, DateTime orderDate, string addressInvoice, string addressDelivery,
                           double orderAmount, double orderDiscountAmount, double orderShippingAmount, double orderSubtotalAmount, double orderVatAmount,
                           double orderTotalAmount, long contactId, string vehicleReg, string additionInfo, DateTime requiredByDate, bool flgDispatchDate,
                           DateTime dateDespatchDate, string orderedBy, string orderStatus, string userField01, string userField02, string userField03,
                           string userField04, string userField05, string userField06, string userField07, string userField08, string userField09,
                           string userField10, long createdBy, DateTime dateCreated, long lastAmendedBy, DateTime dateLastAmended)
        {
            const string METHOD_NAME = "PurchaseOrdersDB.Update()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(PurchaseOrdersQueries.update(this.DatabaseType, orderId, flgEformsImport, eformsImportId, flgPoPlaced, poType, orderRef,
                                                                      customerRef, supplierId, supplierAddress, supplierTelephone, orderDate, addressInvoice,
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
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Updating Purchase Order By Order Id.", ex);
            }
            return returnValue;
        }

        public bool DeleteByOrderId(long orderId)
        {
            const string METHOD_NAME = "PurchaseOrdersDB.DeleteByOrderId()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(PurchaseOrdersQueries.delete(this.DatabaseType, orderId), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while deleting Purchase Order By Order Id.", ex);
            }
            return returnValue;
        }

        public bool DeleteByFlgDeleted(long orderId)
        {
            const string METHOD_NAME = "PurchaseOrdersDB.DeleteByFlgDeleted()";
            bool returnValue = false;
            try
            {
                using (OleDbConnection conn = this.getDbConnection())
                {
                    using (OleDbCommand objCmdUpdate =
                        new OleDbCommand(PurchaseOrdersQueries.deleteFlagDeleted(this.DatabaseType, orderId), conn))
                    {
                        objCmdUpdate.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Deleting Purchase Order by Flg Deleted.", ex);
            }
            return returnValue;
        }
        private PurchaseOrders Load_PurchaseOrders(OleDbDataReader dr)
        {
            const string METHOD_NAME = "PurchaseOrdersDB.Load_PurchaseOrders()";
            PurchaseOrders purchaseOrders = null;
            try
            {
                if (dr != null)
                {

                    purchaseOrders = new PurchaseOrders();
                    purchaseOrders.OrderId = DBUtil.GetLongValue(dr, "order_id");
                    purchaseOrders.FlgEformsImport = DBUtil.GetBooleanValue(dr, "flg_eforms_import");
                    purchaseOrders.EformsImportId = DBUtil.GetLongValue(dr, "eforms_import_id");
                    purchaseOrders.FlgPoPlaced = DBUtil.GetBooleanValue(dr, "flg_po_placed");
                    purchaseOrders.PoType = DBUtil.GetIntValue(dr, "po_type");
                    purchaseOrders.OrderRef = DBUtil.GetStringValue(dr, "order_ref");
                    purchaseOrders.CustomerRef = DBUtil.GetStringValue(dr, "customer_ref");
                    purchaseOrders.SupplierId = DBUtil.GetLongValue(dr, "supplier_id");
                    purchaseOrders.SupplierAddress = DBUtil.GetStringValue(dr, "supplier_address");
                    purchaseOrders.SupplierTelephone = DBUtil.GetStringValue(dr, "supplier_telephone");
                    purchaseOrders.OrderDate = DBUtil.GetDateTimeValue(dr, "order_date");
                    purchaseOrders.AddressInvoice = DBUtil.GetStringValue(dr, "address_invoice");
                    purchaseOrders.AddressDelivery = DBUtil.GetStringValue(dr, "address_delivery");
                    purchaseOrders.ContactId = DBUtil.GetLongValue(dr, "contact_id");
                    purchaseOrders.VehicleReg = DBUtil.GetStringValue(dr, "vehicle_reg");
                    purchaseOrders.AdditionInfo = DBUtil.GetStringValue(dr, "addition_info");
                    purchaseOrders.RequiredByDate = DBUtil.GetDateTimeValue(dr, "required_by_date");
                    purchaseOrders.FlgDispatchDate = DBUtil.GetBooleanValue(dr, "flg_dispatch_date");
                    purchaseOrders.DateDespatchDate = DBUtil.GetDateTimeValue(dr, "date_despatch_date");
                    purchaseOrders.OrderedBy = DBUtil.GetStringValue(dr, "ordered_by");
                    purchaseOrders.OrderStatus = DBUtil.GetIntValue(dr, "order_status");
                    purchaseOrders.UserField01 = DBUtil.GetStringValue(dr, "user_field_01");
                    purchaseOrders.UserField02 = DBUtil.GetStringValue(dr, "user_field_02");
                    purchaseOrders.UserField03 = DBUtil.GetStringValue(dr, "user_field_03");
                    purchaseOrders.UserField04 = DBUtil.GetStringValue(dr, "user_field_04");
                    purchaseOrders.UserField05 = DBUtil.GetStringValue(dr, "user_field_05");
                    purchaseOrders.UserField06 = DBUtil.GetStringValue(dr, "user_field_06");
                    purchaseOrders.UserField07 = DBUtil.GetStringValue(dr, "user_field_07");
                    purchaseOrders.UserField08 = DBUtil.GetStringValue(dr, "user_field_08");
                    purchaseOrders.UserField09 = DBUtil.GetStringValue(dr, "user_field_09");
                    purchaseOrders.UserField10 = DBUtil.GetStringValue(dr, "user_field_10");
                    purchaseOrders.CreatedBy = DBUtil.GetLongValue(dr, "created_by");
                    purchaseOrders.DateCreated = DBUtil.GetDateTimeValue(dr, "date_created");
                    purchaseOrders.LastAmendedBy = DBUtil.GetLongValue(dr, "last_amended_by");
                    purchaseOrders.DateLastAmended = DBUtil.GetDateTimeValue(dr, "date_last_amended");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error occured while Loading Purchase Order.", ex);
            }
            return purchaseOrders;
        }
    }
}
