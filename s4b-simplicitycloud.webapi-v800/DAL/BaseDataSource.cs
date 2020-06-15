using Google.Apis.Download;
using Google.Apis.Drive.v3;
using SimplicityOnlineBLL.Entities;


namespace SimplicityOnlineWebApi.DAL
{
    public class BaseDataSource
    {
        public BaseDataSource Source = null;
        public DriveService service = null;
        public virtual DriveService Authenticate(DriveRequest request)
        {
            return null;
        }

        public virtual DriveService AuthenticateServiceAccount(DriveRequest request)
        {
            return null;
        }
        public virtual AttachmentFiles GetMetadata_ById(DriveRequest request, string fileId)
        {
            return null;
        }
        public virtual AttachmentFiles GetFileMetadata(DriveService service, string fileId)
        {
            return null;
        }
        public virtual AttachmentFilesFolder GetMetadata_ByName(DriveRequest request, string FileOrFolderName, string parentFolderId)
        {
            return null;
        }
        public virtual AttachmentFilesFolder GetGFiles(DriveRequest request, string fileId)
        {
            return null;
        }

        public virtual AttachmentFilesFolder GetFiles(DriveService service, string search)
        {
            return null;
        }

        //public override AttachmentFilesFolder GetRootFolderContents(DriveRequest request, string searchCriteria)
        //{
        //    return null;
        //}
        public virtual AttachmentFilesFolder GetFolderContentsById(DriveRequest request, string folderId)
        {
            return null;
        }
        public virtual AttachmentFilesFolder GetAttachmentFilesAndFolders(DriveRequest request, string search)
        {
            return null;
        }      
        public virtual AttachmentFilesFolder GetFilesofFolder(DriveRequest request, string search)
        {
            return null;
        }
        public virtual AttachmentFilesFolder GetFolderDetail(DriveRequest request, string searchCriteria)
        {
            return null;
        }
        public virtual AttachmentFilesFolder AddFolder(DriveRequest request)
        {
            return null;
        }
        public virtual AttachmentFiles AddFile(DriveRequest request)
        {
            return null;
        }
        public virtual AttachmentFiles CopyFile(DriveRequest request, string destinationFolderId)
        {
            return null;
        }
        public virtual AttachmentFiles MoveFile(DriveRequest request, string destinationFolderId)
        {
            return null;
        }
        public virtual AttachmentFilesFolder RenameFolder(DriveRequest request)
        {
            return null;
        }
        public virtual bool DeleteFile(DriveRequest request)
        {
            return false;
        }
        public virtual AttachmentFiles RenameFile(DriveRequest request)
        {
            return null;
        }
        public virtual SimplicityFile DownloadFile(DriveRequest request)
        {
            return null;
        }

        public string GetFolderIdByName(DriveRequest request, string searchCriteria)
        {
            if (request == null)
                return null;
            else if (string.IsNullOrEmpty(request.EmailAccount) || string.IsNullOrEmpty(request.KeyFilePath))
                return null;

            var driveService = Authenticate(request);
            if (driveService == null) return null;
            AttachmentFilesFolder files = GetFiles(driveService, searchCriteria);
            string folderId = string.Empty;
            if (files != null && files.Folders != null && files.Folders.Count > 0)
            {
                folderId = files.Folders[0].Id;
            }
            return folderId;
        }

        public void SetDataSource(AttachmentFolderMode mode)
        {
            switch (mode)
            {
                case AttachmentFolderMode.GOOGLEDRIVE:
                    this.Source = new GoogleDrive();
                    break;
                case AttachmentFolderMode.ONEDRIVE:
                    break;
                default:
                    break;
            } 
        }

        public virtual AttachmentFiles UploadFile(Cld_Ord_Labels_Files oiFireProtectionIImages, string tempFilePath)
        {
            return null;
        }

        public virtual AttachmentFiles UploadFileOnServer(DriveRequest request, string tempFilePath)
        {
            return null;
        }

        public virtual bool ShareFile(DriveRequest request)
        {
            return true;
        }
        public virtual bool GetFileSharedPerissions(DriveRequest driveRequest)
        {
            return true;
        }

        public string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
        public string GetDriveMimeType(string docType)
        {
            string mimeType = "application/pdf";
            switch (docType)
            {
                case "spreadsheet":
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case "document":
                    mimeType= "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case "presentation":
                    mimeType= "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                    break;
                default:
                    break;
            }
            return mimeType;
        }
        
    }

    enum PermissionType
    {
        user = 0,
        group = 1,
        domain = 2,
        anyone = 3
    }
    enum PermissionRoles
    {
        owner = 0,
        writer = 1,
        commenter = 2,
        reader = 3
    }
}
