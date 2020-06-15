using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.Commons;
using Microsoft.VisualBasic;
using SimplicityOnlineWebApi.DAL;
using Newtonsoft.Json;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class EdcGdprRepository: IEdcGdprRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public EdcGdprRepository()
        {
            
        }

        public ResponseModel InsertGDPR(HttpRequest request, EdcGdpr obj)
        {
            EdcGdpr result = new EdcGdpr();
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EdcGdprDB gdprDB = new EdcGdprDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        
                            
                            obj.CreatedBy = Convert.ToInt32(request.Headers["UserId"]);
                            obj.DateCreated = DateTime.Now;
                            if (gdprDB.insert(obj))
                            {
                                result = obj;
                                returnValue.TheObject = result;
                                returnValue.IsSucessfull = true;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
       

        public ResponseModel Update(HttpRequest request, EdcGdpr obj)
        {
            EdcGdpr result = new EdcGdpr();
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EdcGdprDB gdprDB = new EdcGdprDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        obj.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
                        obj.DateLastAmended = DateTime.Now;
                        if (gdprDB.updateByEntityId(obj))
                        {
                            result = obj;
                            returnValue.TheObject = result;
                            returnValue.IsSucessfull = true;
                         }
                         else{
                          returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + obj.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        public ResponseModel GetByEntityId(HttpRequest Request, long entityId)
        {
            ResponseModel returnValue = new ResponseModel();
            EdcGdpr result = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EdcGdprDB gdprDB = new EdcGdprDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = gdprDB.selectByEntityId(entityId);
                        returnValue.TheObject = result;
                        returnValue.IsSucessfull = true;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.IsSucessfull = false;
                returnValue.Message = "Exception Occur " + ex.Message;
            }
            return returnValue;
        }
    }
}
