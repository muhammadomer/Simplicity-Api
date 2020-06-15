using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;
using Microsoft.Extensions.Logging;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class PercentageRateRespository: IPercentageRateRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        private ILogger<OrderItemsRepository> _logger;

        public ResponseModel GetAdjustmentCodes(HttpRequest request)
        {
            ResponseModel returnValue = new ResponseModel();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        int count = 0;
                        PercentageRatesDB percentageRateDB = new PercentageRatesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = percentageRateDB.selectPercentageRateOfType("A", out count);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = percentageRateDB.ErrorMessage;
                        }
                        else
                        {
                            returnValue.IsSucessfull = true;
                        }
                    }
                    else
                    {
                        returnValue.Message = SimplicityConstants.MESSAGE_INVALID_PROJECT_ID;
                    }
                }
                else
                {
                    returnValue.Message = SimplicityConstants.MESSAGE_INVALID_REQUEST;
                }
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While Getting adjustment codes. " + ex.Message + " " + ex.InnerException;
                _logger.LogError(ex.Message, ex);
            }

            return returnValue;
        }
    }
}
