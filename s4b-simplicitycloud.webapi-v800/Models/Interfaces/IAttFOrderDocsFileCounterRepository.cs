using Microsoft.AspNetCore.Http;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IAttFOrderDocsFileCounterRepository : IRepository
    {
        long GetAndUpdateAttfOrderDocsFileCounterNextFileNoByJobSequenceAndFlgMasterFile(HttpRequest request, long jobSequence, bool flgMasterFile);
    }
}