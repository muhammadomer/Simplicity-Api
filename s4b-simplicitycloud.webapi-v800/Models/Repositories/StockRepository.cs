using System;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineDAL;
using SimplicityOnlineWebApi.Models.Interfaces;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class StockRepository : IStockRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public StockRepository()
        {
        }

        public StockJobReqHeader InsertStockJobReqHeader(HttpRequest Request, StockJobReqHeader obj)
        {
            const string METHOD_NAME = "S4BFormsRepository.InsertStockJobReqHeader()";
            StockJobReqHeader returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    long sequence = -1;
                    StockJobReqHeaderDB stockJobReqHeaderDB = new StockJobReqHeaderDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (stockJobReqHeaderDB.InsertStockJobReqHeader(out sequence, obj.FlgDeleted, obj.JobSequence ?? 0, obj.FlgAuthorised,
                                                                    obj.AuthorisedBy ?? 0, obj.DateAuthorised, obj.PoType, obj.FlgPoPlaced,
                                                                    obj.PoSequence ?? 0, obj.UserField01, obj.UserField02,
                                                                    obj.UserField03, obj.UserField04, obj.UserField05, obj.UserField06,
                                                                    obj.UserField07, obj.UserField08, obj.UserField09, obj.UserField10,
                                                                    obj.CreatedBy, obj.DateCreated, obj.LastAmendedBy, obj.DateLastAmended))
                    {
                        obj.Sequence = sequence;
                        returnValue = obj;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Insert Stock Job Request Header Row", ex);
            }
            return returnValue;
        }

        public StockJobRequest InsertStockJobRequest(HttpRequest Request, StockJobRequest obj)
        {
            const string METHOD_NAME = "S4BFormsRepository.InsertStockJobRequest()";
            StockJobRequest returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    long sequence = -1;
                    StockJobRequestDB stockJobRequestDB = new StockJobRequestDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (stockJobRequestDB.InsertStockJobRequest(out sequence, obj.JobSequence ?? 0, obj.JoinSequence ?? 0, obj.TransType, obj.EntityId ?? 0,
                                                                obj.StockCode, obj.StockUnit, obj.StockDesc, obj.StockQuantity, obj.StockAmountEst,
                                                                obj.StockRequestedDate, obj.DateStockRequired, obj.FlgStockOrdered,
                                                                obj.StockOrderedDate, obj.FlgStockReceived, obj.StockReceivedDate, obj.FlgSorDrillDown,
                                                                obj.SorItemCode, obj.ItemType, obj.ItemHours, obj.CreatedBy, obj.DateCreated,
                                                                obj.LastAmendedBy, obj.DateLastAmended))
                    {
                        obj.Sequence = sequence;
                        returnValue = obj;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Insert Stock Job Request Row", ex);
            }
            return returnValue;
        }

        public StockJobReceived InsertStockJobReceived(HttpRequest Request, StockJobReceived obj)
        {
            const string METHOD_NAME = "S4BFormsRepository.InsertStockJobReceived()";
            StockJobReceived returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    long sequence = -1;
                    StockJobReceivedDB stockJobReceivedDB = new StockJobReceivedDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (stockJobReceivedDB.Insert(out sequence, obj.RequestSequence ?? 0, obj.DeliveryRef, obj.TransType, obj.EntityId ?? 0,
                                                  obj.StockRecievedDate, obj.StockCode, obj.StockQuantity, obj.StockAmount, obj.FlgFromStockroom,
                                                   obj.JobSequence ?? 0, obj.CreatedBy, obj.DateCreated, obj.LastAmendedBy, obj.DateLastAmended))
                    {
                        obj.Sequence = sequence;
                        returnValue = obj;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Insert Stock Job Received Row", ex);
            }
            return returnValue;
        }

        public bool UpdateStockDetailsIncrementQuantityAvail(HttpRequest Request, StockDetails obj)
        {
            const string METHOD_NAME = "S4BFormsRepository.UpdateStockDetailsIncrementQuantityAvail()";
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    StockDetailsDB stockDetailsDB = new StockDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (stockDetailsDB.UpdateIncrementAvailableStockQtyByStockCodeAndEntityId(obj.StockCode, obj.EntityId ?? 0, obj.StockQuantityAvail,
                                                                                              obj.LastAmendedBy, obj.DateLastAmended))
                    {
                        returnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Update Stock Details Increment Available Quantity.", ex);
            }
            return returnValue;
        }

        public StockGroup GetStockGroupByStockCodeAndTreeViewLevel(HttpRequest Request, string groupCode, int treeviewLevel)
        {
            const string METHOD_NAME = "S4BFormsRepository.ProcessLowryPO()";
            StockGroup returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    StockGroupDB stockGroupDB = new StockGroupDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = stockGroupDB.GetStockGroupByGroupCodeAndTreeviewLevel(groupCode, treeviewLevel);
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Get Stock Group By Group Code And Treeview Level.", ex);
            }
            return returnValue;
        }

        public StockList GetStockListByStockCodeAndEntityId(HttpRequest Request, string stockCode, long entityId)
        {
            const string METHOD_NAME = "S4BFormsRepository.GetStockListByStockCodeAndEntityId()";
            StockList returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    StockListDB stockListDB = new StockListDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = stockListDB.SelectByStockCodeAndEntityId(stockCode, entityId);
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to Get Stock Group By Group Code And Treeview Level.", ex);
            }
            return returnValue;
        }

    }
}