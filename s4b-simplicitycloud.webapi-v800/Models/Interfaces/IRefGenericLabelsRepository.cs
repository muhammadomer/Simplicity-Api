using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IRefGenericLabelsRepository : IRepository
    {
        List<RefGenericLabels> GetAllGenericLabels(HttpRequest request, HttpResponse response);
        ResponseModel AddGenericLable(RefGenericLabels obj,HttpRequest request);
        RefGenericLabels GetGenericLableById(long id,HttpRequest request, HttpResponse response);
        ResponseModel UpdateGenericLable(RefGenericLabels Obj, HttpRequest request);

    }
}
