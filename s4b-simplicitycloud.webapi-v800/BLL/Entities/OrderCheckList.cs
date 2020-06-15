using System;

namespace SimplicityOnlineBLL.Entities
{
    public class RefOrderCheckList
    {
        public long? Sequence { get; set; }
        public bool FlgDeleted { get; set; }
        public long? ListSequence { get; set; }
        public string CheckDesc { get; set; }
        public bool FlgCompulsory { get; set; }
        public bool FlgOrdEnqDataCapture { get; set; }
        public long? CreatedBy { get; set; }
        public string DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public string DateLastAmended { get; set; }
    }

    public class OrderCheckList
    {
        public long? RefSequence { get; set; }
        public long? ListSequence { get; set; }
        public long? JobSequence { get; set; }
        public string CheckDesc { get; set; }
        public bool FlgCompulsory { get; set; }
        public bool FlgOrdEnqDataCapture { get; set; }
        public long? JoinSequence { get; set; }
        public long? ItemSequence { get; set; }
        public bool FlgChecked { get; set; }
        public bool FlgCheckedYes { get; set; }
        public bool FlgCheckedNo { get; set; }
        public bool FlgCheckedDate { get; set; }
        public DateTime? CheckedDate { get; set; }
        public string CheckedDetails { get; set; }
    }

    public class OrderCheckListMain
    {
        public long? Sequence { get; set; }
        public long? JobSequence { get; set; }
        public bool FlgCancelCheckList { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }

    public class OrderCheckListItems
    {
        public long? Sequence { get; set; }
        public long? JoinSequence { get; set; }
        public long? JobSequence { get; set; }
        public long? CheckSequence { get; set; }
        public bool FlgChecked { get; set; }
        public bool FlgCheckedYes { get; set; }
        public bool FlgCheckedNo { get; set; }
        public bool FlgCheckedDate { get; set; }
        public DateTime? CheckedDate { get; set; }
        public string CheckedDetails { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public long? LastAmendedBy { get; set; }
        public DateTime? DateLastAmended { get; set; }
    }
}