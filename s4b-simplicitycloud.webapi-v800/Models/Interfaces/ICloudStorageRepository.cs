using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface ICloudStorageRepository : IRepository
    {
        string Message { set; get; }
        AttachmentFilesFolder GetContentsByFolderName(string folderName, RequestHeaderModel header);
        AttachmentFilesFolder GetRootFolderContents(RequestHeaderModel header);
        AttachmentFilesFolder GetFolderContentById(string folderId, RequestHeaderModel header);
        AttachmentFiles AddFile(DriveRequest driveRequest, RequestHeaderModel header);
        AttachmentFilesFolder AddFolder(DriveRequest driveRequest, RequestHeaderModel header);

        AttachmentFiles CopyFile(DriveRequest driveRequest, string destinationFolderId, RequestHeaderModel header);

        AttachmentFiles MoveFile(DriveRequest driveRequest, string destinationFolderId, RequestHeaderModel header);

        AttachmentFiles RenameFile(DriveRequest driveRequest, RequestHeaderModel header);

        AttachmentFilesFolder RenameFolderByRootFolderName(DriveRequest driveRequest, RequestHeaderModel header);

        AttachmentFilesFolder RenameFolder(DriveRequest driveRequest, RequestHeaderModel header);
        bool DeleteFile(DriveRequest driveRequest, RequestHeaderModel header);
        AttachmentFiles UploadFile(Cld_Ord_Labels_Files oiFireProtectionIImages, RequestHeaderModel header);
        AttachmentFiles AddFileInSpecificFolder(DriveRequest driveRequest, RequestHeaderModel header);
        bool ShareFile(DriveRequest driveRequest, RequestHeaderModel header);
        bool GetFileSharedPerissions(DriveRequest driveRequest, RequestHeaderModel header);
        SimplicityFile DowloadFile(string fileId, RequestHeaderModel header);
        //ResponseModel GetMailMergeFolderContents(RequestHeaderModel header);
        ResponseModel InsertGDriveFileMapping(RequestHeaderModel header, List<GDriveMapping> obj);
        AttachmentFilesFolder GetFileOrFolderMeta_ByName(RequestHeaderModel header, string folderName, string parentFolderId);
    }
}
