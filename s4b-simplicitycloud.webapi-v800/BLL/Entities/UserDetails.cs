using SimplicityOnlineWebApi.ClientInvoice.Entities;
using SimplicityOnlineWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineBLL.Entities
{
    public class UserDetails : ResponseModel
    {
        public int UserId { get; set;}
        public int UserLevel { get; set; }
        public bool FlgDeleted { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserLogon { get; set; }
        public string UserEnable { get; set; }
        public string UserEnableReminder { get; set; }
        public string UserTelephone { get; set; }
        public string UserTelExt { get; set; }
        public string UserTelMobile { get; set; }
        public string UserEmailLogon { get; set; }
        public string UserEmailEnable { get; set; }
        public string UserEmailSMTP { get; set; }
        public int UserEmailSSL { get; set; }
        public int UserEmailAuthMethod { get; set; }
        public string UserEmailHTMLFile { get; set; }
        public string UserLocation { get; set; }
        public string UserDepartment { get; set; }
        public bool FlgSageOptOut { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime DateLastAmended { get; set; }        
        public long? ResourceSequence { get; set; }
        public string SecurityAnswer { get; set; }
        public string SecurityQuestion { get; set; }
        public bool Bit { get; set; }
        public string RedirectUrl { get; set; }
        public string ConfirmPassword { get; set; }
        public string OwnerName { get; set; }
        public string ForgotPasswordString { get; set; }
        public string DomainName { get; set; }
        public List<CldSettings> CldSettings { get; set; }
        public List<ApplicationSettings> AppSettings { get; set; }
        public List<RefGenericLabels> GenericLabels { get; set; }        
    }
}
