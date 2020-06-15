using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IRossumRepository : IRepository
    {
        public string DebugContent { get; set; }
        //string GetToken(RequestHeaderModel header);
        ResponseModel GetUnConfirmedFilesList(RequestHeaderModel header,DateTime? fromDate, DateTime? toDate);
        Task<ResponseModel> GetAnnotationURL(RequestHeaderModel header, int annotationId, string host);
        ResponseModel InsertRossumFiles(RequestHeaderModel header, List<RossumFile> receivedFilesList);
        ResponseModel DeleteRossumFile(long sequence, RequestHeaderModel header);
        Task<string> RossumWebhook(RossWebHook hook, RequestHeaderModel header);
        Task ScheduleUploadToRossumAsync(RequestHeaderModel header);
        List<RossumDocumentType> GetDocumentTypes(RequestHeaderModel header);
        //string GetRossumToken(RequestHeaderModel header);
        Task<ResponseModel> SupplierInvoiceImportAsync(int annotationId, RequestHeaderModel header);
        Task SchedulerRossumMainCall();
        ResponseModel GetBySequence(RequestHeaderModel header, long sequence);
        RossumFile GetDebugData(RequestHeaderModel header, long sequence);
        public ResponseModel MoveInvoiceFileToSuccessFolder(InvoiceItemised invoice, RequestHeaderModel header);
    }
}
