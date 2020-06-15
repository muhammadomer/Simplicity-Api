using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class RefJobStatusTypeRepository : IRefJobStatusTypeRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public RefJobStatusTypeRepository()
        {
            
        }

        public List<RefJobStatusType> GetAllJobStatusTypes(HttpRequest request, HttpResponse response)
        {
            List<RefJobStatusType> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        RefJobStatusTypeDB RefJobStatusTypeDB = new RefJobStatusTypeDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = RefJobStatusTypeDB.selectAllRefJobStatusType();
                        if (returnValue == null)
                        {
                            response.Headers["message"] = "No Visit Status Found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Headers["message"] = "Exception occured while getting all Job Status Types. " + ex.Message;
            }
            return returnValue;
        }

        public int geOrdertStatusByVisitStatus(int visitStatus)//Tempoprary for Woodvale
        {
            int returnValue = -1;
            switch (visitStatus)
            {
                case 1:
                    returnValue = 1;
                    break;
                case 2:
                    returnValue = 2;
                    break;
                case 3:
                    returnValue = 5;
                    break;
                case 4:
                    returnValue = 6;
                    break;
                case 5:
                    returnValue = 7;
                    break;
                case 6:
                    returnValue = 8;
                    break;
                case 7:
                    returnValue = 14;
                    break;
                case 8:
                    returnValue = 15;
                    break;
                case 9:
                    returnValue = 16;
                    break;
                case 10:
                    returnValue = 17;
                    break;
                case 11:
                    returnValue = 18;
                    break;
                case 12:
                    returnValue = 19;
                    break;
                case 13:
                    returnValue = 3;
                    break;
            }
            return returnValue;
        }
    }
}
