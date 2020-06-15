using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{


    public class RefDiaryAppRatesRepository : IRefDiaryAppRatesRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public RefDiaryAppRatesRepository()
        {
            
        }

        public RefDiaryAppRates AddDiaryAppRates(RefDiaryAppRates Obj, HttpRequest request)
        {
            RefDiaryAppRates result = new RefDiaryAppRates();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefDiaryAppRatesDB VisitStatus = new RefDiaryAppRatesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = VisitStatus.insertRefDiaryAppRates(Obj);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }     

        public List<RefDiaryAppRates> GetDiaryAppRatesById(long TypeCode, HttpRequest Request, HttpResponse Response)
        {
            List<RefDiaryAppRates> returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefDiaryAppRatesDB DiaryAppRates = new RefDiaryAppRatesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = DiaryAppRates.selectAllRefDiaryAppRatesrateSequence(TypeCode);
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }


        public List<RefDiaryAppRates> GetDiaryAppRates(HttpRequest Request, HttpResponse Response)
        {
            List<RefDiaryAppRates> returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefDiaryAppRatesDB DiaryAppRates = new RefDiaryAppRatesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = DiaryAppRates.selectAllRefDiaryAppRates();
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue = null;
            }
            return returnValue;
        }

        public RefDiaryAppRates UpdateDiaryAppRates(RefDiaryAppRates model, HttpRequest request)
        {
            RefDiaryAppRates result = new RefDiaryAppRates();
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefDiaryAppRatesDB DiaryAppRates = new RefDiaryAppRatesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        result = DiaryAppRates.updateByrateSequence(model);
                    }
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }
    }
}
