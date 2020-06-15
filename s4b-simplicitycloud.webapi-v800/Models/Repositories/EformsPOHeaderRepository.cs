using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineDAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class EformsPOHeaderRepository : IEformsPOHeaderRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public EformsPoHeader Insert(HttpRequest Request, EformsPoHeader obj)
        {
            const string METHOD_NAME = "EformsPOHeaderRepository.Insert()";
            EformsPoHeader returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    EformsPoHeaderDB eformsPoHeaderDB = new EformsPoHeaderDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    long sequence = -1;
                    if(eformsPoHeaderDB.InsertEformsPoHeader(out sequence, false, obj.DataType, obj.NfsSubmitNo, obj.NfsSubmitTimeStamp,
                                                             obj.ImpRef, obj.FormType, obj.JobRef, obj.FlgValidJobRef, obj.JobSequence ?? 0, obj.SupplierShortName,
                                                             obj.FlgValidSupplierShortName, obj.SupplierId ?? 0, obj.SupplierMultiAddId ?? 0, obj.SupplierEmail, obj.AttentionOf,
                                                             obj.NfPoRef, obj.DatePoDate, obj.RequiredByDate, obj.FlgDeliverToSite, obj.OrderedByShortName, obj.FlgValidOrderedByShortName,
                                                             obj.OrderedById ?? 0, obj.RequestedByShortName, obj.FlgValidRequestedByShortName, obj.RequestedById ?? 0,
                                                             obj.PoAddressInvoice, obj.PoNotes, obj.PoVoTypeSequence ?? 0, obj.VoRef, obj.OrderId ?? 0, obj.OrderAmount, obj.OrderDiscountAmount,
                                                             obj.OrderShippingAmount, obj.OrderSubtotalAmount, obj.OrderVatAmount, obj.OrderTotalAmount, obj.FlgOtherIssue,
                                                             obj.CreatedBy ?? 0, obj.DateCreated, obj.LastAmendedBy ?? 0, obj.DateLastAmended))
                    {
                        returnValue = obj;
                        returnValue.Sequence = sequence;
                    }
                    else
                    {
                        Message = eformsPoHeaderDB.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Inserting eForms PO Header.", ex);
            }
            return returnValue;
        }

        public bool UpdateOrderIdAndAmounts(HttpRequest request, EformsPoHeader obj)
        {
            const string METHOD_NAME = "EformsPOHeaderRepository.UpdateOrderIdAndAmounts()";
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    EformsPoHeaderDB eformsPoHeaderDB = new EformsPoHeaderDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (eformsPoHeaderDB.UpdateOrderIdAndAmounts(obj.Sequence ?? 0, obj.OrderId ?? 0, obj.OrderAmount, obj.OrderSubtotalAmount, 
                                                                 obj.OrderTotalAmount, obj.FlgOtherIssue,
                                                                 obj.LastAmendedBy ?? 0, obj.DateLastAmended))
                    {
                        returnValue = true;
                    }
                    else
                    {
                        Message = eformsPoHeaderDB.ErrorMessage;
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
