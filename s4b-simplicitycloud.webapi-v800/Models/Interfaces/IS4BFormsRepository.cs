using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IS4BFormsRepository : IRepository
    {
        ResponseModel GetTemplateURL(NaturalFormRequest naturalFormRequest, HttpRequest Request);
        ResponseModel ProcessSubmittedForm(HttpRequest request);
        ResponseModel ProcessSubmittedFormVideoFile(HttpRequest request);
        S4BFormPrepopulationDataModel GetPrepopulationData(S4BFormPrepopulationDataRequest s4BFormPrepopulationDataRequest, HttpRequest request);
        ResponseModel GetFilesAndUpdate(HttpRequest request, string filePath);
        ResponseModel GetAppointmentNotesSetting(HttpRequest request);
    }
}
