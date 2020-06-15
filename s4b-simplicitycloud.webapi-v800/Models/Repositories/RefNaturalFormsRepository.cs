using Microsoft.AspNetCore.Http;

using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class RefNaturalFormsRepository : IRefNaturalFormsRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public RefNaturalFormsRepository()
        {
                  
        }

        public ResponseModel GetAllRefNaturalForms(HttpRequest request)
        {   
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {

                        DiaryAppRefNaturalFormsDB refNaturalFormsDB = new DiaryAppRefNaturalFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<RefNaturalForm> refNaturalforms  = refNaturalFormsDB.SelectAllFields();
                        if(refNaturalforms != null)
                        {
                            returnValue.TheObject = refNaturalforms;
                            returnValue.IsSucessfull = true;
                        }
                        else
                        {
                            returnValue.Message = refNaturalFormsDB.ErrorMessage;
                        }
                }
                else
                {
                    returnValue.Message = Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message =Message=  Utilities.GenerateAndLogMessage("Ref Natural Forms", "Error Occured while Getting User Diary Ref Natural Forms", ex.InnerException);
            }
            return returnValue;
        }

        public ResponseModel GetRefNaturalFormsByClientId(HttpRequest request,long ClientId)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {

                    DiaryAppRefNaturalFormsDB refNaturalFormsDB = new DiaryAppRefNaturalFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<RefNaturalForm> refNaturalforms = refNaturalFormsDB.SelectAllFieldsByClientId(ClientId);
                    if (refNaturalforms != null)
                    {
                        returnValue.TheObject = refNaturalforms;
                        returnValue.IsSucessfull = true;
                    }
                    else
                    {
                        returnValue.Message = refNaturalFormsDB.ErrorMessage;
                    }
                }
                else
                {
                    returnValue.Message = Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = Utilities.GenerateAndLogMessage("Ref Natural Forms", "Error Occured while Getting User Diary Ref Natural Forms", ex.InnerException);
            }
            return returnValue;
        }
    }
}
