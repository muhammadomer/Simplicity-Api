
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class OINFSubmissionDataRepository : IOINFSubmissionDataRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public OINFSubmissionDataRepository()
        {
            
        }

        public List<OINFSubmissionData> GetAllOINFSubmissionData(HttpRequest request, int sequence)
        {
            List<OINFSubmissionData> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                int userId = Int32.Parse(request.Headers["UserId"]);
                string token = request.Headers["token"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OINFSubmissionDataDB OINFSubmissionDataDB = new OINFSubmissionDataDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = OINFSubmissionDataDB.selectAllBySequence(sequence);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO:
            }
            return returnValue;
        }

        public OINFSubmissionData insert(HttpRequest request, OINFSubmissionData obj)
        {
            OINFSubmissionData Obj = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OINFSubmissionDataDB OINFSubmissionDataDB = new OINFSubmissionDataDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        Obj = OINFSubmissionDataDB.insert(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                Obj = null;
            }
            return Obj;
        }
    }
}
