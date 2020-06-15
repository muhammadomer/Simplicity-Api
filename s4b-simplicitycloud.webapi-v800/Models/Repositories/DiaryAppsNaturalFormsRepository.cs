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
    public class DiaryAppsNaturalFormsRepository : IDiaryAppsNaturalFormsRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public DiaryAppsNaturalFormsRepository()
    {
      
    }

        public ResponseModel GetAllNaturalFormsByFormSequence(HttpRequest request, long deSequence, long formSequence)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {

                    DiaryAppNaturalFormsDB naturalFormsDB = new DiaryAppNaturalFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<DiaryAppNaturalForm> naturalforms = naturalFormsDB.SelectAllDANFByFormSequence(deSequence,formSequence);
                    if (naturalforms != null)
                    {
                        returnValue.TheObject = naturalforms;
                        returnValue.IsSucessfull = true;
                    }
                    else
                    {
                        returnValue.Message = naturalFormsDB.ErrorMessage;
                    }
                }
                else
                {
                    returnValue.Message = Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = Utilities.GenerateAndLogMessage("Diary App Natural Forms", "Error Occured while Getting Diary App Natural Forms", ex.InnerException);
            }
            return returnValue;
        }

        public ResponseModel GetAllNaturalFormsByDESequence(HttpRequest request, long deSequence)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {

                    DiaryAppNaturalFormsDB naturalFormsDB = new DiaryAppNaturalFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<DiaryAppNaturalForm> naturalforms = naturalFormsDB.SelectAllDANFByDESequence(deSequence);
                    if (naturalforms != null)
                    {
                        returnValue.TheObject = naturalforms;
                        returnValue.IsSucessfull = true;
                    }
                    else
                    {
                        returnValue.Message = naturalFormsDB.ErrorMessage;
                    }
                }
                else
                {
                    returnValue.Message = Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = Utilities.GenerateAndLogMessage("Diary App Natural Forms", "Error Occured while Getting Diary App Natural Forms", ex.InnerException);
            }
            return returnValue;
        }

        public ResponseModel GetUnassignedNaturalFormsOfDESequence(HttpRequest request, long deSequence)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {

                    DiaryAppNaturalFormsDB naturalFormsDB = new DiaryAppNaturalFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<DiaryAppNaturalForm> naturalforms = naturalFormsDB.SelectUnassignedDANFOfDESequence(deSequence);
                    if (naturalforms != null)
                    {
                        returnValue.TheObject = naturalforms;
                        returnValue.IsSucessfull = true;
                    }
                    else
                    {
                        returnValue.Message = naturalFormsDB.ErrorMessage;
                    }
                }
                else
                {
                    returnValue.Message = Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = Message = Utilities.GenerateAndLogMessage("Diary App Natural Forms", "Error Occured while Getting Diary App Natural Forms", ex.InnerException);
            }
            return returnValue;
        }
        public ResponseModel InsertNaturalForm(DiaryAppNaturalForm[] DiaryAppNaturalForms,long deSequence, HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            List<DiaryAppNaturalForm> objNatForms = new List<DiaryAppNaturalForm>();
            long appSequene = 0;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    int userId = Convert.ToInt32(request.Headers["UserId"]);
                    if (settings != null)
                    {
                        DiaryAppNaturalFormsDB naturalFormDB = new DiaryAppNaturalFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        foreach (DiaryAppNaturalForm frm in DiaryAppNaturalForms)
                        {
                            appSequene = frm.DiaryAppSequence ?? 0;
                        }
						appSequene = deSequence;
                        //---Find Existing entries
                        List<DiaryAppNaturalForm> existDanf = naturalFormDB.SelectAllDANFByDESequence(appSequene);
                        foreach (DiaryAppNaturalForm frm in DiaryAppNaturalForms)
                        {
                            DiaryAppNaturalForm find = existDanf.Find(x => x.RefNaturalForm.FormSequence == frm.FormSequence);
                            if (find == null) //new entry
                            {
                                DiaryAppNaturalForm obj = new DiaryAppNaturalForm();
                                frm.CreatedBy = userId;
                                obj = naturalFormDB.InsertNaturalForms(frm);
                                if (obj.Sequence > 0)
                                    objNatForms.Add(obj);
                            }
                        }
                        //---Remove entries
                        foreach (DiaryAppNaturalForm existElement in existDanf)
                        {
                            DiaryAppNaturalForm find  = Array.Find(DiaryAppNaturalForms, element => element.FormSequence == existElement.RefNaturalForm.FormSequence);
                            if (find == null) //if not exist in new list
                            {
                                naturalFormDB.DeleteNaturalFormsBySequence(existElement.Sequence ?? 0);
                            }
                        }
                        returnValue.TheObject = objNatForms;
                        if (objNatForms.Count > 0)
                            returnValue.IsSucessfull = true;
                        else
                            returnValue.IsSucessfull = false;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While inserting. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel InsertPasteDiaryAppsNaturalForms(DiaryAppNaturalForm DiaryAppNaturalForm, HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppNaturalFormsDB naturalFormDB = new DiaryAppNaturalFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        DiaryAppNaturalForm obj = new DiaryAppNaturalForm();
                        obj = naturalFormDB.InsertPasteDiaryAppsNaturalForms(DiaryAppNaturalForm);
                        returnValue.TheObject = obj;
                        if (obj.Sequence > 0)
                            returnValue.IsSucessfull = true;
                        else
                            returnValue.IsSucessfull = false;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While inserting. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel InsertTFRFromUnscheduled(long deSequence, long DESequenceUnscheduled,  HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppNaturalFormsDB naturalFormDB = new DiaryAppNaturalFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        int userId = Convert.ToInt32(request.Headers["UserId"]);
                        DiaryAppNaturalForm obj = new DiaryAppNaturalForm();
                        obj = naturalFormDB.InsertTFRFromUnscheduled(deSequence,DESequenceUnscheduled,userId);
                        returnValue.TheObject = obj;
                        if (obj.Sequence > 0)
                            returnValue.IsSucessfull = true;
                        else
                            returnValue.IsSucessfull = false;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While inserting. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel DeleteNaturalFormsBySequence(long Sequence,  HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppNaturalFormsDB naturalFormDB = new DiaryAppNaturalFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        bool ret = naturalFormDB.DeleteNaturalFormsBySequence(Sequence);
                        returnValue.TheObject = ret;
                        returnValue.IsSucessfull = ret;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While deleting. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public ResponseModel DeleteNaturalFormsByDESequence(long deSequence, HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        DiaryAppNaturalFormsDB naturalFormDB = new DiaryAppNaturalFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        bool ret = naturalFormDB.DeleteNaturalFormsByDeSequence(deSequence);
                        returnValue.TheObject = ret;
                        returnValue.IsSucessfull = ret;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While deleting. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

       
    }
}
