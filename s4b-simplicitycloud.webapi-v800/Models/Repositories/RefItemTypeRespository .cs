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
    public class RefItemTypesRespository: IRefItemTypeRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        private ILogger<OrderItemsRepository> _logger;

        public ResponseModel GetItemType(HttpRequest request, bool isAllItems)
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
                        RefItemTypeDB refItemTypeDB = new RefItemTypeDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue.TheObject = refItemTypeDB.selectItemType(isAllItems, out count);
                        returnValue.Count = count;
                        if (returnValue.TheObject == null)
                        {
                            returnValue.Message = refItemTypeDB.ErrorMessage;
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
                returnValue.Message = "Exception Occured While Getting Item Type. " + ex.Message + " " + ex.InnerException;
                _logger.LogError(ex.Message, ex);
            }

            return returnValue;
        }
    }
}
