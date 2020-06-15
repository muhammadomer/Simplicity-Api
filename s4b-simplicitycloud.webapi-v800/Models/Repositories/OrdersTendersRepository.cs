using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineBLL.Entities;
using System;
using System.Text;
using System.Collections.Generic;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.BLL.Entities;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class OrdersTendersRepository : IOrdersTendersRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        
       

        public List<OrdTendersTP> GetTendersSpecificationsByEntityId(long EntityId, bool flgFilterByStatus, int statusSequence,bool fliterOutFutureTender, bool flgFilterByFinalised, bool flgTenderFinalised, HttpRequest request)
        {
            List<OrdTendersTP> returnVal = new List<OrdTendersTP>();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectAllOrdersTendersByEntityId(EntityId, flgFilterByStatus, statusSequence, fliterOutFutureTender, flgFilterByFinalised, flgTenderFinalised);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

        public List<OrdTendersSpecs> GetTendersSpecificationsByClientId(long ClientId, bool flgFilterByStatus, int statusSequence, bool flgFilterByAwarded, bool flgTenderAwarded, HttpRequest request)
        {
            List<OrdTendersSpecs> returnVal = new List<OrdTendersSpecs>();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectAllOrdersTendersByJobClientId(ClientId, flgFilterByStatus, statusSequence, flgFilterByAwarded, flgTenderAwarded);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

        public List<OrdTendersSpecs> GetTendersSpecificationsByViewerId(long ViewerId, bool flgFilterByStatus, int statusSequence, bool flgFilterByAwarded, bool flgTenderAwarded, HttpRequest request)
        {
            List<OrdTendersSpecs> returnVal = new List<OrdTendersSpecs>();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectAllOrdersTendersByViewerId(ViewerId, flgFilterByStatus, statusSequence, flgFilterByAwarded, flgTenderAwarded);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }


        public List<OrdTendersTP> GetTenderTPDetailsBySequence(long sequence, bool loadAll, HttpRequest request)
        {
            List<OrdTendersTP> returnVal = new List<OrdTendersTP>();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectTenderTPDetailsBySequence(sequence, loadAll);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

        public OrdTendersTPNotifications GetTenderTPDetailsBySequenceForNotifications(long sequence, HttpRequest request)
        {
            OrdTendersTPNotifications returnVal = new OrdTendersTPNotifications();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectTenderTPDetailsBySequenceForNotifications(sequence);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

        public OrdTendersSpecs GetTenderDetailsBySequence(long sequence, bool loadAll, HttpRequest request)
        {
            OrdTendersSpecs returnVal = new OrdTendersSpecs();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectOrderTenderDetailsBySequence(sequence, loadAll);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

        public OrdTendersSpecsClient GetTenderDetailsBySequence4Client(long sequence, bool loadAll, HttpRequest request)
        {
            OrdTendersSpecsClient returnVal = new OrdTendersSpecsClient();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectOrderTenderDetailsBySequence4Client(sequence, loadAll);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

        public List<OrdTendersSpecsFiles> GetTenderSpecsFilesByTenderSpecSequence(long tenderSpecSequence, HttpRequest request)
        {
            List<OrdTendersSpecsFiles> returnVal = new List<OrdTendersSpecsFiles>();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectAllOrdersTendersSpecsFilesByTenderSpecSequence(tenderSpecSequence);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

        public List<OrdTendersTPQS> GetTendersQAsByTenderTPSequence(long sequence, HttpRequest request)
        {
            List<OrdTendersTPQS> returnVal = new List<OrdTendersTPQS>();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectOrdTendersQAByTenderTPSequence(sequence);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

        public OrdTendersTPQS GetTenderQuestionDetail(long sequence, HttpRequest request)
        {
            OrdTendersTPQS returnVal = new OrdTendersTPQS();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectOrdTendersQABySequence(sequence);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

        public List<OrdTendersSpecsFiles> GetTenderSpecsFilesByTenderSpecSequenceAndLatestVersion(long tenderSpecSequence, HttpRequest request)
        {
            List<OrdTendersSpecsFiles> returnVal = new List<OrdTendersSpecsFiles>();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectAllOrdersTendersSpecsFilesByTenderSpecSequenceAndLatestVersion(tenderSpecSequence);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

        public List<OrdTendersSpecsFiles> GetTenderSpecsFilesByGuId(long guId, HttpRequest request)
        {
            List<OrdTendersSpecsFiles> returnVal = new List<OrdTendersSpecsFiles>();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectAllOrdersTendersSpecsFilesByGUId(guId);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

        public List<OrdTendersTPFiles> GetTendersTPFilesByTenderTPSequence(long tenderTPSequence, HttpRequest request)
        {
            List<OrdTendersTPFiles> returnVal = new List<OrdTendersTPFiles>();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectAllOrdersTendersTPFilesByTenderTPSequence(tenderTPSequence);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

        public OrdTendersTPFiles GetTendersTPFilesBySequence(long tpFileSequence, HttpRequest request)
        {
            OrdTendersTPFiles returnVal = new OrdTendersTPFiles();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectTendersTPFilesByFileSequence(tpFileSequence);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

        public List<OrdTendersTPFiles> GetTendersTPFilesByGuId(long guId, HttpRequest request)
        {
            List<OrdTendersTPFiles> returnVal = new List<OrdTendersTPFiles>();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectAllOrdersTendersTPFilesByGUId(guId);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

        /// <summary>
        /// This Method will email the Tender Notification to Job Manager 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="s4bFormRequest"></param>
        /// <param name="s4bFormSubmission"></param>
        /// <param name="s4bFormContentsPath"></param>
        /// <param name="s4bFormsControls"></param>
        /// <returns></returns>
        public  bool SendTenderNotificationToJobManager(long sequence,long jobSequence,long tpQsSequence, long tpFileSequence,string notificationType, HttpRequest request,HttpResponse response)
        {
            bool returnValue = false;
            Message = "";
            try
            {
                string emailAddresses = "";
                Orders order = new OrdersRepository(null).GetOrderDetailsBySequence(jobSequence, request);
                OrdTendersTPNotifications tender = new OrdersTendersRepository().GetTenderTPDetailsBySequenceForNotifications(sequence, request);

                if (jobSequence > 0)
                {
                    if (order.JobManagerId > 0)
                    {
                        EntityDetailsCore jobManager = new EntityDetailsCoreRepository().GetEntityByEntityId(request, order.JobManagerId);
                        emailAddresses = jobManager.Email;
                        tender.JobManager = jobManager.NameLong;
                        int webId = Int32.Parse(request.Headers["WebId"]);
                        WebThirdParties returnedWebUsers = new WebThirdPartiesRepository().GetUsersByUserId(webId, request, response);
                        if(returnedWebUsers!=null)
                            tender.UserName = returnedWebUsers.UserName;
                    }
                    //---get posted question detail
                    if (tpQsSequence > 0)
                    {
                        OrdTendersTPQS questionDetail = this.GetTenderQuestionDetail(tpQsSequence, request);
                        if (questionDetail!=null)
                        {
                            tender.QuestionPosted = questionDetail.TPQuestion.ToString();
                            tender.QuestionAnswerPosted = questionDetail.OwnerAnswer.ToString();
                            tender.DateQuestionPosted = questionDetail.DateLastAmended;
                        }
                    }
                    //---get uploaded file details
                    if (tpFileSequence > 0)
                    {
                        OrdTendersTPFiles fileDetail = this.GetTendersTPFilesBySequence(tpFileSequence, request);
                        if (fileDetail != null)
                        {
                            tender.UploadedFileName = fileDetail.FileName.ToString();
                            tender.UploadedFileDesc = fileDetail.FileDesc.ToString();
                            tender.UploadedFileVersion = fileDetail.VersionNo.ToString();
                            tender.DateFileUploaded = fileDetail.DateLastAmended;
                        }
                    }

                    if (string.IsNullOrEmpty(emailAddresses))
                    {
                        emailAddresses = new CldSettingsRepository().GetDefaultEmailForDistribution(request);
                    }

                    if (!string.IsNullOrEmpty(emailAddresses))
                    {
                        
                        CldSettingsRepository cldSettingsRepository = new CldSettingsRepository();
                        CldSettings emailSubjectSetting=null;
                        CldSettings emailContentSetting=null;
                        CldSettings emailHeaderContent =  cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingEmailHeaderContent);
                        CldSettings emailFooterContent =  cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingEmailFooterContent);


                        if (notificationType == "Accepted")
                        {
                            emailSubjectSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingTenderAcceptedEmailSubject);
                            emailContentSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingTenderAcceptedEmailContent);
                        }
                        if (notificationType == "Declined")
                        {
                            emailSubjectSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingTenderDeclinedEmailSubject);
                            emailContentSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingTenderDeclinedEmailContent);
                        }
                        if (notificationType == "Finalized")
                        {
                            emailSubjectSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingTenderFinalizeEmailSubject);
                            emailContentSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingTenderFinalizeEmailContent);
                        }
                        if (notificationType == "QuestionPosted")
                        {
                            emailSubjectSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingTenderPostQuestionEmailSubject);
                            emailContentSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingTenderPostQuestionEmailContent);
                        }
                        if (notificationType == "FileUploaded")
                        {
                            emailSubjectSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingTenderFileUploadedEmailSubject);
                            emailContentSetting = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingTenderFileUploadedEmailContent);
                        }


                        ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                        
                        string emailSubject = emailSubjectSetting.SettingValue;
                        string emailContent = emailContentSetting.SettingValue;

                        //
                        emailContent = PopulateBody(emailContent,tender);
                        //
                        EmailContact fromContact = new EmailContact();
                        fromContact.EmailAddress = settings.AdminEmailAddress;
                        //----Emprt file attachment
                        List<string> fileAttachments = new List<string>();
                        //
                        if (Utilities.SendMail(fromContact, Utilities.GetEmailContactsFromEmailAddresses(emailAddresses),
                                               null,
                                               null,
                                               emailSubject, emailContent, fileAttachments, emailHeaderContent.SettingValue, emailFooterContent.SettingValue))
                        {
                            returnValue = true;
                            Message = "Email has been sent successfully";
                        }
                        else
                        {
                            returnValue = false;
                            Message = "Error occured in sending email ";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            return returnValue;
        }

        private string PopulateBody(string body, OrdTendersTPNotifications tender)
        {
            StringBuilder strBuilder = new StringBuilder(body);
            strBuilder.Replace("{ProjectTitle}", tender.ProjectTitle);
            strBuilder.Replace("{Tender Item Closing Date}", ((DateTime)tender.ClosingDate).ToString("dd/MMM/yyyy HH:mm tt"));
            strBuilder.Replace("{Todays date}", DateTime.Now.ToString("dd/MMM/yyyy HH:mm tt"));
            strBuilder.Replace("{Job Reference}", tender.JobRef);
            if (tender.FlgSpecShowJobAddress)
                strBuilder.Replace("{Job Address:Only if the show job address setting is True}", "Invitation to Tender Address:" + tender.JobAddress);
            else
                strBuilder.Replace("{Job Address:Only if the show job address setting is True}", "");
            strBuilder.Replace("{Job Address}", tender.JobAddress);
            strBuilder.Replace("{Tender Item Package}", tender.PackDesc);
            strBuilder.Replace("{Tender Item Description}", tender.SpecNotes);
            strBuilder.Replace("{Tender Contractor Name}", tender.ContractorName);
            strBuilder.Replace("{Job Manager}", tender.JobManager);
            strBuilder.Replace("{Web User Name}",tender.UserName);
            if (tender.DateTenderFinalised!=null)   strBuilder.Replace("{Tender Finalised Date}", ((DateTime)tender.DateTenderFinalised).ToString("dd/MM/yyyy HH:mm tt"));
            if(tender.DateSpecPublished!=null)      strBuilder.Replace("{Tender Published Date}", ((DateTime)tender.DateSpecPublished).ToString("dd/MM/yyyy HH:mm tt"));
            if(tender.DateTenderAwarded!=null)      strBuilder.Replace("{Tender Awarded Date}", ((DateTime)tender.DateTenderAwarded).ToString("dd/MM/yyyy HH:mm tt"));
            strBuilder.Replace("{Tender Category Description}", tender.CategoryDesc);
            strBuilder.Replace("{Tender Status}", tender.TenderAcceptanceStatus);
            strBuilder.Replace("{Tender Status Description}", tender.StatusDesc);

            strBuilder.Replace("{Question Detail}", tender.QuestionPosted);
            strBuilder.Replace("{Answer Detail}", tender.QuestionAnswerPosted);
            if (tender.DateQuestionPosted != null) strBuilder.Replace("{Question Posted Date}", ((DateTime)tender.DateQuestionPosted).ToString("dd/MM/yyyy HH:mm tt"));

            strBuilder.Replace("{Uploaded File Name}", tender.UploadedFileName);
            strBuilder.Replace("{Uploaded File Description}", tender.UploadedFileDesc);
            strBuilder.Replace("{Uploaded File Version}", tender.UploadedFileVersion);
            if (tender.DateFileUploaded != null) strBuilder.Replace("{File Uploaded Date}", ((DateTime)tender.DateFileUploaded).ToString("dd/MM/yyyy HH:mm tt"));

            return strBuilder.ToString();
        }

        public OrdTendersTPFiles InsertOrdTendersTPFiles(OrdTendersTPFiles ordTendersTPFiles, HttpRequest request)
        {
            OrdTendersTPFiles newOrdTendersTPFiles = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int userId = Utilities.GetUserIdFromRequest(request);
                        List<OrdTendersTP> tenderList = this.GetTenderTPDetailsBySequence(ordTendersTPFiles.JoinSequence ?? 0, true, request);
                        if(tenderList!=null)
                        {
                            OrdTendersTP tender = tenderList[0];
                            CldSettingsRepository cldSettingsRepository = new CldSettingsRepository();
                            CldSettings cldSettingThirdPartyFolderName = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingTenderSpecFileCabThirdPartyFolderName);
                            string thirdPartyFolderName = "TP";
                            if (cldSettingThirdPartyFolderName!=null)
                            {
                                thirdPartyFolderName = cldSettingThirdPartyFolderName.SettingValue;
                            }
                            CldSettings cldSettingProjectTenderFolderName = cldSettingsRepository.GetCldSettingsBySettingName(request, SimplicityConstants.CldSettingTenderSpecFileCabFolderName);
                            string projectTenderFolderName = "ProjectTenders";
                            if (cldSettingProjectTenderFolderName != null)
                            {
                                projectTenderFolderName = cldSettingProjectTenderFolderName.SettingValue;
                            }
                            string targetFolder = tender.OrdersTendersSpecs.OrderMeHeader.Order.JobRef + "," + projectTenderFolderName + "," + tender.JoinSequence + "," + thirdPartyFolderName + "," + tender.Sequence; // ordTendersSpecsFiles.JobRef + "," + projectSetting.TenderSpecFileCabFolderName + "," + ordTendersSpecsFiles.Sequence + "," + projectSetting.TenderSpecFileCabOwnerFolderName;
                            Cld_Ord_Labels_Files oiFireProtectionIImages = new Cld_Ord_Labels_Files();
                            oiFireProtectionIImages.Base64Img = ordTendersTPFiles.Base64Img;
                            oiFireProtectionIImages.FlgIsBase64Img = ordTendersTPFiles.FlgIsBase64Img;
                            oiFireProtectionIImages.ImageName = ordTendersTPFiles.FileName;
                            DriveRequest driveRequest = new DriveRequest { Name = ordTendersTPFiles.FileName, ParentFolderNames = targetFolder, FireProtectionImages = oiFireProtectionIImages };
                            AttachmentFilesFolderRepository attachmentFolderRepository = new AttachmentFilesFolderRepository();
                            AttachmentFiles file = attachmentFolderRepository.AddFileInSpecificFolder(driveRequest, request, null);
                            if (file != null)
                            {
                                OrdersTendersDB OrdTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                                ordTendersTPFiles.FlgUploadComplete = true;
                                ordTendersTPFiles.FileCabId = file.Id;
                                ordTendersTPFiles.VersionNo = 1; //TODO: need to increment it to as per the requirements.
                                ordTendersTPFiles.CreatedBy = userId;
                                ordTendersTPFiles.DateCreated = DateTime.Now;
                                ordTendersTPFiles.LastAmendedBy = userId;
                                ordTendersTPFiles.DateLastAmended = DateTime.Now;
                                newOrdTendersTPFiles = OrdTendersDB.InsertOrdTendersTPFiles(ordTendersTPFiles);
                                newOrdTendersTPFiles.Base64Img = "";
                                if (newOrdTendersTPFiles!=null && !tender.FlgTenderUploads)
                                {
                                    tender.FlgTenderUploads = true;
                                    tender.LastAmendedBy = userId;
                                    tender.DateLastAmended = DateTime.Now;
                                    if(!UpdateOrdTendersTPFlgTenderUploads(tender, request))
                                    {
                                        //TODO: Log Error
                                    }
                                }
                            }
                            else
                            {
                                newOrdTendersTPFiles = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                newOrdTendersTPFiles = null;
            }
            return newOrdTendersTPFiles;
        }

        public OrdTendersTPQS InsertOrdTendersTPQA(OrdTendersTPQS ordTendersTPQA, HttpRequest request)
        {
            OrdTendersTPQS newOrdTendersTPQA = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB OrdTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        ordTendersTPQA.CreatedBy = Utilities.GetUserIdFromRequest(request);
                        ordTendersTPQA.DateCreated = DateTime.Now;
                        ordTendersTPQA.LastAmendedBy = Utilities.GetUserIdFromRequest(request);
                        ordTendersTPQA.DateLastAmended = DateTime.Now;
                        newOrdTendersTPQA = OrdTendersDB.InsertOrdTendersTPQA(ordTendersTPQA);
                    }
                }
            }
            catch (Exception ex)
            {
                newOrdTendersTPQA = null;
            }
            return newOrdTendersTPQA;
        }

        public bool UpdateOrdTendersTPFlgTenderUploads(OrdTendersTP ordTendersTP, HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB OrdTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = OrdTendersDB.UpdateOrdTendersTPFlgTenderUpload(ordTendersTP);
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
            }
            return returnValue;
        }

        public bool UpdateOrdTendersTP(OrdTendersTP ordTendersTP, HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB OrdTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = OrdTendersDB.UpdateOrdTendersTP(ordTendersTP);
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
            }
            return returnValue;
        }

        public bool UpdateOrdTendersTPFileDeletedFlag(long sequence, bool flgDeleted, HttpRequest request)
        {
            bool returnValue = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB OrdTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = OrdTendersDB.UpdateOrdTendersTPFileDeletedFlag(sequence, flgDeleted);
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
            }
            return returnValue;
        }

        public List<RefOrdTenderStatus> GetRefOrderTenderStatus(HttpRequest request)
        {
            List<RefOrdTenderStatus> returnVal = new List<RefOrdTenderStatus>();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersTendersDB ordersTendersDB = new OrdersTendersDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersTendersDB.selectAllRefOrderTenderStatus();
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }
    }
}
