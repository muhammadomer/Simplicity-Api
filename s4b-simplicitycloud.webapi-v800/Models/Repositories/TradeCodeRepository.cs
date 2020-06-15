using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.BLL.Entities;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Commons;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class TradeCodeRepository : ITradeCodeRepository
    {        
        private ILogger<TradeCodeRepository> _logger;
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public TradeCodeRepository(ILogger<TradeCodeRepository> logger)
        {
            
        }

        public List<TradeCode> GetAllTradeCodes(HttpRequest request)
        {
            List<TradeCode> returnValue = null;

            string projectId = request.Headers["ProjectId"];
            if (!string.IsNullOrWhiteSpace(projectId))
            {
                ProjectSettings settings = Configs.settings[projectId];
                if (settings != null)
                {
                    TradeCodeDB tradeCodeDB = new TradeCodeDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = tradeCodeDB.getAllTradeCodes();
                }
            }
            return returnValue;
        }
    }
}
