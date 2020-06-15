using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface INaturalFormsRepository : IRepository
    {
        ResponseModel GetTemplateURL(NaturalFormRequest naturalFormRequest, HttpRequest Request);

  
    }
}
