using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class RefDiaryAppTypesRepository : IRefDiaryTypesRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        

        public RefDiaryAppTypesRepository()
        {
            
        }

        public RefDiaryAppTypes AddDiaryAppTypes(RefDiaryAppTypes Obj, HttpRequest request)
        {
            RefDiaryAppTypes result = new RefDiaryAppTypes();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefDiaryAppTypesDB VisitStatus = new RefDiaryAppTypesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = VisitStatus.insertRefDiaryAppTypes(Obj);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        public List<RefDiaryAppTypes> GetAllDiaryAppTypes(HttpRequest request, HttpResponse response)
        {
            List<RefDiaryAppTypes> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefDiaryAppTypesDB refRefDiaryAppTypesDB = new RefDiaryAppTypesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = refRefDiaryAppTypesDB.selectAllRefDiaryAppTypes();
                        if (returnValue == null)
                        {
                            response.Headers["message"] = "No Visit Status Found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Headers["message"] = "Exception occured while getting all Visit Status Types. " + ex.Message;
            }
            return returnValue;
        }

        public List<RefDiaryAppTypes> GetDiaryAppTypesById(long TypeCode, HttpRequest Request, HttpResponse Response)
        {
            List<RefDiaryAppTypes> returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefDiaryAppTypesDB DiaryAppTypes = new RefDiaryAppTypesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = DiaryAppTypes.selectAllRefDiaryAppTypesappTypeCode(TypeCode);
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }

        public RefDiaryAppTypes UpdateDiaryAppTypes(RefDiaryAppTypes model, HttpRequest request)
        {
            RefDiaryAppTypes result = new RefDiaryAppTypes();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefDiaryAppTypesDB DiaryAppTypes = new RefDiaryAppTypesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = DiaryAppTypes.updateByappTypeCode(model);
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
