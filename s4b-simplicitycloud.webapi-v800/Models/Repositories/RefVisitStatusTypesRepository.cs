using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class RefVisitStatusTypesRepository : IRefVisitStatusTypesRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public RefVisitStatusTypesRepository()
        {
            
        }

        public RefVisitStatusTypes AddVisitStatusTypes(RefVisitStatusTypes Obj, HttpRequest request)
        {
            RefVisitStatusTypes result = new RefVisitStatusTypes();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefVisitStatusTypesDB VisitStatus = new RefVisitStatusTypesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = VisitStatus.insertRefVisitStatusTypes(Obj);
                     }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        public List<RefVisitStatusTypes> GetAllVisitStatusTypes(HttpRequest request, HttpResponse response)
        {
            List<RefVisitStatusTypes> returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    RefVisitStatusTypesDB refVisitStatusTypesDB = new RefVisitStatusTypesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = refVisitStatusTypesDB.selectAllRefVisitStatusTypes();
                    if (returnValue == null)
                    {
                        response.Headers["message"] = "No Visit Status Found.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Headers["message"] = "Exception occured while getting all Visit Status Types. " + ex.Message;
            }
            return returnValue;
        }

        public RefVisitStatusTypes GetVisitStatusById(int visitId, HttpRequest Request, HttpResponse Response)
        {
            RefVisitStatusTypes result = new RefVisitStatusTypes();
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefVisitStatusTypesDB VisitStatus = new RefVisitStatusTypesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = VisitStatus.GetVisitStatusById(visitId);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        internal RefVisitStatusTypes GetRefVisitStatusTypeById(HttpRequest request, int visitStatus)
        {
            RefVisitStatusTypes returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    RefVisitStatusTypesDB refVisitStatusTypesDB = new RefVisitStatusTypesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = refVisitStatusTypesDB.selectAllRefVisitStatusTypesByStatusId(visitStatus);
                }
            }
            catch (Exception ex)
            {
                //Log Error
            }
            return returnValue;
        }

        internal RefVisitStatusTypes GetRefVisitStatusTypeByDesc(HttpRequest request, string visitStatusDesc)
        {
            RefVisitStatusTypes returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefVisitStatusTypesDB refVisitStatusTypesDB = new RefVisitStatusTypesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = refVisitStatusTypesDB.selectAllRefVisitStatusTypesByStatusDesc(visitStatusDesc);
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Error
            }
            return returnValue;
        }

        public RefVisitStatusTypes UpdateVisitStatus(RefVisitStatusTypes model, HttpRequest request)
        {
            RefVisitStatusTypes result = new RefVisitStatusTypes();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefVisitStatusTypesDB VisitStatus = new RefVisitStatusTypesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = VisitStatus.updateRefVisitStatusTypes(model);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }
    }
}
