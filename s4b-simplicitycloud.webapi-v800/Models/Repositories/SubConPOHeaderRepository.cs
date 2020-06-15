using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineDAL;
using Microsoft.VisualBasic;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class SubConPOHeaderRepository : ISubConPOHeaderRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public string GenerateNewPORef(HttpRequest request)
        {
            const string METHOD_NAME = "SubConPOHeaderRepository.GenerateNewPORef()";
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

        public SubConPoHeader Insert(HttpRequest request, SubConPoHeader obj)
        {
            const string METHOD_NAME = "SubConPOHeaderRepository.Insert()";
            SubConPoHeader returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    SubConPoHeaderDB SubConPoHeaderDB = new SubConPoHeaderDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    long orderId = -1;
                    if(SubConPoHeaderDB.Insert(out orderId, obj.FlgEformsImport, obj.EformsImportId ?? 0, obj.FlgPoPlaced, 
                                               obj.PoType, obj.PORef, obj.CustomerRef, obj.JobSequence ?? 0, obj.EntityId ?? 0, obj.EntityAddress, 
                                               obj.EntityTelephone, obj.PODate, obj.AddressInvoice, obj.AddressDelivery, obj.PoAmtMat, 
                                               obj.PoAmtLab, obj.PoAmtDiscount, obj.PoAmtShipping, obj.PoAmtSubtotal, obj.PoAmtVat, 
                                               obj.PoAmtTotal,obj.RequestedId ?? 0,  obj.VehicleReg, obj.poNotes, obj.RequiredByDate, 
                                               obj.FlgDispatchDate, obj.DateDespatchDate, obj.OrderedBy,
                                               obj.POStatus, obj.UserField01, obj.UserField02, obj.UserField03, obj.UserField04, 
                                               obj.UserField05, obj.UserField06, obj.UserField07, obj.UserField08, obj.UserField09, obj.UserField10, 
                                               obj.CreatedBy ?? 0, obj.DateCreated, obj.LastAmendedBy ?? 0, obj.DateLastAmended))
                    {
                        returnValue = obj;
                        returnValue.Sequence = orderId;
                        if(orderId>0 && obj.SubConPOItems!=null && obj.SubConPOItems.Count>0)
                        {
                            for(int index=0;index<obj.SubConPOItems.Count;index++)
                            {
                                SubConPOItemsRepository subConPOItemsRepository = new SubConPOItemsRepository();
                                if (obj.SubConPOItems[index] != null)
                                {
                                    obj.SubConPOItems[index].POSequence = orderId;
                                    SubConPOItems subConPOItemsNew = subConPOItemsRepository.Insert(request, obj.SubConPOItems[index]);
                                    if (subConPOItemsNew == null)
                                    {
                                        Message += subConPOItemsRepository.Message + "\n";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Message = SubConPoHeaderDB.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured while Inserting Sub Con PO Header.", ex);
            }
            return returnValue;
        }

        public bool UpdatePORefAndAmounts(HttpRequest request, SubConPoHeader obj)
        {
            const string METHOD_NAME = "SubConPOHeaderRepository.UpdatePORefAndAmounts()";
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    SubConPoHeaderDB subConPoHeaderDB = new SubConPoHeaderDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (subConPoHeaderDB.UpdatePORefAndAmounts(obj.Sequence ?? 0, obj.PORef, obj.PoAmtMat, obj.PoAmtSubtotal,
                                                               obj.PoAmtTotal, obj.LastAmendedBy ?? 0, obj.DateLastAmended))
                    {
                        returnValue = true;
                    }
                    else
                    {
                        Message = subConPoHeaderDB.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Updating eForms PO Header Order Id And Amounts.", ex);
            }
            return returnValue;
        }

    }
}
