using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class DiaryAppNaturalForm
    {
        public long? Sequence { get; set; }
        public long? DiaryAppSequence { get; set; }
        public long? FormSequence { get; set; }
        public long? DeSequence { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public DiaryApps DiaryApp{ get; set; }
        public RefNaturalForm RefNaturalForm { get; set; }
    }

    public enum DiaryAppNaturalFormOperation
    {
        ememSqlTableDANF_Delete,
        ememSqlTableDANF_Insert,
        ememSqlTableDANF_InsertPasteDiaryEntry,
        ememSqlTableDANF_InsertTFR_FromUnscheduled,
        ememSqlTableDANF_Select
    }

    public enum DiaryAppsUnschedNFOperation
    {
        ememSqlTableDAUNF_Delete,
        ememSqlTableDAUNF_Insert,
        ememSqlTableDAUNF_InsertTFR_FromDE,
        ememSqlTableDAUNF_Select
    }
}
