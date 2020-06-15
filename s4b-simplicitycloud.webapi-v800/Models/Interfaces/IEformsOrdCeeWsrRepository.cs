using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IEformsOrdCeeWsrRepository : IRepository
    {
        EformsOrdCeeWsr Insert(EformsOrdCeeWsr Obj, HttpRequest request);
        List<EformsOrdCeeWsr> GetAllBySequence(HttpRequest request, HttpResponse response, long sequence);
        EformsOrdCeeWsr Update(EformsOrdCeeWsr model, HttpRequest request);
    }
}
