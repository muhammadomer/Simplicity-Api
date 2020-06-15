using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class RefPropertyTypesRespository: IRefPropertyTypesRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public  List<RefPropertyType> GetAllPropertyTypes(HttpRequest request, HttpResponse response)
        {
            List<RefPropertyType> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefPropertyTypeDB refVisitStatusTypesDB = new RefPropertyTypeDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = refVisitStatusTypesDB.selectAllRefPropertyTypes();
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
    }
}
