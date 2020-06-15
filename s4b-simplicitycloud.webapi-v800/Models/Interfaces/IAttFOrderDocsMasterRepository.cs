using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IAttFOrderDocsMasterRepository : IRepository
    {
        SimplicityFile GetAttfOrderDocsMasterByJobSequenceAndSequence(HttpRequest request, long jobSequence, long sequence);
        List<AttfOrdDocsMasters> GetAttfOrderDocsMasterByJobSequence(HttpRequest request, long jobSequence);
        bool PutAttFOrdDocsMaster(HttpRequest request, AttfOrdDocsMastersFile attfOrdDocsMastersFile);
    }
}