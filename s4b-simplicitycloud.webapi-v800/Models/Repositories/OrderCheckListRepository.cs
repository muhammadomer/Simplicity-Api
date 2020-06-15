using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.Commons;
using Newtonsoft.Json;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class OrderCheckListRepository : IOrderCheckListRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }


        public OrderCheckListRepository()
        {
            
        }

        public ResponseModel UpdateOrderCheckList(HttpRequest request, RequestModel reqModel)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    if (reqModel != null && reqModel.TheObject != null)
                    {
                        List<OrderCheckList> ocl = JsonConvert.DeserializeObject<List<OrderCheckList>>(reqModel.TheObject.ToString());
                        if(ocl!=null)
                        {
                            int userId = Utilities.GetUserIdFromRequest(request);
                            DateTime? currentDateTime = DateTime.Now;
                            OrderCheckListDB oclDB = new OrderCheckListDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                            List<OrderCheckList> oclReturned = new List<OrderCheckList>();
                            long joinSequence = -1;
                            foreach (OrderCheckList oclReq in ocl)
                            {
                                if (oclReq.JoinSequence <= 0 && joinSequence<=0)
                                {
                                    OrderCheckListMain oclMain = new OrderCheckListMain();
                                    oclMain.JobSequence = oclReq.JobSequence;
                                    oclMain.FlgCancelCheckList = false;
                                    oclMain.CreatedBy = userId;
                                    oclMain.DateCreated = currentDateTime;
                                    oclMain.LastAmendedBy = userId;
                                    oclMain.DateLastAmended = currentDateTime;
                                    OrderCheckListMain oclMainNew = oclDB.insertOrderCheckListMain(oclMain);
                                    joinSequence = oclMainNew.Sequence ?? 0;
                                }
                                else if(oclReq.JoinSequence>0)
                                {
                                    joinSequence = oclReq.JoinSequence ?? 0;
                                }
                                OrderCheckListItems oclItem = new OrderCheckListItems();
                                oclItem.Sequence = oclReq.ItemSequence;
                                oclItem.JoinSequence = joinSequence;
                                oclItem.JobSequence = oclReq.JobSequence;
                                oclItem.CheckSequence = oclReq.RefSequence;
                                oclItem.FlgChecked = oclReq.FlgChecked;
                                oclItem.FlgCheckedYes = oclReq.FlgCheckedYes;
                                oclItem.FlgCheckedNo = oclReq.FlgCheckedNo;
                                oclItem.FlgCheckedDate = oclReq.FlgCheckedDate;
                                oclItem.CheckedDate = oclReq.CheckedDate;
                                oclItem.CheckedDetails = oclReq.CheckedDetails;
                                oclItem.LastAmendedBy = userId;
                                oclItem.DateLastAmended = currentDateTime;
                                OrderCheckListItems newOCLItem = null;
                                if (oclItem.Sequence <= 0)
                                {
                                    oclItem.CreatedBy = userId;
                                    oclItem.DateCreated = currentDateTime;
                                    newOCLItem = oclDB.insertOrderCheckListItem(oclItem);
                                }
                                else
                                {
                                    newOCLItem = oclDB.updateOrderCheckListItem(oclItem);
                                }
                                oclReq.JoinSequence = joinSequence;
                                oclReq.ItemSequence = newOCLItem.Sequence;
                                oclReturned.Add(oclReq);
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
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured while getting Order By Job. " + ex.Message + " " + ex.InnerException;
                //TODO: Logging
            }
            return returnValue;
        }

        public ResponseModel GetOrderCheckListByJobSequence(HttpRequest request, long jobSequence)
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
                        OrderCheckListDB orderCheckListDB = new OrderCheckListDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = orderCheckListDB.selectAllOrderCheckListByJobSequence(jobSequence);
                        if(returnValue.TheObject==null)
                        {
                            returnValue.Message = orderCheckListDB.ErrorMessage;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                    else
                    {
                        returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_PROJECT_ID;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured while getting Order By Job. " + ex.Message + " " + ex.InnerException;
                //TODO: Logging 
            }
            return returnValue;
        }

    }
}
