using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineDAL;
using Microsoft.VisualBasic;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class PurchaseOrdersRepository : IPurchaseOrdersRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public string GenerateNewPORef(HttpRequest request)
        {
            const string METHOD_NAME = "PurchaseOrdersRepository.GenerateNewPORef()";
            string returnValue = SimplicityConstants.NotSet;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    ApplicationSettingsRepository applicationSettingsRepository = new ApplicationSettingsRepository();
                    ApplicationSettings applicationSettings = applicationSettingsRepository.GetApplicationSettingsBySettingId(request, SimplicityConstants.ApplicationSettingsLastPONo);
                    if (applicationSettings != null && Information.IsNumeric(applicationSettings.Setting1))
                    {
                        long newPONo_2ndPart = Convert.ToInt32(applicationSettings.Setting1) + 1;
                        if (newPONo_2ndPart > 0)
                        {
                            applicationSettings = applicationSettingsRepository.GetApplicationSettingsBySettingId(request, SimplicityConstants.ApplicationSettingsPONoPrefix);
                            if (applicationSettings != null)
                            {
                                string poNoPrefix = applicationSettings.Setting1;
                                if (poNoPrefix != "")
                                {
                                    string newPONo = poNoPrefix + newPONo_2ndPart.ToString("00000");
                                    InvoiceEntriesNewRepository invoiceEntriesNewRepository = new InvoiceEntriesNewRepository();
                                    InvoiceEntriesNew invoiceEntriesNew = invoiceEntriesNewRepository.GetByInvoiceNo(request, newPONo);
                                    if (invoiceEntriesNew == null)
                                    {
                                        if(applicationSettingsRepository.UpdateOrdersBillsLastPONo(request, newPONo_2ndPart.ToString()))
                                        {
                                            returnValue = newPONo;
                                        }
                                    }
                                    else
                                    {
                                        Message = applicationSettingsRepository.Message;
                                    }
                                }
                                else
                                {
                                    Message = applicationSettingsRepository.Message;
                                }
                            }
                            else
                            {
                                Message = applicationSettingsRepository.Message;
                            }
                        }
                        else
                        {
                            Message = "Unable to retrieve New PO No.";
                        }
                    }
                    else
                    {
                        Message = "Unable to retrieve New PO No. Reason: " + applicationSettingsRepository.Message;
                    }
                }
                else
                {
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Generating New PO Ref.", ex);
            }
            return returnValue;
        }

        public PurchaseOrders Insert(HttpRequest request, PurchaseOrders obj)
        {
            const string METHOD_NAME = "PurchaseOrdersRepository.Insert()";
            PurchaseOrders returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    PurchaseOrdersDB purchaseOrdersDB = new PurchaseOrdersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    long orderId = -1;
                    if(purchaseOrdersDB.Insert(out orderId, obj.FlgEformsImport, obj.EformsImportId ?? 0, obj.FlgPoPlaced, 
                                               obj.PoType, obj.OrderRef, obj.CustomerRef, obj.SupplierId ?? 0, obj.SupplierAddress, obj.SupplierTelephone, 
                                               obj.OrderDate, obj.AddressInvoice, obj.AddressDelivery, obj.OrderAmount, obj.OrderDiscountAmount, obj.OrderShippingAmount,
                                               obj.OrderSubtotalAmount, obj.OrderVatAmount, obj.OrderTotalAmount, obj.ContactId ?? 0, obj.VehicleReg, 
                                               obj.AdditionInfo, obj.RequiredByDate, obj.FlgDispatchDate, obj.DateDespatchDate, obj.OrderedBy,
                                               obj.OrderStatus, obj.UserField01, obj.UserField02, obj.UserField03, obj.UserField04, 
                                               obj.UserField05, obj.UserField06, obj.UserField07, obj.UserField08, obj.UserField09, obj.UserField10, 
                                               obj.CreatedBy ?? 0, obj.DateCreated, obj.LastAmendedBy ?? 0, obj.DateLastAmended))
                    {
                        returnValue = obj;
                        returnValue.OrderId = orderId;
                        if(orderId>0 && obj.POItems!=null && obj.POItems.Count>0)
                        {
                            for(int index=0;index<obj.POItems.Count;index++)
                            {
                                PurchaseOrderItemsRepository purchaseOrderItemsRepository = new PurchaseOrderItemsRepository();
                                if (obj.POItems[index] != null)
                                {
                                    obj.POItems[index].OrderId = orderId;
                                    PurchaseOrderItems purchaseOrderItemsNew = purchaseOrderItemsRepository.Insert(request, obj.POItems[index]);
                                    if (purchaseOrderItemsNew == null)
                                    {
                                        Message += purchaseOrderItemsRepository.Message + "\n";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Message = purchaseOrdersDB.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured while Inserting Purchase Orders.", ex);
            }
            return returnValue;
        }
    }
}
