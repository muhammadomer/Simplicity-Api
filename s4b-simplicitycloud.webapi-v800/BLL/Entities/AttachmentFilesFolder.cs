using System;
using System.Collections.Generic;
using System.IO;

namespace SimplicityOnlineBLL.Entities
{

    public class AttachmentFilesFolder
    {
        public string Description { get; set; }
        //public string ETag { get; set; }
        public string Id { get; set; }
        public string ParentFolderId { get; set; }
        public string Name { get; set; }
        public string ParentFolderName { get; set; }
        public long? Size { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public List<AttachmentFilesFolder> Folders { get; set; }
        public List<AttachmentFiles> Files { get; set; }

        public int FilesCount { get; set; }
        public int FoldersCount { get; set; }


        //public string AppProperties { get; set; }
        //public string Capabilities { get; set; }
        //public string ContentHints { get; set; }

        //public string CreatedTimeRaw { get; set; }


        //public string ExplicitlyTrashed { get; set; }
        //public string FileExtension { get; set; }
        //public string FolderColorRgb { get; set; }
        //public string FullFileExtension { get; set; }
        //public string HeadRevisionId { get; set; }
        //public string IconLink { get; set; }

        //public string ImageMediaMetadata { get; set; }

        //public string LastModifyingUser { get; set; }
        //public string Md5Checksum { get; set; }

        //public string ModifiedByMeTime { get; set; }
        //public string ModifiedByMeTimeRaw { get; set; }

        //public string ModifiedTimeRaw { get; set; }

        //public string OwnedByMe { get; set; }
        //public string Owners{ get; set; }

        //public string Parents { get; set; }
        //public string Permissions { get; set; }
        //public string Properties { get; set; }
        //public string MyProperty { get; set; }
        //public string QuotaBytesUsed { get; set; }
        //public string Shared { get; set; }
        //public string SharedWithMeTime { get; set; }
        //public string SharedWithMeTimeRaw { get; set; }
        //public string SharingUser { get; set; }

        //public string Spaces { get; set; }
        //public string Starred { get; set; }

        //public string ThumbnailLink { get; set; }
        //public string Trashed { get; set; }
        //public string Version { get; set; }
        //public string VideoMediaMetadata { get; set; }
        //public string ViewedByMe { get; set; }
        //public string ViewedByMeTime { get; set; }
        //public string ViewedByMeTimeRaw { get; set; }
        //public string ViewersCanCopyContent { get; set; }
        //public string WebContentLink { get; set; }
        //public string WebViewLink { get; set; }
        //public string WritersCanShare { get; set; }

    }

    public class AttachmentFiles
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

    public class AttachmentFilesFolderTransfer
    {
        public string[] SourceFilesId { get; set; }
        public string DestinationFolderId { get; set; }
    }

    public class DriveRequest
    {
        public string EmailAccount { get; set; }
        public string UserAccount { get; set; }
        public string KeyFilePath { get; set; }
        public string RootFolder { get; set; }
        //',' seperated list of folders
        public string ParentFolderNames { get; set; }
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
        //list of folders which will be created when a new jobRef is created
        public List<string> SubFolders { get; set; }
        public string Email { get; set; }
        public Cld_Ord_Labels_Files FireProtectionImages { get; set; }
        public SimplicityFile File { get; set; }
    }

    public enum AttachmentFolderMode
    {
        DATABASE=1,
        GOOGLEDRIVE=2,
        ONEDRIVE=3,
    }

    public enum MimeTypes
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

    public enum GDriveSearchQuery
    {
        FolderContent_ByFolderId,
        FolderMetaData_ByFolderId,
        FolderContent_ByFolderName,
        FolderMetaData_ByFolderName,
        FolderExistance_ByFolderId,
        FileExistance_ByFileId,
        RootFolderContents_ByFolderId,
        GetFolderId_ByFolderName_AndParentId
    }
    public class GDriveMapping
    {
        public long? sequence { get; set; }
        public string fileName { get; set; }
        public string oldFileId { get; set; }
        public string newFileId { get; set; }
    }
}
