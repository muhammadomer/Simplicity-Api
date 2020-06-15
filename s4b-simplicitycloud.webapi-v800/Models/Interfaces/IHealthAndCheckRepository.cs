using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IHealthAndCheckRepository : IRepository
    {
        
        ResponseModel GetQuestionList(HttpRequest Request);
		ResponseModel GetS4bCheckTypesByType(HttpRequest request, int checkType);
		ResponseModel GetS4bCheckPymtTypes(HttpRequest request);
		ResponseModel SaveS4bCheckAudit(HttpRequest request, S4bCheckAudit s4bCheckAudit);
		ResponseModel SaveS4bCheckTimesheet(HttpRequest request, S4bCheckTimeSheet obj);
		ResponseModel DeleteS4bCheckTimesheetBySequence(HttpRequest request, S4bCheckTimeSheet obj);
	}
}
