using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IS4bFormsAssignRepository : IRepository
    {
        ResponseModel GetAllAssignUser(long FormSeq, HttpRequest request);
        ResponseModel GetUnAssignUsers(long FormSeq, HttpRequest request);
        bool UpdateFormUserAssignment(long formSeq, List<long> assignUserIds, HttpRequest request);
    }
}
