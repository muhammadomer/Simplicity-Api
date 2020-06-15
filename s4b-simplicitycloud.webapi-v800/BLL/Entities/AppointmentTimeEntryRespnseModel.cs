using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class AppointmentTimeEntryRespnseModel
    {
        public ResponseModel Appointments { get; set; }
        public List<AppointmentTimeEntries> TimeEntries { get; set; }
    }
}
