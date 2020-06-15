using SimplicityOnlineBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplicityOnlineWebApi.BLL.Entities
{
    public class DiaryAppsWebAssign
    {
        public long? SequenceId { get; set; }
        public long? DeSequence { get; set; }
        public long? ResourceSequence { get; set; }
        public long? EntityId { get; set; }
        public int WebId { get; set; }
        public DateTime? DateAppStart { get; set; }
        public string AddInfo { get; set; }
        public bool FlgComplete { get; set; }
        public DateTime? DateAppCompleted { get; set; }
        public bool FlgDelay { get; set; }
        public string DelayReason { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
        public DiaryApps DiaryApp{ get; set; }
        public WebThirdParties WebThirdParty { get; set; }
        public List<NaturalForm> Forms { get; set; }
        public ActionType ActionType { get; set; }
    }

    public enum ActionType
    {
        AssignTo = 1,
        Notes = 2,
        Delay = 3,
        AptCompleted = 4
    }
}
