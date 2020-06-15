using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class DiaryAppsSmartForTimeSheet
    {
        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public DateTime? DateAppStart { get; set; }
        public DateTime? DateAppEnd { get; set; }
        public bool IsTimeSheetDone { get; set; }
        public string AppNotes { get; set; }
        public long? VisitStatus { get; set; }
        public OrdersSmartForTimeSheet Order { get; set; }
        public List<long> S4BFormsIds { get; set; }
        public DiaryAppsGPS DAGPS { get; set; }
    }

    public class OrdersSmartForTimeSheet
    {
        public string JobRef { get; set; }
        public string JobClientName { get; set; }
        public string JobAddress { get; set; }
        public string OccupierName { get; set; }
        public string OccupierTelHome { get; set; }
        public string OccupierTelMobile { get; set; }
        public string OccupierEmail { get; set; }
        public string JobClientRef { get; set; }
        public string JobDesc { get; set; }
        public DateTime? JobDateDue { get; set; }
    }
}
