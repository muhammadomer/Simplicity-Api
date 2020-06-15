using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IDiaryAppsNaturalFormsRepository : IRepository
    {
        ResponseModel GetAllNaturalFormsByFormSequence(HttpRequest request, long deSequence, long formSequence);
        ResponseModel GetAllNaturalFormsByDESequence(HttpRequest request, long deSequence);
        ResponseModel GetUnassignedNaturalFormsOfDESequence(HttpRequest request, long deSequence);
        ResponseModel InsertNaturalForm(DiaryAppNaturalForm[] diaryAppNaturalForms,long deSequence,HttpRequest request);
        ResponseModel InsertPasteDiaryAppsNaturalForms(DiaryAppNaturalForm DiaryAppNaturalForm, HttpRequest request);
        ResponseModel InsertTFRFromUnscheduled(long deSequence, long deSequenceUnscheduled, HttpRequest request);
        ResponseModel DeleteNaturalFormsByDESequence(long deSequenc, HttpRequest request);
        ResponseModel DeleteNaturalFormsBySequence(long Sequence, HttpRequest request);
    }
}
