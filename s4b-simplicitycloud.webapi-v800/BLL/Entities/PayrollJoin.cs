using System;

namespace SimplicityOnlineBLL.Entities
{
    public class PayrollJoin
    {
        public long? Sequence { get; set; }
        public long? PrReference { get; set; }
        public string PrFullName { get; set; }
        public long? EntityId { get; set; }
        public int UserId { get; set; }
        public long? WebViewerId { get; set; }
        public double LunchBreak { get; set; }
        public double HoursPerWeek { get; set; }
        public string HoursDesc { get; set; }
        public double PcentJobCostUplift { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}