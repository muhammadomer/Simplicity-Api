using SimplicityOnlineWebApi.BLL.Entities;
using System;

namespace SimplicityOnlineBLL.Entities
{
    public class S4BSubmissionsDataH
    {
        public long? Sequence { get; set; }
        public long? JoinSequence { get; set; }
        public bool FlgYesOrNo1 { get; set; }
        public bool FlgYesOrNo2 { get; set; }
        public DateTime? DateUser1 { get; set; }
        public DateTime? DateUser2 { get; set; }
        public double UserAmt1 { get; set; }
        public double UserAmt2 { get; set; }
        public double UserQty1 { get; set; }
        public double UserQty2 { get; set; }
        public string UserText1 { get; set; }
        public string UserText2 { get; set; }
        public string UserMemo1 { get; set; }
        public string UserMemo2 { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }        
    }
}