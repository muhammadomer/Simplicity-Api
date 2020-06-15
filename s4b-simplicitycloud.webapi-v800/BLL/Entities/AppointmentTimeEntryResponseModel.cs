using SimplicityOnlineWebApi.BLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class AppointmentTimeEntryResponseModel
    {
        public List<DiaryAppsSmartForTimeSheet> Appointments { get; set; }
        public List<AppointmentTimeEntries> TimeEntries { get; set; }
    }
}