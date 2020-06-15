using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace SimplicityOnlineWebApi.Models.Interfaces
{
   public interface IOINFSubmissionDataRepository : IRepository
    {
        List<OINFSubmissionData> GetAllOINFSubmissionData(HttpRequest request, int sequence);
        OINFSubmissionData insert(HttpRequest request, OINFSubmissionData obj);
    }
}
