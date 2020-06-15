using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using SimplicityOnlineBLL.Entities;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IReportRepository : IRepository
    {
        ResponseModel GenerateReport(HttpRequest request,string templateData);
    }
}
