using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Commons;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class S4bFormSubmissionRepository : IS4bFormSubmissionRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        private ILogger<S4bFormSubmissionRepository> _logger;
        public List<S4bFormSubmissions> getFormSubmissionLsit(HttpRequest request)
        {
            List<S4bFormSubmissions> list = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
            if (settings != null)
                {
                    S4bFormSubmissionsDB SubObject = new S4bFormSubmissionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    list = SubObject.getFormSubmissionList();
                }
            }
            catch(Exception ex)
            {
                list = null;
            }
            return list;
        }

        public S4bFormSubmissions getFormSubmissionBySubmitNo(HttpRequest request, string s4bSubmitNo)
        {
            S4bFormSubmissions s4bSubmission = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        S4bFormSubmissionsDB SubObject = new S4bFormSubmissionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        s4bSubmission = SubObject.getFormSubmissionBySubmitNo(s4bSubmitNo);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return s4bSubmission;
        }

        public S4bFormSubmissions getFormSubmissionBySequenceNo(HttpRequest request, long sequenceNo)
        {
            S4bFormSubmissions s4bSubmission = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        S4bFormSubmissionsDB SubObject = new S4bFormSubmissionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        s4bSubmission = SubObject.getFormSubmissionBySequenceNo(sequenceNo);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return s4bSubmission;
        }

        public S4bFormSubmissions getFormSubmissionInfoBySequenceNo(HttpRequest request, long sequenceNo)
        {
            S4bFormSubmissions s4bSubmission = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        S4bFormSubmissionsDB SubObject = new S4bFormSubmissionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        s4bSubmission = SubObject.getFormSubmissionInfoBySequenceNo(sequenceNo);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return s4bSubmission;
        }

        public S4bFormSubmissions InsertFormSubmission(S4bFormSubmissions s4bFormSubmissionObj, HttpRequest request)
        {
            S4bFormSubmissions s4bFormSubmission = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        S4bFormSubmissionsDB SubObject = new S4bFormSubmissionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        s4bFormSubmission = SubObject.InsertFormSubmission(s4bFormSubmissionObj);
                        var numSeq = s4bFormSubmission.Sequence ?? 0;
                        s4bFormSubmission.S4bSubmitNo = numSeq.ToString("00000");
                        SubObject.UpdateFormSubmission(s4bFormSubmission);
                    }
                }
            }
            catch (Exception ex)
            {
                s4bFormSubmission = null;
            }
            return s4bFormSubmission;
        }

        public bool UpdateFormSubmission(S4bFormSubmissions obj, HttpRequest request)
        {
            bool result = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        S4bFormSubmissionsDB SubObject = new S4bFormSubmissionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = SubObject.UpdateFormSubmission(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public bool UpdateFlgCompletedFormSubmission(S4bFormSubmissions obj, HttpRequest request)
        {
            bool result = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        S4bFormSubmissionsDB SubObject = new S4bFormSubmissionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = SubObject.UpdateFlgCompleted(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        ResponseModel IS4bFormSubmissionRepository.S4BeFormsList(ClientRequest clientRequest, HttpRequest request)
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
                        S4bFormSubmissionsDB s4bSubDB = new S4bFormSubmissionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = s4bSubDB.S4BeFormsList(clientRequest, out count, true);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = s4bSubDB.ErrorMessage;
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
                returnValue.Message = "Exception Occured While Getting Submission List. " + ex.Message + " " + ex.InnerException;
                _logger.LogError(ex.Message, ex);
            }

            return returnValue;
        }

        public List<S4bFormSubmissions>  getFormSubmissionListByJobSequence(HttpRequest request,long jobSequence)
        {
            List<S4bFormSubmissions> returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    S4bFormSubmissionsDB SubObject = new S4bFormSubmissionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = SubObject.getFormSubmissionListByJobSequence(jobSequence);
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;

        }
    }
}
