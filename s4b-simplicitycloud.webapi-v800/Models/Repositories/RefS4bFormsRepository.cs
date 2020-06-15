using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.BLL.Entities;
using System.IO;
using SimplicityOnlineWebApi.Commons;
using System.Linq;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class RefS4bFormsRepository : IRefS4bFormsRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public RefS4bFormsRepository()
        {
            
        }

        public ResponseModel AddRecord(RefS4bForms obj, HttpRequest request)
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
                        long formSeq = obj.FormSequence ?? 0;
                        RefS4bFormsDB naturalForm = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        DateTime? currentDateTime = DateTime.Now;
                        obj.DefaultId = 0;
                        obj.ClientId = -1;
                        obj.CategorySequence = 0;
                        obj.DateCreated = currentDateTime;
                        obj.DateLastAmended = currentDateTime;
                        obj.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]); 
                        obj.CreatedBy = Convert.ToInt32(request.Headers["UserId"]);
                        returnValue.TheObject = naturalForm.insertRefNaturalForms(obj);
                        returnValue.IsSucessfull = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public ResponseModel updateRecord(RefS4bForms obj, HttpRequest request)
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
                        RefS4bFormsDB naturalForm = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        DateTime? currentDateTime = DateTime.Now;
                        obj.DateLastAmended = currentDateTime;
                        obj.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
                        returnValue.TheObject = naturalForm.updateByformSequence(obj);
                        returnValue.IsSucessfull = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public RefS4bForms GetByFormId(string formId, HttpRequest request)
        {
            RefS4bForms result = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefS4bFormsDB naturalForm = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = naturalForm.getByFormId(formId);
                     }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }
        public bool UnDeleteTemp(long formSeq, HttpRequest request)
        {
            bool result = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    RefS4bFormsDB naturalForm = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    result = naturalForm.UndeleteByFlgDeleted(formSeq);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }


        public List<RefS4bForms> GetFormList(HttpRequest request)
        {
            List<RefS4bForms> result = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefS4bFormsDB naturalForm = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = naturalForm.getAllForms();
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }
        public RefS4bForms GetRecodsById(long id, HttpRequest request)
        {
            RefS4bForms result = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    RefS4bFormsDB naturalForm = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    result = naturalForm.getRecordById(id);
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        public bool SaveJsonFile(RefS4bForms form, HttpRequest request)
        {
            bool result = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    string Directorypath = settings.S4BFormsRootFolderPath + form.FormId;
                    if (!Directory.Exists(Directorypath))
                    {
                        result = true;
                        Directory.CreateDirectory(Directorypath);
                        File.WriteAllBytes(Directorypath + "/" + SimplicityConstants.S4BFormJsonFileName, Convert.FromBase64String(form.Filebasecode));
                    }
                    else
                    {
                        result = true;
                        File.WriteAllBytes(Directorypath + "/" + SimplicityConstants.S4BFormJsonFileName, Convert.FromBase64String(form.Filebasecode));
                    }

                    RefS4bFormsDB refS4bFormsDB = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    refS4bFormsDB.updateLastAmendedDateByformSequence(form.FormSequence ?? 0, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public bool SavePdfFile(RefS4bForms form, HttpRequest request)
        {
            bool result = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        string Directorypath = settings.S4BFormsRootFolderPath + form.FormId;
                        if (!Directory.Exists(Directorypath))
                        {
                            result = true;
                            Directory.CreateDirectory(Directorypath);
                            File.WriteAllBytes(Directorypath + "/" + SimplicityConstants.S4BFormTemplateName, Convert.FromBase64String(form.Filebasecode));
                        }
                        else
                        {
                            result = true;
                            File.WriteAllBytes(Directorypath + "/" + SimplicityConstants.S4BFormTemplateName, Convert.FromBase64String(form.Filebasecode));
                        }
                    }

                    RefS4bFormsDB refS4bFormsDB = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    refS4bFormsDB.updateLastAmendedDateByformSequence(form.FormSequence ?? 0, DateTime.Now);
                    
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public bool SaveReferenceFile(RefS4bForms form, HttpRequest request)
        {
            bool result = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    string Directorypath = settings.S4BFormsRootFolderPath + form.FormId;
                    if (!Directory.Exists(Directorypath))
                    {
                        result = true;
                        Directory.CreateDirectory(Directorypath);
                        File.WriteAllBytes(Directorypath + "/" + form.FileName, Convert.FromBase64String(form.Filebasecode));
                    }
                    else
                    {
                        result = true;
                        File.WriteAllBytes(Directorypath + "/" + form.FileName, Convert.FromBase64String(form.Filebasecode));
                    }

                    RefS4bFormsDB refS4bFormsDB = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    refS4bFormsDB.updateLastAmendedDateByformSequence(form.FormSequence ?? 0, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public bool SaveReferenceFiles(List<RefS4bForms> forms, HttpRequest request)
        {
            bool result = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        string Directorypath = string.Empty;
                        foreach (var form in forms)
                        {
                            Directorypath = System.IO.Path.Combine(settings.S4BFormsRootFolderPath, form.FormId, Commons.SimplicityConstants.S4BFormReferenceFilesFolderName);
                            if (!Directory.Exists(Directorypath))
                            {                            
                                Directory.CreateDirectory(Directorypath);
                                File.WriteAllBytes(Directorypath + "/" + form.FileName, Convert.FromBase64String(form.Filebasecode));
                            }
                            else
                            {                             
                                File.WriteAllBytes(Directorypath + "/" + form.FileName, Convert.FromBase64String(form.Filebasecode));
                            }
                        }

                        result = true;
                        if (forms != null && forms.Count >= 1)
                        {                                                        
                            RefS4bFormsDB refS4bFormsDB = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                            refS4bFormsDB.updateLastAmendedDateByformSequence(forms[0].FormSequence ?? 0, DateTime.Now);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        

        public bool DeleteTemplate(long FormSequence, HttpRequest request)
        {
            bool result = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefS4bFormsDB naturalForm = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = naturalForm.deleteByFlgDeleted(FormSequence);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public List<RefS4bForms> LoadTemplateByFlgDelete(HttpRequest request)
        {
            List<RefS4bForms> result = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefS4bFormsDB naturalForm = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = naturalForm.getDeleteTemplate();
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        public List<RefS4bForms> GetUserChangedForms(DateTime? lastSyncDate, HttpRequest request)
        {
            List<RefS4bForms> result = new List<RefS4bForms>();
            string projectId = request.Headers["ProjectId"];
            long userId = long.Parse(request.Headers["UserId"]);

            if (!string.IsNullOrWhiteSpace(projectId))
            {
                ProjectSettings settings = Configs.settings[projectId];
                if (settings != null)
                {
                    RefS4bFormsDB db = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    result = db.selectUserChangedRefForms(lastSyncDate, userId);
                }
            }

            return result;
        }

        public bool SetTempDefault(long formSeq, HttpRequest request)
        {
            bool result = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefS4bFormsDB naturalForm = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = naturalForm.SetTemplateAsDefault(formSeq);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public SimplicityFile MakeZipFileOfTemplate(string formId, HttpRequest request)
        {
            const string METHO_NAME = "RefS4BFormsRepository.MakeZipFileOfTemplate()";
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
            if (settings != null)
            {
                try
                {
                    var baseOutputStream = new System.IO.MemoryStream();
                    ICSharpCode.SharpZipLib.Zip.ZipOutputStream zipOutput = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(baseOutputStream);
                    zipOutput.IsStreamOwner = false;

                    /* 
                    * Higher compression level will cause higher usage of reources
                    * If not necessary do not use highest level 9
                    */

                    zipOutput.SetLevel(3);
                    byte[] buffer = new byte[4096];
                    SharpZipLibHelper.ZipFolder(Path.Combine(settings.S4BFormsRootFolderPath, formId), zipOutput);

                    zipOutput.Finish();
                    zipOutput.Close();

                    /* Set position to 0 so that cient start reading of the stream from the begining */
                    baseOutputStream.Position = 0;

                    return new SimplicityFile()
                    {
                        MemStream = baseOutputStream
                    };
                }
                catch (Exception ex)
                {
                    Message = Utilities.GenerateAndLogMessage(METHO_NAME, "Unable to Make Zip File of Template '" + formId + "'", ex);
					Utilities.WriteLog(Message);
                }
            }
            return null;
        }

        public List<RefS4bMapping> GetTemplateMapping(long formSeq, HttpRequest request)
        {
            List<RefS4bMapping> result = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefS4bFormsDB db = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = db.GetS4bMapping(formSeq);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        public long AddTemplateMapping(RefS4bMapping mapping, HttpRequest request)
        {
            long result = 0;
            string projectId = request.Headers["ProjectId"];
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
            if (settings != null)
            {
                RefS4bFormsDB db = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                var formMappings = db.GetS4bMapping(mapping.FormSequence ?? 0);
                if (formMappings != null && formMappings.Any(x => x.FieldName == mapping.FieldName))
                {
                    throw new ArgumentException($"'{mapping.FieldName}' field already exist");
                }
                if (mapping.FieldValueType == 0)
                {
                    mapping.FieldValue = "Not Set";
                }
                result = db.insertMapping(mapping.FormSequence ?? 0, mapping.FieldName, mapping.FieldValueType, mapping.FieldValue, long.Parse(request.Headers["UserId"]), DateTime.Now);
            }

            return result;
        }
        public bool UpdateTemplateMapping(RefS4bMapping mapping, HttpRequest request)
        {
            bool result = false;
            string projectId = request.Headers["ProjectId"];
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                ProjectSettings settings = Configs.settings[projectId];
                if (settings != null)
                {
                    RefS4bFormsDB db = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    var formMappings = db.GetS4bMapping(mapping.FormSequence ?? 0);
                    if (formMappings != null && formMappings.Any(x => x.FieldName == mapping.FieldName && x.Sequence != mapping.Sequence))
                    {
                        throw new ArgumentException($"'{mapping.FieldName}' field already exist");
                    }
                    if (mapping.FieldValueType == 0)
                    {
                        mapping.FieldValue = "Not Set";
                    }
                    result = db.updateMapping(mapping.Sequence ?? 0, mapping.FormSequence ?? 0, mapping.FieldName, mapping.FieldValueType, mapping.FieldValue, long.Parse(request.Headers["UserId"]), DateTime.Now);
                }
            }

            return result;
        }
        public bool DeleteTemplateMapping(long sequence, HttpRequest request)
        {
            bool result = false;
            string projectId = request.Headers["ProjectId"];
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                ProjectSettings settings = Configs.settings[projectId];
                if (settings != null)
                {
                    RefS4bFormsDB db = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    result = db.deleteMapping(sequence);
                }
            }
            return result;
        }

        public ResponseModel GetUserTemplatesBySyncDate(HttpRequest request, DateTime? lastSyncDate)
        {
            const string METHOD_NAME = "RefS4BFormsRepository.GetUserTemplatesBySyncDate()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    long userId = long.Parse(request.Headers["UserId"]);
                    RefS4bFormsDB refS4bFormsDB = new RefS4bFormsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<long> activeTemplateSequences = refS4bFormsDB.GetFormSequencesByUserId(userId);
                    if (activeTemplateSequences != null)
                    {
                        RefS4BFormsSync refS4BFormsSync = new RefS4BFormsSync();
                        refS4BFormsSync.ActiveTemplates = activeTemplateSequences;
                        List<RefS4bFormsMin> modifiedTemplates = refS4bFormsDB.selectUserChangedRefFormsMin(lastSyncDate, userId);
                        if (modifiedTemplates!=null)
                        {
                            refS4BFormsSync.ModifiedTemplates = modifiedTemplates;
                        }
                        else
                        {
                            returnValue.Message = refS4bFormsDB.ErrorMessage;
                        }
                        refS4BFormsSync.ServerProcessingTime = DateTime.Now;
                        returnValue.TheObject = refS4BFormsSync;
                        returnValue.IsSucessfull = true;
                    }
                    else
                    {
                        returnValue.Message = refS4bFormsDB.ErrorMessage;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting User Templates By Sync Date.", ex);
            }
            return returnValue;
        }
    }
}
