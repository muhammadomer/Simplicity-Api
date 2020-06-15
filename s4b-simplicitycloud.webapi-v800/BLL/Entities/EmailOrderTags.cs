using SimplicityOnlineBLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class EmailOrderTags
    {
        public EmailContact From { get; set; }
        public List<EmailContact> To { get; set; }
        public List<EmailContact> Cc { get; set; }
        public List<EmailContact> Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<EmailAttachments> Attachments { get; set; }
        public List<Orders> Orders  { get; set; }
    }
}
