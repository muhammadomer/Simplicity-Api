using SimplicityOnlineWebApi.BLL.Entities;
using System.Collections.Generic;

namespace SimplicityOnlineBLL.Entities
{
    public class AppointmentWithTimeEntry
    {
        public ResponseModel Appointments { get; set; }
        public ResponseModel TimeEntries { get; set; }
    }
}
