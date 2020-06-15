using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace SimplicityOnlineBLL.Entities
{
    public class AttachmentFilesFolderRepository : IAttachmentFilesFolderRepository
    {
        //private readonly ILoggerFactory Logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public AttachmentFilesFolderRepository()
        {
            //
        }

        public ResponseModel GetMailMergeFolderContents(HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    CldSettingsRepository cldSettingsRepository = new CldSettingsRepository();
                    string folderNameForMailMergeTemplates = cldSettingsRepository.GetFolderNameForMailMergeTemplates(request);
                    AttachmentFilesFolder folderContents = GetAttachmentFilesFolderByFolderName(folderNameForMailMergeTemplates, request);
                    if(folderContents!=null)
                    {
                        returnValue.IsSucessfull = true;
                        returnValue.TheObject = folderContents;
                    }
                    else
                    {
                        returnValue.Message = Message;
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Exception Occured while getting Filing Cabinet Mail Merge Folder Contents. " + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        public AttachmentFilesFolder GetAttachmentFilesFolderByFolderName(string folderName, HttpRequest request)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    string crieria = string.Empty;  
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

                        string rootFolderId = GetRootFolderId(baseDataSource, driveRequest, settings.RootFolder);
                        crieria = GetCriteria(string.IsNullOrEmpty(folderName) ? settings.RootFolder : folderName, rootFolderId);

                        returnValue = baseDataSource.Source.GetAttachmentFilesAndFolders(driveRequest, crieria);
                        if (returnValue != null && returnValue.Folders != null)
                        {
                            IterateOverNestedFolder(baseDataSource, returnValue, driveRequest);
                        }
                        else if (returnValue!=null && returnValue.Id==null)
                        {                            
                            driveRequest.Name = folderName;
                            driveRequest.ParentFolderId = rootFolderId; //rootFolder.Folders[0].Id;
                            returnValue = baseDataSource.Source.AddFolder(driveRequest);
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
                Message = "Exception Occured while getting Filing Cabinet Folder Contents By Folder Name. "  + ex.Message + " " + ex.InnerException;
            }
            return returnValue;
        }

        private string GetRootFolderId(BaseDataSource baseDataSource, DriveRequest driveRequest ,string appRootFolder)
        {
            //baseDataSource.SetDataSource(mode);
            string crieria = string.Format("('root' in parents) and trashed=false ");
            AttachmentFilesFolder rootFolder = baseDataSource.Source.GetFolderDetail(driveRequest, crieria);
            string rootFolderId = string.Empty; 
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
        public AttachmentFilesFolder GetRootFolderContents(HttpRequest request, HttpResponse response)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                //ProjectSettings settings = Configs.settings["KILNBRIDGE"];
                if (settings != null)
                {
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);
                        string crieria = string.Format("(name contains '{0}' and trashed=false) ", settings.RootFolder);
                        returnValue = baseDataSource.Source.GetAttachmentFilesAndFolders(new DriveRequest
                        {
                            EmailAccount = settings.EmailAccount,
                            KeyFilePath = settings.KeyFilePath,
                            RootFolder = settings.RootFolder
                        }, crieria);
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

        public AttachmentFilesFolder GetFolderContentsById(string folderId, HttpRequest request, HttpResponse response)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                //ProjectSettings settings = Configs.settings["KILNBRIDGE"];
                if (settings != null)
                {
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);                        
                        returnValue = baseDataSource.Source.GetFolderContentsById(new DriveRequest
                        {
                            EmailAccount = settings.EmailAccount,
                            KeyFilePath = settings.KeyFilePath,
                            RootFolder = settings.RootFolder
                        }, folderId);
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

        public AttachmentFiles AddFile(DriveRequest driveRequest, HttpRequest request, HttpResponse response)
        {
            AttachmentFiles returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                string projectId = request.Headers["ProjectId"];
                if (string.IsNullOrEmpty(projectId)) return null;
                if (settings != null)
                {
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);
                        driveRequest.EmailAccount = settings.EmailAccount;
                        driveRequest.KeyFilePath = settings.KeyFilePath;
                        string tempFolderPath = string.Format(@"{0}{1}", settings.TempUploadFolderPath, projectId);

                        AttachmentFiles file = baseDataSource.Source.UploadFile(driveRequest.FireProtectionImages, tempFolderPath);
                        if (file != null && !string.IsNullOrEmpty(file.FilePath))
                        {
                            driveRequest.FilePath = file.FilePath;
                            returnValue = baseDataSource.Source.AddFile(driveRequest);
                        }
                        else
                        {
                            return null;
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

        public AttachmentFiles CopyFile(DriveRequest driveRequest, string destinationFolderId, HttpRequest request, HttpResponse response)
        {
            AttachmentFiles returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);
                        driveRequest.EmailAccount = settings.EmailAccount;
                        driveRequest.KeyFilePath = settings.KeyFilePath;
                        returnValue = baseDataSource.Source.CopyFile(driveRequest, destinationFolderId);
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

        public AttachmentFiles MoveFile(DriveRequest driveRequest, string destinationFolderId, HttpRequest request, HttpResponse response)
        {
            AttachmentFiles returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    var mode = (AttachmentFolderMode)Enum.Parse(typeof(AttachmentFolderMode), settings.AttachmentFolderMode);
                    if (mode != AttachmentFolderMode.DATABASE)
                    {
                        BaseDataSource baseDataSource = new BaseDataSource();
                        baseDataSource.SetDataSource(mode);
                        driveRequest.EmailAccount = settings.EmailAccount;
                        driveRequest.KeyFilePath = settings.KeyFilePath;
                        returnValue = baseDataSource.Source.MoveFile(driveRequest, destinationFolderId);
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

        public AttachmentFiles RenameFile(DriveRequest driveRequest, HttpRequest request, HttpResponse response)
        {
            AttachmentFiles returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
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
                        returnValue = baseDataSource.Source.RenameFile(driveRequest);
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

        public AttachmentFilesFolder RenameFolderByRootFolderName(DriveRequest driveRequest, HttpRequest request, HttpResponse response)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
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

                        string rootFolderId = GetRootFolderId(baseDataSource, driveRequest, settings.RootFolder);
                        //check if same folder already exists
                        //string crieria = string.Format("(name='{0}' and trashed=false and '{1}' in parents) ", driveRequest.Name, rootFolderId);
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

        public AttachmentFilesFolder AddFolder(DriveRequest driveRequest, HttpRequest request, HttpResponse response)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
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
                        string criteria = GetCriteria(driveRequest.Name, driveRequest.ParentFolderId);
                        returnValue = CreateNewFolderIfNotExists(baseDataSource, driveRequest, criteria, driveRequest.Name, driveRequest.ParentFolderId);
                        //returnValue = baseDataSource.Source.AddFolder(driveRequest);
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

        public AttachmentFilesFolder AddJobRefFolderWithSubFolders(DriveRequest driveRequest, HttpRequest request, HttpResponse response)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
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
                        var rootFolderId = GetRootFolderId(baseDataSource, driveRequest, settings.RootFolder);
                        driveRequest.ParentFolderId = rootFolderId;
                        driveRequest.Description = driveRequest.Name;
                        AttachmentFilesFolder jobRefFolder = null;
                        //check if jobRef folder exitst..
                        //var crieria = string.Format("(name='{0}' and '{1}' in parents and trashed=false) ", driveRequest.Name, rootFolderId);
                        var crieria = GetCriteria(driveRequest.Name, rootFolderId);
                        if (!IsFolderExists(baseDataSource,driveRequest, crieria))
                        {
                            jobRefFolder = baseDataSource.Source.AddFolder(driveRequest);
                            //var jobRefFolder = CreateNewFolderIfNotExists(baseDataSource, driveRequest, crieria, driveRequest.Name, rootFolderId);
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
                                        if (parentFolderName  != item.ParentFolderName)
                                        {
                                            while (parentFolderList.Count>0)
                                            {
                                                //crieria = string.Format("(name='{0}' and '{1}' in parents and trashed=false) ", item.ParentFolderName, currentParent);
                                                crieria = GetCriteria(item.ParentFolderName, currentParent);
                                                AttachmentFilesFolder attachmentFilesFolder = baseDataSource.Source.GetFolderDetail(driveRequest, crieria);
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

        public AttachmentFilesFolder RenameFolder(DriveRequest driveRequest, HttpRequest request, HttpResponse response)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
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

        public bool DeleteFile(DriveRequest driveRequest, HttpRequest request, HttpResponse response)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
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
                returnValue= false;
            }
            return returnValue;
        }

        public AttachmentFiles UploadFile(Cld_Ord_Labels_Files oiFireProtectionIImages, HttpRequest request, HttpResponse response)
        {
            AttachmentFiles returnValue = null; ;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]); string projectId = request.Headers["ProjectId"];
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

        public AttachmentFiles AddFileInSpecificFolder(DriveRequest driveRequest, HttpRequest request, HttpResponse response)
        {
            AttachmentFiles returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    string crieria = string.Empty;
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

        public SimplicityFile DowloadFile(string fileId, HttpRequest request, HttpResponse response)
        {
            SimplicityFile returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
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

        public bool ShareFile(DriveRequest driveRequest, HttpRequest request, HttpResponse response)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
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
                        returnValue = baseDataSource.Source.ShareFile(driveRequest);
                    }

                }
            }
            catch (Exception ex)
            {
                returnValue = false;
            }
            return returnValue;
        }

        public bool GetFileSharedPerissions(DriveRequest driveRequest, HttpRequest request, HttpResponse response)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
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
        public AttachmentFilesFolder GetFilesByFolderNameTest(string folderName, HttpRequest request)
        {
            AttachmentFilesFolder returnValue = null;
            try
            {
                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    string crieria = string.Empty;
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

                        string rootFolderId = GetRootFolderId(baseDataSource, driveRequest, settings.RootFolder);
                        crieria = "mimeType = 'application/vnd.google-apps.folder' and name = '" + folderName + "'";
                        returnValue = baseDataSource.Source.GetFilesofFolder(driveRequest, crieria);
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
            string crieria = string.Empty;
            string currentParent = "root"; //GetRootFolderId(baseDataSource, driveRequest, mode, settings.RootFolder);
            string[] nameList = parentFolderNames.Split(',');

            currentParent = GetRootFolderId(baseDataSource, aRequest, settings.RootFolder);
            for (int i = 0; i < nameList.Length; i++)
            {
                //crieria = string.Format("(name='{0}' and '{1}' in parents and trashed=false) ", nameList[i], currentParent);
                crieria = GetCriteria(nameList[i], currentParent);
                newlyCreatedFolder = CreateNewFolderIfNotExists(baseDataSource, aRequest, crieria, nameList[i], currentParent);
                if (newlyCreatedFolder.Id == null)
                    currentParent = newlyCreatedFolder.Folders[0].Id;
                else currentParent = newlyCreatedFolder.Id;
            }
            return newlyCreatedFolder;
        }

        private AttachmentFilesFolder CreateNewFolderIfNotExists(BaseDataSource baseDataSource, DriveRequest aRequest, string crieria, string folderName, string parentFolderId)
        {
            AttachmentFilesFolder newlyCreatedFolder = baseDataSource.Source.GetFolderDetail(aRequest, crieria);
            if (newlyCreatedFolder == null)
            {
                aRequest.Name = folderName;
                aRequest.ParentFolderId = parentFolderId;
                newlyCreatedFolder = baseDataSource.Source.AddFolder(aRequest);
            }
            return newlyCreatedFolder;
        }
        private bool IsFolderExists(BaseDataSource baseDataSource, DriveRequest aRequest, string crieria)
        {
            AttachmentFilesFolder newlyCreatedFolder = baseDataSource.Source.GetFolderDetail(aRequest, crieria);
            return newlyCreatedFolder != null;
        }

        private string GetCriteria(string folderName, string parentFolderId)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            return string.Format("((name='{0}' or name='{1}' or name='{2}' or name='{3}') and '{4}' in parents and trashed=false) ", folderName,textInfo.ToUpper(folderName),textInfo.ToLower(folderName),textInfo.ToTitleCase(folderName), parentFolderId);
            //return string.Format("(name contains '{0}' and '{1}' in parents and trashed=false) ", folderName, parentFolderId);
        }

        public ResponseModel InsertGDriveFileMapping(HttpRequest request, List<GDriveMapping> obj)
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
    }
}
