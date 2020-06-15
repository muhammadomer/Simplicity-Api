using Microsoft.Extensions.Logging;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;


using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class PassthroughRepository : IPassthroughRepository
    {
        
        private ILogger<PassthroughRepository> LOGGER;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public PassthroughRepository(ILogger<PassthroughRepository> _LOGGER)
        {
            
            LOGGER = _LOGGER;
        }

        public PassthroughModel GetPassthroughModelBySequence(long sequence, string projectId)
        {
            PassthroughModel passthroughModel = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        PassthroughDB passthroughDB = new PassthroughDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        passthroughModel = passthroughDB.getPassthroughBySequence(sequence);
                    }
                }
            }
            catch (Exception ex)
            {
                LOGGER.LogError(ex.Message);
            }
            return passthroughModel;
        }

        public bool DeletePassthroughBySequence(long sequence, string projectId)
        {
            bool returnValue = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        PassthroughDB passthroughDB = new PassthroughDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = passthroughDB.deletePassthroughBySequence(sequence);
                    }
                }
            }
            catch (Exception ex)
            {
                LOGGER.LogError(ex.Message);
            }
            return returnValue;
        }

        public PassthroughModel GetPassthroughModelByPassthroughString(string passthroughString, string projectId)
        {
            PassthroughModel passthroughModel = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        PassthroughDB passthroughDB = new PassthroughDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));

                        passthroughModel = passthroughDB.getPassthroughByPassthroughString(passthroughString);
                    }
                }
            }
            catch (Exception ex)
            {
                LOGGER.LogError(ex.Message);
            }
            return passthroughModel;
        }

        public PassthroughModel Create(PassthroughModel passthroughModel, HttpRequest request, HttpResponse Response)
        {
            PassthroughModel returnValue = null;
            try
            {


                ProjectSettings settings = Configs.settings[request.Headers["ProjectId"]];
                if (settings != null)
                {
                    if (passthroughModel != null)
                    {
                        PassthroughDB passthroughDB = new PassthroughDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        PassthroughModel newPassthroughModel = null;

                        long sequence = -1;
                        if (passthroughDB.insert(out sequence, passthroughModel.PassthroughString, passthroughModel.JobSequence ?? 0, passthroughModel.JobClientId ?? 0, passthroughModel.JobAddressId ?? 0, passthroughModel.EntityId, passthroughModel.FlagAdminMode, passthroughModel.ComponentName, Convert.ToInt32(request.Headers["UserId"]), DateTime.Now))
                        {
                            newPassthroughModel = passthroughDB.getPassthroughBySequence(sequence);
                        }

                        returnValue = newPassthroughModel;
                    }
                }
            }
            catch (Exception ex)
            {
                LOGGER.LogError(ex.Message);
            }

            return returnValue;
        }
    }
}
