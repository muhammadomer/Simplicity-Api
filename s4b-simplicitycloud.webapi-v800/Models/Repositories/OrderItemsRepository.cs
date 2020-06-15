
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Commons;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class OrderItemsRepository : IOrderItemsRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        
        private ILogger<OrderItemsRepository> _logger;

        public OrderItemsRepository()
        {
            
        }

        public ResponseModel GetOrderItemsByJobSequence(ClientRequest clientRequest,int jobSequence,HttpRequest request, HttpResponse response)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int count = 0;
                        OrderItemsDB orderItemsDB = new OrderItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = orderItemsDB.selectOrderItemsByJobSequence(clientRequest,jobSequence, out count, true);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = orderItemsDB.ErrorMessage;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                    else
                    {
                        returnValue.Message = SimplicityConstants.MESSAGE_INVALID_PROJECT_ID;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While Getting Order items List. " + ex.Message + " " + ex.InnerException;
                _logger.LogError(ex.Message, ex);
            }

            return returnValue;
        }
      public List<OrderItems> GetAllOrderItems(int sequence, HttpRequest request)
      {
         List<OrderItems> returnValue = null;
         try
         {
            string projectId = request.Headers["ProjectId"];
            //IEnumerable<string> userId;
            Microsoft.Extensions.Primitives.StringValues userId;
            request.Headers.TryGetValue("UserId", out userId);


            //int userId = Int32.Parse(request.Headers["UserId"]);

            if (userId.ToString() == "undefined")
            {
               int webUserId = Int32.Parse(request.Headers["WebId"]);
            }
            string token = request.Headers["token"];
            if (!string.IsNullOrWhiteSpace(projectId))
            {
               ProjectSettings settings = Configs.settings[projectId];
               if (settings != null)
               {
                  OrderItemsDB orderItemsDB = new OrderItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                  returnValue = orderItemsDB.selectAllOrderItemsSequence(sequence);

               }
            }
         }
         catch (Exception ex)
         {

         }
         return returnValue;
      }

      //Get All Order Items excluding text lines
      public List<OrderItems> GetOrderItemsNT(int sequence, HttpRequest request)
      {
         List<OrderItems> returnValue = null;
         try
         {
            string projectId = request.Headers["ProjectId"];
            //IEnumerable<string> userId;
            Microsoft.Extensions.Primitives.StringValues userId;
            request.Headers.TryGetValue("UserId", out userId);


            //int userId = Int32.Parse(request.Headers["UserId"]);

            if (userId.ToString() == "undefined")
            {
               int webUserId = Int32.Parse(request.Headers["WebId"]);
            }
            string token = request.Headers["token"];
            if (!string.IsNullOrWhiteSpace(projectId))
            {
               ProjectSettings settings = Configs.settings[projectId];
               if (settings != null)
               {
                  OrderItemsDB orderItemsDB = new OrderItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                  Utilities.WriteLog("Project settings:");
                  returnValue = orderItemsDB.selectNTOrderItemsSequence(sequence);

               }
            }
         }
         catch (Exception ex)
         {

         }
         return returnValue;
      }
		public ResponseModel GetOrderItemDescByItemCode(  HttpRequest request,string itemCode)
		{
			ResponseModel returnValue = new ResponseModel();
			try
			{
				string projectId = request.Headers["ProjectId"];
				if (!string.IsNullOrWhiteSpace(projectId))
				{
					ProjectSettings settings = Configs.settings[projectId];
					if (settings != null)
					{
						
						OrderItemsDB orderItemsDB = new OrderItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
						string value = orderItemsDB.selectItemDescByItemCode(itemCode);
						if (value == "")
						{
							returnValue.Message = orderItemsDB.ErrorMessage;
						}
						else
						{
							returnValue.TheObject = value;
							returnValue.IsSucessfull = true;
						}
					}
					else
					{
						returnValue.Message = SimplicityConstants.MESSAGE_INVALID_PROJECT_ID;
					}
				}
				else
				{
					returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
				}
			}
			catch (Exception ex)
			{
				returnValue.Message = "Exception Occured While Getting Order items Desc. " + ex.Message + " " + ex.InnerException;
				_logger.LogError(ex.Message, ex);
			}

			return returnValue;
		}
		public ResponseModel UpdateOrderItems(RequestModel reqModel, HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        if (reqModel != null && reqModel.TheObject != null)
                        {
                            List<OrderItems> ocl = JsonConvert.DeserializeObject<List<OrderItems>>(reqModel.TheObject.ToString());
                            if (ocl != null)
                            {
                                int userId = Utilities.GetUserIdFromRequest(request);
                                DateTime? currentDateTime = DateTime.Now;
                                OrderItemsDB oclDB = new OrderItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                List<OrderItems> oclReturned = new List<OrderItems>();
                                long Sequence = -1;
                                foreach (OrderItems oclReq in ocl)
                                {
                                    OrderItems oclItem = new OrderItems();
                                    if (oclReq.IsDelRow == true)  //case:delete
                                    {
                                        if (oclReq.Sequence > 0)
                                             oclDB.DeleteOrderItem(oclReq.Sequence ?? 0);
                                    }
                                    else if (oclReq.Sequence <= 0) //case:insert
                                    {
                                        oclItem = oclReq;
                                        oclItem.FlgCompleted = false;
                                        oclItem.FlgRowLocked = false;
                                        oclItem.TransType = "B";
                                        oclItem.GroupId = 1;
                                        oclItem.AssignedTo = -1;
                                        oclItem.CreatedBy = userId;
                                        oclItem.DateCreated = currentDateTime;
                                        oclItem.LastAmendedBy = userId;
                                        oclItem.DateLastAmended = currentDateTime;
                                        OrderItems oclMainNew = oclDB.InsertOrderItem(oclItem);
                                        oclItem.Sequence = oclMainNew.Sequence;
                                        oclReturned.Add(oclItem);
                                    }
                                    else if (oclReq.Sequence > 0)  //case:update
                                    {
                                        oclItem = oclReq;
                                        oclItem.LastAmendedBy = userId;
                                        oclItem.DateLastAmended = currentDateTime;
                                        oclItem = oclDB.UpdateOrderItem(oclItem);
                                        oclReturned.Add(oclItem);
                                    }
                                }
                                returnValue.TheObject = oclReturned;
                                returnValue.IsSucessfull = true;
                            }
                            else
                            {
                                returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + reqModel.TheObject.ToString();
                            }
                        }
                        else
                        {
                            returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                        }
                      
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured while saving Order Item " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }
        public OrderItems CreateOrderItems(OrderItems Oi, HttpRequest request)
        {
            OrderItems Obj = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    OrderItemsDB OrderItemsDB = new OrderItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    Obj = OrderItemsDB.InsertOrderItem(Oi);
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message + " " + ex.InnerException;
                Obj = null;
            }
            return Obj;
        }

        
        public List<EntityDetailsCoreMin> GetAllSupliers(HttpRequest request, string transType)
        {
            List<EntityDetailsCoreMin> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EntityDetailsCoreDB SuplierDB = new EntityDetailsCoreDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = SuplierDB.getSelectAllByTransType(transType,null);
                        if (returnValue == null)
                        {
                            //response.Headers["message"] = "No Supliers Found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception occured while getting all Supliers. " + ex.Message;
            }
            return returnValue;
        }
    }
}
