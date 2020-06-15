using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IPassthroughRepository : IRepository
    {
        PassthroughModel GetPassthroughModelByPassthroughString(string passthroughString, string projectId);
        PassthroughModel GetPassthroughModelBySequence(long sequence, string projectId);
        bool DeletePassthroughBySequence(long sequence, string projectId);
        PassthroughModel Create(PassthroughModel passthroughModel, HttpRequest request, HttpResponse Response);
    }
}
