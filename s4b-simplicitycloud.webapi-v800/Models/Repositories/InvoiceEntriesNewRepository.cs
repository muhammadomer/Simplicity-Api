using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using SimplicityOnlineWebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

using SimplicityOnlineWebApi.Commons;
using SimplicityOnlineWebApi.DAL;

namespace SimplicityOnlineWebApi.Models.Repositories
{
    public class InvoiceEntriesNewRepository : IInvoiceEntriesNewRepository
    {
        
        public string Message { get; set; }
        public bool IsSecondaryDatabase { get; set; }
        public string SecondaryDatabaseId { get; set; }
        public InvoiceEntriesNewRepository()
        {
            
        }

        public List<InvoiceEntriesNew> getByClientInvoiceNo(HttpRequest Request, string invoiceNo)
        {
            const string METHOD_NAME = "InvoiceEntriesNewRepository.getByClientInvoiceNo()";
            List<InvoiceEntriesNew> returnValue = null;
            try
            {
                string projectId = Request.Headers["ProjectId"];
                if (!string.IsNullOrWhiteSpace(projectId))
                {
                    ProjectSettings settings = Configs.settings[projectId];
                    if (settings != null)
                    {
                        InvoiceEntriesNewDB invoiceEntriesNewDB = new InvoiceEntriesNewDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                        returnValue = invoiceEntriesNewDB.selectByInvoiceNoTransTypeAndEntryType(invoiceNo, SimplicityConstants.ClientTransType, SimplicityConstants.EntryTypeSalesInvoice);
                    }
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting Invoice Entries New By Client Invoice No.", ex);
            }
            return returnValue;
        }

        public InvoiceEntriesNew GetByInvoiceNo(HttpRequest request, string invoiceNo)
        {
            const string METHOD_NAME = "InvoiceEntriesNewRepository.GetByInvoiceNo()";
            InvoiceEntriesNew returnValue = null;
            try
            {
                ProjectSettings settings = Utilities.GetProjectSettingsFromProjectId(request.Headers["ProjectId"]);
                if (settings != null)
                {
                    InvoiceEntriesNewDB invoiceEntriesNewDB = new InvoiceEntriesNewDB(Utilities.GetDatabaseInfoFromSettings(settings, this.IsSecondaryDatabase, this.SecondaryDatabaseId));
                    returnValue = invoiceEntriesNewDB.GetInvoiceEntriesNewByInvoiceNo(invoiceNo);
                }
                else
                {
                    Message = SimplicityConstants.MESSAGE_INVALID_REQUEST_HEADER;
                }
            }
            catch (Exception ex)
            {
                Message = Utilities.GenerateAndLogMessage(METHOD_NAME, "Error Occured while Getting Invoice Entries New By Invoice No.", ex);
            }
            return returnValue;
        }
    }
}
