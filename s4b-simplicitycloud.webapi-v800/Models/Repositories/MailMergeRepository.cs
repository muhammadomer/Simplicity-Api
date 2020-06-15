using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Models.Interfaces;
using GemBox.Document;
using Newtonsoft.Json;
using System.Linq;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class MailMergeRepository : IMailMergeRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public ResponseModel GetAllMailMergeCodes(HttpRequest Request)
        {
            const string METHOD_NAME = "MailMergeRepository.GetAllMailMergeCodes()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    MailMergeDB mailMergeDB = new MailMergeDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<RefMailMergeCodes> refMailMergeCodes = mailMergeDB.SelectAll();
                    if(refMailMergeCodes==null)
                    {
                        returnValue.Message = mailMergeDB.ErrorMessage;
                    }
                    else
                    {
                        returnValue.TheObject = refMailMergeCodes;
                        returnValue.IsSucessfull = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while getting Mail Merge Codes.", ex);
            }
            return returnValue;
        }

        public ResponseModel GetAllMailMergeCodesMin(HttpRequest Request)
        {
            const string METHOD_NAME = "MailMergeRepository.GetAllMailMergeCodesMin()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(Request.Headers["ProjectId"]);
                if (settings != null)
                {
                    MailMergeDB mailMergeDB = new MailMergeDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<RefMailMergeCodesMin> refMailMergeCodes = mailMergeDB.SelectAllMin();
                    if (refMailMergeCodes == null)
                    {
                        returnValue.Message = mailMergeDB.ErrorMessage;
                    }
                    else
                    {
                        returnValue.TheObject = refMailMergeCodes;
                        returnValue.IsSucessfull = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while getting Mail Merge Codes.", ex);
            }
            return returnValue;
        }

        public ResponseModel PerformMailMergeByJobRef(HttpRequest request, RequestModel reqModel)
        {
            const string METHOD_NAME = "MailMergeRepository.PerformMailMergeByJobRef()";
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    if (reqModel != null && reqModel.TheObject != null)
                    {
                        MailMergeRequest mailMergeRequest = JsonConvert.DeserializeObject<MailMergeRequest>(reqModel.TheObject.ToString());
                        if(mailMergeRequest!=null)
                        {
                            SimplicityFile templateFile = new AttachmentFilesFolderRepository().DowloadFile(mailMergeRequest.TemplateId, request, null);
                            LoadOptions loadOption;
                            if (Utilities.IsValidMailMergeTemplate(templateFile.ContentType, out loadOption))
                            {
                                if (templateFile.MemStream != null)
                                {
                                    List<OrderMailMergeCodes> orderMailMergeCodes = GetMailMergeDetailsByJob(request, mailMergeRequest.JobSequence ?? 0);
                                    DocumentModel document = DocumentModel.Load(templateFile.MemStream, loadOption);
                                    if (orderMailMergeCodes != null)
                                    {
                                        foreach (OrderMailMergeCodes orderMailMergeCode in orderMailMergeCodes)
                                        {
                                            foreach (ContentRange item in document.Content.Find(orderMailMergeCode.MergeCode).Reverse())
                                            {
                                                item.LoadText(string.Format(orderMailMergeCode.MergeValue));
                                            }
                                        }
                                    }
                                    document.Save("");
                                }
                                else
                                {
                                    Message = "Unable to Download Template from Filing Cabinet for File Id '" + mailMergeRequest.TemplateId + "'" ;
                                }
                            }
                            else
                            {
                                Message = "Invalid Template for Mail Merge. Supported templates are .DOT, .DOTX, .XLT and .XLTX.";
                            }
                        }
                        else
                        {
                            returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + reqModel.TheObject.ToString();
                        }
                    }
                    else
                    {
                        returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST + " " + reqModel.TheObject.ToString();
                    }
                }
                else
                {
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while getting Mail Merge Codes.", ex);
            }
            return returnValue;
        }

        public List<OrderMailMergeCodes> GetMailMergeDetailsByJob(HttpRequest request, long jobSequence)
        {
            const string METHOD_NAME = "MailMergeRepository.GetMailMergeDetailsByJob()";
            List<OrderMailMergeCodes> returnvalue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    MailMergeDB mailMergeDB = new MailMergeDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    List<RefMailMergeCodes> refMailMergeCodes = mailMergeDB.SelectAll();
                    if (refMailMergeCodes != null)
                    {
                        foreach(RefMailMergeCodes mergeCode in refMailMergeCodes)
                        {
                            if(!mergeCode.NewTableName.Equals("X", StringComparison.InvariantCultureIgnoreCase))
                            {
                                string sql = "";
                                switch (mergeCode.NewTableName.ToLower())
                                {
                                    case SimplicityConstants.DB_TABLE_ORDERS:
                                        sql = "SELECT " + mergeCode.NewColumnName + " FROM " + mergeCode.NewTableName + " " +
                                              " WHERE sequence = " + jobSequence;
                                        break;
                                }
                                if (!string.IsNullOrEmpty(sql))
                                {
                                    string value = mailMergeDB.GetDBValueFromQuery(sql);
                                    OrderMailMergeCodes orderMailMergeCodes = new OrderMailMergeCodes();
                                    orderMailMergeCodes.MergeCode = mergeCode.MergeCode;
                                    orderMailMergeCodes.MergeValue = value;
                                    if(returnvalue==null)
                                    {
                                        returnvalue = new List<OrderMailMergeCodes>();
                                    }
                                    returnvalue.Add(orderMailMergeCodes);
                                }
                            }
                        }
                    }
                    else
                    {
                        Message = "No Mail Merge Code Found.";
                    }
                }
            }
            catch(Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while getting Order Mail Merge Codes.", ex);
            }
            return returnvalue;
        }
    }
}
