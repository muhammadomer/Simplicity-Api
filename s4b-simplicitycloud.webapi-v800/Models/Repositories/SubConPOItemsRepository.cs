using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineDAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class SubConPOItemsRepository : ISubConPOItemsRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public SubConPOItems Insert(HttpRequest Request, SubConPOItems obj)
        {
            const string METHOD_NAME = "SubConPOItemsRepository.Insert()";
            SubConPOItems returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    SubConPOItemsDB SubConPOItemsDB = new SubConPOItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    long sequence = -1;
                    if(SubConPOItemsDB.Insert(out sequence, obj.POSequence ?? 0, obj.EntityId ?? 0, obj.ItemType, obj.ItemCode, 
                                                   obj.ItemDesc, obj.ItemUnit, obj.ItemQuantity, obj.ItemAmountMat, obj.ItemAmountLabour,
                                                   obj.ItemAmountNet, obj.FlgItemDiscount, obj.ItemDiscountPcent, obj.ItemDiscountAmount, 
                                                   obj.ItemSubtotal, obj.FlgItemVat, obj.ItemVatPcent, obj.ItemVatAmount, obj.ItemTotal, obj.ItemHours,
                                                   obj.CreatedBy, obj.DateCreated, obj.LastAmendedBy, obj.DateLastAmended))
                    {
                        returnValue = obj;
                        returnValue.Sequence = sequence;
                    }
                    else
                    {
                        Message = SubConPOItemsDB.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Inserting Purchase Orders Items.", ex);
            }
            return returnValue;
        }
    }
}
