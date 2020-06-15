using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class S4BSubmissionsDataHRepository : IS4BSubmissionsDataHRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public S4BSubmissionsDataH Insert(S4BSubmissionsDataH Obj, HttpRequest request)
        {
            S4BSubmissionsDataH result = new S4BSubmissionsDataH();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        S4BSubmissionsDataHDB S4BSubmissionsDataHDB = new S4BSubmissionsDataHDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        long sequence = -1;
                        if(S4BSubmissionsDataHDB.insert(out sequence, Obj))
                        {
                            Obj.Sequence = sequence;
                        }
                     }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        public List<S4BSubmissionsDataH> GetAll(HttpRequest request, HttpResponse response)
        {
            List<S4BSubmissionsDataH> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        S4BSubmissionsDataHDB S4BSubmissionsDataHDB = new S4BSubmissionsDataHDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = S4BSubmissionsDataHDB.selectAll();
                        if (returnValue == null)
                        {
                            response.Headers["message"] = "No Record Found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Headers["message"] = "Exception occured while getting all Records. " + ex.Message;
            }
            return returnValue;
        }
    }
}
