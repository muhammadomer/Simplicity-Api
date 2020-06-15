using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class RefGenericLabelsRepository : IRefGenericLabelsRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public RefGenericLabelsRepository()
        {
            
        }

        public ResponseModel AddGenericLable(RefGenericLabels obj,HttpRequest request)
        {
            const string METHOD_NAME = "RefGenericLabelsRepository.AddGenericLable()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                ProjectSettings settings = Configs.settings[projectId];
                if (settings != null)
                {
                    RefGenericLabelsDB genericLabelsDB = new RefGenericLabelsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue.IsSucessfull = genericLabelsDB.insertRefGenericLabels(obj.GenericFieldId ?? 0, obj.GeneticFieldName, obj.CustomisedFieldName);
                    if (!returnValue.IsSucessfull)
                    {
                        returnValue.Message = genericLabelsDB.ErrorMessage;
                    }
                }
            }
            catch(Exception ex)
            {
                returnValue.Message= "In Method " + METHOD_NAME + " Error occured while inserting generic labels. Details: " + ex.Message +" "+ ex.InnerException;
                //log.Error(returnValue.Message);
            }
            return returnValue;
        }
  
        public List<RefGenericLabels> GetAllGenericLabels(HttpRequest request, HttpResponse response)
        {
            List<RefGenericLabels> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                //IEnumerable<string> userId;
                Microsoft.Extensions.Primitives.StringValues userId;
                request.Headers.TryGetValue("UserId", out userId);
                

                //int userId = Int32.Parse(request.Headers["UserId"]);
                if (userId.ToString() == "undefined")
                {
                    int webUserId = Int32.Parse(request.Headers["WebId"]);
                }
                string token = request.Headers["token"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefGenericLabelsDB genericLabelsDB = new RefGenericLabelsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = genericLabelsDB.selectAllRef_Generic_Labels();
                        if (returnValue == null)
                        {
                            response.Headers["message"] = "No Generic Label Found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Headers["message"] = "Exception occured while getting all Generic Labels. " + ex.Message;
            }
            return returnValue;
        }

        public RefGenericLabels GetGenericLableById(long genericFieldId, HttpRequest request,HttpResponse response)
        {
            RefGenericLabels returnValue = new RefGenericLabels();
            try
            {
                string projectId = request.Headers["ProjectId"];
                ProjectSettings settings = Configs.settings[projectId];
                if (settings != null)
                {
                    RefGenericLabelsDB genericLabelsDB = new RefGenericLabelsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = genericLabelsDB.selectAllRef_Generic_LabelsByGenericFieldId(genericFieldId);
                    if (returnValue == null)
                    {
                        response.Headers["message"] = "No Generic Label Found";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Headers["message"]= "Exception occured while getting generic labels. " + ex.Message;
            }
            return returnValue;
        }

        public ResponseModel UpdateGenericLable(RefGenericLabels obj,HttpRequest request)
        {
            const string METHOD_NAME = "RefGenericLabelsRepository.UpdateGenericLable()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                ProjectSettings settings = Configs.settings[projectId];
                if (settings != null)
                {
                    RefGenericLabelsDB genericLabelsDB = new RefGenericLabelsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue.IsSucessfull = genericLabelsDB.updateBygenericFieldId(obj.GenericFieldId ?? 0, obj.GeneticFieldName, obj.CustomisedFieldName);
                    if (!returnValue.IsSucessfull)
                    {
                       returnValue.Message = genericLabelsDB.ErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "In Method " + METHOD_NAME + " Error occured while inserting generic labels. Details: " + ex.Message + " " + ex.InnerException;
                //log.Error(returnValue.Message);
            }
            return returnValue;
        }
    }
}
