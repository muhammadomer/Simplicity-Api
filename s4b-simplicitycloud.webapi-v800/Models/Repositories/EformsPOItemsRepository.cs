using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineDAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class EformsPOItemsRepository : IEformsPOItemsRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public EFormsPOItems Insert(HttpRequest Request, EFormsPOItems obj)
        {
            const string METHOD_NAME = "EformsPOItemsRepository.Insert()";
            EFormsPOItems returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    EformsPoItemsDB eformsPOItemsDB = new EformsPoItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    long sequence = -1;
                    if(eformsPOItemsDB.InsertEformsPoItems(out sequence, false, obj.DataType, obj.ImpRef, obj.JoinSequence ?? 0,
                                                           obj.ItemType, obj.ItemCode, obj.ItemDesc, obj.ItemUnit, obj.ItemQuantity, obj.ItemAmtUnitPrice,
                                                           obj.ItemAmtSubtotalBeforeDiscount, obj.FlgItemDiscount, obj.ItemDiscountPcent, obj.ItemAmtDiscount, obj.ItemAmtSubtotal,
                                                           obj.FlgItemVat, obj.ItemVatPcent, obj.ItemAmtVat, obj.ItemAmtTotal, obj.DateItemDueDate, obj.FlgDeliverToSite,
                                                           obj.FlgDeliveryNote, obj.DeliveryNoteRef, obj.DeliveryNoteQty, obj.DateDeliveryNote,
                                                           obj.CreatedBy, obj.DateCreated, obj.LastAmendedBy, obj.DateLastAmended))
                    {
                        returnValue = obj;
                        returnValue.Sequence = sequence;
                    }
                    else
                    {
                        Message = eformsPOItemsDB.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Inserting eForms PO Header.", ex);
            }
            return returnValue;
        }
    }
}
