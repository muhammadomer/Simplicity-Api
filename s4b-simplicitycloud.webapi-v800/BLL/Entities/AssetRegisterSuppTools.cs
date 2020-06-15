namespace SimplicityOnlineBLL.Entities
{
    public class AssetRegisterSuppTools
    {
        public long? Sequence { get; set; }
        public long? JoinSequence { get; set; }
        public string AssetId { get; set; }
        public long? VehicleSequence { get; set; }
        public bool FlgMinimumAge { get; set; }
        public long? MinimumAge { get; set; }
        public bool FlgCertificationReq { get; set; }
        public long? PoweredById { get; set; }
        public long? GradingId { get; set; }
        public long? AcquiredTypeId { get; set; }
        public long? ServiceTypeId { get; set; }
        public long? StateTypeId { get; set; }
        public bool FlgOutOfService { get; set; }
        public string DateOutOfService { get; set; }
        public bool FlgDueInService { get; set; }
        public string DateDueInService { get; set; }
        public string AssetNotes { get; set; }
        public long? LastAmendedBy { get; set; }
        public string DateLastAmended { get; set; }
        public long? PowerById { get; internal set; }
    }
}