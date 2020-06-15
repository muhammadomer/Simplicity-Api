using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.BLL.Entities;
using System;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IRefS4bFormsRepository : IRepository
    {
        RefS4bForms GetByFormId(string formId, HttpRequest request);
        List<RefS4bForms> GetFormList(HttpRequest request);
        RefS4bForms GetRecodsById(long FormSequence, HttpRequest request);
        ResponseModel updateRecord(RefS4bForms obj, HttpRequest request);
        ResponseModel AddRecord(RefS4bForms obj, HttpRequest request);
        bool SaveJsonFile(RefS4bForms form, HttpRequest request);
        bool SavePdfFile(RefS4bForms form, HttpRequest request);
        bool SaveReferenceFile(RefS4bForms form, HttpRequest request);
        bool SaveReferenceFiles(List<RefS4bForms> forms, HttpRequest request);
        bool DeleteTemplate(long FormSequence, HttpRequest request);
        List<RefS4bForms> LoadTemplateByFlgDelete(HttpRequest request);
        List<RefS4bForms> GetUserChangedForms(DateTime? lastSyncDate ,HttpRequest request);
        bool UnDeleteTemp(long formSeq, HttpRequest request);
        bool SetTempDefault(long formSeq, HttpRequest request);
        SimplicityFile MakeZipFileOfTemplate(string formId, HttpRequest request);
        List<RefS4bMapping> GetTemplateMapping(long formSeq, HttpRequest request);
        long AddTemplateMapping(RefS4bMapping mapping, HttpRequest request);
        bool UpdateTemplateMapping(RefS4bMapping mapping, HttpRequest request);
        bool DeleteTemplateMapping(long sequence, HttpRequest request);
        ResponseModel GetUserTemplatesBySyncDate(HttpRequest request, DateTime? lastSyncDate);
    }
}
