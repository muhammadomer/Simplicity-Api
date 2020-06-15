using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class TmpTimesheetRepository : ITmpTimesheetRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public TmpTimesheet insert(HttpRequest Request, TmpTimesheet obj)
        {
            TmpTimesheet returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    long sequence = -1;
                    TmpTimesheetDB tmpTimesheetDB = new TmpTimesheetDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (tmpTimesheetDB.insertTmpTimesheet(out sequence, obj.ImpRef, obj.DataStatus, obj.UncWebSessionId,
                                                          obj.RowEmployeeName, obj.RowDesc, obj.RowDesc2, obj.RowDesc3,
                                                          obj.DateRowStartTime, obj.DateRowFinishTime, obj.RowTimeTotal,
                                                          obj.RowPymtType, obj.RowNotes, obj.DateRowDate, obj.RowJobRef,
                                                          obj.FlgJobRefValid, obj.JobSequence, obj.FlgPayrollEntry, obj.EntityId ?? 0,
                                                          obj.FlgLessBreakTime, obj.RowAssetName, obj.CreatedBy ?? 0, obj.DateCreated, obj.LastAmendedBy ?? 0, obj.DateLastAmended))
                    {
                        obj.Sequence = sequence;
                        returnValue = obj;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel CreateTimeSheetData(HttpRequest Request, TmpTimesheet obj)
        {
            const string METHOD_NAME = "TmpTimesheetRepository.CreateTimeSheetData()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    long sequence = -1;
                    obj.JobSequence = (obj.JobSequence == null ? -1 : obj.JobSequence);                    
                    obj.RowJobRef = (obj.RowJobRef == null ? "" : obj.RowJobRef);
                    obj.CreatedBy = 1;
                    obj.DateCreated = DateTime.Now;
                    obj.LastAmendedBy = 1;
                    obj.DateLastAmended = DateTime.Now;
                    
                    TimeSpan span = (obj.DateRowFinishTime.Value.Subtract(obj.DateRowStartTime.Value));
                    double rowTimeTotal = span.TotalSeconds / 3600.00;

                    TmpTimesheetDB tmpTimesheetDB = new TmpTimesheetDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (tmpTimesheetDB.CreateTmpTimesheet(out sequence,
                                                            obj.DateRowStartTime, obj.DateRowFinishTime,
                                                            obj.RowPymtType, obj.RowNotes, obj.RowJobRef,
                                                            obj.FlgJobRefValid, obj.JobSequence,
                                                            obj.DeSequence, obj.StartTimeLocation, obj.FinishTimeLocation,
                                                            obj.CreatedBy ?? 0, obj.DateCreated, obj.LastAmendedBy ?? 0, obj.DateLastAmended,
                                                            rowTimeTotal, obj.UserId ?? 0, obj.DateRowStartTime, obj.UserId ?? 0)
                        )
                    {
                        obj.Sequence = sequence;
                        returnValue.IsSucessfull = true;
                        returnValue.TheObject = obj;
                    }
                    else
                    {
                        returnValue.Message = "Unable to Process Timesheet Request Reason: " + Message;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Processing Timesheet.", ex);              
            }
            return returnValue;
        }
        
        public List<AppointmentTimeEntries> GetAllTimeEntriesByDate(HttpRequest request, DateTime? appStartDate)
        {
            const string METHOD_NAME = "TmpTimesheetRepository.GetAllTimeEntriesByDate()";
            List<AppointmentTimeEntries> returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    if (appStartDate != null )
                    {
                        TmpTimesheetDB tmpTimesheetDB = new TmpTimesheetDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = tmpTimesheetDB.GetAllTimeEntriesByDate(appStartDate);
                    }
                    else
                        Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                }
                else
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting Diary Apps Assets.", ex);
            }
            return returnValue;
        }
    }
}
