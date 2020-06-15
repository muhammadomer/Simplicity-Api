using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class HealthAndCheckRepository : IHealthAndCheckRepository
	{
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

		public ResponseModel GetQuestionList(HttpRequest request)
		{
            ResponseModel returnValue = new ResponseModel();
			S4bQuestionList questionList = new S4bQuestionList();
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    
                    RefS4bCheckTypesDB refDB = new RefS4bCheckTypesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
					questionList.RefS4bCheckTypes = refDB.selectAll();
					//----Get Check Payment Types
					RefS4bCheckPaymentTypesDB refPymtDB = new RefS4bCheckPaymentTypesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
					questionList.RefS4bCheckPaymentTypes = refPymtDB.selectAll();
					//-----
					returnValue.IsSucessfull = true;
					returnValue.TheObject = questionList;
				}

                else
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage("GetQuestionList", "Error Occured while Getting Question.", ex);
            }
            return returnValue;
        }

		public ResponseModel GetS4bCheckTypesByType(HttpRequest request,int checkType)
		{
			ResponseModel returnValue = new ResponseModel();
			try
			{
				ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
				if (settings != null)
				{

					RefS4bCheckTypesDB refDB = new RefS4bCheckTypesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
					returnValue.TheObject = refDB.selectAllByCheckType(checkType);
					returnValue.IsSucessfull = true;
				}

				else
					Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
			}
			catch (Exception ex)
			{
				Message = Utilities.GenerateAndLogMessage("GetQuestionList", "Error Occured while Getting Check Type.", ex);
			}
			return returnValue;
		}

		public ResponseModel GetS4bCheckPymtTypes(HttpRequest request)
		{
			ResponseModel returnValue = new ResponseModel();
			try
			{
				ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
				if (settings != null)
				{
					
					RefS4bCheckPaymentTypesDB refPymtDB = new RefS4bCheckPaymentTypesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
					returnValue.TheObject = refPymtDB.selectAll();
					//-----
					returnValue.IsSucessfull = true;
				}

				else
					Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
			}
			catch (Exception ex)
			{
				Message = Utilities.GenerateAndLogMessage("GetQuestionList", "Error Occured while Getting S4b Check Payment Types.", ex);
			}
			return returnValue;
		}

		public ResponseModel SaveS4bCheckAudit(HttpRequest request, S4bCheckAudit obj)
		{
			S4bCheckAudit result = new S4bCheckAudit();
			ResponseModel returnValue = new ResponseModel();
			try
			{
				string projectId = request.Headers["ProjectId"];
				if (!string.IsNullOrWhiteSpace(projectId))
				{
					ProjectSettings settings = Configs.settings[projectId];
					if (settings != null)
					{
						S4bCheckauditDB auditDB = new S4bCheckauditDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
						//----check Order does not has pending request otherwise it can insert another Request for invoice
						
						long sequence = -1;
						obj.CreatedBy = Convert.ToInt32(request.Headers["UserId"]);
						obj.DateCreated = DateTime.Now;
						if (auditDB.insert(out sequence, obj))
						{
							result = obj;
							result.Sequence = sequence;
							//---Insert into audit Fails
							S4bCheckauditFailsDB auditFailsDB = new S4bCheckauditFailsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
							long newSequence = -1;
							List<S4bCheckAuditFails> itemReturned = new List<S4bCheckAuditFails>();
							foreach (S4bCheckAuditFails item in obj.S4bCheckAuditFails) { 
								newSequence = -1;
								item.JoinSequence = sequence;
								item.CreatedBy = Convert.ToInt32(request.Headers["UserId"]);
								item.DateCreated = DateTime.Now;

								auditFailsDB.insert(out newSequence, item);
								item.Sequence = newSequence;
								itemReturned.Add(item);
							}
							result.S4bCheckAuditFails = itemReturned;
							returnValue.TheObject = result;
							returnValue.IsSucessfull = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Utilities.WriteLog("Error occur during saving S4b Check Audit. Error is:" + ex.Message);
				throw ex;
			}
			return returnValue;
		}


		public ResponseModel SaveS4bCheckTimesheet(HttpRequest request, S4bCheckTimeSheet obj)
		{
			S4bCheckTimeSheet result = new S4bCheckTimeSheet();
			ResponseModel returnValue = new ResponseModel();
			try
			{
				string projectId = request.Headers["ProjectId"];
				if (!string.IsNullOrWhiteSpace(projectId))
				{
					ProjectSettings settings = Configs.settings[projectId];
					if (settings != null)
					{
						S4bCheckTimesheetDB timesheetDB = new S4bCheckTimesheetDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
						//----check Order does not has pending request otherwise it can insert another Request for invoice

						long sequence = -1;
						obj.CreatedBy = Convert.ToInt32(request.Headers["UserId"]);
						obj.DateCreated = DateTime.Now;
						if (timesheetDB.insert(out sequence, obj))
						{
							result = obj;
							result.Sequence = sequence;
							
							returnValue.TheObject = result;
							returnValue.IsSucessfull = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Utilities.WriteLog("Error occur during saving S4b Check Time sheet. Error is:" + ex.Message);
				throw ex;
			}
			return returnValue;
		}

		public ResponseModel DeleteS4bCheckTimesheetBySequence(HttpRequest request, S4bCheckTimeSheet obj)
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
						S4bCheckTimesheetDB timesheetDB = new S4bCheckTimesheetDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
						//----check Order does not has pending request otherwise it can insert another Request for invoice

						
						obj.LastAmendedBy = Convert.ToInt32(request.Headers["UserId"]);
						obj.DateLastAmended = DateTime.Now;
						if (timesheetDB.deleteBySequence(obj))
						{
							returnValue.IsSucessfull = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Utilities.WriteLog("Error occur during deleting S4b Check Time sheet. Error is:" + ex.Message);
				throw ex;
			}
			return returnValue;
		}
	}
}
