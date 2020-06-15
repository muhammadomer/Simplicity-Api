using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineBLL.Entities;

using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class EntityDetailsJoinRepository:IEntityDetailsJoinRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public List<EntityDetailsJoin> GetSelectAllByEntityId(long entityId, HttpRequest Request)
        {
            List<EntityDetailsJoin> returnVal = new List<EntityDetailsJoin>();
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EntityDetailsJoinDB DetailsCoreDB = new EntityDetailsJoinDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = DetailsCoreDB.selectAllEntityDetailsJoinentityId(entityId);
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = null;
            }
            return returnVal;
        }
        public bool UpdateEntityDetailJoin(long entityId, string transType, HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EntityDetailsJoinDB orderDB = new EntityDetailsJoinDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = orderDB.update(entityId, transType);
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured While Updating Entity Details Join " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }
        public bool InsertEntityDetailJoin(long entityId, string transType, HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    EntityDetailsJoinDB edcDB = new EntityDetailsJoinDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if(edcDB.insertEntityDetailsJoin(entityId, transType))
                    {
                        returnValue = true;
                    }
                    else
                    {
                        Message = edcDB.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured While Inserting Entity Details Join " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }
    }
}
