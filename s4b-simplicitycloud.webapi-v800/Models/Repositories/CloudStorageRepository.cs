using Google.Apis.Drive.v3.Data;

using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;

namespace SimplicityOnlineBLL.Entities
{
    public class CloudStorageRepository : ICloudStorageRepository
    {
        //private readonly ILoggerFactory Logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        protected readonly ICldSettingsRepository CldSettingsRepository;

        public CloudStorageRepository(ICldSettingsRepository cldSettingsRepository)
        {
            this.CldSettingsRepository = cldSettingsRepository;
        }

        public AttachmentFilesFolder GetContentsByFolderName(string folderName, RequestHeaderModel header)
        {
            AttachmentFilesFolder returnValue = null;
            if (string.IsNullOrEmpty(folderName)) return returnValue;
            try
            {
                ProjectSettings settings = Configs.settings[header.ProjectId];
                if (settings != null)
                {
                    string searchQuery = string.Empty;  
                    BaseDataSource baseDataSource = new BaseDataSource();
                    baseDataSource.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
                    DriveRequest driveRequest = new DriveRequest
                    {
                        EmailAccount = settings.EmailAccount,
                        KeyFilePath = settings.KeyFilePath,
                        RootFolder = settings.RootFolder
                    };

                    searchQuery = string.Format("('{0}' in parents and trashed=false)", folderName);
                    returnValue = baseDataSource.Source.GetGFiles(driveRequest, searchQuery);
                    if (returnValue != null && returnValue.Folders != null)
                    {
                        IterateOverNestedFolder(baseDataSource, returnValue, driveRequest);
                    }
                    else if (returnValue!=null && returnValue.Id==null)
                    {                            
                        driveRequest.Name = folderName;
                        string rootFolderId = GetSimplicityRootFolderId(baseDataSource, driveRequest, settings.RootFolder);
                        driveRequest.ParentFolderId = rootFolderId; //rootFolder.Folders[0].Id;
                        returnValue = baseDataSource.Source.AddFolder(driveRequest);
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured while getting Filing Cabinet Folder Contents By Folder Name. "  + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        private string GetSimplicityRootFolderId(BaseDataSource baseDataSource, DriveRequest driveRequest ,string appRootFolder)
        {
            //baseDataSource.SetDataSource(mode);
            string criteria = string.Format("(name='"+ appRootFolder + "' and 'root' in parents) and mimeType = 'application/vnd.google-apps.folder' and trashed=false ");
            AttachmentFilesFolder rootFolder = baseDataSource.Source.GetGFiles(driveRequest, criteria);
            string rootFolderId = string.Empty;
            if (rootFolder.Folders == null || rootFolder.FoldersCount == 0)
                return "";
            foreach (AttachmentFilesFolder item in rootFolder.Folders)
            {
                if (item.Name == appRootFolder)
                {
                    rootFolderId = item.Id;
                    break;
                }
            }
            return rootFolderId;
        }

        public void UpdateAllFolderIds(RequestHeaderModel header)
        {
            string errorMessage = "Project settings not available.";
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
            if (settings == null) throw new Exception();
            try
            {
                BaseDataSource baseDataSource = new BaseDataSource();
                baseDataSource.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
                DriveRequest driveRequest = new DriveRequest
                {
                    EmailAccount = settings.EmailAccount,
                    KeyFilePath = settings.KeyFilePath,
                    RootFolder = settings.RootFolder,
                };
                //Note: Updating App Root Folder ID
                errorMessage = "Exception while updating App Root Folder Id";
                AttachmentFilesFolder newFolder = new AttachmentFilesFolder();
                string appRootFolderName = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.CldSettingFilingCabinetRootFolder).SettingValue;
                string appRootfolderId = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.CldSettingFilingCabinetRootFolderId).SettingValue;
                driveRequest.FileId = "1KRx9cytcI9S2e8HOcceY5v0Xt2y6HOpH";
                //ShareFile(driveRequest, header);
                //DeleteFile(driveRequest, header);
                if (appRootfolderId.ToLower() == "notset" || appRootfolderId.ToLower() == "not set")
                {
                    driveRequest.ParentFolderId = "root";
                    driveRequest.Name = appRootFolderName;
                    newFolder = AddFolder(driveRequest, header);
                    //ShareFile(driveRequest, header);
                    if (newFolder != null && newFolder.Folders != null && newFolder.FoldersCount > 0)
                    {
                        appRootfolderId = newFolder.Folders.FirstOrDefault().Id;
                        CldSettingsRepository.UpdateSettingsBySettingName(header.ProjectId, SimplicityConstants.CldSettingFilingCabinetRootFolderId, appRootfolderId);
                    }
                }

                //NOTE:  Updating Rossum Root Folder ID
                errorMessage = "Exception while updating Rossum Root Folder Id";
                newFolder = null;
                string rossumRootFolderName = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_ROOT_FOLDER_NAME).SettingValue;
                string RossumRootFolderId = CldSettingsRepository.GetCldSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_ROOT_FOLDER_ID).SettingValue;
                if (RossumRootFolderId.ToLower() == "notset" || RossumRootFolderId.ToLower() == "not set")
                {
                    driveRequest.ParentFolderId = appRootfolderId;
                    driveRequest.Name = rossumRootFolderName;
                    newFolder = AddFolder(driveRequest, header);
                    //Sharing with admin email account.
                    //driveRequest = new DriveRequest();
                    //driveRequest.FileId = appRootfolderId;
                    //ShareFile(driveRequest, header);

                    if (newFolder != null && newFolder.Folders != null && newFolder.FoldersCount > 0)
                    {
                        RossumRootFolderId = newFolder.Folders.FirstOrDefault().Id;
                        CldSettingsRepository.UpdateSettingsBySettingName(header.ProjectId, SimplicityConstants.ROSSUM_ROOT_FOLDER_ID, RossumRootFolderId);
                    }
                    else 
                    {
                        throw new Exception("Could not create Rossum Root Folder");
                    }
                }
                // Note: Update All Rossum document type folder Ids
                errorMessage = "Exception while creating/updating doc type folder";
                List<RossumDocumentType> rossumDocTypeList = new List<RossumDocumentType>();

                RossumFilesDB rossumDB = new RossumFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                rossumDocTypeList = rossumDB.GetDocTypesAll();
                errorMessage = "Rossum Doc Types not found.";
                if (rossumDocTypeList.Count == 0)
                    throw new InvalidDataException();

                List<RossumDocumentType> docTypes = rossumDocTypeList.Where(x => string.IsNullOrEmpty(x.DocTypeFolderCabId) || string.IsNullOrEmpty(x.ReceivedFolderCabId) || string.IsNullOrEmpty(x.InReviewFolderCabId) || string.IsNullOrEmpty(x.SuccessFolderCabId) || string.IsNullOrEmpty(x.FailedFolderCabId)).ToList();
                if (docTypes.Count() == 0) 
                    return;
                
                AttachmentFilesFolder rossumRootFolder = GetFolderContentById(RossumRootFolderId, header);
                if (rossumRootFolder == null)
                    rossumRootFolder = new AttachmentFilesFolder();

                driveRequest = new DriveRequest();
                newFolder = new AttachmentFilesFolder();
                if (rossumRootFolder.Folders == null) rossumRootFolder.Folders = new List<AttachmentFilesFolder>();

                errorMessage = "Exception occured in Document types iteration";
                foreach (RossumDocumentType item in rossumDocTypeList)
                {
                    AttachmentFilesFolder doctype = rossumRootFolder.Folders.Where(x => x.Name.ToUpper() == item.DocTypeFolderName.ToUpper()).FirstOrDefault();
                    if (doctype == null)
                    {
                        doctype = new AttachmentFilesFolder();
                        driveRequest.ParentFolderId = RossumRootFolderId;
                        driveRequest.Name = item.DocTypeFolderName;
                        errorMessage = "Exception occured while adding doc type folder";
                        newFolder = AddFolder(driveRequest, header);
                        doctype.Id = newFolder.Id;
                    }
                    //criteria = "'" + doctype.Id + "' in parents and mimeType = 'application/vnd.google-apps.folder' ";
                    errorMessage = "Exception occured while getting folder content of a doc type";
                    AttachmentFilesFolder folderDetail = GetFolderContentById(doctype.Id, header);
                    if (folderDetail == null)
                    {
                        folderDetail = new AttachmentFilesFolder();
                        folderDetail.Folders = new List<AttachmentFilesFolder>();
                    }
                    RossumDocumentType doc = new RossumDocumentType();
                    doc.DocTypeKey = item.DocTypeKey;
                    doc.DocTypeFolderCabId = doctype.Id;
                    //Received Folder
                    errorMessage = "Exception occured while processing Received folder";
                    AttachmentFilesFolder receivedFolder = folderDetail.Folders.Where(x => x.Name.ToLower() == item.ReceivedFolderName.ToLower()).FirstOrDefault();
                    if (receivedFolder == null)
                    {
                        driveRequest.ParentFolderId = doctype.Id;
                        driveRequest.Name = item.ReceivedFolderName;
                        newFolder = AddFolder(driveRequest, header);
                        doc.ReceivedFolderCabId = newFolder.Id;
                    }
                    else
                        doc.ReceivedFolderCabId = receivedFolder.Id;

                    //failedFolder
                    errorMessage = "Exception occured while processing Failed folder";
                    AttachmentFilesFolder failedFolder = folderDetail.Folders.Where(x => x.Name.ToLower() == item.FailedFolderName.ToLower()).FirstOrDefault();
                    if (failedFolder == null)
                    {
                        driveRequest.ParentFolderId = doctype.Id;
                        driveRequest.Name = item.FailedFolderName;
                        newFolder = AddFolder(driveRequest, header);
                        doc.FailedFolderCabId = newFolder.Id;
                    }
                    else
                        doc.FailedFolderCabId = failedFolder.Id;

                    //SuccessFolder
                    errorMessage = "Exception occured while processing Success folder";
                    AttachmentFilesFolder successFolder = folderDetail.Folders.Where(x => x.Name.ToLower() == item.SuccessFolderName.ToLower()).FirstOrDefault();
                    if (successFolder == null)
                    {
                        driveRequest.ParentFolderId = doctype.Id;
                        driveRequest.Name = item.SuccessFolderName;
                        newFolder = AddFolder(driveRequest, header);
                        doc.SuccessFolderCabId = newFolder.Id;
                    }
                    else
                        doc.SuccessFolderCabId = successFolder.Id;
                    //InReview Folder
                    errorMessage = "Exception occured while processing InReview folder";
                    AttachmentFilesFolder inReviewFolder = folderDetail.Folders.Where(x => x.Name.ToLower() == item.InReviewFolderName.ToLower()).FirstOrDefault();
                    if (inReviewFolder == null)
                    {
                        driveRequest.ParentFolderId = doctype.Id;
                        driveRequest.Name = item.InReviewFolderName;
                        newFolder = AddFolder(driveRequest, header);
                        doc.InReviewFolderCabId = newFolder.Id;
                    }
                    else
                        doc.InReviewFolderCabId = inReviewFolder.Id;
                    rossumDB.UpdateDocTypeCabIds(doc);
                }
                
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception while updating Folder IDs. Error Message: " + errorMessage);
            }
        }
        private string GetRossumRootFolderId(BaseDataSource baseDataSource, DriveRequest driveRequest, string appRootFolder)
        {
            //baseDataSource.SetDataSource(mode);
            string criteria = string.Format("(name='" + appRootFolder + "' and 'root' in parents) and mimeType = 'application/vnd.google-apps.folder' and trashed=false ");
            AttachmentFilesFolder rootFolder = baseDataSource.Source.GetGFiles(driveRequest, criteria);
            string rootFolderId = string.Empty;
            if (rootFolder.Folders == null || rootFolder.FoldersCount == 0)
                return "";
            foreach (AttachmentFilesFolder item in rootFolder.Folders)
            {
                if (item.Name == appRootFolder)
                {
                    rootFolderId = item.Id;
                    break;
                }
            }
            return rootFolderId;
        }

        /// <summary>
        /// Loop through all the folders & files
        /// </summary>
        /// <param name="baseDataSource"></param>
        /// <param name="returnValue"></param>
        /// <param name="driveRequest"></param>
        public AttachmentFilesFolder GetRootFolderContents(RequestHeaderModel header)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[header.ProjectId];
                if (settings != null)
                {
                    BaseDataSource baseDataSource = new BaseDataSource();
                    baseDataSource.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
                    string searchQuery = string.Format("('{0}' in parents and trashed=false) ", settings.RootFolder);
                    returnValue = baseDataSource.Source.GetAttachmentFilesAndFolders(new DriveRequest
                    {
                        EmailAccount = settings.EmailAccount,
                        KeyFilePath = settings.KeyFilePath,
                        RootFolder = settings.RootFolder,
                    }, searchQuery);
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;

        }

        public AttachmentFilesFolder GetFolderContentById(string folderId, RequestHeaderModel header)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[header.ProjectId];
                if (settings != null)
                {
                    string searchQuery = string.Format("('{0}' in parents and trashed=false)", folderId);
                    BaseDataSource baseDataSource = new BaseDataSource();
                    baseDataSource.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
                    returnValue = baseDataSource.Source.GetGFiles(new DriveRequest
                    {
                        EmailAccount = settings.EmailAccount,
                        KeyFilePath = settings.KeyFilePath,
                        RootFolder = settings.RootFolder
                    }, searchQuery);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteLog("Exception while getting Folder Content", "GetFolderContentById-Repository");
            }
            return returnValue;

        }

        public bool GetFileSharedPerissions(DriveRequest driveRequest, RequestHeaderModel header)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Configs.settings[header.ProjectId];
                //ProjectSettings settings = Configs.settings["KILNBRIDGE"];                
                if (settings != null)
                {
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);
                        driveRequest.EmailAccount = settings.EmailAccount;
                        driveRequest.KeyFilePath = settings.KeyFilePath;
                        returnValue = baseDataSource.Source.GetFileSharedPerissions(driveRequest);
                    }

                }
            }
            catch (Exception ex)
            {
                returnValue = false;
            }
            return returnValue;
        }

        public AttachmentFilesFolder GDriveQuery(RequestHeaderModel header, string criteria)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[header.ProjectId];
                if (settings != null)
                {
                    BaseDataSource baseDataSource = new BaseDataSource();
                    baseDataSource.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
                    DriveRequest driveRequest = new DriveRequest
                    {
                        EmailAccount = settings.EmailAccount,
                        KeyFilePath = settings.KeyFilePath,
                        RootFolder = settings.RootFolder
                    };
                    returnValue = baseDataSource.Source.GetFolderDetail(driveRequest, criteria);
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured while getting Filing Cabinet Folder Contents By Folder Name. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }
        
        public AttachmentFiles GetFileOrFolderMeta_ById(RequestHeaderModel header, string FileOrfolderId)
        {
            AttachmentFiles returnValue = null;
            ProjectSettings settings = Configs.settings[header.ProjectId];
            if (settings == null) return returnValue;
            BaseDataSource GDrive = new BaseDataSource();
            GDrive.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
            DriveRequest driveRequest = new DriveRequest
            {
                EmailAccount = settings.EmailAccount,
                KeyFilePath = settings.KeyFilePath,
                RootFolder = settings.RootFolder
            };
            returnValue = GDrive.Source.GetMetadata_ById(driveRequest, FileOrfolderId);
            return returnValue;
        }
        public AttachmentFilesFolder GetFileOrFolderMeta_ByName(RequestHeaderModel header, string folderName, string parentFolderId)
        {
            AttachmentFilesFolder returnValue = null;
            ProjectSettings settings = Configs.settings[header.ProjectId];
            if (settings == null) return returnValue;
            BaseDataSource GDrive = new BaseDataSource();
            GDrive.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
            DriveRequest driveRequest = new DriveRequest
            {
                EmailAccount = settings.EmailAccount,
                KeyFilePath = settings.KeyFilePath,
                RootFolder = settings.RootFolder
            };
            returnValue = GDrive.Source.GetMetadata_ByName(driveRequest, folderName, parentFolderId);
            return returnValue;
        }
        public AttachmentFilesFolder GetContent_ByFolderId(RequestHeaderModel header, string folderId)
        {
            AttachmentFilesFolder returnValue = null;
            ProjectSettings settings = Configs.settings[header.ProjectId];
            if (settings == null) return returnValue;
            BaseDataSource GDrive = new BaseDataSource();
            GDrive.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
            DriveRequest driveRequest = new DriveRequest
            {
                EmailAccount = settings.EmailAccount,
                KeyFilePath = settings.KeyFilePath,
                RootFolder = settings.RootFolder
            };
            returnValue = GDrive.Source.GetFolderContentsById(driveRequest, folderId);
            return returnValue;
        }
        public AttachmentFilesFolder GetContents_ByFolderName(RequestHeaderModel header, string folderName, string ParentFolderId)
        {
            AttachmentFilesFolder returnValue = null;
            BaseDataSource GDrive = new BaseDataSource();
            GDrive.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
            string criteria = string.Format("(name='{0}' and '{1}' in parents and transh=false)",folderName,ParentFolderId);
            returnValue = GDriveQuery(header, criteria);
            return returnValue;
        }

        private string GetCriteria(string folderName, string parentFolderId)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            return string.Format("((name='{0}' or name='{1}' or name='{2}' or name='{3}') and '{4}' in parents and trashed=false) ", folderName,textInfo.ToUpper(folderName),textInfo.ToLower(folderName),textInfo.ToTitleCase(folderName), parentFolderId);
            //return string.Format("(name contains '{0}' and '{1}' in parents and trashed=false) ", folderName, parentFolderId);
        }

        public AttachmentFilesFolder AddFolder(DriveRequest driveRequest, RequestHeaderModel header)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[header.ProjectId];
                if (settings != null)
                {
                    BaseDataSource baseDataSource = new BaseDataSource();
                    baseDataSource.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
                    driveRequest.EmailAccount = settings.EmailAccount;
                    driveRequest.KeyFilePath = settings.KeyFilePath;
                    string criteria = GetCriteria(driveRequest.Name, driveRequest.ParentFolderId);
                    returnValue = CreateNewFolderIfNotExists(baseDataSource, driveRequest, criteria, driveRequest.Name, driveRequest.ParentFolderId);
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }

        #region ****************************************************************** Archive Region

        /// <summary>
        /// Loop through all the folders & files
        /// </summary>
        /// <param name="baseDataSource"></param>
        /// <param name="returnValue"></param>
        /// <param name="driveRequest"></param>
        private void IterateOverNestedFolder(BaseDataSource baseDataSource, AttachmentFilesFolder returnValue, DriveRequest driveRequest)
        {
            if (returnValue.Folders != null)
            {
                foreach (AttachmentFilesFolder item in returnValue.Folders)
                {
                    var nestedFolder = baseDataSource.Source.GetFolderContentsById(driveRequest, item.Id);
                    if (nestedFolder != null)
                    {
                        if (nestedFolder.Files != null)
                        {
                            item.Files = nestedFolder.Files;
                            item.FilesCount = nestedFolder.FilesCount;
                        }
                        if (nestedFolder.Folders != null)
                        {
                            item.Folders = nestedFolder.Folders;
                            item.FoldersCount = nestedFolder.FoldersCount;
                        }
                    }
                    IterateOverNestedFolder(baseDataSource, item, driveRequest);
                }
            }
        }
        public AttachmentFilesFolder GetFolderContentsById(string folderId, RequestHeaderModel header)
        {
            AttachmentFilesFolder returnValue = null;
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
            if (settings == null)
                returnValue = null;
            try
            {
                BaseDataSource baseDataSource = new BaseDataSource();
                baseDataSource.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
                returnValue = baseDataSource.Source.GetFolderContentsById(new DriveRequest
                {
                    EmailAccount = settings.EmailAccount,
                    KeyFilePath = settings.KeyFilePath,
                    RootFolder = settings.RootFolder
                }, folderId);
            }
            catch (Exception ex)
            {
            }
            return returnValue;

        }
        public AttachmentFiles AddFile(DriveRequest driveRequest, RequestHeaderModel header)
        {
            AttachmentFiles returnValue = null;
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
            if (settings == null)
                returnValue = null;
            try
            {
                BaseDataSource baseDataSource = new BaseDataSource();
                baseDataSource.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
                driveRequest.EmailAccount = settings.EmailAccount;
                driveRequest.KeyFilePath = settings.KeyFilePath;
                string tempFolderPath = string.Format(@"{0}{1}", settings.TempUploadFolderPath, header.ProjectId);

                AttachmentFiles file = baseDataSource.UploadFileOnServer(driveRequest, tempFolderPath);
                if (file != null && !string.IsNullOrEmpty(file.FilePath))
                {
                    driveRequest.FilePath = file.FilePath;
                    returnValue = baseDataSource.Source.AddFile(driveRequest);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public AttachmentFiles CopyFile(DriveRequest driveRequest, string destinationFolderId, RequestHeaderModel header)
        {
            AttachmentFiles returnValue = null;
            ProjectSettings settings = Configs.settings[header.ProjectId];
            if (settings == null) return returnValue;
            try
            {
                BaseDataSource GDrive = new BaseDataSource();
                GDrive.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
                driveRequest.EmailAccount = settings.EmailAccount;
                driveRequest.KeyFilePath = settings.KeyFilePath;
                returnValue = GDrive.Source.CopyFile(driveRequest, destinationFolderId);
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public AttachmentFiles MoveFile(DriveRequest driveRequest, string destinationFolderId, RequestHeaderModel header)
        {
            AttachmentFiles returnValue = null;
            ProjectSettings settings = Configs.settings[header.ProjectId];
            if (settings == null) return returnValue;
            try
            {
                BaseDataSource GDrive = new BaseDataSource();
                GDrive.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
                driveRequest.EmailAccount = settings.EmailAccount;
                driveRequest.KeyFilePath = settings.KeyFilePath;
                returnValue = GDrive.Source.MoveFile(driveRequest, destinationFolderId);
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public AttachmentFiles RenameFile(DriveRequest driveRequest, RequestHeaderModel header)
        {
            AttachmentFiles returnValue = null;
            ProjectSettings settings = Configs.settings[header.ProjectId];
            if (settings == null) return returnValue;
            try
            {
                BaseDataSource GDrive = new BaseDataSource();
                GDrive.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
                driveRequest.EmailAccount = settings.EmailAccount;
                driveRequest.KeyFilePath = settings.KeyFilePath;
                returnValue = GDrive.Source.RenameFile(driveRequest);
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public AttachmentFilesFolder RenameFolderByRootFolderName(DriveRequest driveRequest, RequestHeaderModel header)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[header.ProjectId];
                //ProjectSettings settings = Configs.settings["KILNBRIDGE"];
                if (settings != null)
                {
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);
                        driveRequest.EmailAccount = settings.EmailAccount;
                        driveRequest.KeyFilePath = settings.KeyFilePath;

                        string rootFolderId = GetSimplicityRootFolderId(baseDataSource, driveRequest, settings.RootFolder);
                        //check if same folder already exists
                        //string criteria = string.Format("(name='{0}' and trashed=false and '{1}' in parents) ", driveRequest.Name, rootFolderId);
                        string criteria = GetCriteria(driveRequest.Name, rootFolderId);
                        returnValue = baseDataSource.Source.GetAttachmentFilesAndFolders(driveRequest, criteria);
                        //if same folder doesn't exists then rename
                        if (returnValue.Id == null)
                        {
                            //criteria = string.Format("(name='{0}' and trashed=false and '{1}' in parents) ", driveRequest.ParentFolderName, rootFolderId);
                            criteria = GetCriteria(driveRequest.ParentFolderName, rootFolderId);
                            returnValue = baseDataSource.Source.GetAttachmentFilesAndFolders(driveRequest, criteria);
                            if (returnValue != null)
                            {
                                driveRequest.FolderId = returnValue.Id;
                                returnValue = baseDataSource.Source.RenameFolder(driveRequest);
                            }
                        }

                    }
                    else
                    {
                        returnValue = null;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public AttachmentFilesFolder AddJobRefFolderWithSubFolders(DriveRequest driveRequest, RequestHeaderModel header)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[header.ProjectId];
                //ProjectSettings settings = Configs.settings["KILNBRIDGE"];
                if (settings != null)
                {
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);
                        driveRequest.EmailAccount = settings.EmailAccount;
                        driveRequest.KeyFilePath = settings.KeyFilePath;

                        //Create jobRef folder
                        var rootFolderId = GetSimplicityRootFolderId(baseDataSource, driveRequest, settings.RootFolder);
                        driveRequest.ParentFolderId = rootFolderId;
                        driveRequest.Description = driveRequest.Name;
                        AttachmentFilesFolder jobRefFolder = null;
                        //check if jobRef folder exitst..
                        //var criteria = string.Format("(name='{0}' and '{1}' in parents and trashed=false) ", driveRequest.Name, rootFolderId);
                        var criteria = GetCriteria(driveRequest.Name, rootFolderId);
                        if (!IsFolderExists(baseDataSource, driveRequest, criteria))
                        {
                            jobRefFolder = baseDataSource.Source.AddFolder(driveRequest);
                            //var jobRefFolder = CreateNewFolderIfNotExists(baseDataSource, driveRequest, criteria, driveRequest.Name, rootFolderId);
                            string currentParent = string.Empty;
                            if (jobRefFolder.Id == null)
                                currentParent = jobRefFolder.Folders[0].Id;
                            else currentParent = jobRefFolder.Id;
                            AttachmentFilesFolderDB db = new AttachmentFilesFolderDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                            List<AttachmentFilesFolder> attachmentFolders = db.GetAttachFolderStructure();
                            Stack<string> parentFolderList = new Stack<string>();
                            Stack<string> parentFolderCompleteList = new Stack<string>();
                            if (jobRefFolder != null)
                            {
                                string parentFolderName = string.Empty;
                                parentFolderList.Push(currentParent);
                                parentFolderCompleteList.Push(currentParent);

                                foreach (AttachmentFilesFolder item in attachmentFolders)
                                {
                                    driveRequest.Name = driveRequest.Description = item.Name;
                                    if (string.IsNullOrEmpty(item.ParentFolderName))
                                    {
                                        driveRequest.ParentFolderId = currentParent;
                                    }
                                    else
                                    {
                                        if (parentFolderName != item.ParentFolderName)
                                        {
                                            while (parentFolderList.Count > 0)
                                            {
                                                //criteria = string.Format("(name='{0}' and '{1}' in parents and trashed=false) ", item.ParentFolderName, currentParent);
                                                criteria = GetCriteria(item.ParentFolderName, currentParent);
                                                AttachmentFilesFolder attachmentFilesFolder = baseDataSource.Source.GetFolderDetail(driveRequest, criteria);
                                                if (attachmentFilesFolder != null && attachmentFilesFolder.Folders != null)
                                                {
                                                    currentParent = attachmentFilesFolder.Folders[0].Id;
                                                    break;
                                                }
                                                else
                                                {
                                                    currentParent = parentFolderList.Pop();
                                                }
                                            }

                                            parentFolderCompleteList.Push(currentParent);
                                            parentFolderList = new Stack<string>();
                                            foreach (string each in parentFolderCompleteList)
                                            {
                                                parentFolderList.Push(each);
                                            }
                                            parentFolderName = item.ParentFolderName;
                                        }
                                        driveRequest.ParentFolderId = currentParent;
                                    }
                                    returnValue = baseDataSource.Source.AddFolder(driveRequest);
                                }
                            }
                            //if (jobRefFolder != null && driveRequest.SubFolders != null && driveRequest.SubFolders.Count > 0)
                            //{
                            //    jobRefFolder.Folders = new List<AttachmentFilesFolder>();
                            //    foreach (string item in driveRequest.SubFolders)
                            //    {
                            //        driveRequest.Name = driveRequest.Description = item;
                            //        driveRequest.ParentFolderId = currentParent;
                            //        returnValue = baseDataSource.Source.AddFolder(driveRequest);
                            //        jobRefFolder.Folders.Add(returnValue);
                            //    }
                            //}
                        }
                        //return JobReffolder detail
                        returnValue = jobRefFolder;

                    }
                    else
                    {
                        returnValue = null;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public AttachmentFilesFolder RenameFolder(DriveRequest driveRequest, RequestHeaderModel header)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[header.ProjectId];
                if (settings != null)
                {
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);
                        driveRequest.EmailAccount = settings.EmailAccount;
                        driveRequest.KeyFilePath = settings.KeyFilePath;
                        returnValue = baseDataSource.Source.RenameFolder(driveRequest);
                    }
                    else
                    {
                        Message = "The Filing Cabinet Mode has not been implemented.";
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Unable to Rename Folder. Exception occured " + ex.Message + " - " + ex.InnerException;
            }
            return returnValue;
        }
        public bool DeleteFile(DriveRequest driveRequest, RequestHeaderModel header)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Configs.settings[header.ProjectId];
                if (settings != null)
                {
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);
                        driveRequest.EmailAccount = settings.EmailAccount;
                        driveRequest.KeyFilePath = settings.KeyFilePath;
                        returnValue = baseDataSource.Source.DeleteFile(driveRequest);
                    }
                    else
                    {
                        returnValue = false;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
            }
            return returnValue;
        }
        public AttachmentFiles UploadFile(Cld_Ord_Labels_Files oiFireProtectionIImages, RequestHeaderModel header)
        {
            AttachmentFiles returnValue = null; ;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId); string projectId = header.ProjectId;
                if (string.IsNullOrEmpty(projectId)) return null;
                if (settings != null)
                {
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);
                        string tempFolderPath = string.Format(@"{0}{1}", settings.TempUploadFolderPath, projectId);
                        returnValue = baseDataSource.Source.UploadFile(oiFireProtectionIImages, tempFolderPath);
                    }

                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }
        public AttachmentFiles AddFileInSpecificFolder(DriveRequest driveRequest, RequestHeaderModel header)
        {
            AttachmentFiles returnValue = null;
            try
            {
                string projectId = header.ProjectId;
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
                if (settings != null)
                {
                    string criteria = string.Empty;
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);
                        driveRequest.EmailAccount = settings.EmailAccount;
                        driveRequest.KeyFilePath = settings.KeyFilePath;
                        driveRequest.RootFolder = settings.RootFolder;
                        driveRequest.UserAccount = settings.UserAccount;
                        AttachmentFilesFolder newlyCreatedFolder = null;
                        driveRequest.ParentFolderNames = driveRequest.ParentFolderNames.Replace("xxxx_custom_folder_name", settings.UploadDocumentFolderName);
                        newlyCreatedFolder = CreateNestedFoldersIfNotExists(settings, baseDataSource, driveRequest.ParentFolderNames);
                        string tempFolderPath = string.Format(@"{0}{1}", settings.TempUploadFolderPath, projectId);
                        if (driveRequest.FireProtectionImages != null)
                        {
                            AttachmentFiles file = baseDataSource.Source.UploadFile(driveRequest.FireProtectionImages, tempFolderPath);
                            if (file != null && !string.IsNullOrEmpty(file.FilePath))
                            {
                                driveRequest.FilePath = file.FilePath;
                                driveRequest.ParentFolderId = newlyCreatedFolder.Id != null ? newlyCreatedFolder.Id : newlyCreatedFolder.Folders[0].Id;
                                returnValue = baseDataSource.Source.AddFile(driveRequest);
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                    else
                    {
                        returnValue = null;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public SimplicityFile DowloadFile(string fileId, RequestHeaderModel header)
        {
            SimplicityFile returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[header.ProjectId];
                //ProjectSettings settings = Configs.settings["KILNBRIDGE"];
                if (settings != null)
                {
                    DriveRequest driveRequest = new DriveRequest();
                    driveRequest.FileId = fileId;
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);
                        driveRequest.EmailAccount = settings.EmailAccount;
                        driveRequest.KeyFilePath = settings.KeyFilePath;
                        returnValue = baseDataSource.Source.DownloadFile(driveRequest);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return returnValue;
        }
        public bool ShareFile(DriveRequest driveRequest, RequestHeaderModel header)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Configs.settings[header.ProjectId];
                //ProjectSettings settings = Configs.settings["KILNBRIDGE"];                
                if (settings != null)
                {
                    BaseDataSource baseDataSource = new BaseDataSource();
                    baseDataSource.SetDataSource(AttachmentFolderMode.GOOGLEDRIVE);
                    driveRequest.EmailAccount = settings.EmailAccount;
                    driveRequest.KeyFilePath = settings.KeyFilePath;
                    driveRequest.UserAccount = settings.UserAccount;
                    returnValue = baseDataSource.Source.ShareFile(driveRequest);
                }
            }
            catch (Exception ex)
            {
                returnValue = false;
            }
            return returnValue;
        }
        public AttachmentFilesFolder GetFilesByFolderNameTest(string folderName, RequestHeaderModel header)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[header.ProjectId];
                if (settings != null)
                {
                    string criteria = string.Empty;
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);
                        DriveRequest driveRequest = new DriveRequest
                        {
                            EmailAccount = settings.EmailAccount,
                            KeyFilePath = settings.KeyFilePath,
                            RootFolder = settings.RootFolder
                    };

                        string rootFolderId = GetSimplicityRootFolderId(baseDataSource, driveRequest, settings.RootFolder);
                        criteria = "mimeType = 'application/vnd.google-apps.folder' and name = '" + folderName + "'";
                        returnValue = baseDataSource.Source.GetFilesofFolder(driveRequest, criteria);
                    }
                    else
                    {
                        returnValue = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured while getting Filing Cabinet Folder Contents By Folder Name. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }
        private AttachmentFilesFolder CreateNestedFoldersIfNotExists(ProjectSettings settings, BaseDataSource baseDataSource, string parentFolderNames)
        {
            DriveRequest aRequest = new DriveRequest();
            aRequest.EmailAccount = settings.EmailAccount;
            aRequest.KeyFilePath = settings.KeyFilePath;
            aRequest.RootFolder = settings.RootFolder;
            aRequest.UserAccount = settings.UserAccount;
            AttachmentFilesFolder parentFolder = null;
            AttachmentFilesFolder newlyCreatedFolder = null;
            string criteria = string.Empty;
            string currentParent = "root"; //GetRootFolderId(baseDataSource, driveRequest, mode, settings.RootFolder);
            string[] nameList = parentFolderNames.Split(',');

            currentParent = GetSimplicityRootFolderId(baseDataSource, aRequest, settings.RootFolder);
            for (int i = 0; i < nameList.Length; i++)
            {
                //criteria = string.Format("(name='{0}' and '{1}' in parents and trashed=false) ", nameList[i], currentParent);
                criteria = GetCriteria(nameList[i], currentParent);
                newlyCreatedFolder = CreateNewFolderIfNotExists(baseDataSource, aRequest, criteria, nameList[i], currentParent);
                if (newlyCreatedFolder.Id == null)
                    currentParent = newlyCreatedFolder.Folders[0].Id;
                else currentParent = newlyCreatedFolder.Id;
            }
            return newlyCreatedFolder;
        }
        private AttachmentFilesFolder CreateNewFolderIfNotExists(BaseDataSource baseDataSource, DriveRequest aRequest, string criteria, string folderName, string parentFolderId)
        {
            AttachmentFilesFolder newlyCreatedFolder = baseDataSource.Source.GetFolderDetail(aRequest, criteria);
            if (newlyCreatedFolder == null)
            {
                aRequest.Name = folderName;
                aRequest.ParentFolderId = parentFolderId;
                newlyCreatedFolder = baseDataSource.Source.AddFolder(aRequest);
            }
            return newlyCreatedFolder;
        }
        public ResponseModel InsertGDriveFileMapping(RequestHeaderModel header, List<GDriveMapping> obj)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = header.ProjectId;
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        AttachmentFilesFolderDB fileDB = new AttachmentFilesFolderDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        List<GDriveMapping> itemReturned = new List<GDriveMapping>();
                        foreach (GDriveMapping item in obj)
                        {
                            GDriveMapping newItem = fileDB.insertGDriveFileMapping(item);
                            item.sequence = newItem.sequence;
                            itemReturned.Add(item);

                        }
                        returnValue.TheObject = itemReturned;
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
        private bool IsFolderExists(BaseDataSource baseDataSource, DriveRequest aRequest, string criteria)
        {
            AttachmentFilesFolder newlyCreatedFolder = baseDataSource.Source.GetFolderDetail(aRequest, criteria);
            return newlyCreatedFolder != null;
        }

        #endregion
    }
}
