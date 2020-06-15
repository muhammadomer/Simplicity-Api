using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IMailMergeRepository : IRepository
    {
        ResponseModel GetAllMailMergeCodes(HttpRequest Request);
        ResponseModel GetAllMailMergeCodesMin(HttpRequest Request);
        ResponseModel PerformMailMergeByJobRef(HttpRequest request, RequestModel reqModel);
    }
}
