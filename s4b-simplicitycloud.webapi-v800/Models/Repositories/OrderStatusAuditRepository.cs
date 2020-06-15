using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;

using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class OrderStatusAuditRepository : IOrderStatusAuditRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public OrderStatusAuditRepository()
        {
            
        }

        public bool InsertOrderStatusAudit(HttpRequest request, OrderStatusAudit orderStatusAudit)
        {
            bool returnValue = false;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrderStatusAuditDB OrderStatusAuditDB = new OrderStatusAuditDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        long sequence = -1;
                        returnValue = OrderStatusAuditDB.insertOrderStatusAudit(out sequence, orderStatusAudit.JobSequence ?? 0, 
                                                                                orderStatusAudit.StatusType ?? 0, orderStatusAudit.FlgJobClientId,
                                                                                orderStatusAudit.JobClientId ?? 0, orderStatusAudit.FlgStatusRef,
                                                                                orderStatusAudit.StatusRef, orderStatusAudit.DateStatusRef,
                                                                                orderStatusAudit.StatusRef2, orderStatusAudit.StatusDesc, 1, DateTime.Now,
                                                                                1, DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
                //Log Error
            }
            return returnValue;
        }
    }
}
