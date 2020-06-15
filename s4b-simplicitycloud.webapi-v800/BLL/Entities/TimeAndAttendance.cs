using System;

namespace SimplicityOnlineBLL.Entities
{
    public class PRSnap365Budget
    {
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public int TeamId { get; set; }
        public int LocationId { get; set; }
        public int Year { get; set; }
        public double Value { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }

    public class PRSnap365Revenue
    {
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public int TeamId { get; set; }
        public int LocationId { get; set; }
        public DateTime? EntryDate { get; set; }
        public double EntryValue { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }

    public class PRRosterInOutSummary
    {
        public long? Sequence { get; set; }
        public int deviceId { get; set; }
        public long? PrReference { get; set; }
        public DateTime? DateWorked { get; set; }
        public bool FlgHoursIssueIn { get; set; }
        public bool FlgHoursIssueOut { get; set; }
        public double HoursWorked { get; set; }
        public bool FlgBreakIssueBreak { get; set; }
        public bool FlgBreakIssueReturn { get; set; }
        public double HoursBreak { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }

    public class PRRosterInOutSummaryDD
    {
        public long? Sequence { get; set; }
        public long? JoinSequence { get; set; }
        public long? PRPayElement { get; set; }
        public long? PrReference { get; set; }
        public double ElementHours { get; set; }
        public double ElementRate { get; set; }
        public double ElementAmount { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}