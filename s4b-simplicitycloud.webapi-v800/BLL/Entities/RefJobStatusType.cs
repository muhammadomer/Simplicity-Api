namespace SimplicityOnlineBLL.Entities
{
    public class RefJobStatusType
    {
        public int StatusId { get; set; }
        public int StatusIndex { get; set; }
        public string StatusDesc { get; set; }
        public bool FlgAutoUpdate { get; set; }
        public long? OrderFieldId { get; set; }
        public bool FlgLinkToDiaryApps { get; set; }
        public bool FlgCompletedStatus { get; set; }
    }
}
