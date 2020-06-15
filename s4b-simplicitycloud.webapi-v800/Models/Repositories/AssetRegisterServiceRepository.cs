using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class AssetRegisterServiceRepository : IAssetRegisterServiceRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public AssetRegisterService insert(HttpRequest request, AssetRegisterService obj)
        {
            AssetRegisterService returnValue = null;
            long sequence = -1;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    AssetRegisterServiceDB AssetRegisterServiceDB = new AssetRegisterServiceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (AssetRegisterServiceDB.insertAssetRegisterService(out sequence, obj.FlgDeleted, obj.FlgNotActive, obj.AssetSequence ?? 0,
                                                                            obj.JobSequence ?? 0, obj.DaSequence ?? 0, obj.DaAppType ?? 0,
                                                                            obj.DateDaStart, obj.DateService, obj.ServiceInitials, obj.ServiceNotes,
                                                                            obj.ConditionSequence ?? 0, obj.ServiceBy ?? 0, obj.FlgNewJobCreated,
                                                                            obj.FlgNewApp, obj.FlgValidated, obj.ValidatedBy ?? 0, obj.DateValidated,
                                                                            obj.CreatedBy ?? 0, obj.DateCreated, obj.LastAmendedBy ?? 0,
                                                                            obj.DateLastAmended))
                    {
                        obj.Sequence = sequence;
                        returnValue = obj;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }

        public AssetRegisterService Update(HttpRequest request, AssetRegisterService obj)
        {
            AssetRegisterService returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    AssetRegisterServiceDB AssetDB = new AssetRegisterServiceDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    if (AssetDB.updateBySequence(obj.Sequence ?? 0, obj.FlgDeleted, obj.FlgNotActive, obj.AssetSequence ?? 0,
                                                    obj.JobSequence ?? 0, obj.DaSequence ?? 0, obj.DaAppType ?? 0,
                                                    obj.DateDaStart, obj.DateService, obj.ServiceInitials, obj.ServiceNotes,
                                                    obj.ConditionSequence ?? 0, obj.ServiceBy ?? 0, obj.FlgNewJobCreated,
                                                    obj.FlgNewApp, obj.FlgValidated, obj.ValidatedBy ?? 0, obj.DateValidated,
                                                    obj.CreatedBy ?? 0, obj.DateCreated, obj.LastAmendedBy ?? 0,
                                                    obj.DateLastAmended))
                    {
                        returnValue = obj;
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }
    }
}
