
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class ApplicationWebPagesRepository : IApplicationWebPagesRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }

        public ApplicationWebPagesRepository()
        {
            //
        }

        public List<ApplicationWebPages> GetAllApplicationWebPages(HttpRequest request, HttpResponse response)
        {
            List<ApplicationWebPages> returnValue = null;
            try
            {
                string projectId = request.Headers["ProjectId"];
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]); if (settings != null)
                {
                    ApplicationWebPagesDB webPagesDB = new ApplicationWebPagesDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = webPagesDB.GetAllApplicationWebPages();
                    if (returnValue == null)
                    {
                        response.Headers["message"] = "No Application Web Page Found.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Headers["message"] = "Exception occured while getting all Application Web Pages. " + ex.Message;
            }
            return returnValue;
        }
    }    
}
