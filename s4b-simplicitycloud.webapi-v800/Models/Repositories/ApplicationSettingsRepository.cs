
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SimplicityOnlineWebApi.BLL.Entities;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.DAL;
using SimplicityOnlineWebApi.DAL.QueriesRepo;
using SimplicityOnlineWebApi.Commons;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class ApplicationSettingsRepository : IApplicationSettingsRepository
    {
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }


        public ApplicationSettingsRepository()
        {
        }

        public List<ApplicationSettings> GetApplicationSettingsAll(HttpRequest request)
        {
            List<ApplicationSettings> returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    ApplicationSettingsDB ApplicationSettingsDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = ApplicationSettingsDB.selectAll();
                }
            }
            catch (Exception ex)
            {
                //TODO: write exception code
            }
            return returnValue;
        }

        public ApplicationSettings GetApplicationSettingsBySettingId(HttpRequest request, string settingId)
        {
            ApplicationSettings returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    ApplicationSettingsDB ApplicationSettingsDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = ApplicationSettingsDB.selectBySettingId(settingId);
                }
            }
            catch (Exception ex)
            {
                //TODO: Write exception code

            }
            return returnValue;
        }

        public ApplicationSettings GetOrdersBillsLastInvoiceNo(HttpRequest request)
        {
            ApplicationSettings returnValue = null;
            try
            {
                returnValue = GetApplicationSettingsBySettingId(request, SimplicityConstants.ApplicationSettingsLastInvoiceNo);
            }
            catch (Exception ex)
            {
                //TODO: Write exception code
            }
            return returnValue;
        }

        public bool UpdateOrdersBillsLastInvoiceNo(HttpRequest request, string invoiceNo)
        {
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    ApplicationSettingsDB ApplicationSettingsDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = ApplicationSettingsDB.updateSetting1BySettingId(SimplicityConstants.ApplicationSettingsLastInvoiceNo, invoiceNo);
                }
            }
            catch (Exception ex)
            {
                //TODO: Write exception code
            }
            return returnValue;
        }

        public ApplicationSettings GetOrdersBillsInvoiceNoPrefix(HttpRequest request)
        {
            ApplicationSettings returnValue = null;
            try
            {
                returnValue = GetApplicationSettingsBySettingId(request, SimplicityConstants.ApplicationSettingsInvoiceNoPrefix);
            }
            catch (Exception ex)
            {
                //TODO: Write exception code
            }
            return returnValue;
        }

        public bool UpdateOrdersBillsLastPONo(HttpRequest request, string poNo)
        {
            const string METHOD_NAME = "ApplicationSettingsRepository.UpdateOrdersBillsLastPONo()";
            bool returnValue = false;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    ApplicationSettingsDB ApplicationSettingsDB = new ApplicationSettingsDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = ApplicationSettingsDB.updateSetting1BySettingId(SimplicityConstants.ApplicationSettingsLastPONo, poNo);
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Updating Order Bills Last PO No.", ex);
            }
            return returnValue;
        }
    }
}
