using System;

namespace SimplicityOnlineWebApi.ClientInvoice.Entities
{
    public class Parameters
    {
        public int entityId { get; set; }
        public string job { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string type { get; set; }
        public string status { get; set; }
    }
}
