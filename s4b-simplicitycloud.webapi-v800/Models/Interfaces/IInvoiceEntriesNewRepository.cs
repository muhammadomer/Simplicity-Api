using Microsoft.AspNetCore.Http;
using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.Models.Interfaces
{
    public interface IInvoiceEntriesNewRepository : IRepository
    {
        List<InvoiceEntriesNew> getByClientInvoiceNo(HttpRequest Request, string invoiceNo);
    }
}
