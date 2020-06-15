using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class NaturalFormsRepository : INaturalFormsRepository
    {
        
        private ILogger<NaturalFormsRepository> LOGGER;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public NaturalFormsRepository()
        {
            
        }

        public ResponseModel GetTemplateURL(NaturalFormRequest naturalFormRequest, HttpRequest Request)
        {
            ResponseModel response = null;
            try
            {
                response = new ResponseModel();
                response.IsSucessfull = false;

                string projectId = Request.Headers["ProjectId"];
                if(naturalFormRequest==null)
                {
                    response.Message = "Invalid Request. The Request object is null";
                    return response;
                }
                if(naturalFormRequest.Form == null)
                {
                    response.Message = "Invalid Request. The Form object is null";
                    return response;
                }
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        if (naturalFormRequest.IsThirdParty)
                        {
                            DiaryAppsWebAssignDB daWebAssignDB = new DiaryAppsWebAssignDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                            string templateURL = daWebAssignDB.GetThirdPartyTemplateURL(naturalFormRequest, settings);
                            if(string.IsNullOrEmpty(templateURL))
                            {
                                response.Message = "Unable to get Template URL. Details: " + daWebAssignDB.ErrorMessage;
                            }
                            else
                            {
                                response.IsSucessfull = true;
                                response.Message = templateURL;
                            }
                        }
                        else
                        {
                            response.Message = "Request is not implemented for Non Third Party Templates.";
                            return response;
                        }
                    }
                    else
                    {
                    response.Message = "No Appointment found for sequence. ";
                    }
                }
                else
                {
                    response.Message = "Unable to load Project Settings from User session.";
                }
            }
            catch (Exception ex)
            {
            }
            return response;
        }
    }
}
