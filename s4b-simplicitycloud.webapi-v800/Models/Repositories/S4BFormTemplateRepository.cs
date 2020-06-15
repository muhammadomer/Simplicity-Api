using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Commons;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;


using Newtonsoft.Json;
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using System.Text;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class S4BFormTemplateRepository : IS4BFormTemplateRepository
    {
        
        private ILogger<S4BFormTemplateRepository> LOGGER;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public S4BFormTemplateRepository()
        {
            
        }

        public ResponseModel GetTemplateBySequence(long sequence,HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    S4BFormTemplateDB formTemplateDB = new S4BFormTemplateDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    S4bFormSubmissions s4bFormSubmission = new S4bFormSubmissionRepository().getFormSubmissionBySequenceNo(request, sequence);
                    returnValue.TheObject = formTemplateDB.GetTemplateBySequence(sequence, s4bFormSubmission.S4bSubmitNo, settings.S4BFormsSubmissionRootFolderWWW);
                    if (returnValue.TheObject == null)
                    {
                        returnValue.Message = formTemplateDB.ErrorMessage;
                    }
                    else
                    {
                        returnValue.IsSucessfull = true;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_PROJECT_ID;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While Getting template data. " + ex.Message + " " + ex.InnerException;
                
            }
            return returnValue;
        }

        public ResponseModel Update(ExpandoObject templateData,long joinSequence, HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
            try
            {
                if (settings != null)
                {
                    long userId = Int32.Parse(request.Headers["UserId"]);
                    S4BFormTemplateDB formTemplateDB = new S4BFormTemplateDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue.IsSucessfull = formTemplateDB.Update(templateData, joinSequence, userId);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }
        public ResponseModel GeneratePDF(long sequence, HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel(); ;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                S4BFormTemplateDB formTemplateDB = new S4BFormTemplateDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                
                S4bFormSubmissions s4bFormSubmission = new S4bFormSubmissionRepository().getFormSubmissionBySequenceNo(request, sequence);
                ExpandoObject data = formTemplateDB.GetTemplateBySequence(sequence, s4bFormSubmission.S4bSubmitNo, settings.S4BFormsSubmissionRootFolderWWW);

                string templateId = s4bFormSubmission.RefNatForms.FormId;
                string templateFolderPath = Path.Combine(settings.S4BFormsRootFolderPath, templateId);
                string emptyJsonFilePath = Path.Combine(templateFolderPath, SimplicityConstants.S4BFormJsonFileName);
                RootObject coOrdinateSystem = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(emptyJsonFilePath));
                string pdfTemplateFilePath = Path.Combine(templateFolderPath, SimplicityConstants.S4BFormTemplateName);
                string submissionPath = Path.Combine(settings.S4BFormsSubmissionRootFolderPath, s4bFormSubmission.S4bSubmitNo);
                PdfDocument pdfDoc = PdfReader.Open(pdfTemplateFilePath, PdfDocumentOpenMode.Import);
                PdfDocument pdfNewDoc = new PdfDocument();
                for (int page = 0; page < pdfDoc.Pages.Count; page++)
                {
                    pdfNewDoc.AddPage(pdfDoc.Pages[page]);
                }
                Dictionary<string, string> fieldMappingDictionary= new Dictionary<string, string>();
                //----create dictionary of objects properties
                foreach (KeyValuePair<string, object> kvp in data)
                { 
                    if (kvp.Key != "TemplateImages")
                    {
                        fieldMappingDictionary.Add(kvp.Key, kvp.Value.ToString());
                    }else
                    {   
                        List<TemplateImages> imagesList = null;
                        imagesList = kvp.Value as List<TemplateImages>;

                        foreach (TemplateImages item in imagesList)
                        {
                            fieldMappingDictionary.Add(item.FieldName, item.FileName);
                        }

                    }
                }
                Dictionary<int, Page> s4bFormPages = new S4BFormsRepository().LoadS4BPages(coOrdinateSystem);
                Dictionary<int, XGraphics> pageGraphicMap = new Dictionary<int, XGraphics>();
                foreach (var pageItem in s4bFormPages)
                {
                    foreach (S4BFormsControl s4bFormControl in pageItem.Value.controls)
                    {
                        if (s4bFormControl.fieldName != null && fieldMappingDictionary.ContainsKey(s4bFormControl.fieldName.ToUpper()))
                        {
                            // Write the updated value
                            int currentPageNumber = pageItem.Key - 1; // -1 because page number start from 1 in json file
                            PdfPage currentPage = pdfNewDoc.Pages[currentPageNumber];
                            XGraphics gfx = null;
                            if (!pageGraphicMap.ContainsKey(currentPageNumber))
                            {
                                gfx = XGraphics.FromPdfPage(currentPage);
                                pageGraphicMap.Add(currentPageNumber, gfx);
                            }
                            else
                            {
                                gfx = pageGraphicMap[currentPageNumber];
                            }

                            XPoint point = new XPoint(s4bFormControl.bounds.x + .5, s4bFormControl.bounds.y + 10.1);
                            string fieldValue = fieldMappingDictionary[s4bFormControl.fieldName.ToUpper()];
                            if (!string.IsNullOrEmpty(fieldValue))
                            {
                                if (s4bFormControl.type == "text" || s4bFormControl.type == "optionList")
                                {
                                    //XFont font = new XFont("Times New Roman", 10, XFontStyle.Regular);
                                    XFont font = new XFont(coOrdinateSystem.view.defaultControlStyle.fontName, coOrdinateSystem.view.defaultControlStyle.fontSize - 4, XFontStyle.Regular);
                                    // Get the updated value from the dictionary and write it on the given X, Y
                                    if (s4bFormControl.fieldName.Contains("_date"))
                                    {
                                        DateTime dt = DateTime.MinValue;
                                        if (DateTime.TryParse(fieldValue, out dt))
                                        {
                                            fieldValue = dt.ToString("dd-MM-yyyy");
                                        }
                                    }
                                    //---If multiline then arrange text in multiline
                                    if (s4bFormControl.isMultiLine == true && fieldValue.Length > 100)
                                    {
                                        fieldValue = SplitLineToMultiline(fieldValue, 100);
                                    }
                                    XColor color = new XColor();
                                    color = XColor.FromArgb(0, 0, 0); //default color
                                    if (s4bFormControl.style != null)
                                    {
                                        if (s4bFormControl.style.color != null)
                                        {
                                            System.Drawing.Color _color = System.Drawing.ColorTranslator.FromHtml("#" + s4bFormControl.style.color);
                                            color = XColor.FromArgb(_color.ToArgb());
                                        }
                                    }
                                    XBrush redBrush = new XSolidBrush(color);
                                    if (s4bFormControl.isMultiLine == true) {
                                            XTextFormatter tf = new XTextFormatter(gfx);
                                            var layoutRectangle = new XRect(s4bFormControl.bounds.x + .5, s4bFormControl.bounds.y + 10.1, s4bFormControl.bounds.width, s4bFormControl.bounds.height);
                                            tf.DrawString(fieldValue, font, redBrush, layoutRectangle);
                                   } else { 
                                     gfx.DrawString(fieldValue, font, redBrush, point);
                                    }
                                }
                                else if (s4bFormControl.type == "image" || s4bFormControl.type == "signature")
                                {
                                    // Here we need to draw the image
                                    point.Y = point.Y - 10;
                                    string imagePath = Path.Combine(submissionPath, fieldValue);
                                    XImage image = XImage.FromFile(imagePath);
                                    gfx.DrawImage(image, point.X, point.Y, s4bFormControl.bounds.width, s4bFormControl.bounds.height);
                                }
                                else
                                {
                                    // Type is unknown
                                    continue;
                                }
                                gfx.Save();
                            }

                        }
                    }
                }
                //---save PDF file
                string fileName = string.Format("{0}_{1}_{2}_{3}", s4bFormSubmission.S4bSubmitNo,
                                  "V" + (++s4bFormSubmission.CreatedPDFCount).ToString(),
                                  DateTime.Now.ToString("yyyyMMddHHmm"),
                                  s4bFormSubmission.TemplateName).Replace("/", "_").Replace(" ", "") + ".pdf";
                string fileCompletePath = settings.TempUploadFolderPath + "/" + fileName;
                pdfNewDoc.Save(fileCompletePath);
                //----Update File Cab Id

                string parentFolderName = s4bFormSubmission.Orders.JobRef + "," + settings.FilingCabinetS4BFormsFolder;
                string filingCabinetId = S4BFormsRepository.UploadToDrive(request, fileCompletePath, parentFolderName, fileName);
                if (!string.IsNullOrEmpty(filingCabinetId))
                {
                    s4bFormSubmission.FileCabId = filingCabinetId;
                    if (UpdateFileCabIdAndPDFCount(s4bFormSubmission, request))
                    {
                        //TODO: Log Error
                    }
                }
                returnValue.IsSucessfull = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        private static string SplitLineToMultiline(string input, int rowLength)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder line = new StringBuilder();

            LinkedList<string> stack = new LinkedList<string>(input.Split(' '));
            var count = stack.Count;
            for (int i=0;i< count; i++)
            {
                var word = stack.First();
                stack.RemoveFirst();
                if (word.Length > rowLength)
                {
                    string head = word.Substring(0, rowLength);
                    string tail = word.Substring(rowLength);

                    word = head;
                    stack.AddLast(tail);
                }

                if (line.Length + word.Length > rowLength)
                {
                    result.AppendLine(line.ToString());
                    line.Clear();
                }

                line.Append(word + " ");
            }

            result.Append(line);

            return result.ToString();
        }

        public ResponseModel EmailSubmissionPdfFile(long sequence, HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                //---Read File cabinet id from database
                S4BFormTemplateDB formTemplateDB = new S4BFormTemplateDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                S4bFormSubmissions s4bFormSubmission = new S4bFormSubmissionRepository().getFormSubmissionInfoBySequenceNo(request, sequence);
                //----read json file for getting some values from document
                string templateId = s4bFormSubmission.RefNatForms.FormId;
                string templateFolderPath = Path.Combine(settings.S4BFormsRootFolderPath, templateId);
                string jsonFilePath = Path.Combine(templateFolderPath, SimplicityConstants.S4BFormJsonFileName);
                if (!File.Exists(jsonFilePath))
                {
                    returnValue.Message = jsonFilePath + " does not exist in the submission.";
                }
                RootObject s4bFormRootObject = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(jsonFilePath));
                Dictionary<String, S4BFormsControl> s4bFormsControls = new S4BFormsRepository().LoadS4BFormsControls(s4bFormRootObject);
                S4BFormRequest s4bFormRequest = new S4BFormRequest();
                s4bFormRequest.FormId = "";
                s4bFormRequest.FormName = s4bFormSubmission.SubmitDetails;
                //
                CldSettingsRepository cldSettingsRepository = new CldSettingsRepository();
                CldSettings emailSubjectSetting = null;
                CldSettings emailContentSetting = null;
                CldSettings emailFromSetting = null;
                //----Download File From Google Drive
                SimplicityFile downloadFile = null;
                if (s4bFormSubmission.FileCabId != null && s4bFormSubmission.clientEmail != "")
                {
                    DriveRequest driveRequest = new DriveRequest();
                    driveRequest.FileId = s4bFormSubmission.FileCabId;
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);
                        driveRequest.EmailAccount = settings.EmailAccount;
                        driveRequest.KeyFilePath = settings.KeyFilePath;
                        downloadFile = baseDataSource.Source.DownloadFile(driveRequest);
                        downloadFile.Base64String = Convert.ToBase64String(downloadFile.MemStream.ToArray());
                        downloadFile.Extension = System.IO.Path.GetExtension(downloadFile.FileName).ToLower();
                        //-----Email File
                        List<string> fileAttachmentsPaths = new List<string>();
                        string filePath = Path.Combine(settings.EmailAttachmentsPath, DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMddHHmmssff") + "_" + downloadFile.FileName);

                        if (Utilities.SaveBase64File(downloadFile.Base64String, filePath))
                        {
                            fileAttachmentsPaths.Add(filePath);
                        }
                        //---Get logged in user email address
                        int userId = Int32.Parse(request.Headers["UserId"]);
                        UserDetails userDetail = new UserDetailsRepository(null).GetUserByUserId(userId, request);
                        //---Get Cloud Settings
                        emailSubjectSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingS4BEFormsClientEmailSubjectJobTicket);
                        emailContentSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingS4BEFormsClientEmailContentJobTicket);
                        emailFromSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingS4BEFormsClientFromEmailJobTicket);
                        EmailOrderTags emailOptions = new EmailOrderTags();
                        EmailContact fromContact = new EmailContact();
                        if (userDetail.UserEmail.Trim().Length > 0 && userDetail.UserEmail != "N/A")
                        {
                            fromContact.EmailAddress = userDetail.UserEmail;
                        }
                        else
                        {
                            fromContact.EmailAddress = emailFromSetting.SettingValue;
                        }
                        emailOptions.From = new EmailContact();
                        emailOptions.From = fromContact;
                        EmailContact toContact = new EmailContact();
                        toContact.EmailAddress = s4bFormSubmission.clientEmail;
                        emailOptions.To = new List<EmailContact>();
                        emailOptions.To.Add(toContact);
                        emailOptions.Subject = emailSubjectSetting.SettingValue;
                        emailOptions.Subject = Utilities.replaceS4BFormsKeywords(emailOptions.Subject, s4bFormRequest, s4bFormSubmission, s4bFormsControls);
                        emailOptions.Body = Utilities.replaceS4BFormsKeywords(emailContentSetting.SettingValue,s4bFormRequest, s4bFormSubmission, s4bFormsControls);

                        if (Utilities.SendMail(emailOptions.From, emailOptions.To, emailOptions.Cc, emailOptions.Bcc, emailOptions.Subject, emailOptions.Body,
                                                fileAttachmentsPaths, "", ""))
                        {
                            returnValue.IsSucessfull = true;
                            returnValue.Message = "Email has been successfully Sent.";
                        }
                        else
                        {
                            returnValue.IsSucessfull = false;
                            returnValue.Message = "Unable to Send Email. " + Utilities.Message;
                        }
                    }
                }else
                {
                    returnValue.IsSucessfull = false;
                    returnValue.Message = "Unable to Send Email. File cabinet id or client email is not set" ;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Error Occured while Sending Email. " + ex.Message;
            }
            return returnValue;
        }

        private bool UpdateFileCabIdAndPDFCount(S4bFormSubmissions submissionData, HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    S4bFormSubmissionsDB submissionDB = new S4bFormSubmissionsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = submissionDB.UpdateFileCabIdAndPDFCount(submissionData);
                }
            }
            catch (Exception ex)
            {

            }
            return returnValue;

        }
    }
}
