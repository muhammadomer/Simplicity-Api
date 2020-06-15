using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IS4bFormSubmissionRepository : IRepository
    {
        List<S4bFormSubmissions> getFormSubmissionLsit(HttpRequest request);
        S4bFormSubmissions InsertFormSubmission(S4bFormSubmissions obj, HttpRequest request);
        bool UpdateFormSubmission(S4bFormSubmissions obj, HttpRequest request);
        ResponseModel S4BeFormsList(ClientRequest requestModel, HttpRequest request);
        List<S4bFormSubmissions> getFormSubmissionListByJobSequence(HttpRequest request, long jobSequence);

    }
}
