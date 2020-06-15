namespace SimplicityOnlineBLL.Entities
{
    public class PassthroughModel
    {
        public long? Sequence { get; set; }
        public string PassthroughString { get; set; }
        public string ComponentName { get; set; }
        public long? JobSequence { get; set; }
        public long? JobClientId { get; set; }
        public long? JobAddressId { get; set; }
        public string InternalId { get; set; }
        public int EntityId { get; set; }
        public int CreatedBy { get; set; }
        public bool FlagAdminMode { get; set; }
    }
}
