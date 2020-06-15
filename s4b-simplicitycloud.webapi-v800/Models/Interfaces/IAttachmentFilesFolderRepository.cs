using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IAttachmentFilesFolderRepository : IRepository
    {
        string Message { set; get; }
        AttachmentFilesFolder GetAttachmentFilesFolderByFolderName(string jobRef, HttpRequest request);
        AttachmentFilesFolder GetRootFolderContents(HttpRequest request, HttpResponse response);
        AttachmentFilesFolder GetFolderContentsById(string folderId, HttpRequest request, HttpResponse response);
        AttachmentFiles AddFile(DriveRequest driveRequest, HttpRequest request, HttpResponse response);
        AttachmentFilesFolder AddFolder(DriveRequest driveRequest, HttpRequest request, HttpResponse response);
        AttachmentFilesFolder AddJobRefFolderWithSubFolders(DriveRequest driveRequest, HttpRequest request, HttpResponse response);

        AttachmentFiles CopyFile(DriveRequest driveRequest, string destinationFolderId, HttpRequest request, HttpResponse response);

        AttachmentFiles MoveFile(DriveRequest driveRequest, string destinationFolderId, HttpRequest request, HttpResponse response);

        AttachmentFiles RenameFile(DriveRequest driveRequest, HttpRequest request, HttpResponse response);

        AttachmentFilesFolder RenameFolderByRootFolderName(DriveRequest driveRequest, HttpRequest request, HttpResponse response);

        AttachmentFilesFolder RenameFolder(DriveRequest driveRequest, HttpRequest request, HttpResponse response);
        bool DeleteFile(DriveRequest driveRequest, HttpRequest request, HttpResponse response);
        AttachmentFiles UploadFile(Cld_Ord_Labels_Files oiFireProtectionIImages, HttpRequest request, HttpResponse response);
        AttachmentFiles AddFileInSpecificFolder(DriveRequest driveRequest, HttpRequest request, HttpResponse response);
        bool ShareFile(DriveRequest driveRequest, HttpRequest request, HttpResponse response);
        bool GetFileSharedPerissions(DriveRequest driveRequest, HttpRequest request, HttpResponse response);
        SimplicityFile DowloadFile(string fileId, HttpRequest request, HttpResponse response);
        ResponseModel GetMailMergeFolderContents(HttpRequest request);
        ResponseModel InsertGDriveFileMapping(HttpRequest request, List<GDriveMapping> obj);
    }
}
