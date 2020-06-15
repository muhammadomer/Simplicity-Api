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
    public class RefProductUnitRespository: IRefProductUnitRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        private ILogger<OrderItemsRepository> _logger;

        public ResponseModel GetProductUnits(RequestHeaderModel header)
        {
            ResponseModel returnValue = new ResponseModel();
            ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(header.ProjectId);
            if (settings == null)
            {
                returnValue.Message = SimplicityConstants.MESSAGE_INVALID_PROJECT_ID; returnValue.IsSucessfull = false;
                Utilities.WriteLog(returnValue.Message);
                return returnValue;
            }
            try
            {
                int count = 0;
                RefProductUnitDB productUnitDB = new RefProductUnitDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                returnValue.TheObject = productUnitDB.selectProductUnit(out count);
                returnValue.Count = count;
                if (returnValue.TheObject == null)
                    returnValue.Message = productUnitDB.ErrorMessage;
                else
                    returnValue.IsSucessfull = true;
            }
            catch (Exception ex)
            {
                returnValue.Message = "Exception Occured While Getting Item Type. " + ex.Message + " " + ex.InnerException;
                _logger.LogError(ex.Message, ex);
            }

            return returnValue;
        }
    }
}
