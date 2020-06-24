using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class OrdersKpiRepository : IOrdersKpiRepository
	{
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

		public ResponseModel GetOutstandingKpiOrderList(HttpRequest request, ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate)
		{
			ResponseModel returnValue = new ResponseModel();
			try
			{
				ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
				if (settings != null)
				{
					int count = 0;
					OrdersKpiDB refDB = new OrdersKpiDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
					returnValue.TheObject = refDB.selectOutstandingKpiOrderList(clientRequest,fromDate,toDate, out count,true);
					
					//-----
					returnValue.IsSucessfull = true;
				}

				else
					Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
			}
			catch (Exception ex)
			{
				Message = Utilities.GenerateAndLogMessage("GetHealthCheckAuditList", "Error Occured while Getting GetHealthCheckAuditList.", ex);
			}
			return returnValue;
		}

		public ResponseModel GetSuccessKpiOrderList(HttpRequest request, ClientRequest clientRequest, DateTime? fromDate, DateTime? toDate)
		{
			ResponseModel returnValue = new ResponseModel();
			try
			{
				ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
				if (settings != null)
				{
					int count = 0;
					OrdersKpiDB refDB = new OrdersKpiDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
					returnValue.TheObject = refDB.selectSuccessKpiOrderList(clientRequest, fromDate, toDate, out count, true);

					//-----
					returnValue.IsSucessfull = true;
				}

				else
					Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
			}
			catch (Exception ex)
			{
				Message = Utilities.GenerateAndLogMessage("GetHealthCheckAuditList", "Error Occured while Getting GetHealthCheckAuditList.", ex);
			}
			return returnValue;
		}
	}
}
