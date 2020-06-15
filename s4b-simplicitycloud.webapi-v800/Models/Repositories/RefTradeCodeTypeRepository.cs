using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimplicityOnlineWebApi.BLL.Entities;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Commons;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class RefTradeCodeTypeRepository : IRefTradeCodeTypeRepository
    {        
        private ILogger<RefTradeCodeTypeRepository> _logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public RefTradeCodeTypeRepository(ILogger<RefTradeCodeTypeRepository> logger)
        {
            
        }

        public List<RefTradeCodeType> GetAllTradeCodes(HttpRequest request)
        {
            List<RefTradeCodeType> returnValue = null;

            string projectId = request.Headers["ProjectId"];
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                ProjectSettings settings = Configs.settings[projectId];
                if (settings != null)
                {
                    RefTradeCodeTypeDB tradeCodeDB = new RefTradeCodeTypeDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = tradeCodeDB.getAllRefTradeCodeType();
                }
            }
            return returnValue;
        }
    }
}
