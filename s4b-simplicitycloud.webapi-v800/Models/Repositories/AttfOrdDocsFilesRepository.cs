using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class AttfOrdDocsFilesRepository : IAttfOrdDocsFilesRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public AttfOrdDocsFilesRepository()
        {
        }

        public bool insertAttfOrdDocsFiles(HttpRequest request, out long sequence, bool flgDeleted, long jobSequence, long fileMasterId, string fileSubmissonId,
                                           string fileDescription, string fileNotes, string filePathAndName, long createdBy,
                                           DateTime? dateCreated, long lastAmendedBy, DateTime? dateLastAmended)
        {
            bool returnValue = false;
            sequence = -1;
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
            if (settings != null)
            {
                AttfOrdDocsFilesDB attfOrdDocsFilesDB = new AttfOrdDocsFilesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                if(attfOrdDocsFilesDB.insertAttfOrdDocsFiles(out sequence, flgDeleted, jobSequence, fileMasterId, fileSubmissonId,
                                                                                    fileDescription, fileNotes, filePathAndName, createdBy, dateCreated, lastAmendedBy, dateLastAmended))
                {
                    returnValue = true;
                }
                else
                {
                    Message = attfOrdDocsFilesDB.ErrorMessage;
                }
            }
            return returnValue;
        }
    }
}
