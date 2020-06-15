
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class OrdersMeSchItemsRepository : IOrdersMeSchItemsRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        

        public OrdersMeSchItemsRepository()
        {
            
        }

       
      public OrdersMeSchItems Update(OrdersMeSchItems Oi, HttpRequest request)
        {
			OrdersMeSchItems Obj = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
						OrdersMeSchItemsDB OrderItemsDB = new OrdersMeSchItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        Obj = OrderItemsDB.UpdateOrdersMeSchItems(Oi);
                    }
                }
            }
            catch (Exception ex)
            {
                Obj = null;
            }
            return Obj;
        }
        public OrdersMeSchItems Insert(OrdersMeSchItems Oi, HttpRequest request)
        {
			OrdersMeSchItems Obj = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
					OrdersMeSchItemsDB OrderItemsDB = new OrdersMeSchItemsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    Obj = OrderItemsDB.InsertOrdersMeSchItems(Oi);
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message + " " + ex.InnerException;
                Obj = null;
            }
            return Obj;
        }
    }
}
