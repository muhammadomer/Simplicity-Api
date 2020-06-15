using SimplicityOnlineWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineBLL.Entities
{
    public class WebThirdParties : ResponseModel
    {
        public int WebId { get; set;}
        public long? EntityId { get; set; }
        public int WebLevel { get; set; }
        public bool FlgDeleted { get; set; }
        public string UserName { get; set; }
        public string UserLogon { get; set; } // User Email Address
        public bool FlgReset { get; set; }
        public string WebEnable { get; set; }
        public string WebEnableNew { get; set; }
        public string WebEnableNewConfirm { get; set; }
        public string WebTelephone { get; set; }
        public string WebTelExt { get; set; }
        public string WebTelMobile { get; set; }
        public string WebCompany { get; set; }
        public string WebDepartment { get; set; }
        public string WebLocation { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}
