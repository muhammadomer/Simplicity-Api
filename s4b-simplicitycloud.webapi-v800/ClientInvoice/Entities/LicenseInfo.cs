using System;

namespace SimplicityOnlineWebApi.ClientInvoice.Entities
{
    public class LicenseInformation
    {
        public string projectId { get; set; }
        public string companyName { get; set; }
        public long noOfAllowedUsers { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string zipCode { get; set; }
        public string address { get; set; }
        public string LicenseInfo { get; set; }
        public DateTime generateDate { get; set; }
    }
}
