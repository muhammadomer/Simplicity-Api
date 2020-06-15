
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
    public class OrdersMeSchHeaderRepository : IOrdersMeSchHeaderRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        

        public OrdersMeSchHeaderRepository()
        {
            
        }

       
      public OrdersMeSchHeader Update(OrdersMeSchHeader Oi, HttpRequest request)
        {
			OrdersMeSchHeader Obj = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
						OrdersMeSchHeaderDB OrderHeadersDB = new OrdersMeSchHeaderDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        Obj = OrderHeadersDB.UpdateOrdersMeSchHeader(Oi);
                    }
                }
            }
            catch (Exception ex)
            {
                Obj = null;
            }
            return Obj;
        }
        public OrdersMeSchHeader Insert(OrdersMeSchHeader Oi, HttpRequest request)
        {
			OrdersMeSchHeader Obj = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
					OrdersMeSchHeaderDB OrderHeadersDB = new OrdersMeSchHeaderDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    Obj = OrderHeadersDB.InsertOrdersMeSchHeader(Oi);
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message + " " + ex.InnerException;
                Obj = null;
				throw ex;
            }
            return Obj;
        }
    }
}
