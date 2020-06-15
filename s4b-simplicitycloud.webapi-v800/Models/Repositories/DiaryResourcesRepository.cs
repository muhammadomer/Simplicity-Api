using Microsoft.AspNetCore.Http;

using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class DiaryResourcesRepository: IDiaryResourcesRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public DiaryResourcesRepository()
        {
            
        }

        public List<DiaryResourcesMin> GetAllDiaryResources(HttpRequest Request)
        {
            List<DiaryResourcesMin> returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryResourcesDB diaryResourcesDB = new DiaryResourcesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = diaryResourcesDB.AllDiaryResources();
                        if (returnValue == null)
                        {
                            //Report back error
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        public ResponseModel GetDiaryResourceNotesbyEntityId(HttpRequest Request, long entityId)
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
                        DiaryResourcesDB NotesDB = new DiaryResourcesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = NotesDB.getResourceNotesByEntityId(entityId);
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = NotesDB.ErrorMessage;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                    else {
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
                returnValue.Message = "Exception Occured While Gettingall Clients Notes " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public DiaryResources GetDiaryResourceByUserId(HttpRequest Request, int userId)
        {
            DiaryResources returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryResourcesDB diaryResourcesDB = new DiaryResourcesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = diaryResourcesDB.selectDiaryResourceByUserId(userId);
                        if (returnValue == null)
                        {
                            //Report back error
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal double GetResourceVAMCostRate(HttpRequest request, long sequence)
        {
            const string METHOD_NAME = "DiaryResourcesRepository.GetResourceVAMCostRate()";
            double returnValue = 0;
            try
            {
                string projectId = request.Headers["ProjectId"];
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    DiaryResourcesDB diaryResourcesDB = new DiaryResourcesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = diaryResourcesDB.GetResourceVAMCostRate(sequence);
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Exception Occured While getting VAM Cost Rate.", ex);
            }
            return returnValue;
        }
    }
}
