using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface ISiteInspectionRepository : IRepository
    {
        List<string> FindMatchingContractNos(string contractNo, HttpRequest request);
        SubmissionsDataFh GetBySequence(long sequence, HttpRequest request);
        bool Update(SubmissionsDataFh siteInspection, HttpRequest request);

        bool Insert(SubmissionsDataFh siteInspection, HttpRequest request,HttpResponse response);
        bool Insert(S4BSubmissionsData2 submissionData, HttpRequest request, HttpResponse response);
        List<SubmissionsDataFh> GetSubmissionsDataFhList(HttpRequest request);

        bool GeneratePDF(long sequence, HttpRequest request);
        SubmissionsImagesFh UploadImageWithSequence(SubmissionsImagesFh fileDetail, HttpRequest request, HttpResponse response);
    }
}
