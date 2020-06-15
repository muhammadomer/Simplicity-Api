using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class RefOrderTypeRepository : IRefOrderTypeRepository
    {        
        private ILogger<RefOrderTypeRepository> _logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }


        public RefOrderTypeRepository(ILogger<RefOrderTypeRepository> logger)
        {
            
        }

        public List<RefOrderType> GetAllOrderTypes(HttpRequest request)
        {
            List<RefOrderType> returnValue = null;

            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
            if (settings != null)
            {
                RefOrderTypeDB refOrderTypeDB = new RefOrderTypeDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                returnValue = refOrderTypeDB.selectAllRefOrderTypes();
            }
            return returnValue;
        }
    }
}
