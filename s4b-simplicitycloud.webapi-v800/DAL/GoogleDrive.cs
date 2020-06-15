using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Requests;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace SimplicityOnlineWebApi.DAL
{
    public class GoogleDrive : BaseDataSource
    {
        
        //public override DriveService Authenticate(DriveRequest request)
        //{
        //    string[] scopes = new string[] { DriveService.Scope.Drive,  // view and manage your files and documents
        //                                     DriveService.Scope.DriveAppdata,  // view and manage its own configuration data
        //                                     //DriveService.Scope.DriveAppsReadonly,   // view your drive apps
        //                                     DriveService.Scope.DriveFile,   // view and manage files created by this app
        //                                     DriveService.Scope.DriveMetadataReadonly,   // view metadata for files
        //                                     DriveService.Scope.DriveReadonly,   // view files and documents on your drive
        //                                     DriveService.Scope.DriveScripts };  // modify your app scripts


        //    try
        //    {
        //        // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
        //        UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = request.ClientId, ClientSecret = request.ClientSecret}
        //                                                                                     , scopes
        //                                                                                     , request.UserName
        //                                                                                     , CancellationToken.None
        //                                                                                     , new FileDataStore("SimplicityOnline.Drive.Auth.Store")).Result;                

        //        if (service == null)
        //        {
        //            service = new DriveService(new BaseClientService.Initializer()
        //            {
        //                HttpClientInitializer = credential,
        //                ApplicationName = "Simplicity Online",
        //            });
        //        }                

        //        if (service == null)
        //        {
        //            return null;
        //        }
        //        return service;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// Authenticating to Google using a Service account
        /// Documentation: https://developers.google.com/accounts/docs/OAuth2#serviceaccount
        /// </summary>
        /// <param name="emailAccount">From Google Developer console https://console.developers.google.com</param>
        /// <param name="keyFilePath">Location of the Service account key file downloaded from Google Developer console https://console.developers.google.com</param>
        /// <returns></returns>
        public override DriveService AuthenticateServiceAccount(DriveRequest request)
        {
            if (request == null)
                return null;
            else if (string.IsNullOrEmpty(request.EmailAccount) || string.IsNullOrEmpty(request.KeyFilePath))
                return null;

            // check the file exists
            if (!System.IO.File.Exists(request.KeyFilePath))
            {
                return null;
            }

            //Google Drive scopes Documentation:   https://developers.google.com/drive/web/scopes
            //--- Following Scope was used For Non GSuite Accounts Configuration
            //string[] scopes = new string[] { DriveService.Scope.Drive  // view and manage your files and documents
            //                                 DriveService.Scope.DriveAppdata,  // view and manage its own configuration data                                             
            //                                 //DriveService.Scope.DriveAppsReadonly,   // view your drive apps
            //                                 DriveService.Scope.DriveFile,   // view and manage files created by this app
            //                                 DriveService.Scope.DriveMetadataReadonly,   // view metadata for files
            //                                 DriveService.Scope.DriveReadonly,   // view files and documents on your drive
            //                                 DriveService.Scope.DriveScripts
            //                };  // modify your app scripts     

            //---- This scope is used For G-Suite Account
            string[] scopes = new string[] { DriveService.Scope.Drive };  // modify your app scripts 

            var certificate = new X509Certificate2(request.KeyFilePath, "notasecret", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            try
            {
                ServiceAccountCredential credential = null;
                if (request.UserAccount != "" && request.UserAccount != null)
                {
                    credential = new ServiceAccountCredential(
                        new ServiceAccountCredential.Initializer(request.EmailAccount)
                        {
                            Scopes = scopes,
                            User = request.UserAccount
                        }.FromCertificate(certificate));
                }
                else {
                    credential = new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(request.EmailAccount)
                    {
                        Scopes = scopes
                    }.FromCertificate(certificate));
                }

                if (service == null) {
                    // Create the service.
                    service = new DriveService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "simplicityonlineproject"
                    });
                }
                return service;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.InnerException);
                return null;
            }
        }

        public override AttachmentFiles GetFileMetadata(DriveService service, string fileId)
        {
            AttachmentFiles attachmentFile = null;
            File file = new File();
            try
            {
                FilesResource.GetRequest getRequest = service.Files.Get(fileId);
                file = getRequest.Execute();
            }
            catch (Exception ex)
            {
                // In the event there is an error with the request.
                return null;
            }
            if (file != null)
            {
                attachmentFile = new AttachmentFiles { Id = file.Id, Description = file.Description, MimeType = file.MimeType, Name = file.Name, Size = file.Size, DateCreated = file.CreatedTime, DateLastAmended = file.ModifiedTime};
            }
            return attachmentFile;
        }

        public override AttachmentFilesFolder GetFiles(DriveService service, string search)
        {
            AttachmentFilesFolder attachments = null;
            IList<File> files = new List<File>();
            
            try
            {
                //List all of the files and directories for the current user.  
                FilesResource.ListRequest list = service.Files.List();
                //FilesResource.ListRequest list = service.Files.List()
                list.PageSize = 100;
                if (search != null)
                {
                    list.Q = search; //string.Format("'{0}' in parents", "0B7tji1MGQFO3Y3oxcnc2WUpuQXc"); //"mimeType = 'application/vnd.google-apps.folder'";
                }
                files = list.Execute().Files;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            if (files != null && files.Count > 0)
            {
                
                attachments = new AttachmentFilesFolder();
                foreach (var item in files)
                {
                    
                    //folders
                    if (item.MimeType == "application/vnd.google-apps.folder")
                    {
                        if (attachments.Folders == null)
                            attachments.Folders = new List<AttachmentFilesFolder>();
                        attachments.Folders.Add(new AttachmentFilesFolder { Id = item.Id, Description = item.Description, Name = item.Name, Size = item.Size, DateCreated = item.CreatedTime, DateLastAmended = item.ModifiedTime });
                    }
                    else //files
                    {
                        if (attachments.Files == null)
                            attachments.Files = new List<AttachmentFiles>();
                        attachments.Files.Add(new AttachmentFiles { Id = item.Id, Description = item.Description, MimeType = item.MimeType, Name = item.Name, Size = item.Size, DateCreated = item.CreatedTime, DateLastAmended = item.ModifiedTime });
                    }
                }
            }
            if (attachments != null && attachments.Files != null && attachments.Files.Count > 0)
            {
                attachments.FilesCount = attachments.Files.Count;
            }
            if (attachments != null && attachments.Folders != null && attachments.Folders.Count > 0)
            {
                attachments.FoldersCount= attachments.Folders.Count;
            }
            return attachments;
        }

        public override AttachmentFilesFolder GetAttachmentFilesAndFolders(DriveRequest request, string searchCriteria)
        {
            //var driveService = Authenticate(request);
            service = AuthenticateServiceAccount(request);
            if (service == null) return null;
            AttachmentFilesFolder files = GetFiles(service, searchCriteria);
            AttachmentFilesFolder filesInSpecific = null;
            if (files != null && files.Folders != null && files.Folders.Count > 0)
            {
                filesInSpecific = GetFiles(service, string.Format("('{0}' in parents and trashed=false)", files.Folders[0].Id));
                if (filesInSpecific == null)
                {
                    filesInSpecific = new AttachmentFilesFolder();
                }
                //set parent details                
                filesInSpecific.Id = files.Folders[0].Id;
                filesInSpecific.Name = files.Folders[0].Name;
            }
            else
            {
                filesInSpecific = new AttachmentFilesFolder();
            }
            return filesInSpecific == null ? new AttachmentFilesFolder() : filesInSpecific;
        }   
        public override AttachmentFilesFolder GetFilesofFolder(DriveRequest request, string searchCriteria)
        {
            //var driveService = Authenticate(request);
            service = AuthenticateServiceAccount(request);
            if (service == null) return null;
            AttachmentFilesFolder files = GetFiles(service, searchCriteria);
            AttachmentFilesFolder filesInSpecific = null;
            if (files != null && files.Folders != null && files.Folders.Count > 0)
            {
                filesInSpecific = GetFiles(service, string.Format("('{0}' in parents and trashed=false)", files.Folders[0].Id));
                if (filesInSpecific == null)
                {
                    filesInSpecific = new AttachmentFilesFolder();
                }
                var receivedFolder = filesInSpecific.Folders.Where(x => x.Name == "Received").FirstOrDefault();
                filesInSpecific.Id = receivedFolder.Id;
                filesInSpecific.Name = receivedFolder.Name;
                var folders = filesInSpecific.Folders;
                filesInSpecific = GetFiles(service, string.Format("('{0}' in parents and trashed=false)", filesInSpecific.Id));
                filesInSpecific.Folders = folders;
            }
            else
            {
                filesInSpecific = new AttachmentFilesFolder();
            }
            return filesInSpecific == null ? new AttachmentFilesFolder() : filesInSpecific;
        }

        public override AttachmentFilesFolder GetFolderDetail(DriveRequest request, string searchCriteria)
        {
            //var driveService = Authenticate(request);
            service = AuthenticateServiceAccount(request);
            if (service == null) return null;
            AttachmentFilesFolder files = GetFiles(service, searchCriteria);
            return files;
        }

        //public override AttachmentFilesFolder GetRootFolderContents(DriveRequest request, string searchCriteria)
        //{
        //    if (request == null)
        //        return null;
        //    else if (string.IsNullOrEmpty(request.EmailAccount) || string.IsNullOrEmpty(request.KeyFilePath))
        //        return null;

        //    var driveService = AuthenticateServiceAccount(request.EmailAccount, request.KeyFilePath);
        //    if (driveService == null) return null;
        //    AttachmentFilesFolder files = GetFiles(driveService, searchCriteria);
        //    AttachmentFilesFolder filesInSpecific = null;
        //    if (files != null && files.Folders != null && files.Folders.Count > 0)
        //    {
        //        filesInSpecific = GetFiles(driveService, string.Format("('{0}' in parents and trashed=false)", files.Folders[0].Id));
        //    }
        //    return filesInSpecific;
        //}

        public override AttachmentFilesFolder GetFolderContentsById(DriveRequest request, string folderId)
        {
            if (request == null)
                return null;
            else if (string.IsNullOrEmpty(request.EmailAccount) || string.IsNullOrEmpty(request.KeyFilePath))
                return null;

            service = AuthenticateServiceAccount(request);
            if (service == null)
                return null;
            AttachmentFilesFolder filesInFolder = null;
            filesInFolder = GetFiles(service, string.Format("('{0}' in parents and trashed=false)", folderId));
            return filesInFolder;
        }

        private AttachmentFilesFolder CreateFolder(DriveService service, DriveRequest request)
        {
            File metaData = new File();
            metaData.Name = request.Name.ToUpper();
            metaData.Description = request.Description;
            metaData.MimeType = request.MimeType;
            metaData.Parents = new List<string> { request.ParentFolderId };
            metaData.CreatedTime = DateTime.Now;
            //metaData.LastModifyingUser = request.UserName;

            var driveRequest = service.Files.Create(metaData);
            File newFolder = null;
            try
            {
                newFolder = driveRequest.Execute();
            }
            catch (Exception ex)
            {
                throw;
            }

            AttachmentFilesFolder folder = null;
            if (newFolder != null) {
                folder = new AttachmentFilesFolder { Id = newFolder.Id, Name = newFolder.Name, Description = newFolder.Description };
                folder.Folders = new List<AttachmentFilesFolder>();
                folder.Folders.Add(folder);
            }
            return folder;
        }

        private AttachmentFilesFolder UpdateFolderName(DriveService service, DriveRequest request)
        {
            File metaData = new File();
            metaData.Name = request.Name;
            metaData.ModifiedTime = DateTime.Now;
            var driveRequest = service.Files.Update(metaData, request.FolderId);
            File newFolder = null;
            try
            {
                newFolder = driveRequest.Execute();
            }
            catch (Exception ex)
            {
                throw;
            }
            AttachmentFilesFolder folder = null;
            if (newFolder != null)
            {
                folder = new AttachmentFilesFolder { Id = newFolder.Id, Name = newFolder.Name, Description = newFolder.Description };
            }
            return folder;
        }

        internal bool DeleteFile(DriveService service, DriveRequest request)
        {
            bool result = false;
            string deletedFile = null;
            try
            {
                deletedFile = service.Files.Delete(request.FileId).Execute();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                throw;
            }
            return result;
        }

        private AttachmentFiles CreateFile(DriveService service, DriveRequest request)
        {
            File metaData = new File();
            metaData.Name = request.Name;
            metaData.Parents = new List<string> { request.ParentFolderId  };
            FilesResource.CreateMediaUpload driveRequest = null;
            File file = null;
            AttachmentFiles newFile = null;
            try
            {
                byte[] byteArray = System.IO.File.ReadAllBytes(request.FilePath);
                using (var stream = new System.IO.MemoryStream(byteArray))
                {
                    var contentType = GetMimeType(request.Name);
                    driveRequest = service.Files.Create(metaData, stream, contentType);                    
                    driveRequest.Upload();
                }
                file = driveRequest.ResponseBody;
            }
            catch (Exception ex)
            {
                throw;
            }
            if (file != null)
            {
                newFile = new AttachmentFiles { Id = file.Id, Name = file.Name, Description = file.Description, MimeType = file.MimeType, Size = file.Size };
            }
            return newFile;
        }

        private AttachmentFiles UpdateFileName(DriveService service, DriveRequest request)
        {
            File metaData = new File();
            metaData.Name = request.Name;
            File file = null;
            AttachmentFiles newFile = null;
            try
            {
                FilesResource.UpdateRequest updateRequest = service.Files.Update(metaData, request.FileId);
                file = updateRequest.Execute();

            }
            catch (Exception ex)
            {
                throw;
            }

            if (file != null)
            {
                newFile = new AttachmentFiles { Id = file.Id, Name = file.Name, Description = file.Description, MimeType = file.MimeType, Size = file.Size };
            }
            return newFile;
        }

        private string GetLocalMimeType(string googleMimeType)
        {
            Dictionary<string, string> mimeTypes = new Dictionary<string, string>();
            mimeTypes.Add("application/vnd.google-apps.audio", "");
            mimeTypes.Add("application/vnd.google-apps.document", "docx");
            mimeTypes.Add("application/vnd.google-apps.drawing", "jpeg");
            mimeTypes.Add("application/vnd.google-apps.file", "");
            mimeTypes.Add("application/vnd.google-apps.folder", "");
            mimeTypes.Add("application/vnd.google-apps.form", "");
            mimeTypes.Add("application/vnd.google-apps.fusiontable", "");
            mimeTypes.Add("application/vnd.google-apps.map", "");
            mimeTypes.Add("application/vnd.google-apps.photo", "jpeg");
            mimeTypes.Add("application/vnd.google-apps.presentation", "pptx");
            mimeTypes.Add("application/vnd.google-apps.script", "js");
            mimeTypes.Add("application/vnd.google-apps.sites", "");
            mimeTypes.Add("application/vnd.google-apps.spreadsheet", "xlsx");
            mimeTypes.Add("application/vnd.google-apps.unknown", "");
            mimeTypes.Add("application/vnd.google-apps.video", "");
            return mimeTypes[googleMimeType];
        }
        internal SimplicityFile DownloadFileContents(DriveService service, DriveRequest driveRequest)
        {
            FilesResource.GetRequest request = service.Files.Get(driveRequest.FileId);
            
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            SimplicityFile downloadFile = new SimplicityFile();
            downloadFile.FileName = driveRequest.Name;
            if (!string.IsNullOrEmpty(downloadFile.FileName))
            {
                downloadFile.ContentType = MimeKit.MimeTypes.GetMimeType(downloadFile.FileName);
            }
            // Add a handler which will be notified on progress changes. It will notify on each chunk download and when the download is completed or failed.
            try
            {
                //request.MediaDownloader.ProgressChanged += MediaDownloader_ProgressChanged;
                request.MediaDownloader.ProgressChanged += (IDownloadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            {
                                //Console.WriteLine(progress.BytesDownloaded);
                                
                                break;
                            }
                        case DownloadStatus.Completed:
                            {
                                stream.Position = 0;
                                downloadFile.MemStream = stream;
                                break;
                            }
                        case DownloadStatus.Failed:
                            {
                                stream = null;
                                downloadFile.MemStream = null;
                                //progress.BytesDownloaded
                                break;
                            }
                    }
                };
                request.Download(stream);

            }
            catch (Exception exp)
            {
                stream = null;
                downloadFile.MemStream = null;
                //fileStream = null;
                throw;
            }
            return downloadFile;
        }
        internal SimplicityFile DownloadDriveContents(DriveService service, DriveRequest driveRequest,string docType)
        {
            string mimeType = GetDriveMimeType(docType);
            FilesResource.ExportRequest request = service.Files.Export(driveRequest.FileId, mimeType);
            SimplicityFile downloadFile = new SimplicityFile();
            downloadFile.FileName = driveRequest.Name;
            if (!string.IsNullOrEmpty(downloadFile.FileName))
            {
                downloadFile.ContentType = MimeKit.MimeTypes.GetMimeType(downloadFile.FileName);
            }
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            try
            {
                request.MediaDownloader.ProgressChanged += (IDownloadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            {
                                break;
                            }
                        case DownloadStatus.Completed:
                            {
                                stream.Position = 0;
                                downloadFile.MemStream = stream;
                                break;
                            }
                        case DownloadStatus.Failed:
                            {
                                stream = null;
                                downloadFile.MemStream = null;
                                break;
                            }
                    }
                };
                request.Download(stream);

            }
            catch (Exception exp)
            {
                stream = null;
                downloadFile.MemStream = null;
                throw;
            }
            return downloadFile;
        }

        internal SimplicityFile DownloadDocument(DriveService service, DriveRequest driveRequest)
        {
            var fileDetail = GetFileMetadata(service, driveRequest.FileId);
            
            if (fileDetail.Name.Contains("."))
            {
                driveRequest.Name = fileDetail.Name;
                return DownloadFileContents(service, driveRequest);
            }
            else
            {
                string docType = "";
                if (fileDetail.MimeType.Contains("spreadsheet"))
                {
                    docType = "spreadsheet";
                    driveRequest.Name = string.Format("{0}.{1}", fileDetail.Name, "xlsx");
                }
                else if (fileDetail.MimeType.Contains("document"))
                {
                    docType = "document";
                    driveRequest.Name = string.Format("{0}.{1}", fileDetail.Name, "docx");

                }
                else if (fileDetail.MimeType.Contains("presentation"))
                {
                    docType = "presentation";
                    driveRequest.Name = string.Format("{0}.{1}", fileDetail.Name, "pptx");
                }
                return DownloadDriveContents(service, driveRequest, docType);
            }
        }

        //private void MediaDownloader_ProgressChanged(IDownloadProgress progress)
        //{
        //    switch (progress.Status)
        //    {
        //        case DownloadStatus.Downloading:
        //            {
        //                Console.WriteLine(progress.BytesDownloaded);
        //                break;
        //            }
        //        case DownloadStatus.Completed:
        //            {
        //                //Console.WriteLine("Download complete.");    
        //                string filePath = @"C:\Temp\KILNBRIDGE\Sheeta.xlsx";
        //                using (System.IO.FileStream file = new System.IO.FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
        //                {
        //                    stream.Position = 0;
        //                    stream.WriteTo(file);
        //                    stream.Close();
        //                    file.Close();
        //                    LoadMemoryStream(filePath);
        //                    //System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //                    //using (System.IO.FileStream fs=System.IO.File.OpenRead(filePath))
        //                    //{
        //                    //    fs.CopyTo(ms);
        //                    //}
        //                }
        //                break;
        //            }
        //        case DownloadStatus.Failed:
        //            {
        //                Console.WriteLine("Download failed.");
        //                break;
        //            }
        //    }
        //}

        //internal System.IO.MemoryStream LoadMemoryStream(string filePath)
        //{
        //    System.IO.MemoryStream memStream = null;
        //    using (System.IO.FileStream fileStream = System.IO.File.OpenRead(filePath))
        //    {
        //        //create new MemoryStream object
        //        memStream = new System.IO.MemoryStream();
        //        memStream.SetLength(fileStream.Length);
        //        //read file to MemoryStream
        //        fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
        //        fileStream.Close();
        //    }
        //    return memStream;
        //}
        public override AttachmentFilesFolder AddFolder(DriveRequest request)
        {
            service = AuthenticateServiceAccount(request);
            if (service == null) return null;
            request.MimeType= "application/vnd.google-apps.folder";
            return CreateFolder(service, request);
        }

        public override AttachmentFiles CopyFile(DriveRequest request, string destinationFolderId)
        {
            service = AuthenticateServiceAccount(request);
            if (service == null) return null;
            File metaData = new File();
            metaData.Parents = new List<string> { destinationFolderId };
            AttachmentFiles returnValue = null;
            try
            {
                File newFile = null;
                newFile = service.Files.Copy(metaData, request.FileId).Execute();
                if (newFile != null)
                    returnValue = new AttachmentFiles { Id = newFile.Id, Name = newFile.Name, Description = newFile.Description, MimeType = newFile.MimeType, Size = newFile.Size };
            }
            catch (Exception ex)
            {
                throw;
            }
            return returnValue;
        }

        public override AttachmentFiles MoveFile(DriveRequest request, string destinationFolderId)
        {
            service = AuthenticateServiceAccount(request);
            if (service == null) return null;
            AttachmentFiles newFile = null;
            try
            {
                FilesResource.GetRequest getRequest = service.Files.Get(request.FileId);
                getRequest.Fields = "parents";
                File file = getRequest.Execute();
                var previousParents = String.Join(",", file.Parents);
                var updateRequest = service.Files.Update(new File(), request.FileId);
                updateRequest.Fields = "id, parents";
                updateRequest.AddParents = destinationFolderId;
                updateRequest.RemoveParents = previousParents;
                file = updateRequest.Execute();
                if (file != null)
                {
                    newFile = new AttachmentFiles { Id = file.Id, Name = file.Name, Description = file.Description, MimeType = file.MimeType, Size = file.Size };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return newFile;
        }

        public override AttachmentFiles AddFile(DriveRequest request)
        {
            service = AuthenticateServiceAccount(request);
            if (service == null) return null;
            return CreateFile(service, request);
        }

        public override AttachmentFilesFolder RenameFolder(DriveRequest request)
        {
            service = AuthenticateServiceAccount(request);
            if (service == null) return null;
            return UpdateFolderName(service, request);
        }

        public override bool DeleteFile(DriveRequest request)
        {
            service = AuthenticateServiceAccount(request);
            if (service == null) return false;
            return DeleteFile(service, request);
        }

        public override AttachmentFiles RenameFile(DriveRequest request)
        {
            service = AuthenticateServiceAccount(request);
            if (service == null) return null;
            return UpdateFileName(service, request);
        }

        public override SimplicityFile DownloadFile(DriveRequest request)
        {
            service = AuthenticateServiceAccount(request);
            if (service == null) return null;
            return DownloadDocument(service, request);
        }

        public override AttachmentFiles UploadFile(Cld_Ord_Labels_Files oiFireProtectionIImages, string tempFilePath)
        {
            if(!System.IO.Directory.Exists(tempFilePath))
            {
                System.IO.Directory.CreateDirectory(tempFilePath);
            }
            AttachmentFiles attachmentFile = new AttachmentFiles();
            string imagePathAndFileName = string.Format(@"{0}\{1}", tempFilePath, oiFireProtectionIImages.ImageName);
            if (oiFireProtectionIImages.FlgIsBase64Img)
            {
                if (oiFireProtectionIImages.Base64Img != null && oiFireProtectionIImages.Base64Img != "")
                {
                    byte[] imageBytes = Convert.FromBase64String(oiFireProtectionIImages.Base64Img);
                    System.IO.File.WriteAllBytes(imagePathAndFileName, imageBytes);
                    oiFireProtectionIImages.Base64Img = null;
                }
            }
            attachmentFile.FilePath = imagePathAndFileName;
            return attachmentFile;
        }

        public override bool ShareFile(DriveRequest driveRequest)
        {
            service = AuthenticateServiceAccount(driveRequest);
            if (service == null) return false;
            var batch = new BatchRequest(service);

            BatchRequest.OnResponse<Permission> callback = delegate (
                Permission permission,
                RequestError error,
                int index,
                System.Net.Http.HttpResponseMessage message)
            {
                if (error != null)
                {
                    //if error
                    //Console.WriteLine(error.Message);
                }
                else
                {
                    //successfull
                    var id = permission.Id;
                    //Console.WriteLine("Permission ID: " + permission.Id);
                }
            };

            Permission userPermission = new Permission();
            userPermission.Type = PermissionType.user.ToString();
            userPermission.Role = PermissionRoles.writer.ToString();
            userPermission.EmailAddress = driveRequest.Email;
            //userPermission.DisplayName = "user a";
            var request = service.Permissions.Create(userPermission, driveRequest.FileId);
            request.Fields = "id";
            //request.Fields = "displayName";
            batch.Queue(request, callback);
            var task = batch.ExecuteAsync();

            return true;
        }

        public override bool GetFileSharedPerissions(DriveRequest driveRequest)
        {
            service = AuthenticateServiceAccount(driveRequest);
            if (service == null) return false;
            var batch = new BatchRequest(service);

            //BatchRequest.OnResponse<Permission> callback = delegate (
            //    Permission permission,
            //    RequestError error,
            //    int index,
            //    System.Net.Http.HttpResponseMessage message)
            //{
            //    if (error != null)
            //    {
            //        //if error
            //        //Console.WriteLine(error.Message);
            //    }
            //    else
            //    {
            //        //successfull
            //        //Console.WriteLine("Permission ID: " + permission.Id);
            //    }
            //};

            var request = service.Permissions.List(driveRequest.FileId).Execute();
            //request.Fields = "id";
            //batch.Queue(request, callback);
            //var task = batch.ExecuteAsync();

            return true;
        }

        public override AttachmentFilesFolder GetGFiles(DriveRequest request, string search)
        {
            AttachmentFilesFolder storageContents = null;
            service = AuthenticateServiceAccount(request);
            if (service == null) return storageContents;
            storageContents = GetFiles(service, search);
            return storageContents;
        }

        public override AttachmentFiles GetMetadata_ById(DriveRequest request, string fileId)
        {
            AttachmentFiles attachmentFile = null;
            service = AuthenticateServiceAccount(request);
            if (service == null) return attachmentFile;

            File file = new File();
            try
            {
                FilesResource.GetRequest getRequest = service.Files.Get(fileId);
                file = getRequest.Execute();
            }
            catch (Exception ex)
            {
                // In the event there is an error with the request.
                return null;
            }
            if (file != null)
                attachmentFile = new AttachmentFiles { Id = file.Id, Description = file.Description, MimeType = file.MimeType, Name = file.Name, Size = file.Size, DateCreated = file.CreatedTime, DateLastAmended = file.ModifiedTime };
            return attachmentFile;
        }

        public override AttachmentFilesFolder GetMetadata_ByName(DriveRequest request, string FileOrFolderName, string parentFolderId)
        {
            string searchQuery = string.Format("(name='{0}' and '{1}' parents and trashed=false)", FileOrFolderName, parentFolderId);
            return GetGFiles(request, searchQuery);
        }

        private AttachmentFilesFolder PrepareAttachmentFilesFolder(IList<File> files)
        {
            AttachmentFilesFolder storageContents = null;
            if (files != null && files.Count > 0)
            {
                storageContents = new AttachmentFilesFolder();
                foreach (var item in files)
                {
                    if (item.MimeType == "application/vnd.google-apps.folder") //folders
                    {
                        if (storageContents.Folders == null)
                            storageContents.Folders = new List<AttachmentFilesFolder>();
                        storageContents.Folders.Add(new AttachmentFilesFolder { Id = item.Id, Description = item.Description, Name = item.Name, Size = item.Size, DateCreated = item.CreatedTime, DateLastAmended = item.ModifiedTime });
                    }
                    else //files
                    {
                        if (storageContents.Files == null)
                            storageContents.Files = new List<AttachmentFiles>();
                        storageContents.Files.Add(new AttachmentFiles { Id = item.Id, Description = item.Description, MimeType = item.MimeType, Name = item.Name, Size = item.Size, DateCreated = item.CreatedTime, DateLastAmended = item.ModifiedTime });
                    }
                }
            }
            if (storageContents != null && storageContents.Files != null && storageContents.Files.Count > 0)
                storageContents.FilesCount = storageContents.Files.Count;
            if (storageContents != null && storageContents.Folders != null && storageContents.Folders.Count > 0)
                storageContents.FoldersCount = storageContents.Folders.Count;
            return storageContents;
        }

        public override AttachmentFiles UploadFileOnServer(DriveRequest request, string tempFilePath)
        {
            AttachmentFiles attachmentFile = new AttachmentFiles();
            if (!System.IO.Directory.Exists(tempFilePath))
                System.IO.Directory.CreateDirectory(tempFilePath);
            if (request.File.Base64String != null && request.File.Base64String != "" && !string.IsNullOrEmpty(request.Name))
            {
                string imagePathAndFileName = string.Format(@"{0}\{1}", tempFilePath, request.Name);
                byte[] imageBytes = Convert.FromBase64String(request.File.Base64String);
                System.IO.File.WriteAllBytes(imagePathAndFileName, imageBytes);
                request.File.Base64String = null;
                attachmentFile.FilePath = imagePathAndFileName;
            }
            return attachmentFile;
        }


        //public string GetRootFolder(DriveService service)
        //{
        //    try
        //    {
        //        About about = service.About.Get().Execute();

        //        Console.WriteLine("Current user name: " + about.Name);
        //        Console.WriteLine("Root folder ID: " + about.RootFolderId);
        //        Console.WriteLine("Total quota (bytes): " + about.QuotaBytesTotal);
        //        Console.WriteLine("Used quota (bytes): " + about.QuotaBytesUsed);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("An error occurred: " + e.Message);
        //    }
        //}

    }
}

#region Archive


#endregion
