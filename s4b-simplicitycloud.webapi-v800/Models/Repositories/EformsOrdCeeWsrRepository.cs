using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class EformsOrdCeeWsrRepository : IEformsOrdCeeWsrRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public EformsOrdCeeWsr Insert(EformsOrdCeeWsr Obj, HttpRequest request)
        {
            EformsOrdCeeWsr result = new EformsOrdCeeWsr();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EformsOrdCeeWsrDB eformsOrdCeeWsrDB = new EformsOrdCeeWsrDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        long sequence = -1;
                        if(eformsOrdCeeWsrDB.insertEformsOrdCeeWsr(out sequence, Obj.FormId, Obj.FormSubmissionId, Obj.FormTimeStamp, Obj.JobSequence ?? 0,
                                                                   Obj.RowNo ?? 0, Obj.RowDesc, Obj.RowRefNo, Obj.DateRowSampleDate, Obj.CreatedBy ?? 0, Obj.DateCreated,
                                                                   Obj.LastAmendedBy ?? 0, Obj.DateLastAmended))
                        {
                            Obj.Sequence = sequence;
                        }
                     }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        public List<EformsOrdCeeWsr> GetAllBySequence(HttpRequest request, HttpResponse response, long sequence)
        {
            List<EformsOrdCeeWsr> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EformsOrdCeeWsrDB EformsOrdCeeWsrDB = new EformsOrdCeeWsrDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = EformsOrdCeeWsrDB.selectAllEformsOrdCeeWsrSequence(sequence);
                        if (returnValue == null)
                        {
                            response.Headers["message"] = "No Record Found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Headers["message"] = "Exception occured while getting all Records. " + ex.Message;
            }
            return returnValue;
        }

        public EformsOrdCeeWsr Update(EformsOrdCeeWsr model, HttpRequest request)
        {
            EformsOrdCeeWsr result = new EformsOrdCeeWsr();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        EformsOrdCeeWsrDB eformsOrdCeeWsrDB = new EformsOrdCeeWsrDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        if(eformsOrdCeeWsrDB.update(model.Sequence ?? 0, model.FormId, model.FormSubmissionId, model.FormTimeStamp,
                                                          model.JobSequence ?? 0, model.RowNo ?? 0, model.RowDesc, model.RowRefNo, 
                                                          model.DateRowSampleDate, model.CreatedBy ?? 0, model.DateCreated, 
                                                          model.LastAmendedBy ?? 0, model.DateLastAmended))
                        {
                            //TODO: Log Error
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }
    }
}
