using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IRefNaturalFormsRepository : IRepository
    {  
        ResponseModel GetAllRefNaturalForms(HttpRequest request);
        ResponseModel GetRefNaturalFormsByClientId(HttpRequest request,long clientId);
    }
}
