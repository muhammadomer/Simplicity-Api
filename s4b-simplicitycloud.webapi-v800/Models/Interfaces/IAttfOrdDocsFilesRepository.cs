using Microsoft.AspNetCore.Http;
using System;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IAttfOrdDocsFilesRepository : IRepository
    {
        bool insertAttfOrdDocsFiles(HttpRequest request, out long sequence, bool flgDeleted, long jobSequence, long fileMasterId, string fileSubmissonId,
                                           string fileDescription, string fileNotes, string filePathAndName, long createdBy,
                                           DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended);
    }
}