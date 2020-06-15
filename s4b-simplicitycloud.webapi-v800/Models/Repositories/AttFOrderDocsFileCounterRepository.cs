using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class AttFOrderDocsFileCounterRepository : IAttFOrderDocsFileCounterRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public AttFOrderDocsFileCounterRepository()
        {
        }

        public long GetAndUpdateAttfOrderDocsFileCounterNextFileNoByJobSequenceAndFlgMasterFile(HttpRequest request, long jobSequence, bool flgMasterFile)
        {
            long returnValue = -1;
            string projectId = request.Headers["ProjectId"];
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                ProjectSettings settings = Configs.settings[projectId];
                if (settings != null)
                {
                    AttfOrdDocsFileCounterDB attfOrdDocsFileCounterDB = new AttfOrdDocsFileCounterDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    AttfOrdDocsFileCounter obj = attfOrdDocsFileCounterDB.selectAllAttfOrdDocsFileCounterByJobSequenceAndFlgMasterFile(jobSequence, flgMasterFile);
                    if (obj == null)
                    {
                        returnValue = 1;
                        long sequence = -1;
                        if(!attfOrdDocsFileCounterDB.insertAttfOrdDocsFileCounter(out sequence, jobSequence, flgMasterFile, returnValue))
                        {
                            Message = attfOrdDocsFileCounterDB.ErrorMessage;
                        }
                    }
                    else
                    { 
                        returnValue = (obj.LastFileNo ?? 0 )+ 1;
                        if (!attfOrdDocsFileCounterDB.updateBySequence(obj.Sequence ?? 0, obj.JobSequence ?? 0, obj.FlgMasterFile, returnValue))
                        {
                            Message = attfOrdDocsFileCounterDB.ErrorMessage;
                        }
                    }
                }
            }
            return returnValue;
        }
    }
}
