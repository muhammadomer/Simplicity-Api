
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
    public class ScheduleItemsRepository : IScheduleItemsRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        
        private ILogger<ScheduleItemsRepository> _logger;

        public ScheduleItemsRepository()
        {
        }
        public ResponseModel GetItemsGroupsHierarchy(HttpRequest request,int groupId)
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
                        ScheduleItemsDB scheduleItemsDB = new ScheduleItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = scheduleItemsDB.selectScheduleItemHierarchy(groupId, out count, true);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = scheduleItemsDB.ErrorMessage;
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
                returnValue.Message = "Exception Occured While Getting schedule items hierarchy. " + ex.Message + " " + ex.InnerException;
                //_logger.LogError(ex.Message, ex);
            }

            return returnValue;
        }

        public ResponseModel GetScheduleItemsByGroup(HttpRequest request, int groupId,string parentCode)
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
                        ScheduleItemsDB scheduleItemsDB = new ScheduleItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = scheduleItemsDB.selectScheduleItemsByGroup(groupId,parentCode, out count, true);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = scheduleItemsDB.ErrorMessage;
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
                returnValue.Message = "Exception Occured While Getting schedule items List. " + ex.Message + " " + ex.InnerException;
                _logger.LogError(ex.Message, ex);
            }

            return returnValue;
        }
        public ResponseModel GetItemsGroupsDesc(HttpRequest request)
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
                        ScheduleItemsDB scheduleItemsDB = new ScheduleItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = scheduleItemsDB.selectItemsGroupsDesc(out count, true);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = scheduleItemsDB.ErrorMessage;
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
                returnValue.Message = "Exception Occured While Getting Group List. " + ex.Message + " " + ex.InnerException;
                _logger.LogError(ex.Message, ex);
            }

            return returnValue;
        }

    }
}
