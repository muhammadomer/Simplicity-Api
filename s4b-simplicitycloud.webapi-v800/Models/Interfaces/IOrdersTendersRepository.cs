using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using SimplicityOnlineBLL.Entities;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IOrdersTendersRepository : IRepository
    {
        List<OrdTendersTP> GetTendersSpecificationsByEntityId(long EntityId, bool flgFilterByStatus, int statusSequence,bool fliterOutFutureTender, bool flgFilterByFinalised, bool flgTenderFinalised, HttpRequest request);
        List<OrdTendersTP> GetTenderTPDetailsBySequence(long sequence, bool loadAll, HttpRequest request);
        OrdTendersSpecs GetTenderDetailsBySequence(long sequence, bool loadAll, HttpRequest request);
        List<OrdTendersSpecsFiles> GetTenderSpecsFilesByTenderSpecSequence(long tenderSpecSequence, HttpRequest request);
        List<OrdTendersTPFiles> GetTendersTPFilesByTenderTPSequence(long tenderTPSequence, HttpRequest request);
        List<RefOrdTenderStatus> GetRefOrderTenderStatus(HttpRequest request);
        OrdTendersTPFiles InsertOrdTendersTPFiles(OrdTendersTPFiles ordTendersTPFiles, HttpRequest request);
        bool UpdateOrdTendersTP(OrdTendersTP ordTendersTP, HttpRequest request);
        List<OrdTendersTPFiles> GetTendersTPFilesByGuId(long guId, HttpRequest request);
        List<OrdTendersSpecsFiles> GetTenderSpecsFilesByGuId(long guId, HttpRequest request);
        bool UpdateOrdTendersTPFileDeletedFlag(long sequence, bool flgDeleted, HttpRequest request);
        List<OrdTendersTPQS> GetTendersQAsByTenderTPSequence(long sequence, HttpRequest request);
        OrdTendersTPQS InsertOrdTendersTPQA(OrdTendersTPQS ordTendersTPQA, HttpRequest request);
        OrdTendersTPQS GetTenderQuestionDetail(long sequence, HttpRequest request);
        List<OrdTendersSpecs> GetTendersSpecificationsByClientId(long ClientId, bool flgFilterByStatus, int statusSequence, bool flgFilterByAwarded, bool flgTenderAwarded, HttpRequest request);
        List<OrdTendersSpecs> GetTendersSpecificationsByViewerId(long ViewerId, bool flgFilterByStatus, int statusSequence, bool flgFilterByAwarded, bool flgTenderAwarded, HttpRequest request);
        bool SendTenderNotificationToJobManager(long sequence, long jobSequence,long tpQsSequence,long tpFileSequence,string notificationType, HttpRequest request,HttpResponse response);
        OrdTendersSpecsClient GetTenderDetailsBySequence4Client(long sequence, bool loadAll, HttpRequest request);
    }
}
