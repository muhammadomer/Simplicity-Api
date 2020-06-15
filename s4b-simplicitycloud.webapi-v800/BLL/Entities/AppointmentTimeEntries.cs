using SimplicityOnlineWebApi.BLL.Entities;
using System;

namespace SimplicityOnlineBLL.Entities
{
    public class AppointmentTimeEntries
    {
        public long? Sequence { get; set; }

        public DateTime? DateRowStartTime { get; set; }
        public DateTime? DateRowFinishTime { get; set; }

        public string RowPymtType { get; set; }
        public string RowNotes { get; set; }
        public string RowJobRef { get; set; }

        public bool FlgJobRefValid { get; set; }
        public long? JobSequence { get; set; }

        public string StartTimeLocation { get; set; }
        public string FinishTimeLocation { get; set; }

        public double DeSequence { get; set; }

        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }

        public DiaryAppsSmartForTimeSheet Appointment { get; set; }
    }
}