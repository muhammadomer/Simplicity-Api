using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IS4BFormTemplateRepository : IRepository
    {
        ResponseModel GetTemplateBySequence(long sequence, HttpRequest request);
        ResponseModel Update(ExpandoObject templateData,long joinSequence, HttpRequest request);
        ResponseModel GeneratePDF(long sequence, HttpRequest request);
        ResponseModel EmailSubmissionPdfFile(long sequence, HttpRequest request);
    }
}
