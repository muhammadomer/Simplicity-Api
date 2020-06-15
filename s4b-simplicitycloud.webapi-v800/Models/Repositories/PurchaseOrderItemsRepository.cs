using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineDAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class PurchaseOrderItemsRepository : IPurchaseOrderItemsRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public PurchaseOrderItems Insert(HttpRequest Request, PurchaseOrderItems obj)
        {
            const string METHOD_NAME = "PurchaseOrderItemsRepository.Insert()";
            PurchaseOrderItems returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    PurchaseOrderItemsDB purchaseOrderItemsDB = new PurchaseOrderItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    long sequence = -1;
                    if(purchaseOrderItemsDB.Insert(out sequence, obj.OrderId ?? 0, obj.ItemImportType, obj.RequestSequence ?? 0, obj.JobSequence ?? 0, 
                                                   obj.TransType, obj.EntityId ?? 0, obj.ItemType, obj.ItemHours, obj.ItemCode, 
                                                   obj.ItemDesc, obj.ItemUnit, obj.ItemQuantity, obj.ItemAmount, obj.FlgItemDiscount, 
                                                   obj.ItemDiscountPcent, obj.ItemDiscountAmount, obj.ItemSubtotal, obj.FlgItemVat, 
                                                   obj.ItemVatPcent, obj.ItemVatAmount, obj.ItemTotal))
                    {
                        returnValue = obj;
                        returnValue.Sequence = sequence;
                    }
                    else
                    {
                        Message = purchaseOrderItemsDB.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Inserting Purchase Orders Items.", ex);
            }
            return returnValue;
        }

        public ResponseModel GetAllPurchaseOrderItems(HttpRequest Request, ClientRequest clientRequest)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int count = 0;
                        PurchaseOrderItemsDB objDB = new PurchaseOrderItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = objDB.selectAllPOItems(clientRequest, out count, true);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = SimplicityConstants.MESSAGE_NO_RECORD_FOUND;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

    }
}
