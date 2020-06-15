using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineBLL.Entities
{
    public class UserNotifications
    {
        public long? Sequence { get; set; }
        public int UserId { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string click_action { get; set; }
        public string registration_ids { get; set; }
        public string data { get; set; }
        public bool flgMarkAsRead { get; set; }
        public string sound { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }

    public class SMSNotifications
    {
        public long? Sequence { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public long? ReceiverId { get; set; }
        public string SendTo { get; set; } 
        public DateTime SentAt { get; set; }
        public bool FlgMarkAsSend { get; set; }
        public bool FlgReminderEnabled { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }

    public class NotificationsToken
    {
        public long? Sequence { get; set; }
        public int UserId { get; set; }
        public string FirebaseToken { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}
