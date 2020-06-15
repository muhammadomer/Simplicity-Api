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
    public class TimeAndAttendanceRepository : ITimeAndAttendanceRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }


        public TimeAndAttendanceRepository()
        {
            
        }

        public ResponseModel UpdateBudget(HttpRequest request, RequestModel reqModel)
        {
            const string METHOD_NAME = "TimeAndAttendanceRepository.UpdateBudget()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    if (reqModel != null && reqModel.TheObject != null)
                    {
                        List<PRSnap365Budget> budgetList = JsonConvert.DeserializeObject<List<PRSnap365Budget>>(reqModel.TheObject.ToString());
                        if(budgetList!=null)
                        {
                            int userId = Utilities.GetUserIdFromRequest(request);
                            DateTime? currentDateTime = DateTime.Now;
                            TimeAndAttendanceDB timeAndAttendanceDB = new TimeAndAttendanceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                            List<PRSnap365Budget> budgetReturned = new List<PRSnap365Budget>();
                            foreach (PRSnap365Budget budgetReq in budgetList)
                            {                                
                                if (budgetReq.Sequence <= 0)
                                {
                                    budgetReq.CreatedBy = userId;
                                    budgetReq.DateCreated = currentDateTime;
                                    budgetReturned.Add(timeAndAttendanceDB.InsertBudget(budgetReq));
                                }
                                else
                                {
                                    budgetReq.LastAmendedBy = userId;
                                    budgetReq.DateLastAmended = currentDateTime;
                                    budgetReturned.Add(timeAndAttendanceDB.UpdateBudget(budgetReq));
                                }
                            }
                            returnValue.TheObject = budgetReturned;
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
                returnValue.Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Updating Budget.", ex);
            }
            return returnValue;
        }

        public ResponseModel UpdateRevenue(HttpRequest request, RequestModel reqModel)
        {
            const string METHOD_NAME = "TimeAndAttendanceRepository.UpdateRevenue()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    if (reqModel != null && reqModel.TheObject != null)
                    {
                        List<PRSnap365Revenue> revenueList = JsonConvert.DeserializeObject<List<PRSnap365Revenue>>(reqModel.TheObject.ToString());
                        if (revenueList != null)
                        {
                            int userId = Utilities.GetUserIdFromRequest(request);
                            DateTime? currentDateTime = DateTime.Now;
                            TimeAndAttendanceDB timeAndAttendanceDB = new TimeAndAttendanceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                            List<PRSnap365Revenue> revenueReturned = new List<PRSnap365Revenue>();
                            foreach (PRSnap365Revenue revenueReq in revenueList)
                            {
                                if (revenueReq.Sequence <= 0)
                                {
                                    revenueReq.CreatedBy = userId;
                                    revenueReq.DateCreated = currentDateTime;
                                    revenueReturned.Add(timeAndAttendanceDB.InsertRevenue(revenueReq));
                                }
                                else
                                {
                                    revenueReq.LastAmendedBy = userId;
                                    revenueReq.DateLastAmended = currentDateTime;
                                    revenueReturned.Add(timeAndAttendanceDB.UpdateRevenue(revenueReq));
                                }
                            }
                            returnValue.TheObject = revenueReturned;
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
                returnValue.Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Updating Revenue.", ex);
            }
            return returnValue;
        }

        public ResponseModel GetBudget(HttpRequest request, int yearValue, int teamId, int locationId)
        {
            const string METHOD_NAME = "TimeAndAttendanceRepository.GetBudget()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        TimeAndAttendanceDB timeAndAttendanceDB = new TimeAndAttendanceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = timeAndAttendanceDB.GetPRSnap365Budget(teamId, locationId, yearValue);
                        if(returnValue.TheObject==null)
                        {
                            returnValue.Message = timeAndAttendanceDB.ErrorMessage;
                            returnValue.IsSucessfull = timeAndAttendanceDB.ErrorMessage.Equals(SimplicityConstants.MESSAGE_NO_RECORD_FOUND);
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
                returnValue.Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting Budget.", ex);
            }
            return returnValue;
        }

        public ResponseModel GetRevenue(HttpRequest request, int teamId, int locationId, DateTime? entryDate1, DateTime? entryDate2)
        {
            const string METHOD_NAME = "TimeAndAttendanceRepository.GetRevenue()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        TimeAndAttendanceDB timeAndAttendanceDB = new TimeAndAttendanceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = timeAndAttendanceDB.GetPRSnap365Revenue(teamId, locationId, entryDate1, entryDate2);
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = timeAndAttendanceDB.ErrorMessage;
                            returnValue.IsSucessfull = timeAndAttendanceDB.ErrorMessage.Equals(SimplicityConstants.MESSAGE_NO_RECORD_FOUND);

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
                returnValue.Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting Revenue.", ex);
            }
            return returnValue;
        }

        public ResponseModel GetRosterDetails(HttpRequest request, DateTime? entryDate1, DateTime? entryDate2, long prReference, long summarySequence)
        {
            const string METHOD_NAME = "TimeAndAttendanceRepository.GetRosterDetails()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        Dictionary<string, IEnumerable<object>> returnList = new Dictionary<string, IEnumerable<object>>();
                        TimeAndAttendanceDB timeAndAttendanceDB = new TimeAndAttendanceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<PRRosterInOutSummary> prRosterInOutSummary = timeAndAttendanceDB.GetPRRosterInOutSummary(entryDate1, entryDate2, prReference, summarySequence);
                        returnList.Add("PRRosterInOutSummary", prRosterInOutSummary);
                        List<PRRosterInOutSummaryDD> prRosterInOutSummaryDD = timeAndAttendanceDB.GetPRRosterInOutSummaryDD(entryDate1, entryDate2, prReference, summarySequence);
                        returnList.Add("PRRosterInOutSummaryDD", prRosterInOutSummaryDD);
                        List<PRSnap365Revenue> prSnap365Revenue = timeAndAttendanceDB.GetPRSnap365Revenue(-1, -1, entryDate1, entryDate2);
                        returnList.Add("PRSnap365Revenue", prSnap365Revenue);
                        List<PRSnap365Budget> prSnap365Budget = null;
                        if (entryDate1!=null)
                        {
                            prSnap365Budget = timeAndAttendanceDB.GetPRSnap365Budget(-1, -1, ((DateTime)entryDate1).Year);                            
                        }
                        returnList.Add("PRSnap365Budget", prSnap365Budget);
                        returnValue.TheObject = returnList;
                        returnValue.IsSucessfull = true;
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
                returnValue.Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While Getting Roster Details.", ex);
            }
            return returnValue;
        }
    }
}
