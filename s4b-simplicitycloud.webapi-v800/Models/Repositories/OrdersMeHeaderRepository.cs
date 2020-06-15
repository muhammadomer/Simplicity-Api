using Microsoft.AspNetCore.Http;

using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class OrdersMeHeaderRepository : IOrdersMeHeaderRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
		
		public OrdersMeHeaderRepository()
		{
			
		}
		public OrdersMeHeader GetOrdersMeHeaderBySequence(long sequence, HttpRequest request)
        {
            OrdersMeHeader returnVal = new OrdersMeHeader();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        OrdersMeHeaderDB ordersMeHeaderDB = new OrdersMeHeaderDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnVal = ordersMeHeaderDB.selectAllOrdersMeHeaderSequence(sequence, false);
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Headers["message"] = "Exception occured while getting Users. " + ex.Message;
            }
            return returnVal;
        }

		public OrdersMeHeader GetOrdersMeHeaderByJobSequence(long jobSequence, HttpRequest request)
		{
			OrdersMeHeader returnVal = new OrdersMeHeader();
			try
			{
				string projectId = request.Headers["ProjectId"];
				if (!string.IsNullOrWhiteSpace(projectId))
				{
					ProjectSettings settings = Configs.settings[projectId];
					if (settings != null)
					{
						OrdersMeHeaderDB ordersMeHeaderDB = new OrdersMeHeaderDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
						returnVal = ordersMeHeaderDB.selectAllOrdersMeHeaderByJobSequence(jobSequence);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return returnVal;
		}
	}
}
