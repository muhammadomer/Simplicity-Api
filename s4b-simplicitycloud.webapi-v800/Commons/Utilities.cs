using Microsoft.VisualBasic;
using SimplicityOnlineBLL.Entities;
using System;
using System.IO;
using System.Xml;
using SimplicityOnlineWebApi.BLL.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.ComponentModel;
using SimplicityOnlineWebApi.DAL;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Data.OleDb;
using SimplicityOnlineWebApi.Models.Repositories;
using System.IO.Compression;
using GemBox.Document;
using System.Globalization;
using SimplicityOnlineWebApi.ClientInvoice.Entities;
using System.Text;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using PdfSharp;
using PdfSharp.Pdf.IO;
using System.Diagnostics.Eventing.Reader;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SimplicityOnlineWebApi.Commons
{
    public static class Utilities
    {
        public static string Message { get; set; }

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ProjectSettings LoadConfigSettings(string configFilePath)
        {
            const string METHOD_NAME = "Utilities.LoadConfigSettings()";
            ProjectSettings returnValue = null;
            if (configFilePath == "" || !File.Exists(configFilePath))
            {
                return returnValue;
            }
            else
            {
                try
                {
                    returnValue = new ProjectSettings();
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(configFilePath);
                    string rootElement = @"/settings";
                    string connectionString = xmlDoc.SelectSingleNode(rootElement + "/ConnectionString/@value").Value;
                    returnValue.ConnectionString = connectionString;
                    returnValue.LinkServer = xmlDoc.SelectSingleNode(rootElement + "/LinkServer/@value").Value;
                    returnValue.HomePath = xmlDoc.SelectSingleNode(rootElement + "/HomePath/@value").Value;
                    returnValue.AttFileOrderDocsPath = xmlDoc.SelectSingleNode(rootElement + "/AttFileOrdDocPath/@value").Value;
                    returnValue.AttFileOrderDocsLocationDB = xmlDoc.SelectSingleNode(rootElement + "/AttFileOrdDocLocationDB/@value").Value;
                    returnValue.AdminEmailAddress = "support@simplicity4business.com";
                    if (xmlDoc.SelectSingleNode(rootElement + "/AdminEmailAddress/@value") != null)
                    {
                        returnValue.AdminEmailAddress = xmlDoc.SelectSingleNode(rootElement + "/AdminEmailAddress/@value").Value;
                    }
                    returnValue.IsDiaryAppFormsEnabled = false;
                    if (xmlDoc.SelectSingleNode(rootElement + "/IsDiaryAppFormsEnabled/@value") != null)
                    {
                        returnValue.IsDiaryAppFormsEnabled = Boolean.Parse(xmlDoc.SelectSingleNode(rootElement + "/IsDiaryAppFormsEnabled/@value").Value);
                    }
                    XmlNodeList formsNodes = xmlDoc.SelectNodes(rootElement + "/Forms/Form");
                    if (formsNodes.Count > 0)
                    {
                        List<NaturalForm> forms = new List<NaturalForm>();
                        int index = 0;
                        foreach (XmlNode formNode in formsNodes)
                        {
                            string formId = formsNodes[index].SelectSingleNode("@id").Value.Trim();
                            string formName = formsNodes[index].SelectSingleNode("@name").Value.Trim();
                            string isAssetRequired = formsNodes[index].SelectSingleNode("@asset").Value.Trim();
                            string isSupplier = formsNodes[index].SelectSingleNode("@supplier").Value.Trim();
                            NaturalForm form = new NaturalForm(formId, formName, Boolean.Parse(isAssetRequired), Boolean.Parse(isSupplier));
                            forms.Add(form);
                            index++;
                        }
                        returnValue.Forms = forms;
                    }
                    returnValue.TagImagePath = "";
                    if (xmlDoc.SelectSingleNode(rootElement + "/TagImageFilePath/@value") != null)
                    {
                        returnValue.TagImagePath = xmlDoc.SelectSingleNode(rootElement + "/TagImageFilePath/@value").Value;
                    }
                    returnValue.TagImageBaseURL = "";
                    if (xmlDoc.SelectSingleNode(rootElement + "/TagImagebaseURL/@value") != null)
                    {
                        returnValue.TagImageBaseURL = xmlDoc.SelectSingleNode(rootElement + "/TagImagebaseURL/@value").Value;
                    }
                    if (returnValue.LinkServer.ToString() != "")
                    {
                        returnValue.LinkServer = returnValue.LinkServer.ToString() + "...";
                    }
                    returnValue.SessionExpiryHours = 1; // No Of hours for session validity. default to 1 hour.
                    if (xmlDoc.SelectSingleNode(rootElement + "/SessionExpiryHours/@value") != null)
                    {
                        returnValue.SessionExpiryHours = Int32.Parse(xmlDoc.SelectSingleNode(rootElement + "/SessionExpiryHours/@value").Value);
                    }
                    returnValue.EmailAttachmentsPath = "";
                    if (xmlDoc.SelectSingleNode(rootElement + "/EmailAttachmentsPath/@value") != null)
                    {
                        returnValue.EmailAttachmentsPath = xmlDoc.SelectSingleNode(rootElement + "/EmailAttachmentsPath/@value").Value;
                    }
                    returnValue.FromEmailAddress = "";
                    if (xmlDoc.SelectSingleNode(rootElement + "/FromEmailAddress/@value") != null)
                    {
                        returnValue.FromEmailAddress = xmlDoc.SelectSingleNode(rootElement + "/FromEmailAddress/@value").Value;
                    }
                    returnValue.DatabaseType = "SQLSERVER";
                    if (xmlDoc.SelectSingleNode(rootElement + "/DatabaseType/@value") != null)
                    {
                        returnValue.DatabaseType = xmlDoc.SelectSingleNode(rootElement + "/DatabaseType/@value").Value.ToUpper();
                    }
                    returnValue.AttachmentFolderMode = "DATABASE";
                    if (xmlDoc.SelectSingleNode(rootElement + "/AttachmentFolderMode/@value") != null)
                    {
                        returnValue.AttachmentFolderMode = xmlDoc.SelectSingleNode(rootElement + "/AttachmentFolderMode/@value").Value.ToUpper();
                    }
                    returnValue.EmailAccount = string.Empty;
                    if (xmlDoc.SelectSingleNode(rootElement + "/EmailAccount/@value") != null)
                    {
                        returnValue.EmailAccount = xmlDoc.SelectSingleNode(rootElement + "/EmailAccount/@value").Value;
                    }
                    returnValue.UserAccount = string.Empty;
                    if (xmlDoc.SelectSingleNode(rootElement + "/UserAccount/@value") != null)
                    {
                        returnValue.UserAccount = xmlDoc.SelectSingleNode(rootElement + "/UserAccount/@value").Value;
                    }
                    returnValue.KeyFilePath = string.Empty;
                    if (xmlDoc.SelectSingleNode(rootElement + "/KeyFilePath/@value") != null)
                    {
                        returnValue.KeyFilePath = xmlDoc.SelectSingleNode(rootElement + "/KeyFilePath/@value").Value;
                    }
                    returnValue.RootFolder = string.Empty;
                    if (xmlDoc.SelectSingleNode(rootElement + "/RootFolder/@value") != null)
                    {
                        returnValue.RootFolder = xmlDoc.SelectSingleNode(rootElement + "/RootFolder/@value").Value;
                    }
                    returnValue.TempUploadFolderPath = string.Empty;
                    if (xmlDoc.SelectSingleNode(rootElement + "/TempUploadFolderPath/@value") != null)
                    {
                        returnValue.TempUploadFolderPath = xmlDoc.SelectSingleNode(rootElement + "/TempUploadFolderPath/@value").Value;
                    }
                    returnValue.AutoCreateJobRef = false;
                    if (xmlDoc.SelectSingleNode(rootElement + "/AutoCreateJobRef/@value") != null)
                    {
                        returnValue.AutoCreateJobRef = Convert.ToBoolean(xmlDoc.SelectSingleNode(rootElement + "/AutoCreateJobRef/@value").Value);
                    }
                    returnValue.EnableUploadDocumentFolderName = true;
                    if (xmlDoc.SelectSingleNode(rootElement + "/EnableUploadDocumentFolderName/@value") != null)
                    {
                        returnValue.EnableUploadDocumentFolderName = Convert.ToBoolean(xmlDoc.SelectSingleNode(rootElement + "/EnableUploadDocumentFolderName/@value").Value);
                    }
                    returnValue.UploadDocumentFolderName = "OrdersLabels";
                    if (xmlDoc.SelectSingleNode(rootElement + "/UploadDocumentFolderName/@value") != null)
                    {
                        returnValue.UploadDocumentFolderName = xmlDoc.SelectSingleNode(rootElement + "/UploadDocumentFolderName/@value").Value;
                    }
                    returnValue.JobRefNumberLength = 8;
                    if (xmlDoc.SelectSingleNode(rootElement + "/JobRefNumberLength/@value") != null)
                    {
                        returnValue.JobRefNumberLength = Int32.Parse(xmlDoc.SelectSingleNode(rootElement + "/JobRefNumberLength/@value").Value);
                    }

                    returnValue.ManualCreateJobRefForCreateOrder = false;
                    if (xmlDoc.SelectSingleNode(rootElement + "/ManualCreateJobRefForCreateOrder/@value") != null)
                    {
                        returnValue.ManualCreateJobRefForCreateOrder = bool.Parse(xmlDoc.SelectSingleNode(rootElement + "/ManualCreateJobRefForCreateOrder/@value").Value);
                    }
                    returnValue.FilingCabinetS4BFormsFolder = "S4BForms";
                    if (xmlDoc.SelectSingleNode(rootElement + "/FilingCabinetS4BFormsFolder/@value") != null)
                    {
                        returnValue.FilingCabinetS4BFormsFolder = xmlDoc.SelectSingleNode(rootElement + "/FilingCabinetS4BFormsFolder/@value").Value;
                    }
                    returnValue.S4BFormsRootFolderPath = "";
                    if (xmlDoc.SelectSingleNode(rootElement + "/S4BFormsRootFolderPath/@value") != null)
                    {
                        returnValue.S4BFormsRootFolderPath = xmlDoc.SelectSingleNode(rootElement + "/S4BFormsRootFolderPath/@value").Value;
                    }
                    returnValue.S4BFormsSubmissionRootFolderPath = "";
                    if (xmlDoc.SelectSingleNode(rootElement + "/S4BFormsSubmissionFolderPath/@value") != null)
                    {
                        returnValue.S4BFormsSubmissionRootFolderPath = xmlDoc.SelectSingleNode(rootElement + "/S4BFormsSubmissionFolderPath/@value").Value;
                    }
                    returnValue.S4BFormsSubmissionRootFolderWWW = "";
                    if (xmlDoc.SelectSingleNode(rootElement + "/S4BFormsSubmissionRootFolderWWW/@value") != null)
                    {
                        returnValue.S4BFormsSubmissionRootFolderWWW = xmlDoc.SelectSingleNode(rootElement + "/S4BFormsSubmissionRootFolderWWW/@value").Value;
                    }
                    returnValue.S4BFormsSubmissionDefaultJobRef = "S4BFORMSSUBMISSIONS";
                    if (xmlDoc.SelectSingleNode(rootElement + "/S4BFormsSubmissionDefaultJobRef/@value") != null)
                    {
                        returnValue.S4BFormsSubmissionDefaultJobRef = xmlDoc.SelectSingleNode(rootElement + "/S4BFormsSubmissionDefaultJobRef/@value").Value;
                    }
                    if (xmlDoc.SelectSingleNode(rootElement + "/SecondaryConnections/connection") != null)
                    {
                        XmlNodeList secondaryConnNodes = xmlDoc.SelectNodes(rootElement + "/SecondaryConnections/connection");
                        if (secondaryConnNodes.Count > 0)
                        {
                            Dictionary<string, DatabaseInfo> connections = new Dictionary<string, DatabaseInfo>();
                            foreach (XmlNode connNode in secondaryConnNodes)
                            {
                                DatabaseInfo dbInfo = new DatabaseInfo();
                                string connName = connNode.SelectSingleNode("@name").Value.Trim();
                                dbInfo.ConnectionString = connNode.SelectSingleNode("@value").Value.Trim();
                                dbInfo.DatabaseType = connNode.SelectSingleNode("@type").Value.Trim();
                                connections.Add(connName, dbInfo);
                            }
                            returnValue.SecondaryConnections = connections;
                        }
                    }
                    if (xmlDoc.SelectSingleNode(rootElement + "/SecondaryExportsFolders/ExportsFolder") != null)
                    {
                        XmlNodeList s4BFormsSubmissionsExportFolders = xmlDoc.SelectNodes(rootElement + "/SecondaryExportsFolders/ExportsFolder");
                        if (s4BFormsSubmissionsExportFolders.Count > 0)
                        {
                            Dictionary<string, string> exportFolders = new Dictionary<string, string>();
                            foreach (XmlNode connNode in s4BFormsSubmissionsExportFolders)
                            {
                                string exportFolderId = connNode.SelectSingleNode("@name").Value.Trim();
                                string exportFolderPath = connNode.SelectSingleNode("@value").Value.Trim();
                                exportFolders.Add(exportFolderId, exportFolderPath);
                            }
                            returnValue.SecondaryS4BFormsSubmissionsExportFolder = exportFolders;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Message = GenerateAndLogMessage(METHOD_NAME, "Unable to Load Configuration Settings.", ex);
                }
            }
            return returnValue;
        }

        internal static bool IsS4BFormTemplateSecondary(string formId)
        {
            bool returnValue = false;
            switch (formId)
            {
                case "360000168":
                case "360000167":
                case "1114043282":
                    returnValue = true;
                    break;
            }
            return returnValue;
        }

        internal static bool IsS4BeFormCreateJobRef(string formId)
        {
            bool returnValue = false;
            switch (formId)
            {
                case "470969094":
                case "1023744743":
                    returnValue = true;
                    break;
            }
            return returnValue;
        }

        internal static string GetSecondaryIdByS4BFormId(string formId)
        {
            string returnValue = "";
            switch (formId)
            {
                case "360000168":
                case "1114043282":
                    returnValue = SimplicityConstants.DB_CONNECTIONSTRINGS_CAPELPLANT_ID;
                    break;
                case "360000167":
                    returnValue = SimplicityConstants.DB_CONNECTIONSTRINGS_CAPELREC_ID;
                    break;
            }
            return returnValue;
        }

        internal static bool IsSmartAppCldSetting(CldSettings cldSetting)
        {
            bool returnValue = false;
            switch (cldSetting.SettingName)
            {
                case SimplicityConstants.CldSettingIsAppointmentsS4BFormsLinked:
                case SimplicityConstants.CldSettingS4BFormShowDiaryJobNotesBoth:
                case SimplicityConstants.CldSettingIsSuspendS4BFormsDownloadForUnfinishedSubmissins:
                case SimplicityConstants.CldSettingS4BFormsImageQuality:
                    returnValue = true;
                    break;
            }
            return returnValue;
        }

        internal static bool IsValidMailMergeTemplate(string contentType, out LoadOptions loadOption)
        {
            bool returnValue = false;
            loadOption = LoadOptions.DocxDefault;
            switch (contentType)
            {
                case "DOCX":
                case "XLSX":
                    returnValue = true;
                    loadOption = LoadOptions.DocxDefault;
                    break;
                case "PDF":
                    returnValue = true;
                    loadOption = LoadOptions.DocxDefault;
                    break;
                default:
                    break;
            }
            return returnValue;
        }

        internal static string GenerateAndLogMessage(string methodName, string message, Exception ex)
        {
            string returnValue = message;
            if (ex != null)
            {
                returnValue = returnValue + " In Method: " + methodName;
                returnValue = returnValue + " " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        internal static DatabaseInfo getSecondaryConnectionStringById(ProjectSettings settings, string subDatabaseId)
        {
            DatabaseInfo returnValue = null;
            if (settings != null && settings.SecondaryConnections != null & settings.SecondaryConnections.Count > 0)
            {
                foreach (KeyValuePair<string, DatabaseInfo> item in settings.SecondaryConnections)
                {
                    if (item.Key.Equals(subDatabaseId, StringComparison.InvariantCultureIgnoreCase))
                    {
                        returnValue = item.Value;
                    }
                }
            }
            return returnValue;
        }

        internal static string getDBWildCardByType(string datebaseType)
        {
            string returnValue = "%";
            switch (datebaseType)
            {
                case SimplicityConstants.DB_MSACCESS:
                    returnValue = "*";
                    break;
            }
            return returnValue;
        }

        public static String getOrderTemplatePath(ProjectSettings config, string path)
        {
            string returnValue = "";
            String[] findString = new String[1];
            findString[0] = @"\backup\".ToLower();
            string[] splittedPaths = path.ToLower().Split(findString, 2, StringSplitOptions.None);
            if (splittedPaths.Length > 1)
            {
                returnValue = config.HomePath + "BackUp\\" + splittedPaths[1];
            }
            else
            {
                returnValue = path;
            }
            return returnValue;
        }

        internal static ResponseModel generateResponseModelWithMessage(string message)
        {
            ResponseModel response = new ResponseModel();
            response.IsSucessfull = false;
            response.Message = message;
            return response;
        }

        internal static string getDBSqlString(string sql)
        {
            return String.IsNullOrWhiteSpace(sql) ? string.Empty : sql.Replace("'", "''");
        }

        internal static string formatDates(object jobDateDue, string format)
        {
            string returnValue = "N/A";
            if (jobDateDue != null && jobDateDue != DBNull.Value)
            {
                DateTime? jobDateDueNew = DateTime.MinValue;
                try
                {
                    jobDateDueNew = Convert.ToDateTime(jobDateDue.ToString());
                }
                catch (Exception ex)
                { }
                if (jobDateDueNew != DateTime.MinValue)
                {
                    returnValue = ((DateTime)jobDateDueNew).ToString(format);
                }
            }
            return returnValue;
        }

        internal static string generateLogoURL(ProjectSettings settings, string imageURL, string imageFileName)
        {
            return getLogoURLFromExtension(Configs.DocumentLogos, imageURL, Path.GetExtension(imageFileName));
        }

        public static void EmptyDirectory(this System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        public static string zipFiles(string sourceFolderPath, string targetFolderPath, string targetZipFileName, string[] excludeFileNames, string tempFolderPath)
        {
            string returnValue = "";
            try
            {
                if (Directory.Exists(tempFolderPath))
                {
                    DirectoryInfo directory = new DirectoryInfo(tempFolderPath);
                    directory.EmptyDirectory();
                }
                else
                {
                    Directory.CreateDirectory(tempFolderPath);
                }
                string[] fileEntries = Directory.GetFiles(sourceFolderPath);
                foreach (string filePath in fileEntries)
                {
                    bool isExcludeFile = false;
                    string fileName = Path.GetFileName(filePath);
                    if (excludeFileNames.Length > 0 && Array.IndexOf(excludeFileNames, fileName) > -1)
                    {
                        isExcludeFile = true;
                    }
                    if (!isExcludeFile)
                    {
                        File.Copy(filePath, Path.Combine(tempFolderPath, fileName));
                    }
                }
                ZipFile.CreateFromDirectory(tempFolderPath, Path.Combine(targetFolderPath, targetZipFileName));
                returnValue = Path.Combine(targetFolderPath, targetZipFileName);
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        private static string getLogoURLFromExtension(List<RefDocsLogos> documentLogos, string fileURL, string ext)
        {
            string returnLogoURL = "";
            string questionMarkURL = "";
            foreach (RefDocsLogos item in documentLogos)
            {
                if (item.Ext.Equals(""))
                {
                    if (!item.IsImgExt)
                    {
                        questionMarkURL = item.LogoURL;
                    }
                }
                else
                {
                    if (item.Ext.ToLower().Contains(ext.ToLower()))
                    {
                        if (item.IsImgExt)
                        {
                            returnLogoURL = fileURL;
                        }
                        else
                        {
                            returnLogoURL = item.LogoURL;
                        }
                    }
                }
            }
            if (ext.Equals("") || returnLogoURL.Equals(""))
            {
                returnLogoURL = questionMarkURL;
            }
            return returnLogoURL;
        }

        internal static string ExtractEmailAddressesFromEmailFields(HttpRequest request, string emailField, Dictionary<string, S4BFormsControl> s4bFormsControls)
        {
            const string METHOD_NAME = "Utilities.ExtractEmailAddressesFromEmailFields()";
            string returnValue = "";
            try
            {
                if (!string.IsNullOrEmpty(emailField))
                {
                    string[] splittedEmailAddresses = emailField.Split(SimplicityConstants.EmailAddressSeparator);
                    for (int index = 0; index < splittedEmailAddresses.Length; index++)
                    {
                        if (splittedEmailAddresses[index].Equals(SimplicityConstants.S4BFormKeywordSubmitterEmail, StringComparison.InvariantCultureIgnoreCase))
                        {
                            int userId = Int32.Parse(request.Headers["UserId"]);
                            UserDetails userDetail = new UserDetailsRepository(null).GetUserByUserId(userId, request);
                            if (userDetail != null)
                            {
                                if (!string.IsNullOrEmpty(userDetail.UserEmail))
                                {
                                    returnValue += userDetail.UserEmail;
                                }
                            }
                        }
                        else if (splittedEmailAddresses[index].Contains(SimplicityConstants.S4BFormKeywordDocumentVariable))
                        {
                            string[] splittedVariable = splittedEmailAddresses[index].Split('#');
                            if (splittedVariable.Length > 0)
                            {
                                returnValue += GetS4BFormFieldValueByFieldName(s4bFormsControls, splittedVariable[1]);
                            }
                        }
                        else
                        {
                            returnValue += splittedEmailAddresses[index];
                        }
                        returnValue += SimplicityConstants.EmailAddressSeparator;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = GenerateAndLogMessage(METHOD_NAME, "Error occured while Extracting Email Addresses from S4BForms Email Fields", ex);
            }
            return returnValue;
        }

        internal static bool IsS4BFormSubmissionHighRisk(Dictionary<string, S4BFormsControl> s4bFormsControls)
        {
            const string METHOD_NAME = "Utilities.IsS4BFormSubmissionHighRisk()";
            bool returnValue = false;
            try
            {
                string pageDeclaration = GetS4BFormFieldValueByFieldName(s4bFormsControls, SimplicityConstants.S4BFormFieldNamePageDeclaration);
                if (pageDeclaration.Equals(SimplicityConstants.S4BFormFieldValuePageDeclarationHighRisk, StringComparison.InvariantCultureIgnoreCase))
                {
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to establish if the submitted form is High Risk.", ex);
            }
            return returnValue;
        }

        internal static string GetS4BFormFieldValueByFieldName(Dictionary<string, S4BFormsControl> s4bFormsControls, string fieldName)
        {
            const string METHOD_NAME = "Utilities.GetS4BFormFieldValueByFieldName()";
            string returnValue = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(fieldName))
                {
                    if (s4bFormsControls.ContainsKey(fieldName))
                    {
                        S4BFormsControl fieldControl = s4bFormsControls[fieldName];
                        if (fieldControl.fieldValue != null)
                        {
                            returnValue = fieldControl.fieldValue.Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Unable to get '" + fieldName + "' from S4BFormsControl.", ex);
            }
            return returnValue;
        }

        internal static string GetDateValueForDML(string databaseType, DateTime? datValue)
        {
            string returnValue = "";
            switch (databaseType)
            {
                case "MSACCESS":
                    returnValue = datValue == DateTime.MinValue ? "null" : "#" + ((DateTime)datValue).ToString("MM/dd/yyyy") + "#";
                    break;
                case "SQLSERVER":
                    returnValue = datValue == DateTime.MinValue ? "null" : "'" + ((DateTime)datValue).ToString("yyyy-MM-dd") + "'";
                    break;
            }
            return returnValue;
        }

        internal static string GetDateTimeForDML(string databaseType, DateTime? datValue, bool isWithQuotesOrHashes, bool isWithTime)
        {
            string returnValue = "null";
            string quotesOrHash = "";
            string dateTimeFormat = "";
            if (datValue != null && datValue != DateTime.MinValue)
            {
                if (isWithTime)
                {
                    dateTimeFormat = " HH:mm:ss";
                }
                switch (databaseType)
                {
                    case "MSACCESS":
                        if (isWithQuotesOrHashes)
                        {
                            quotesOrHash = "#";
                        }
                        dateTimeFormat = "MM/dd/yyyy" + dateTimeFormat;
                        break;
                    case "SQLSERVER":
                    default:
                        if (isWithQuotesOrHashes)
                        {
                            quotesOrHash = "'";
                        }
                        dateTimeFormat = "yyyy-MM-dd" + dateTimeFormat;
                        break;
                }
                returnValue = quotesOrHash + ((DateTime)datValue).ToString(dateTimeFormat) + quotesOrHash;
            }
            return returnValue;
        }

        internal static string getSQLDate(DateTime? datValue)
        {
            string returnValue = (datValue == null || datValue == DateTime.MinValue ? "null" : "'" + ((DateTime)datValue).ToString("yyyy-MM-dd HH:mm:ss") + "'");
            return returnValue;
        }

        internal static string getSQLDateWithoutQuotes(DateTime? datValue)
        {
            string returnValue = (datValue == DateTime.MinValue ? "null" : ((DateTime)datValue).ToString("yyyy-MM-dd HH:mm:ss"));
            return returnValue;
        }

        internal static string getSQLDateWithoutTimeAndQuotes(DateTime? datValue)
        {
            string returnValue = (datValue == DateTime.MinValue ? "null" : ((DateTime)datValue).ToString("yyyy-MM-dd"));
            return returnValue;
        }

        internal static string getAccessDate(DateTime? datValue)
        {
            string returnValue = datValue == DateTime.MinValue ? "null" : "#" + ((DateTime)datValue).ToString("MM/dd/yyyy HH:mm:ss") + "#";
            return returnValue;
        }

        internal static string getAccessDateWithoutHashes(DateTime? datValue)
        {
            string returnValue = (datValue == DateTime.MinValue ? "null" : ((DateTime)datValue).ToString("MM/dd/yyyy HH:mm:ss"));
            return returnValue;
        }

        internal static string getAccessDateWithoutTimeAndHashes(DateTime? datValue)
        {
            string returnValue = (datValue == DateTime.MinValue ? "null" : ((DateTime)datValue).ToString("MM/dd/yyyy"));
            return returnValue;
        }

        internal static string replaceS4BFormsKeywords(string text, S4BFormRequest s4bFormRequest, S4bFormSubmissions s4bFormSubmissions, Dictionary<string, S4BFormsControl> s4bFormsControls)
        {
            string returnValue = text;
            returnValue = returnValue.Replace(SimplicityConstants.S4BFormKeywordTemplateId, s4bFormRequest.FormId);
            returnValue = returnValue.Replace(SimplicityConstants.S4BFormKeywordTemplateName, s4bFormSubmissions.TemplateName);
            returnValue = returnValue.Replace(SimplicityConstants.S4BFormKeywordDocumentName, s4bFormRequest.FormName);
            returnValue = returnValue.Replace(SimplicityConstants.S4BFormKeywordDocumentTimeStamp, s4bFormSubmissions.S4bSubmitTs);
            returnValue = returnValue.Replace(SimplicityConstants.S4BFormKeywordDocumentSubmitNumber, s4bFormSubmissions.S4bSubmitNo);
            // This Code is only supporting one Document variable in the content at the moment.
            if (returnValue.Contains(SimplicityConstants.S4BFormKeywordDocumentVariable))
            {
                int startIndexForDocumentVariable = returnValue.IndexOf(SimplicityConstants.S4BFormKeywordDocumentVariable);
                int endIndexForDocumentVariable = returnValue.IndexOf("#", startIndexForDocumentVariable + SimplicityConstants.S4BFormKeywordDocumentVariable.Length + 1);
                string documentVariableName = returnValue.Substring(startIndexForDocumentVariable + SimplicityConstants.S4BFormKeywordDocumentVariable.Length + 1, endIndexForDocumentVariable - startIndexForDocumentVariable - SimplicityConstants.S4BFormKeywordDocumentVariable.Length - 1);
                if (documentVariableName.Length > 0)
                {
                    string valueToBeReplaced = GetS4BFormFieldValueByFieldName(s4bFormsControls, documentVariableName);
                    returnValue = returnValue.Replace(SimplicityConstants.S4BFormKeywordDocumentVariable + "#" + documentVariableName + "#", valueToBeReplaced);
                }
                if (returnValue.Contains(SimplicityConstants.S4BFormKeywordDocumentVariable))
                {
                    startIndexForDocumentVariable = returnValue.IndexOf(SimplicityConstants.S4BFormKeywordDocumentVariable);
                    endIndexForDocumentVariable = returnValue.IndexOf("#", startIndexForDocumentVariable + SimplicityConstants.S4BFormKeywordDocumentVariable.Length + 1);
                    documentVariableName = returnValue.Substring(startIndexForDocumentVariable + SimplicityConstants.S4BFormKeywordDocumentVariable.Length + 1, endIndexForDocumentVariable - startIndexForDocumentVariable - SimplicityConstants.S4BFormKeywordDocumentVariable.Length - 1);
                    if (documentVariableName.Length > 0)
                    {
                        string valueToBeReplaced = GetS4BFormFieldValueByFieldName(s4bFormsControls, documentVariableName);
                        returnValue = returnValue.Replace(SimplicityConstants.S4BFormKeywordDocumentVariable + "#" + documentVariableName + "#", valueToBeReplaced);
                    }
                }
            }
            return returnValue;
        }

        public static string replaceSpecialChars(string inputString)
        {
            if (inputString == null) return "";
            string returnValue = inputString;
            try
            {
                returnValue = inputString.Replace("&", " ").Replace(",", " ");
                returnValue = returnValue.Replace("'", "`");
            }
            catch (Exception ex)
            {
                ex = ex;
            }
            return returnValue;
        }

        public static string replaceSpecialCharsForInsert(string inputString)
        {
            string returnValue = inputString;
            try
            {
                returnValue = inputString.Replace("&", " ").Replace(",", " ");
                returnValue = returnValue.Replace("'", "`");
            }
            catch (Exception ex)
            {
                ex = ex;
            }
            return returnValue;
        }

        public static string strUFFAFU(string strMagic)
        {
            int intChar;
            int intKeyChar;
            int intPosInString;
            string strMagicWord = "";
            string strKeyVal = "@ABBA@ABBA@ABBA@ABBA@ABBA@ABBA@ABBA@ABBA@";

            for (intPosInString = 0; intPosInString < strMagic.Length; intPosInString++)
            {
                intChar = 255 - Strings.AscW(strMagic.Substring(intPosInString, 1));
                intKeyChar = Strings.AscW(strKeyVal.Substring(intPosInString, 1));
                strMagicWord = strMagicWord + Strings.ChrW(intChar ^ intKeyChar);
            }
            return strMagicWord;
        }

        public static int GetBooleanForDML(string databaseType, bool val)
        {
            int returnValue = 0;
            if (databaseType.Equals(SimplicityConstants.DB_MSACCESS))
            {
                returnValue = val ? -1 : 0;
            }
            else
            {
                returnValue = val ? 1 : 0;
            }
            return returnValue;
        }

        public static int getSQLBoolean(bool val)
        {
            return val ? 1 : 0;
        }

        public static int getSQLBoolean(string dbType, bool val)
        {
            switch (dbType)
            {
                case SimplicityConstants.DB_MSACCESS:
                    return val ? -1 : 0;
                case SimplicityConstants.DB_MSSQLSERVER:
                    return val ? 1 : 0;
                default:
                    return val ? 1 : 0;
            }
        }

        public static string GenratePassword()
        {
            int[] numbers = new int[] {
                33,35,36,37,38,40,41,42,43,48,49,
                50,51,52,53,54,55,56,57,64,63,
                65,66,67,68,69,70,71,72,73,74,75,
                76,77,78,79,80,81,82,83,84,85,86,
                87,88,89,90,97,98,99,100,101,102,
                103,104,105,106,107,108,109,110,
                111,112,113,114,115,116,117,118,
                119,120,121,122,
            };
            var pass = "";
            Random rd = new Random();

            for (int i = 0; i < 12; i++)
            {
                foreach (var num in numbers)
                {
                    var randNum = rd.Next(30, 130);
                    if (randNum == num)
                    {
                        pass += Strings.ChrW(randNum);
                    }
                }

            }
            return pass;
        }

        public static string GetDBString(object val)
        {
            return (val == null || val == DBNull.Value) ? "" : val.ToString();
        }

        public static long GetDBLong(object val)
        {
            return (val == null || val == DBNull.Value || !Information.IsNumeric(val.ToString())) ? 0 : long.Parse(val.ToString());
        }

        public static void WriteLog(string message)
        {
            try
            {
                log.Info(message);
            }
            catch (Exception ex)
            {
            }
        }

        public static void WriteLog(string message, string methodName)
        {
            try
            {
                log.Info("\n  ----- METHOD: "+ methodName + "-----\n");
                log.Info(message);
                log.Info("\n -------\n");
            }
            catch (Exception ex)
            {
            }
        }
        internal static DateTime? getDBDate(object val)
        {

            if (val == null || val == DBNull.Value)
            {
                return null;
            }
            else
            {
                return DateTime.Parse(val.ToString());
            }
        }

        public static bool IsEmailValid(string email)
        {
            string MatchEmailPattern =
          @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
          + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
		    [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
          + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
		    [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
          + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";
            if (Regex.IsMatch(email, MatchEmailPattern))
                return true;
            else
                return false;
        }

        internal static bool SendMail(EmailContact fromAddress, List<EmailContact> toRecipients, List<EmailContact> ccRecipients,
                                      List<EmailContact> bccRecipients, string subject, string body, List<string> fileAttachmentsPaths,
                                      string header, string footer)
        {
            bool returnValue = false;
            Message = "";
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromAddress.EmailAddress, fromAddress.FullName, System.Text.Encoding.UTF8);
                if (toRecipients != null)
                {
                    foreach (EmailContact toRecipient in toRecipients)
                    {
                        if (!string.IsNullOrEmpty(toRecipient.EmailAddress))
                        {
                            message.To.Add(new MailAddress(toRecipient.EmailAddress));
                        }
                    }
                }
                if (ccRecipients != null)
                {
                    foreach (EmailContact ccRecipient in ccRecipients)
                    {
                        if (!string.IsNullOrEmpty(ccRecipient.EmailAddress))
                        {
                            message.CC.Add(new MailAddress(ccRecipient.EmailAddress));
                        }
                    }
                }
                if (bccRecipients != null)
                {
                    foreach (EmailContact bccRecipient in bccRecipients)
                    {
                        if (!string.IsNullOrEmpty(bccRecipient.EmailAddress))
                        {
                            message.Bcc.Add(new MailAddress(bccRecipient.EmailAddress));
                        }
                    }
                }
                message.IsBodyHtml = true;
                message.Subject = subject.Replace("\r\n", " "); //TODO: Need to make this fix better. V6.4.6
                string emailBody = header;
                emailBody = emailBody + @"<p style = ""font-size:15px;font-family:Tahoma"" >" + body + "</p>";
                emailBody = emailBody + footer;
                message.Body = emailBody;
                if (fileAttachmentsPaths != null)
                {
                    foreach (string filePath in fileAttachmentsPaths)
                    {
                        message.Attachments.Add(new Attachment(filePath));
                    }
                }
                Guid userToken = Guid.NewGuid();
                SmtpClient smtpServer = new SmtpClient(Configs.EmailSettings.HostName);
                smtpServer.Port = Configs.EmailSettings.HostPort;
                smtpServer.Credentials = new System.Net.NetworkCredential(Configs.EmailSettings.HostUser, Configs.EmailSettings.HostPassword);
                smtpServer.EnableSsl = Configs.EmailSettings.IsEnableSsl;
                smtpServer.SendCompleted += new SendCompletedEventHandler(SendMailCompletedCallback);
                smtpServer.SendAsync(message, userToken.ToString());
                returnValue = true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return returnValue;
        }

        internal static int GetUserIdFromRequest(HttpRequest request)
        {
            int returnValue = -1;
            Microsoft.Extensions.Primitives.StringValues userId;
            request.Headers.TryGetValue("UserId", out userId);
            if (userId.ToString() == "undefined")
            {
                Microsoft.Extensions.Primitives.StringValues webUserId;
                request.Headers.TryGetValue("WebId", out webUserId);
                if (webUserId.ToString() != "undefined")
                {
                    if (!string.IsNullOrEmpty(webUserId[0]) && Information.IsNumeric(webUserId[0]))
                    {
                        returnValue = Int32.Parse(webUserId[0]);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(userId[0]) && Information.IsNumeric(userId[0]))
                {
                    returnValue = Int32.Parse(userId[0]);
                }
            }
            return returnValue;
        }

        private static void SendMailCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            String token = (string)e.UserState;
            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            else if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
        }

        internal static double GetQuantityFromDelayHours(string hours)
        {
            double returnValue = 0;
            if (hours != null)
            {
                switch (hours.ToUpper())
                {
                    case "00:00":
                        returnValue = 0;
                        break;
                    case "00:15":
                        returnValue = 0.25;
                        break;
                    case "00:30":
                        returnValue = .5;
                        break;
                    case "00:45":
                        returnValue = .75;
                        break;
                    case "01:00":
                        returnValue = 1;
                        break;
                    case "01:15":
                        returnValue = 1.25;
                        break;
                    case "01:30":
                        returnValue = 1.5;
                        break;
                    case "01:45":
                        returnValue = 1.75;
                        break;
                    case "02:00":
                        returnValue = 2.00;
                        break;
                }
            }
            return returnValue;
        }

        internal static string StrHash(string vstrPlain)
        {
            string returnValue = "";
            int intCount;
            int intCount2;
            int intCount3;
            int intCount4;
            int[] intHash = new int[7];
            string strTemp;
            double dblHash = 0.0;
            double dblTemp;

            if (!string.IsNullOrEmpty(vstrPlain))
            {
                strTemp = vstrPlain;
                intCount = strTemp.Length;
                intHash[1] = Strings.AscW(strTemp.Substring(0, 1));
                for (intCount3 = 2; intCount3 <= 6; intCount3++)
                {
                    intCount4 = 1 + (intHash[intCount3 - 1] % intCount);
                    intHash[intCount3] = Strings.AscW(strTemp.Substring(intCount4 - 1, 1));
                    if (intHash[intCount3] == intHash[intCount3 - 1])
                    {
                        intHash[intCount3] = intHash[intCount3] + 1;
                    }
                }
                for (intCount2 = 1; intCount2 <= intCount; intCount2++)
                {
                    dblTemp = intCount2;
                    for (intCount3 = 1; intCount3 <= 6; intCount3++)
                    {
                        intCount4 = 1 + ((intCount2 + intHash[intCount3]) % intCount);
                        dblTemp = dblTemp * Strings.AscW(strTemp.Substring(intCount4 - 1, 1));
                    }
                    dblHash = dblHash + dblTemp;
                }
            }
            returnValue = dblHash.ToString();
            returnValue = returnValue.Substring(returnValue.Length - 8, 8);
            return returnValue;
        }

        internal static string StrCreateEnabler()
        {
            string returnValue = "";
            returnValue = ((DateTime.Now.Year * 1000) + IntDaysPassed(DateTime.Now.Month) + DateTime.Now.Day).ToString();
            return returnValue;
        }

        internal static int IntDaysPassed(int vintMonth)
        {
            int returnValue = 0;
            int[] intDays = new int[] { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
            if (vintMonth > 2 && DateTime.Now.Year % 4 == 0)
            {
                returnValue = intDays[vintMonth - 1] + 1;
            }
            else
            {
                returnValue = intDays[vintMonth - 1];
            }
            return returnValue;
        }

        internal static bool SaveBase64File(string based64BinaryString, string filePathWithName)
        {
            bool returnValue = false;
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePathWithName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePathWithName));
                }
                string fileBase64 = based64BinaryString.Replace("data:image/jpeg;base64,", " ");
                fileBase64 = fileBase64.Replace("data:image/png;base64,", " ");
                fileBase64 = fileBase64.Replace("data:text/plain;base64,", " ");
                fileBase64 = fileBase64.Replace("data:;base64,", " ");
                fileBase64 = fileBase64.Replace("data:application/octet-stream;base64,", " ");
                fileBase64 = fileBase64.Replace("data:application/zip;base64,", " ");
                File.WriteAllBytes(filePathWithName, Convert.FromBase64String(fileBase64));
                returnValue = true;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static bool SaveFileFromRequest(HttpRequest request, string fileName, string filePath)
        {
            bool returnValue = false;
            try
            {
                FileStream objFileStream = File.Open(filePath, FileMode.Open);
                IFormFile file = request.Form.Files.GetFile(fileName);
                if (file != null)
                {
                    file.CopyTo(objFileStream);
                    returnValue = true;
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static bool GenerateOrderPDFWithTagAndImages(ProjectSettings settings, Orders ord, string filePathWithName)
        {
            bool returnValue = false;
            try
            {
                if (ord != null)
                {
                    OrdersDB orderDB = new OrdersDB(GetDatabaseInfoFromSettings(settings, false, ""));
                    Orders order = orderDB.getOrderByJobSequence(ord.Sequence ?? 0);
                    if (order != null)
                    {
                        string stringToPrint = "Contract Reference: " + order.JobRef;
                        double pageMarginTop = 10;
                        double pageMarginLeft = 5;
                        double drawLocationX = pageMarginLeft, drawLocationY = pageMarginTop;
                        double IMAGE_HEIGHT = 300, IMAGE_WIDTH = 300;
                        PdfDocument document = new PdfDocument();
                        PdfPage page = document.AddPage();
                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        XFont fontHeadings = new XFont("Verdana", 20, XFontStyle.Bold);
                        XFont fontText = new XFont("Verdana", 16, XFontStyle.Regular);
                        gfx.DrawString(stringToPrint, fontHeadings, XBrushes.Black, new XRect(drawLocationX, drawLocationY, page.Width, page.Height), XStringFormats.TopLeft);
                        drawLocationY += MeasureHeight(gfx, stringToPrint, fontHeadings, page.Width.Point - drawLocationX);
                        if (ord.OI_FireProtection_I != null && ord.OI_FireProtection_I.Count > 0)
                        {
                            Cld_Ord_Labels_DB oI_FireProtection_IDB = new Cld_Ord_Labels_DB(GetDatabaseInfoFromSettings(settings, false, ""));
                            bool imageAddedToPage = false;
                            foreach (Cld_Ord_Labels tag in ord.OI_FireProtection_I)
                            {
                                Cld_Ord_Labels orderTag = oI_FireProtection_IDB.selectAllCld_Ord_LabelsBySequence(tag.Sequence ?? 0);
                                if (orderTag != null)
                                {
                                    if (imageAddedToPage)
                                    {
                                        page = document.AddPage();
                                        gfx = XGraphics.FromPdfPage(page);
                                        drawLocationY = pageMarginTop;
                                        imageAddedToPage = false;
                                    }
                                    stringToPrint = "Tag Number: " + orderTag.TagNo;
                                    gfx.DrawString(stringToPrint, fontHeadings, XBrushes.Black, new XRect(drawLocationX, drawLocationY, page.Width, page.Height), XStringFormats.TopLeft);
                                    drawLocationY += MeasureHeight(gfx, stringToPrint, fontHeadings, page.Width.Point - drawLocationX);
                                    if (tag.OI_FireProtection_I_Images != null && tag.OI_FireProtection_I_Images.Count > 0)
                                    {
                                        Cld_Ord_Labels_FilesDB oI_FireProtection_I_ImagesDB = new Cld_Ord_Labels_FilesDB(GetDatabaseInfoFromSettings(settings, false, ""));
                                        foreach (Cld_Ord_Labels_Files tagImg in tag.OI_FireProtection_I_Images)
                                        {
                                            Cld_Ord_Labels_Files tagImage = oI_FireProtection_I_ImagesDB.selectCld_Ord_Labels_FilesBySequence(tagImg.Sequence ?? 0);
                                            if (tagImage != null)
                                            {
                                                if (imageAddedToPage)
                                                {
                                                    page = document.AddPage();
                                                    gfx = XGraphics.FromPdfPage(page);
                                                    drawLocationY = pageMarginTop;
                                                }
                                                stringToPrint = "Image Description: " + tagImage.FileDesc;
                                                stringToPrint += "\nImage Date:" + ((DateTime)tagImage.FileDate).ToString("dd/MM/yyyy");
                                                stringToPrint += "\nImage User:" + tagImage.ImageUser;
                                                XTextFormatter tf = new XTextFormatter(gfx);
                                                tf.DrawString(stringToPrint, fontText, XBrushes.Black, new XRect(drawLocationX, drawLocationY, page.Width, page.Height), XStringFormats.TopLeft);
                                                drawLocationY += MeasureHeight(gfx, stringToPrint, fontText, page.Width.Point - drawLocationX);
                                                string imgFilePath = Path.Combine(settings.TagImagePath, tagImage.FileNameAndPath);
                                                if (File.Exists(imgFilePath))
                                                {
                                                    XImage img = XImage.FromFile(imgFilePath);
                                                    gfx.DrawImage(img, new XRect(drawLocationX, drawLocationY, IMAGE_WIDTH, IMAGE_HEIGHT));
                                                    drawLocationY += IMAGE_HEIGHT;
                                                    imageAddedToPage = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!Directory.Exists(Path.GetDirectoryName(filePathWithName)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(filePathWithName));
                        }
                        document.Save(filePathWithName);
                        document.Close();
                    }
                }
                returnValue = true;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static string FormatJobRefForCreateOrder(string jobRef)
        {
            string returnValue = jobRef;
            if (!string.IsNullOrWhiteSpace(returnValue))
            {
                returnValue = returnValue.Trim().ToUpper();
                returnValue = Utilities.replaceSpecialChars(returnValue);
                returnValue = returnValue.Replace(" ", "_");
            }
            return returnValue;
        }

        public static double MeasureHeight(this PdfSharp.Drawing.XGraphics gfx, string text, PdfSharp.Drawing.XFont font, double width)
        {
            var lines = text.Split('\n');
            double totalHeight = 0;
            foreach (string line in lines)
            {
                var size = gfx.MeasureString(line, font);
                double height = size.Height + (size.Height * Math.Floor(size.Width / width));
                totalHeight += height;
            }
            return totalHeight;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public static List<EmailContact> GetEmailContactsFromEmailAddresses(string emailAddresses)
        {
            List<EmailContact> returnValue = null;
            if (!string.IsNullOrEmpty(emailAddresses))
            {
                string[] splittedEmailAddresses = emailAddresses.Split(SimplicityConstants.EmailAddressSeparator);
                if (splittedEmailAddresses.Length > 0)
                {
                    returnValue = new List<EmailContact>();
                    for (int index = 0; index < splittedEmailAddresses.Length; index++)
                    {
                        splittedEmailAddresses[index] = splittedEmailAddresses[index].Trim();
                        if (!string.IsNullOrWhiteSpace(splittedEmailAddresses[index]))
                        {
                            EmailContact emailContact = new EmailContact();
                            emailContact.EmailAddress = splittedEmailAddresses[index];
                            returnValue.Add(emailContact);
                        }
                    }
                }
            }
            return returnValue;
        }

        public static int GetWoodvaleStatusByStatusDesc(string visitStatus)
        {
            int returnValue = -1;
            switch (visitStatus.ToUpper())
            {
                case "1.  WORK CARRIED OUT IMMEDIATELY.":
                    returnValue = 5;
                    break;
                case "2.  TEMP FIX & FURTHER WORK REQUIRED.":
                    returnValue = 6;
                    break;
                case "3.  NO TEMP FIX & FURTHER WORK REQUIRED.":
                    returnValue = 7;
                    break;
                case "4.  NO WORK REQUIRED.":
                    returnValue = 8;
                    break;
            }
            return returnValue;
        }

        public static int GetVisitStatusIdByStatusDesc(HttpRequest request, string visitStatus)
        {
            int returnValue = -1;
            RefVisitStatusTypesRepository repos = new RefVisitStatusTypesRepository();
            RefVisitStatusTypes refVisitStatusTypes = repos.GetRefVisitStatusTypeByDesc(request, visitStatus);
            if (refVisitStatusTypes != null)
            {
                returnValue = refVisitStatusTypes.StatusId;
            }
            return returnValue;
        }

        public static DiaryResources GetDiaryResourceByUserId(HttpRequest request, int userId)
        {
            DiaryResources returnValue = null;
            DiaryResourcesRepository repos = new DiaryResourcesRepository();
            returnValue = repos.GetDiaryResourceByUserId(request, userId);
            return returnValue;
        }

        public static long GetDBAutoNumber(OleDbConnection conn)
        {
            long returnValue = -1;
            try
            {
                string sql = "select @@IDENTITY";
                using (OleDbCommand objCommand =
                    new OleDbCommand(sql, conn))
                {
                    OleDbDataReader dr = objCommand.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        returnValue = long.Parse(dr[0].ToString());
                    }
                    else
                    {
                        //ErrorMessage = "Unable to get Auto Number after inserting the TMP OI FP Header Record.                                                 '" + METHOD_NAME + "'\n";
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        internal static bool IsValidEmailId(string InputEmail)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(InputEmail);
            if (match.Success)
                return true;
            else
                return false;
        }

        internal static OrdersMin GetSecondaryOrderByJobSequence(HttpRequest request, long jobSequence, string secondaryDatabaseId, out bool flgInvalidOrder)
        {
            const string METHOD_NAME = "Utilties.GetSecondaryOrderByJobSequence()";
            OrdersMin returnValue = null;
            flgInvalidOrder = false;
            try
            {
                OrdersRepository ordersRepository = new OrdersRepository(null);
                OrdersRepository ordersRepositorySecondary = new OrdersRepository(true, secondaryDatabaseId);
                Orders order = ordersRepository.GetOrderDetailsBySequence(jobSequence, request);
                if (order != null)
                {
                    string secondaryClientRef = order.JobRef;
                    if (!string.IsNullOrEmpty(secondaryClientRef))
                    {
                        List<OrdersMin> orderListSecondary = ordersRepositorySecondary.GetAllOrdersMinByJobClientRef(request, secondaryClientRef);
                        if (orderListSecondary != null && orderListSecondary.Count > 0)
                        {
                            returnValue = orderListSecondary[0];
                        }
                        else
                        {
                            if (new CldSettingsRepository().GetIsS4BFormJobRefPaddedByZeros(request))
                            {
                                if (Information.IsNumeric(secondaryClientRef) && secondaryClientRef.Length < 8)
                                {
                                    string paddedJobClientRef = secondaryClientRef.ToString().PadLeft(8, '0');
                                    orderListSecondary = new OrdersRepository(true, secondaryDatabaseId).GetAllOrdersMinByJobClientRef(request, paddedJobClientRef);
                                    if (orderListSecondary != null && orderListSecondary.Count > 0)
                                    {
                                        returnValue = orderListSecondary[0];
                                    }
                                }
                            }
                        }
                    }
                }
                if (returnValue == null)
                {
                    ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(secondaryDatabaseId);
                    if (settings != null)
                    {
                        List<OrdersMin> orderListSecondary = ordersRepositorySecondary.GetAllOrdersMinByJobRef(settings.S4BFormsSubmissionDefaultJobRef, request);
                        if (orderListSecondary != null && orderListSecondary.Count > 0)
                        {
                            flgInvalidOrder = true;
                            returnValue = orderListSecondary[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = GenerateAndLogMessage(METHOD_NAME, "Unable to Get Secondary Order By Job Sequence", ex);
            }
            return returnValue;
        }

        internal static string GenerateFullAddressFromEntityDetailsCoreObject(EntityDetailsCore edc)
        {
            string returnValue = "";
            if (!String.IsNullOrWhiteSpace(edc.AddressNo))
            {
                if (!string.IsNullOrWhiteSpace(returnValue))
                {
                    returnValue += Environment.NewLine;
                }
                returnValue += edc.AddressNo.Trim();
            }
            if (!String.IsNullOrWhiteSpace(edc.AddressLine1))
            {
                if (!string.IsNullOrWhiteSpace(returnValue))
                {
                    returnValue += Environment.NewLine;
                }
                returnValue += edc.AddressLine1.Trim();
            }
            if (!String.IsNullOrWhiteSpace(edc.AddressLine2))
            {
                if (!string.IsNullOrWhiteSpace(returnValue))
                {
                    returnValue += Environment.NewLine;
                }
                returnValue += edc.AddressLine2.Trim();
            }
            if (!String.IsNullOrWhiteSpace(edc.AddressLine3))
            {
                if (!string.IsNullOrWhiteSpace(returnValue))
                {
                    returnValue += Environment.NewLine;
                }
                returnValue += edc.AddressLine3.Trim();
            }
            if (!String.IsNullOrWhiteSpace(edc.AddressLine4))
            {
                if (!string.IsNullOrWhiteSpace(returnValue))
                {
                    returnValue += Environment.NewLine;
                }
                returnValue += edc.AddressLine4.Trim();
            }
            if (!String.IsNullOrWhiteSpace(edc.AddressLine5))
            {
                if (!string.IsNullOrWhiteSpace(returnValue))
                {
                    returnValue += Environment.NewLine;
                }
                returnValue += edc.AddressLine5.Trim();
            }
            if (!String.IsNullOrWhiteSpace(edc.AddressPostCode))
            {
                if (!string.IsNullOrWhiteSpace(returnValue))
                {
                    returnValue += Environment.NewLine;
                }
                returnValue += edc.AddressPostCode.Trim();
            }
            return returnValue;
        }

        internal static ProjectSettings GetProjectSettingsFromProjectId(string projectId)
        {
            ProjectSettings returnValue = null;
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                returnValue = Configs.settings[projectId];
            }
            return returnValue;
        }

        internal static DatabaseInfo GetDatabaseInfoFromSettings(ProjectSettings settings, bool isSecondaryDatabase, string secondaryDatabaseId)
        {
            DatabaseInfo returnValue = new DatabaseInfo();
            if (isSecondaryDatabase)
            {
                returnValue = settings.SecondaryConnections[secondaryDatabaseId];
            }
            else
            {
                returnValue.ConnectionString = settings.ConnectionString;
                returnValue.DatabaseType = settings.DatabaseType;
            }
            return returnValue;
        }

        private static double UltimateRoundingFunction(double amountToRound, double nearstOf, double fairness)
        {
            return Math.Floor(amountToRound / nearstOf + fairness) * nearstOf;
        }

        internal static double NearestOfQuarter(double amountToRound)
        {
            return UltimateRoundingFunction(amountToRound, 0.25, 0.5);
        }

        internal static Orders GetOrderFromS4BFormsControl(HttpRequest request, S4bFormSubmissions s4bFormSubmission, Dictionary<string, S4BFormsControl> s4bFormsControls)
        {
            const string METHOD_NAME = "Utilities.SetS4BFormsSubmissionsOrdersFromS4BFormsControl()";
            string jobRef = "";
            Orders returnValue = null;
            bool isJobRefVerified = false;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (s4bFormsControls.ContainsKey("FIELD_VAR_JOB_REFERENCE"))
                {
                    S4BFormsControl jobRefControl = s4bFormsControls["FIELD_VAR_JOB_REFERENCE"];
                    jobRef = jobRefControl.fieldValue.Trim();
                }
                else if (s4bFormsControls.ContainsKey("FIELD_PG1_JOB_REF"))
                {
                    S4BFormsControl jobRefControl = s4bFormsControls["FIELD_PG1_JOB_REF"];
                    jobRef = jobRefControl.fieldValue.Trim();
                }
                if (string.IsNullOrEmpty(jobRef))
                {
                    if (s4bFormsControls.ContainsKey("VARIABLE_PG1_JOB_SEQUENCE"))
                    {
                        S4BFormsControl jobSequenceControl = s4bFormsControls["VARIABLE_PG1_JOB_SEQUENCE"];
                        if (jobSequenceControl.fieldValue != null)
                        {
                            string jobSequenceControlValue = jobSequenceControl.fieldValue.Trim();
                            long jobSequence = long.Parse(jobSequenceControlValue);
                            returnValue = new OrdersRepository(null).GetOrderDetailsBySequence(jobSequence, request);
                            if (returnValue != null)
                            {
                                isJobRefVerified = true;
                            }
                        }
                    }
                }
                else
                {
                    returnValue = new OrdersRepository(null).GetOrderByJobRef(jobRef, request);
                    if (returnValue == null)
                    {
                        if (new CldSettingsRepository().GetIsS4BFormJobRefPaddedByZeros(request))
                        {
                            if (Information.IsNumeric(jobRef) && jobRef.Length < 8)
                            {
                                jobRef = jobRef.ToString().PadLeft(8, '0');
                                returnValue = new OrdersRepository(null).GetOrderByJobRef(jobRef, request);
                                if (returnValue != null)
                                {
                                    isJobRefVerified = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        isJobRefVerified = true;
                    }
                }
                if (!isJobRefVerified)
                {
                    string formId = "";
                    if (s4bFormSubmission != null && s4bFormSubmission.RefNatForms != null)
                    {
                        formId = s4bFormSubmission.RefNatForms.FormId;
                    }
                    if (!string.IsNullOrWhiteSpace(jobRef) && settings.AutoCreateJobRef && Utilities.IsS4BeFormCreateJobRef(formId))
                    {
                        returnValue = new OrdersRepository(null).CreateOrderByJobRef(jobRef, true, request, null);
                    }
                    else
                    {
                        jobRef = settings.S4BFormsSubmissionDefaultJobRef;
                        returnValue = new OrdersRepository(null).GetOrderByJobRef(jobRef, request);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured While Setting Order for S4BForms Submissions from S4BForms Control.", ex);
            }
            return returnValue;
        }

        internal static string GenerateS4BeFormZipFileName(S4bFormSubmissions s4bFormSubmission, int versionNo)
        {
            return string.Format("{0}_{1}_{2}_{3}", s4bFormSubmission.S4bSubmitNo,
                                                   "V" + versionNo, DateTime.Now.ToString("yyyyMMddHHmm"),
                                                   s4bFormSubmission.TemplateName).Replace("/", "_").Replace(" ", "") + ".zip";
        }

        internal static string GenerateS4BeFormPdfFileName(S4bFormSubmissions s4bFormSubmission, int versionNo)
        {
            return string.Format("{0}_{1}_{2}_{3}_{4}", s4bFormSubmission.S4bSubmitNo,
                                                                   "V" + versionNo, DateTime.Now.ToString("yyyyMMddHHmm")
                                                                  , s4bFormSubmission.Orders.JobRef, s4bFormSubmission.TemplateName).Replace("/", "_").Replace(" ", "") + ".pdf";
        }

        internal static string GenerateS4BeFormVideoZipFileName(long FormName, string TemplateName, int versionNo)
        {
            return string.Format("{0}_{1}_{2}_{3}", FormName,
                                                   "V" + versionNo, DateTime.Now.ToString("yyyyMMddHHmm"),
                                                   TemplateName).Replace("/", "_").Replace(" ", "") + ".zip";
        }

        #region MyRegion

        internal static string GetEncodeedString(string str)
        {
            return WebUtility.HtmlEncode(str);
        }
        
        internal static string CalculateAmount(decimal? value)
        {
            string showdec = "";
            string amount = "";
            if (value == null)
                showdec = "";
            else
                amount = Convert.ToString(value);

            //float.TryParse(amount,NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture.NumberFormat, out float num);

            if (amount != null && amount != "" && amount.Length > 0)
            {
                float num = float.Parse(amount, CultureInfo.InvariantCulture.NumberFormat);

                //if (Information.IsNumeric(num) == true)
                //{
                if (num != 0)
                    showdec = Strings.FormatNumber(num, 2, 0, 0, TriState.True);
                else
                        showdec = "0.00";
                //}
                //else
                //    showdec = "0.00";
            }
            else
                showdec = "0.00";

            return showdec;
        }

        internal static string AmountWithHtmlText(decimal? value)
        {
            decimal? mainValue = 0;
            if (value < 0)
                mainValue = (-1) * value;
            else
                mainValue = value;

            string str = CalculateAmount(mainValue);


            if (str != null && str.Length > 0)
            {
                string returnvalue = "";
                if (value < 0)
                    returnvalue = "-" + "&#163;" + str;
                else
                    returnvalue = "&#163;" + str;
                return returnvalue;
            }
            else
                return "";
        }

        internal static string AppendDataInvoiceDetailPdf(InvoiceDetailResponse detail, BankDetailsCompanyNoVatNoResponse bankDetails, string htmlFilestr)
        {
            string datastr = htmlFilestr;
            string invoiceTitle = ""; string invoiceFooterNote = "";
            if (detail.invoiceType == "Pro Forma")
            {
                invoiceTitle = "Pro Forma Invoice";
                invoiceFooterNote = "This is an Application for Payment. A VAT invoice will be issued after payment is received. (Amount £00.00 VAT £00.00 Total £00.00).";
            }
            else if (detail.invoiceType == "CN")
            {
                invoiceTitle = "Credit Note";
                invoiceFooterNote = "";
            }
            else if (detail.invoiceType == "R")
            {
                invoiceTitle = "Retention Invoice";
                invoiceFooterNote = "";
            }
            else
            {
                invoiceTitle = "Invoice";
                invoiceFooterNote = "";
            }
            datastr = datastr.Replace("[INVOICE_TITIE]", invoiceTitle);
            datastr = datastr.Replace("[FOOTER_INVOICE_NOTE]", "<br/><br /><div style='padding-top:5px;font-size: 12px;text-align:center'>" + invoiceFooterNote + "</div>");

            datastr = datastr.Replace("[INVOICE_DETAIL_TITIE]", detail.invoiceNo);
            datastr = datastr.Replace("[INVOICE_DATE]", detail.invoiceDate);
            datastr = datastr.Replace("[CLIENT_CODE]", GetEncodeedString(detail.nameLong));
            datastr = datastr.Replace("[JOB_REF]", GetEncodeedString(detail.jobRef));
            datastr = datastr.Replace("[JOB_ADDRESS]", GetEncodeedString(detail.jobAddress));
            datastr = datastr.Replace("[YOUR_REF_CODE]", GetEncodeedString(detail.jobClientRef));
            datastr = datastr.Replace("[TRADE_CODE]", GetEncodeedString(detail.jobTradeCode));
            datastr = datastr.Replace("[INVOICE_NO]", detail.invoiceNo);

            string startDateStr = (detail.jobDateStart == null || detail.jobDateStart == "") ? "" : Convert.ToDateTime(detail.jobDateStart).ToString();
            string endDateStr = (detail.jobDateFinish == null || detail.jobDateFinish == "") ? "" : Convert.ToDateTime(detail.jobDateFinish).ToString();
            string dates = startDateStr + " to " + endDateStr;
            datastr = datastr.Replace("[DATES_STR]", dates);

            datastr = datastr.Replace("[START_DATE]", (detail.jobDateStart == null || detail.jobDateStart == "") ? "" : Convert.ToDateTime(detail.jobDateStart).ToString("dd/MM/yyyy"));
            datastr = datastr.Replace("[FINISH_DATE]", (detail.jobDateFinish == null || detail.jobDateFinish == "") ? "" : Convert.ToDateTime(detail.jobDateFinish).ToString("dd/MM/yyyy"));

            string companyAddress = detail.invaddr;
            companyAddress = companyAddress.Replace("<br>", "<br/>");
            companyAddress = companyAddress.Replace("\r\n", "<br/>");
            datastr = datastr.Replace("[COMPANY_ADDRESS]", companyAddress);

            datastr = datastr.Replace("[CLIENT_COMPANY_ADDRESS]", "");

            // ADD invoice Detail items 
            if (detail.invoiceItems != null)
            {
                if (detail.invoiceItems.Count > 0)
                {
                    datastr = datastr.Replace("[INVOICE_DETAIL_ITEMS]", GetInvoiceTableBodyItems(detail.invoiceItems));
                }
                else
                    datastr = datastr.Replace("[INVOICE_DETAIL_ITEMS]", "");
            }
            else
                datastr = datastr.Replace("[INVOICE_DETAIL_ITEMS]", "");

            //Add total items
            datastr = AddTotalItems(datastr, detail);

            if (detail.footNote == null || detail.footNote.Trim() == "")
                datastr = datastr.Replace("[INVOICE_NOTE]", "");
            else
                datastr = datastr.Replace("[INVOICE_NOTE]", AddInvoiceNote(detail.footNote));

            // Add Payment Items 
            if (detail.invoicePayments != null)
            {
                if (detail.invoicePayments.Count > 0)
                    datastr = datastr.Replace("[PAYMENT_MODE_ITEMS]", GetPaymentTableBodyItems(detail.invoicePayments, detail.paidToDate));
                else
                    datastr = datastr.Replace("[PAYMENT_MODE_ITEMS]", "");
            }
            else
                datastr = datastr.Replace("[PAYMENT_MODE_ITEMS]", "");


            // Adding Bank Details
            datastr = AddFooterBankDetails(datastr, bankDetails);

            return datastr;
        }

        internal static string GetInvoiceTableBodyItems(List<InvoiceDetailItemsResponse> invoiceItems)
        {
            StringBuilder sb = new StringBuilder();
            foreach (InvoiceDetailItemsResponse item in invoiceItems)
            {
                sb.Append(InvoiceDetailItems(item));
            }
            return sb.ToString();
        }
                
        internal static string InvoiceDetailItems(InvoiceDetailItemsResponse itemRow)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr>");
            sb.Append("\r\n");
            sb.Append("	<td nowrap='nowrap' id='code-td' class='text-left'>	");
            sb.Append("\r\n");
            sb.AppendFormat("{0}", itemRow.flgRowIsText == true ? "" : itemRow.itemCode);
            sb.Append("\r\n");
            sb.Append("	</td>");
            sb.Append("\r\n");
            sb.Append("	<td nowrap='nowrap' id='description-td' class='text-left'>	");
            sb.Append("\r\n");
            sb.AppendFormat("{0}", GetEncodeedString(itemRow.itemDesc));
            sb.Append("\r\n");
            sb.Append("	</td>");
            sb.Append("\r\n");
            sb.Append("	<td nowrap='nowrap' id='unit-td' class='text-center'>	");
            sb.Append("\r\n");
            sb.AppendFormat("{0}", itemRow.itemUnits);
            sb.Append("\r\n");
            sb.Append("	</td>");
            sb.Append("\r\n");
            sb.Append("	<td nowrap='nowrap' id='qty-td' class='text-right'>	");
            sb.Append("\r\n");
            sb.AppendFormat("{0}", itemRow.itemQuantity);
            sb.Append("\r\n");
            sb.Append("	</td>");
            sb.Append("\r\n");
            sb.Append("	<td nowrap='nowrap' id='item-amt-td' class='text-right'>	");
            sb.Append("\r\n");
            sb.AppendFormat("		&#163;{0}", CalculateAmount(itemRow.amountPayment));
            sb.Append("\r\n");
            sb.Append("	</td>");
            sb.Append("\r\n");
            sb.Append("	<td nowrap='nowrap' id='total-amt-td' class='text-right'>	");
            sb.Append("\r\n");
            sb.AppendFormat("		&#163;{0}", CalculateAmount(itemRow.amountVat));
            sb.Append("\r\n");
            sb.Append("	</td>");
            sb.Append("\r\n");
            sb.Append("	<td nowrap='nowrap' id='inv-amt-td' class='text-right'>	");
            sb.Append("\r\n");
            sb.AppendFormat("		&#163;{0}", CalculateAmount(itemRow.amountPayment));
            sb.Append("\r\n");
            sb.Append("	</td>");
            sb.Append("\r\n");
            sb.Append("</tr>");
            sb.Append("\r\n");
            return sb.ToString();
        }

        internal static string AddTotalItems(string datastr, InvoiceDetailResponse detail)
        {
            if (detail.subtotalScheduledItems != null)
                datastr = datastr.Replace("[SUBTOTAL_SCHEDULED_ITEMS]", "&#163;" + CalculateAmount(detail.subtotalScheduledItems));
            else
                datastr = datastr.Replace("[SUBTOTAL_SCHEDULED_ITEMS]", "&#163;0.00");

            if (detail.invoiceAmount != null)
                datastr = datastr.Replace("[INVOICE_AMOUNT]", "&#163;" + CalculateAmount(detail.invoiceAmount));
            else
                datastr = datastr.Replace("[INVOICE_AMOUNT]", "&#163;0.00");

            if (detail.discount != null)
                datastr = datastr.Replace("[DISCOUNT]", "&#163;" + CalculateAmount(detail.discount));
            else
                datastr = datastr.Replace("[DISCOUNT]", "&#163;0.00");

            if (detail.retentionTotal != null)
                datastr = datastr.Replace("[RETENTION_TOTAL]", "&#163;" + CalculateAmount(detail.retentionTotal));
            else
                datastr = datastr.Replace("[RETENTION_TOTAL]", "&#163;0.00");

            if (detail.subTotal != null)
                datastr = datastr.Replace("[SUB_TOTAL]", "&#163;" + CalculateAmount(detail.subTotal));
            else
                datastr = datastr.Replace("[SUB_TOTAL]", "&#163;0.00");

            if (detail.vat != null)
                datastr = datastr.Replace("[VAT]", "&#163;" + CalculateAmount(detail.vat));
            else
                datastr = datastr.Replace("[VAT]", "&#163;0.00");

            if (detail.invoiceTotal != null)
                datastr = datastr.Replace("[INVOICE_TOTAL]", "&#163;" + CalculateAmount(detail.invoiceTotal));
            else
                datastr = datastr.Replace("[INVOICE_TOTAL]", "&#163;0.00");

            return datastr;
        }

        internal static string GetPaymentTableBodyItems(List<InvoiceDetailPaymentResponse> payments, decimal? paidTodateAmount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(AddPaymentHeader());
            foreach (InvoiceDetailPaymentResponse item in payments)
            {
                if (payments.Count == 1)
                    sb.Append(AddPaymentItemsRow(item, false));
                else
                {
                    bool isBorderAdded = !(payments.IndexOf(item) == (payments.Count - 1));
                    sb.Append(AddPaymentItemsRow(item, isBorderAdded));
                }
            }
            sb.Append(AddPaymentFooter(paidTodateAmount));
            return sb.ToString();
        }

        internal static string AddPaymentHeader()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='invoice-payment-div'>");
            sb.Append("\r\n");
            sb.Append("	<div id='invoice-payment-title'>Payments Mode</div>");
            sb.Append("\r\n");
            sb.Append("	<table width='100%' cellpadding='0' cellspacing='0'>");
            sb.Append("\r\n");
            sb.Append("		<thead>");
            sb.Append("\r\n");
            sb.Append("			<tr>");
            sb.Append("\r\n");
            sb.Append("				<td nowrap='nowrap' id='payment-date-td' class='text-left'>");
            sb.Append("\r\n");
            sb.Append("					Date");
            sb.Append("\r\n");
            sb.Append("				</td>");
            sb.Append("\r\n");
            sb.Append("				<td nowrap='nowrap' id='payment-type-td' class='text-left'>");
            sb.Append("\r\n");
            sb.Append("					Type");
            sb.Append("\r\n");
            sb.Append("				</td>");
            sb.Append("\r\n");
            sb.Append("				<td nowrap='nowrap' id='payment-reference-td' class='text-left'>");
            sb.Append("\r\n");
            sb.Append("					Reference");
            sb.Append("\r\n");
            sb.Append("				</td>");
            sb.Append("\r\n");
            sb.Append("				<td nowrap='nowrap' id='payment-amount-td' class='text-right'>");
            sb.Append("\r\n");
            sb.Append("					Amount");
            sb.Append("\r\n");
            sb.Append("				</td>");
            sb.Append("\r\n");
            sb.Append("			</tr>");
            sb.Append("\r\n");
            sb.Append("		</thead>");
            sb.Append("\r\n");
            sb.Append("		<tbody>");
            sb.Append("\r\n");
            return sb.ToString();
        }

        internal static string AddPaymentItemsRow(InvoiceDetailPaymentResponse paymentRow, bool isBorderAdd)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("                <tr>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='payment-date-td' class='text-left " + (isBorderAdd == false ? "no-border" : "") + "'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", paymentRow.entryDate);
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='payment-type-td' class='text-left " + (isBorderAdd == false ? "no-border" : "") + "'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", paymentRow.entryType);
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='payment-reference-td' class='text-left " + (isBorderAdd == false ? "no-border" : "") + "'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", paymentRow.invoicenoOrItemRef);
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='payment-amount-td' class='text-right " + (isBorderAdd == false ? "no-border" : "") + "'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", AmountWithHtmlText(paymentRow.entryAmtAllocated));
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                </tr>");
            sb.Append("\r\n");
            return sb.ToString();
        }

        internal static string AddPaymentFooter(decimal? paidTodateAmount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("			</tbody>");
            sb.Append("\r\n");
            sb.Append("            <tfoot>");
            sb.Append("\r\n");
            sb.Append("                <tr>");
            sb.Append("\r\n");
            sb.Append("                    <td colspan='3' style='border-top: 2px solid #0FA2DF;border-bottom: 2px solid #0FA2DF;'>");
            sb.Append("\r\n");
            sb.Append("                        Paid to Date");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td align='right' style='border-top: 2px solid #0FA2DF;border-bottom: 2px solid #0FA2DF;'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", AmountWithHtmlText(paidTodateAmount));
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                </tr>");
            sb.Append("\r\n");
            sb.Append("            </tfoot>");
            sb.Append("\r\n");
            sb.Append("        </table>");
            sb.Append("\r\n");
            sb.Append("    </div>");
            sb.Append("\r\n");
            return sb.ToString();
        }
        
        internal static string AddFooterBankDetails(string datastr, BankDetailsCompanyNoVatNoResponse bankDetails)
        {
            if (bankDetails == null)
            {
                datastr = datastr.Replace("[BANK_DETAILS]", "");
                return datastr;
            }

            StringBuilder sb = new StringBuilder();

            //[BANK_NAME]
            if (!string.IsNullOrEmpty(bankDetails.bankName))
                sb.AppendFormat("<div style='padding-top:5px;font-size: 12px;'>Owner Bank Name : </div>", bankDetails.bankName);

            //[BANK_SORT_CODE]
            if (!string.IsNullOrEmpty(bankDetails.bankSortCode))
                sb.AppendFormat("<div style='padding-top:5px;font-size: 12px;'>Owner Bank Sort Code : {0}</div>", bankDetails.bankSortCode);

            //[BANK_ACCOUNT_NO]
            if (!string.IsNullOrEmpty(bankDetails.bankAccountNo))
                sb.AppendFormat("<div style='padding-top:5px;font-size: 12px;'>Owner Bank Account No. : {0}</div>", bankDetails.bankAccountNo);

            //[BANK_ACCOUNT_NAME]
            if (!string.IsNullOrEmpty(bankDetails.bankAccountName))
                sb.AppendFormat("<div style='padding-top:5px;font-size: 12px;'>Owner Bank Account Name : {0}</div>", bankDetails.bankAccountName);

            //[COMPANY_REGISTRATION_NO]
            if (!string.IsNullOrEmpty(bankDetails.companyRegistrationNo))
                sb.AppendFormat("<div style='padding-top:5px;font-size: 12px;'>Company Registration No. : {0}</div>", bankDetails.companyRegistrationNo);

            //[VAT_NO]
            if (!string.IsNullOrEmpty(bankDetails.vatNo))
                sb.AppendFormat("<div style='padding-top:5px;font-size: 12px;'>VAT No. : {0}</div>", bankDetails.vatNo);

            datastr = datastr.Replace("[BANK_DETAILS]", sb.ToString());
            return datastr;
        }

        #region Old Code

        internal static string GetInvoiceTableBodyItems(List<InvoiceDetailItemsResponse> invoiceItems, decimal? amountSubTotal, decimal? ordersBillsAmountVat, decimal? amountTotal)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(AddInvoiceDetailItemTableHeader());
            foreach (InvoiceDetailItemsResponse item in invoiceItems)
            {
                sb.Append(AddInvoiceDetailItemTableBodyItems(item));
            }
            sb.Append(AddInvoiceDetailItemTableFooter(amountSubTotal, ordersBillsAmountVat, amountTotal));
            return sb.ToString();
        }

        internal static string AddInvoiceDetailItemTableHeader()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("    <div id='invoice-items-div'>");
            sb.Append("\r\n");
            sb.Append("        <table>");
            sb.Append("\r\n");
            sb.Append("            <thead>");
            sb.Append("\r\n");
            sb.Append("                <tr>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='code-td' class='text-left'>");
            sb.Append("\r\n");
            sb.Append("                        Code");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='description-td'>");
            sb.Append("\r\n");
            sb.Append("                        Description");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='unit-td' class='text-center'>");
            sb.Append("\r\n");
            sb.Append("                        Unit");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='qty-td' class='text-right'>");
            sb.Append("\r\n");
            sb.Append("                        Qty");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='item-amt-td' class='text-right'>");
            sb.Append("\r\n");
            sb.Append("                        Item Amt");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='total-amt-td' class='text-right'>");
            sb.Append("\r\n");
            sb.Append("                        Total Amt");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='inv-amt-td' class='text-right'>");
            sb.Append("\r\n");
            sb.Append("                        Inv Amt");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                </tr>");
            sb.Append("\r\n");
            sb.Append("            </thead>");
            sb.Append("\r\n");
            sb.Append("            <tbody>");
            sb.Append("\r\n");
            return sb.ToString();
        }

        internal static string AddInvoiceDetailItemTableBodyItems(InvoiceDetailItemsResponse itemRow)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("                <tr>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='code-td' class='text-left'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", itemRow.flgRowIsText == true ? "" : itemRow.itemCode);
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='description-td' class='text-left'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", GetEncodeedString(itemRow.itemDesc));
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='unit-td' class='text-center'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", itemRow.flgRowIsText == true ? "" : itemRow.itemUnits);
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='qty-td' class='text-right'>");
            sb.Append("\r\n");
            // sb.AppendFormat("                        {0}", itemRow.flgRowIsText == true ? "" : (itemRow.itemQuantity == 0 ? "" : Convert.ToString(itemRow.itemQuantity)));
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='item-amt-td' class='text-right'>");
            sb.Append("\r\n");
            //    sb.AppendFormat("                        {0}", itemRow.flgRowIsText == true ? "" : AmountWithHtmlText(itemRow.itemAmount));
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='total-amt-td' class='text-right'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", itemRow.flgRowIsText == true ? "" : AmountWithHtmlText(itemRow.amountPayment));
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='inv-amt-td' class='text-right'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", itemRow.flgRowIsText == true ? "" : AmountWithHtmlText(itemRow.amountPayment));
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                </tr>");
            sb.Append("\r\n");
            return sb.ToString();
        }

        internal static string AddInvoiceDetailItemTableFooter(decimal? amountSubTotal, decimal? ordersBillsAmountVat, decimal? amountTotal)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("			</tbody>");
            sb.Append("\r\n");
            sb.Append("            <tfoot>");
            sb.Append("\r\n");
            sb.Append("                <tr>");
            sb.Append("\r\n");
            sb.Append("                    <td colspan='6' style='border-top: 2px solid #0FA2DF;'>");
            sb.Append("\r\n");
            sb.Append("                        Net Total");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td align='right' style='border-top: 2px solid #0FA2DF;'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", AmountWithHtmlText(amountSubTotal));
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                </tr>");
            sb.Append("\r\n");
            sb.Append("                <tr>");
            sb.Append("\r\n");
            sb.Append("                    <td colspan='6'>");
            sb.Append("\r\n");
            sb.Append("                        VAT");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td align='right'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", AmountWithHtmlText(amountSubTotal));
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                </tr>");
            sb.Append("\r\n");
            sb.Append("                <tr>");
            sb.Append("\r\n");
            sb.Append("                    <td colspan='6' style='border-bottom: 2px solid #0FA2DF;'>");
            sb.Append("\r\n");
            sb.Append("                        Invoice Total");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td align='right' style='border-bottom: 2px solid #0FA2DF;'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", AmountWithHtmlText(amountSubTotal));
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                </tr>");
            sb.Append("\r\n");
            sb.Append("            </tfoot>");
            sb.Append("\r\n");
            sb.Append("        </table>");
            sb.Append("\r\n");
            sb.Append("    </div>");
            sb.Append("\r\n");
            return sb.ToString();
        }

        internal static string AddInvoiceNote(string invoiceNote)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='invoice-note-div'>");
            sb.Append("\r\n");
            sb.Append("    <div id='invoice-note-title'>Invoice Notes:</div>");
            sb.Append("\r\n");
            sb.AppendFormat("    <div id='invoice-note-data'>{0}</div>", invoiceNote);
            sb.Append("\r\n");
            sb.Append("</div>");
            sb.Append("\r\n");
            return sb.ToString();
        }
        
        #endregion

        internal static void GenerateInvoiceDetailPdf(string htmlContent, string basePath, string paymentStatus, string fileName, CompanyAddressWithDetailResponse address, bool? excludeLogo)
        {
            //PdfDocument pdf = PdfGenerator.GeneratePdf(htmlContent, PageSize.A4);
            PdfSharp.Pdf.PdfDocument pdf = PdfGenerator.GeneratePdf(htmlContent, PageSize.A4, 20, null, null, null);

            if (excludeLogo != null && excludeLogo != true)
            {
                if (pdf.PageCount >= 1)
                {
                    // Add Logo
                    PdfPage currentPage = pdf.Pages[0];
                    XGraphics gfx = XGraphics.FromPdfPage(currentPage);

                    string logoPath = basePath + @"\ClientInvoice\logo.png";
                    if (File.Exists(logoPath))
                    {
                        XImage logoImage = XImage.FromFile(logoPath);
                        gfx.DrawImage(logoImage, (double)400, (double)30);
                    }
                    // Add Payment Status
                    if (paymentStatus == "PAID")
                    {
                        string paymentImagePath = basePath + @"\ClientInvoice\paid.png";
                        if (File.Exists(paymentImagePath))
                        {
                            XImage paymentImage = XImage.FromFile(paymentImagePath);
                            gfx.DrawImage(paymentImage, (double)400, (double)80, (double)50, (double)50);
                        }
                    }
                    // Add address below Logo                    
                    //XFont font = new XFont("Verdana", 10, XFontStyle.Regular);
                    //XTextFormatter tf = new XTextFormatter(gfx);
                    //tf.Alignment = XParagraphAlignment.Right;
                    //tf.DrawString(address.AddressFull, font, XBrushes.Black, new XRect(470, 80, 100, 100), XStringFormats.TopLeft);
                    gfx.Save();
                }
            }
            pdf.Save(fileName);
        }

        internal static string AppendDataInvoiceStatementsPdf(StatementListResponse statement, string datastr, Parameters request)
        {
            string startDateStr = (request.startDate == null) ? "" : Convert.ToDateTime(request.startDate).ToString("dd/MM/yyyy");
            string endDateStr = (request.endDate == null) ? "" : Convert.ToDateTime(request.endDate).ToString("dd/MM/yyyy");
            string dates = "";
            if (startDateStr != null && startDateStr.Length > 0)
                dates = startDateStr;
            if (endDateStr != null && endDateStr.Length > 0)
                dates = ((startDateStr != null && startDateStr.Length > 0) ? dates + " to " : "") + endDateStr;
            datastr = datastr.Replace("[DATES_STR]", (endDateStr != null && endDateStr.Length > 0) ? dates : "");

            // ADD invoice statement items 
            if (statement.data != null)
            {
                if (statement.data.Count > 0)
                    datastr = datastr.Replace("[STATEMENT_ITEMS]", GetInvoiceStatementTableBodyItems(statement.data, statement.openingBalance, statement.closingBalance));
                else
                    datastr = datastr.Replace("[STATEMENT_ITEMS]", "");
            }
            else
                datastr = datastr.Replace("[STATEMENT_ITEMS]", "");

            return datastr;
        }

        internal static string GetInvoiceStatementTableBodyItems(List<StatementResponse> items, decimal? openingBalance, decimal? closingBalance)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("    <div id='invoice-items-div'>");
            sb.Append("\r\n");
            sb.Append("        <table>");
            sb.Append("\r\n");

            //Add Table header
            sb.Append("            <thead>");
            sb.Append("\r\n");
            sb.Append(AddInvoiceStatementItemTableHeader());
            sb.Append("            </thead>");
            sb.Append("\r\n");

            sb.Append("            <tbody>");
            sb.Append("\r\n");

            //Add Opening Balance
            sb.Append(AddInvoiceStatementOpeninBalance(openingBalance));

            //Add Statement Items
            foreach (StatementResponse item in items)
            {
                sb.Append(AddInvoiceStatementItems(item));
            }

            sb.Append("            </tbody>");
            sb.Append("\r\n");

            //Add Closing Balance with table footer
            sb.Append("            <tfoot>");
            sb.Append("\r\n");
            sb.Append(AddInvoiceStatementClosingBalance(closingBalance));
            sb.Append("            </tfoot>");
            sb.Append("\r\n");

            sb.Append("        </table>");
            sb.Append("\r\n");
            sb.Append("    </div>");
            sb.Append("\r\n");

            return sb.ToString();
        }

        internal static string AddInvoiceStatementItemTableHeader()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("                <tr>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='date-td' class='text-left'>");
            sb.Append("\r\n");
            sb.Append("                        Date");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='invoice-type-td' class='text-left'>");
            sb.Append("\r\n");
            sb.Append("                        Invoice Type");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='client-td'>");
            sb.Append("\r\n");
            sb.Append("                        Client");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='job-ref-td'>");
            sb.Append("\r\n");
            sb.Append("                        Job Ref");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='ref-td'>");
            sb.Append("\r\n");
            sb.Append("                        Invoice Ref");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='amt-td' class='text-right'>");
            sb.Append("\r\n");
            sb.Append("                        &nbsp;");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='amt-td' class='text-right'>");
            sb.Append("\r\n");
            sb.Append("                        &nbsp;");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='balance-td' class='text-right'>");
            sb.Append("\r\n");
            sb.Append("                        Balance");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                </tr>");
            sb.Append("\r\n");
            return sb.ToString();
        }

        internal static string AddInvoiceStatementOpeninBalance(decimal? openingBalance)
        {
            StringBuilder sb = new StringBuilder();
            if (openingBalance != null)
            {
                sb.Append("                <tr>");
                sb.Append("\r\n");
                sb.Append("                    <td nowrap='nowrap' id='date-td' class='text-left'></td>");
                sb.Append("\r\n");
                sb.Append("                    <td nowrap='nowrap' id='invoice-type-td' class='text-left'></td>");
                sb.Append("\r\n");
                sb.Append("                    <td nowrap='nowrap' id='client-td' class='text-left'></td>");
                sb.Append("\r\n");
                sb.Append("                    <td nowrap='nowrap' id='job-ref-td' class='text-left'></td>");
                sb.Append("\r\n");
                sb.Append("                    <td nowrap='nowrap' id='ref-td' class='text-left'>");
                sb.Append("\r\n");
                sb.Append("                        Opening Balance");
                sb.Append("\r\n");
                sb.Append("                    </td>");
                sb.Append("\r\n");
                sb.Append("                    <td nowrap='nowrap' id='amt-td' class='text-center'></td>");
                sb.Append("\r\n");
                sb.Append("                    <td nowrap='nowrap' id='amt-td' class='text-center'></td>");
                sb.Append("\r\n");
                sb.Append("                    <td nowrap='nowrap' id='balance-td' class='text-right'>");
                sb.Append("\r\n");
                sb.AppendFormat("                        {0}", AmountWithHtmlText(openingBalance));
                sb.Append("\r\n");
                sb.Append("                    </td>");
                sb.Append("\r\n");
                sb.Append("                </tr>");
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        internal static string AddInvoiceStatementItems(StatementResponse item)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("                <tr>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='date-td' class='text-left'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", item.date);
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='invoice-type-td' class='text-left'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", item.entryType);
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='client-td' class='text-left'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", item.client);
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='job-ref-td' class='text-left'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", item.jobRef);
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='ref-td' class='text-left " + (item.entryType == "SI" ? "blue-text" : "") + "'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", item.invoicenoOrItemRef);
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='amt-td' class='text-center'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", (item.firstBalance != null && item.firstBalance > 0 ? AmountWithHtmlText(item.firstBalance) : ""));
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("                    <td nowrap='nowrap' id='amt-td' class='text-center'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", (item.secondBalance != null && item.secondBalance > 0 ? AmountWithHtmlText(item.secondBalance) : ""));
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td nowrap='nowrap' id='balance-td' class='text-right'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", AmountWithHtmlText(item.balance));
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                </tr>");
            sb.Append("\r\n");
            return sb.ToString();
        }

        internal static string AddInvoiceStatementClosingBalance(decimal? closingBalance)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("                <tr>");
            sb.Append("\r\n");
            sb.Append("                    <td colspan='7' style='border-top: 2px solid #0FA2DF;border-bottom: 2px solid #0FA2DF;' align='right'>");
            sb.Append("\r\n");
            sb.Append("                        Closing Balance");
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                    <td style='border-top: 2px solid #0FA2DF;border-bottom: 2px solid #0FA2DF;' align='right'>");
            sb.Append("\r\n");
            sb.AppendFormat("                        {0}", AmountWithHtmlText(closingBalance));
            sb.Append("\r\n");
            sb.Append("                    </td>");
            sb.Append("\r\n");
            sb.Append("                </tr>");
            sb.Append("\r\n");

            return sb.ToString();
        }

        internal static void GenerateInvoiceStatementsPdf(string htmlContent, string basePath, string fileName)
        {
            PdfGenerateConfig config = new PdfGenerateConfig();
            config.PageOrientation = PageOrientation.Landscape;
            config.PageSize = PageSize.A4;

            PdfDocument pdf = PdfGenerator.GeneratePdf(htmlContent, config);

            if (pdf.PageCount >= 1)
            {
                string logoPath = basePath + @"\ClientInvoice\logo.png";
                if (File.Exists(logoPath))
                {
                    // Add Logo
                    PdfPage currentPage = pdf.Pages[0];
                    XGraphics gfx = XGraphics.FromPdfPage(currentPage);
                    XImage logoImage = XImage.FromFile(logoPath);
                    gfx.DrawImage(logoImage, (double)650, (double)30);
                    gfx.Save();
                }
            }
            pdf.Save(fileName);
        }

        internal static byte[] ReadAllBytes(string fileName)
        {
            byte[] buffer = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            return buffer;
        }

        #endregion

        public static RequestHeaderModel prepareRequestModel(HttpRequest request)
        {
            RequestHeaderModel header = new RequestHeaderModel();
            header.ProjectId = request.Headers["projectId"];
            header.UserId = Convert.ToInt32(request.Headers["UserId"]);

            return header;
        }

        public static ResponseModel DebugRequestBody(HttpRequest request)
        {
            string streamText;
            ResponseModel returnValue = new ResponseModel();
            using (StreamReader reader = new StreamReader(request.Body))
            {
                streamText = reader.ReadToEndAsync().Result;
            }
            var a = JsonConvert.DeserializeObject<DiaryAppsReturned>(streamText);
            returnValue.TheObject = a;
            return returnValue;
        }
    }


}
