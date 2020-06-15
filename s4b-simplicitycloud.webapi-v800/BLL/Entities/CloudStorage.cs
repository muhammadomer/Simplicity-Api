using System;
using System.Collections.Generic;


namespace SimplicityOnlineBLL.Entities
{

    // NOTE: its a replace of AttachmentFilesFolder class
    public class CloudStorageObject 
        //Folder or files
    {
        public string Description { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string ObjectPath { get; set; }
        public string ParentFolderId { get; set; }
        public string ParentFolderName { get; set; }
        public long? Size { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public List<CloudStorageObject> Folders { get; set; }
        public List<CloudStorageObject> Files { get; set; }

        public int FilesCount { get; set; }
        public int FoldersCount { get; set; }
    }

    public class CloudStorageObjectsTransfer
    {
        public string[] SourceFilesId { get; set; }
        public string DestinationFolderId { get; set; }
    }

    public class CloudStorageRequest
    {
        public string EmailAccount { get; set; }
        public string UserAccount { get; set; }
        public string KeyFilePath { get; set; }
        public string RootFolder { get; set; }
        public string ParentFolderNames { get; set; } //',' seperated list of folders
        public string ParentFolderName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentFolderId { get; set; }
        public string FileId { get; set; }
        public string FolderId { get; set; }
        public string MimeType { get; set; }
        public string SearchCriteria { get; set; }
        public bool IsRootFolder { get; set; }
        public string FilePath { get; set; }

    }

    public class CloudFile
    {
        public string Description { get; set; }
        public string Id { get; set; }
        public string MimeType { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public long? Size { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }

    public enum CloudStorageType
    {
        S3= 1,
        GOOGLEDRIVE = 2,
        ONEDRIVE = 3,
    }

    public enum MimeType
    {
        audio,
        document,
        drawing,
        file,
        folder,
        form,
        fusiontable,
        map,
        photo,
        presentation,
        script,
        sites,
        spreadsheet,
        unknown,
        video
    }
}
