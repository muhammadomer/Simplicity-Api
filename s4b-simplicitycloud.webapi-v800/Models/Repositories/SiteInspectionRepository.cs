using SimplicityOnlineWebApi.Models.Interfaces;
using System;
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
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class SiteInspectionRepository : ISiteInspectionRepository
    {
        
        private ILogger<SiteInspectionRepository> LOGGER;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public SiteInspectionRepository()
        {
            
        }

        public List<string> FindMatchingContractNos(string contractNo, HttpRequest request)
        {
            List<string> returnValue = null;

            ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
            if (settings != null)
            {
                SiteInspectionDB siteInspectionDB = new SiteInspectionDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                returnValue = siteInspectionDB.FindMatchingContractNos(contractNo);
            }

            return returnValue;
        }


        public List<SubmissionsDataFh> GetSubmissionsDataFhList(HttpRequest request)
        {
            List<SubmissionsDataFh> returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    SiteInspectionDB siteInspectionDB = new SiteInspectionDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = siteInspectionDB.GetSubmissionsDataFhList();
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }


        public SubmissionsDataFh GetBySequence(long sequence, HttpRequest request)
        {
            SubmissionsDataFh returnValue = null;

            ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
            if (settings != null)
            {
                SiteInspectionDB siteInspectionDB = new SiteInspectionDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                returnValue = siteInspectionDB.GetBySequence(sequence);

                if (returnValue != null)
                {
                    if (returnValue.CreatedBy > 0 || returnValue.LastAmendedBy > 0)
                    {
                        UserDetailsDB userDB = new UserDetailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        UserDetails userDetails = null;
                        if (returnValue.CreatedBy > 0)
                        {
                            userDetails = userDB.getUserByUserId(returnValue.CreatedBy);
                            if (userDetails != null)
                            {
                                returnValue.CreatedByUserName = userDetails.UserName;
                            }
                        }

                        if (returnValue.LastAmendedBy > 0)
                        {
                            userDetails = userDB.getUserByUserId(returnValue.LastAmendedBy);
                            if (userDetails != null)
                            {
                                returnValue.LastAmendedByUserName = userDetails.UserName;
                            }
                        }
                    }

                    if (returnValue.SiteInspectionImages != null)
                    {
                        returnValue.SiteInspectionImages.ForEach(x =>
                        {
                            if (String.IsNullOrWhiteSpace(x.FilePath) == false)
                            {
                                x.FileWWWurl = string.Format("{0}/{1}/{2}", settings.S4BFormsSubmissionRootFolderWWW, returnValue.SubmitNo, x.FilePath);
                            }
                        });
                    }
                }
                
            }

            return returnValue;
        }

        public SubmissionsImagesFh UploadImageWithSequence(SubmissionsImagesFh fileDetail, HttpRequest request, HttpResponse response)
        {
            
            bool retValue=false;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                List<string> fileAttachmentsPaths = new List<string>();

                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssff") + "_" + fileDetail.FileName.Replace(" ", string.Empty);
                if (fileDetail.Base64File != null )
                {                    
                    string filePathWithName = Path.Combine(settings.S4BFormsSubmissionRootFolderPath, fileDetail.SubmitNo, fileName);
                    string filePath= Path.Combine(settings.S4BFormsSubmissionRootFolderPath, fileDetail.SubmitNo, fileName);
                    if (fileDetail.IsBase64)
                    {
                        if (Utilities.SaveBase64File(fileDetail.Base64File, filePathWithName))
                        {
                            fileAttachmentsPaths.Add(filePathWithName);
                        }
                    }
                    else
                    {
                        if (Utilities.SaveFileFromRequest(request, fileDetail.FileName, filePath))
                        {
                            fileAttachmentsPaths.Add(filePath);
                        }
                    }
                }
                if (fileAttachmentsPaths.Count>0)
                {
                    //Here we go to insert data into table
                    fileDetail.FilePath = fileName;
                    if (fileDetail.Sequence < 0)
                    {
                        
                        SiteInspectionDB siteInspectionDB = new SiteInspectionDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        retValue = siteInspectionDB.InsertSubmissionImages(fileDetail);
                    }
                    else
                    {
                        SiteInspectionDB siteInspectionDB = new SiteInspectionDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        retValue = siteInspectionDB.UpdateSubmissionImages(fileDetail);
                    }

                    fileDetail.FileWWWurl = string.Format("{0}/{1}/{2}", settings.S4BFormsSubmissionRootFolderWWW, fileDetail.SubmitNo, fileName);
                }
                else
                {
                    fileDetail.FileWWWurl = "";
                }
            }
            catch (Exception ex)
            {
                 
            }
            return fileDetail;
        }

        public bool Insert(SubmissionsDataFh siteInspection, HttpRequest request, HttpResponse response)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    SiteInspectionDB siteInspectionDB = new SiteInspectionDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = siteInspectionDB.Insert(siteInspection);
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured in SiteInspectionRepository.Insert" + ex.Message + " " + ex.InnerException;
                returnValue = false;
            }
            return returnValue;
        }

        public bool Insert(S4BSubmissionsData2 submissionData, HttpRequest request, HttpResponse response)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    SiteInspectionDB siteInspectionDB = new SiteInspectionDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = siteInspectionDB.Insert(submissionData);
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured in SiteInspectionRepository.Insert" + ex.Message + " " + ex.InnerException;
                returnValue = false;
            }
            return returnValue;
        }

        public bool Update(SubmissionsDataFh siteInspection, HttpRequest request)
        {
            bool returnValue = false;
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
            if (settings != null)
            {
                siteInspection.LastAmendedBy = Int32.Parse(request.Headers["UserId"]);
                SiteInspectionDB siteInspectionDB = new SiteInspectionDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                returnValue = siteInspectionDB.Update(siteInspection);
            }
            return returnValue;
        }

        public bool GeneratePDF(long sequence, HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                SubmissionsDataFh submissionDataFh = new SiteInspectionRepository().GetBySequence(sequence, request);
                S4bFormSubmissions s4bFormSubmission = new S4bFormSubmissionRepository().getFormSubmissionBySubmitNo(request, submissionDataFh.SubmitNo);
                string templateId = s4bFormSubmission.RefNatForms.FormId;// "470969094";// "1023744743"; //TODO: Need to get this template id from database
                string templateFolderPath = Path.Combine(settings.S4BFormsRootFolderPath, templateId);
                string emptyJsonFilePath = Path.Combine(templateFolderPath, SimplicityConstants.S4BFormJsonFileName);
                RootObject coOrdinateSystem = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(emptyJsonFilePath));
                string pdfTemplateFilePath = Path.Combine(templateFolderPath, SimplicityConstants.S4BFormTemplateName);
                string submissionPath = Path.Combine(settings.S4BFormsSubmissionRootFolderPath, submissionDataFh.SubmitNo);
                PdfDocument pdfDoc = PdfReader.Open(pdfTemplateFilePath, PdfDocumentOpenMode.Import);
                PdfDocument pdfNewDoc = new PdfDocument();
                for (int page = 0; page < pdfDoc.Pages.Count; page++)
                {
                    pdfNewDoc.AddPage(pdfDoc.Pages[page]);
                }
                Dictionary<string, string> fieldMappingDictionary = submissionDataFh.GetFieldMappingDictionary();
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
                            if(!string.IsNullOrEmpty(fieldValue))
                            {
                                if (s4bFormControl.type == "text" || s4bFormControl.type=="optionList")
                                {
                                    XFont font = new XFont("Times New Roman", 10, XFontStyle.Regular);
                                    // Get the updated value from the dictionary and write it on the given X, Y
                                    if (s4bFormControl.fieldName.Contains("_date"))
                                    {
                                        DateTime dt = DateTime.MinValue;
                                        if (DateTime.TryParse(fieldValue, out dt))
                                        {
                                            fieldValue = dt.ToString("dd-MM-yyyy");
                                        }
                                    }
                                    gfx.DrawString(fieldValue, font, XBrushes.Black, point);

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
                string fileName = string.Format("{0}_{1}_{2}_{3}", submissionDataFh.SubmitNo,
                                  "V" + (++submissionDataFh.CreatedPDFCount).ToString(),
                                  DateTime.Now.ToString("yyyyMMddHHmm"),
                                  s4bFormSubmission.TemplateName).Replace("/", "_").Replace(" ", "") + ".pdf";
                string fileCompletePath = settings.TempUploadFolderPath + "/" + fileName;
                pdfNewDoc.Save(fileCompletePath);
                string parentFolderName = submissionDataFh.P1ContractNo + "," + settings.FilingCabinetS4BFormsFolder;
                string filingCabinetId = S4BFormsRepository.UploadToDrive(request, fileCompletePath, parentFolderName, fileName);
                if(!string.IsNullOrEmpty(filingCabinetId))
                {
                    submissionDataFh.FileCabId = filingCabinetId;
                    if (!new SiteInspectionRepository().UpdateFileCabIdAndPDFCount(submissionDataFh, request))
                    {
                        //TODO: Log Error
                    }
                }
                returnValue = true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        private bool UpdateFileCabIdAndPDFCount(SubmissionsDataFh submissionDataFh, HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    SiteInspectionDB siteInspectionDB = new SiteInspectionDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = siteInspectionDB.UpdateFileCabIdAndPDFCount(submissionDataFh);
                }
            }
            catch(Exception ex)
            {

            }
            return returnValue;

        }
    }
}
