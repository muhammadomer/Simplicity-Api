using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class OrdersDistributionListRepository : IOrdersDistributionListRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public OrdersDistributionListRepository()
        {
            
        }

        public List<OrdersDistributionList> GetByJobSequence(HttpRequest Request, long jobSequence)
        {
            List<OrdersDistributionList> returnVal = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersDistributionListDB ordersDistributionListDB = new OrdersDistributionListDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersDistributionListDB.getOrdersDistributionListByJobSequence(jobSequence);
                    }
                }
            }
            catch (Exception ex)
            {
                returnVal = null;
            }
            return returnVal;
        }
    }
}
