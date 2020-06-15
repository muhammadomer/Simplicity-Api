using System;

namespace SimplicityOnlineBLL.Entities
{
    public class WebViewer : ResponseModel
    {
        public int ViewerId { get; set;}
        public int ViewerLevel { get; set; }
        public bool FlgDeleted { get; set; }
        public string ViewerName { get; set; }
        public string ViewerLogon { get; set; } // User Email Address
        public bool FlgReset { get; set; }
        public string ViewerEnable { get; set; }
        public string ViewerEnableNew { get; set; }
        public string ViewerEnableNewConfirm { get; set; }
        public string ViewerTelephone { get; set; }
        public string ViewerTelExt { get; set; }
        public string ViewerTelMobile { get; set; }
        public string ViewerCompany { get; set; }
        public string ViewerDepartment { get; set; }
        public string ViewerLocation { get; set; }
        public bool FlgTimesheets { get; set; }
        public WebViewerAssignedTo AssignedTo { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }

    public class WebViewerAssignedTo
    {
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public long? EntityId { get; set; } //Mostly the Client Id
        public long? DefaultJobAddress { get; set; }
        public int DefaultJobStatus { get; set; }
        public long? DefaultJobManager { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public int ViewerId { get; internal set; }
    }
}
