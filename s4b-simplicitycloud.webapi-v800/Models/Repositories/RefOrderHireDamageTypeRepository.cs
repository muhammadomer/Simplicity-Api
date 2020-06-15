using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class RefOrderHireDamageTypeRepository : IRefOrderHireDamageTypeRepository
    {        
        private ILogger<RefOrderHireDamageTypeRepository> _logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public RefOrderHireDamageTypeRepository(ILogger<RefOrderHireDamageTypeRepository> logger)
        {
            
        }

        public List<RefOrderHireDamageType> GetAllDamageTypes(HttpRequest request)
        {
            List<RefOrderHireDamageType> returnValue = null;

            string projectId = request.Headers["ProjectId"];
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                ProjectSettings settings = Configs.settings[projectId];
                if (settings != null)
                {
                    RefOrderHireDamageTypeDB objDB = new RefOrderHireDamageTypeDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = objDB.getAllDamageTypes();
                }
            }
            return returnValue;
        }
    }
}
